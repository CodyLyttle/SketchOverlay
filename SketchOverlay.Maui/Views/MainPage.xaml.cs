using SketchOverlay.Library.Drawing;
using SketchOverlay.Maui.ViewModels;

namespace SketchOverlay.Maui.Views;

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