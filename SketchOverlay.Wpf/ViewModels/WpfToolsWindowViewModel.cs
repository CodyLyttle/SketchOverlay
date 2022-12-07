using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Wpf.ViewModels;

internal class WpfToolsWindowViewModel : ToolsWindowViewModel<WpfDrawing, WpfImageSource, WpfBrush>
{
    public WpfToolsWindowViewModel(ICanvasProperties<WpfBrush> canvasProps,
        IColorPalette<WpfBrush> drawingColors,
        IDrawingToolCollection<WpfDrawing, WpfImageSource, WpfBrush> drawingTools,
        ICanvasStateManager canvasStateManager,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, canvasStateManager, messenger)
    {
    }
}