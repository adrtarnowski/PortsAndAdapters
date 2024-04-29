using Microsoft.AspNetCore.Mvc;

namespace Kitbag.Builder.WebApi.Exceptions.Handlers;

public static class ProblemDetailsExtensions
{
    public const string CorrelationId = "correlationId";
    public const string ConversationId = "correlationId";
    public const string TraceId = "traceId";
    public const string UserId = "userId";
    public const string RequestId = "requestId";

    public static void SetExtension(this ProblemDetails problemDetails, string key, object value)
    {
        problemDetails.Extensions[key] = value;
    }
}