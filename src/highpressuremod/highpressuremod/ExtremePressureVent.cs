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
	internal class GasVentExtremePressure_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.GASVENTEXTREMEPRESSURE.NAME", "Extreme Pressure Gas Vent");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.GASVENTEXTREMEPRESSURE.DESC", "Extreme Pressure Gas Vent can exhaust gas under Extreme pressure.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.GASVENTEXTREMEPRESSURE.EFFECT", "Releases Gas into Extreme pressure locations.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"HVAC").Equals(po.category)).data;
			category.Add(GasVentExtremePressureConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(GasVentExtremePressureConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class GasVentExtremePressure_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["PrecisionPlumbing"]);
			ls.Add(GasVentExtremePressureConfig.ID);
			Database.Techs.TECH_GROUPING["PrecisionPlumbing"] = (string[])ls.ToArray();
		}
	}

	public class GasVentExtremePressureConfig : IBuildingConfig
	{
		public const string ID = "GasVentExtremePressure";
		private const ConduitType CONDUIT_TYPE = ConduitType.Gas;
		public const float OVERPRESSURE_MASS = 4000f;
		
		public override BuildingDef CreateBuildingDef()
		{
			string id = "GasVentExtremePressure";
			int width = 1;
			int height = 1;
			string anim = "ventgas_powered_kanim";
			int hitpoints = 30;
			float construction_time = 150f;
			string[] construction_materials = new string[2]
			{
	 SimHashes.Steel.ToString(),
	  "Plastic"
			};
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[2]
			{
	  BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
	  BUILDINGS.CONSTRUCTION_MASS_KG.TIER1[0]
			}, construction_materials, 1600f, BuildLocationRule.Anywhere, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
			buildingDef.InputConduitType = ConduitType.Gas;
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.ViewMode = OverlayModes.GasConduits.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			SoundEventVolumeCache.instance.AddVolume("ventgas_kanim", "GasVent_clunk", NOISE_POLLUTION.NOISY.TIER0);
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<Exhaust>();
			Vent vent = go.AddOrGet<Vent>();
			vent.conduitType = ConduitType.Gas;
			vent.endpointType = Endpoint.Sink;
			vent.overpressureMass = 400f;
			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Gas;
			conduitConsumer.ignoreMinMassCheck = true;
			BuildingTemplates.CreateDefaultStorage(go, false).showInUI = true;
			go.AddOrGet<SimpleVent>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGetDef<VentController.Def>();
		}
	}


}
