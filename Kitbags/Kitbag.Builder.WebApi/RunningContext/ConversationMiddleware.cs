using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kitbag.Builder.WebApi.RunningContext;

public class ConversationMiddleware
{
    private readonly RequestDelegate _next;
    public const string ConversationHeaderKey = "ConversationId";

    public ConversationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(ConversationHeaderKey))
        {
            var conversationId = Guid.NewGuid();
            context.Request.Headers.Add(ConversationHeaderKey, conversationId.ToString());
        }

        await _next.Invoke(context);
    }
}