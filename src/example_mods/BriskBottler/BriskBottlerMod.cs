using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Harmony;
using KSerialization;
using STRINGS;
using BUILDINGS = TUNING.BUILDINGS;

namespace BriskBottler
{
    public class BriskBottlerMod
	{
		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class BriskBottlerBuildingsPatch
		{
  			private static void Prefix()
			{
                Strings.Add("STRINGS.BUILDINGS.PREFABS.BRISKBOTTLER.NAME", "Liquid Bottler");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.BRISKBOTTLER.DESC", "This Bottler station has access to: {Liquids");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.BRISKBOTTLER.EFFECT", "Liquid Available: {Liquids}");

                List<string> category = (List<string>)BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Plumbing).data;
                category.Add(BriskBottlerConfig.ID);
			}

			private static void Postfix()
			{
                object obj = Activator.CreateInstance(typeof(BriskBottlerConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
        public class BriskBottlerDbPatch
		{
			private static void Prefix()
			{
                List<string> ls = new List<string>(Techs.TECH_GROUPING["ImprovedLiquidPiping"]) { BriskBottlerConfig.ID };
                Techs.TECH_GROUPING["ImprovedLiquidPiping"] = ls.ToArray();
			}
		}

	}
}