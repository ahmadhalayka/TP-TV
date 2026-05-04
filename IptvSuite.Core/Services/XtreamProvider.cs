using System.Net;
using System.Text.Json;
using IptvSuite.Core.Models;

namespace IptvSuite.Core.Services;

/// <summary>
/// Xtream Codes Provider
/// يعتمد على player_api.php?action=...
/// ويدعم get_live_categories / get_live_streams
/// </summary>
public class XtreamProvider : IStreamProvider
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _json = new(JsonSerializerDefaults.Web);

    public XtreamProvider(HttpClient http)
    {
        _http = http;
    }

    public string Name => "Xtream";
    public ProviderMode Mode => ProviderMode.Xtream;
    public bool SupportsCatalog => true;

    public async Task<bool> TestAsync(ProviderConfig cfg, CancellationToken ct = default)
    {
        cfg.Normalize();

        if (string.IsNullOrWhiteSpace(cfg.XtreamBaseUrl))
            return false;

        var url = $"{cfg.XtreamBaseUrl}/player_api.php?username={Enc(cfg.Username)}&password={Enc(cfg.Password)}";

        try
        {
            var resp = await _http.GetAsync(url, ct);
            var body = await resp.Content.ReadAsStringAsync(ct);

            // كثير سيرفرات ترجع 200 حتى لو auth=0، لذلك نفحص JSON
            if (!resp.IsSuccessStatusCode) return false;

            // فحص بسيط بدون DTO كامل
            if (body.Contains("\"auth\":1", StringComparison.OrdinalIgnoreCase)) return true;
            if (body.Contains("\"status\":\"Active\"", StringComparison.OrdinalIgnoreCase)) return true;

            return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<Category>> GetLiveCategoriesAsync(ProviderConfig cfg, CancellationToken ct = default)
    {
        cfg.Normalize();

        var url = $"{cfg.XtreamBaseUrl}/player_api.php?username={Enc(cfg.Username)}&password={Enc(cfg.Password)}&action=get_live_categories";
        var json = await _http.GetStringAsync(url, ct);

        var dtos = JsonSerializer.Deserialize<List<CategoryDto>>(json, _json) ?? new();
        return dtos.Select(x => new Category { Id = x.category_id ?? "", Name = x.category_name ?? "" })
                   .Where(x => !string.IsNullOrWhiteSpace(x.Id))
                   .ToList();
    }

    public async Task<List<MediaItem>> GetLiveChannelsAsync(ProviderConfig cfg, string? categoryId = null, CancellationToken ct = default)
    {
        cfg.Normalize();

        var url = $"{cfg.XtreamBaseUrl}/player_api.php?username={Enc(cfg.Username)}&password={Enc(cfg.Password)}&action=get_live_streams";
        if (!string.IsNullOrWhiteSpace(categoryId))
            url += $"&category_id={Enc(categoryId)}";

        var json = await _http.GetStringAsync(url, ct);

        var dtos = JsonSerializer.Deserialize<List<LiveDto>>(json, _json) ?? new();

        return dtos.Select(x => new MediaItem
        {
            Id = x.stream_id ?? "",
            Name = x.name ?? "",
            Logo = x.stream_icon,
            CategoryId = x.category_id,
            Type = MediaType.Live,
            StreamUrl = null
        })
                   .Where(x => !string.IsNullOrWhiteSpace(x.Id))
                   .ToList();
    }

    public Task<string> GetPlaybackUrlAsync(ProviderConfig cfg, MediaItem item, CancellationToken ct = default)
    {
        cfg.Normalize();

        // Xtream تشغيل Live غالباً يكون /live/USER/PASS/STREAM_ID.ts (أو m3u8 حسب السيرفر) [4](https://xn--gl-xka.dev/blog/xtream-codes-api-guide)
        var ext = string.IsNullOrWhiteSpace(cfg.XtreamLiveExt) ? "ts" : cfg.XtreamLiveExt.Trim().TrimStart('.');
        var url = $"{cfg.XtreamBaseUrl}/live/{EncPath(cfg.Username)}/{EncPath(cfg.Password)}/{EncPath(item.Id)}.{ext}";
        return Task.FromResult(url);
    }

    private static string Enc(string s) => WebUtility.UrlEncode(s ?? "");
    private static string EncPath(string s) => WebUtility.UrlEncode(s ?? "");

    private class CategoryDto
    {
        public string? category_id { get; set; }
        public string? category_name { get; set; }
    }

    private class LiveDto
    {
        public string? stream_id { get; set; }
        public string? name { get; set; }
        public string? stream_icon { get; set; }
        public string? category_id { get; set; }
    }
}