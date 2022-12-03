using System.Windows.Media;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SketchOverlay.Library.Drawing;
using SketchOverlay.Library.Drawing.Canvas;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Wpf.Drawing;
using SketchOverlay.Wpf.ViewModels;
using Color = System.Drawing.Color;

namespace SketchOverlay.Wpf;

public static class DependencyBuilderExtensions
{
    public static ServiceCollection AddServices(this ServiceCollection builder)
    {
        builder.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);

        // Drawing
        builder.AddSingleton<IColorPalette<Color>, DrawingColors>();
        builder.AddSingleton<ICanvasProperties<Color>, CanvasProperties<Color>>();
        builder.AddSingleton<ICanvasManager<DrawingGroup>, WpfCanvasManager>();

        // Tools
        DrawingToolCollection<System.Windows.Media.Drawing, ImageSource, Color> drawingToolCollection =
            new WpfDrawingToolFactory().CreateDrawingToolCollection();

        builder.AddSingleton<IDrawingToolCollection<System.Windows.Media.Drawing, ImageSource, Color>>(drawingToolCollection);
        builder.AddSingleton<IDrawingToolRetriever<System.Windows.Media.Drawing, Color>>(drawingToolCollection);
        return builder;
    }

    public static ServiceCollection AddViewModels(this ServiceCollection builder)
    {
        builder.AddSingleton<WpfOverlayWindowViewModel>();
        builder.AddSingleton<WpfDrawingToolWindowViewModel>();
        return builder;
    }
}