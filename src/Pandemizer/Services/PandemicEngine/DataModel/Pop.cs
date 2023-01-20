namespace Pandemizer.Services.PandemicEngine.DataModel;

/// <summary>
/// Represents a Pop in decoded state
/// </summary>
public class Pop
{
    #region Properties
    public uint Count { get; set; }
    
    public StateOfLife StateOfLife { get; set; }
    
    public Age Age { get; set; }
    
    public bool PreExistingCondition { get; set; }

    #endregion

    #region Constructors

    public Pop(uint key, uint count)
    {
        Count = count;
        StateOfLife = AttributeHelper.GetStateOfLive(key);
        Age = AttributeHelper.GetAge(key);
        PreExistingCondition = AttributeHelper.GetPreExistingCondition(key).ToString() == "True";
    }

    #endregion
}