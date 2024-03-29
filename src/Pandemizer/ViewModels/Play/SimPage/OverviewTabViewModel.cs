﻿using System;
using System.Collections.Generic;
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

    private ISeries[]? _healthStateSeries;
    private ISeries[]? _ratesSeries;
    
    private IEnumerable<ISeries>? _healthStateDistributionSeries;
    
    private static readonly Axis _iterationAxis = new()
    {
        Name = "Iteration",
        NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 1,
        Labeler = x => $"{ApplicationHelper.IntToFormattedNum((int)x)}"
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
    
    public ObservableCollection<ObservablePoint> HealthyData { get; set; } = new();
    public ObservableCollection<ObservablePoint> InfectedData { get; set; } = new();
    public ObservableCollection<ObservablePoint> DeadData { get; set; } = new();
    public ObservableCollection<ObservablePoint> ImmuneData { get; set; } = new();
    public ObservableCollection<ObservablePoint> ImmuneRateData { get; set; } = new();
    public ObservableCollection<ObservablePoint> IncidenceData { get; set; } = new();
    public ObservableCollection<ObservablePoint> DeathRateData { get; set; } = new();
    public ObservableCollection<ObservableValue> HealthStateDistributionData { get; set; } = new()
    {
        new ObservableValue(),
        new ObservableValue(),
        new ObservableValue(),
        new ObservableValue()
    };

    public ObservableCollection<ObservableValue> IncidenceRateChangedData { get; set; } = new()
    {
        new ObservableValue(0),
        new ObservableValue(0),
        new ObservableValue(0),
        new ObservableValue(0)
    };
    
    public ObservableCollection<ObservableValue> ImmuneRateChangedData { get; set; } = new()
    {
        new ObservableValue(0),
        new ObservableValue(0),
        new ObservableValue(0),
        new ObservableValue(0)
    };
    
    public ObservableCollection<ObservableValue> DeathRateChangedData { get; set; } = new()
    {
        new ObservableValue(0),
        new ObservableValue(0),
        new ObservableValue(0),
        new ObservableValue(0)
    };
    
    public SolidColorPaint CustomLegend { get; set; } = new() {Color = new SKColor(255, 255, 255)};
    public ISeries[]? HealthStatesSeries 
    {
        get => _healthStateSeries;
        set => this.RaiseAndSetIfChanged(ref _healthStateSeries, value);
    }

    public ISeries[]? RatesSeries
    {
        get => _ratesSeries;
        set => this.RaiseAndSetIfChanged(ref _ratesSeries, value);
    }
    public IEnumerable<ISeries>? HealthStateDistributionSeries 
    {
        get => _healthStateDistributionSeries;
        set => this.RaiseAndSetIfChanged(ref _healthStateDistributionSeries, value);
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

        UpdateChangedRates();
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
        
        string[] states = {"Healthy", "Infected", "Immune", "Dead"};
        var stateColors = new List<SolidColorPaint>()
        {
            new(ApplicationColors.HealthyColor), 
            new(ApplicationColors.InfectedColor), 
            new(ApplicationColors.ImmuneColor), 
            new(ApplicationColors.DeadColor)
        };
        var stateIndex = 0;
        HealthStateDistributionSeries = HealthStateDistributionData.AsLiveChartsPieSeries((_, series) =>
        {
            series.Name = $"{states[stateIndex]}";
            series.Fill = stateColors[stateIndex];
            series.Stroke = new SolidColorPaint(new SKColor(255,255,255));
            series.Stroke.StrokeThickness = 3;
            stateIndex = stateIndex == 3 ? stateIndex = 0 : stateIndex + 1;
        });
        
        RefreshCharts();
    }

    #endregion

    #region Private Methods

    private void OnToggleHealthStatesAxis(string parameter)
    {
        var p = Convert.ToInt32(parameter);

        if (HealthStatesSeries == null) 
            return;
        
        HealthStatesSeries[p].IsVisible = !HealthStatesSeries[p].IsVisible;
    }
    
    private void OnToggleRatesAxis(string parameter)
    {
        var p = Convert.ToInt32(parameter);
        
        if (RatesSeries != null)
            RatesSeries[p].IsVisible = !RatesSeries[p].IsVisible;
    }

    private void UpdateChangedRates()
    {
        var state = HealthyData.Count - 1;

        if (state > 1)
        {
            ImmuneRateChangedData[0].Value = 1 - ImmuneRateData[^1].Y / ImmuneRateData[^2].Y;
            IncidenceRateChangedData[0].Value =  1 - IncidenceData[^1].Y / IncidenceData[^2].Y;
            DeathRateChangedData[0].Value =  1 - DeathRateData[^1].Y / DeathRateData[^2].Y;
        }
        
        if (state > 5)
        {
            ImmuneRateChangedData[1].Value = 1 - ImmuneRateData[^1].Y / ImmuneRateData[^6].Y;
            IncidenceRateChangedData[1].Value = 1 - IncidenceData[^1].Y / IncidenceData[^6].Y;
            DeathRateChangedData[1].Value = 1 - DeathRateData[^1].Y / DeathRateData[^6].Y;
        }
        
        if (state > 10)
        {
            ImmuneRateChangedData[2].Value = 1 - ImmuneRateData[^1].Y / ImmuneRateData[^11].Y;
            IncidenceRateChangedData[2].Value = 1 - IncidenceData[^1].Y / IncidenceData[^11].Y;
            DeathRateChangedData[2].Value = 1 - DeathRateData[^1].Y / DeathRateData[^11].Y;
        }
        
        if (state > 25)
        {
            ImmuneRateChangedData[3].Value = 1 - ImmuneRateData[^1].Y / ImmuneRateData[^26].Y;
            IncidenceRateChangedData[3].Value = 1 - IncidenceData[^1].Y / IncidenceData[^26].Y;
            DeathRateChangedData[3].Value = 1 - DeathRateData[^1].Y / DeathRateData[^26].Y;
        }

        for (var i = 0; i < 4; i++)
        {
            var immuneValue = ImmuneRateChangedData[i].Value > 1 ? ImmuneRateChangedData[i].Value! : -ImmuneRateChangedData[i].Value;
            ImmuneRateChangedData[i].Value = Math.Round(immuneValue!.Value * 100, 1);
            
            var infectedValue = IncidenceRateChangedData[i].Value > 1 ? IncidenceRateChangedData[i].Value! : -IncidenceRateChangedData[i].Value;
            IncidenceRateChangedData[i].Value = Math.Round(infectedValue!.Value * 100, 1);
            
            var deadValue = DeathRateChangedData[i].Value > 1 ? DeathRateChangedData[i].Value! : -DeathRateChangedData[i].Value;
            DeathRateChangedData[i].Value = Math.Round(deadValue!.Value * 100, 1);
            
        }
    }
    
    #endregion
}