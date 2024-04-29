using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kitbag.Builder.HttpClient.Common;

public interface IHttpClient
{
    Task<HttpResponseMessage> GetAsync(string uri);
    Task<T> GetAsync<T>(string uri);
    Task<HttpResponseMessage> PostAsync(string uri, object? data = null);
    Task<T> PostAsync<T>(string uri, object? data = null);
    Task<HttpResponseMessage> PutAsync(string uri, object? data = null);
    Task<T> PutAsync<T>(string uri, object? data = null);
    Task<HttpResponseMessage> DeleteAsync(string uri);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    Task<T> SendAsync<T>(HttpRequestMessage request);
    void SetHeaders(IDictionary<string, string> headers);
    void SetAuthorization(string? parameter = null);
}