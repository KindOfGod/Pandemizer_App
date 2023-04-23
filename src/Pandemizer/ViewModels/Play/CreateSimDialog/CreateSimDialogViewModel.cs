using System.Collections.ObjectModel;
using System.Reactive;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using ReactiveUI;

namespace Pandemizer.ViewModels.Play.CreateSimDialog;

public class CreateSimDialogViewModel : ViewModelBase
{
    #region Fields

    private string _simName = "NewSim";

    private SimInfo _simInfo = new();
    private SimSettings _simSettings = new();
    
    public Virus _selectedVirus = new();

    private ObservableCollection<Virus> _viruses = new();
    #endregion
    
    #region Properties
    public SimSettings SimSettings 
    {
        get => _simSettings;
        set => this.RaiseAndSetIfChanged(ref _simSettings, value);
    }
    
    public Virus SelectedVirus
    {
        get => _selectedVirus;
        set => this.RaiseAndSetIfChanged(ref _selectedVirus, value);
    }
    
    public ObservableCollection<Virus> Viruses
    {
        get => _viruses;
        set => this.RaiseAndSetIfChanged(ref _viruses, value);
    }
    
    public string SimName
    {
        get => _simName;
        set
        {
            this.RaiseAndSetIfChanged(ref _simName, value);
            CheckIfNameExists(_simName);
        }
    }

    #endregion
    
    #region Commands
    public ReactiveCommand<Unit, Sim> CreateSimCommand { get; }

    #endregion

    #region Constructors

    public CreateSimDialogViewModel()
    {
        Init();
        
        CreateSimCommand = ReactiveCommand.Create(() =>
        {
            SimSettings.Virus = SelectedVirus;
            return SimEngine.CreateNewSim(_simInfo, SimSettings);
        });
    }

    #endregion

    #region PrivateMethods

    private async void Init()
    {
        foreach (var virus in await ApplicationService.DataService.ReadAllViruses())
        {
            if(virus != null)
                Viruses.Add(virus);
        }

        SelectedVirus = Viruses[0];
        _simInfo.Name = SimName;
    }
    
    private void CheckIfNameExists(string name)
    {
        //check if name exists
        
        _simInfo.Name = name;
    }

    #endregion
}