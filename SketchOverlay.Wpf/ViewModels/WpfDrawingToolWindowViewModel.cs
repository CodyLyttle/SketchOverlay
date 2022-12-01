using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Wpf.ViewModels;

internal class WpfDrawingToolWindowViewModel : DrawingToolWindowViewModel<Drawing, ImageSource, Color>
{
    public WpfDrawingToolWindowViewModel(ICanvasProperties<Color> canvasProps,
        IColorPalette<Color> drawingColors,
        IDrawingToolCollection<Drawing, ImageSource, Color> drawingTools,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, messenger)
    {
    }
}