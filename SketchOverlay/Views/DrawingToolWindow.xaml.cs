using SketchOverlay.ViewModels;

namespace SketchOverlay.Views;

public partial class DrawingToolWindow
{
    public DrawingToolWindow()
    {
        InitializeComponent();
        BindingContext = MauiProgram.GetService<MauiDrawingToolWindowViewModel>();
    }
}