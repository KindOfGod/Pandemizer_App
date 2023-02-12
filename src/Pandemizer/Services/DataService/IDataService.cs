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
}