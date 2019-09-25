using System.Collections.Generic;
using System.Linq;

namespace SynaptikonFramework.Util.Math
{
    public class Vector2D: Point2D
    {
        readonly float hashCode;

        // Constructors.
        public Vector2D(float x, float y): base(x, y) { X = x; Y = y; hashCode = !float.IsNaN(x) && !float.IsNaN(y) ? x * y : -1; }
        public Vector2D() : this(float.NaN, float.NaN) { }

        public static Vector2D operator -(Vector2D v, Vector2D w)
        {
            return new Vector2D(v.X - w.X, v.Y - w.Y);
        }

        public static Vector2D operator +(Vector2D v, Vector2D w)
        {
            return new Vector2D(v.X + w.X, v.Y + w.Y);
        }

        public static float operator *(Vector2D v, Vector2D w)
        {
            return v.X * w.X + v.Y * w.Y;
        }

        public static Vector2D operator *(Vector2D v, float mult)
        {
            return new Vector2D(v.X * mult, v.Y * mult);
        }

        public static Vector2D operator *(float mult, Vector2D v)
        {
            return new Vector2D(v.X * mult, v.Y * mult);
        }

        public float Cross(Vector2D v)
        {
            return X * v.Y - Y * v.X;
        }

        public static T GetClosestPoint<T>(List<T> points, Vector2D v1) where T : Vector2D
        {
            return points.OrderBy(v2 => GetDistance(v1, v2)).First();
        }

        public static Vector2D GetFarestPoint(List<Vector2D> points, Vector2D v1)
        {
            return points.OrderByDescending(v2 => GetDistance(v1, v2)).First();
        }

        public static float GetDistance(Vector2D v1, Vector2D v2)
        {
            return (float)System.Math.Sqrt((System.Math.Pow(v1.X - v2.X, 2) + System.Math.Pow(v1.Y - v2.Y, 2)));
        }


        public override bool Equals(object obj)
        {
            var v = (Vector2D)obj;
            return System.Math.Abs((X - v.X)) < float.Epsilon && System.Math.Abs((Y - v.Y)) < float.Epsilon;
        }

		public override int GetHashCode()
		{
            return (int)(hashCode);
		}

		/// <summary>
		/// Gets / sets the angle of this vector. Changing the angle changes the x and y but retains the same length.
		/// </summary>
		/// <value>The angle.</value>
		public float Angle
        {
            set
            {
                float len = Length;
                X = (float)(System.Math.Cos(value) * len);
                Y = (float)(System.Math.Sin(value) * len);
            }
            get
            {
                return (float)System.Math.Atan2(Y, X);
            }
        }

        /// <summary>
        /// Sets / gets the length or magnitude of this vector. Changing the length will change the x and y but not the angle of this vector.
        /// </summary>
        /// <value>The length.</value>
        public float Length
        {
            set
            {
                float a = value;
                X = (float)(System.Math.Cos(a) * value);
                Y = (float)(System.Math.Sin(a) * value);
            }
            get
            {
                return (float)System.Math.Sqrt(LengthSQ);
            }
        }

        /// <summary>
        /// Gets the length of this vector, squared.
        /// </summary>
        /// <value>The length sq.</value>
        public float LengthSQ
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        /**
         * Calculates the angle between two vectors.
         * @param v1 The first Vector2D instance.
         * @param v2 The second Vector2D instance.
         * @return Number the angle between the two given vectors.
         */
        public static float AngleBetween(Vector2D vector1, Vector2D vector2)
        {
            float xDiff = vector2.X - vector1.X;
            float yDiff = vector2.Y - vector1.Y;
            return (float)((360.0 - (System.Math.Atan2(yDiff, xDiff) * 180.0 / System.Math.PI)) % 360.0);
        }


        /// <summary>
        /// Whether or not this vector is normalized, i.e. its length is equal to one.
        /// </summary>
        /// <returns>Boolean True if length is one, otherwise false.</returns>
        public bool IsNormalized()
        {
            return System.Math.Abs(Length - 1.0d) < float.Epsilon;
        }

        /// <summary>
        /// Normalizes this vector. Equivalent to setting the length to one, but more efficient.
        /// </summary>
        /// <returns>A reference to this vector.</returns>
        public Vector2D Normalize()
        {
            if (System.Math.Abs(Length) < float.Epsilon)
            {
                X = 1;
                return this;
            }
            var len = Length;
            X /= len;
            Y /= len;
            return this;

        }

        /// <summary>
        /// Calculates the dot product of this vector and another given vector.
        /// </summary>
        /// <returns>The dot product of this vector and the one passed in as a parameter.</returns>
        /// <param name="v2">V2 Another Vector2D instance.</param>
        public float DotProd(Vector2D v2)
        {
            return X * v2.X + Y* v2.Y;
        }

        /// <summary>
        /// Generates a copy of this vector.
        /// </summary>
        /// <returns>The clone.</returns>
        public Vector2D Clone()
        {
            return new Vector2D(X, Y);
        }

        public override string ToString()
        {
            return string.Concat("x:", X, ", Y: ", Y);
        }

    }
}
