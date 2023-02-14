using System.Collections.ObjectModel;
using System.Linq;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
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
    
    private static readonly Axis _iterationAxis = new()
    {
        Name = "Iteration",
        NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 1
    };

    private static readonly Axis _utilizationAxis = new()
    {
        Name = "Utilization in %",
        NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinStep = 0.01
    };

    private static readonly PolarAxis _ageAxis = new()
    {
        LabelsRotation = LiveCharts.TangentAngle,
        LabelsBackground = LvcColor.Empty,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        Labels = new[] {"Child", "YoungAdult", "Adult", "Pensioner"}
    };

    #endregion
    
    #region Properties

    public ObservableCollection<ObservablePoint> HospitalizedData { get; set; } = new();
    public ObservableCollection<int> AgeData { get; set; } = new(){0,0,0,0};
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

    #endregion

    #region Axis

    public Axis[] XHospitalized { get; set; } = {_iterationAxis};
    public Axis[] YHospitalized { get; set; } = {_utilizationAxis};
    public PolarAxis[] AgeAxis { get; set; } = { _ageAxis};

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
            new PolarLineSeries<int>
            {
                Values = AgeData,
                LineSmoothness = 0,
                GeometrySize= 0,
                DataLabelsPaint = new SolidColorPaint(SKColors.Transparent),
                Fill = new SolidColorPaint(SKColors.Blue.WithAlpha(90))
            }
        };
        
        RefreshCharts();
    }

    #endregion
}