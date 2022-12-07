using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Maui.ViewModels;

public class MauiOverlayWindowViewModel : OverlayWindowViewModel<MauiDrawing, MauiDrawingOutput, MauiImageSource, MauiColor>
{
    public MauiOverlayWindowViewModel(ICanvasDrawingManager<MauiDrawing> canvasManager, IMessenger messenger) 
        : base(canvasManager, messenger)
    {
    }
}