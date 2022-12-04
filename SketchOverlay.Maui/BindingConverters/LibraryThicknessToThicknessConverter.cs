using System.Globalization;
using SketchOverlay.Library;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Maui.BindingConverters;

internal class LibraryThicknessToThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not LibraryThickness libraryMargin)
            throw new ValueConverterTypeException<LibraryThickness>(value);

        return new Thickness
        {
            Left = libraryMargin.Left, 
            Top = libraryMargin.Top
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Thickness mauiThickness)
            throw new ValueConverterTypeException<Thickness>(value);

        return new LibraryThickness
        {
            Left = mauiThickness.Left,
            Top = mauiThickness.Top,
            Right = mauiThickness.Right,
            Bottom = mauiThickness.Bottom
        };
    }
}