using System;
using System.Numerics;

namespace CanYouCount
{
	public class SeededRandomService : IRandomService
	{
		public readonly int Seed;

		private System.Random _random;

		public static int GetRandomSeed()
			=> new System.Random().Next();

		public SeededRandomService(int? seed = null)
		{
			Seed = seed ?? GetRandomSeed();
			_random = new System.Random(Seed);
		}

		public void InitializeSystem() { } // No initialization needed

		public int RandInt()
			=> _random.Next();
		public int RandInt(int maxValue)
			=> _random.Next(maxValue);
		public int RandInt(int minValue, int maxValue)
			=> _random.Next(minValue, maxValue);

		public double RandDouble()
			=> _random.NextDouble();
		public double RandDouble(double maxValue)
			=> RandDouble() * maxValue;
		public double RandDouble(double minValue, double maxValue)
			=> minValue + (RandDouble() * (maxValue - minValue));

		public float RandFloat()
			=> (float)_random.NextDouble();
		public float RandFloat(float maxValue)
			=> RandFloat() * maxValue;
		public float RandFloat(float minValue, float maxValue)
			=> minValue + (RandFloat() * (maxValue - minValue));

		public bool Bool()
			=> RandDouble() >= 0.5;
		public bool Bool(double probability)
			=> RandDouble() >= probability;

		public Vector2 RandUnitVector2()
		{
			var angle = RandDouble(0, Math.PI * 2);
			return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
		}

		public override string ToString()
			=> $"Random ({Seed})";
	}
}
