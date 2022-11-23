using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Material.Icons;
using Pandemizer.ViewModels.Compare;
using Pandemizer.ViewModels.Library;
using Pandemizer.ViewModels.Play;
using Pandemizer.ViewModels.Sandbox;
using Pandemizer.ViewModels.Viruses;
using ReactiveUI;

namespace Pandemizer.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        private bool _isPaneOpen;
        private bool _isNavVisible;

        private ViewModelBase? _navigationContent;
        private ViewModelBase? _fullscreenContent;
        private ListBoxItem? _selectedMenuItem;

        private List<ViewModelBase> _navigationPageInstances;
        
        private MaterialIconKind _hamburgerMenuIcon;
        
        #endregion

        #region Properties
        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
        }
        
        public bool IsNavVisible
        {
            get => _isNavVisible;
            set => this.RaiseAndSetIfChanged(ref _isNavVisible, value);
        }

        public ViewModelBase? NavigationContent
        {
            get => _navigationContent;
            set => this.RaiseAndSetIfChanged(ref _navigationContent, value);
        }
        
        public ViewModelBase? FullscreenContent
        {
            get => _fullscreenContent;
            set => this.RaiseAndSetIfChanged(ref _fullscreenContent, value);
        }

        public ListBoxItem? SelectedMenuItem
        {
            get => _selectedMenuItem;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedMenuItem, value);
                OnNavigationChanged();
            }
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
            IsNavVisible = true;
            
            HamburgerMenuIcon = MaterialIconKind.HamburgerMenuBack;
            _navigationPageInstances = new List<ViewModelBase>();

            HamburgerMenuClick = ReactiveCommand.Create(OnHamburgerMenuClick);
        }

        #endregion

        #region Commands

        public ReactiveCommand<Unit, Unit> HamburgerMenuClick { get; }

        #endregion

        #region Public Methods
        /// <summary>
        /// Sets ViewModel into FullscreenFrame and disables navigation.
        /// </summary>
        public void ChangeFullscreenView(ViewModelBase viewModel)
        {
            IsNavVisible = false;
            FullscreenContent = viewModel;
        }
        
        /// <summary>
        /// Removes ViewModel from FullscreenFrame and enables navigation.
        /// </summary>
        public void ResetFullscreenView()
        {
            IsNavVisible = true;
            FullscreenContent = null;
        }

        #endregion
        
        #region Private Methods

        private void OnHamburgerMenuClick()
        {
            IsPaneOpen = !IsPaneOpen;
            HamburgerMenuIcon = HamburgerMenuIcon == MaterialIconKind.HamburgerMenu
                ? MaterialIconKind.HamburgerMenuBack
                : MaterialIconKind.HamburgerMenu;
        }

        private void OnNavigationChanged()
        {
            NavigationContent = SelectedMenuItem?.Name switch
            {
                "Play" => GetPageInstance(typeof(PlayPageViewModel)),
                "Sandbox" => GetPageInstance(typeof(SandboxPageViewModel)),
                "Viruses" => GetPageInstance(typeof(VirusesPageViewModel)),
                "Library" => GetPageInstance(typeof(LibraryPageViewModel)),
                "Compare" => GetPageInstance(typeof(ComparePageViewModel)),
                _ => null
            };
        }

        private ViewModelBase? GetPageInstance(Type type)
        {
            var instance = _navigationPageInstances.FirstOrDefault(p => p.GetType() == type);

            if (instance != null)
                return instance;

            switch (type.Name)
            {
                case nameof(PlayPageViewModel):
                    instance = new PlayPageViewModel();
                    break;
                case nameof(SandboxPageViewModel):
                    instance = new SandboxPageViewModel();
                    break;
                case nameof(VirusesPageViewModel):
                    instance = new VirusesPageViewModel();
                    break;
                case nameof(LibraryPageViewModel):
                    instance = new LibraryPageViewModel();
                    break;
                case nameof(ComparePageViewModel):
                    instance = new ComparePageViewModel();
                    break;
                default:
                    return null;
            }
            
            _navigationPageInstances.Add(instance);
            return instance;
        }

        #endregion
    }
}