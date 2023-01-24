namespace Pandemizer.Services.PandemicEngine;

/// <summary>
/// Helps to check and override Enums in Pop
/// </summary>
public static class AttributeHelper
{
    //StateOfLive
    public static bool CheckStateOfLive(this uint value, StateOfLife check)
    {
        return (value & (uint)StateOfLife.Compare) == (uint)check;
    }
    
    public static uint OverrideStateOfLive(this uint value, StateOfLife check)
    {
        return (value & ~(uint)StateOfLife.Compare) + (uint)check;
    }

    public static StateOfLife GetStateOfLive(this uint value)
    {
        return (StateOfLife)(value & (uint) StateOfLife.Compare);
    }
    
    //Age
    public static bool CheckAge(this uint value, Age check)
    {
        return (value & (uint)Age.Compare) == (uint)check;
    }
    
    public static uint OverrideAge(this uint value, Age check)
    {
        return (value & ~(uint)Age.Compare) + (uint)check;
    }

    public static Age GetAge(this uint value)
    {
        return (Age)(value & (uint) Age.Compare);
    }
    
    //PreExistingCondition
    public static bool CheckPreExistingCondition(this uint value, PreExistingCondition check)
    {
        return (value & (uint)PreExistingCondition.Compare) == (uint)check;
    }
    
    public static uint OverridePreExistingCondition(this uint value, PreExistingCondition check)
    {
        return (value & ~(uint)PreExistingCondition.Compare) + (uint)check;
    }
    public static PreExistingCondition GetPreExistingCondition(this uint value)
    {
        return (PreExistingCondition)(value & (uint) PreExistingCondition.Compare);
    }
    
    //IsHospitalized
    public static bool CheckIsHospitalized(this uint value, IsHospitalized check)
    {
        return (value & (uint)IsHospitalized.Compare) == (uint)check;
    }
    
    public static uint OverrideIsHospitalized(this uint value, IsHospitalized check)
    {
        return (value & ~(uint)IsHospitalized.Compare) + (uint)check;
    }
    public static IsHospitalized GetIsHospitalized(this uint value)
    {
        return (IsHospitalized)(value & (uint) IsHospitalized.Compare);
    }
}