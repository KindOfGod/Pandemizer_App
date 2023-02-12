using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using ReactiveUI;

namespace Pandemizer.ViewModels.Play;

public class PlayPageViewModel : ViewModelBase
{
    #region Fields

    private Sim? _selectedSim;

    #endregion
    
    #region Properties

    public Sim? SelectedSim
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
    public ReactiveCommand<Unit, Unit> DeleteClick { get; }
    public ReactiveCommand<Unit, Unit> CreateClick { get; }

    #endregion

    #region Constructors

    public PlayPageViewModel()
    {
        PlayClick = ReactiveCommand.Create(OnPlayClick);
        DeleteClick = ReactiveCommand.Create(OnDeleteClick);
        CreateClick = ReactiveCommand.Create(OnCreateClick);
    }

    #endregion

    #region Private Method

    private void OnPlayClick()
    {
        if(_selectedSim == null)
            return;
            
        LoadSimulation(SelectedSim?.SimInfo.Name!);
    }
    
    private void OnDeleteClick()
    {
        if(_selectedSim == null)
            return;
            
        ApplicationService.DataService.DeleteSim(SelectedSim?.SimInfo.Name!);
        Games.Remove(Games.First(x => x.SimInfo.Name == SelectedSim?.SimInfo.Name));
    }
    
    private async void OnCreateClick()
    {
        var newSim = SimEngine.CreateNewSim(new SimInfo() 
        {
            Name = "TestSim"
        },
        new SimSettings()
        {
            Scope = 100_000_000
        });
        
        ApplicationService.Simulations.Add(newSim);
        SelectedSim = newSim;
        await ApplicationService.DataService.SaveSim(newSim);
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
        var sim = await ApplicationService.DataService.ReadSim(name);
        
        if(sim != null)
            StartSimulation(sim);
    }

    #endregion
}