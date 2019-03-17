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
		public static string GetElementLinkEngine(string element)
		{
			return UI.FormatAsLink(element + " Engine", element + "ENGINE");
		}

		public static string GetDescription(string element, float exhaustVelocity, float minFuel, float maxFuel)
		{
			string velocity = StarmapScreenOverhaul.FormatDistance(exhaustVelocity, "", "m/s");
			string lowFuel = StarmapScreenOverhaul.FormatDistance(minFuel, "", "kg");
			string highFuel = StarmapScreenOverhaul.FormatDistance(maxFuel, "", "kg");

			return $"{GetElementLink(element)} engines have an exhaust velocity of {velocity} and need {lowFuel} to {highFuel} of fuel to operate efficiently.";
		}

		public static string GetElementLink(string element)
		{
			string additional = "";
			if (element == "Hydrogen")
				additional = "LIQUID";
			return UI.FormatAsLink(element, additional + element.ToUpper());
		}

		public static string GetEffect(string element)
		{
			return $"Burns {GetElementLink(element)} to propel rockets for space exploration.";
		}
	}

	public partial class MethaneEngineStats
	{
		public static readonly string Element = "Methane";

		public static readonly string NAME = EngineBaseStats.GetElementLinkEngine(Element);
		public static readonly string DESC = EngineBaseStats.GetDescription(Element, ExhaustVelocity, MinFuel, MaxFuel);
		public static readonly string EFFECT = EngineBaseStats.GetEffect(Element);

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
		public static readonly string Element = "Hydrogen";

		public static readonly string NAME = EngineBaseStats.GetElementLinkEngine(Element);
		public static readonly string DESC = EngineBaseStats.GetDescription(Element, ExhaustVelocity, MinFuel, MaxFuel);
		public static readonly string EFFECT = EngineBaseStats.GetEffect(Element);

		public const string TechGroup = "EnginesIII";
		public const string BuildTab = "Rocketry";

		public const float ExhaustVelocity = 4423;
		public const float RangePenalty = 1200000;
		public const float MinFuel = 2200;
		public const float MaxFuel = 2900;
	}

	public class PetroleumEngineStats
	{
		public static readonly string Element = "Petroleum";

		public static readonly string NAME = EngineBaseStats.GetElementLinkEngine(Element);
		public static readonly string DESC = EngineBaseStats.GetDescription(Element, ExhaustVelocity, MinFuel, MaxFuel);
		public static readonly string EFFECT = EngineBaseStats.GetEffect(Element);

		public const string TechGroup = "EnginesII";
		public const string BuildTab = "Rocketry";

		public const float ExhaustVelocity = 2751;
		public const float RangePenalty = 435000;
		public const float MinFuel = 1200;
		public const float MaxFuel = 1700;
	}

	public class SteamEngineStats
	{
		public static readonly string Element = "Steam";

		public static readonly string NAME = EngineBaseStats.GetElementLinkEngine(Element);
		public static readonly string DESC = EngineBaseStats.GetDescription(Element, ExhaustVelocity, MinFuel, MaxFuel)
									+ $"Stores {StarmapScreenOverhaul.FormatDistance(MaxStorage, "", "kg")} of {EngineBaseStats.GetElementLink(Element)}. Can't utilize additional fuel tanks.";
		public static readonly string EFFECT = $"Uses {EngineBaseStats.GetElementLink(Element)} to propel rockets for space exploration.";

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
		private static readonly string Element1 = "Iron";
		private static readonly string Element2 = "Oxylite";

		public static readonly string NAME = UI.FormatAsLink("Solid Booster", nameof(SolidBooster).ToUpper());
		public static readonly string DESC = $"Boosters increase the range of a rocket:\n" +
									$"First:  {StarmapScreenOverhaul.FormatDistance(Ranges[0])}\n" +
									$"Second: {StarmapScreenOverhaul.FormatDistance(Ranges[1])}\n" +
									$"Third:  {StarmapScreenOverhaul.FormatDistance(Ranges[2])}\n";

		public static readonly string EFFECT = $"Burns {EngineBaseStats.GetElementLink(Element1)} and {EngineBaseStats.GetElementLink(Element2)} to boost the range of a rocket.";

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
