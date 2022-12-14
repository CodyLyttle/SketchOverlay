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

    protected override IEllipseTool<WpfDrawing, Brush>? CreateEllipseTool()
    {
        return new WpfEllipseTool();
    }

    protected override ILineTool<WpfDrawing, WpfBrush>? CreateLineTool()
    {
        return new WpfLineTool();
    }

    protected override IPaintBrushTool<WpfDrawing, WpfBrush>? CreatePaintBrushTool()
    {
        return new WpfPaintbrushTool();
    }

    protected override IRectangleTool<WpfDrawing, WpfBrush>? CreateRectangleTool()
    {
        return new WpfRectangleTool();
    }
}