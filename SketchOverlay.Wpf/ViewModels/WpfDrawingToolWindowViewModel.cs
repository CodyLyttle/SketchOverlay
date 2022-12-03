using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Wpf.ViewModels;

internal class WpfDrawingToolWindowViewModel : DrawingToolWindowViewModel<WpfDrawing, WpfImageSource, WpfColor>
{
    public WpfDrawingToolWindowViewModel(ICanvasProperties<WpfColor> canvasProps,
        IColorPalette<WpfColor> drawingColors,
        IDrawingToolCollection<WpfDrawing, WpfImageSource, WpfColor> drawingTools,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, messenger)
    {
    }
}