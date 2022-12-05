using System.Collections.ObjectModel;
using Pandemizer.Services.PandemicEngine.DataModel;
using ReactiveUI;

namespace Pandemizer.ViewModels.Viruses;

public class VirusesPageViewModel : ViewModelBase
{
    #region Fields
    
    private Virus _selectedVirus = new ();

    private ObservableCollection<Virus> _virusList = new();

    #endregion
    
    #region Properties

    public Virus SelectedVirus
    {
        get => _selectedVirus;
        set => this.RaiseAndSetIfChanged(ref _selectedVirus, value);
    }

    public ObservableCollection<Virus> VirusList
    {
        get => _virusList;
        set => this.RaiseAndSetIfChanged(ref _virusList, value);
    }

    #endregion

    #region Constructors

    public VirusesPageViewModel()
    {
        for (var i = 0; i < 100; i++)
        {
            VirusList.Add(new Virus()
                {
                    Name = $"Virus {i}"
                });
        }

        SelectedVirus = VirusList[0];
    }

    #endregion
}