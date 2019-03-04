/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using STRINGS;

namespace RocketOverhaul
{
	public partial class EngineBase
	{
		public static string Equation = "distance = oxidizer_efficiency * (133320 * ((exhaust_velocity * fuel_in_kg * 0.00147) - fuel_in_kg^2) - distance_penalty)";
	}

	public class MethaneEngine
	{
		public static string Element = "Methane";

		public static string Id = Element + "Engine";
		public static string NAME = UI.FormatAsLink(Element + " Engine", Element + "Engine");
		public static string DESC = $"{Element} engines have an exhaust velocity of {ExhaustVelocity} m/s and a distance penalty of {DistancePenalty} Km. " +
									$"\n" + EngineBase.Equation;
		public static string EFFECT = "Burns " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public static readonly string TechGroup = "EnginesIII";
		public static readonly string BuildTab = "Rocketry";

		public static readonly float ExhaustVelocity = 6127;
		public static readonly float DistancePenalty = 2380000;
	}

	public partial class HydrogenEngine
	{
		public static string Element = "Hydrogen";

		public static string Id = Element + "Engine";
		public static string NAME = UI.FormatAsLink(Element + " Engine", Element + "Engine");
		public static string DESC = $"{Element} engines have an exhaust velocity of {ExhaustVelocity} m/s and a distance penalty of {DistancePenalty} Km. " +
									$"\n" + EngineBase.Equation;
		public static string EFFECT = "Burns " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public static readonly string TechGroup = "EnginesIII";
		public static readonly string BuildTab = "Rocketry";

		public static readonly float ExhaustVelocity = 4423;
		public static readonly float DistancePenalty = 1200000;
	}

	public class PetroleumEngine
	{
		public static string Element = "Petroleum";

		public static string Id = Element + "Engine";
		public static string NAME = UI.FormatAsLink(Element + " Engine", Element + "Engine");
		public static string DESC = $"{Element} engines have an exhaust velocity of {ExhaustVelocity} m/s and a distance penalty of {DistancePenalty} Km. " +
									$"\n" + EngineBase.Equation;
		public static string EFFECT = "Burns " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public static readonly string TechGroup = "EnginesII";
		public static readonly string BuildTab = "Rocketry";

		public static readonly float ExhaustVelocity = 2751;
		public static readonly float DistancePenalty = 435000;
	}

	public class SteamEngine
	{
		public static string Element = "Steam";

		public static string Id = Element + "Engine";
		public static string NAME = UI.FormatAsLink(Element + " Engine", Element + "Engine");
		public static string DESC = $"{Element} engines have an exhaust velocity of {ExhaustVelocity} m/s and a distance penalty of {DistancePenalty} Km. \n" +
									$"Internally stores {MaxStorage} Kg of {Element} \n" +
									EngineBase.Equation;
		public static string EFFECT = "Uses " + UI.FormatAsLink(Element, Element.ToUpper()) + " to propel rockets for space exploration.";

		public static readonly string TechGroup = "BasicRocketry";
		public static readonly string BuildTab = "Rocketry";

		public static readonly float ExhaustVelocity = 2751;
		public static readonly float DistancePenalty = 435000;
		public static readonly float MaxStorage = 500;
	}

	public class SolidBooster
	{
		private static string Element1 = "Iron";
		private static string Element2 = "Oxylite";

		public static string Id = nameof(SolidBooster);
		public static string NAME = UI.FormatAsLink("Solid Booster", nameof(SolidBooster));
		public static string DESC = $"Boosters increase the range of a rocket:\n" +
									$"First:  +30,000 Km\n" +
									$"Second: +20,000 Km\n" +
									$"Third:  +10,000 Km\n" +
									$"Fourth: +0      Km";

		public static string EFFECT = "Uses " + UI.FormatAsLink(Element1, Element1.ToUpper()) + " and " + UI.FormatAsLink(Element2, Element2.ToUpper()) + " to boost the range of a rocket.";

		public static readonly string TechGroup = "EnginesI";
		public static readonly string BuildTab = "Rocketry";
	}


}
