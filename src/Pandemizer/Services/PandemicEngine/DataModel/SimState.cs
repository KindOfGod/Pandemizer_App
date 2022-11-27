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

        public long Incidence = 0; // new infected / heavilyInfected
        public long UnknownIncidence = 0; // Incidence + imperceptibly infected

        public long DeathRate = 0;
        public long ImmuneRate = 0;

        public long TotalInfected = 0;
        public long UnknownTotalInfected = 0;

        public long Healthy = 0;
        public long ImperceptibleInfected = 0;
        public long Infected = 0;
        public long HeavilyInfected = 0;
        public long Dead = 0;
        public long Immune = 0;
        
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