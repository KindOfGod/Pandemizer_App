using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Pandemizer.Services;
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
        
        if (sender is Control {Name: "VirusName"})
        {
            var oldValue = e.OldValue?.ToString();
            var newValue = e.NewValue?.ToString();

            if (oldValue != null && newValue != null && oldValue != "" && newValue != "")
            {
                if(newValue.Contains(oldValue) || oldValue.Contains(newValue))
                    ApplicationService.DataService.DeleteVirus(oldValue);
            }
                
        }
        
        switch (e.Property.Name)
        {
            case "Value":
            case "Text":
                viewModel.SaveVirus();
                break;
        }
    }
}