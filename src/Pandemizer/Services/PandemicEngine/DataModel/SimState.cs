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

        public long TotalInfected = 0;
        public long UnknownTotalInfected = 0;

        public long CntHealthy = 0;
        public long CntImperceptibleInfected = 0;
        public long CntInfected = 0;
        public long CntHeavilyInfected = 0;
        public long CntDead = 0;

        public long Recovered = 0;

        #endregion

        public SimState(int scope)
        {
            PopIndex = new Dictionary<uint, uint>(scope / 100);
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}