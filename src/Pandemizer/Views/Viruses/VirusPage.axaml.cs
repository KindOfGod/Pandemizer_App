using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pandemizer.ViewModels.Viruses;

namespace Pandemizer.Views.Viruses;

public partial class VirusPage : UserControl
{
    public VirusPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void AvaloniaObject_OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (DataContext is not VirusesPageViewModel viewModel)
            return;

        switch (e.Property.Name)
        {
            case "Value":
            case "Text":
                viewModel.SaveVirus();
                break;
        }
    }
}