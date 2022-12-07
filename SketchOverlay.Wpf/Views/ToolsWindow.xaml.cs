using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SketchOverlay.Wpf.ViewModels;

namespace SketchOverlay.Wpf.Views;

public partial class ToolsWindow : UserControl
{
    public ToolsWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<WpfToolsWindowViewModel>();
        Width = 260;
        Height = 310;
    }
}