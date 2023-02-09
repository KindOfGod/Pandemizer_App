namespace Pandemizer.Services.PandemicEngine.DataModel;

/// <summary>
/// Contains all basic information about a Sim.
/// </summary>
public class SimInfo
{
    public int Seed { get; set; } = 1;
    public int Iteration { get; set; } = 1;
    public string Name { get; set; } = "Unknown";
}