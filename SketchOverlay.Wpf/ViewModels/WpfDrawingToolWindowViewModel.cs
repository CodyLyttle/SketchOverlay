using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Wpf.ViewModels;

internal class WpfDrawingToolWindowViewModel : DrawingToolWindowViewModel<System.Windows.Media.Drawing, ImageSource, Color>
{
    public WpfDrawingToolWindowViewModel(ICanvasProperties<Color> canvasProps,
        IColorPalette<Color> drawingColors,
        IDrawingToolCollection<System.Windows.Media.Drawing, ImageSource, Color> drawingTools,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, messenger)
    {
    }
}