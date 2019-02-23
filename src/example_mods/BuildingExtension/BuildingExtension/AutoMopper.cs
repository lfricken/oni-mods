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

	internal class IDS
	{
		public const string ID = "AutoMopper";
		public const string NAME = "Auto Mopper";
		public const string DESCRIPTION = "The Auto Mopper pumps water from long distances.";
		public const string EFFECT = "Useful to sweep small amounts of Liquid.";
		public const string TECH = "PrecisionPlumbing";
		public const string PLANCATEGORY = "Plumbing";
	}
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class __LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS." + IDS.ID.ToUpper() + ".NAME", IDS.NAME);
			Strings.Add("STRINGS.BUILDINGS.PREFABS." + IDS.ID.ToUpper() + ".DESC", IDS.DESCRIPTION);
			Strings.Add("STRINGS.BUILDINGS.PREFABS." + IDS.ID.ToUpper() + ".EFFECT", IDS.EFFECT);

			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)IDS.PLANCATEGORY).Equals(po.category)).data;
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
	public class AutoMopper : IBuildingConfig
	{
		public const string ID = IDS.ID;
		private static readonly LogicPorts.Port[] INPUT_PORTS;

		public override BuildingDef CreateBuildingDef()
		{
			string id = IDS.ID;
			int width = 1;
			int height = 2;
			string anim = "miniwaterpump_kanim";
			int hitpoints = 100;
			float construction_time = 60f;
			float[] tieR4 = new float[1] {50f };
			string[] allMetals = new string[1]
	{
	  SimHashes.Steel.ToString()
	};
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
			buildingDef.PermittedRotations = PermittedRotations.R360;
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 10f;
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = 2f;
			buildingDef.OutputConduitType = ConduitType.Liquid;
			buildingDef.Floodable = false;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PowerInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 1);
			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, AutoMopper.INPUT_PORTS);
			AutoMopper.AddVisualizer(go, true);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, AutoMopper.INPUT_PORTS);
			AutoMopper.AddVisualizer(go, false);

		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, AutoMopper.INPUT_PORTS);
			go.AddOrGet<LogicOperationalController>();
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<EnergyConsumer>();
			go.AddOrGet<Pump>();
			go.AddOrGet<Storage>().capacityKg = 5f;
			ElementConsumer elementConsumer = go.AddOrGet<ElementConsumer>();
			elementConsumer.configuration = ElementConsumer.Configuration.AllLiquid;
			elementConsumer.consumptionRate = 2.5f;
			elementConsumer.storeOnConsume = true;
			elementConsumer.showInStatusPanel = false;
			elementConsumer.consumptionRadius = (byte)23;
			ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
			conduitDispenser.conduitType = ConduitType.Liquid;
			conduitDispenser.alwaysDispense = true;
			conduitDispenser.elementFilter = (SimHashes[])null;
			go.AddOrGetDef<OperationalController.Def>();
			AutoMopper.AddVisualizer(go, false);
		}
		
		private static void AddVisualizer(GameObject prefab, bool movable)
		{
			StationaryChoreRangeVisualizer choreRangeVisualizer = prefab.AddOrGet<StationaryChoreRangeVisualizer>();
			choreRangeVisualizer.x = -23;
			choreRangeVisualizer.y = -23;
			choreRangeVisualizer.width = 47;
			choreRangeVisualizer.height = 47;
			choreRangeVisualizer.movable = movable;
		}
		static AutoMopper()
		{
			AutoMopper.INPUT_PORTS = new LogicPorts.Port[1]
			{
	  LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 1), "Logic Port", false)
			};
		}
	}

}
