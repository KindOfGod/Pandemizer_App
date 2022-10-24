using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.Tasks;
using Pandemizer.Services.DataService.Enums;
using Pandemizer.Services.SimulationEngine.Datamodel;

namespace Pandemizer.Services.DataService;

public class DataServiceImpl : IDataService
{
    #region Fields

    private const string GamesDirectory = "Games";

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

            var zipContent = new MemoryStream();
            var archive = new ZipArchive(zipContent, ZipArchiveMode.Create);
            
            AddZipEntry("SimSettings.json", JsonSerializer.SerializeToUtf8Bytes(sim.SimSettings), archive);
            AddZipEntry("PeopleBase.json", JsonSerializer.SerializeToUtf8Bytes(sim.PeopleBase), archive);

            await File.WriteAllBytesAsync(file, zipContent.ToArray());
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
    
    private static void AddZipEntry(string fileName, byte[] fileContent,ZipArchive archive)
    {
        using (var stream = archive.CreateEntry(fileName).Open())
        {
            stream.Write(fileContent, 0, fileContent.Length);
        }
    }

    #endregion
}