using System.IO;
using System.Threading.Tasks;
using Ionic.Zip;
using Ionic.Zlib;
using Newtonsoft.Json;
using Pandemizer.Services.DataService.Enums;

namespace Pandemizer.Services.DataService;

public class DataServiceImpl : IDataService
{
    #region Fields

    private const string GamesDirectory = "Games";

    #endregion
    
    #region XML

    

    #endregion

    #region JSON

    /*public async Task<SaveResult> SaveNewSim(Sim sim)
    {
        SaveResult res;
        
        try
        {
            res = await Task.Run(async () =>
            {
                var file = Path.Combine(GamesDirectory, sim.Name) + ".zip";

                if (!Directory.Exists(GamesDirectory))
                    Directory.CreateDirectory(GamesDirectory);

                if (File.Exists(file))
                    return SaveResult.FileNameAlreadyExists;
                
                using (var zip = new ZipFile(file))
                {
                    zip.CompressionLevel = CompressionLevel.BestSpeed;
                    zip.AddEntry("SimSettings.json", JsonConvert.SerializeObject(sim.SimSettings));
                    zip.AddEntry("PeopleBase.json", JsonConvert.SerializeObject(sim.PeopleBase));
                    zip.Save();
                }

                return await AppendToSimSave(sim);
            });
        }
        catch (DirectoryNotFoundException)
        {
            res = SaveResult.InvalidDirectory;
        }
        catch
        {
            res = SaveResult.Failed;
        }
        
        return res;
    }

    public async Task<SaveResult> AppendToSimSave(Sim sim)
    {
        try
        {
            await Task.Run(() =>
            {
                var file = Path.Combine(GamesDirectory, sim.Name) + ".zip";

                using (var zip = ZipFile.Read(file))
                {
                    zip.CompressionLevel = CompressionLevel.BestSpeed;
                    zip.AddEntry($"PeopleStates_{sim.Iteration}.json",
                        JsonConvert.SerializeObject(sim.PeopleStates[sim.Iteration]));
                    zip.AddEntry($"SimStates_{sim.Iteration}.json",
                        JsonConvert.SerializeObject(sim.SimStates[sim.Iteration]));
                    zip.Save();
                }
            });

        }
        catch (DirectoryNotFoundException)
        {
            return SaveResult.InvalidDirectory;
        }
        catch
        {
            return SaveResult.Failed;
        }
        
        return SaveResult.Successful;
    }*/

    #endregion
}