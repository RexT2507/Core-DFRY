using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ApiLibrary.Core.HttpContent
{
    public class PartialContent : ActionResult, IActionResult
    {
        private readonly object _result;

        public PartialContent(object o)
        {
            _result = o;
        }
        public override Task ExecuteResultAsync(ActionContext context)
        {
            ObjectResult or = new ObjectResult(_result);
            or.StatusCode = 206;
            context.HttpContext.Response.StatusCode = 206;
            return or.ExecuteResultAsync(context);
        }
    }
}
