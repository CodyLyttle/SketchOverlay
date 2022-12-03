using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SketchOverlay.Library.Drawing.Tools;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfDrawingToolFactory : DrawingToolFactory<System.Windows.Media.Drawing, ImageSource, System.Drawing.Color>
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

    protected override ILineTool<System.Windows.Media.Drawing, System.Drawing.Color>? CreateLineTool()
    {
        return base.CreateLineTool();
    }

    protected override IPaintBrushTool<System.Windows.Media.Drawing, System.Drawing.Color>? CreatePaintBrushTool()
    {
        return base.CreatePaintBrushTool();
    }

    protected override IRectangleTool<System.Windows.Media.Drawing, System.Drawing.Color>? CreateRectangleTool()
    {
        return base.CreateRectangleTool();
    }
}