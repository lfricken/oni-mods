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
		public static bool log = true;
		public static void TryLog(string message)
		{
			if (log)
				Debug.Log(message);
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
				TryLog(nameof(GetAverageOxidizerEfficiency) + ": " + __result);
			}
		}

		[HarmonyPatch(typeof(RocketStats))]
		[HarmonyPatch(nameof(RocketStats.GetBoosterThrust))]
		public static class GetBoosterThrust
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(RocketStats __instance, ref float __result)
			{
				RocketStatsOverhaul _this = (RocketStatsOverhaul)__instance;
				__result = _this.GetBoosterThrust();
				TryLog(nameof(GetBoosterThrust) + ": " + __result);
			}
		}

		[HarmonyPatch(typeof(RocketStats))]
		[HarmonyPatch(nameof(RocketStats.GetRocketMaxDistance))]
		public static class GetRocketMaxDistance
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(RocketStats __instance, ref float __result)
			{
				RocketStatsOverhaul _this = (RocketStatsOverhaul)__instance;
				__result = _this.GetRocketMaxDistance();
				TryLog(nameof(GetRocketMaxDistance) + ": " + __result);
			}
		}

		[HarmonyPatch(typeof(RocketStats))]
		[HarmonyPatch(nameof(RocketStats.GetTotalThrust))]
		public static class GetTotalThrust
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(RocketStats __instance, ref float __result)
			{
				RocketStatsOverhaul _this = (RocketStatsOverhaul)__instance;
				__result = _this.GetTotalThrust();
				TryLog(nameof(GetTotalThrust) + ": " + __result);
			}
		}
	}
}
