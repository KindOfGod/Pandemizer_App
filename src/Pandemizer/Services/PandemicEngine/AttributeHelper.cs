namespace Pandemizer.Services.PandemicEngine;

public static class AttributeHelper
{
    public static bool CheckStateOfLive(uint value, StateOfLife check)
    {
        return (value & (uint)StateOfLife.Compare) == (uint)check;
    }
    
    public static uint OverrideStateOfLive(uint value, StateOfLife check)
    {
        return (value & ~(uint)StateOfLife.Compare) + (uint)check;
    }
    
    public static bool CheckAge(uint value, Age check)
    {
        return (value & (uint)Age.Compare) == (uint)check;
    }
    
    public static uint OverrideAge(uint value, Age check)
    {
        return (value & ~(uint)Age.Compare) + (uint)check;
    }
}