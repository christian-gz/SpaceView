using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SpaceView.Converter;

public class CurrentViewToBackgroundBrush : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return Brushes.White;
        }

        string? currentView = value.ToString();
        string? targetView = parameter.ToString();

        if (currentView == targetView)
        {
            return new SolidColorBrush(Color.Parse("#e9ebe9"));
        }

        return Brushes.White;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}