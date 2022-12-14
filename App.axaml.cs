using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Pandemizer.Services;
using Pandemizer.ViewModels;
using MainWindow = Pandemizer.Views.MainWindow;

namespace Pandemizer
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var mainViewModel = new MainWindowViewModel();
                
                desktop.MainWindow = new MainWindow
                {
                    DataContext = mainViewModel,
                };

                ApplicationService._mainWindowViewModel = mainViewModel;
            }

            base.OnFrameworkInitializationCompleted();
            
            //Todo: Remove
            ApplicationService.TEST();
        }
    }
}