using System;
using System.Diagnostics;
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

    private string? _iterationTime;
    private string? _popCount;

    //base info
    private string? _iteration;
    private string? _simName;
    
    private string? _healthy;
    private string? _infected;
    private string? _immune;
    private string? _dead;

    //additional info
    private string? _incidence;
    private string? _deathRate;
    private string? _hospitalRate;
    
    private bool _iterationButtonsEnabled;

    private OverviewTabViewModel? _overviewTab;
    private HealthcareTabViewModel? _healthcareTab;
    private PopulationTabViewModel? _populationTab;
    
    #endregion

    #region Properties
    
    //base info
    public string? IterationTime
    {
        get => _iterationTime;
        set => this.RaiseAndSetIfChanged(ref _iterationTime, value);
    }
    public string? PopCount
    {
        get => _popCount;
        set => this.RaiseAndSetIfChanged(ref _popCount, value);
    }
    public string? Iteration
    {
        get => _iteration;
        set => this.RaiseAndSetIfChanged(ref _iteration, value);
    }
    public string? SimName
    {
        get => _simName;
        set => this.RaiseAndSetIfChanged(ref _simName, value);
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
    
    public string? Immune
    {
        get => _immune;
        set => this.RaiseAndSetIfChanged(ref _immune, value);
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
    
    public string? HospitalRate
    {
        get => _hospitalRate;
        set => this.RaiseAndSetIfChanged(ref _hospitalRate, value);
    }
    
    //Tabs

    public OverviewTabViewModel? OverviewTab
    {
        get => _overviewTab;
        set => this.RaiseAndSetIfChanged(ref _overviewTab, value);
    }
    
    public HealthcareTabViewModel? HealthcareTab
    {
        get => _healthcareTab;
        set => this.RaiseAndSetIfChanged(ref _healthcareTab, value);
    }
    
    public PopulationTabViewModel? PopulationTab
    {
        get => _populationTab;
        set => this.RaiseAndSetIfChanged(ref _populationTab, value);
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
        var testSim = SimEngine.CreateNewSim(new SimInfo()
            {
                Name = "TestSim"
            },
            new SimSettings());

        for (var i = 0; i < 5; i++)
            SimEngine.IterateSimulation(testSim);

        _currentSim = testSim;
        
        Init();
        RefreshUi(testSim.SimInfo.Iteration + 1);
    }
    public SimulationPageViewModel(Sim sim)
    {
        _currentSim = sim;
        
        Init();
        
        //+ 1 for iteration 0
        RefreshUi(sim.SimInfo.Iteration + 1);
        
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

        //save updated iteration
        ApplicationService.DataService.SaveSim(_currentSim);
    }

    /// <summary>
    /// Iterates Simulation cntInt times.
    /// </summary>
    private async void Iterate(int iterationCount)
    {
        var timer = new Stopwatch();
        timer.Start();

        IterationButtonsEnabled = false;
        
        var iterations = _currentSim.SimStates.Count - 1;
        
        if(iterations > _currentSim.SimSettings.IterationLimit)
            return;
        if (iterations + iterationCount > _currentSim.SimSettings.IterationLimit)
            iterationCount = _currentSim.SimSettings.IterationLimit - iterations;

        await Task.Run((() =>
        {
            for (var i = iterationCount; i > 0; i--)
                SimEngine.IterateSimulation(_currentSim);
        }));

        RefreshUi(iterationCount);
        
        if(_currentSim.SimStates.Count - 1 < _currentSim.SimSettings.IterationLimit)
            IterationButtonsEnabled = true;
        
        timer.Stop();
        IterationTime = $"{Math.Round(timer.Elapsed.TotalMilliseconds, 2):N2} ms";
    }

    /// <summary>
    /// Refreshes visible information on UI. It also adds the last 'missingIterations'-values to charts.
    /// </summary>
    private void RefreshUi(int missingIterations)
    {
        var state = _currentSim.SimStates[^1];
        var stateNum = _currentSim.SimStates.Count - 1;

        //basic info
        Iteration = ApplicationHelper.IntToFormattedNum(stateNum);
        
        Healthy = ApplicationHelper.IntToFormattedNum((int)state.Healthy + (int)state.ImperceptibleInfected); // show ImperceptibleInfected as Healthy
        Infected = ApplicationHelper.IntToFormattedNum((int)state.TotalInfected);
        Immune = ApplicationHelper.IntToFormattedNum((int)state.Immune);
        Dead = ApplicationHelper.IntToFormattedNum((int)state.Dead);
        
        //additional info
        Incidence = ApplicationHelper.IntToFormattedNum((int)state.Incidence);
        DeathRate = ApplicationHelper.IntToFormattedNum((int)state.DeathRate);
        HospitalRate = ApplicationHelper.IntToFormattedNum((int)state.HospitalizedPercent) + "%";

        //charts
        for (var i = missingIterations; i > 0; i--)
        {
            state = _currentSim.SimStates[^i];
            
            stateNum = _currentSim.SimStates.Count - i;
            
            OverviewTab?.IncidenceData.Add(new ObservablePoint(stateNum, state.Incidence));
            OverviewTab?.DeathRateData.Add(new ObservablePoint(stateNum, state.DeathRate));

            OverviewTab?.HealthyData.Add(new ObservablePoint(stateNum, state.Healthy + state.ImperceptibleInfected));
            OverviewTab?.InfectedData.Add(new ObservablePoint(stateNum, state.Infected + state.HeavilyInfected));
            OverviewTab?.DeadData.Add(new ObservablePoint(stateNum, state.Dead));
            OverviewTab?.ImmuneData.Add(new ObservablePoint(stateNum, state.Immune));
            OverviewTab?.ImmuneRateData.Add(new ObservablePoint(stateNum, state.ImmuneRate));
            
            HealthcareTab?.HospitalizedData.Add(new ObservablePoint(stateNum, state.HospitalizedPercent));
        }

        //healthcare polar charts
        HealthcareTab!.AgeData[0].Value = (int) state.HospitalizedChildren;
        HealthcareTab!.AgeData[1].Value = (int) state.HospitalizedYoungAdults;
        HealthcareTab!.AgeData[2].Value = (int) state.HospitalizedAdults;
        HealthcareTab!.AgeData[3].Value = (int) state.HospitalizedPensioner;
        
        //healthcare pie charts
        HealthcareTab!.PreExistingConditionData[0].Value = (int) state.HospitalizedNoPreExistingCondition;
        HealthcareTab!.PreExistingConditionData[1].Value = (int) state.HospitalizedPreExistingCondition;

        if (state is {HospitalizedNoPreExistingCondition: 0, HospitalizedPreExistingCondition: 0})
            HealthcareTab!.PreExistingConditionData[0].Value = 1;
        
        PopCount = ApplicationHelper.IntToFormattedNum(state.PopIndex.Count);

        RefreshChartsAndData();
    }

    /// <summary>
    /// Resets all zoom and panning of charts in control
    /// </summary>
    private void RefreshChartsAndData()
    {
        OverviewTab?.RefreshCharts();
        HealthcareTab?.RefreshCharts();
        PopulationTab?.RefreshData(_currentSim.SimStates[^1].PopIndex);
    }
    
    private void Init()
    {
        OverviewTab = new OverviewTabViewModel();
        HealthcareTab = new HealthcareTabViewModel();
        PopulationTab = new PopulationTabViewModel();
        
        OverviewTab.Init();
        HealthcareTab.Init();

        SimName = _currentSim.SimInfo.Name;

        IterationButtonsEnabled = true;
    }

    #endregion
}