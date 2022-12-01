using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Maui.Drawing;

internal class MauiColorPalette : IColorPalette<Color>
{
    public MauiColorPalette()
    {
        DrawingColors colors = new();
        Colors = colors.Colors.Select(c => c.ToMauiColor());
        DefaultPrimaryColor = colors.DefaultPrimaryColor.ToMauiColor();
        DefaultSecondaryColor = colors.DefaultSecondaryColor.ToMauiColor();
        PrimaryColor = colors.PrimaryColor.ToMauiColor();
        SecondaryColor = colors.SecondaryColor.ToMauiColor();
    }

    public IEnumerable<Color> Colors { get; }
    public Color DefaultPrimaryColor { get; }
    public Color DefaultSecondaryColor { get; }
    public Color PrimaryColor { get; set; }
    public Color SecondaryColor { get; set; }
}