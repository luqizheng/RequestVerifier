using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RequestVerifier
{
    public class SignResponseAttribute : ActionFilterAttribute
    {
        private readonly string _header;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        public SignResponseAttribute(string header = "sign")
        {
            _header = header;
        }

        private bool IsEncrypt(HttpContext httpContext)
        {
            var method = httpContext.Request.Method.ToLower();
            if (method != "post" && method != "put")
                return false;
            return true;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!IsEncrypt(context.HttpContext))
                return;
            context.HttpContext.Response.Headers.Add(_header, "");
        }
    }
}