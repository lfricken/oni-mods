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
		public static readonly float Exhaust = 0.00146959f;
		public static readonly int Exponent = 2;
		public static readonly float Range = 133320.0f;
		public static readonly float FuelPerXTick = 900f;
	}

	public class ModulePenalties
	{
		public static readonly float ResearchPenalty = 2000f;
		public static readonly float TouristPenalty = 8000f;
		public static readonly float CargoPenalty = 30000f;
	}

	public class OxidizerEfficiency
	{
		public static readonly float Mixed = 1.2f;
		public static readonly float Lox = 1.0f;
		public static readonly float OxyRock = 0.8f;
	}

	public partial class EngineBaseStats
	{
		public static string Equation = "\ndistance = oxidizer_efficiency * (133320 * ((exhaust_velocity * fuel_in_kg * 0.00147) - fuel_in_kg^2) - distance_penalty)";

	}

	public partial class MethaneEngineStats
	{
		public static string Element = "Methane";

		public static string NAME = UI.FormatAsLink(Element + " Engine", Element + "Engine");
		public static string DESC = $"{Element} engines have an exhaust velocity of {ExhaustVelocity} m/s and a distance penalty of {DistancePenalty} km. " +
									$"\n" + EngineBaseStats.Equation;
		public static string EFFECT = "Burns " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public static readonly string TechGroup = "EnginesIII";
		public static readonly string BuildTab = "Rocketry";

		public static readonly SimHashes FuelType = SimHashes.LiquidMethane;
		public static readonly float ExhaustVelocity = 6127;
		public static readonly float DistancePenalty = 2380000;
		public static readonly float MinFuel = 3200;
		public static readonly float MaxFuel = 4000;
	}

	public partial class HydrogenEngineStats
	{
		public static string Element = "Hydrogen";

		public static string NAME = UI.FormatAsLink(Element + " Engine", Element + "Engine");
		public static string DESC = $"{Element} engines have an exhaust velocity of {ExhaustVelocity} m/s and a distance penalty of {DistancePenalty} km. " +
									$"\n" + EngineBaseStats.Equation;
		public static string EFFECT = "Burns " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public static readonly string TechGroup = "EnginesIII";
		public static readonly string BuildTab = "Rocketry";

		public static readonly float ExhaustVelocity = 4423;
		public static readonly float DistancePenalty = 1200000;
		public static readonly float MinFuel = 2200;
		public static readonly float MaxFuel = 2900;
	}

	public class PetroleumEngineStats
	{
		public static string Element = "Petroleum";

		public static string NAME = UI.FormatAsLink(Element + " Engine", Element + "Engine");
		public static string DESC = $"{Element} engines have an exhaust velocity of {ExhaustVelocity} m/s and a distance penalty of {DistancePenalty} km. " +
									$"\n" + EngineBaseStats.Equation;
		public static string EFFECT = "Burns " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public static readonly string TechGroup = "EnginesII";
		public static readonly string BuildTab = "Rocketry";

		public static readonly float ExhaustVelocity = 2751;
		public static readonly float DistancePenalty = 435000;
		public static readonly float MinFuel = 1200;
		public static readonly float MaxFuel = 1700;
	}

	public class SteamEngineStats
	{
		public static string Element = "Steam";

		public static string NAME = UI.FormatAsLink(Element + " Engine", Element + "Engine");
		public static string DESC = $"{Element} engines have an exhaust velocity of {ExhaustVelocity} m/s and a distance penalty of {DistancePenalty} km. \n" +
									$"Internally stores {MaxStorage} Kg of {Element} \n" +
									EngineBaseStats.Equation;
		public static string EFFECT = "Uses " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public static readonly string TechGroup = "BasicRocketry";
		public static readonly string BuildTab = "Rocketry";

		public static readonly float ExhaustVelocity = 2751;
		public static readonly float DistancePenalty = 435000;
		public static readonly float MinFuel = 250;
		public static readonly float MaxFuel = 500;
		public static readonly float MaxStorage = 500;
	}

	public class SolidBoosterStats
	{
		private static string Element1 = "Iron";
		private static string Element2 = "Oxylite";

		public static string NAME = UI.FormatAsLink("Solid Booster", nameof(SolidBooster));
		public static string DESC = $"Boosters increase the range of a rocket:\n" +
									$"First:  +30,000 km\n" +
									$"Second: +20,000 km\n" +
									$"Third:  +10,000 km\n" +
									$"Fourth: +0      km";

		public static string EFFECT = "Uses " + UI.FormatAsLink(Element1, Element1.ToUpper()) + " and " + UI.FormatAsLink(Element2, Element2.ToUpper()) + " to boost the range of a rocket.";

		public static readonly string TechGroup = "EnginesI";
		public static readonly string BuildTab = "Rocketry";

		public static readonly float MaxFuelStorage = 400;
		public static readonly float MaxOxyStorage = 400;

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
