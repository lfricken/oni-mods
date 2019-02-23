using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;

namespace BuildingExtension
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class PressureLiquidPump_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.PRESSURELIQUIDPUMP.NAME", "Pressure Liquid Pump");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.PRESSURELIQUIDPUMP.DESC", "The Pressure Liquid Pump pumps Liquid at high pressure.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.PRESSURELIQUIDPUMP.EFFECT", "Useful to move large amounts of Liquid.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);

			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Plumbing").Equals(po.category)).data;

			category.Add(PressureLiquidPumpConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(PressureLiquidPumpConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class PressureLiquidPump_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["PrecisionPlumbing"]);
			ls.Add(PressureLiquidPumpConfig.ID);
			Database.Techs.TECH_GROUPING["PrecisionPlumbing"] = (string[])ls.ToArray();
		}
	}

	public class PressureLiquidPumpConfig : IBuildingConfig
	{
		public const string ID = "PressureLiquidPump";
		private static readonly LogicPorts.Port[] INPUT_PORTS;

		public override BuildingDef CreateBuildingDef()
		{
			string id = "PressureLiquidPump";
			int width = 2;
			int height = 2;
			string anim = "pumpliquid_kanim";
			int hitpoints = 100;
			float construction_time = 300f;
			float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
			string[] allMetals = new string[1]
	{
	  SimHashes.Steel.ToString()
	};
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 490f;
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = 2f;
			buildingDef.OutputConduitType = ConduitType.Liquid;
			buildingDef.Floodable = false;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PowerInputOffset = new CellOffset(0, 1);
			buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, PressureLiquidPumpConfig.INPUT_PORTS);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, PressureLiquidPumpConfig.INPUT_PORTS);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, PressureLiquidPumpConfig.INPUT_PORTS);
			go.AddOrGet<LogicOperationalController>();
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<EnergyConsumer>();
			go.AddOrGet<Pump>();
			go.AddOrGet<Storage>().capacityKg = 400f;
			ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
			elementConsumer.configuration = ElementConsumer.Configuration.AllLiquid;
			elementConsumer.consumptionRate = 200f;
			elementConsumer.storeOnConsume = true;
			elementConsumer.showInStatusPanel = false;
			elementConsumer.consumptionRadius = (byte)2;
			ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
			conduitDispenser.conduitType = ConduitType.Liquid;
			conduitDispenser.alwaysDispense = true;
			conduitDispenser.elementFilter = (SimHashes[])null;
			go.AddOrGetDef<OperationalController.Def>();
		}

		static PressureLiquidPumpConfig()
		{
			PressureLiquidPumpConfig.INPUT_PORTS = new LogicPorts.Port[1]
			{
	  LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 1), "Logic Port", false)
			};
		}
	}

}
