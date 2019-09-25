using System;
using System.Collections.Generic;
using System.Linq;

namespace SynaptikonFramework.Util
{
    public static class IEnumerableExtension {
        /// <summary>
        /// Gets a random element 
        /// </summary>
        /// <returns>The random element</returns>
        /// <param name="source">Source.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T RandomElement<T>(this IEnumerable<T> source) {
            var random = new Random();
            return source.ElementAt(random.Next(0, source.Count()));
        }
    }
}