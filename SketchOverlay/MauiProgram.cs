using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Drawing;
using SketchOverlay.Drawing.Tools;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Models;
using SketchOverlay.LibraryAdapters;
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
            .AddViewModels()
            .AddWindowStyling();

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
        System.Drawing.Color primaryColor = ColorPalette.Instance.DefaultPrimaryColor;
        System.Drawing.Color? secondaryColor = ColorPalette.Instance.DefaultSecondaryColor;
        float strokeSize = 4;

        DrawingToolCollection<IDrawable, ImageSource> drawingTools = new(
            new DrawingToolInfo<IDrawable, ImageSource>(new MauiPaintBrushTool(primaryColor, strokeSize),
                ImageSource.FromFile("placeholder_paintbrush.png"), "Paintbrush"),
            new DrawingToolInfo<IDrawable, ImageSource>(new MauiLineTool(primaryColor, strokeSize),
                ImageSource.FromFile("placeholder_line.png"), "Line"),
            new DrawingToolInfo<IDrawable, ImageSource>(new MauiRectangleTool(secondaryColor, primaryColor, strokeSize),
                ImageSource.FromFile("placeholder_rectangle.png"), "Rectangle"));

        builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
        builder.Services.AddSingleton<IDrawingToolCollection<IDrawable, ImageSource>>(drawingTools);
        builder.Services.AddSingleton<IDrawingToolRetriever<IDrawable>>(drawingTools);
        builder.Services.AddSingleton<ICanvasManager<IDrawable>, MauiCanvasManager>();
        return builder;
    }

public static MauiAppBuilder AddViewModels(this MauiAppBuilder builder)
{
    builder.Services.AddSingleton<MauiOverlayWindowViewModel>();
    builder.Services.AddSingleton<MauiDrawingToolWindowViewModel>();
    return builder;
}

public static MauiAppBuilder AddWindowStyling(this MauiAppBuilder builder)
{
    // Unfortunately, WinUI doesn't support background transparency in the way WPF does.
    // It's possible to leverage Win32 to modify the window opacity, however, this also affects the opacity of all child controls.
    // See issue: https://github.com/microsoft/microsoft-ui-xaml/issues/1247

    return builder;
}
}
