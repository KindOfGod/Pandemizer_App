using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            
            var timer = new Stopwatch();
            timer.Start();
            
            if(sim == null)
                return;

            foreach (var pop in sim.SimStates[^1].PopIndex)
            {
                //only simulate living people
                //respect possible duplicates in dictionary
                if(!AttributeHelper.CheckStateOfLive(pop.Key, StateOfLife.Dead))
                    SimHelper.MergeDictionaries(newPopIndex, IteratePip(pop.Key, pop.Value, sim));
                else
                    SimHelper.AddValueToDictionary(newPopIndex, pop.Key, pop.Value);
            }

            newState.PopIndex = newPopIndex;
            sim.SimStates.Add(newState);
            
            timer.Stop();
            newState.IterationTime = timer.Elapsed;
            
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
            var virus = sim.SimSettings.Virus;
            var prevState = sim.SimStates[^1];
            
            var isEndangeredAge = false;
            if(virus.EndangeredAgeGroup != null)
                isEndangeredAge = AttributeHelper.CheckAge(pop, (Age)virus.EndangeredAgeGroup);
            
            var preConditionModifier = AttributeHelper.CheckPreExistingCondition(pop, PreExistingCondition.True) ? virus.PreConditionModifier : 1;
            
            //healthy
            if (AttributeHelper.CheckStateOfLive(pop, StateOfLife.Healthy))
            {
                var rateOfInfection = isEndangeredAge ? virus.EndangeredAgeInfectionRate : virus.BaseInfectionRate;
                
                //calculate modifier based on infection count
                rateOfInfection += virus.InfectionSpreadRate * rateOfInfection * ((double)prevState.UnknownTotalInfected / (settings.Scope - prevState.Dead - prevState.Immune));
                
                //rateOfInfection can be 0.1 and 0.5 in worst case
                
                // FEATURES
                rateOfInfection *= preConditionModifier; // pre-existing condition

                if (rateOfInfection > 1 - settings.ProbabilityDeviation)
                    rateOfInfection = 1 - settings.ProbabilityDeviation;

                var newInfected = SimHelper.DecideCountWithDeviation(count, rateOfInfection, settings.ProbabilityDeviation);
                
                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, virus.InfectionSeverity), newInfected);
                SimHelper.AddValueToDictionary(newPopIndex, pop, count - newInfected);
            }
            //heavily infected
            else if (AttributeHelper.CheckStateOfLive(pop, StateOfLife.HeavilyInfected))
            {
                var rateOfDead = isEndangeredAge ? virus.EndangeredAgeDeathRate : virus.BaseDeathRate;
                var rateOfImmune = isEndangeredAge ? virus.EndangeredImmunityRate : virus.BaseImmunityRate;
                
                // FEATURES
                rateOfImmune *= virus.SurvivalInstinctMultiplier; // survival instinct
                rateOfDead *= preConditionModifier; // pre-existing condition
                rateOfImmune /= preConditionModifier; // pre-existing condition
                
                //rate of pops dying & rate of pops getting immune
                var newDead = SimHelper.DecideCountWithDeviation(count, rateOfDead, settings.ProbabilityDeviation);
                var newImmune = SimHelper.DecideCountWithDeviation(count - newDead, rateOfImmune, settings.ProbabilityDeviation);
                
                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, StateOfLife.Dead), newDead);
                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, StateOfLife.Immune), newImmune);
                SimHelper.AddValueToDictionary(newPopIndex, pop, count - newDead - newImmune);
            }
            //immune
            else if (AttributeHelper.CheckStateOfLive(pop, StateOfLife.Immune))
            {
                var immunityLostRate = virus.ImmunityLostRate;
                
                // FEATURES
                immunityLostRate *= preConditionModifier;
                
                var susceptible = SimHelper.DecideCountWithDeviation(count, immunityLostRate, settings.ProbabilityDeviation);

                SimHelper.AddValueToDictionary(newPopIndex, AttributeHelper.OverrideStateOfLive(pop, StateOfLife.Healthy), susceptible);
                SimHelper.AddValueToDictionary(newPopIndex, pop, count - susceptible);
            }
            //infected & imperceptible infected
            else
            {
                var rateOfWorsening = isEndangeredAge ? virus.EndangeredAgeRateOfGettingWorse : virus.RateOfGettingWorse;
                var rateOfImmune = isEndangeredAge ? virus.EndangeredImmunityRate : virus.BaseImmunityRate;
                
                // FEATURES
                rateOfWorsening *= preConditionModifier;
                rateOfImmune /= preConditionModifier;
                
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
        
        #endregion
    }
}