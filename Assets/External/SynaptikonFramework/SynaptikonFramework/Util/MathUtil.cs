using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SynaptikonFramework.Util.Math;

namespace SynaptikonFramework.Util
{
    public static class MathUtil
    {
        public static float Difference(float number1, float number2)
        {
            return number1 > number2 ? number1 - number2 : number2 - number1;
        }

        public static int Difference(int number1, int number2)
        {
            return number1 > number2 ? number1 - number2 : number2 - number1;
        }

        public static long Difference(long number1, long number2)
        {
            return number1 > number2 ? number1 - number2 : number2 - number1;
        }

        public static float GetDistance(float p1x, float p1y, float p2x, float p2y)
        {
            return (float)System.Math.Sqrt((System.Math.Pow(p1x - p2x, 2) + System.Math.Pow(p1y - p2y, 2)));
        }


        // Combinations
        public static IEnumerable<IEnumerable<T>> GetKCombs<T>(IEnumerable<T> list, uint length) where T : IComparable
        {
            if (length == 0) return new List<List<T>>();
            if (length == 1) return list.Select(t => new T[] { t });
            return GetKCombs(list, length - 1)
                .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) > 0),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static IEnumerable<IEnumerable<T>> GetAllCombos<T>(T[] list, uint currentLetterDifference)
        {
            var returnList = new List<List<T>>();
            for (var i = 1; i < System.Math.Pow(2, list.Length); i++)
            {
                var result = Convert.ToString(i, 2).PadLeft(list.Length, '0');
                var cnt = Regex.Matches(Regex.Escape(result), "1").Count;
                if (cnt == currentLetterDifference)
                {
                    var list2 = new List<T>();
                    for (int j = 0; j < list.Length; j++)
                        if (Char.GetNumericValue(result[j]) == 1)
                            list2.Add(list[j]);
                    returnList.Add(list2);
                }
            }
            return returnList;
        }

        public static LinearFunctionVO GetLinearRegression(List<Vector2D> points)
        {
            int amountOfPoints = points.Count;
            if(amountOfPoints > 1)
            {
                // Calculate for at least 2 points.
                double sumX = 0;
                double sumY = 0;
                double sumXX = 0;
                double sumXY = 0;

                for (int i = 0; i < amountOfPoints; i++){
                    sumX += points[i].X;
                    sumY += points[i].Y;
                    sumXX += points[i].X * points[i].X;
                    sumXY += points[i].X * points[i].Y;
                }

                double a1numerator = amountOfPoints * sumXY - sumX * sumY;
                double a0numerator = sumY * sumXX - sumX * sumXY;
                double denominator = (amountOfPoints * sumXX - System.Math.Pow(sumX, 2));

                double a1 = a1numerator / denominator;
                double a0 = a0numerator / denominator;

                return new LinearFunctionVO(a0, a1);
            }else if (amountOfPoints == 1)
            {
                // Calculate for 1 point its horisontal line.
                return new LinearFunctionVO(points[0].Y, 0);
            }
            else
            {
                // No way to calculate when no points.
                return null;
            }
        }
    }
}
