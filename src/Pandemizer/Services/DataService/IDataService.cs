using System.Collections.Generic;
using System.Threading.Tasks;
using Pandemizer.Services.DataService.Enums;
using Pandemizer.Services.PandemicEngine.DataModel;

namespace Pandemizer.Services.DataService;

public interface IDataService
{
    public Task<SaveResult> SaveSim(Sim sim);
    public Task<Sim?> ReadSim(string simName);
    public Task<List<Sim?>> ReadAllSims();
    public Task DeleteSim(string simName);
    public Task<SaveResult> SaveVirus(Virus virus);
    public Task<Virus?> ReadVirus(string virusName);
    public Task<List<Virus?>> ReadAllViruses();
    public Task DeleteVirus(string virusName);
}