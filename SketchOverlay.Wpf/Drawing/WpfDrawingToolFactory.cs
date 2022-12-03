using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SketchOverlay.Library.Drawing.Tools;
using SketchOverlay.Wpf.Drawing.Tools;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfDrawingToolFactory : DrawingToolFactory<WpfDrawing, WpfImageSource, WpfBrush>
{
    protected override ImageSource CreateImageSource(string fileName)
    {
        BitmapImage image = new();
        image.BeginInit();
        image.UriSource = new Uri(
            Path.Combine("..", "Resources", "Images", "DrawingTools", fileName),
            UriKind.Relative);
       
        image.EndInit();
        return image;
    }

    protected override ILineTool<WpfDrawing, WpfBrush>? CreateLineTool()
    {
        return base.CreateLineTool();
    }

    protected override IPaintBrushTool<WpfDrawing, WpfBrush>? CreatePaintBrushTool()
    {
        return base.CreatePaintBrushTool();
    }

    protected override IRectangleTool<WpfDrawing, WpfBrush>? CreateRectangleTool()
    {
        return base.CreateRectangleTool();
    }
}