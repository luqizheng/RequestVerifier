using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RequestVerifier.Signature;

namespace RequestVerifier
{
    public static class SignatureExtentions
    {
        public static IServiceCollection AddVerifyRequestSignature<T>(this IServiceCollection service, string httpSignatureHeaderName, Func<T> addSignature)
        where T : ISignature
        {
            service.AddScoped(typeof(ISignature), typeof(T));

            service.AddSingleton(a => new SignatureSetting()
            {
                Header = httpSignatureHeaderName
            });
            service.AddTransient(sp => (ISignature)addSignature());
            return service;
        }
        public static IApplicationBuilder UseResponseSignature(this IApplicationBuilder app)
        {
            app.UseMiddleware<VerifySignatureMiddleware>();
            return app;
        }
    }


}
