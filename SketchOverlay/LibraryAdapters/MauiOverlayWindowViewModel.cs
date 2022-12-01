using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.LibraryAdapters;

// Bypass XAML {x:Type} restriction on generic parameters.
public class MauiOverlayWindowViewModel : OverlayWindowViewModel<IDrawable, IDrawable, ImageSource, Color>
{
    public MauiOverlayWindowViewModel(ICanvasManager<IDrawable> canvasManager, IMessenger messenger) 
        : base(canvasManager, messenger)
    {
    }
}