using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kitbag.Builder.WebApi.Common
{
    public class ActionCorrelationMiddleware
    {
        private readonly RequestDelegate _next;
        public const string ActionCorrelationHeaderKey = "ActionCorrelationId";

        public ActionCorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(ActionCorrelationHeaderKey))
            {
                var actionCorrelationId = Guid.NewGuid();
                context.Request.Headers.Add(ActionCorrelationHeaderKey, actionCorrelationId.ToString());
            }

            await this._next.Invoke(context);
        }
    }
}