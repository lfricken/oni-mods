/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>


namespace RocketOverhaul
{
	public class StarmapScreenOverhaul
	{
		public class Caption
		{
			public static string TotalOxidizableFuel = "Total Oxidizable Fuel";
			public static string EngineExhaustVelocity = "Engine Exhaust Velocity";
			public static string AverageOxidizerEfficiency = "Average Oxidizer Efficiency";

			public static string TotalEngineThrust = "Main Engine";
			public static string TotalBoosterThrust = "Solid Boosters";
			public static string Payload = "Payload";
			public static string TotalDistance = "Total Range";
		}

		/// <summary>
		/// Returns 
		/// </summary>
		public static string FormatDistance(float distanceInKm)
		{
			string sign = "+";
			if (distanceInKm < 0)
			{
				sign = "";
			}

			return sign + string.Format(FormatNoDecimalAndCommas, distanceInKm) + Unit;
		}
		private static readonly string Unit = "km";
		public static readonly string FormatNoDecimalAndCommas = "{0:n0}"; // no decimals and use , seperators for thousands place
	}
}
