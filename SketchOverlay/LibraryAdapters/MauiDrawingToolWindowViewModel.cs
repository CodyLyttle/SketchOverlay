using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.LibraryAdapters;
 
// Bypass XAML {x:Type} restriction on generic parameters.
internal class MauiDrawingToolWindowViewModel : DrawingToolWindowViewModel<IDrawable, ImageSource>
{
    public MauiDrawingToolWindowViewModel(IDrawingToolCollection<IDrawable, ImageSource> drawingTools, IMessenger messenger)
        : base( drawingTools, messenger)
    {
    }
}