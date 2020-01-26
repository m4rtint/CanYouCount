using System.Numerics;

namespace CanYouCount
{
	public interface IRandomService
	{
		int RandInt();
		int RandInt(int maxValue);
		int RandInt(int minValue, int maxValue);

		double RandDouble();
		double RandDouble(double maxValue);
		double RandDouble(double minValue, double maxValue);

		float RandFloat();
		float RandFloat(float maxValue);
		/// <summary>
		/// Returns a random value between minValue (inclusive) and maxValue (exclusive)
		/// </summary>
		/// <param name="minValue">The inclusive lower bound</param>
		/// <param name="maxValue">The exclusive upper boudn</param>
		/// <returns></returns>
		float RandFloat(float minValue, float maxValue);

		bool Bool();
		bool Bool(double probability);

		Vector2 RandUnitVector2();
	}
}
