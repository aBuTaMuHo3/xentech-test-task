using System;
using System.Collections.Generic;
using System.Linq;

namespace MinLibs.Utils
{
	public interface IRandom
	{
		int Seed { get; }

		int Count { get; }

		float Next();
		
		int Next (int excludedMaxValue);

		int Next (int includedMinValue, int excludedMaxValue);

		float Next (float excludedMaxValue);

		float Next (float includedMinValue, float excludedMaxValue);

		T Next<T>(IEnumerable<T> collection);

		bool TossCoin();
	}

	public class SystemRandom : IRandom
	{
		private readonly Random random;

		public int Seed { get; }
		
		public int Count { get; private set; }

		public SystemRandom ()
		{
			Seed = -1;
			random = new Random();
		}

		public SystemRandom (int seed)
		{
			Seed = seed;
			random = new Random(seed);
		}

		public float Next()
		{
			Count++;
			return (float)random.NextDouble();
		}

		public int Next (int excludedMaxValue)
		{
			return random.Next(excludedMaxValue);
		}

		public int Next (int includedMinValue, int excludedMaxValue)
		{
			return random.Next(includedMinValue, excludedMaxValue);
		}

		public float Next (float excludedMaxValue)
		{
			return Next(0, excludedMaxValue);
		}

		public float Next (float includedMinValue, float excludedMaxValue)
		{
			var diff = excludedMaxValue - includedMinValue;
			diff *= Next();
			diff += includedMinValue;

			return diff;
		}

		public T Next<T>(IEnumerable<T> collection)
		{
			var count = collection.Count();
			var index = Next(count);

			return collection.ElementAt(index);
		}

		public bool TossCoin()
		{
			return Next() < 0.5f;
		}
	}
}