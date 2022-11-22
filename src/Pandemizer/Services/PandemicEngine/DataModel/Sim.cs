using System.Collections.Generic;

namespace Pandemizer.Services.PandemicEngine.DataModel
{
    /// <summary>
    /// Contains all information and settings from a single Simulation
    /// </summary>
    public class Sim
    {
        public SimSettings SimSettings { get; set; }
        public List<SimState> SimStates { get; set; }
        
        public Sim(SimSettings simSettings, List<SimState> simStates)
        {
            SimSettings = simSettings;
            SimStates = simStates;
        }
    }
}