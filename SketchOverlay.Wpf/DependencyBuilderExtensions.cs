using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Wpf.Drawing;
using SketchOverlay.Wpf.Drawing.Tools;
using SketchOverlay.Wpf.ViewModels;

namespace SketchOverlay.Wpf;

public static class DependencyBuilderExtensions
{
    public static ServiceCollection AddServices(this ServiceCollection builder)
    {
        builder.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        // Drawing
        builder.AddSingleton<IColorPalette<WpfBrush>, WpfColorPalette>();
        builder.AddSingleton<ICanvasProperties<WpfBrush>, WpfCanvasProperties>();
        builder.AddSingleton<ICanvasManager<WpfDrawingOutput>, WpfCanvasManager>();
        builder.AddSingleton<IDrawingStack<WpfDrawing, WpfDrawingOutput>, DrawingStack>();

        // Tools
        IDrawingToolCollection<WpfDrawing, WpfImageSource, WpfBrush> drawingToolCollection =
            new WpfDrawingToolFactory().CreateDrawingToolCollection();

        builder.AddSingleton(drawingToolCollection);
        builder.AddSingleton<IDrawingToolRetriever<WpfDrawing, WpfBrush>>(drawingToolCollection);
        builder.AddSingleton<ILineTool<WpfDrawing, WpfBrush>>(drawingToolCollection.GetTool<WpfLineTool>());
        builder.AddSingleton<IPaintBrushTool<WpfDrawing, WpfBrush>>(drawingToolCollection.GetTool<WpfPaintbrushTool>());
        builder.AddSingleton<IRectangleTool<WpfDrawing, WpfBrush>>(drawingToolCollection.GetTool<WpfRectangleTool>());
        return builder;
    }

    public static ServiceCollection AddViewModels(this ServiceCollection builder)
    {
        builder.AddSingleton<WpfOverlayWindowViewModel>();
        builder.AddSingleton<WpfToolsWindowViewModel>();
        return builder;
    }
}