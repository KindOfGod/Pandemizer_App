using System.Collections.ObjectModel;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using ReactiveUI;
using SkiaSharp;

namespace Pandemizer.ViewModels.Play.SimPage;

public class OverviewTabViewModel : ViewModelBase
{
    #region Fields

    public ISeries[] _incidenceSeries;
    public ObservableCollection<ObservablePoint> IncidenceData { get; set; } = new();

    private static Axis _iterationAxis = new()
    {
        Name = "Iteration",
        NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinLimit = 0,
        MinStep = 1
    };

    private static Axis _poeopleAxis = new()
    {
        Name = "People",
        NamePadding = new LiveChartsCore.Drawing.Padding(0, 5),
        NameTextSize = 15,
        LabelsPaint = new SolidColorPaint(SKColors.White),
        NamePaint = new SolidColorPaint(SKColors.White),
        MinLimit = 0,
        MinStep = 1
    };
        

    #endregion
    
    #region Properties
    public ISeries[] IncidenceSeries 
    {
        get => _incidenceSeries;
        set => this.RaiseAndSetIfChanged(ref _incidenceSeries, value);
    }

    #endregion

    #region Axes
    public Axis[] XIncidence { get; set; } = {_iterationAxis};
    public Axis[] YIncidence { get; set; } = {_poeopleAxis};

    #endregion

    #region Public Methods

    public void Init()
    {
        IncidenceSeries = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Stroke = new SolidColorPaint(SKColors.Yellow, 3),
                GeometryFill = new SolidColorPaint(SKColors.Yellow),
                GeometryStroke = null,
                GeometrySize = 15,
                Values = IncidenceData,
                Name = "",
                Fill = null
            }
        };
    }

    #endregion
}