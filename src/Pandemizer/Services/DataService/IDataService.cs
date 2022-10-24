using System.Threading.Tasks;
using Pandemizer.Services.DataService.Enums;
using Pandemizer.Services.SimulationEngine.Datamodel;

namespace Pandemizer.Services.DataService;

public interface IDataService
{
    Task<SaveResult> AppendDataAsync(string filename, Sim game);
}