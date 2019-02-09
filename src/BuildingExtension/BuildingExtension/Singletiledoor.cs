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
	internal class SmallPressureDoorConfig_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SMALLPRESSUREDOOR.NAME", "Small Mechanized Airlock");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SMALLPRESSUREDOOR.DESC", "One tile Version of the Mechanized Airlock. ");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SMALLPRESSUREDOOR.EFFECT", "If you want to have an Airlock on small space.(please note its maller than it looks)");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Base").Equals(po.category)).data;
			category.Add(SmallPressureDoorConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(SmallPressureDoorConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class SmallPressureDoorConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["DirectedAirStreams"]);
			ls.Add(SmallPressureDoorConfig.ID);
			Database.Techs.TECH_GROUPING["DirectedAirStreams"] = (string[])ls.ToArray();
		}
	}

	public class SmallPressureDoorConfig : IBuildingConfig
	{
		public const string ID = "SmallPressureDoor";


		public override BuildingDef CreateBuildingDef()
		{
			string id = "SmallPressureDoor";
			int width = 1;
			int height = 1;
			string anim = "door_external_kanim";
			int hitpoints = 30;
			float construction_time = 60f;
			float[] tieR4 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
			string[] allMetals = MATERIALS.ALL_METALS;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Tile;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 1f);
			buildingDef.Overheatable = false;
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 120f;
			buildingDef.Entombable = false;
			buildingDef.IsFoundation = true;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.TileLayer = ObjectLayer.Backwall;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PermittedRotations = PermittedRotations.R90;
			buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
			//buildingDef.ObjectLayer = ObjectLayer.Backwall;
			SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Open_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
			SoundEventVolumeCache.instance.AddVolume("door_external_kanim", "Close_DoorPressure", NOISE_POLLUTION.NOISY.TIER2);
			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			Door door = go.AddOrGet<Door>();
			door.hasComplexUserControls = true;
			door.unpoweredAnimSpeed = 0.65f;
			door.poweredAnimSpeed = 5f;
			door.doorClosingSoundEventName = "MechanizedAirlock_closing";
			door.doorOpeningSoundEventName = "MechanizedAirlock_opening";
			go.AddOrGet<AccessControl>();
			go.AddOrGet<KBoxCollider2D>();
			Prioritizable.AddRef(go);
			go.AddOrGet<Workable>().workTime = 5f;
			GeneratedBuildings.RegisterLogicPorts(go, DoorConfig.INPUT_PORTS);
			UnityEngine.Object.DestroyImmediate((UnityEngine.Object)go.GetComponent<BuildingEnabledButton>());
			go.GetComponent<AccessControl>().controlEnabled = true;
			go.GetComponent<KBatchedAnimController>().initialAnim = "closed";
		}
	}

}
