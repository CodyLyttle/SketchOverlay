using System.Drawing;

namespace SketchOverlay.Library.Drawing;

public class DrawingColors : IColorPalette<Color>
{
    private static readonly Color[] DarkColors =
    {
        Color.Black,
        Color.DarkRed,
        Color.Orange,
        Color.DarkGreen,
        Color.DarkBlue,
        Color.DarkViolet
    };

    private static readonly Color[] MediumColors =
    {
        Color.Gray,
        Color.Red,
        Color.Gold,
        Color.ForestGreen,
        Color.Blue,
        Color.Violet
    };

    private static readonly Color[] LightColors =
    {
        Color.White,
        Color.OrangeRed,
        Color.Yellow,
        Color.LawnGreen,
        Color.DeepSkyBlue,
        Color.PaleVioletRed
    };

    public DrawingColors()
    {
        PrimaryColor = DefaultPrimaryColor;
        SecondaryColor = DefaultSecondaryColor;
    }

    public IEnumerable<Color> Colors => DarkColors
        .Concat(MediumColors)
        .Concat(LightColors);

    public Color DefaultPrimaryColor => DarkColors[1];

    public Color DefaultSecondaryColor => Color.Transparent;
    
    public Color PrimaryColor { get; set; }
    
    public Color SecondaryColor { get; set; }
}