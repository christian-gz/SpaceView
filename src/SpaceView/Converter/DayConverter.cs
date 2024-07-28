using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace SpaceView.Converter;

public class DayConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int days)
        {
            return days == 1 ? $"{days} day" : $"{days} days";
        }

        return BindingOperations.DoNothing;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
