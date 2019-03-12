/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>


namespace RocketOverhaul
{
	public class StarmapScreenOverhaul
	{
		public class Caption
		{
			public static string EngineExhaustVelocity = "Exhaust Velocity";
			public static string RecommendedOxidizableFuel = "Recommended Oxidizable Fuel";
			public static string TotalOxidizableFuel = "Oxidizable Fuel";

			public static string TotalEngineThrust = "Main Engine";
			public static string TotalOxidizerEfficiency = "Oxidizer Efficiency";
			public static string TotalBoosterThrust = "Solid Boosters";
			public static string TotalPayload = "Payload";
			public static string TotalDistance = "Total Range";

			public static string OxyRock = "Oxylite (80%)";
			public static string LiquidOxygen = "Liquid Oxygen (100%)";
			public static string Mixed = "1:1 Mix (120%)";
		}

		/// <summary>
		/// Returns 
		/// </summary>
		public static string FormatDistance(float distanceInKm, string positiveNumberSign = PositiveNumberSign, string unit = Unit)
		{
			if (distanceInKm < 0)
			{
				positiveNumberSign = "";
			}

			return positiveNumberSign + string.Format(FormatNoDecimalAndCommas, distanceInKm) + unit;
		}
		public const string Unit = "km";
		public const string PositiveNumberSign = "+";
		public static readonly string FormatNoDecimalAndCommas = "{0:n0}"; // no decimals and use , seperators for thousands place
	}
}
