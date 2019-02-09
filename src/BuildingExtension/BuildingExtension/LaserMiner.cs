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
	internal class LaserMinerConfig_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERMINER.NAME", "Laser Miner");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERMINER.DESC", "The Laser Miner Drills a Long Hole.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.LASERMINER.EFFECT", "The Laser Miner is perfect to drill tunnels or clean comet residue.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Conveyance").Equals(po.category)).data;
			category.Add(LaserMinerConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(LaserMinerConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class LaserMinerConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["SolidTransport"]);
			ls.Add(LaserMinerConfig.ID);
			Database.Techs.TECH_GROUPING["SolidTransport"] = (string[])ls.ToArray();
		}
	}
	public class LaserMinerConfig : IBuildingConfig
	{
		public const string ID = "LaserMiner";
		private const int RANGE = 60;
		private const int X = 0;
		private const int Y = 0;
		private const int WIDTH = 60;
		private const int HEIGHT = 4;
		private const int VISION_OFFSET = 1;
		private static readonly LogicPorts.Port[] INPUT_PORTS;

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LaserMiner", 2, 2, "auto_miner_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, TUNING.NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 120f;
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = 2f;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "LaserMiner");
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<Operational>();
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<MiningSounds>();
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LaserMinerConfig.INPUT_PORTS);
			LaserMinerConfig.AddVisualizer(go, true);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LaserMinerConfig.INPUT_PORTS);
			LaserMinerConfig.AddVisualizer(go, false);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LaserMinerConfig.INPUT_PORTS);
			go.AddOrGet<LogicOperationalController>();
			AutoMiner autoMiner = go.AddOrGet<AutoMiner>();
			autoMiner.x = X;
			autoMiner.y = Y;
			autoMiner.width = WIDTH;
			autoMiner.height = HEIGHT;
			autoMiner.vision_offset = new CellOffset(0, 1);
			LaserMinerConfig.AddVisualizer(go, false);
		}

		private static void AddVisualizer(GameObject prefab, bool movable)
		{
			StationaryChoreRangeVisualizer choreRangeVisualizer = prefab.AddOrGet<StationaryChoreRangeVisualizer>();
			choreRangeVisualizer.x = X;
			choreRangeVisualizer.y = Y;
			choreRangeVisualizer.width = WIDTH;
			choreRangeVisualizer.height = HEIGHT;
			choreRangeVisualizer.vision_offset = new CellOffset(0, 1);
			choreRangeVisualizer.movable = movable;
			choreRangeVisualizer.blocking_tile_visible = false;
			KPrefabID component = prefab.GetComponent<KPrefabID>();
		}

		static LaserMinerConfig()
		{
			LaserMinerConfig.INPUT_PORTS = new LogicPorts.Port[1]
			{
	  LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), "Logic Port", false)
			};
		}

	}
}
