using Pandemizer.Services;

namespace Pandemizer.Services.PandemicEngine.DataModel
{
    /// <summary>
    /// Contains all dynamic Properties of a Simulation. Used to initialize a Sim.
    /// All Properties can be changed by the User and they have to contain a default value.
    /// </summary>
    public class SimSettings
    {
        // General
        public int Scope { get; set; } = 100_000_000;
        public int IterationLimit { get; set; } = 1_000;

        public double ProbabilityDeviation { get; set; } = 0.1;
        
        //Virus
        public Virus Virus { get; set; } = new();
        
        // Health Distribution
        public double InitialProportionOfInfected { get; set; } //evenly distributed between age groups
        public StateOfLife HealthIllnessSeverity { get; set; } = StateOfLife.Infected; // can't be Healthy or Dead
        
        // Age Distribution --> has to be 1 in total
        public double AgeProportionOfChildren { get; set; } = 0.15; 
        public double AgeProportionOfYoungAdults { get; set; } = 0.1;
        public double AgeProportionOfAdults { get; set; } = 0.45;
        public double AgeProportionOfPensioner { get; set; } = 0.3;
        
        // Pre-existing Condition distribution
        public double InitialProportionOfPreConditioned { get; set; } = 0.1; // can't be greater than 1
        
        //Hospital Settings
        public int HospitalCap { get; set; } = 1_000_000;
        public double InfectedHospitalizing { get; set; } = 0.5;
        public double HeavilyInfectedHospitalizing { get; set; } = 0.9;
    }
}