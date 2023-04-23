using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Pandemizer.Services.DataService;
using Pandemizer.Services.PandemicEngine.DataModel;
using Pandemizer.ViewModels;
using Pandemizer.ViewModels.Play.CreateSimDialog;

namespace Pandemizer.Services
{
    /// <summary>
    /// Main Service which controls the Application.
    /// </summary>
    public static class ApplicationService
    {

        #region Fields

        public static IDataService DataService;
        public static MainWindowViewModel? MainWindowViewModel;

        public static ObservableCollection<Sim> Simulations = new();
        
        #endregion
        
        #region Constructors
        static ApplicationService()
        {
            DataService = new DataServiceImpl();
        }

        #endregion

        #region Public Methods
        
        /// <summary>
        /// Change the viewModel the application is displaying in (app)full screen
        /// </summary>
        public static void ChangeFullscreenView(ViewModelBase viewModel)
        {
            MainWindowViewModel?.ChangeFullscreenView(viewModel);
        }
        
        /// <summary>
        /// Reset (app)full screen
        /// </summary>
        public static void ResetFullScreenView()
        {
            MainWindowViewModel?.ResetFullscreenView();
        }
        
        /// <summary>
        /// OpenCreateDialog
        /// </summary>
        public static async Task<Sim?> OpenCreateDialog()
        {
            var viewModel = new CreateSimDialogViewModel();
            return await MainWindowViewModel?.ShowCreateDialog.Handle(viewModel)!;
        }

        public static void OnStartUp()
        {
            LoadSimulations();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load all simulations
        /// </summary>
        private static async void LoadSimulations()
        {
            foreach (var sim in (await DataService.ReadAllSims()).Where(sim => sim != null))
            {
                Simulations.Add(sim!);
            }
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