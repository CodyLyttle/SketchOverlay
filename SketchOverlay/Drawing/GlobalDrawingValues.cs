using SketchOverlay.Drawing.Tools;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Drawing;

internal static class GlobalDrawingValues
{
    public const double MinimumDrawingSize = 1;
    public const double DefaultDrawingSize = 2;
    public const double MaximumDrawingSize = 32;

    public static readonly Color[] DrawingColors =
    {
        Colors.Black,
        Colors.DarkRed, 
        Colors.Orange,
        Colors.DarkGreen, 
        Colors.DarkBlue, 
        Colors.DarkViolet,

        Colors.Gray,
        Colors.Red,
        Colors.Gold,
        Colors.ForestGreen,
        Colors.Blue,
        Colors.Violet,

        Colors.White,
        Colors.OrangeRed,
        Colors.Yellow,
        Colors.LawnGreen,
        Colors.DeepSkyBlue,
        Colors.PaleVioletRed,
    };

    public static readonly DrawingToolInfo<IDrawable,ImageSource>[] DrawingTools =
    {
        new(new BrushTool(), ImageSource.FromFile("placeholder_paintbrush.png"), "Paintbrush"),
        new(new LineTool(), ImageSource.FromFile("placeholder_line.png"), "Line"),
        new(new RectangleTool(), ImageSource.FromFile("placeholder_rectangle.png"), "Rectangle")
    };

    public static readonly DrawingToolInfo<IDrawable, ImageSource> DefaultDrawingTool = DrawingTools[0];
    public static readonly Color DefaultDrawingColor = DrawingColors[0];
}
