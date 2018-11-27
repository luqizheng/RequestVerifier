using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RequestVerifier.Signature;

namespace RequestVerifier
{
    public class VerifySignatureMiddleware
    {
        private readonly ILogger<VerifySignatureMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly SignatureSetting _setting;


        public VerifySignatureMiddleware(RequestDelegate next, SignatureSetting setting,
            ILogger<VerifySignatureMiddleware> logger)
        {
            _next = next;
            _setting = setting;

            _logger = logger;
        }

        public Task Invoke(HttpContext context)
        {
            try
            {
                if (!_setting.Support(context.Request.Method) || context.Response.Headers.ContainsKey(_setting.Header))
                    return Task.FromResult(0);

                var bodyStream = context.Response.Body;
                var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;


                var result = _next(context);


                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBody = new StreamReader(responseBodyStream).ReadToEnd();

                if (context.Response.Headers.TryGetValue(_setting.Header, out var sign))
                    BuildResponse(context, responseBody);

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                responseBodyStream.CopyTo(bodyStream);

                return result;
            }

            catch (Exception nottdException)
            {
                context.Response.StatusCode = 500;
                context.Response.WriteAsync(JsonConvert.SerializeObject(nottdException));
                return Task.CompletedTask;
            }
        }

        private void BuildResponse(HttpContext context, string responseBody)
        {
            var headers = context.Response.Headers;

            var signature = context.RequestServices.GetService(typeof(ISignature)) as ISignature;

            headers[_setting.Header] = signature.Sign(Encoding.UTF8.GetBytes(responseBody));
        }
    }
}