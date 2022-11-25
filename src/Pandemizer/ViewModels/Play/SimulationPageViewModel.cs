using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using Pandemizer.ViewModels.Play.SimPage;
using ReactiveUI;
using SkiaSharp;

namespace Pandemizer.ViewModels.Play;

public class SimulationPageViewModel : ViewModelBase
{
    #region Fields

    private Sim _currentSim;

    //base info
    private string _iteration;
    
    private string _healthy;
    private string _infected;
    private string _vaccinated;
    private string _dead;
    
    //additional info
    private string _incidence;

    private OverviewTabViewModel _overviewTab;
    
    #endregion

    #region Properties
    
    //base info
    public string Iteration
    {
        get => _iteration;
        set => this.RaiseAndSetIfChanged(ref _iteration, value);
    }
    public string Healthy
    {
        get => _healthy;
        set => this.RaiseAndSetIfChanged(ref _healthy, value);
    }
    
    public string Infected
    {
        get => _infected;
        set => this.RaiseAndSetIfChanged(ref _infected, value);
    }
    
    public string Vaccinated
    {
        get => _vaccinated;
        set => this.RaiseAndSetIfChanged(ref _vaccinated, value);
    }
    
    public string Dead
    {
        get => _dead;
        set => this.RaiseAndSetIfChanged(ref _dead, value);
    }
    
    //additional info
    public string Incidence
    {
        get => _incidence;
        set => this.RaiseAndSetIfChanged(ref _incidence, value);
    }
    
    //Tabs

    public OverviewTabViewModel OverviewTab
    {
        get => _overviewTab;
        set => this.RaiseAndSetIfChanged(ref _overviewTab, value);
    }
    
    #endregion

    #region Constructors
    //default constructor for design time
    public SimulationPageViewModel()
    {
        Init();
    }
    public SimulationPageViewModel(Sim sim)
    {
        _currentSim = sim;
        
        Init();
        RefreshUi();
        
        ForwardCommand = ReactiveCommand.Create(OnForwardCommand);
        ForwardCommand5 = ReactiveCommand.Create(OnForwardCommand5);
        ForwardCommand15 = ReactiveCommand.Create(OnForwardCommand15);
        ForwardCommand60 = ReactiveCommand.Create(OnForwardCommand60);
    }

    #endregion
    
    #region Commands

    public ReactiveCommand<Unit, Unit> ForwardCommand { get; }
    public ReactiveCommand<Unit, Unit> ForwardCommand5 { get; }
    public ReactiveCommand<Unit, Unit> ForwardCommand15 { get; }
    public ReactiveCommand<Unit, Unit> ForwardCommand60 { get; }

    #endregion

    #region Private Methods

    private void OnForwardCommand() { Iterate(1); }
    private void OnForwardCommand5() { Iterate(5); }
    private void OnForwardCommand15() { Iterate(15); }
    private void OnForwardCommand60() { Iterate(60); }

    /// <summary>
    /// Iterates Simulation cntInt times.
    /// </summary>
    private async void Iterate(int cntInt)
    {
        await Task.Run((() =>
        {
            for (var i = cntInt; i > 0; i--)
                SimEngine.IterateSimulation(_currentSim);
        }));
        
        RefreshUi();
    }

    private void RefreshUi()
    {
        var state = _currentSim.SimStates[^1];
        var stateNum = _currentSim.SimStates.Count - 1;
        
        //basic info
        Iteration = ApplicationHelper.IntToFormatedNum(stateNum);
        
        Healthy = ApplicationHelper.IntToFormatedNum((int)state.Healthy);
        Infected = ApplicationHelper.IntToFormatedNum((int)state.TotalInfected);
        Vaccinated = ApplicationHelper.IntToFormatedNum((int)state.Vaccinated);
        Dead = ApplicationHelper.IntToFormatedNum((int)state.Dead);
        
        //additional info
        Incidence = ApplicationHelper.IntToFormatedNum((int)state.Incidence);
        
        //graphs
        OverviewTab.IncidenceData.Add(new ObservablePoint(stateNum, state.Incidence));
        
        OverviewTab.HealthyData.Add(new ObservablePoint(stateNum, state.Healthy + state.ImperceptibleInfected));
        OverviewTab.InfectedData.Add(new ObservablePoint(stateNum, state.Infected));
        OverviewTab.HeavilyInfectedData.Add(new ObservablePoint(stateNum, state.HeavilyInfected));
        OverviewTab.DeadData.Add(new ObservablePoint(stateNum, state.Dead));
        
        RefreshCharts();
    }

    /// <summary>
    /// Resets all zoom and panning of charts in control
    /// </summary>
    private void RefreshCharts()
    {
        OverviewTab.RefreshCharts();
    }
    
    private void Init()
    {
        OverviewTab = new OverviewTabViewModel();
        
        OverviewTab.Init();
    }

    #endregion
}