using System.Collections.ObjectModel;
using System.Reactive;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using ReactiveUI;

namespace Pandemizer.ViewModels.Play;

public class PlayPageViewModel : ViewModelBase
{
    #region Fields

    private Sim _selectedSim;

    #endregion
    
    #region Properties

    public Sim SelectedSim
    {
        get => _selectedSim;
        set => this.RaiseAndSetIfChanged(ref _selectedSim, value);
    }
    
    public ObservableCollection<Sim> Games
    {
        get => ApplicationService.Simulations;
        set => this.RaiseAndSetIfChanged(ref ApplicationService.Simulations, value);
    }

    #endregion
    
    #region Commands
    public ReactiveCommand<Unit, Unit> PlayClick { get; }

    #endregion

    #region Constructors

    public PlayPageViewModel()
    {
        PlayClick = ReactiveCommand.Create(OnPlayClick);
    }

    #endregion

    #region Private Method

    private void OnPlayClick()
    {
        if(_selectedSim != null)
            LoadSimulation(SelectedSim.SimInfo.Name);
    }

    /// <summary>
    /// Starts simulation with given Sim.
    /// </summary>
    private static void StartSimulation(Sim sim)
    {
        ApplicationService.ChangeFullscreenView(new SimulationPageViewModel(sim));
    }

    private static async void LoadSimulation(string name)
    {
        //Generate SaveGame
        
        /*var newSim = SimEngine.CreateNewSim(new SimInfo() 
            {
                Name = "TestSim"
            },
            new SimSettings()
            {
                Scope = 100_000_000
            });
        
        for(var i = 0; i < 100; i++)
            SimEngine.IterateSimulation(newSim);

        await ApplicationService._dataService.SaveSim(newSim);*/

        var sim = await ApplicationService.DataService.ReadSim(name);
        
        if(sim != null)
            StartSimulation(sim);
    }

    #endregion
}