using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SketchOverlay.Wpf.BindingConverters;

internal class InvertedBooleanToVisibilityConverter : IValueConverter
{
    private static readonly BooleanToVisibilityConverter BaseConverter = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var visibility = (Visibility)BaseConverter.Convert(value, targetType, parameter, culture);
        return visibility == Visibility.Visible
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return !(bool)BaseConverter.ConvertBack(value, targetType, parameter, culture);
    }
}
