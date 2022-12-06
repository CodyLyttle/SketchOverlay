using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Maui.Drawing;
using SketchOverlay.Maui.Drawing.Tools;
using SketchOverlay.Maui.ViewModels;
using SketchOverlay.Maui.Views;

namespace SketchOverlay.Maui;

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
        builder.Services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        // Drawing
        builder.Services.AddSingleton<IColorPalette<MauiColor>, MauiColorPalette>();
        builder.Services.AddSingleton<ICanvasProperties<MauiColor>, MauiCanvasProperties>();
        builder.Services.AddSingleton<ICanvasManager<MauiDrawing>, MauiCanvasManager>();
        builder.Services.AddSingleton<IDrawingStack<MauiDrawing, MauiDrawingOutput>, DrawableStack>();

        // Tools
        IDrawingToolCollection<MauiDrawing, MauiImageSource, MauiColor> drawingToolCollection = 
            new MauiDrawingToolFactory().CreateDrawingToolCollection();

        builder.Services.AddSingleton(drawingToolCollection);
        builder.Services.AddSingleton<IDrawingToolRetriever<MauiDrawing, MauiColor>>(drawingToolCollection);
        builder.Services.AddSingleton<IPaintBrushTool<MauiDrawing, MauiColor>>(drawingToolCollection.GetTool<MauiPaintBrushTool>());
        builder.Services.AddSingleton<ILineTool<MauiDrawing, MauiColor>>(drawingToolCollection.GetTool<MauiLineTool>());
        builder.Services.AddSingleton<IRectangleTool<MauiDrawing, MauiColor>>(drawingToolCollection.GetTool<MauiRectangleTool>());
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
