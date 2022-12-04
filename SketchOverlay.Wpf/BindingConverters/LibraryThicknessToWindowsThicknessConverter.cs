using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SketchOverlay.Library;
using SketchOverlay.Library.Models;

namespace SketchOverlay.Wpf.BindingConverters;

internal class LibraryThicknessToWindowsThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not LibraryThickness libraryThickness)
            throw new ValueConverterTypeException<LibraryThickness>(value);

        return new Thickness(
            libraryThickness.Left, 
            libraryThickness.Top, 
            libraryThickness.Right,
            libraryThickness.Bottom);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Thickness windowsThickness)
            throw new ValueConverterTypeException<Thickness>(value);

        return new LibraryThickness(
            windowsThickness.Left,
            windowsThickness.Top,
            windowsThickness.Right,
            windowsThickness.Bottom);
    }
}