namespace Pandemizer.Services.PandemicEngine.DataModel;

/// <summary>
/// Represents a Pop in decoded state
/// </summary>
public class Pop
{
    #region Properties
    public uint ID { get; set; }
    public uint Count { get; set; }
    
    public StateOfLife StateOfLife { get; set; }
    
    public Age Age { get; set; }
    
    public bool PreExistingCondition { get; set; }
    
    public bool IsHospitalized { get; set; }

    #endregion

    #region Constructors

    public Pop(uint key, uint count)
    {
        ID = key;
        Count = count;
        StateOfLife = key.GetStateOfLive();
        Age = key.GetAge();
        PreExistingCondition = key.GetPreExistingCondition().ToString() == "True";
        IsHospitalized = key.GetIsHospitalized().ToString() == "True";
    }

    #endregion
}