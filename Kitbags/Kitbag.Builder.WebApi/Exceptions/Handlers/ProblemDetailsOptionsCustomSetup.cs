using Hellang.Middleware.ProblemDetails;
using Kitbag.Builder.WebApi.RunningContext;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Kitbag.Builder.WebApi.Exceptions.Handlers;

public class ProblemDetailsOptionsCustomSetup : IConfigureOptions<ProblemDetailsOptions>
{
    private readonly IHostEnvironment _hostingEnvironment;
    private readonly IHttpRunningContextProvider _httpRunningContextProvider;

    public ProblemDetailsOptionsCustomSetup(IHostEnvironment hostingEnvironment,
        IHttpRunningContextProvider httpRunningContextProvider)
    {
        _hostingEnvironment = hostingEnvironment;
        _httpRunningContextProvider = httpRunningContextProvider;
    }

    public void Configure(ProblemDetailsOptions options)
    {
        options.IncludeExceptionDetails = (context, exception) => _hostingEnvironment.IsDevelopment();
        options.ShouldLogUnhandledException = (http, ex, problemDetails) => true;
        options.OnBeforeWriteDetails = (ctx, details) =>
        {
            details.SetExtension(ProblemDetailsExtensions.UserId, _httpRunningContextProvider.UserId!);
            details.SetExtension(ProblemDetailsExtensions.TraceId, ctx.TraceIdentifier);
            details.SetExtension(ProblemDetailsExtensions.RequestId, _httpRunningContextProvider.RequestId!);
            details.SetExtension(ProblemDetailsExtensions.CorrelationId, _httpRunningContextProvider.CorrelationId!);
            details.SetExtension(ProblemDetailsExtensions.ConversationId, _httpRunningContextProvider.ConversationId!);
            details.Instance = ctx.Request.Path;
        };
    }
}