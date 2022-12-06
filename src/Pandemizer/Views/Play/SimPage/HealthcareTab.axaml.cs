using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pandemizer.Views.Play.SimPage;

public partial class HealthcareTab : UserControl
{
    public HealthcareTab()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}