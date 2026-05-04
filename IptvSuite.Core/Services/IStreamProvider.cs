using IptvSuite.Core.Models;

namespace IptvSuite.Core.Services;

/// <summary>
/// واجهة موحدة لمزوّدي IPTV
/// Xtream: يدعم كتالوج (تصنيفات/قنوات/..)
/// DirectUrl: تشغيل مباشر (قد لا يدعم كتالوج)
/// </summary>
public interface IStreamProvider
{
    string Name { get; }
    ProviderMode Mode { get; }
    bool SupportsCatalog { get; }

    /// <summary>
    /// اختبار اتصال/تسجيل بسيط حسب المزود
    /// </summary>
    Task<bool> TestAsync(ProviderConfig cfg, CancellationToken ct = default);

    /// <summary>
    /// تصنيفات Live (إن كانت مدعومة)
    /// </summary>
    Task<List<Category>> GetLiveCategoriesAsync(ProviderConfig cfg, CancellationToken ct = default);

    /// <summary>
    /// قنوات Live (إن كانت مدعومة)
    /// </summary>
    Task<List<MediaItem>> GetLiveChannelsAsync(ProviderConfig cfg, string? categoryId = null, CancellationToken ct = default);

    /// <summary>
    /// توليد رابط تشغيل لعنصر (قناة/فلم/..)
    /// </summary>
    Task<string> GetPlaybackUrlAsync(ProviderConfig cfg, MediaItem item, CancellationToken ct = default);
}
