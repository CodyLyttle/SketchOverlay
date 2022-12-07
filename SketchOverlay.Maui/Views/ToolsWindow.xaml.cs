using SketchOverlay.Maui.ViewModels;

namespace SketchOverlay.Maui.Views;

public partial class ToolsWindow
{
    public ToolsWindow()
    {
        InitializeComponent();
        BindingContext = MauiProgram.GetService<MauiToolsWindowViewModel>();
        HeightRequest = 310;
        WidthRequest =300;
    }
}