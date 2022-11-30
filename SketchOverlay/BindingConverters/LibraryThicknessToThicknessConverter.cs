using System.Globalization;
using SketchOverlay.Library.Models;

namespace SketchOverlay.BindingConverters;

internal class LibraryThicknessToThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not LibraryThickness libraryMargin)
            throw new ArgumentOutOfRangeException(nameof(value), 
                $"{nameof(value)} must be of type {nameof(LibraryThickness)}");

        return new Thickness
        {
            Left = libraryMargin.Left, 
            Top = libraryMargin.Top
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Thickness mauiThickness)
            throw new ArgumentOutOfRangeException(nameof(value),
                $"{nameof(value)} must be of type {nameof(Thickness)}");

        return new LibraryThickness
        {
            Left = mauiThickness.Left,
            Top = mauiThickness.Top,
            Right = mauiThickness.Right,
            Bottom = mauiThickness.Bottom
        };
    }
}