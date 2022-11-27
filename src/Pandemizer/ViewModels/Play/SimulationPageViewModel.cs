using System;
using System.Reactive;
using System.Threading.Tasks;
using LiveChartsCore.Defaults;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using Pandemizer.ViewModels.Play.SimPage;
using ReactiveUI;

namespace Pandemizer.ViewModels.Play;

public class SimulationPageViewModel : ViewModelBase
{
    #region Fields

    private readonly Sim _currentSim = null!;

    //base info
    private string? _iteration;
    
    private string? _healthy;
    private string? _infected;
    private string? _vaccinated;
    private string? _dead;
    
    //additional info
    private string? _incidence;
    private string? _deathRate;
    
    private bool _iterationButtonsEnabled;

    private OverviewTabViewModel? _overviewTab;
    
    #endregion

    #region Properties
    
    //base info
    public string? Iteration
    {
        get => _iteration;
        set => this.RaiseAndSetIfChanged(ref _iteration, value);
    }
    public string? Healthy
    {
        get => _healthy;
        set => this.RaiseAndSetIfChanged(ref _healthy, value);
    }
    
    public string? Infected
    {
        get => _infected;
        set => this.RaiseAndSetIfChanged(ref _infected, value);
    }
    
    public string? Vaccinated
    {
        get => _vaccinated;
        set => this.RaiseAndSetIfChanged(ref _vaccinated, value);
    }
    
    public string? Dead
    {
        get => _dead;
        set => this.RaiseAndSetIfChanged(ref _dead, value);
    }
    
    //additional info
    public string? Incidence
    {
        get => _incidence;
        set => this.RaiseAndSetIfChanged(ref _incidence, value);
    }
    
    public string? DeathRate
    {
        get => _deathRate;
        set => this.RaiseAndSetIfChanged(ref _deathRate, value);
    }
    
    //Tabs

    public OverviewTabViewModel? OverviewTab
    {
        get => _overviewTab;
        set => this.RaiseAndSetIfChanged(ref _overviewTab, value);
    }
    
    //rest
    
    public bool IterationButtonsEnabled
    {
        get => _iterationButtonsEnabled;
        set => this.RaiseAndSetIfChanged(ref _iterationButtonsEnabled, value);
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
        RefreshUi(1);
        
        ForwardCommand = ReactiveCommand.Create<string>(OnForwardCommand);
    }

    #endregion
    
    #region Commands

    public ReactiveCommand<string, Unit>? ForwardCommand { get; }

    #endregion

    #region Private Methods

    private void OnForwardCommand(string parameter)
    {
        var p = Convert.ToInt32(parameter);
        
        if (p == 0)
            p = _currentSim.SimSettings.IterationLimit;
        
        Iterate(p);
    }

    /// <summary>
    /// Iterates Simulation cntInt times.
    /// </summary>
    private async void Iterate(int cntInt)
    {
        IterationButtonsEnabled = false;
        
        var iterations = _currentSim.SimStates.Count - 1;
        
        if(iterations > _currentSim.SimSettings.IterationLimit)
            return;
        if (iterations + cntInt > _currentSim.SimSettings.IterationLimit)
            cntInt = _currentSim.SimSettings.IterationLimit - iterations;

        await Task.Run((() =>
        {
            for (var i = cntInt; i > 0; i--)
                SimEngine.IterateSimulation(_currentSim);
        }));

        RefreshUi(cntInt);
        
        if(_currentSim.SimStates.Count - 1 < _currentSim.SimSettings.IterationLimit)
            IterationButtonsEnabled = true;
    }

    /// <summary>
    /// Refreshes visible information on UI. It also add the last 'cnt'-values to charts.
    /// </summary>
    private void RefreshUi(int cnt)
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
        DeathRate = ApplicationHelper.IntToFormatedNum((int)state.DeathRate);
        
        //charts
        for (var i = cnt; i > 0; i--)
        {
            state = _currentSim.SimStates[^i];
            
            stateNum = _currentSim.SimStates.Count - i;
            
            OverviewTab?.IncidenceData.Add(new ObservablePoint(stateNum, state.Incidence));
            OverviewTab?.DeathRateData.Add(new ObservablePoint(stateNum, state.DeathRate));

            OverviewTab?.HealthyData.Add(new ObservablePoint(stateNum, state.Healthy + state.ImperceptibleInfected));
            OverviewTab?.InfectedData.Add(new ObservablePoint(stateNum, state.Infected));
            OverviewTab?.HeavilyInfectedData.Add(new ObservablePoint(stateNum, state.HeavilyInfected));
            OverviewTab?.DeadData.Add(new ObservablePoint(stateNum, state.Dead));
        }
        
        RefreshCharts();
    }

    /// <summary>
    /// Resets all zoom and panning of charts in control
    /// </summary>
    private void RefreshCharts()
    {
        OverviewTab?.RefreshCharts();
    }
    
    private void Init()
    {
        OverviewTab = new OverviewTabViewModel();
        
        OverviewTab.Init();

        IterationButtonsEnabled = true;
    }

    #endregion
}