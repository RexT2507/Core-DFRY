﻿using ApiLibrary.Core.Attributes;
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

        // --- GET --- //

        [HttpGet]
        public async Task<ActionResult<IEnumerable<T>>> GetElements([FromQuery] string range, [FromQuery] string sort, [FromQuery] string fields)
        {
            int pagination;

            if (range == null)
            {
                if (sort == null)
                {
                    if (fields == null)
                    {
                        var set = await _db.Set<T>().ToListAsync();
                        return Ok(set);
                    }
                    else
                    {
                        var set = _db.Set<T>().SelectOnly(fields);
                        return Ok(set);
                    }
                }
                else
                {
                    try
                    {
                        IOrderedQueryable<T> oq = null;
                        var tab = sort.Split(',');
                        string sortProp = typeof(T).GetProperty(tab[0], BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance).Name;
                        oq = _db.Set<T>().OrderBy(sortProp);
                        foreach (var item in tab.Skip(1))
                        {
                            string thenProp = typeof(T).GetProperty(item, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance).Name;
                            oq = oq.ThenBy(thenProp);
                        }
                        return Ok(oq);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e.Message);
                    }
                }
            }

            try { pagination = this.GetType().GetCustomAttribute<MaxPaginationAttribute>().Range; } catch (NullReferenceException) { pagination = typeof(ControllerRef<C, T, K>).GetCustomAttribute<MaxPaginationAttribute>().Range; }

            var myRange = range.Split("-");

            IQueryable<T> query = _db.Set<T>().AsQueryable<T>();
            query = query.Where(x => x.deletedAt == null);

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

            var displayed = await query.ToListAsync();

            if (sort != null)
            {
                try
                {
                    IOrderedQueryable<T> oq = null;
                    var tab = sort.Split(',');
                    string sortProp = typeof(T).GetProperty(tab[0], BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance).Name;
                    oq = _db.Set<T>().OrderBy(sortProp);
                    foreach (var item in tab.Skip(1))
                    {
                        string thenProp = typeof(T).GetProperty(item, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance).Name;
                        oq = oq.ThenBy(thenProp);
                    }
                    return Partial(oq);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
            if (fields != null)
            {
                var set = query.SelectOnly(fields);
                return Partial(set);
            }

            return Partial(displayed);
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