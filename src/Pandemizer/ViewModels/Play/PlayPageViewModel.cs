using System.Reactive;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using Pandemizer.ViewModels.Compare;
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
        var sim = SimEngine.CreateNewSim(new SimSettings()
        {
           Scope = 100_000_000
        });
        StartSimulation(sim);
    }

    /// <summary>
    /// Starts simulation with given Sim.
    /// </summary>
    private void StartSimulation(Sim sim)
    {
        ApplicationService.ChangeFullscreenView(new SimulationPageViewModel(sim));
    }

    #endregion
}