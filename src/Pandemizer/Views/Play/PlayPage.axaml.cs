using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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