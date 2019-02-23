using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Harmony;
using KSerialization;
using STRINGS;
using BUILDINGS = TUNING.BUILDINGS;

namespace BriskArctic
{
	public class BriskArcticMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class BriskArcticBuildingsPatch
		{
			private static void Prefix()
			{
				Strings.Add("STRINGS.BUILDINGS.PREFABS.ARCTIC.NAME", "Arctic Chill");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.ARCTIC.DESC", "This Cools down the Area");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.ARCTIC.EFFECT", "Uses Hydrogen and a bit of Power to cool down the air.");


                List<string> category = (List<string>)BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Utilities).data;
                category.Add(BriskArcticConfig.ID);

			}

			private static void Postfix()
			{
				object obj = Activator.CreateInstance(typeof(BriskArcticConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
        public class BriskArcticDbPatch
		{
			private static void Prefix()
			{
                List<string> ls = new List<string>(Techs.TECH_GROUPING["TemperatureModulation"]) { BriskArcticConfig.ID };
                Techs.TECH_GROUPING["TemperatureModulation"] = ls.ToArray();
			}
		}

	}
}