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
	internal class WireRefinedConfig_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINED.NAME", "Super Wire");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINED.DESC", "The Super Wire can transport 20000 Watts.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINED.EFFECT", "A very strong but expensive wire to connect in lategame.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Power").Equals(po.category)).data;
			category.Add(SuperWireRefinedConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(SuperWireRefinedConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class WireRefinedConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["PrettyGoodConductors"]);
			ls.Add(SuperWireRefinedConfig.ID);
			Database.Techs.TECH_GROUPING["PrettyGoodConductors"] = (string[])ls.ToArray();
		}
	}
	public class SuperWireRefinedConfig : BaseWireConfig
	{
		public const string ID = "SuperWireRefined";

		public override BuildingDef CreateBuildingDef()
		{
			string id = "SuperWireRefined";
			string anim = "utilities_electric_conduct_kanim";
			float construction_time = 3f;
			float[] construction_mass = new float[3] { 60,20,20 };
			string[] construction_materials = new string[3]
			{
				SimHashes.Steel.ToString(),
				SimHashes.TempConductorSolid.ToString(),
				"Plastic"
			};
			float insulation = 0.05f;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = this.CreateBuildingDef(id, anim, construction_time, construction_mass, insulation, BUILDINGS.DECOR.NONE, none);
			buildingDef.MaterialCategory = construction_materials;
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			this.DoPostConfigureComplete(Wire.WattageRating.Max20000, go);
		}
	}
}
