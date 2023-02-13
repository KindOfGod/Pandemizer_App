using System;
using System.Diagnostics;
using System.Linq;
using Pandemizer.Services.PandemicEngine.DataModel;

namespace Pandemizer.Services.PandemicEngine;

public static partial class SimEngine
{
    /// <summary>
    /// Calculates Stats for last SimState
    /// </summary>
    private static void CalculateStateStats(Sim sim)
    {
        var prevState = sim.SimStates[^2];
        var state = sim.SimStates[^1];
        
        var timer = new Stopwatch();
        timer.Start();
        
        foreach (var (pop, count) in state.PopIndex)
        {
            //State of Life
            switch (pop.GetStateOfLive())
            {
                case StateOfLife.ImperceptiblyInfected:
                    state.ImperceptibleInfected += Convert.ToInt64(count);
                    break;
                case StateOfLife.Infected:
                    state.Infected += Convert.ToInt64(count);
                    break;
                case StateOfLife.HeavilyInfected:
                    state.HeavilyInfected += Convert.ToInt64(count);
                    break;
                case StateOfLife.Dead:
                    state.Dead += Convert.ToInt64(count);
                    break;
                case StateOfLife.Immune:
                    state.Immune += Convert.ToInt64(count);
                    break;
                default:
                    state.Healthy += Convert.ToInt64(count);
                    break;
            }

            //incidence
            state.TotalInfected = state.Infected + state.HeavilyInfected;
            state.UnknownTotalInfected = state.ImperceptibleInfected + state.Infected + state.HeavilyInfected;
            
            //immune
            state.ImmuneRate = state.Immune - prevState.Immune;

            var unknownInc = prevState.Healthy - state.Healthy;
            state.UnknownIncidence = unknownInc >= 0 ? unknownInc : 0;

            var inc = prevState.Healthy + prevState.ImperceptibleInfected - state.Healthy - prevState.ImperceptibleInfected;
            state.Incidence = inc >= 0 ? inc : 0;
            
            //death
            state.DeathRate = state.Dead - prevState.Dead;
            
            //hospital
            if (pop.CheckIsHospitalized(IsHospitalized.True))
                state.Hospitalized += count;
        }

        state.HospitalizedPercent = state.Hospitalized * 100 / sim.SimSettings.HospitalCap;
        
        timer.Stop();
        state.StatsTime = timer.Elapsed;
    }

    private static void CalculateSimInfo(Sim sim)
    {
        //Refresh SimInfo
        sim.SimInfo.Iteration++;
        sim.SimInfo.Healthy = ApplicationHelper.IntToFormattedNum((int)sim.SimStates[^1].Healthy);
        sim.SimInfo.Infected = ApplicationHelper.IntToFormattedNum((int)sim.SimStates[^1].Infected);
        sim.SimInfo.Dead = ApplicationHelper.IntToFormattedNum((int)sim.SimStates[^1].Dead);
        sim.SimInfo.Immune = ApplicationHelper.IntToFormattedNum((int)sim.SimStates[^1].Immune);
    }
}