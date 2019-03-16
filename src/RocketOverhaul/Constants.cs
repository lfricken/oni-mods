/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using STRINGS;
using System.Collections.Generic;

namespace RocketOverhaul
{
	/// <summary>
	/// Computed via https://github.com/lfricken/oni-mods/blob/master/dev_utils/rocket_distance_overhaul.py
	/// </summary>
	public class DistanceEquationScalars
	{
		public const float Exhaust = 0.00146959f;
		public const int Exponent = 2;
		public const float Range = 133320.0f;
		public const float FuelPerXTick = 900f;
	}

	public class ModulePenalties
	{
		public const float ResearchPenalty = 2000f;
		public const float TouristPenalty = 8000f;
		public const float CargoPenalty = 30000f;
	}

	public class OxidizerEfficiency
	{
		public const float Mixed = 1.2f;
		public const float Lox = 1.0f;
		public const float OxyRock = 0.8f;
	}

	public partial class EngineBaseStats
	{
		public static string GetName(string element)
		{
			return UI.FormatAsLink(element + " Engine", element + "Engine");
		}

		public static string GetDescription(string element, float exhaustVelocity, float minFuel, float maxFuel)
		{
			string velocity = StarmapScreenOverhaul.FormatDistance(exhaustVelocity, "", "m/s");
			string lowFuel = StarmapScreenOverhaul.FormatDistance(minFuel, "", "kg");
			string highFuel = StarmapScreenOverhaul.FormatDistance(maxFuel, "", "kg");

			return $"{UI.FormatAsLink(element, element.ToUpper())} engines have an exhaust velocity of {velocity} and need {lowFuel} to {highFuel} of fuel to operate efficiently.";
		}

		public static string GetEffect(string element)
		{
			return "Burns " + UI.FormatAsLink(element, element.ToUpper() + " (Liquid)") + " to propel rockets for space exploration.";
		}
	}

	public partial class MethaneEngineStats
	{
		public static string Element = "Methane";

		public static string NAME = EngineBaseStats.GetName(Element);
		public static string DESC = EngineBaseStats.GetDescription(Element, ExhaustVelocity, MinFuel, MaxFuel);
		public static string EFFECT = EngineBaseStats.GetEffect(Element);

		public const string TechGroup = "EnginesIII";
		public const string BuildTab = "Rocketry";

		public const SimHashes FuelType = SimHashes.LiquidMethane;
		public const float ExhaustVelocity = 6127;
		public const float RangePenalty = 2380000;
		public const float MinFuel = 3200;
		public const float MaxFuel = 4000;
	}

	public partial class HydrogenEngineStats
	{
		public static string Element = "Hydrogen";

		public static string NAME = EngineBaseStats.GetName(Element);
		public static string DESC = EngineBaseStats.GetDescription(Element, ExhaustVelocity, MinFuel, MaxFuel);
		public static string EFFECT = EngineBaseStats.GetEffect(Element);

		public const string TechGroup = "EnginesIII";
		public const string BuildTab = "Rocketry";

		public const float ExhaustVelocity = 4423;
		public const float RangePenalty = 1200000;
		public const float MinFuel = 2200;
		public const float MaxFuel = 2900;
	}

	public class PetroleumEngineStats
	{
		public static string Element = "Petroleum";

		public static string NAME = EngineBaseStats.GetName(Element);
		public static string DESC = EngineBaseStats.GetDescription(Element, ExhaustVelocity, MinFuel, MaxFuel);
		public static string EFFECT = EngineBaseStats.GetEffect(Element);

		public const string TechGroup = "EnginesII";
		public const string BuildTab = "Rocketry";

		public const float ExhaustVelocity = 2751;
		public const float RangePenalty = 435000;
		public const float MinFuel = 1200;
		public const float MaxFuel = 1700;
	}

	public class SteamEngineStats
	{
		public static string Element = "Steam";

		public static string NAME = EngineBaseStats.GetName(Element);
		public static string DESC = EngineBaseStats.GetDescription(Element, ExhaustVelocity, MinFuel, MaxFuel)
									+ $"Stores {StarmapScreenOverhaul.FormatDistance(MaxStorage, "", "kg")} of {UI.FormatAsLink(Element, Element.ToUpper())}. Can't utilize additional fuel tanks.";
		public static string EFFECT = "Uses " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public const string TechGroup = "BasicRocketry";
		public const string BuildTab = "Rocketry";

		public const float ExhaustVelocity = 2751;
		public const float RangePenalty = 435000;
		public const float MinFuel = 250;
		public const float MaxFuel = 500;
		public const float MaxStorage = 500;
	}

	public class SolidBoosterStats
	{
		private static string Element1 = "Iron";
		private static string Element2 = "Oxylite";

		public static string NAME = UI.FormatAsLink("Solid Booster", nameof(SolidBooster));
		public static string DESC = $"Boosters increase the range of a rocket:\n" +
									$"First:  {StarmapScreenOverhaul.FormatDistance(Ranges[0])}\n" +
									$"Second: {StarmapScreenOverhaul.FormatDistance(Ranges[1])}\n" +
									$"Third:  {StarmapScreenOverhaul.FormatDistance(Ranges[2])}\n";

		public static string EFFECT = "Burns " + UI.FormatAsLink(Element1, Element1.ToUpper()) + " and " + UI.FormatAsLink(Element2, Element2.ToUpper()) + " to boost the range of a rocket.";

		public const string TechGroup = "EnginesI";
		public const string BuildTab = "Rocketry";

		public const float MaxFuelStorage = 400;
		public const float MaxOxyStorage = 400;

		/// <summary>
		/// Range contributions per booster.
		/// </summary>
		public static List<float> Ranges
		{
			get
			{
				var list = new List<float>();
				list.Add(30000);
				list.Add(20000);
				list.Add(10000);
				return list;
			}
		}
	}
}
