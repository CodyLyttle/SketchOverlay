using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.ViewModels;

namespace SketchOverlay.Views;

public partial class MainPage : ContentPage
{
    public MainPage(OverlayWindowViewModel<IDrawable,ImageSource> viewModel, IDrawingCanvas<IDrawable> canvas)
    {
        InitializeComponent();
        BindingContext = viewModel;

        if (canvas is not IDrawable drawableCanvas)
            throw new ArgumentOutOfRangeException(nameof(canvas),
                $"{nameof(canvas)} argument was not of type {nameof(IDrawable)}");

        graphicsView.Drawable = drawableCanvas;
        canvas.RequestRedraw += (_, _) => graphicsView.Invalidate();
    }
}