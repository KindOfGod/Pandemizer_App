using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using ReactiveUI;

namespace Pandemizer.ViewModels.Play.SimPage;

public class PopulationTabViewModel : ViewModelBase
{
    #region Fields

    private ObservableCollection<Pop> _popIndex = new();

    #endregion

    #region Proporties

    public ObservableCollection<Pop> PopIndex
    {
        get => _popIndex;
        set => this.RaiseAndSetIfChanged(ref _popIndex, value);
    }

    #endregion

    #region Constructors

    public PopulationTabViewModel()
    {
        
    }

    #endregion

    #region Public Methods

    public void RefreshData(Dictionary<uint, uint> popIndex)
    {
        var popList = new List<Pop>();
        
        //Convert Data from Dictionary to ObservableCollection of Pops
        foreach (var (pop, count) in popIndex)
            popList.Add(new Pop(pop, count));

        PopIndex = new ObservableCollection<Pop>(popList);
    }

    #endregion
}