using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace SpaceView.Converter;

public class ApodMediaTypeIsVideo : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string mediaType)
        {
            return mediaType == "video";
        }

        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}