/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using STRINGS;

namespace BuildableSteamGeyser
{
	public partial class BuildableSteamGeyser
	{
		public static readonly string DisplayName = "Geothermal Steam Pump";
		public static readonly string Description = $"Pumps large amounts of " + UI.FormatAsLink("Steam", "STEAM") +
			$" at {CelciusOutputTemperature}°C out of the ground. Can't be turned off once active.";
		public static readonly LocString Effect = (LocString)("Produces " + UI.FormatAsLink("Steam", "STEAM") + ".");

		public static readonly string TechGroup = "ValveMiniaturization";
		public static readonly string BuildTab = "Plumbing";

		public const float CelciusOutputTemperature = 175f;
		public const float EmissionRate = 100f;
	}
}
