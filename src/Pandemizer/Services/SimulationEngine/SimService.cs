using Pandemizer.Services.SimulationEngine.Datamodel;

namespace Pandemizer.Services.SimulationEngine;

public static class SimService
{
    public static Sim CurrentSimulation = null;
    
    #region Public Methods

    public static void CreateSimulation(string simName, SimSettings simSettings)
    {
        CurrentSimulation = new Sim(simName, simSettings);
        ApplicationService._dataService.CreateNewSimSave(CurrentSimulation);
    }
    
    #endregion
}