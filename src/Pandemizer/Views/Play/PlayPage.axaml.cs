using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pandemizer.ViewModels.Play;
using Pandemizer.ViewModels.Play.CreateSimDialog;
using ReactiveUI;

namespace Pandemizer.Views.Play;

public partial class PlayPage : UserControl
{
    public PlayPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}