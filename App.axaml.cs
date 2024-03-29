using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
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

                ApplicationService.MainWindowViewModel = mainViewModel;
                ApplicationService.OnStartUp();
            }

            base.OnFrameworkInitializationCompleted();
            
            //Todo: Remove
            ApplicationService.TEST();
        }
    }
}