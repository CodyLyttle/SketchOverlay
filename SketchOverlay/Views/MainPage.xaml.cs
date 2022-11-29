using SketchOverlay.Library.Drawing;
using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MauiOverlayWindowViewModel viewModel, ICanvasManager<IDrawable> canvasManager)
    {
        InitializeComponent();
        BindingContext = viewModel;
        graphicsView.Drawable = canvasManager.DrawingOutput;
        canvasManager.RequestRedraw += (_, _) => graphicsView.Invalidate();
    }
}