using System;
using System.Collections.Generic;
using System.Linq;
using Pandemizer.Services.PandemicEngine.DataModel;

namespace Pandemizer.Services.PandemicEngine
{
    /// <summary>
    /// SimHelper contains Methods to assist the SimEngine.
    /// </summary>
    public static class SimHelper
    {
        private static Random Rnd = new(0);

        /// <summary>
        /// Set Seed for SimHelper
        /// </summary>
        public static void SetSeed(int seed)
        {
            Rnd =  new Random(seed);
        }
        
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
        public static void AddValueToDictionary(this Dictionary<uint, uint> destination, 
            uint key, uint count)
        {
            if (count == 0)
                return;
            
            if (destination.ContainsKey(key))
                    destination[key] += count;
            else
                destination.Add(key, count);
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