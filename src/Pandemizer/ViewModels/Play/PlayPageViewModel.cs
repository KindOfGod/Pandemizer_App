using System.Reactive;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using ReactiveUI;

namespace Pandemizer.ViewModels.Play;

public class PlayPageViewModel : ViewModelBase
{
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
        LoadSimulation("TestSim");
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
        
        for(var i = 0; i < 1000; i++)
            SimEngine.IterateSimulation(newSim);

        await ApplicationService._dataService.SaveSim(newSim);
        */
        
        var sim = await ApplicationService._dataService.ReadSim(name);
        
        if(sim != null)
            StartSimulation(sim);
    }

    #endregion
}