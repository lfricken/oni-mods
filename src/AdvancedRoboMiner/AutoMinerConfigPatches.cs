/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using System;
using UnityEngine;

namespace AdvancedRoboMiner
{
	class AutoMinerConfigPatches
	{
		[HarmonyPatch(typeof(AutoMinerConfig))]
		[HarmonyPatch(nameof(AutoMinerConfig.CreateBuildingDef))]
		public static class CreateBuildingDef
		{
			static bool Prefix() { return false; }
			static void Postfix(ref BuildingDef __result)
			{
				__result.EnergyConsumptionWhenActive = AdvancedRoboMiner.EnergyConsumption;
				__result.SelfHeatKilowattsWhenActive = AdvancedRoboMiner.HeatProduction;
			}
		}

		[HarmonyPatch(typeof(AutoMinerConfig))]
		[HarmonyPatch(nameof(AutoMinerConfig.DoPostConfigureComplete))]
		public static class DoPostConfigureComplete
		{
			static bool Prefix() { return true; }
			static void Postfix(GameObject go)
			{
				GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
				go.AddOrGet<LogicOperationalController>();

				AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
				autoMiner.width = AdvancedRoboMiner.Range.width;
				autoMiner.height = AdvancedRoboMiner.Range.height;
				autoMiner.x = AdvancedRoboMiner.Range.xOffset;
				autoMiner.y = 0;
				autoMiner.vision_offset = AdvancedRoboMiner.Range.VisionOffset;

				AddVisualizer(go, false);
			}
		}

		[HarmonyPatch(typeof(AutoMinerConfig))]
		[HarmonyPatch(nameof(AddVisualizer))]
		public static class AddVisualizerPatch
		{
			static bool Prefix() { return true; }
			static void Postfix(GameObject prefab, bool movable)
			{
				AddVisualizer(prefab, movable);
			}
		}

		private static void AddVisualizer(GameObject go, bool movable)
		{
			StationaryChoreRangeVisualizer choreRangeVisualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
			choreRangeVisualizer.width = AdvancedRoboMiner.Range.width;
			choreRangeVisualizer.height = AdvancedRoboMiner.Range.height;
			choreRangeVisualizer.x = AdvancedRoboMiner.Range.xOffset;
			choreRangeVisualizer.y = 0;
			choreRangeVisualizer.vision_offset = AdvancedRoboMiner.Range.VisionOffset;
			choreRangeVisualizer.movable = movable;
			choreRangeVisualizer.blocking_tile_visible = false;

			go.GetComponent<KPrefabID>().instantiateFn += (prefab =>
			{
				StationaryChoreRangeVisualizer component = prefab.GetComponent<StationaryChoreRangeVisualizer>();
				Func<int, bool> callback = AutoMiner.DigBlockingCB;
				component.blocking_cb = callback;
			});
		}
	}
}
