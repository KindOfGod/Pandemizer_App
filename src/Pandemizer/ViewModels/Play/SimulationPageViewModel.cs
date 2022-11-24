using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using ReactiveUI;
using SkiaSharp;

namespace Pandemizer.ViewModels.Play;

public class SimulationPageViewModel : ViewModelBase
{
    #region Fields

    private Sim _currentSim;

    //base info
    private string _healthy;
    private string _infected;
    private string _vaccinated;
    private string _dead;
    
    //additional info
    private string _incidence;
    
    //graphs
    public ISeries[] _incidenceSeries;
    private ObservableCollection<ObservablePoint> _incidenceData { get; set; } = new();

    #endregion

    #region Properties
    
    //base info
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
    
    
    //graphs
    public Axis[] XAxes { get; set; } =
    {
        new Axis
        {
            Name = "Iteration",
            NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
            NameTextSize = 15,
            LabelsPaint = new SolidColorPaint(SKColors.White),
            NamePaint = new SolidColorPaint(SKColors.White)
        }
    };
    public ISeries[] IncidenceSeries 
    {
        get => _incidenceSeries;
        set => this.RaiseAndSetIfChanged(ref _incidenceSeries, value);
    }

    #endregion

    #region Constructors
    //default constructor for design time
    public SimulationPageViewModel()
    {
        InitGraphs();
    }
    public SimulationPageViewModel(Sim sim)
    {
        _currentSim = sim;
        
        InitGraphs();
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
        Healthy = ApplicationHelper.IntToFormatedNum((int)state.Healthy);
        Infected = ApplicationHelper.IntToFormatedNum((int)state.TotalInfected);
        Vaccinated = ApplicationHelper.IntToFormatedNum((int)state.Vaccinated);
        Dead = ApplicationHelper.IntToFormatedNum((int)state.Dead);
        
        //additional info
        Incidence = ApplicationHelper.IntToFormatedNum((int)state.Incidence);
        
        //graphs
        _incidenceData.Add(new ObservablePoint(stateNum, state.Incidence));
    }

    private void InitGraphs()
    {
        IncidenceSeries = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(SKColors.Yellow, 3),
                GeometryFill = new SolidColorPaint(SKColors.Yellow),
                GeometryStroke = null,
                GeometrySize = 15,
                Values = _incidenceData,
                Name = "",
                Fill = null
            }
        };
    }

    #endregion
}