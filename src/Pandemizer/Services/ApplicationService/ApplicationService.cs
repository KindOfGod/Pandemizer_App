using Pandemizer.Services.DataService;
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
        public static MainWindowViewModel _mainWindowViewModel;
        
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
            _mainWindowViewModel.ChangeFullscreenView(viewModel);
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