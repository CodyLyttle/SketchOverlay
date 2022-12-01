using System.Drawing;

namespace SketchOverlay.Library.Drawing;

public interface IColorPalette<TColor>
{
    public IEnumerable<TColor> Colors { get; }
    public TColor DefaultPrimaryColor { get; }
    public TColor DefaultSecondaryColor { get; }
    public TColor PrimaryColor { get; set; }
    public TColor SecondaryColor { get; set; }
}