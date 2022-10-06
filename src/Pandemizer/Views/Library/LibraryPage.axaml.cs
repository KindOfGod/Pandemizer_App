using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Pandemizer.Views.Library;

public partial class LibraryPage : UserControl
{
    public LibraryPage()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}