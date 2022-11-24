using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pandemizer.Views.Play.SimPage;

public partial class OverviewTab : UserControl
{
    public OverviewTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}