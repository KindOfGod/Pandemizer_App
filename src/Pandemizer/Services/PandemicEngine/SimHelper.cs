using System;
using System.Collections.Generic;
using System.Linq;

namespace Pandemizer.Services.PandemicEngine
{
    /// <summary>
    /// SimHelper contains Methods to assist the SimEngine.
    /// </summary>
    public static class SimHelper
    {
        //todo: use random seed! static seed only for debug use
        private const int Seed = 1;
        private static readonly Random Rnd = new(Seed);
        
        /// <summary>
        /// Merges two dictionaries with no duplicate key values.
        /// </summary>
        public static void MergeDictionariesNoDuplicates(Dictionary<uint, uint> destination,
            Dictionary<uint, uint> source)
        {
            if (source.Count == 0)
                return;

            source.ToList().ForEach(x => destination.Add(x.Key, x.Value));
        }
        
        /// <summary>
        /// Merges two dictionaries with duplicate key values.
        /// </summary>
        public static void MergeDictionaries(Dictionary<uint, uint> destination,
            Dictionary<uint, uint> source)
        {
            if (source.Count == 0)
                return;

            foreach (var pop in source)
            {
                if (destination.ContainsKey(pop.Key))
                    destination[pop.Key] += pop.Value;
                else
                    destination.Add(pop.Key, pop.Value);
            }
        }
        
        /// <summary>
        /// Adds entry with a possibility of an existing key.
        /// </summary>
        public static void AddValueToDictionary(Dictionary<uint, uint> destination, uint key, uint value)
        {
            if (destination.ContainsKey(key))
                    destination[key] += value;
            else
                if(value != 0)
                    destination.Add(key, value);
        }
        
        /// <summary>
        /// Decide a true/false decision
        /// </summary>
        public static bool DecideWithProbability(double percentage)
        {
            var num = Rnd.Next(0, 100);
            return num <= percentage * 100;
        }
        
        /// <summary>
        /// Decide a percentage with a random deviation
        /// </summary>
        public static uint DecideCountWithDeviation(uint count, double percentage, double deviation)
        {
            var dev = deviation * 100;
            var resDev = (double)Rnd.Next(-(int)dev, (int)dev);
            var cnt = (int) (count * (percentage * (1 + resDev / 100)));
            
            return Convert.ToUInt32(cnt);
        }
    }
}