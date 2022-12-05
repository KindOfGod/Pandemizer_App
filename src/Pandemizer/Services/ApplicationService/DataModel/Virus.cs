namespace Pandemizer.Services.DataModel;

public class Virus
{

    #region Properties
    
    public string Name { get; set; } = "VirusName";
    
    // Base
    public string BaseInfectionRate { get; set; } = "0.000001";
    
    public string RateOfGettingWorse { get; set; } = "0.05";
    
    public string BaseDeathRate { get; set; } = "0.01";
    
    public string BaseImmunityRate { get; set; } = "0.001";
    
    //Endangered
    
    public string? EndangeredAgeGroup { get; set; } = "Pensioner";
    
    public string EndangeredAgeInfectionRate  { get; set; } = "0.000005";

    public string EndangeredAgeRateOfGettingWorse  { get; set; } = "0.1";

    public string EndangeredAgeDeathRate  { get; set; } = "0.05";
    
    public string EndangeredImmunityRate { get; set; } = "0.0005";
    
    //Survival
    
    public string SurvivalInstinctMultiplier { get; set; } = "2";
    
    //Immunity

    public string ImmunityLostRate { get; set; } = "0.01";
    
    public string InfectionSpreadRate { get; set; } = "100000";

    #endregion
}