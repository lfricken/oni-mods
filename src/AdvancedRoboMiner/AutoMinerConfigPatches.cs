/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using TUNING;
using UnityEngine;

namespace AdvancedRoboMiner
{
	class AutoMinerConfigPatches
	{
		//[HarmonyPatch(typeof(AutoMinerConfig))]
		//[HarmonyPatch(nameof(AutoMinerConfig.CreateBuildingDef))]
		//public static class CreateBuildingDef
		//{
		//	static bool Prefix() { return false; } // skip original method
		//	static void Postfix(int cell, ref BuildingDef __result)
		//	{
		//		__result.EnergyConsumptionWhenActive = AdvancedRoboMiner.EnergyConsumption;
		//		__result.SelfHeatKilowattsWhenActive = AdvancedRoboMiner.HeatProduction;
		//	}
		//}

		//[HarmonyPatch(typeof(AutoMinerConfig))]
		//[HarmonyPatch(nameof(AutoMinerConfig.DoPostConfigureComplete))]
		//public static class DoPostConfigureComplete
		//{
		//	static bool Prefix() { return false; }
		//	static void Postfix(GameObject go)
		//	{
		//		AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
		//		autoMiner.width = AdvancedRoboMiner.Range.x;
		//		autoMiner.height = AdvancedRoboMiner.Range.y;
		//		autoMiner.x = AdvancedRoboMiner.Range.xOffset;
		//	}
		//}

		//[HarmonyPatch(typeof(AutoMinerConfig))]
		//[HarmonyPatch(nameof(AddVisualizer))]
		//public static class AddVisualizer
		//{
		//	static bool Prefix() { return false; }
		//	static void Postfix(GameObject go)
		//	{
		//		StationaryChoreRangeVisualizer choreRangeVisualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
		//		choreRangeVisualizer.width = AdvancedRoboMiner.Range.x;
		//		choreRangeVisualizer.height = AdvancedRoboMiner.Range.y;
		//		choreRangeVisualizer.x = AdvancedRoboMiner.Range.xOffset;
		//	}
		//}
	}
}
