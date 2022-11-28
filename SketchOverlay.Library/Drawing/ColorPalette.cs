using System.Drawing;

namespace SketchOverlay.Library.Drawing;

public class ColorPalette : IColorPalette
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

    public ColorPalette()
    {
        PrimaryColor = DefaultPrimaryColor;
        SecondaryColor = DefaultSecondaryColor;
    }

    public ColorPalette(Color primaryColor, Color secondaryColor)
    {
        PrimaryColor = primaryColor;
        SecondaryColor = secondaryColor;
    }

    public IEnumerable<Color> Colors => DarkColors
        .Concat(MediumColors)
        .Concat(LightColors);

    public Color DefaultPrimaryColor => DarkColors[0];
    
    public Color DefaultSecondaryColor => LightColors[0];
    
    public Color PrimaryColor { get; set; }
    
    public Color SecondaryColor { get; set; }
}