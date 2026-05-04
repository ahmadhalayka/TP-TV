using IptvSuite.Core.Models;

namespace IptvSuite.Core.Services;

/// <summary>
/// واجهة خدمة IPTV
/// </summary>
public interface IIptvService
{
    Task<bool> LoginAsync(IptvAccount account);

    Task<List<Category>> GetLiveCategoriesAsync();
    Task<List<MediaItem>> GetLiveChannelsAsync(string? categoryId = null);

    Task<string> BuildLiveStreamUrlAsync(string streamId);
}