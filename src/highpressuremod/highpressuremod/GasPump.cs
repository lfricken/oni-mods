using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;

namespace BuildingExtension { 

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class GasPumpConfig_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.PRESSUREGASPUMP.NAME", "Pressure Gas Pump");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.PRESSUREGASPUMP.DESC", "The Pressure Gas Pump pumps gas at high pressure.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.PRESSUREGASPUMP.EFFECT", "Useful to move large amounts of gas.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"HVAC").Equals(po.category)).data;
			category.Add(PressureGasPumpConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(PressureGasPumpConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class GasPumpConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["PrecisionPlumbing"]);
			ls.Add(PressureGasPumpConfig.ID);
			Database.Techs.TECH_GROUPING["PrecisionPlumbing"] = (string[])ls.ToArray();
		}
	}

	public class PressureGasPumpConfig : IBuildingConfig
	{
		public const string ID = "PressureGasPump";
		private static readonly LogicPorts.Port[] INPUT_PORTS;


		public override BuildingDef CreateBuildingDef()
		{
			string id = "PressureGasPump";
			int width = 2;
			int height = 2;
			string anim = "pumpgas_kanim";
			int hitpoints = 30;
			float construction_time = 150f;
			float[] tieR1 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
			string[] allMetals = new string[1]
	{
	  SimHashes.Steel.ToString()
	};
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues tieR2 = TUNING.NOISE_POLLUTION.NOISY.TIER2;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, tieR2, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 490f;
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
			buildingDef.OutputConduitType = ConduitType.Gas;
			buildingDef.Floodable = true;
			buildingDef.ViewMode = OverlayModes.GasConduits.ID; 
			buildingDef.AudioCategory = "Metal";
			buildingDef.PowerInputOffset = new CellOffset(0, 1);
			buildingDef.UtilityOutputOffset = new CellOffset(1, 1);
			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, PressureGasPumpConfig.INPUT_PORTS);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, PressureGasPumpConfig.INPUT_PORTS);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, PressureGasPumpConfig.INPUT_PORTS);
			go.AddOrGet<LogicOperationalController>();
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<EnergyConsumer>();
			go.AddOrGet<Pump>();
			go.AddOrGet<Storage>().capacityKg = 20f;
			ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
			elementConsumer.configuration = ElementConsumer.Configuration.AllGas;
			elementConsumer.consumptionRate = 10f;
			elementConsumer.storeOnConsume = true;
			elementConsumer.showInStatusPanel = false;
			elementConsumer.consumptionRadius = (byte)5;
			ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
			conduitDispenser.conduitType = ConduitType.Gas;
			conduitDispenser.alwaysDispense = true;
			conduitDispenser.elementFilter = (SimHashes[])null;
			go.AddOrGetDef<OperationalController.Def>();
		}

		static PressureGasPumpConfig()
		{
			PressureGasPumpConfig.INPUT_PORTS = new LogicPorts.Port[1]
			{
	  LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 1), "Logic Port", false)
			};
		}
	}

}
