using System.Collections.Generic;

namespace Pandemizer.Services.SimulationEngine.Datamodel;
/// <summary>
/// A Sim contains all static information of a simulation
/// </summary>
public class Sim
{
    #region Properties

    public string SimulationName;

    public PeopleBase PeopleBase;
    public SimSettings SimSettings;

    public List<SimState> SimStates;
    public List<PeopleState> PeopleStates;

    #endregion
    
    #region Constructors

    public Sim(string simName, SimSettings simSettings)
    {
        SimulationName = simName;
        SimSettings = simSettings;

        PeopleBase = SimHelper.GenerateInitialPeopleBase(simSettings);
        
        PeopleStates = new List<PeopleState>
        {
            SimHelper.GenerateInitialPeopleState(simSettings, PeopleBase)
        };

        SimStates = new List<SimState>
        {
            SimHelper.GenerateInitialSimState(simSettings, PeopleStates[0])
        };
    }

    #endregion
}