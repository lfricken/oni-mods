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
	internal class SuperSolarPanel_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERSOLARPANEL.NAME", "Super Solar Panel");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERSOLARPANEL.DESC", "The Super Solar Panel produes double power.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERSOLARPANEL.EFFECT", "A superior Panel with more power for spacefaring colonys.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Power").Equals(po.category)).data;
			category.Add(SuperSolarPanelConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(SuperSolarPanelConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class SuperSolarPanel_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["RenewableEnergy"]);
			ls.Add(SuperSolarPanelConfig.ID);
			Database.Techs.TECH_GROUPING["RenewableEnergy"] = (string[])ls.ToArray();
		}
	}
	
	public class SuperSolarPanelConfig : IBuildingConfig
	{
		public const string ID = "SuperSolarPanel";
		public const float WATTS_PER_LUX = 0.00103f;
		public const float MAX_WATTS = 1000f;

		public override BuildingDef CreateBuildingDef()
		{
			string id = "SuperSolarPanel";
			int width = 7;
			int height = 3;
			string anim = "solar_panel_kanim";
			int hitpoints = 100;
			float construction_time = 320f;
			float[] tieR3 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
			float[] construction_mass = new float[3] { BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0], BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0] };
			string[] construction_materials = new string[3]
			{
				SimHashes.Glass.ToString(),
				SimHashes.TempConductorSolid.ToString(),
				"Plastic"
			};
			float melting_point = 2400f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues tieR5 = NOISE_POLLUTION.NOISY.TIER5;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tieR5, 0.2f);
			buildingDef.GeneratorWattageRating = 1000f;
			buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
			buildingDef.BuildLocationRule = BuildLocationRule.Anywhere;
			buildingDef.HitPoints = 10;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.AudioSize = "large";
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<LoopingSounds>();
			Prioritizable.AddRef(go);
			Tinkerable.MakePowerTinkerable(go);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.AddOrGet<Repairable>().expectedRepairTime = 52.5f;
			go.AddOrGet<SolarPanel>().powerDistributionOrder = 9;
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}

}
