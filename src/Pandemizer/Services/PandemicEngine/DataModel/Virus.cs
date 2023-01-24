namespace Pandemizer.Services.PandemicEngine.DataModel;

/// <summary>
/// Contains all probabilities and factors for a virus.
/// </summary>
public class Virus
{
    public string Name { get; set; } = "Virus";
    
    public double BaseInfectionRate { get; set; } = 0.000_001; // can't be greater than 1
    public double EndangeredAgeInfectionRate  { get; set; } = 0.000_005; //can't be greater than 1
    public Age? EndangeredAgeGroup { get; set; } = Age.Pensioner; // can be null
    
    //multiplier of infection spread rate if everybody is infected
    public double InfectionSpreadRate { get; set; } = 100_000; //0 -> no influence if more people infected, x > 0 -> x-times as infective
        
    //infection kind
    public StateOfLife InfectionSeverity { get; set; } = StateOfLife.ImperceptiblyInfected;
    public double RateOfGettingWorse { get; set; } = 0.05; // can't be greater than 1
    public double EndangeredAgeRateOfGettingWorse  { get; set; } = 0.1; //can't be greater than 1
        
    //rate of death
    public double BaseDeathRate { get; set; } = 0.01; // can't be greater than 1
    public double EndangeredAgeDeathRate  { get; set; } = 0.05; //can't be greater than 1
        
    //rate of immunity
    public double BaseImmunityRate { get; set; } = 0.001; // can't be greater than 1
    public double EndangeredImmunityRate { get; set; } = 0.000_5; // can't be greater than 1
    public double SurvivalInstinctMultiplier { get; set; } = 2; // multiplies immunity rate in life/death situation
        
    //rate of losing immunity
    public double ImmunityLostRate { get; set; } = 0.01; // can't be greater than 1
    
    //Pre-existing condition modifier
    public double PreConditionModifier { get; set; }  = 1.5; // works in both directions, 1 > bad for pop, 1 < good for pop
    
    //Hospital modifier
    public double HospitalizedModifier { get; set; } = 10; // multiplies positive effects, gets worse as closer HospitalCap is fulfilled
}