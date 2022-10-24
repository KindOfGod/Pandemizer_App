using System.Collections.Generic;

namespace Pandemizer.Services.SimulationEngine.Datamodel;

public class Game
{
    #region Properties

    public string Name { get; set; }
    public List<Person> People { get; set; }

    #endregion
    

    #region Constructors

    public Game(string name)
    {
        Name = name;
        
        People = new List<Person>();
    }

    #endregion
}