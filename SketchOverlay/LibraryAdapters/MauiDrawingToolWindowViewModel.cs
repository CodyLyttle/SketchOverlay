using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.LibraryAdapters;
 
// Bypass XAML {x:Type} restriction on generic parameters.
internal class MauiDrawingToolWindowViewModel : DrawingToolWindowViewModel<IDrawable, ImageSource>
{
    public MauiDrawingToolWindowViewModel(IMessenger messenger) : base(messenger)
    {
    }
}