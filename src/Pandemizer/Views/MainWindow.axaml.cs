using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using Pandemizer.Services.PandemicEngine.DataModel;
using Pandemizer.ViewModels;
using Pandemizer.ViewModels.Play.CreateSimDialog;
using Pandemizer.Views.Play.CreateSimDialog;
using ReactiveUI;

namespace Pandemizer.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.ShowCreateDialog.RegisterHandler(DoShowDialogAsync)));
        }
        
        private async Task DoShowDialogAsync(InteractionContext<CreateSimDialogViewModel, Sim?> interaction)
        {
            var dialog = new CreateSimDialog
            {
                DataContext = interaction.Input
            };

            var result = await dialog.ShowDialog<Sim?>(this);
            interaction.SetOutput(result);
        }
    }
}