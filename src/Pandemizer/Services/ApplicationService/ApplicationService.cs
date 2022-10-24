using Avalonia.Logging;
using Pandemizer.Services.DataService;
using Pandemizer.Services.SimulationEngine;
using Pandemizer.Services.SimulationEngine.Datamodel;

namespace Pandemizer.Services
{
    public static class ApplicationService
    {

        #region Fields

        public static IDataService _dataService;
        
        #endregion
        
        #region Constructors
        static ApplicationService()
        {
            _dataService = new DataServiceImpl();
        }

        #endregion

        #region Public Methods

        

        #endregion

        //Todo: Remove
        #region TEST METHODS

        public static void TEST()
        {
            SimService.CreateSimulation("test", new SimSettings()
            {
                Scope = 10000
            });
        }

        #endregion
    }
}