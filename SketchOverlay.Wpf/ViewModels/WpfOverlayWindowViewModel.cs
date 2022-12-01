using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Wpf.ViewModels;

internal class WpfOverlayWindowViewModel : OverlayWindowViewModel<Drawing, DrawingGroup, ImageSource, Color>
{
    public WpfOverlayWindowViewModel(ICanvasManager<DrawingGroup> canvasManager, IMessenger messenger) 
        : base(canvasManager, messenger)
    {
    }
}