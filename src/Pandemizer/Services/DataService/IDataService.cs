using System.Threading.Tasks;
using Pandemizer.Services.DataService.Enums;
using Pandemizer.Services.SimulationEngine.Datamodel;

namespace Pandemizer.Services.DataService;

public interface IDataService
{
    public Task<SaveResult> CreateNewSimSave(Sim sim);
}