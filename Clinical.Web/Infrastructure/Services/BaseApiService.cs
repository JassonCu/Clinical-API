using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Clinical.Web.Core.Models;

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

    protected async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            var client = CreateClient();
            var response = await client.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode) return default;
            var json = await response.Content.ReadAsStringAsync();
            var wrapped = JsonSerializer.Deserialize<ApiResponse<T>>(json, JsonOptions);
            return wrapped is { IsSuccess: true } ? wrapped.Data : default;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET {Endpoint} failed", endpoint);
            return default;
        }
    }

    protected async Task<(bool Success, string? Error)> PostAsync<T>(string endpoint, T body)
    {
        try
        {
            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(endpoint, content);
            var json = await response.Content.ReadAsStringAsync();
            var wrapped = TryDeserializeBase(json);
            if (response.IsSuccessStatusCode && (wrapped?.IsSuccess ?? true)) return (true, null);
            return (false, wrapped?.Message ?? TryExtractError(json));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} failed", endpoint);
            return (false, "Error de conexión con el servidor.");
        }
    }

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
                var wrapped = JsonSerializer.Deserialize<ApiResponse<TResult>>(json, JsonOptions);
                if (wrapped is { IsSuccess: true })
                    return (true, wrapped.Data, null);
                return (false, default, wrapped?.Message ?? "Error en la operación.");
            }
            return (false, default, TryExtractError(json));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST {Endpoint} failed", endpoint);
            return (false, default, "Error de conexión con el servidor.");
        }
    }

    protected async Task<(bool Success, string? Error)> PutAsync<T>(string endpoint, T body)
    {
        try
        {
            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
            var response = await client.PutAsync(endpoint, content);
            var json = await response.Content.ReadAsStringAsync();
            var wrapped = TryDeserializeBase(json);
            if (response.IsSuccessStatusCode && (wrapped?.IsSuccess ?? true)) return (true, null);
            return (false, wrapped?.Message ?? TryExtractError(json));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PUT {Endpoint} failed", endpoint);
            return (false, "Error de conexión con el servidor.");
        }
    }

    protected async Task<(bool Success, string? Error)> PatchAsync<T>(string endpoint, T body)
    {
        try
        {
            var client = CreateClient();
            var content = new StringContent(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");
            var response = await client.PatchAsync(endpoint, content);
            var json = await response.Content.ReadAsStringAsync();
            var wrapped = TryDeserializeBase(json);
            if (response.IsSuccessStatusCode && (wrapped?.IsSuccess ?? true)) return (true, null);
            return (false, wrapped?.Message ?? TryExtractError(json));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PATCH {Endpoint} failed", endpoint);
            return (false, "Error de conexión con el servidor.");
        }
    }

    protected async Task<(bool Success, string? Error)> DeleteAsync(string endpoint)
    {
        try
        {
            var client = CreateClient();
            var response = await client.DeleteAsync(endpoint);
            var json = await response.Content.ReadAsStringAsync();
            var wrapped = TryDeserializeBase(json);
            if (response.IsSuccessStatusCode && (wrapped?.IsSuccess ?? true)) return (true, null);
            return (false, wrapped?.Message ?? TryExtractError(json));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "DELETE {Endpoint} failed", endpoint);
            return (false, "Error de conexión con el servidor.");
        }
    }

    private ApiResponse? TryDeserializeBase(string json)
    {
        try { return JsonSerializer.Deserialize<ApiResponse>(json, JsonOptions); }
        catch { return null; }
    }

    private static string TryExtractError(string json)
    {
        try
        {
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("message", out var msg) && msg.ValueKind == JsonValueKind.String)
                return msg.GetString() ?? "Error desconocido";
            if (doc.RootElement.TryGetProperty("title", out var title))
                return title.GetString() ?? "Error desconocido";
        }
        catch { }
        return "Error en la operación.";
    }
}
