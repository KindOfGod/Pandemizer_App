﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using DynamicData;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Pandemizer.Services;
using ReactiveUI;
using SkiaSharp;

namespace Pandemizer.ViewModels.Play.SimPage;

public class HealthcareTabViewModel : ViewModelBase
{
    #region Fields

    private ISeries[]? _hospitalizedSeries;
    private ISeries[]? _ageSeries;
    private IEnumerable<ISeries>? _preExistingConditionSeries;

    private static readonly Axis _iterationAxis = new()
    {
        Name = "Iteration",
        NamePadding = new Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 1
    };

    private static readonly Axis _utilizationAxis = new()
    {
        Name = "Utilization in %",
        NamePadding = new Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 0.01
    };

    private static readonly PolarAxis _ageAngleAxis = new()
    {
        LabelsRotation = LiveCharts.TangentAngle,
        LabelsBackground = LvcColor.Empty,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        TextSize = 18,
        LabelsVerticalAlignment = Align.End,
        Labels = new[] {"Child", "YoungAdult", "Adult", "Pensioner"}
    };

    private static readonly PolarAxis _ageRadialAxes = new()
    {
        LabelsPaint = new SolidColorPaint(SKColors.Transparent),
        LabelsBackground = LvcColor.Empty
    };
    
    #endregion
    
    #region Properties
    
    public SolidColorPaint CustomLegend { get; set; } = new() {Color = new SKColor(255, 255, 255)};
    public ObservableCollection<ObservablePoint> HospitalizedData { get; set; } = new();

    public ObservableCollection<ObservableValue> AgeData { get; set; } = new()
    {
        new ObservableValue(),
        new ObservableValue(),
        new ObservableValue(),
        new ObservableValue()
    };
    
    public ObservableCollection<ObservableValue> PreExistingConditionData { get; set; } = new()
    {
        new ObservableValue(),
        new ObservableValue()
    };
    
    public ISeries[]? HospitalizedSeries 
    {
        get => _hospitalizedSeries;
        set => this.RaiseAndSetIfChanged(ref _hospitalizedSeries, value);
    }
    
    public ISeries[]? AgeSeries 
    {
        get => _ageSeries;
        set => this.RaiseAndSetIfChanged(ref _ageSeries, value);
    }
    
    public IEnumerable<ISeries>? PreExistingConditionSeries 
    {
        get => _preExistingConditionSeries;
        set => this.RaiseAndSetIfChanged(ref _preExistingConditionSeries, value);
    }
    #endregion

    #region Axis

    public Axis[] XHospitalized { get; set; } = {_iterationAxis};
    public Axis[] YHospitalized { get; set; } = {_utilizationAxis};
    public PolarAxis[] AgeAngleAxis { get; set; } = {_ageAngleAxis};
    public PolarAxis[] AgeRadialAxis { get; set; } = {_ageRadialAxes};

    #endregion
    
    #region Constructor

    public HealthcareTabViewModel()
    {
        
    }

    #endregion

    #region Public Methods

    public void RefreshCharts()
    {
        XHospitalized.First().MinLimit = null;
        XHospitalized.First().MaxLimit = null;
        YHospitalized.First().MinLimit = null;
        YHospitalized.First().MaxLimit = null;
    }
    
    public void Init()
    {
        HospitalizedSeries = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(ApplicationColors.IterationColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.IterationColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = HospitalizedData,
                Name = "Hospitalized",
                Fill = null
            }
        };
        
        AgeSeries = new ISeries[]
        {
            new PolarLineSeries<ObservableValue>
            {
                Values = AgeData,
                LineSmoothness = 0.5,
                GeometrySize = 0,
                GeometryFill = new SolidColorPaint(ApplicationColors.IterationColor),
                GeometryStroke = new SolidColorPaint(ApplicationColors.IterationColor),
                Stroke = new SolidColorPaint(ApplicationColors.IterationColor, 3),
                Fill = new SolidColorPaint(new SKColor(ApplicationColors.IterationColor.Red, ApplicationColors.IterationColor.Green,ApplicationColors.IterationColor.Blue, 69)),
                Name = "AgeDistribution"
            }
        };

        var name = "Not Conditioned";
        PreExistingConditionSeries = PreExistingConditionData?.AsLiveChartsPieSeries((value, series) =>
        {
            series.Name = $"{name}";
            name = name == "Not Conditioned" ? "Conditioned" : "Not Conditioned";
            series.Fill = name == "Not Conditioned" ? new SolidColorPaint(ApplicationColors.HealthyColor) : new SolidColorPaint(ApplicationColors.IterationColor);
            series.DataLabelsPaint = new SolidColorPaint(new SKColor(255,255,255));
            series.DataLabelsPosition = PolarLabelsPosition.Outer;
            series.DataLabelsFormatter = p => $"{p.StackedValue!.Share:P2}";
        });

        RefreshCharts();
    }

    #endregion
}