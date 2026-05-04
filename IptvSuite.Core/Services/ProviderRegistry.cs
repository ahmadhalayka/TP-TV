using IptvSuite.Core.Models;

namespace IptvSuite.Core.Services;

/// <summary>
/// سجل المزوّدات: يرجع المزود حسب Mode
/// </summary>
public class ProviderRegistry
{
    private readonly List<IStreamProvider> _providers;

    public ProviderRegistry(IEnumerable<IStreamProvider> providers)
    {
        _providers = providers.ToList();
    }

    public IStreamProvider Get(ProviderMode mode)
    {
        var p = _providers.FirstOrDefault(x => x.Mode == mode);
        if (p == null)
            throw new InvalidOperationException($"No provider registered for mode: {mode}");
        return p;
    }

    public IReadOnlyList<IStreamProvider> All() => _providers;
}
