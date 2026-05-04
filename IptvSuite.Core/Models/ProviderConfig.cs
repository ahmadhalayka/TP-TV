namespace IptvSuite.Core.Models;

public enum ProviderMode
{
    Xtream,
    DirectUrl
}

/// <summary>
/// إعدادات مزوّد IPTV (Xtream أو Direct URL)
/// </summary>
public class ProviderConfig
{
    public ProviderMode Mode { get; set; } = ProviderMode.Xtream;

    // مشترك بين الطريقتين (إذا احتجنا username/pass داخل template)
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";

    // Xtream
    /// <summary>
    /// مثال: http://server:8080
    /// </summary>
    public string XtreamBaseUrl { get; set; } = "";

    /// <summary>
    /// امتداد تشغيل LIVE في Xtream: ts أو m3u8 (افتراضي ts)
    /// </summary>
    public string XtreamLiveExt { get; set; } = "ts";

    // Direct URL
    /// <summary>
    /// مثال: http://117.55.202.102:8080
    /// </summary>
    public string DirectBaseUrl { get; set; } = "";

    /// <summary>
    /// قالب اختياري لتوليد الرابط:
    /// {base} {user} {pass} {id}
    /// مثال: {base}/{user}/{pass}/{id}.m3u8
    /// </summary>
    public string DirectTemplate { get; set; } = "";

    /// <summary>
    /// هل نسمح بـ HTTP (cleartext)؟ (مفيد لو السيرفر HTTP فقط)
    /// </summary>
    public bool AllowHttp { get; set; } = true;

    public ProviderConfig Normalize()
    {
        XtreamBaseUrl = (XtreamBaseUrl ?? "").Trim().TrimEnd('/');
        DirectBaseUrl = (DirectBaseUrl ?? "").Trim().TrimEnd('/');
        Username = (Username ?? "").Trim();
        Password = (Password ?? "").Trim();
        XtreamLiveExt = string.IsNullOrWhiteSpace(XtreamLiveExt) ? "ts" : XtreamLiveExt.Trim().TrimStart('.');
        DirectTemplate = (DirectTemplate ?? "").Trim();
        return this;
    }
}