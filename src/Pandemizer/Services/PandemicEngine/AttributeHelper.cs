namespace Pandemizer.Services.PandemicEngine;

/// <summary>
/// Helps to check and override Enums in Pop
/// </summary>
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
    
    public static bool CheckPreExistingCondition(uint value, PreExistingCondition check)
    {
        return (value & (uint)PreExistingCondition.Compare) == (uint)check;
    }
    
    public static uint OverridePreExistingCondition(uint value, PreExistingCondition check)
    {
        return (value & ~(uint)PreExistingCondition.Compare) + (uint)check;
    }
}