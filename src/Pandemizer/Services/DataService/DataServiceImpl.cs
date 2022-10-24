using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pandemizer.Services.DataService.Enums;
using Pandemizer.Services.SimulationEngine.Datamodel;

namespace Pandemizer.Services.DataService;

public class DataServiceImpl : IDataService
{
    #region Fields

    private const string GamesDirectory = "Games";
    private const Formatting JsonFormat = Formatting.Indented;

    #endregion
    
    #region XML

    

    #endregion

    #region JSON

    public async Task<SaveResult> CreateNewSimSave(Sim sim)
    {
        try
        {
            var file = Path.Combine(GamesDirectory, sim.Name) + ".zip";
      
            if (!Directory.Exists(GamesDirectory))
                Directory.CreateDirectory(GamesDirectory);

            if (File.Exists(file))
                return SaveResult.FileNameAlreadyExists;
            
            using(var outStream = new MemoryStream())
            using (var zip = new ZipArchive(outStream, ZipArchiveMode.Create, true))
            {
                var simSettings = zip.CreateEntry("SimSettings.json", CompressionLevel.Fastest);

                var content = JsonConvert.SerializeObject(sim.SimSettings, JsonFormat);
                
                await using (var entryStream = simSettings.Open())
                await using (var fileToCompressStream = new MemoryStream(Encoding.ASCII.GetBytes(content)))
                {
                    await fileToCompressStream.CopyToAsync(entryStream);
                }
            }
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
    }

    #endregion
}