using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Wpf.ViewModels;

internal class WpfDrawingToolWindowViewModel : DrawingToolWindowViewModel<WpfDrawing, WpfImageSource, WpfBrush>
{
    public WpfDrawingToolWindowViewModel(ICanvasProperties<WpfBrush> canvasProps,
        IColorPalette<WpfBrush> drawingColors,
        IDrawingToolCollection<WpfDrawing, WpfImageSource, WpfBrush> drawingTools,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, messenger)
    {
    }
}