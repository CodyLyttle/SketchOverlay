using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Maui.ViewModels;

internal class MauiToolsWindowViewModel : ToolsWindowViewModel<MauiDrawing, MauiImageSource, MauiColor>
{
    public MauiToolsWindowViewModel(
        ICanvasProperties<MauiColor> canvasProps,
        IColorPalette<MauiColor> drawingColors,
        IDrawingToolCollection<MauiDrawing, MauiImageSource, MauiColor> drawingTools,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, messenger)
    {
    }
}