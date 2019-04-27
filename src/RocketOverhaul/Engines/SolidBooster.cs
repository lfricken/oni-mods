/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;

namespace RocketOverhaul
{
	public class SolidBoosterConfigPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class LoadGeneratedBuildings
		{
			static void Postfix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SolidBoosterConfig.ID.ToUpperInvariant()}.NAME", SolidBoosterStats.NAME);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SolidBoosterConfig.ID.ToUpperInvariant()}.DESC", SolidBoosterStats.DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SolidBoosterConfig.ID.ToUpperInvariant()}.EFFECT", SolidBoosterStats.EFFECT);
			}
		}
	}
}
