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
		[HarmonyPatch(typeof(AutoMinerConfig))]
		[HarmonyPatch(nameof(AutoMinerConfig.CreateBuildingDef))]
		public static class CreateBuildingDef
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(int cell, ref BuildingDef __result)
			{
				BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("AutoMiner", 2, 2, "auto_miner_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
				buildingDef.Floodable = false;
				buildingDef.AudioCategory = "Metal";
				buildingDef.RequiresPowerInput = true;
				buildingDef.EnergyConsumptionWhenActive = AdvancedRoboMiner.EnergyConsumption;
				buildingDef.ExhaustKilowattsWhenActive = 0.0f;
				buildingDef.SelfHeatKilowattsWhenActive = AdvancedRoboMiner.HeatProduction;
				buildingDef.PermittedRotations = PermittedRotations.R360;
				GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "AutoMiner");
				__result = buildingDef;
			}
		}

		[HarmonyPatch(typeof(AutoMinerConfig))]
		[HarmonyPatch(nameof(AutoMinerConfig.DoPostConfigureComplete))]
		public static class DoPostConfigureComplete
		{
			static bool Prefix() { return false; }
			static void Postfix(GameObject go)
			{
				AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
				autoMiner.width = AdvancedRoboMiner.Range.x;
				autoMiner.height = AdvancedRoboMiner.Range.y;
				autoMiner.x = AdvancedRoboMiner.Range.xOffset;
			}
		}

		[HarmonyPatch(typeof(AutoMinerConfig))]
		[HarmonyPatch(nameof(AddVisualizer))]
		public static class AddVisualizer
		{
			static bool Prefix() { return false; }
			static void Postfix(GameObject go, bool movable)
			{
				StationaryChoreRangeVisualizer choreRangeVisualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
				choreRangeVisualizer.width = AdvancedRoboMiner.Range.x;
				choreRangeVisualizer.height = AdvancedRoboMiner.Range.y;
				choreRangeVisualizer.x = AdvancedRoboMiner.Range.xOffset;
			}
		}
	}
}
