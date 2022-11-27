namespace Pandemizer.Services.PandemicEngine.DataModel
{
    /// <summary>
    /// Contains all dynamic Properties of a Simulation. Used to initialize a Sim.
    /// All Properties can be changed by the User and they have to contain a default value.
    /// </summary>
    public class SimSettings
    {
        #region People Configuration
        
        // General
        public int Scope { get; set; } = 100_000_000;
        public int IterationLimit { get; set; } = 1_000;

        public double ProbabilityDeviation { get; set; } = 0.1;

        // Health Distribution
        public double InitialProportionOfInfected { get; set; } //evenly distributed between age groups
        public StateOfLife HealthIllnessSeverity { get; set; } = StateOfLife.Infected; // can't be Healthy or Dead
        
        // Age Distribution --> has to be 1 in total
        public double AgeProportionOfChildren { get; set; } = 0.15; 
        public double AgeProportionOfYoungAdults { get; set; } = 0.1;
        public double AgeProportionOfAdults { get; set; } = 0.45;
        public double AgeProportionOfPensioner { get; set; } = 0.3;

        #endregion

        #region Virus Configuration

        //infection rates
        public double BaseInfectionRate { get; set; } = 0.000001; // can't be greater than 1
        public double EndangeredAgeInfectionRate  { get; set; } = 0.000005; //can't be greater than 1
        public Age? EndangeredAgeGroup { get; set; } = Age.Pensioner;
        
        //infection kind
        public StateOfLife InfectionSeverity { get; set; } = StateOfLife.ImperceptiblyInfected;
        public double RateOfGettingWorse { get; set; } = 0.05; // can't be greater than 1
        public double EndangeredAgeRateOfGettingWorse  { get; set; } = 0.1; //can't be greater than 1
        
        //rate of death
        public double BaseDeathRate { get; set; } = 0.01; // can't be greater than 1
        public double EndangeredAgeDeathRate  { get; set; } = 0.05; //can't be greater than 1
        
        //rate of immunity
        public double BaseImmunityRate { get; set; } = 0.001; // can't be greater than 1
        public double EndangeredImmunityRate { get; set; } = 0.0005; // can't be greater than 1
        public double SurvivalInstinctMultiplier { get; set; } = 2; // multiplies immunity rate in life/death situation
        
        //multiplier of infection spread rate if everybody is infected
        public double InfectionSpreadRate { get; set; } = 100000; //0 -> no influence if more people infected, x > 0 -> x-times as infective
        
        #endregion
    }
}