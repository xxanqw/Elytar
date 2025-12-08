using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;

namespace Elytar.Services;

public class ElyByService : IElyByService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private const string BaseUrl = "http://skinsystem.ely.by/textures/";

    public ElyByService(HttpClient httpClient, IMemoryCache cache)
    {
        _httpClient = httpClient;
        _cache = cache;
    }

    public async Task<SkinContext?> GetSkinContextAsync(string username)
    {
        if (_cache.TryGetValue(username, out SkinContext? cachedContext))
        {
            return cachedContext;
        }

        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}{username}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ElyByResponse>(json);

                if (data?.Skin != null)
                {
                    var skinUrl = data.Skin.Url;
                    var isSlim = data.Skin.Metadata?.Model == "slim";

                    var skinResponse = await _httpClient.GetAsync(skinUrl);
                    if (skinResponse.IsSuccessStatusCode)
                    {
                        var skinBytes = await skinResponse.Content.ReadAsByteArrayAsync();
                        var context = new SkinContext(skinBytes, isSlim);

                        _cache.Set(username, context, TimeSpan.FromMinutes(10));
                        return context;
                    }
                }
            }
        }
        catch
        {
            // Log error or ignore
        }

        return null;
    }

    public async Task<byte[]?> GetSkinAsync(string username)
    {
        var context = await GetSkinContextAsync(username);
        return context?.SkinBytes;
    }
}

public class ElyByResponse
{
    [JsonPropertyName("SKIN")]
    public ElyBySkin? Skin { get; set; }
}

public class ElyBySkin
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("metadata")]
    public ElyByMetadata? Metadata { get; set; }
}

public class ElyByMetadata
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
}
