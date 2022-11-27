using System;
using System.Collections.Generic;
using Pandemizer.Services.PandemicEngine.DataModel;

namespace Pandemizer.Services.PandemicEngine
{
    /// <summary>
    /// The SimEngine contains all Methods to generate and run a simulation. The SimEngine does not save a single simulation.
    /// All methods can be used for multiple simulation at once.
    /// </summary>
    public static partial class SimEngine
    {
        #region Public Methods

        /// <summary>
        /// Create and generate new simulation with SimSettings.
        /// </summary>
        public static Sim CreateNewSim(SimSettings settings)
        {
            return new Sim(settings, new List<SimState>{GenerateInitialSimState(settings)});
        }
        
        /// <summary>
        /// Load Simulation from SimSettings and SimStates.
        /// </summary>
        public static Sim LoadSim(SimSettings settings, List<SimState> simStates)
        {
            return new Sim(settings, simStates);
        }
        
        /// <summary>
        /// Iterate Simulation to next state
        /// </summary>
        public static void IterateSimulation(Sim? sim)
        {
            var newState = new SimState();
            var newPopIndex = new Dictionary<uint, uint>();
            
            if(sim == null)
                return;

            foreach (var pop in sim.SimStates[^1].PopIndex)
            {
                //only simulate living people
                //respect possible duplicates in dictionary
                if(!AttributeHelper.CheckStateOfLive(pop.Key, StateOfLife.Dead) && ( !AttributeHelper.CheckStateOfLive(pop.Key, StateOfLife.Immune) || sim.SimSettings.ImmunityDuration < sim.SimStates.Count))
                    SimHelper.MergeDictionaries(newPopIndex, IteratePip(pop.Key, pop.Value, sim));
                else
                    SimHelper.AddValueToDictionary(newPopIndex, pop.Key, pop.Value);
            }

            newState.PopIndex = newPopIndex;
            sim.SimStates.Add(newState);
            
            CalculateStateStats(sim);
        }
        
        #endregion

        #region Private Methods
        /// <summary>
        /// Iterates a single Pop and returns Dictionary with resulting Pops
        /// </summary>
        private static Dictionary<uint, uint> IteratePip(uint pop, uint count, Sim sim)
        {
            var newPopIndex = new Dictionary<uint, uint>();
            var settings = sim.SimSettings;
            var prevState = sim.SimStates[^1];
            
            var isEndangeredAge = false;
            if(settings.EndangeredAgeGroup != null)
                isEndangeredAge = AttributeHelper.CheckAge(pop, (Age)settings.EndangeredAgeGroup);
            
            //healthy
            if (AttributeHelper.CheckStateOfLive(pop, StateOfLife.Healthy))
            {
                var rateOfInfection = isEndangeredAge ? settings.EndangeredAgeInfectionRate : settings.BaseInfectionRate;
                
                //calculate modifier based on infection count
                rateOfInfection = rateOfInfection + settings.InfectionSpreadRate * rateOfInfection * ((double)prevState.UnknownTotalInfected / (settings.Scope - prevState.Dead - prevState.Immune));
                
                //rateOfInfection can be 0.1 and 0.5 in worst case
                
                //new features

                if (rateOfInfection > 1 - settings.ProbabilityDeviation)
                    rateOfInfection = 1 - settings.ProbabilityDeviation;

                var newInfected = SimHelper.DecideCountWithDeviation(count, rateOfInfection, settings.ProbabilityDeviation);
                
                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, settings.InfectionSeverity), newInfected);
                SimHelper.AddValueToDictionary(newPopIndex, pop, count - newInfected);
            }
            //heavily infected
            else if (AttributeHelper.CheckStateOfLive(pop, StateOfLife.HeavilyInfected))
            {
                var rateOfDead = isEndangeredAge ? settings.EndangeredAgeDeathRate : settings.BaseDeathRate;
                var rateOfImmune = isEndangeredAge ? settings.EndangeredImmunityRate : settings.BaseImmunityRate;
                
                //rate of pops dying
                var newDead = SimHelper.DecideCountWithDeviation(count, rateOfDead, settings.ProbabilityDeviation);
                
                //rate of pops getting immune
                rateOfImmune = rateOfImmune * settings.SurvivalInstinctMultiplier;
                var newImmune = SimHelper.DecideCountWithDeviation(count - newDead, rateOfImmune, settings.ProbabilityDeviation);


                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, StateOfLife.Dead), newDead);
                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, StateOfLife.Immune), newImmune);
                SimHelper.AddValueToDictionary(newPopIndex, pop, count - newDead - newImmune);
            }
            //immune
            else if (AttributeHelper.CheckStateOfLive(pop, StateOfLife.Immune))
            {
                var susceptible = SimHelper.DecideCountWithDeviation(count, settings.ImmunityLostRate, settings.ProbabilityDeviation);
                
                if(susceptible > count)
                    Console.WriteLine("x");
                
                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, StateOfLife.Healthy), susceptible);
                SimHelper.AddValueToDictionary(newPopIndex, pop, count - susceptible);
            }
            //infected & imperceptible infected
            else
            {
                var rateOfWorsening = isEndangeredAge ? settings.EndangeredAgeRateOfGettingWorse : settings.RateOfGettingWorse;
                var rateOfImmune = isEndangeredAge ? settings.EndangeredImmunityRate : settings.BaseImmunityRate;
                
                //rate of getting worse
                var newWorse = SimHelper.DecideCountWithDeviation(count, rateOfWorsening, settings.ProbabilityDeviation);
                var severity = AttributeHelper.CheckStateOfLive(pop, StateOfLife.ImperceptiblyInfected) ?
                    StateOfLife.Infected : 
                    StateOfLife.HeavilyInfected;
                
                //rate of pops getting immune
                var newImmune = SimHelper.DecideCountWithDeviation(count - newWorse, rateOfImmune, settings.ProbabilityDeviation);

                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, severity), newWorse);
                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, StateOfLife.Immune), newImmune);
                SimHelper.AddValueToDictionary(newPopIndex, pop, count - newWorse - newImmune);
            }

            return newPopIndex;
        }
        
        /// <summary>
        /// Calculates Stats for last SimState
        /// </summary>
        private static void CalculateStateStats(Sim sim)
        {
            var prevState = sim.SimStates[^2];
            var state = sim.SimStates[^1];
            foreach (var (key, count) in state.PopIndex)
            {
                //State of Life
                if (AttributeHelper.CheckStateOfLive(key, StateOfLife.ImperceptiblyInfected))
                    state.ImperceptibleInfected += Convert.ToInt64(count);
                else if (AttributeHelper.CheckStateOfLive(key, StateOfLife.Infected))
                    state.Infected += Convert.ToInt64(count);
                else if (AttributeHelper.CheckStateOfLive(key, StateOfLife.HeavilyInfected))
                    state.HeavilyInfected += Convert.ToInt64(count);
                else if (AttributeHelper.CheckStateOfLive(key, StateOfLife.Dead))
                    state.Dead += Convert.ToInt64(count);
                else if (AttributeHelper.CheckStateOfLive(key, StateOfLife.Immune))
                    state.Immune += Convert.ToInt64(count);
                else
                    state.Healthy += Convert.ToInt64(count);

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
            }
        }

        #endregion
    }
}