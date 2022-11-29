using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.Views;

public partial class DrawingToolWindow
{
    public DrawingToolWindow()
    {
        InitializeComponent();
        BindingContext = MauiProgram.GetService<MauiDrawingToolWindowViewModel>();
    }
}