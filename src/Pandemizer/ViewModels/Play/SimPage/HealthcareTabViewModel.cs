using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
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
    private IEnumerable<ISeries>? _heavilyInfectedSeries;

    private static readonly Axis _iterationAxis = new()
    {
        Name = "Iteration",
        NamePadding = new Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 1,
        Labeler = x => $"{ApplicationHelper.DoubleToFormattedNum(x)}"
    };

    private static readonly Axis _utilizationAxis = new()
    {
        Name = "Utilization",
        NamePadding = new Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 0.01,
        Labeler = x => $"{Math.Round(x, 2)} %"
    };

    private static readonly Axis _ageXAxis = new()
    {
        LabelsRotation = LiveCharts.TangentAngle,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        TextSize = 18
    };

    private static readonly Axis _ageYAxes = new()
    {
        LabelsPaint = new SolidColorPaint(SKColors.White),
        Labels = new List<string>{"Children", "YoungAdults", "Adults", "Pensioner"}
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
    
    public ObservableCollection<ObservableValue> HeavilyInfectedDistributionData { get; set; } = new()
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
    
    public IEnumerable<ISeries>? HeavilyInfectedSeries 
    {
        get => _heavilyInfectedSeries;
        set => this.RaiseAndSetIfChanged(ref _heavilyInfectedSeries, value);
    }
    #endregion

    #region Axis

    public Axis[] XHospitalized { get; set; } = {_iterationAxis};
    public Axis[] YHospitalized { get; set; } = {_utilizationAxis};
    public Axis[] XAge { get; set; } = {_ageXAxis};
    public Axis[] YAge { get; set; } = {_ageYAxes};

    #endregion
    
    #region Constructor

    #endregion

    #region Public Methods

    public void RefreshCharts()
    {
        XAge.First().MinLimit = null;
        XAge.First().MaxLimit = null;
        YAge.First().MinLimit = null;
        YAge.First().MaxLimit = null;
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
                Stroke = new SolidColorPaint(ApplicationColors.HospitalColor, 3),
                GeometryFill = new SolidColorPaint(ApplicationColors.HospitalColor),
                GeometryStroke = null,
                GeometrySize = 5,
                Values = HospitalizedData,
                Name = "Hospitalized",
                Fill = null
            }
        };
        
        AgeSeries = new ISeries[]
        {
            new RowSeries<ObservableValue>
            {
                Values = AgeData,
                Stroke = new SolidColorPaint(ApplicationColors.HospitalColor, 3),
                Fill = new SolidColorPaint(new SKColor(ApplicationColors.HospitalColor.Red, ApplicationColors.HospitalColor.Green, ApplicationColors.HospitalColor.Blue, 69)),
                DataLabelsPaint = new SolidColorPaint(new SKColor(245, 245, 245)),
                DataLabelsPosition = DataLabelsPosition.End,
                DataLabelsFormatter = p => $"{Math.Round(p.PrimaryValue, 2)}",
                TooltipLabelFormatter = p => $"{Math.Round(p.PrimaryValue, 2)}%"
            }
        };

        var preExistingCondition = "No";
        PreExistingConditionSeries = PreExistingConditionData.AsLiveChartsPieSeries((_, series) =>
        {
            series.Name = $"{preExistingCondition}";
            series.Fill = preExistingCondition == "No" ? new SolidColorPaint(ApplicationColors.HealthyColor) : new SolidColorPaint(ApplicationColors.HospitalColor);
            series.DataLabelsPaint = new SolidColorPaint(new SKColor(255,255,255));
            series.DataLabelsPosition = PolarLabelsPosition.Middle;
            series.Stroke = new SolidColorPaint(new SKColor(255,255,255));
            series.Stroke.StrokeThickness = 3;
            series.DataLabelsFormatter = p => $"{p.StackedValue!.Share:P2}";
            
            preExistingCondition = preExistingCondition == "No" ? "Yes" : "No";
        });
        
        var heavilyInfected = "No";
        HeavilyInfectedSeries = HeavilyInfectedDistributionData.AsLiveChartsPieSeries((_, series) =>
        {
            series.Name = $"{heavilyInfected}";
            series.Fill = heavilyInfected == "No" ? new SolidColorPaint(ApplicationColors.InfectedColor) : new SolidColorPaint(ApplicationColors.HeavilyInfectedColor);
            series.DataLabelsFormatter = p => $"{p.StackedValue!.Share:P2}";
            series.DataLabelsPaint = new SolidColorPaint(new SKColor(255,255,255));
            series.DataLabelsPosition = PolarLabelsPosition.Middle;
            series.Stroke = new SolidColorPaint(new SKColor(255,255,255));
            series.Stroke.StrokeThickness = 3;
            
            heavilyInfected = heavilyInfected == "No" ? "Yes" : "No";
        });

        RefreshCharts();
    }

    #endregion
}