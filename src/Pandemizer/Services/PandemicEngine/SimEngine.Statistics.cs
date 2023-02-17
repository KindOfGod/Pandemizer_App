using System;
using System.Diagnostics;
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
            
            //hospital
            if (pop.CheckIsHospitalized(IsHospitalized.True))
            {
                state.Hospitalized += count;

                if (pop.CheckPreExistingCondition(PreExistingCondition.True))
                    state.HospitalizedPreExistingCondition += count;
                else
                    state.HospitalizedNoPreExistingCondition += count;

                switch (pop.GetAge())
                {
                    case Age.Child:
                        state.HospitalizedChildren += count;
                        break;
                    case Age.YoungAdult:
                        state.HospitalizedYoungAdults += count;
                        break;
                    case Age.Adult:
                        state.HospitalizedAdults += count;
                        break;
                    case Age.Pensioner:
                        state.HospitalizedPensioner += count;
                        break;
                }
            }
        }
        
        //incidence
        state.TotalInfected = state.Infected + state.HeavilyInfected;
        state.UnknownTotalInfected = state.ImperceptibleInfected + state.Infected + state.HeavilyInfected;
        
        var unknownInc = prevState.Healthy - state.Healthy;
        state.UnknownIncidence = unknownInc >= 0 ? unknownInc : 0;

        // calculate incidence counting ImperceptibleInfected as Healthy
        var inc = prevState.Healthy + prevState.ImperceptibleInfected - state.Healthy - state.ImperceptibleInfected;
        state.Incidence = inc >= 0 ? Math.Round((double)inc / sim.SimSettings.Scope * 100_000, 2) : 0;
        
        //immune
        state.ImmuneRate = Math.Round((double)(state.Immune - prevState.Immune) / sim.SimSettings.Scope * 100_000, 2);
            
        //death
        state.DeathRate = Math.Round((double)(state.Dead - prevState.Dead) / sim.SimSettings.Scope * 100_000, 2);

        //hospitalized
        //account people who left in the end of iteration --> 100% cap can be reached this way
        state.HospitalizedPercent = Math.Round((double)(state.Hospitalized + state.ReleasedHospitalized) * 100 / sim.SimSettings.HospitalCap, 2);

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