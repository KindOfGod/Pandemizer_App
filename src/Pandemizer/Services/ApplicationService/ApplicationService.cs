using System.Collections.Generic;
using Pandemizer.Services.DataService;
using Pandemizer.Services.PandemicEngine.DataModel;
using Pandemizer.ViewModels;

namespace Pandemizer.Services
{
    /// <summary>
    /// Main Service which controls the Application.
    /// </summary>
    public static class ApplicationService
    {

        #region Fields

        public static IDataService _dataService;
        public static MainWindowViewModel? _mainWindowViewModel;

        private static List<Sim> _simulations = new List<Sim>();
        
        #endregion
        
        #region Constructors
        static ApplicationService()
        {
            _dataService = new DataServiceImpl();
        }

        #endregion

        #region Public Methods
        public static void ChangeFullscreenView(ViewModelBase viewModel)
        {
            _mainWindowViewModel?.ChangeFullscreenView(viewModel);
        }

        public static void OnStartUp()
        {
            LoadSimulations();
        }

        #endregion

        #region Private Methods

        private static void LoadSimulations()
        {
            _simulations = new List<Sim>();
        }

        #endregion

        //Todo: Remove
        #region TEST METHODS

        public static void TEST()
        {

        }

        #endregion
    }
}