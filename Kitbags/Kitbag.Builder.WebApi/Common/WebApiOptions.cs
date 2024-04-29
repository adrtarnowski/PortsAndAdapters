using System.Collections.Generic;

namespace Kitbag.Builder.WebApi.Common;

public class WebApiOptions
{
    public ICollection<string>? CorsAllowedOrigins { get; set; }
}