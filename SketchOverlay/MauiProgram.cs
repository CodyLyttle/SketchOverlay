using SketchOverlay.Drawing;
using SketchOverlay.Drawing.Canvas;
using SketchOverlay.ViewModels;
using SketchOverlay.Views;

namespace SketchOverlay;

public static class MauiProgram
{
    private static IServiceProvider? _serviceProvider;

    public static TService GetService<TService>()
    {
        if (_serviceProvider == null)
            throw new NullReferenceException("Service provider is null");

        var service = _serviceProvider.GetService<TService>();
        if (service == null)
            throw new ArgumentOutOfRangeException(nameof(TService), $"'{typeof(TService).Name}' service doesn't exist");

        return service;
    }

    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .AddPages()
            .AddServices()
            .AddViewModels();

        MauiApp app = builder.Build();
        _serviceProvider = app.Services;
        return app;
    }

    public static MauiAppBuilder AddPages(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<MainPage>();
        return builder;
    }

    public static MauiAppBuilder AddServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IDrawingCanvas>(new DrawingCanvas(GlobalDrawingValues.DefaultDrawingTool.Tool));
        return builder;
    }

    public static MauiAppBuilder AddViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<OverlayWindowViewModel>();
        builder.Services.AddSingleton<DrawingToolWindowViewModel>();
        return builder;
    }
}
