using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Pandemizer.Services;
using ReactiveUI;
using SkiaSharp;

namespace Pandemizer.ViewModels.Play.SimPage;

public class OverviewTabViewModel : ViewModelBase
{
    #region Fields
    
    private ISeries[] _healthStateSeries;
    private ISeries[] _ratesSeries;

    public ObservableCollection<ObservablePoint> HealthyData { get; set; } = new();
    public ObservableCollection<ObservablePoint> InfectedData { get; set; } = new();
    public ObservableCollection<ObservablePoint> HeavilyInfectedData { get; set; } = new();
    public ObservableCollection<ObservablePoint> DeadData { get; set; } = new();
    public ObservableCollection<ObservablePoint> ImmuneData { get; set; } = new();
    public ObservableCollection<ObservablePoint> ImmuneRateData { get; set; } = new();
    public ObservableCollection<ObservablePoint> IncidenceData { get; set; } = new();
    public ObservableCollection<ObservablePoint> DeathRateData { get; set; } = new();

    public long[] HealthStatePieData { get; set; } = {0, 0, 0, 0};
    
    private static readonly Axis _iterationAxis = new()
    {
        Name = "Iteration",
        NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 1
    };

    private static readonly Axis _peopleAxis = new()
    {
        Name = "People",
        NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 1
    };
        

    #endregion
    
    #region Properties
    public ISeries[] HealthStatesSeries 
    {
        get => _healthStateSeries;
        set => this.RaiseAndSetIfChanged(ref _healthStateSeries, value);
    }

    public ISeries[] RatesSeries
    {
        get => _ratesSeries;
        set => this.RaiseAndSetIfChanged(ref _ratesSeries, value);
    }

    #endregion

    #region Axes
    public Axis[] XHealthStates { get; set; } = {_iterationAxis};
    public Axis[] YHealthStates { get; set; } = {_peopleAxis};
    public Axis[] XRate { get; set; } = {_iterationAxis};
    public Axis[] YRate { get; set; } = {_peopleAxis};

    #endregion

    #region Commands

    public ReactiveCommand<string, Unit> ToggleHealthStatesAxis { get; }
    public ReactiveCommand<string, Unit> ToggleRatesAxis { get; }

    #endregion

    #region Constructors

    public OverviewTabViewModel()
    {
        ToggleHealthStatesAxis = ReactiveCommand.Create<string>(OnToggleHealthStatesAxis);
        ToggleRatesAxis = ReactiveCommand.Create<string>(OnToggleRatesAxis);
    }

    #endregion

    #region Public Methods
    public void RefreshCharts()
    {
        XHealthStates.First().MinLimit = null;
        XHealthStates.First().MaxLimit = null;
        YHealthStates.First().MinLimit = null;
        YHealthStates.First().MaxLimit = null;

        XRate.First().MinLimit = null;
        XRate.First().MaxLimit = null;
        YRate.First().MinLimit = null;
        YRate.First().MaxLimit = null;
    }
    
    public void Init()
    {
        
        HealthStatesSeries = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.HealthyColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.HealthyColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = HealthyData,
                Name = "Healthy",
                Fill = null
            },
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.InfectedColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.InfectedColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = InfectedData,
                Name = "Infected",
                Fill = null
            },
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.HeavilyInfectedColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.HeavilyInfectedColor,3),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = HeavilyInfectedData,
                Name = "Heavily Infected",
                Fill = null
            },
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.ImmuneColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.ImmuneColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = ImmuneData,
                Name = "Immune",
                Fill = null
            },
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.DeadColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.DeadColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = DeadData,
                Name = "Dead",
                Fill = null
            }
        };
        
        RatesSeries = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.InfectedColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.InfectedColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = IncidenceData,
                Name = "Incidence",
                Fill = null
            },
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.ImmuneColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.ImmuneColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = ImmuneRateData,
                Name = "Immunity Rate",
                Fill = null
            },
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.DeadColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.DeadColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = DeathRateData,
                Name = "Death Rate",
                Fill = null
            }
        };
        
        RefreshCharts();
    }

    #endregion

    #region Private Methods

    private void OnToggleHealthStatesAxis(string parameter)
    {
        var p = Convert.ToInt32(parameter);
        HealthStatesSeries[p].IsVisible = !HealthStatesSeries[p].IsVisible;
    }
    
    private void OnToggleRatesAxis(string parameter)
    {
        var p = Convert.ToInt32(parameter);
        RatesSeries[p].IsVisible = !RatesSeries[p].IsVisible;
    }
    
    #endregion
}