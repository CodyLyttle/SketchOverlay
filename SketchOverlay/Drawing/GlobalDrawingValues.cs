using SketchOverlay.Drawing.Tools;
using SketchOverlay.Models;

namespace SketchOverlay.Drawing;

internal static class GlobalDrawingValues
{
    public const double MinimumDrawingSize = 1;
    public const double DefaultDrawingSize = 2;
    public const double MaximumDrawingSize = 32;

    public static readonly Color[] DrawingColors =
    {
        Colors.Red, 
        Colors.Green, 
        Colors.Blue, 
        Colors.Magenta, 
        Colors.Yellow
    };

    public static readonly DrawingToolInfo[] DrawingTools =
    {
        new(new BrushTool(), ImageSource.FromFile("placeholder_paintbrush.png"), "Paintbrush"),
        new(new LineTool(), ImageSource.FromFile("placeholder_line.png"), "Line"),
        new(new RectangleTool(), ImageSource.FromFile("placeholder_rectangle.png"), "Rectangle")
    };

    public static readonly DrawingToolInfo DefaultDrawingTool = DrawingTools[0];
    public static readonly Color DefaultDrawingColor = DrawingColors[0];
}
