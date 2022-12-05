using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SketchOverlay.Wpf.ViewModels;

namespace SketchOverlay.Wpf.Views;

public partial class DrawingToolWindow : UserControl
{
    public DrawingToolWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<WpfDrawingToolWindowViewModel>();
        Width = 260;
        Height = 310;
    }
}