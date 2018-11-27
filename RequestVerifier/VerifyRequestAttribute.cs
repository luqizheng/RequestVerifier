using System.Threading;
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
            var setting = (SignatureSetting)context.HttpContext.RequestServices.GetService(typeof(SignatureSetting));
            if (setting.Support(context.HttpContext.Request.Method))
                return;
            var signature = (ISignature)context.HttpContext.RequestServices.GetService(typeof(ISignature));
            var method = context.HttpContext.Request.Method.ToLower();
            var verifySign = "";
            if (method != "get" && method != "delete")
            {
                context.HttpContext.Request.EnableRewind();
                var stream = context.HttpContext.Request.Body;
                verifySign = signature.Sign(stream);
            }
            else
            {
                var bytes = setting.Encoding.GetBytes(context.HttpContext.Request.QueryString.ToString());
                verifySign = signature.Sign(bytes);
            }
            if (verifySign != context.HttpContext.Request.Headers[_header])
                context.Result = new ForbidResult();
        }
    }
}