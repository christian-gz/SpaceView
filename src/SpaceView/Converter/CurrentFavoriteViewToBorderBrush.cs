using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SpaceView.Converter;

public class CurrentFavoriteViewToBorderBrush : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return new SolidColorBrush(Color.Parse("#e9ebe9"));
        }

        string? currentView = value.ToString();
        string? targetView = parameter.ToString();

        if (currentView == targetView)
        {
            return Brushes.Gainsboro;
        }

        return new SolidColorBrush(Color.Parse("#e9ebe9"));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}