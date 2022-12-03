using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Wpf.Drawing;
using SketchOverlay.Wpf.ViewModels;

namespace SketchOverlay.Wpf;

public static class DependencyBuilderExtensions
{
    public static ServiceCollection AddServices(this ServiceCollection builder)
    {
        builder.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        // Drawing
        builder.AddSingleton<IColorPalette<WpfBrush>, WpfColorPalette>();
        builder.AddSingleton<ICanvasProperties<WpfBrush>, CanvasProperties<WpfBrush>>();
        builder.AddSingleton<ICanvasManager<WpfDrawingOutput>, WpfCanvasManager>();

        // Tools
        DrawingToolCollection<WpfDrawing, WpfImageSource, WpfBrush> drawingToolCollection =
            new WpfDrawingToolFactory().CreateDrawingToolCollection();

        builder.AddSingleton<IDrawingToolCollection<WpfDrawing, WpfImageSource, WpfBrush>>(drawingToolCollection);
        builder.AddSingleton<IDrawingToolRetriever<WpfDrawing, WpfBrush>>(drawingToolCollection);
        return builder;
    }

    public static ServiceCollection AddViewModels(this ServiceCollection builder)
    {
        builder.AddSingleton<WpfOverlayWindowViewModel>();
        builder.AddSingleton<WpfDrawingToolWindowViewModel>();
        return builder;
    }
}