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
        #region Fields

        private static long _hospitalRestSpace = 0;

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Create and generate new simulation with SimInfo and SimSettings.
        /// </summary>
        public static Sim CreateNewSim(SimInfo simInfo, SimSettings settings)
        {
            return new Sim(simInfo, settings, new List<SimState>{GenerateInitialSimState(settings)});
        }
        
        /// <summary>
        /// Load Simulation from SimInfo and SimSettings.
        /// </summary>
        public static Sim LoadSim(SimInfo simInfo, SimSettings settings)
        {
            return new Sim(simInfo, settings, new List<SimState> {GenerateInitialSimState(settings)});
        }

        /// <summary>
        /// Recalculates a sim to its defined iteration (SimInfo)
        /// </summary>
        public static Sim RecalculateSim(Sim sim)
        {
            var iteration = sim.SimInfo.Iteration;
            
            for(var i = 0; i < iteration; i++)
                IterateSimulation(sim);

            sim.SimInfo.Iteration = iteration;
            
            return sim;
        }
        
        /// <summary>
        /// Iterate Simulation to next state
        /// </summary>
        public static void IterateSimulation(Sim sim)
        {
            var newState = new SimState();
            var newPopIndex = new Dictionary<uint, uint>(0);

            _hospitalRestSpace = sim.SimSettings.HospitalCap - sim.SimStates[^1].Hospitalized;
            
            var timer = new Stopwatch();
            timer.Start();

            foreach (var pop in sim.SimStates[^1].PopIndex)
            {
                //only simulate living people
                //respect possible duplicates in dictionary
                if(!pop.Key.CheckStateOfLive(StateOfLife.Dead))
                    SimHelper.MergeDictionaries(newPopIndex, IteratePop(pop.Key, pop.Value, sim));
                else
                    newPopIndex.AddValueToDictionary(pop.Key, pop.Value);
            }

            newState.PopIndex = newPopIndex;
            sim.SimStates.Add(newState);
            
            timer.Stop();
            newState.IterationTime = timer.Elapsed;
            
            CalculateStateStats(sim);

            CalculateSimInfo(sim);
        }
        
        #endregion

        #region Private Methods
        /// <summary>
        /// Iterates a single Pop and returns Dictionary with resulting Pops
        /// </summary>
        private static Dictionary<uint, uint> IteratePop(uint pop, uint count, Sim sim)
        {
            var settings = sim.SimSettings;
            var prevState = sim.SimStates[^1];
            
            //handle hospitalized pops --> default cycle
            if (pop.CheckIsHospitalized(IsHospitalized.True))
                return EvaluatePopsHospitalized(pop, count, sim);

            //handle non-hospitalized pops --> default cycle
            var newPopIndex = IteratePopDefaultCycle(pop, count, sim);
            
            //evaluate hospitalization of non hospitalized pops
            if(prevState.Hospitalized <= settings.HospitalCap)
                newPopIndex = EvaluatePopsHospitalEntering(newPopIndex, settings);

            return newPopIndex;
        }

        private static Dictionary<uint, uint> IteratePopDefaultCycle(uint pop, uint count, Sim sim)
        {
            var newPopIndex = new Dictionary<uint, uint>(0);
            var settings = sim.SimSettings;
            var virus = sim.SimSettings.Virus;
            var prevState = sim.SimStates[^1];
            
            var isEndangeredAge = false;
            if(virus.EndangeredAgeGroup != null)
                isEndangeredAge = pop.CheckAge((Age)virus.EndangeredAgeGroup);
            
            var preConditionModifier = pop.CheckPreExistingCondition(PreExistingCondition.True) ? virus.PreConditionModifier : 1;

            //healthy
            if (pop.CheckStateOfLive(StateOfLife.Healthy))
            {
                var rateOfInfection = isEndangeredAge ? virus.EndangeredAgeInfectionRate : virus.BaseInfectionRate;
                
                //calculate modifier based on infection count
                rateOfInfection += virus.InfectionSpreadRate * rateOfInfection * (((double)prevState.UnknownTotalInfected - prevState.Hospitalized) / (settings.Scope - prevState.Dead - prevState.Immune));
                
                //rateOfInfection can be 0.1 and 0.5 in worst case
                
                // FEATURES
                rateOfInfection *= preConditionModifier; // pre-existing condition

                if (rateOfInfection > 1 - settings.ProbabilityDeviation)
                    rateOfInfection = 1 - settings.ProbabilityDeviation;

                var newInfected = SimHelper.DecideCountWithDeviation(count, rateOfInfection, settings.ProbabilityDeviation);
                
                newPopIndex.AddValueToDictionary(pop.OverrideStateOfLive(virus.InfectionSeverity), newInfected);
                newPopIndex.AddValueToDictionary(pop, count - newInfected);
            }
            //heavily infected
            else if (pop.CheckStateOfLive(StateOfLife.HeavilyInfected))
            {
                var rateOfDead = isEndangeredAge ? virus.EndangeredAgeDeathRate : virus.BaseDeathRate;
                var rateOfImmune = isEndangeredAge ? virus.EndangeredImmunityRate : virus.BaseImmunityRate;
                
                // FEATURES
                rateOfImmune *= virus.SurvivalInstinctMultiplier; // survival instinct modifier
                rateOfDead *= preConditionModifier; // pre-existing condition modifier
                rateOfImmune /= preConditionModifier; // pre-existing condition modifier
                
                //rate of pops dying & rate of pops getting immune
                var newDead = SimHelper.DecideCountWithDeviation(count, rateOfDead, settings.ProbabilityDeviation);
                var newImmune = SimHelper.DecideCountWithDeviation(count - newDead, rateOfImmune, settings.ProbabilityDeviation);
                
                newPopIndex.AddValueToDictionary(pop.OverrideStateOfLive(StateOfLife.Dead).OverrideIsHospitalized(IsHospitalized.False), newDead);
                newPopIndex.AddValueToDictionary(pop.OverrideStateOfLive(StateOfLife.Immune), newImmune);
                newPopIndex.AddValueToDictionary(pop, count - newDead - newImmune);
            }
            //immune
            else if (pop.CheckStateOfLive(StateOfLife.Immune))
            {
                var immunityLostRate = virus.ImmunityLostRate;
                
                // FEATURES
                immunityLostRate *= preConditionModifier;
                
                var susceptible = SimHelper.DecideCountWithDeviation(count, immunityLostRate, settings.ProbabilityDeviation);
                
                newPopIndex.AddValueToDictionary(pop.OverrideStateOfLive(StateOfLife.Healthy), susceptible);
                newPopIndex.AddValueToDictionary(pop, count - susceptible);
            }
            //infected & imperceptible infected
            else
            {
                var rateOfWorsening = isEndangeredAge ? virus.EndangeredAgeRateOfGettingWorse : virus.RateOfGettingWorse;
                var rateOfImmune = isEndangeredAge ? virus.EndangeredImmunityRate : virus.BaseImmunityRate;
                
                // FEATURES
                rateOfWorsening *= preConditionModifier; // pre-existing condition modifier
                rateOfImmune /= preConditionModifier; // pre-existing condition modifier

                //rate of getting worse
                var newWorse = SimHelper.DecideCountWithDeviation(count, rateOfWorsening, settings.ProbabilityDeviation);
                var severity = pop.CheckStateOfLive(StateOfLife.ImperceptiblyInfected) ?
                    StateOfLife.Infected : 
                    StateOfLife.HeavilyInfected;
                
                //rate of pops getting immune
                var newImmune = SimHelper.DecideCountWithDeviation(count - newWorse, rateOfImmune, settings.ProbabilityDeviation);

                newPopIndex.AddValueToDictionary(pop.OverrideStateOfLive(severity), newWorse);
                newPopIndex.AddValueToDictionary(pop.OverrideStateOfLive(StateOfLife.Immune), newImmune);
                newPopIndex.AddValueToDictionary(pop, count - newWorse - newImmune);
            }
            
            return newPopIndex;
        }

        /// <summary>
        /// Calculates IsHospitalized for PopIndex and returns new PopIndex. Pops can't already be in hospital.
        /// </summary>
        private static Dictionary<uint, uint> EvaluatePopsHospitalEntering(Dictionary<uint, uint> oldPopIndex, SimSettings settings)
        {
            var newPopIndex = new Dictionary<uint, uint>(0);

            foreach (var (pop, count) in oldPopIndex)
            {
                if (_hospitalRestSpace == 0 || pop.CheckIsHospitalized(IsHospitalized.True))
                {
                    newPopIndex.AddValueToDictionary(pop, count);
                    continue;
                }
                    

                //Hospital Calculation here
                uint newPatients = pop.GetStateOfLive() switch
                {
                    StateOfLife.Infected => SimHelper.DecideCountWithDeviation(count, settings.InfectedHospitalizing, settings.ProbabilityDeviation),
                    StateOfLife.HeavilyInfected => SimHelper.DecideCountWithDeviation(count, settings.HeavilyInfectedHospitalizing, settings.ProbabilityDeviation),
                    _ => 0
                };

                //take as much patients as space is left in hospital
                if (newPatients > _hospitalRestSpace)
                {
                    newPatients = Convert.ToUInt32(_hospitalRestSpace);
                    _hospitalRestSpace = 0;
                }
                else
                    _hospitalRestSpace -= newPatients;
                
                newPopIndex.AddValueToDictionary(pop.OverrideIsHospitalized(IsHospitalized.True), newPatients);
                newPopIndex.AddValueToDictionary(pop, count - newPatients);
            }

            return newPopIndex;
        }
        
        /// <summary>
        /// Calculates IsHospitalized for PopIndex and returns new PopIndex. Pops have to be in hospital.
        /// </summary>
        private static Dictionary<uint, uint> EvaluatePopsHospitalized(uint pop, uint count, Sim sim)
        {
            var newPopIndex = new Dictionary<uint, uint>(0);
            var settings = sim.SimSettings;
            var virus = sim.SimSettings.Virus;

            var cntLeaveHospital = pop.GetStateOfLive() switch
            {
                StateOfLife.Infected => Convert.ToUInt32(SimHelper.DecideCountWithDeviation(count, virus.HospitalizedInfectedGetHealthy,
                    settings.ProbabilityDeviation)),
                StateOfLife.HeavilyInfected => Convert.ToUInt32(SimHelper.DecideCountWithDeviation(count,
                    virus.HospitalizedHeavilyInfectedGetHealthy, settings.ProbabilityDeviation)),
                _ => (uint)0
            };
            
            newPopIndex.AddValueToDictionary(pop.OverrideStateOfLive(StateOfLife.Immune).OverrideIsHospitalized(IsHospitalized.False), cntLeaveHospital);
            SimHelper.MergeDictionaries(newPopIndex, IteratePopDefaultCycle(pop, count - cntLeaveHospital, sim));

            return newPopIndex;
        }
        #endregion
    }
}