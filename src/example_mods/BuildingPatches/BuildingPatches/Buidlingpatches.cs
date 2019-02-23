using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace BuildingPatches
{
	public class Buidlingpatches
	{
		[HarmonyPatch(typeof(SuitLockerConfig), "CreateBuildingDef", null)]
		public static class SuitLockerConfigMod
		{
			public static void Postfix(BuildingDef __result)
			{
				//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
				__result.Floodable = false;
			}
		}
		[HarmonyPatch(typeof(SuitMarkerConfig), "CreateBuildingDef", null)]
		public static class SuitMarkerConfigMod
		{
			public static void Postfix(BuildingDef __result)
			{
				//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
				__result.Floodable = false;
			}
		}
		[HarmonyPatch(typeof(TravelTubeEntranceConfig), "CreateBuildingDef", null)]
		public static class TravelTubeEntranceConfigMod
		{
			public static void Postfix(BuildingDef __result)
			{
				//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
				__result.Floodable = false;
			}
		}


		[HarmonyPatch(typeof(JetSuitLockerConfig), "CreateBuildingDef", null)]
		public static class JetSuitLockerConfigMod
		{
			public static void Postfix(BuildingDef __result)
			{
				//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
				__result.Floodable = false;
			}
		}
		[HarmonyPatch(typeof(JetSuitMarkerConfig), "CreateBuildingDef", null)]
		public static class JetSuitMarkerConfigMod
		{
			public static void Postfix(BuildingDef __result)
			{
				//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
				__result.Floodable = false;
			}
		}
		[HarmonyPatch(typeof(GasFilterConfig), "CreateBuildingDef", null)]
		public static class GasFilterMod
		{
			public static void Postfix(BuildingDef __result)
			{
				__result.EnergyConsumptionWhenActive = 10f;
			}
		}
		[HarmonyPatch(typeof(LiquidFilterConfig), "CreateBuildingDef", null)]
		public static class LiquidFilterConfigMod
		{
			public static void Postfix(BuildingDef __result)
			{
				__result.EnergyConsumptionWhenActive = 10f;
			}
		}
		/*
		[HarmonyPatch(typeof(AtmoSuitConfig), "CreateEquipmentDef", null)]
		public static class AtmoSuitConfigMod
		{
			public static void Postfix(EquipmentDef __result)
			{
				__result.EffectImmunites.Add(Db.Get().effects.Get("SUNLIGHT_BURNING"));
			}
		}
		[HarmonyPatch(typeof(BottleEmptierConfig), "CreateBuildingDef", null)]
		public static class BottleEmptierConfigMod
		{
			public static void Postfix(BuildingDef __result)
			{
				//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
				__result.OutputConduitType = ConduitType.Liquid;
				__result.UtilityInputOffset = new CellOffset(1, 0);
			}
		}*/
		[HarmonyPatch(typeof(BottleEmptierConfig), "ConfigureBuildingTemplate", null)]
		public static class BottleEmptierConfigMo
		{
			static public void Postfix(ref GameObject go)
			{

				/*.outputOffset = new Vector3(1f, 0.5f);
				//glassForge.outStorage.capacityKg = 2000f;
				//Prioritizable.AddRef(go);
				Storage storage = go.AddOrGet<Storage>();
				storage.storageFilters = STORAGEFILTERS.LIQUIDS;
				storage.showInUI = true;
				storage.showDescriptor = true;
				storage.capacityKg = 200f;
				go.AddOrGet<TreeFilterable>();
				go.AddOrGet<BottleEmptier>();
				Storage storage = go.AddOrGet<Storage>();
				ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
				conduitDispenser.storage = storage;
				conduitDispenser.conduitType = ConduitType.Liquid;
				conduitDispenser.elementFilter = (SimHashes[])null;
				conduitDispenser.alwaysDispense = true;*/
				Storage storage = go.AddOrGet<Storage>();
				storage.capacityKg = 20000f;
			}
		}
		[HarmonyPatch(typeof(FlushToiletConfig), "ConfigureBuildingTemplate", null)]
		public static class FlushToiletConfiggMod
		{
			public static void Prefix(ref GameObject go, BuildingDef __instance)
			{
				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.capacityTag = GameTags.AnyWater;
			}
		}

		/*
		[HarmonyPatch(typeof(SolarPanelConfig), "CreateBuildingDef", null)]
		public static class SolarPanelConfigMod
		{
			public static void Prefix(BuildingDef __instance)
			{
				AccessTools.Field(typeof(SolarPanelConfig), "MAX_WATTS").SetValue(__instance, 500f);
			}
		}*/
		[HarmonyPatch(typeof(LogicSwitchConfig), "ConfigureBuildingTemplate", null)]
		public static class ConfigureBuildingTemplate
		{
			public static void Postfix(ref GameObject go)
			{
				go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
			}
		}

	}
	
}
