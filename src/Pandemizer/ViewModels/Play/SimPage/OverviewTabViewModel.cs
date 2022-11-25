using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Pandemizer.Services;
using ReactiveUI;
using SkiaSharp;

namespace Pandemizer.ViewModels.Play.SimPage;

public class OverviewTabViewModel : ViewModelBase
{
    #region Fields

    public ISeries[] _incidenceSeries;
    public ISeries[] _healthStateSeries;

    public ObservableCollection<ObservablePoint> HealthyData { get; set; } = new();
    public ObservableCollection<ObservablePoint> InfectedData { get; set; } = new();
    public ObservableCollection<ObservablePoint> HeavilyInfectedData { get; set; } = new();
    public ObservableCollection<ObservablePoint> DeadData { get; set; } = new();
    public ObservableCollection<ObservablePoint> IncidenceData { get; set; } = new();

    public long[] HealthStatePieData { get; set; } = {0, 0, 0, 0};
    
    private static Axis _iterationAxis = new()
    {
        Name = "Iteration",
        NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 1
    };

    private static Axis _poeopleAxis = new()
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
    
    public ISeries[] IncidenceSeries 
    {
        get => _incidenceSeries;
        set => this.RaiseAndSetIfChanged(ref _incidenceSeries, value);
    }

    #endregion

    #region Axes
    public Axis[] XHealthStates { get; set; } = {_iterationAxis};
    public Axis[] YHealthStates { get; set; } = {_poeopleAxis};
    public Axis[] XIncidence { get; set; } = {_iterationAxis};
    public Axis[] YIncidence { get; set; } = {_poeopleAxis};

    #endregion

    #region Commands

    public ReactiveCommand<string, Unit> ToogleHealthStatesAxis { get; }

    #endregion

    #region Constructors

    public OverviewTabViewModel()
    {
        ToogleHealthStatesAxis = ReactiveCommand.Create<string>(OnToogleHealthStatesAxis);
    }

    #endregion

    #region Public Methods
    public void RefreshCharts()
    {
        XHealthStates.First().MinLimit = null;
        XHealthStates.First().MaxLimit = null;
        YHealthStates.First().MinLimit = null;
        YHealthStates.First().MaxLimit = null;
        
        XIncidence.First().MinLimit = null;
        XIncidence.First().MaxLimit = null;
        YIncidence.First().MinLimit = null;
        YIncidence.First().MaxLimit = null;
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
                Name = "HeavilyInfected",
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

        IncidenceSeries = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(SKColors.Yellow, 3),
                GeometryFill = new SolidColorPaint(SKColors.Yellow),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = IncidenceData,
                Name = "",
                Fill = null
            }
        };
        
        RefreshCharts();
    }

    #endregion

    #region Private Methods

    private void OnToogleHealthStatesAxis(string parameter)
    {
        var p = Convert.ToInt32(parameter);
        HealthStatesSeries[p].IsVisible = !HealthStatesSeries[p].IsVisible;
    }

    #endregion
}