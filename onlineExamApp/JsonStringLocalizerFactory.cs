
using Microsoft.Extensions.Localization;
using JsonLocalization;

namespace EjadaTraineesManagementSystem;

public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer();
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer();
    }
}
