using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.LibraryAdapters;

// Bypass XAML {x:Type} restriction on generic parameters.
internal class MauiOverlayWindowViewModel : OverlayWindowViewModel<IDrawable, ImageSource>
{
    public MauiOverlayWindowViewModel(IDrawingCanvas<IDrawable> canvas, IMessenger messenger) : base(canvas, messenger)
    {
    }
}