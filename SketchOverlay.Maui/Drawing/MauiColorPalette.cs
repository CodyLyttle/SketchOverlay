using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Maui.Drawing;

internal class MauiColorPalette : IColorPalette<MauiColor>
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

    public IEnumerable<MauiColor> Colors { get; }
    public MauiColor DefaultPrimaryColor { get; }
    public MauiColor DefaultSecondaryColor { get; }
    public MauiColor PrimaryColor { get; set; }
    public MauiColor SecondaryColor { get; set; }
}