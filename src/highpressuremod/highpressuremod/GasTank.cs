using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;


namespace PressureGasReservoir
{
	internal class IDS
	{
		public const string ID = "PressureGasReservoir";
		public const string NAME = "Pressure Gas Reservoir";
		public const string DESCRIPTION = "The Pressure Gas Reservoir stores Pressurized Gas.";
		public const string EFFECT = "It holds vast amounts of Gas.";
		public const string TECH = "PrecisionPlumbing";
	}
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class __LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS." + IDS.ID.ToUpper() + ".NAME", IDS.NAME);
			Strings.Add("STRINGS.BUILDINGS.PREFABS." + IDS.ID.ToUpper() + ".DESC", IDS.DESCRIPTION);
			Strings.Add("STRINGS.BUILDINGS.PREFABS." + IDS.ID.ToUpper() + ".EFFECT", IDS.EFFECT);

			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Base").Equals(po.category)).data;
			category.Add(IDS.ID);

			//TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(PressureLiquidReservoirConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class __Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING[IDS.TECH]);
			ls.Add(IDS.ID);
			Database.Techs.TECH_GROUPING[IDS.TECH] = (string[])ls.ToArray();
		}
	}
}
namespace BuildingExtension
{
	public class PressureGasReservoirConfig : IBuildingConfig
	{
		public const string ID = PressureGasReservoir.IDS.ID;
		private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
		private const int WIDTH = 5;
		private const int HEIGHT = 3;
		public static readonly List<Storage.StoredItemModifier> ReservoirStoredItemModifiers;


		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(PressureGasReservoir.IDS.ID, 5, 3, "gasstorage_kanim", 100, 120f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER5, new string[1] { SimHashes.Steel.ToString() }, 800f, BuildLocationRule.OnFloor, BUILDINGS.DECOR.PENALTY.TIER1, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.InputConduitType = ConduitType.Gas;
			buildingDef.OutputConduitType = ConduitType.Gas;
			buildingDef.Floodable = false;
			buildingDef.ViewMode = OverlayModes.GasConduits.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.UtilityInputOffset = new CellOffset(1, 2);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<Reservoir>();
			Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
			defaultStorage.showDescriptor = true;
			defaultStorage.storageFilters = STORAGEFILTERS.GASES;
			defaultStorage.capacityKg = 15000f;
			defaultStorage.SetDefaultStoredItemModifiers(GasReservoirConfig.ReservoirStoredItemModifiers);
			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Gas;
			conduitConsumer.ignoreMinMassCheck = true;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.alwaysConsume = true;
			conduitConsumer.capacityKG = defaultStorage.capacityKg;
			ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
			conduitDispenser.conduitType = ConduitType.Gas;
			conduitDispenser.elementFilter = (SimHashes[])null;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<StorageController.Def>();
		}

	}


}
