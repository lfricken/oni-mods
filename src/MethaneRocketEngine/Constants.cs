/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using STRINGS;
using TUNING;

namespace MethaneRocketEngine
{
	public partial class MethaneRocketEngine
	{
		public static string Id = nameof(MethaneRocketEngine);
		public static string NAME = UI.FormatAsLink("Methane Engine", nameof(MethaneRocketEngine));
		public static string DESC = $"Methane engines can propel rockets {EfficiencyMultiplier} times further than hydrogen engines.";
		public static string EFFECT = "Burns " + UI.FormatAsLink("Methane", "METHANE") + " to propel rockets for space exploration.\n\nThe engine of a rocket must be built first before more rocket modules may be added.";

		public static string TechGroup = "EnginesIII";
		public static string BuildTab = "Rocketry";

		public static float Efficiency = ROCKETRY.ENGINE_EFFICIENCY.STRONG * EfficiencyMultiplier;


		/// <summary>
		/// How much stronger is this engine than a hydrogen rocket?
		/// </summary>
		private static float EfficiencyMultiplier = 3f;
	}
}
