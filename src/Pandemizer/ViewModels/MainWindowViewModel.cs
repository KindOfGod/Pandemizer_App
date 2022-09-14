using ReactiveUI;

namespace Pandemizer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private bool _isPaneOpen;
        
        #endregion

        #region Properties

        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            IsPaneOpen = true;
        }

        #endregion
    }
}