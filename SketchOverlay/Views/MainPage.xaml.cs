using SketchOverlay.Drawing.Canvas;
using SketchOverlay.ViewModels;

namespace SketchOverlay.Views;

public partial class MainPage : ContentPage
{
    public MainPage(OverlayWindowViewModel viewModel, IDrawingCanvas canvas)
    {
        InitializeComponent();
        BindingContext = viewModel;
        graphicsView.Drawable = canvas;
        canvas.RequestRedraw += (_, _) => graphicsView.Invalidate();
    }
}