using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace SpaceView.Converter;

public class StringToFormattedNumber : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
            {
                return number.ToString("F5", CultureInfo.InvariantCulture);
            }
        }

        return BindingOperations.DoNothing;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}