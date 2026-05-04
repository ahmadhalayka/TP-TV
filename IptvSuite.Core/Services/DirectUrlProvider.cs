using IptvSuite.Core.Models;

namespace IptvSuite.Core.Services;

/// <summary>
/// Direct URL Provider
/// - لا يعتمد على player_api.php
/// - تشغيل مباشر عبر StreamUrl أو عبر Template
/// </summary>
public class DirectUrlProvider : IStreamProvider
{
    public string Name => "Direct URL";
    public ProviderMode Mode => ProviderMode.DirectUrl;
    public bool SupportsCatalog => false;

    public Task<bool> TestAsync(ProviderConfig cfg, CancellationToken ct = default)
    {
        cfg.Normalize();
        // في direct mode "الاختبار" بسيط: وجود base أو template
        if (!string.IsNullOrWhiteSpace(cfg.DirectTemplate)) return Task.FromResult(true);
        if (!string.IsNullOrWhiteSpace(cfg.DirectBaseUrl)) return Task.FromResult(true);
        return Task.FromResult(false);
    }

    public Task<List<Category>> GetLiveCategoriesAsync(ProviderConfig cfg, CancellationToken ct = default)
        => Task.FromResult(new List<Category>());

    public Task<List<MediaItem>> GetLiveChannelsAsync(ProviderConfig cfg, string? categoryId = null, CancellationToken ct = default)
        => Task.FromResult(new List<MediaItem>());

    public Task<string> GetPlaybackUrlAsync(ProviderConfig cfg, MediaItem item, CancellationToken ct = default)
    {
        cfg.Normalize();

        // 1) إذا العنصر يحمل رابط مباشر (أفضل حل ثابت)
        if (!string.IsNullOrWhiteSpace(item.StreamUrl))
            return Task.FromResult(item.StreamUrl!);

        // 2) إذا يوجد Template
        if (!string.IsNullOrWhiteSpace(cfg.DirectTemplate))
        {
            var url = cfg.DirectTemplate
                .Replace("{base}", cfg.DirectBaseUrl)
                .Replace("{user}", cfg.Username)
                .Replace("{pass}", cfg.Password)
                .Replace("{id}", item.Id);

            return Task.FromResult(url);
        }

        // 3) لا يوجد شيء لبناء الرابط
        throw new InvalidOperationException("Direct mode requires MediaItem.StreamUrl OR ProviderConfig.DirectTemplate.");
    }
}