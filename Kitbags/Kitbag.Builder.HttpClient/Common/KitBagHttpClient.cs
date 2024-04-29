using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Polly;

namespace Kitbag.Builder.HttpClient.Common;

public class KitBagHttpClient : IHttpClient
{
    private readonly System.Net.Http.HttpClient _client;
    private readonly HttpClientProperties _options;
    private readonly ILogger<KitBagHttpClient> _logger;
    private const string ApplicationJsonContentType = "application/json";

    private static readonly StringContent EmptyJson =
        new StringContent("{}", Encoding.UTF8, ApplicationJsonContentType);

    private static readonly JsonSerializer _jsonSerializer = new JsonSerializer
    {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    protected enum Method
    {
        Get,
        Post,
        Put,
        Delete
    }

    public KitBagHttpClient(System.Net.Http.HttpClient client, HttpClientProperties options,
        ILogger<KitBagHttpClient> logger)
    {
        _client = client;
        _options = options;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> GetAsync(string uri)
    {
        return await SendAsync(uri, Method.Get);
    }

    public async Task<T> GetAsync<T>(string uri)
    {
        return await SendAsync<T>(uri, Method.Get);
    }

    public async Task<HttpResponseMessage> PostAsync(string uri, object? data = null)
    {
        return await SendAsync(uri, Method.Post, data);
    }

    public async Task<T> PostAsync<T>(string uri, object? data = null)
    {
        return await SendAsync<T>(uri, Method.Post, data);
    }

    public async Task<HttpResponseMessage> PutAsync(string uri, object? data = null)
    {
        return await SendAsync(uri, Method.Put, data);
    }

    public async Task<T> PutAsync<T>(string uri, object? data = null)
    {
        return await SendAsync<T>(uri, Method.Put, data);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string uri)
    {
        return await SendAsync(uri, Method.Delete);
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
    {
        return await Policy.Handle<Exception>()
            .WaitAndRetryAsync(_options.Retries, r => TimeSpan.FromSeconds(Math.Pow(2, r)))
            .ExecuteAsync(() => _client.SendAsync(request));
    }

    public async Task<T> SendAsync<T>(HttpRequestMessage request)
    {
        return await Policy.Handle<Exception>()
            .WaitAndRetryAsync(_options.Retries, r => TimeSpan.FromSeconds(Math.Pow(2, r)))
            .ExecuteAsync(async () =>
            {
                var response = await _client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    return default!;
                }

                var stream = await response.Content.ReadAsStreamAsync();
                return DeserializeJsonFromStream<T>(stream);
            });
    }

    public void SetHeaders(IDictionary<string, string> headers)
    {
        _client.DefaultRequestHeaders.Clear();
        foreach (var (key, value) in headers)
        {
            if (string.IsNullOrEmpty(key))
                continue;
            _client.DefaultRequestHeaders.TryAddWithoutValidation(key, value);
        }
    }

    protected async Task<T> SendAsync<T>(string uri, Method method, object? data = null)
    {
        var response = await SendAsync(uri, method, data);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Request {method} '{uri}' returned {response.StatusCode}");
            return default!;
        }

        var stream = await response.Content.ReadAsStreamAsync();
        return DeserializeJsonFromStream<T>(stream);
    }

    protected async Task<HttpResponseMessage> SendAsync(string uri, Method method, object? data = null)
    {
        return await Policy.Handle<Exception>()
            .WaitAndRetryAsync(_options.Retries, r => TimeSpan.FromSeconds(Math.Pow(2, r)))
            .ExecuteAsync(() =>
            {
                var requestUri = uri.StartsWith("http") ? uri : $"http://{uri}";
                return GetResponseAsync(requestUri, method, data);
            });
    }

    protected Task<HttpResponseMessage> GetResponseAsync(string uri, Method method, object? data = null)
    {
        switch (method)
        {
            case Method.Get:
                return _client.GetAsync(uri);
            case Method.Post:
                return _client.PostAsync(uri, GetJsonPayload(data));
            case Method.Put:
                return _client.PutAsync(uri, GetJsonPayload(data));
            case Method.Delete:
                return _client.DeleteAsync(uri);
            default:
                throw new InvalidOperationException($"Unsupported HTTP method: {method}");
        }
    }

    private static StringContent GetJsonPayload(object? data)
    {
        if (data == null)
            return EmptyJson;

        if (data is string)
            return new StringContent((string) data);

        return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, ApplicationJsonContentType);
    }

    private static T DeserializeJsonFromStream<T>(Stream stream)
    {
        if (stream == null || stream.CanRead == false)
            return default!;

        using var streamReader = new StreamReader(stream);
        using var jsonTextReader = new JsonTextReader(streamReader);
        return _jsonSerializer.Deserialize<T>(jsonTextReader)!;
    }

    public void SetAuthorization(string? parameter = null)
    {
        if (string.IsNullOrEmpty(_options.AuthenticationScheme))
            return;

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(_options.AuthenticationScheme, parameter);
    }
}