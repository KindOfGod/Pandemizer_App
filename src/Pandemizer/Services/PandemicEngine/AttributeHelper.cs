namespace Pandemizer.Services.PandemicEngine;

/// <summary>
/// Helps to check and override Enums in Pop
/// </summary>
public static class AttributeHelper
{
    //StateOfLive
    public static bool CheckStateOfLive(uint value, StateOfLife check)
    {
        return (value & (uint)StateOfLife.Compare) == (uint)check;
    }
    
    public static uint OverrideStateOfLive(uint value, StateOfLife check)
    {
        return (value & ~(uint)StateOfLife.Compare) + (uint)check;
    }

    public static StateOfLife GetStateOfLive(uint value)
    {
        return (StateOfLife)(value & (uint) StateOfLife.Compare);
    }
    
    //Age
    public static bool CheckAge(uint value, Age check)
    {
        return (value & (uint)Age.Compare) == (uint)check;
    }
    
    public static uint OverrideAge(uint value, Age check)
    {
        return (value & ~(uint)Age.Compare) + (uint)check;
    }

    public static Age GetAge(uint value)
    {
        return (Age)(value & (uint) Age.Compare);
    }
    
    //PreExistingCondition
    public static bool CheckPreExistingCondition(uint value, PreExistingCondition check)
    {
        return (value & (uint)PreExistingCondition.Compare) == (uint)check;
    }
    
    public static uint OverridePreExistingCondition(uint value, PreExistingCondition check)
    {
        return (value & ~(uint)PreExistingCondition.Compare) + (uint)check;
    }
    public static PreExistingCondition GetPreExistingCondition(uint value)
    {
        return (PreExistingCondition)(value & (uint) PreExistingCondition.Compare);
    }
}