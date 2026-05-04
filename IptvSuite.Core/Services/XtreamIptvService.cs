
using System.Net.Http.Json;
using IptvSuite.Core.Models;

namespace IptvSuite.Core.Services;

/// <summary>
/// تنفيذ IPTV باستخدام Xtream Codes API
/// </summary>
public class XtreamIptvService : IIptvService
{
    private readonly HttpClient _httpClient;
    private IptvAccount? _account;

    public XtreamIptvService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string LastError { get; private set; } = "";
    public async Task<bool> LoginAsync(IptvAccount account)
    {
        _account = account;

        var url = $"{account.ServerUrl}/player_api.php" +
                  $"?username={account.Username}&password={account.Password}";

        try
        {
            var response = await _httpClient.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();

            // 🔍 اطبع الرد في الديبغ
            System.Diagnostics.Debug.WriteLine("LOGIN RESPONSE:");
            System.Diagnostics.Debug.WriteLine(json);

            if (!response.IsSuccessStatusCode)
                throw new Exception("HTTP Error");

            // ✅ فحص Xtream الحقيقي
            if (json.Contains("\"auth\":1") || json.Contains("\"status\":\"Active\""))
                return true;

            throw new Exception("Xtream auth failed");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("LOGIN ERROR:");
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            LastError = ex.Message;
            return false;
        }
    }

    public async Task<List<Category>> GetLiveCategoriesAsync()
    {
        EnsureLoggedIn();

        var url =
            $"{_account!.ServerUrl}/player_api.php?username={_account.Username}&password={_account.Password}&action=get_live_categories";

        var result = await _httpClient.GetFromJsonAsync<List<CategoryDto>>(url);

        return result?
            .Select(x => new Category
            {
                Id = x.category_id,
                Name = x.category_name
            })
            .ToList()
            ?? new List<Category>();
    }

    public async Task<List<MediaItem>> GetLiveChannelsAsync(string? categoryId = null)
    {
        EnsureLoggedIn();

        var url =
            $"{_account!.ServerUrl}/player_api.php?username={_account.Username}&password={_account.Password}&action=get_live_streams";

        if (!string.IsNullOrWhiteSpace(categoryId))
            url += $"&category_id={categoryId}";

        var result = await _httpClient.GetFromJsonAsync<List<LiveChannelDto>>(url);

        return result?
            .Select(x => new MediaItem
            {
                Id = x.stream_id,
                Name = x.name,
                Logo = x.stream_icon,
                CategoryId = x.category_id,
                Type = MediaType.Live
            })
            .ToList()
            ?? new List<MediaItem>();
    }

    public Task<string> BuildLiveStreamUrlAsync(string streamId)
    {
        EnsureLoggedIn();

        // أغلب سيرفرات Xtream
        var streamUrl =
            $"{_account!.ServerUrl}/live/{_account.Username}/{_account.Password}/{streamId}.m3u8";

        return Task.FromResult(streamUrl);
    }

    private void EnsureLoggedIn()
    {
        if (_account == null)
            throw new InvalidOperationException("You must call LoginAsync first.");
    }

    #region DTOs (internal)

    private class CategoryDto
    {
        public string category_id { get; set; } = "";
        public string category_name { get; set; } = "";
    }

    private class LiveChannelDto
    {
        public string stream_id { get; set; } = "";
        public string name { get; set; } = "";
        public string? stream_icon { get; set; }
        public string? category_id { get; set; }
    }

    #endregion
}
