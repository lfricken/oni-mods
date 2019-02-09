using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using TUNING;
using UnityEngine;

namespace MercuryCoolerConfigMod
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class MercuryCoolerConfig_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Debug.Log(" === GeneratedBuildings Prefix === " + MercuryCoolerConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MERCURYCOOLERCELL.NAME", "Mercury Cooler Cell");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MERCURYCOOLERCELL.DESC", "Mercury Cooler Cell slowly cools over time.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MERCURYCOOLERCELL.EFFECT", "Place the Mercury Cooler Cell to passively cool ares.");

			List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			ls.Add(MercuryCoolerConfig.ID);
			TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof( MercuryCoolerConfig));


		}
		private static void Postfix()
		{

			Debug.Log(" === GeneratedBuildings Postfix === " + MercuryCoolerConfig.ID);
			object obj = Activator.CreateInstance(typeof(MercuryCoolerConfig));
			BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			//Db.Get().Diseases.Add(new Klei.AI.Radioactive());
		}
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class MercuryCoolerConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["RenewableEnergy"]);
			ls.Add(MercuryCoolerConfig.ID);
			Database.Techs.TECH_GROUPING["RenewableEnergy"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}



	public class MercuryCoolerConfig : IBuildingConfig
	{
		private static readonly CellOffset[] overrideOffsets = new CellOffset[4]
		{
	new CellOffset(-1, -1),
	new CellOffset(1, -1),
	new CellOffset(-1, 1),
	new CellOffset(1, 1)
		};
		public const string ID = "MercuryCoolerCell";

		public override BuildingDef CreateBuildingDef()
		{
			string id = "MercuryCoolerCell";
			int width = 1;
			int height = 1;
			string anim = "thermalblock_kanim";
			int hitpoints = 30;
			float construction_time = 120f;
			float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
			string[] anyBuildable = new string[1] { ElementLoader.FindElementByHash(SimHashes.SolidMercury).name };
			float melting_point = 8000f;
			BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, anyBuildable, melting_point, build_location_rule, DECOR.NONE, none, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.ViewMode = SimViewMode.TemperatureMap;
			buildingDef.DefaultAnimState = "off";
			buildingDef.ObjectLayer = ObjectLayer.Backwall;
			buildingDef.SceneLayer = Grid.SceneLayer.TempShiftPlate;
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<AnimTileable>().objectLayer = ObjectLayer.Backwall;
			//go.AddComponent<ZoneTile>();
			BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
			go.GetComponent<KPrefabID>().prefabSpawnFn += (KPrefabID.PrefabFn)(game_object =>
			{
				HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
				StructureTemperatureData data = GameComps.StructureTemperatures.GetData(handle);
				int cell = Grid.PosToCell(game_object);
				data.OverrideExtents(new Extents(cell, MercuryCoolerConfig.overrideOffsets));
				GameComps.StructureTemperatures.SetData(handle, data);
			});
		}
	}

}