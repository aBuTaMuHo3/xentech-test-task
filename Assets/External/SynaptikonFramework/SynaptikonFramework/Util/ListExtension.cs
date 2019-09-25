using System;
using System.Collections.Generic;

namespace SynaptikonFramework.Util
{
    public static class ListExtension
    {   
        /// <summary>
        /// List Extension to randomize a List using the Knuth shuffle algorithmus. 
        /// </summary>
        /// <returns>The same list after shuffled.</returns>
        /// <param name="source">Source list to be shuffle.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> Shuffle<T>(this List<T> source)
        {
            return Shuffle(source, new Random());
        }

        public static List<T> Shuffle<T>(this List<T> source, Random random)
        {
            for (int i = 0; i < source.Count; i++)
            {
                T tmp = source[i];
                int index = random.Next(i, source.Count);
                source[i] = source[index];
                source[index] = tmp;
            }
            return source;
        }

        /// <summary>
        /// Removes elements from the list and give them back. the original list gets modifided.
        /// </summary>
        /// <param name="source">List to be modifed.</param>
        /// <param name="fromIndex">>index (inclusive) where to start the cut</param>
        /// <param name="count">How many elements will be return.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> Splice<T>(this List<T> source, int fromIndex, int count)
        {
            List<T> response = source.GetRange(fromIndex, count);
            //remove the elements 
            source.RemoveRange(fromIndex, count);
            return response;
        }

        /// <summary>
        /// Removes the first element of a list and give it back
        /// </summary>
        /// <param name="source">Source.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T Shift<T>(this List<T> source)
        {
            return source.Splice(0, 1)[0];
        }

        /// <summary>
        /// Clone the specified List.
        /// </summary>
        /// <param name="source">Source list.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<T> Clone<T>(this List<T> source) {
            return new List<T>(source);
        }

        /// <summary>
        /// Fill list with integers row, started with the giving cipher. atention the list will get reseted>  
        /// </summary>
        /// <param name="source">List to fill up.</param>
        /// <param name="amount">amount of element to generate</param>
        /// <param name="startCypher">the value of the fist element of the array</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static List<int> Fill(this List<int> source, int amount, int startCypher = 0) {
            for(int i = 0; i < amount; i++) {
                source.Add(startCypher);
                startCypher++;
            }
            return source;
        }
    }
}