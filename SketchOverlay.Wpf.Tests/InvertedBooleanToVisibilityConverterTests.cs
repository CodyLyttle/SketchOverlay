﻿using System.Globalization;
using System.Windows;
using SketchOverlay.Wpf.BindingConverters;

namespace SketchOverlay.Wpf.Tests;

public class InvertedBooleanToVisibilityConverterTests
{
    private readonly InvertedBooleanToVisibilityConverter _sut;

    public InvertedBooleanToVisibilityConverterTests()
    {
        _sut = new InvertedBooleanToVisibilityConverter();
    }

    [Theory]
    [InlineData(null, Visibility.Visible)]
    [InlineData(false, Visibility.Visible)]
    [InlineData(true, Visibility.Collapsed)]
    public void Convert_ConvertsToExpectedValue(bool? isVisible, Visibility expected)
    {
        // Act
        var actual = (Visibility)_sut.Convert(isVisible!, typeof(bool), null!, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(Visibility.Visible, false)]
    [InlineData(Visibility.Hidden, true)]
    [InlineData(Visibility.Collapsed, true)]
    public void ConvertBack_ConvertsToExpectedValue(Visibility visibility, bool expected)
    {
        // Act
        var actual = (bool)_sut.ConvertBack(visibility, typeof(Visibility), null!, CultureInfo.CurrentCulture);

        // Assert
        Assert.Equal(expected, actual);
    }
}