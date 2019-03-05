/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using System;

namespace RocketOverhaul
{
	public class CommandModule_Patch
	{
		[HarmonyPatch(typeof(CommandModule))]
		[HarmonyPatch("OnPrefabInit")]
		//[HarmonyPatch(new Type[0])]
		public static class OnPrefabInit
		{
			static void Postfix(CommandModule __instance)
			{
				__instance.rocketStats = new RocketStatsOverhaul(__instance);
			}
		}
	}
}
