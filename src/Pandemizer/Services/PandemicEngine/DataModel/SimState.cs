using System;
using System.Collections;
using System.Collections.Generic;

namespace Pandemizer.Services.PandemicEngine.DataModel
{
    /// <summary>
    /// Contains information about a single iteration of a simulation.
    /// </summary>
    public class SimState : IEnumerable
    {
        public Dictionary<uint, uint> PopIndex;

        #region Stats
        
        //base stats
        public double Incidence = 0; // new infected / heavilyInfected
        public double UnknownIncidence = 0; // Incidence + imperceptibly infected

        public double DeathRate = 0;
        public double ImmuneRate = 0;

        public long TotalInfected = 0;
        public long UnknownTotalInfected = 0;
        
        //StateOfLive
        public long Healthy = 0;
        public long ImperceptibleInfected = 0;
        public long Infected = 0;
        public long HeavilyInfected = 0;
        public long Dead = 0;
        public long Immune = 0;
        
        //Hospital
        public long Hospitalized = 0;
        public long ReleasedHospitalized = 0;
        public double HospitalizedPercent = 0;

        public long HospitalizedChildren = 0;
        public long HospitalizedYoungAdults = 0;
        public long HospitalizedAdults = 0;
        public long HospitalizedPensioner = 0;
        
        public long HospitalizedPreExistingCondition = 0;
        public long HospitalizedNoPreExistingCondition = 0;


        public TimeSpan IterationTime = TimeSpan.Zero;
        public TimeSpan StatsTime = TimeSpan.Zero;
        
        #endregion

        public SimState()
        {
            PopIndex = new Dictionary<uint, uint>();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}