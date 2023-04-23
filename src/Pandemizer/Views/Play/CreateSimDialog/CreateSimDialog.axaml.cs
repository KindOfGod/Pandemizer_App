using System;
using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Pandemizer.ViewModels.Play.CreateSimDialog;
using ReactiveUI;

namespace Pandemizer.Views.Play.CreateSimDialog;

public partial class CreateSimDialog : ReactiveWindow<CreateSimDialogViewModel>
{
    public CreateSimDialog()
    {
        InitializeComponent();
        #if DEBUG
            this.AttachDevTools();
        #endif
        
        this.WhenActivated(d => d(ViewModel!.CreateSimCommand.Subscribe(Close!)));
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}