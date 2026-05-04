namespace IptvSuite.Core.Models;

/// <summary>
/// نوع المحتوى
/// </summary>
public enum MediaType
{
    Live,
    Movie,
    Series
}

/// <summary>
/// حساب IPTV (Xtream)
/// </summary>
public class IptvAccount
{
    public string ServerUrl { get; set; } = string.Empty;   // مثال: http://vtv.im:8080
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// التصنيف (Categories)
/// </summary>
public class Category
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// عنصر وسائط (قناة / فلم / حلقة)
/// </summary>
public class MediaItem
{
    public string Id { get; set; } = string.Empty;
    public MediaType Type { get; set; }

    public string Name { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string? CategoryId { get; set; }

    // رابط التشغيل (يُبنى لاحقًا)
    public string? StreamUrl { get; set; }
}

/// <summary>
/// حفظ التقدم (Continue Watching)
/// </summary>
public class PlaybackProgress
{
    public string MediaId { get; set; } = string.Empty;
    public MediaType Type { get; set; }

    public double PositionSeconds { get; set; }
    public DateTime LastUpdated { get; set; }
}
