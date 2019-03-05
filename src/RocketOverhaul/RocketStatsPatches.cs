/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;

namespace RocketOverhaul
{
	/// <summary>
	/// Replaces methods on <see cref="RocketStats"/> since it isn't virtual.
	/// </summary>
	public class RocketStatsPatches
	{
		[HarmonyPatch(typeof(RocketStats))]
		[HarmonyPatch(nameof(RocketStats.GetRocketMaxDistance))]
		public static class GetRocketMaxDistance
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(RocketStats __instance, ref float __result)
			{
				RocketStatsOverhaul _this = (RocketStatsOverhaul)__instance;
				__result = _this.GetRocketMaxDistance();
			}
		}

		[HarmonyPatch(typeof(RocketStats))]
		[HarmonyPatch(nameof(RocketStats.GetAverageOxidizerEfficiency))]
		public static class GetAverageOxidizerEfficiency
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(RocketStats __instance, ref float __result)
			{
				RocketStatsOverhaul _this = (RocketStatsOverhaul)__instance;
				__result = _this.GetAverageOxidizerEfficiency();
			}
		}
	}
}
