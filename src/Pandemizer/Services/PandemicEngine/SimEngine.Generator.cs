using System.Collections.Generic;
using Pandemizer.Services.PandemicEngine.DataModel;

namespace Pandemizer.Services.PandemicEngine;

public static partial class SimEngine
{
    #region Private Methods

    /// <summary>
    /// Generate a Initial Simulation State with valid SimSettings
    /// </summary>
    private static SimState GenerateInitialSimState(SimSettings settings)
    {
        //generate age groups
        var basePopIndex = new Dictionary<uint, uint>
        {
            {(uint) Age.Child, (uint) (settings.Scope * settings.AgeProportionOfChildren)},
            {(uint) Age.YoungAdult, (uint) (settings.Scope * settings.AgeProportionOfYoungAdults)},
            {(uint)Age.Adult, (uint)(settings.Scope * settings.AgeProportionOfAdults)},
            {(uint)Age.Pensioner, (uint)(settings.Scope * settings.AgeProportionOfPensioner)}
        };
        
        //add health state attribute
        basePopIndex = AddAttributeToAllByPercentage(basePopIndex, settings.InitialProportionOfInfected, (uint)settings.HealthIllnessSeverity, (uint)StateOfLife.Healthy);
        basePopIndex = AddAttributeToAllByPercentage(basePopIndex, settings.InitialProportionOfPreConditioned, (uint)PreExistingCondition.True, (uint)PreExistingCondition.False);

        var popIndex = new Dictionary<uint, uint>();
        
        foreach (var (pop, count) in basePopIndex)
            popIndex.Add(pop.OverrideIsHospitalized(IsHospitalized.False), count); //append new attributes here

        var state = new SimState()
        {
            PopIndex = popIndex,
            Healthy = settings.Scope - (uint)(settings.Scope * settings.InitialProportionOfInfected)
        };
        
        return state;
    }

    /// <summary>
    /// Adds a Attribute to a percentage popIndex. Uses alternative value for rest.
    /// </summary>
    private static Dictionary<uint, uint> AddAttributeToAllByPercentage(Dictionary<uint, uint> popIndex, double percentage, uint attribute, uint alternative)
    {
        var resIndex = new Dictionary<uint, uint>();

        foreach (var (pop, popCount) in popIndex)
        {
            var newAttributeCount = (uint)(popCount * percentage);
            
            if(newAttributeCount > 0)
                resIndex.Add(pop | attribute, newAttributeCount);
            
            resIndex.Add(pop | alternative, popCount - newAttributeCount);
        }

        return resIndex;
    }

    #endregion
}