using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pandemizer.Services.DataService.Enums;
using Pandemizer.Services.PandemicEngine;
using Pandemizer.Services.PandemicEngine.DataModel;

namespace Pandemizer.Services.DataService;

/// <summary>
/// This DataService uses xml files to store simulations.
/// </summary>
public class DataServiceImpl : IDataService
{
    #region Fields

    private const string GamesDirectory = "Games";
    private const string VirusesDirectory = "Viruses";

    #endregion
    
    #region Public Methods
    
    /// <summary>
    /// Save a sim to the default game folder.
    /// </summary>
    public async Task<SaveResult> SaveSim(Sim sim)
    {
        SaveResult res;
        
        try
        {
            res = await Task.Run( async () =>
            {
                if (!Directory.Exists(GamesDirectory))
                    Directory.CreateDirectory(GamesDirectory);
                
                var gamePath = Path.Combine(GamesDirectory, sim.SimInfo.Name);
                
                if (!Directory.Exists(gamePath))
                    Directory.CreateDirectory(gamePath);
                
                await File.WriteAllTextAsync(Path.Combine(gamePath, "SimInfo.json"), JsonConvert.SerializeObject(sim.SimInfo, Formatting.Indented));
                await File.WriteAllTextAsync(Path.Combine(gamePath, "SimSettings.json"), JsonConvert.SerializeObject(sim.SimSettings, Formatting.Indented));
                
                return SaveResult.Successful;
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
    
    /// <summary>
    /// Read a sim from the default game folder.
    /// </summary>
    public async Task<Sim?> ReadSim(string simName)
    {
        try
        {
            var sim = await Task.Run(async () =>
            {
                if (!Directory.Exists(GamesDirectory))
                    Directory.CreateDirectory(GamesDirectory);

                var gamePath = Path.Combine(GamesDirectory, simName);
                
                if (!Directory.Exists(gamePath))
                    return null;

                SimInfo? simInfo = null;
                SimSettings? simSettings = null;

                var infPath = Path.Combine(gamePath, "SimInfo.json");

                if (File.Exists(infPath))
                    simInfo = JsonConvert.DeserializeObject<SimInfo>(await File.ReadAllTextAsync(infPath));
                
                var setPath = Path.Combine(gamePath, "SimSettings.json");
                if (File.Exists(setPath))
                    simSettings = JsonConvert.DeserializeObject<SimSettings>(await File.ReadAllTextAsync(setPath));
                
                if(simInfo != null && simSettings != null)
                    return SimEngine.LoadSim(simInfo, simSettings);
                
                return null;
            });

            return sim;
        }
        catch
        {
            // ignored
        }

        return null;
    }
    
    /// <summary>
    /// Reads all non archived simulations
    /// </summary>
    public async Task<List<Sim?>> ReadAllSims()
    {
        var sims = new List<Sim?>();
        
        if (!Directory.Exists(GamesDirectory))
            Directory.CreateDirectory(GamesDirectory);
        
        var subDirs = Directory.GetDirectories(GamesDirectory).Select(Path.GetFileName).ToArray();

        foreach (var dir in subDirs)
        {
            if(dir != null)
                sims.Add(await ReadSim(dir));
        }

        return sims;
    }

    /// <summary>
    /// Deletes a sim with given name
    /// </summary>
    public async Task DeleteSim(string simName)
    {
        await Task.Run(() =>
        {
            if (!Directory.Exists(GamesDirectory))
                Directory.CreateDirectory(GamesDirectory);
        
            var gamePath = Path.Combine(GamesDirectory, simName);
        
            if(!Directory.Exists(gamePath))
                return;
        
            Directory.Delete(gamePath, true);
        });
    }
    
    /// <summary>
    /// Save a virus to the default virus folder.
    /// </summary>
    public async Task<SaveResult> SaveVirus(Virus virus)
    {
        SaveResult res;
        
        try
        {
            res = await Task.Run( async () =>
            {
                if (!Directory.Exists(VirusesDirectory))
                    Directory.CreateDirectory(VirusesDirectory);
                
                var virusPath = Path.Combine(VirusesDirectory, virus.Name);
                
                if (!Directory.Exists(virusPath))
                    Directory.CreateDirectory(virusPath);
                
                await File.WriteAllTextAsync(Path.Combine(virusPath, $"{virus.Name}.json"), JsonConvert.SerializeObject(virus, Formatting.Indented));

                return SaveResult.Successful;
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
    
    /// <summary>
    /// Reads a virus from the default virus folder.
    /// </summary>
    public async Task<Virus?> ReadVirus(string virusName)
    {
        try
        {
            var virus = await Task.Run(async () =>
            {
                if (!Directory.Exists(VirusesDirectory))
                    Directory.CreateDirectory(VirusesDirectory);

                var virusPath = Path.Combine(VirusesDirectory, $"{virusName}\\{virusName}.json");
                
                return !File.Exists(virusPath) ? null : JsonConvert.DeserializeObject<Virus>(await File.ReadAllTextAsync(virusPath));
            });

            return virus;
        }
        catch
        {
            // ignored
        }

        return null;
    }
    
    /// <summary>
    /// Reads all viruses
    /// </summary>
    public async Task<List<Virus?>> ReadAllViruses()
    {
        var viruses = new List<Virus?>();
        
        if (!Directory.Exists(VirusesDirectory))
            Directory.CreateDirectory(VirusesDirectory);
        
        var subDirs = Directory.GetDirectories(VirusesDirectory).Select(Path.GetFileName).ToArray();

        foreach (var dir in subDirs)
        {
            if(dir != null)
                viruses.Add(await ReadVirus(dir));
        }

        return viruses;
    }
    
    /// <summary>
    /// Deletes a virus with given name
    /// </summary>
    public async Task DeleteVirus(string virusName)
    {
        await Task.Run(() =>
        {
            if (!Directory.Exists(VirusesDirectory))
                Directory.CreateDirectory(VirusesDirectory);
        
            var virusPath = Path.Combine(VirusesDirectory, virusName);
        
            if(!Directory.Exists(virusPath))
                return;
        
            Directory.Delete(virusPath, true);
        });
    }

    #endregion
}