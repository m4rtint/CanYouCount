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
		float RandFloat(float minValue, float maxValue);

		bool Bool();
		bool Bool(double probability);

		Vector2 RandUnitVector2();
	}
}
