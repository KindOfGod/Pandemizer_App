using System;
using System.Globalization;

namespace Pandemizer.Services;

public static class ApplicationHelper
{
    private static readonly NumberFormatInfo _numberFormat = new() { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };
    
    /// <summary>
    /// Returns Int as formatted string. 1000 -> 1.000
    /// </summary>
    public static string? IntToFormattedNum(int i)
    {
        return i.ToString("#,##0", _numberFormat);
    }
    
    /// <summary>
    /// Returns Double as formatted string. 1000 -> 1.000
    /// </summary>
    public static string? DoubleToFormattedNum(double d)
    {
        return d.ToString("#,##0", _numberFormat);
    }
}