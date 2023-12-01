namespace Kitbag.Builder.Redis.Common;

public class RedisProperties
{
    public string? Host { get; set; }
    public string? Port { get; set; }
    public bool Ssl { get; set; }
    public string? Password { get; set; }
}