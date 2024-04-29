using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kitbag.Builder.WebApi.RunningContext;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;
    public const string CorrelationHeaderKey = "CorrelationId";

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(CorrelationHeaderKey))
        {
            var correlationId = Guid.NewGuid();
            context.Request.Headers.Add(CorrelationHeaderKey, correlationId.ToString());
        }

        await this._next.Invoke(context);
    }
}