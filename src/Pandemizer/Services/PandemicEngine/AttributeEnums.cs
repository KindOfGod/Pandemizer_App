// ReSharper disable ShiftExpressionZeroLeftOperand
namespace Pandemizer.Services.PandemicEngine
{
    /// <summary>
    /// All states are saved in 32bit ulong.
    /// Least significant bit is first bit.
    /// </summary>
    ///
    /// To override a enum in uint: 
    /// (store & ~Compare) + newValue;
    ///
    /// To check for enum value
    /// store & Compare == Value
    
    
    // Bit 1-3 (3bit)
    public enum StateOfLife : uint
    {
        Healthy = 1,
        ImperceptiblyInfected = 2,
        Infected = 3,
        HeavilyInfected = 4,
        Dead = 5,
        Immune = 6,
        
        Compare = 7
    }

    // Bit 4-6 (3bit)
    public enum Age : uint
    {
        Child = 1 << 3,
        YoungAdult = 2 << 3,
        Adult = 3 << 3,
        Pensioner = 4 << 3,
        
        Compare = 7 << 3
    }
    
    // Bit 7-8 (2bit)
    public enum PreExistingCondition : uint
    {
        False = 1 << 6,
        True = 2 << 6,
        
        Compare = 3 << 6
    }
    
    // Bit 9-10 (2bit)
    public enum IsHospitalized : uint
    {
        False = 1 << 8,
        True = 2 << 8,
        
        Compare = 3 << 8
    }
}