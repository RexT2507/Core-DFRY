using ApiLibrary.Core.Attributes;
using ApiLibrary.Core.Base;
using ApiLibrary.Core.HttpContent;
using ApiLibrary.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ApiLibrary.Core.Utils;
using System.Collections.Specialized;

namespace ApiLibrary.Core.Controllers
{
    /**
     * C : Contexte de la BDD
     * T : Type de donnée du Controller
     * K : type de la clé primaire
     */
    // --- Mettre le mot-clé "virtual" si on veut faire du override dans la classe enfant --- //
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [MaxPagination(25)]
    public abstract class ControllerRef<C, T, K> : ControllerBase where T : BaseModel<K> where C : BaseDbContext
    {

        protected readonly C _db;

        protected PartialContent Partial(object o)
        {
            return new PartialContent(o);
        }

        public ControllerRef(C db)
        {
            _db = db;
        }

        // Methods générique
        private async Task<ActionResult<IEnumerable<T>>> AddFilter(IQueryable<T> query, string range, string sort, string fields)
        {
            int pagination;

            if (range != null)
            {
                try { pagination = this.GetType().GetCustomAttribute<MaxPaginationAttribute>().Range; } catch (NullReferenceException) { pagination = typeof(ControllerRef<C, T, K>).GetCustomAttribute<MaxPaginationAttribute>().Range; }

                var myRange = range.Split("-");

                query = query.Where(x => x.deletedAt != null);

                int start = Convert.ToInt32(myRange[0]);
                int end = Convert.ToInt32(myRange[1]);
                int count = end - start;

                if (count > pagination)
                {
                    return Unauthorized($"Pagination non valide. Valeur max : {pagination}.");
                }

                int total = query.Count();
                query = query.Range(Convert.ToInt32(myRange[0]), Convert.ToInt32(myRange[1]) + 1);

                string url = $"{Request.Scheme}://{Request.Host}{Request.Path}";

                string links = $"<{url}?range=0-{count}>; rel=\"first\", " +
                    $"<{url}?range={(start - count < 0 ? 0 : start - count)}-{(start - count < 0 ? 0 : start - count) + count}>; rel=\"prev\", " +
                    $"<{url}?range={end + 1}-{(end + count > total ? total : end + count)}>; rel=\"next\", " +
                    $"<{url}?range={total - count + 1}-{total}>; rel=\"last\"";

                Response.Headers.Add("Accept-Range", pagination.ToString());
                Response.Headers.Add("Content-Range", $"{start}-{end}/{total}");
                Response.Headers.Add("Link", links);
            }

            if (sort != null)
            {
                try
                {
                    var tab = sort.Split(',');
                    string sortProp = typeof(T).GetProperty(tab[0], BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance).Name;
                    IOrderedQueryable<T> oq = query.OrderBy(sortProp);
                    foreach (var item in tab.Skip(1))
                    {
                        string thenProp = typeof(T).GetProperty(item, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance).Name;
                        oq = oq.ThenBy(thenProp);
                    }

                    query = oq;
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }

            var queryRequest = this.Request.Query;
            // on récupère toutes les proprietés publique de l'objet afin de pouvoir les comparé aux params du header
            foreach (string paramName in queryRequest.Keys)
            {
                if (paramName != "range" && paramName != "sort" && paramName != "fields")
                {
                    try
                    {
                        PropertyInfo Tproperties = typeof(T).GetProperty(paramName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                        if (Tproperties == null)
                        {
                            throw new Exception();
                        }
                        var paramValue = queryRequest[Tproperties.Name].ToString();
                        if (paramValue.StartsWith("[") && paramValue.EndsWith("]"))
                        {
                            Regex pattern = new Regex(@"\[([0-9-.]*),([0-9-.]*)\]");
                            MatchCollection matches = pattern.Matches(paramValue);

                            if (matches.Count() == 1)
                            {
                                string borneStart = matches[0].Groups[1].Value;
                                string borneEnd = matches[0].Groups[2].Value;
                                if (borneStart != "" && borneEnd != "" && (!TreatParams.CanConvert(borneStart, Tproperties.PropertyType) || !TreatParams.CanConvert(borneEnd, Tproperties.PropertyType)))
                                {
                                    throw new Exception();
                                }
                            }
                            else
                            {
                                throw new Exception();
                            }
                        }
                        else
                        {
                            Regex pattern = new Regex(@"\[|\]");
                            Match match = pattern.Match(paramValue);
                            if (match.Success)
                            {
                                throw new Exception();
                            }

                            string[] allInstruction = paramValue.Split(",");

                            foreach (string instruction in allInstruction)
                            {
                                if (!TreatParams.CanConvert(instruction, Tproperties.PropertyType))
                                {
                                    throw new Exception();
                                }
                            }

                        }
                        query = query.WhereFieldIs(paramValue, Tproperties.PropertyType, Tproperties.Name);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
            }

            if (fields != null)
            {
                IEnumerable<dynamic> ts = query.SelectOnly(fields);
                if (range != null)
                    return Partial(ts);
                return Ok(ts);
            }



            // CONDITIONS POUR LE RETOUR

            if (range != null)
                return Partial(query);

            return Ok(query);
        }


        // --- GET --- //

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetElements([FromQuery] string range, [FromQuery] string sort, [FromQuery] string fields)
        {
            return await AddFilter(_db.Set<T>().AsQueryable<T>(), range, sort, fields);
        }

        [Route("search")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> Recherche([FromQuery] string range, [FromQuery] string sort, [FromQuery] string fields)
        {
            IQueryable<T> query = _db.Set<T>().AsQueryable<T>();

            string fieldName = this.Request.Query.Keys.First();
            try
            {
                PropertyInfo Tproperties = typeof(T).GetProperty(fieldName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
                if (Tproperties == null || Tproperties.PropertyType != typeof(string))
                {
                    throw new Exception();
                }
                string RechercheValue = this.Request.Query[Tproperties.Name].ToString();
                query = query.WhereSearchOnField(Tproperties.Name, RechercheValue);

                Dictionary<String, Microsoft.Extensions.Primitives.StringValues> paramsTempo = new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
                foreach (string paramName in this.Request.Query.Keys)
                {
                    if(paramName != fieldName)
                    {
                        paramsTempo.Add(paramName, this.Request.Query[paramName]);
                    }
                }

                this.Request.Query = new Microsoft.AspNetCore.Http.QueryCollection(paramsTempo);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            var kek2 = this.Request.Query;
            return Ok(query);
            //return await AddFilter(query, range, sort, fields);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<T>> GetElementById([FromRoute] int id)
        {
            T item = await _db.Set<T>().FindAsync(id);

            return Ok(item);
        }

        // --- POST --- //

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateElement([FromBody] T item)
        {
            if (ModelState.IsValid)
            {
                _db.Set<T>().Add(item);
                await _db.SaveChangesAsync();

                return Created("", item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // --- PUT --- //

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> UpdateElement([FromRoute] K id, [FromBody] T item)
        {
            if (!(ModelState.IsValid))
            {
                return BadRequest(ModelState);
            }
            if (!(id.Equals(item.ID)))
            {
                return BadRequest();
            }

            try
            {
                _db.Set<T>().Update(item);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return NoContent();
        }

        // --- DELETE --- //

        [Route("{id}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> DeleteElement([FromRoute] int id)
        {
            T delItem = await _db.Set<T>().FindAsync(id);
            if (delItem == null)
                return NotFound();

            _db.Remove(delItem);

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
