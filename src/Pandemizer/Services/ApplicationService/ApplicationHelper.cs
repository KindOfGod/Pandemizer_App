using System.Globalization;

namespace Pandemizer.Services;

public static class ApplicationHelper
{
    private static readonly NumberFormatInfo _numberFormat = new() { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };
    
    /// <summary>
    /// Returns Int as Formated string. 1000 -> 1.000
    /// </summary>
    public static string IntToFormatedNum(int i)
    {
        return i.ToString("#,##0", _numberFormat);
    }
}