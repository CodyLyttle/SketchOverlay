using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Wpf.ViewModels;

namespace SketchOverlay.Wpf.Views;

public partial class OverlayWindow : Window
{
    public OverlayWindow()
    {
        InitializeComponent();
        DataContext = App.Current.Services.GetService<WpfOverlayWindowViewModel>();

        var canvasManager = App.Current.Services.GetService<ICanvasManager<WpfDrawingOutput>>();
        canvasManager!.RequestRedraw += (_, _) => drawingCanvas.Redraw();
        drawingCanvas.AddDrawingElement(canvasManager.DrawingOutput);
    }
}