using System;
using System.Globalization;
using System.Windows.Data;
using SketchOverlay.Library;

namespace SketchOverlay.Wpf.BindingConverters;

internal class InvertedBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
            throw new ValueConverterTypeException<bool>(value);

        return !boolValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
            throw new ValueConverterTypeException<bool>(value);

        return !boolValue;
    }
}