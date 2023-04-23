using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using DynamicData;
using Newtonsoft.Json;
using Pandemizer.Services;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;
using ReactiveUI;

namespace Pandemizer.ViewModels.Viruses;

public class VirusesPageViewModel : ViewModelBase
{
    #region Fields
    
    private Virus _selectedVirus = new ();

    private ObservableCollection<Virus> _virusList = new();

    #endregion
    
    #region Properties

    public Virus SelectedVirus
    {
        get => _selectedVirus;
        set => this.RaiseAndSetIfChanged(ref _selectedVirus, value);
    }

    public ObservableCollection<Virus> VirusList
    {
        get => _virusList;
        set => this.RaiseAndSetIfChanged(ref _virusList, value);
    }
    
    public Age[] AgeValues => Enum.GetValues(typeof(Age)).Cast<Age>().Where(a => a != Age.Compare).ToArray();
    public StateOfLife[] InfectionSeverityValues => Enum.GetValues(typeof(StateOfLife)).Cast<StateOfLife>()
        .Where(a => a is not (StateOfLife.Healthy or StateOfLife.Immune or StateOfLife.Dead or StateOfLife.Compare)).ToArray();

    #endregion

    #region Commands

    public ReactiveCommand<Unit, Unit> CreateVirusCommand { get; }
    
    public ReactiveCommand<Unit, Unit> DuplicateVirusCommand { get; }
    
    public ReactiveCommand<Unit, Unit> DeleteVirusCommand { get; }

    #endregion

    #region Constructors

    public VirusesPageViewModel()
    {
        CreateVirusCommand = ReactiveCommand.Create(OnCreateVirusCommand);
        DuplicateVirusCommand = ReactiveCommand.Create(OnDuplicateVirusCommand);
        DeleteVirusCommand = ReactiveCommand.Create(OnDeleteVirusCommand);

        LoadViruses();
    }

    #endregion

    #region Private Methods

    private async void LoadViruses()
    {
        var viruses = await ApplicationService.DataService.ReadAllViruses();

        foreach (var virus in viruses)
        {
            if(virus == null)
                continue;
            
            VirusList.Add(virus);
        }
        
        if (VirusList.Count > 0)
            SelectedVirus = VirusList[0];
        else
            OnCreateVirusCommand();
    }

    private async void OnCreateVirusCommand()
    {
        var virus = new Virus();
        _ = await ApplicationService.DataService.SaveVirus(virus);
        
        VirusList.Add(virus);
        SelectedVirus = virus;
    }
    
    private async void OnDuplicateVirusCommand()
    {
        var json = JsonConvert.SerializeObject(_selectedVirus, Formatting.Indented);
        var copy = JsonConvert.DeserializeObject<Virus>(json);

        copy!.Name += "_copy";
        _ = await ApplicationService.DataService.SaveVirus(copy);

        VirusList.Add(copy);
    }
    
    private async void OnDeleteVirusCommand()
    {
        await ApplicationService.DataService.DeleteVirus(_selectedVirus.Name);
        VirusList.Remove(_selectedVirus);
        
        if (VirusList.Count > 0)
            SelectedVirus = VirusList[0];
        else
            OnCreateVirusCommand();
    }
    
    #endregion
}