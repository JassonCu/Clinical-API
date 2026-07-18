using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Clinical.Web.Infrastructure.Services;

public abstract class BaseApiService
{
    private readonly IHttpClientFactory _factory;
    private readonly IHttpContextAccessor _accessor;
    private readonly ILogger _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    protected BaseApiService(IHttpClientFactory factory, IHttpContextAccessor accessor, ILogger logger)
    {
        _factory = factory;
        _accessor = accessor;
        _logger = logger;
    }

    protected HttpClient CreateClient()
    {
        var client = _factory.CreateClient("ClinicalAPI");
        var token = _accessor.HttpContext?.Request.Cookies["X-Clinical-Token"];
        if (!string.IsNullOrWhiteSpace(token))
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    /// <summary>GET → deserializes the raw payload on 2xx; default on 404/error.</summary>
    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var client = CreateClient();
            var response = await client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;
            var json = await response.Content.ReadAsStringAsync();
            return string.IsNullOrWhiteSpace(json) ? default : JsonSerializer.Deserialize<T>(json, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET {Endpoint} failed", endpoint);
            return default;
        }
    }

    protected Task<(bool Success, string? Error)> PostAsync<T>(string endpoint, T body)
        => SendAsync(HttpMethod.Post, endpoint, body);

    protected Task<(bool Success, string? Error)> PutAsync<T>(string endpoint, T body)
        => SendAsync(HttpMethod.Put, endpoint, body);

    protected Task<(bool Success, string? Error)> PatchAsync<T>(string endpoint, T body)
        => SendAsync(HttpMethod.Patch, endpoint, body);

    protected Task<(bool Success, string? Error)> DeleteAsync(string endpoint)
        => SendAsync<object?>(HttpMethod.Delete, endpoint, null);

    /// <summary>Command call → success is the HTTP status (200/201/204); error text comes from ProblemDetails.</summary>
    private async Task<(bool Success, string? Error)> SendAsync<T>(HttpMethod method, string endpoint, T? body)
    {
        try
        {
            var client = CreateClient();
            using var request = new HttpRequestMessage(method, endpoint);
            if (body is not null)
                request.Content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode) return (true, null);

            var json = await response.Content.ReadAsStringAsync();
            return (false, ExtractProblem(json));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Method} {Endpoint} failed", method, endpoint);
            return (false, "Error de conexión con el servidor.");
        }
    }

    /// <summary>POST that returns a payload (e.g. auth tokens) → deserializes the raw result on 2xx.</summary>
    protected async Task<(bool Success, TResult? Data, string? Error)> PostWithResultAsync<T, TResult>(string endpoint, T body)
    {
        try
        {
            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);
            var json = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var data = string.IsNullOrWhiteSpace(json) ? default : JsonSerializer.Deserialize<TResult>(json, JsonOptions);
                return (true, data, null);
            }
            return (false, default, ExtractProblem(json));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} failed", endpoint);
            return (false, default, "Error de conexión con el servidor.");
        }
    }

    /// <summary>Pulls a human message out of an RFC 9457 problem+json body.</summary>
    private static string ExtractProblem(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (root.TryGetProperty("detail", out var detail) && detail.ValueKind == JsonValueKind.String)
                return detail.GetString()!;
            if (root.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String)
                return title.GetString()!;
        }
        catch { /* non-JSON body */ }
        return "Error en la operación.";
    }
}
