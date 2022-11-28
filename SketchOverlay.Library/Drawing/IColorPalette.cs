using System.Drawing;

namespace SketchOverlay.Library.Drawing;

public interface IColorPalette
{
    public IEnumerable<Color> Colors { get; }
    public Color DefaultPrimaryColor { get; }
    public Color DefaultSecondaryColor { get; }
    public Color PrimaryColor { get; set; }
    public Color SecondaryColor { get; set; }
}