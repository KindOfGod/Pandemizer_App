namespace Pandemizer.Services.SimulationEngine.Datamodel;
/// <summary>
/// PeopleBase contains all static information about the simulation people. It does not contain
/// dynamic information.
/// </summary>
public class PeopleBase
{
    #region Properties

    public int[] Age;

    #endregion

    #region Constructor

    public PeopleBase(int scope)
    {
        Age = new int[scope];
    }

    #endregion
}