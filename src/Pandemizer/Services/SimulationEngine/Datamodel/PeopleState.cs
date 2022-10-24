using Pandemizer.Services.SimulationEngine.Enums;

namespace Pandemizer.Services.SimulationEngine.Datamodel;
/// <summary>
/// A PeopleState contains all dynamic values of all people in one game iteration.
/// The PeopleState only represents the dynamic data of the people
/// </summary>
public class PeopleState
{
    #region Properties

    public StateOfLife[] StateOfLives;

    #endregion

    #region Constructor

    public PeopleState(int scope)
    {
        StateOfLives = new StateOfLife[scope];
    }

    #endregion
}