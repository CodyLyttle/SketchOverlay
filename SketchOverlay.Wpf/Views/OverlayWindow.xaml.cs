using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SketchOverlay.Wpf.ViewModels;

namespace SketchOverlay.Wpf.Views;

public partial class OverlayWindow : Window
{
    public OverlayWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<WpfOverlayWindowViewModel>();
    }
}