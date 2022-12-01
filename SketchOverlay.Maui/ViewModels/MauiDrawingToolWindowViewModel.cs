using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Maui.ViewModels;

internal class MauiDrawingToolWindowViewModel : DrawingToolWindowViewModel<IDrawable, ImageSource, Color>
{
    public MauiDrawingToolWindowViewModel(
        ICanvasProperties<Color> canvasProps,
        IColorPalette<Color> drawingColors,
        IDrawingToolCollection<IDrawable, ImageSource, Color> drawingTools,
        IMessenger messenger) : base(canvasProps, drawingColors, drawingTools, messenger)
    {
    }
}