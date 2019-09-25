using System;
namespace SynaptikonFramework.Util.Math
{
    public class Rotation2D
    {
        public const int DEGREE_0 = 0;
        public const int DEGREE_90 = 90;
        public const int DEGREE_180 = 180;
        public const int DEGREE_270 = 270;

        private static int[] _standartRotations = new int[]{DEGREE_0,DEGREE_90,DEGREE_180,DEGREE_270};

        /// <summary>
        /// get one random standart rotation Quaternion
        /// </summary>
        /// <returns>one of the standart rotation</returns>
        /// <param name="includeZero">If set to <c>true</c> means include the zero rotation one as posibility </param>
        public static int GetRandom(bool includeZero = true)
        {
            Random random = new Random();
            return _standartRotations[random.Next((includeZero) ? 0 : 1, _standartRotations.GetLength(0))];
        }
    }
}
