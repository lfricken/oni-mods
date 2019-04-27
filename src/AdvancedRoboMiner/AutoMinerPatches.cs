/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;

namespace AdvancedRoboMiner
{
	class AutoMinerPatches
	{
		[HarmonyPatch(typeof(AutoMiner))]
		[HarmonyPatch(nameof(ValidDigCell))]
		public static class ValidDigCell
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(int cell, ref bool __result)
			{
				__result = Grid.Solid[cell] && !Grid.Foundation[cell] && Grid.Element[cell].hardness < AdvancedRoboMiner.MaxHardness;
			}
		}
	}
}
