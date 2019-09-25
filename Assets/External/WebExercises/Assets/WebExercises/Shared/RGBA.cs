using System;
using UnityEngine;

namespace WebExercises.Exercise
{
	public class RGBA
	{
		public Color Color { get; private set; }
		
		private byte[] rgba;

		public RGBA(string rgbHex)
		{
			rgba = GetColorBytes(rgbHex);
			Color = new Color32(rgba[0], rgba[1], rgba[2], rgba[3]);
		}

		private static byte[] GetColorBytes(string rgbHex)
		{
			var rgb = int.Parse(rgbHex, System.Globalization.NumberStyles.HexNumber);
			var r = Convert.ToByte((rgb & 0xff0000) >> 16);
			var g = Convert.ToByte((rgb & 0xff00) >> 8);
			var b = Convert.ToByte(rgb & 0xff);

			return new[] { r, g, b, byte.MaxValue };
		}
	}
}