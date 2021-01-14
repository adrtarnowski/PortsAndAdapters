namespace Kitbag.Builder.HttpClient.Common
{
    public class HttpClientProperties
    {
        public int Retries { get; set; } = 3;
        public int RetryExponentialBase { get; set; } = 2;
        public string AuthenticationScheme = "Bearer";
    }
}