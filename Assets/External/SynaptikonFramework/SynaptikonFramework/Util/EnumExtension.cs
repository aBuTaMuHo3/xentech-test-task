using System;
using System.Collections.Generic;

namespace SynaptikonFramework.Util
{
    public static class EnumExtension
    {
        /// <summary>
        /// Gets a random element from the enum. example of use:  YourEnumType random = default(YourEnumType).RandomElement<YourEnumType>();
        /// </summary>
        /// <returns>The random element</returns>
        /// <param name="source">Source.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T RandomElement<T>(this Enum source) {
            System.Array values = System.Enum.GetValues(typeof(T));
            var random = new Random();
            T answer = (T)values.GetValue(random.Next(0, values.Length));
            return answer;
        }
    }
}