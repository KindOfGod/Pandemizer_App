using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Pandemizer.Core.Converters;

public class FormattedPercentageConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double dValue) 
            return "";

        return dValue switch
        {
            0 => "0%",
            double.NaN => "0%",
            > 1000 => ">1000%",
            < -1000 => "<-1000%",
            _ => $"{dValue}%"
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new Exception();
    }
}