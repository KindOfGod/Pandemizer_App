using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using SkiaSharp;

namespace Pandemizer.Core.Converters;

public class PercentageColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double dValue) 
            return new SolidColorBrush(new Color(255,255,255 ,255));
        
        if(dValue == 0)
            return new SolidColorBrush(new Color(255,255,255 ,255));
        
        return dValue > 0 ? new SolidColorBrush(new Color(255,200, 97, 97)) : new SolidColorBrush(new Color(255,59, 160, 132));
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new Exception();
    }
}