using Microsoft.Extensions.DependencyInjection;

namespace SketchOverlay.Wpf;

public static class DependencyBuilderExtensions
{
    public static ServiceCollection AddServices(this ServiceCollection builder)
    {
        return builder;
    }

    public static ServiceCollection AddViewModels(this ServiceCollection builder)
    {
        return builder;
    }
}