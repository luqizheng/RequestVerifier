using System.IO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RequestVerifier.Signature;

namespace RequestVerifier
{
    public class VerifyRequestAttribute : ActionFilterAttribute
    {
        private readonly string _header;

        public VerifyRequestAttribute(string header = "sign")
        {
            _header = header;
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var method = context.HttpContext.Request.Method.ToLower();
            if (method != "post" || method != "put")
                return;
     
            context.HttpContext.Request.EnableRewind();
            var stream = context.HttpContext.Request.Body;

            var signature = context.HttpContext.RequestServices.GetService(typeof(ISignature)) as ISignature;
            signature.Sign(stream);

            var headerSign = context.HttpContext.Request.Headers[_header];

            if (signature != headerSign)
                context.Result = new ForbidResult();


        }
    }
}