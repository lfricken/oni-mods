using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;

namespace gateinit
{
	internal class IDS
	{
		public const string ID = "outworldstargate";
		public const string NAME = "Stargate";
		public const string DESCRIPTION = "The Stargate creates a Wormhole to transport your dupes to other worlds.";
		public const string EFFECT = "Fill it with material and dupes and when you finished start a new game.";
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
namespace gate
{

	public class GateConfig : IBuildingConfig
	{
		public const string ID = gateinit.IDS.ID;

		public override BuildingDef CreateBuildingDef()
		{
			string id = gateinit.IDS.ID;
			int width = 4;
			int height = 4;
			string anim = "hqbase_kanim";
			int hitpoints = 250;
			float construction_time = 30f;
			float[] tieR7 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER7;
			string[] allMetals = MATERIALS.ALL_METALS;
			float melting_point = 1600f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR7, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER5, none, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.BaseTimeUntilRepair = 400f;
			buildingDef.ShowInBuildMenu = false;
			buildingDef.DefaultAnimState = "idle";
			SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_LP", NOISE_POLLUTION.NOISY.TIER3);
			SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_open", NOISE_POLLUTION.NOISY.TIER4);
			SoundEventVolumeCache.instance.AddVolume("hqbase_kanim", "Portal_close", NOISE_POLLUTION.NOISY.TIER4);
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<LoreBearer>();
			go.AddOrGet<Telepad>();
			Light2D light2D = go.AddOrGet<Light2D>();
			light2D.Color = LIGHT2D.HEADQUARTERS_COLOR;
			light2D.Range = 5f;
			light2D.Offset = LIGHT2D.HEADQUARTERS_OFFSET;
			light2D.overlayColour = LIGHT2D.HEADQUARTERS_OVERLAYCOLOR;
			light2D.shape = LightShape.Circle;
			light2D.drawOverlay = true;


			go.AddOrGet<DropAllWorkable>();
			Prioritizable.AddRef(go);
			go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
			ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
			BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);


			Storage storage = go.AddOrGet<Storage>();
			storage.showInUI = true;
			storage.allowItemRemoval = false;
			storage.showDescriptor = false;
			storage.storageFilters = STORAGEFILTERS.NOT_EDIBLE_SOLIDS;
			storage.storageFullMargin = STORAGE.STORAGE_LOCKER_FILLED_MARGIN;
			storage.fetchCategory = Storage.FetchCategory.GeneralStorage;
			// todo storage.Serialize
		}

		public MinionStartingStats[] statstorage;
		public override void DoPostConfigureComplete(GameObject go)
		{
			//get a dupe save and destroy it:
			new Recipe("TeleportDupe", 30f, (SimHashes)0, null, "Teleport Dupes", 1).SetFabricator(gateinit.IDS.ID.ToUpper(), 5f).AddIngredient(new Recipe.Ingredient(GameTags.Minion.ToString(), 1f));					}
		
	}

}
