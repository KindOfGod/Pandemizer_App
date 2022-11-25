using SkiaSharp;

namespace Pandemizer.Services;

public static class ApplicationColors
{
    public static SKColor IterationColor = new SKColor(153, 9, 63);
    
    public static SKColor HealthyColor = new SKColor(59, 160, 132);
    public static SKColor InfectedColor = new SKColor(229, 226, 127);
    public static SKColor HeavilyInfectedColor = new SKColor(194, 141, 87);
    public static SKColor VaccinatedColor = new SKColor(119, 195, 229);
    public static SKColor DeadColor = new SKColor(127, 103, 67);
}