using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;
using TUNING;
using UnityEngine;

namespace NuclearPowerMod
{

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class RadiumFuelMod_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Debug.Log(" === GeneratedBuildings Prefix === " + RadiumFuelConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.RADIUMFUELCELL.NAME", "Radium Fuel Cell");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.RADIUMFUELCELL.DESC", "The Radium Fuel Cell provides heat over time.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.RADIUMFUELCELL.EFFECT", "Place the Radium Fuel Cell to supply a steam engine with heat. (Only works with the 'Nuclear Power Mod' Script enabled).");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Utilities").Equals(po.category)).data;
			category.Add(RadiumFuelConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof( RadiumFuelConfig));

		}
		private static void Postfix()
		{

			Debug.Log(" === GeneratedBuildings Postfix === " + RadiumFuelConfig.ID);
			object obj = Activator.CreateInstance(typeof(RadiumFuelConfig));
			BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			Db.Get().Diseases.Add(new Klei.AI.Radioactive());
		}
	}

	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class RadiumFuelMod_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["RenewableEnergy"]);
			ls.Add(RadiumFuelConfig.ID);
			Database.Techs.TECH_GROUPING["RenewableEnergy"] = (string[])ls.ToArray();

			//Database.Techs.TECH_GROUPING["TemperatureModulation"].Add("InsulatedPressureDoor");
		}
	}



	public class RadiumFuelConfig : IBuildingConfig
	{
		private static readonly CellOffset[] overrideOffsets = new CellOffset[4]
		{
	new CellOffset(-1, -1),
	new CellOffset(1, -1),
	new CellOffset(-1, 1),
	new CellOffset(1, 1)
		};
		public const string ID = "RadiumFuelCell";

		public override BuildingDef CreateBuildingDef()
		{
			string id = "RadiumFuelCell";
			int width = 1;
			int height = 1;
			string anim = "thermalblock_kanim";
			int hitpoints = 30;
			float construction_time = 120f;
			float[] tieR5 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
			string[] anyBuildable = new string[1] { "Radium" };
			float melting_point = 8000f;
			BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5, anyBuildable, melting_point, build_location_rule, DECOR.NONE, none, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.Overheatable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.BaseTimeUntilRepair = -1f;
			buildingDef.ViewMode = OverlayModes.Temperature.ID; 
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
				StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
				int cell = Grid.PosToCell(game_object);
				payload.OverrideExtents(new Extents(cell, RadiumFuelConfig.overrideOffsets));
				GameComps.StructureTemperatures.SetPayload(handle, ref payload);
			});
		}
	}

}