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

    #endregion
    
    #region XML

    

    #endregion

    #region JSON

    public async Task<SaveResult> AppendDataAsync(string filename, Sim game)
    {
        var file = Path.Combine(GamesDirectory, filename);

        if (!Directory.Exists(GamesDirectory))
            Directory.CreateDirectory(GamesDirectory);
        
        if(!File.Exists(file))
            File.Create(file);
        
        var json = JsonConvert.SerializeObject(game, Formatting.None);
        var bytes = Encoding.ASCII.GetBytes(json);

        await using (var fileStream = File.Open(file, FileMode.Open))
        await using (var gzip = new GZipStream(fileStream, CompressionMode.Compress, false))
        {
            await gzip.WriteAsync(bytes);
        }

        return SaveResult.Successful;
    }

    #endregion
}