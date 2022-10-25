using Pandemizer.Services.SimulationEngine.Datamodel;
using Pandemizer.Services.SimulationEngine.Enums;

namespace Pandemizer.Services.SimulationEngine;

public static class SimHelper
{
    #region Public Methods

    public static PeopleBase GenerateInitialPeopleBase(SimSettings simSettings)
    {
        return new PeopleBase(simSettings.Scope);
    }
    public static PeopleState GenerateInitialPeopleState(SimSettings simSettings, PeopleBase peopleBase)
    {
        return new PeopleState(simSettings.Scope);
    }
    public static SimState GenerateInitialSimState(SimSettings simSettings, PeopleState peopleState)
    {
        return new SimState();
    }

    #endregion

    #region Private Methods

    

    #endregion
}