using System;
using System.Reactive;
using Material.Icons;
using ReactiveUI;

namespace Pandemizer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private bool _isPaneOpen;

        private ViewModelBase _navigationContent;
        private MaterialIconKind _hamburgerMenuIcon;
        
        #endregion

        #region Properties
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
        }

        public ViewModelBase NavigationContent
        {
            get => _navigationContent;
            set => this.RaiseAndSetIfChanged(ref _navigationContent, value);
        }

        public MaterialIconKind HamburgerMenuIcon
        {
            get => _hamburgerMenuIcon;
            set => this.RaiseAndSetIfChanged(ref _hamburgerMenuIcon, value);
        }

        #endregion

        #region Constructors

        public MainWindowViewModel()
        {
            IsPaneOpen = true;
            HamburgerMenuIcon = MaterialIconKind.HamburgerMenuBack;

            NavigationContent = null;

            HamburgerMenuButtonClick = ReactiveCommand.Create(OnHamburgerMenuClick);
        }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> HamburgerMenuButtonClick { get; }

        #endregion

        #region Private Methods

        private void OnHamburgerMenuClick()
        {
            IsPaneOpen = !IsPaneOpen;
            HamburgerMenuIcon = HamburgerMenuIcon == MaterialIconKind.HamburgerMenu
                ? MaterialIconKind.HamburgerMenuBack
                : MaterialIconKind.HamburgerMenu;
        }

        #endregion
    }
}