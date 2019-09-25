using System;
using System.Collections.Generic;

namespace SynaptikonFramework.Util
{
    public static class ArrayExtension
    {
        /// <summary>
        /// Array Extension to randomize an array using the Knuth shuffle algorithmus.  
        /// </summary>
        /// <param name="source">Source array to be shuffle.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>the same array shuffle</returns>
        public static T[] Shuffle<T>(this T[] source)
        {
            return Shuffle(source, new Random());
        }

        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        /// <summary>
        /// Array Extension to randomize an array using the Knuth shuffle algorithmus.  
        /// </summary>
        /// <param name="source">Source array to be shuffle.</param>
        /// <param name="random">The Random instance to be used.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>the same array shuffle</returns>
        public static T[] Shuffle<T>(this T[] source, Random random) {
            for(int i = 0; i < source.Length; i++) {
                T tmp = source[i];
                int index = random.Next(i, source.Length);
                source[i] = source[index];
                source[index] = tmp;
            }
            return source;
        }

        /// <summary>
        /// Initializes an array with a sequence of increasing integers from minNum(INCLUSIVE) to maxNum(EXCLUSIVE).</br>
        /// So sequence(_arMyArray.length) will yield an array of 0,1,2,3,4,...,_arMyArray.length-1.   
        /// </summary>
        /// <param name="maxNum">EXCLUSIVE maximal number of sequence</param>
        /// <param name="minNum">INCLUSIVE minimal number of sequence</param>
        /// <returns>[minNum,minNum+1,...,maxNum-1]</returns>
        public static int[] Sequence(int maxNum, int minNum = 0) {
            int[] ar = new int[maxNum - minNum];
            for(int i = 0; i < ar.Length; i++) {
                ar[i] = minNum + i;
            }
            return ar;
        }
    }
}