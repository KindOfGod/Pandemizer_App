namespace Pandemizer.Services.PandemicEngine.DataModel;

/// <summary>
/// Contains all basic information about a Sim.
/// </summary>
public class SimInfo
{
    public int Seed { get; set; } = 1;
    public int Iteration { get; set; } = 1;
    public string Name { get; set; } = "Unknown";
    
    public string? Healthy { get; set; } = "0";
    public string? Infected { get; set; } = "0";
    public string? Immune { get; set; } = "0";
    public string? Dead { get; set; } = "0";
}