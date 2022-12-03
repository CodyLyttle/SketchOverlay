using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Maui.ViewModels;

internal class MauiDrawingToolWindowViewModel : DrawingToolWindowViewModel<MauiDrawing, MauiImageSource, MauiColor>
{
    public MauiDrawingToolWindowViewModel(
        ICanvasProperties<MauiColor> canvasProps,
        IColorPalette<MauiColor> drawingColors,
        IDrawingToolCollection<MauiDrawing, MauiImageSource, MauiColor> drawingTools,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, messenger)
    {
    }
}