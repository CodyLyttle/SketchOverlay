using SketchOverlay.Maui.ViewModels;

namespace SketchOverlay.Maui.Views;

public partial class DrawingToolWindow
{
    public DrawingToolWindow()
    {
        InitializeComponent();
        BindingContext = MauiProgram.GetService<MauiDrawingToolWindowViewModel>();
        HeightRequest = 310;
        WidthRequest =300;
    }
}