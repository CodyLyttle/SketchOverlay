using System.Globalization;
using SketchOverlay.LibraryAdapters;

namespace SketchOverlay.BindingConverters;

internal class DrawingColorToMauiColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not System.Drawing.Color drawingColor)
            throw new ValueConverterTypeException<System.Drawing.Color>(value);

        return drawingColor.ToMauiColor();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Color mauiColor)
            throw new ValueConverterTypeException<Color>(value);

        return mauiColor.ToDrawingColor();
    }
}