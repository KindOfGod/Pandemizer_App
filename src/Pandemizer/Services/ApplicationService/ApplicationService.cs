using Pandemizer.Services.DataService;
using Pandemizer.Services.SimulationEngine.Datamodel;

namespace Pandemizer.Services.ApplicationService
{
    public static class ApplicationService
    {

        #region Fields

        private static IDataService _dataService;
        
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
            var game = new Game("myGame");
            for (var i = 0; i < 10000; i++)
            {
                game.People.Add(new Person()
                {
                    Name = $"name{i}"
                });
            }

            _dataService.AppendDataAsync( "test", game);
        }

        #endregion
    }
}