using System.Collections.Generic;
using System.Linq;
using SketchOverlay.Library.Drawing;

namespace SketchOverlay.Wpf.Drawing;

internal class WpfColorPalette : IColorPalette<WpfBrush>
{
    public WpfColorPalette()
    {
        DrawingColors libraryColors = new();
        Colors = libraryColors.Colors.Select(c => c.ToWindowsColor().ToSolidColorBrush());
        DefaultPrimaryColor = libraryColors.DefaultPrimaryColor.ToWindowsColor().ToSolidColorBrush();
        DefaultSecondaryColor = libraryColors.DefaultSecondaryColor.ToWindowsColor().ToSolidColorBrush();
        PrimaryColor = libraryColors.PrimaryColor.ToWindowsColor().ToSolidColorBrush();
        SecondaryColor = libraryColors.SecondaryColor.ToWindowsColor().ToSolidColorBrush();
    }

    public IEnumerable<WpfBrush> Colors { get; }
    public WpfBrush DefaultPrimaryColor { get; }
    public WpfBrush DefaultSecondaryColor { get; }
    public WpfBrush PrimaryColor { get; set; }
    public WpfBrush SecondaryColor { get; set; }
}