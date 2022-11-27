using SkiaSharp;

namespace Pandemizer.Services;

public static class ApplicationColors
{
    public static SKColor IterationColor = new(153, 9, 63);
    
    public static SKColor HealthyColor = new(59, 160, 132);
    
    public static SKColor InfectedColor = new(229, 226, 127);
    
    public static SKColor HeavilyInfectedColor = new(194, 141, 87);
    
    public static SKColor ImmuneColor = new(119, 195, 229);
    public static SKColor ImmuneFillColor = new(119, 195, 229, 60);
    
    public static SKColor DeadColor = new(127, 103, 67);
}