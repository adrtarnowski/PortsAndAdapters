using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Kitbag.Builder.WebApi.RunningContext;

public class HttpRunningContextProvider : IHttpRunningContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpRunningContextProvider> _logger;
    private const string RequestIdHeader = "RequestId";
    public string? UserId => _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
    public Guid? RequestId => GetValueFromHttpHeader(RequestIdHeader);
    public Guid? CorrelationId => GetValueFromHttpHeader(CorrelationMiddleware.CorrelationHeaderKey);
    public Guid? ConversationId => GetValueFromHttpHeader(ConversationMiddleware.ConversationHeaderKey);
    private bool IsAvailable => _httpContextAccessor.HttpContext != null;
        
    public HttpRunningContextProvider(IHttpContextAccessor httpContextAccessor,
        ILogger<HttpRunningContextProvider> logger)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
        
    private Guid? GetValueFromHttpHeader(string keyName)
    {
        if (_httpContextAccessor.HttpContext != null && IsAvailable && _httpContextAccessor.HttpContext.Request.Headers.ContainsKey(keyName))
        {
            var isValid = Guid.TryParse(_httpContextAccessor.HttpContext.Request.Headers[keyName], out Guid value);
            if (!isValid)
                _logger.LogError($@"Could not parse header '{keyName}' with value '{_httpContextAccessor.HttpContext.Request.Headers[keyName]}'");
            return value;
        }
        _logger.LogInformation($"Empty guid for '{keyName}' (IsAvailable: {IsAvailable}).");
        return Guid.Empty;
    }
}