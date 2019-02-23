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
	internal class BackgroundLadder_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.BACKGROUNDLADDER.NAME", "Background Ladder");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.BACKGROUNDLADDER.DESC", "The Background Ladder is a Ladder behind other buildings.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.BACKGROUNDLADDER.EFFECT", "The Background Ladder is perfect if you need to reach higher but a building obstruct you.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Base").Equals(po.category)).data;
			
			category.Add(BackgroundLadderConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(BackgroundLadderConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class BackgroundLadder_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["Luxury"]);
			ls.Add(BackgroundLadderConfig.ID);
			Database.Techs.TECH_GROUPING["Luxury"] = (string[])ls.ToArray();
		}
	}

	public class BackgroundLadderConfig : IBuildingConfig
	{
		public const string ID = "BackgroundLadder";

		public override BuildingDef CreateBuildingDef()
		{
			string id = "BackgroundLadder";
			int width = 1;
			int height = 1;
			string anim = "ladder_plastic_kanim";
			int hitpoints = 10;
			float construction_time = 10f;
			float[] tieR1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
			string[] plastics = MATERIALS.PLASTICS;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR1, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.Entombable = false;
			buildingDef.AudioCategory = "Plastic";
			buildingDef.AudioSize = "small";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.DragBuild = true;
			buildingDef.ObjectLayer = ObjectLayer.Backwall;
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			GeneratedBuildings.MakeBuildingAlwaysOperational(go);
			Ladder ladder = go.AddOrGet<Ladder>();
			ladder.upwardsMovementSpeedMultiplier = 1.2f;
			ladder.downwardsMovementSpeedMultiplier = 1.2f;
			go.AddOrGet<AnimTileable>();
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
		}
	}

}
