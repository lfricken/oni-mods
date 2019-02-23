using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Harmony;
using KSerialization;
using STRINGS;
using BUILDINGS = TUNING.BUILDINGS;
using UnityEngine;

namespace BriskDupeFab
{
    public class BriskDupeFabMod
	{

		[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class BriskBottlerBuildingsPatch
		{
  			private static void Prefix()
			{
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DUPEFABRICATOR.NAME", "Dupe Fabricator");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DUPEFABRICATOR.DESC", "This allows you to rip the life of the dupes to help your colony");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.DUPEFABRICATOR.EFFECT", "This allows you to rip the life of the dupes to help your colony");

                List<string> category = (List<string>)BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Refining).data;
                category.Add(BriskDupeFabConfig.ID);
			}

			private static void Postfix()
			{
                object obj = Activator.CreateInstance(typeof(BriskDupeFabConfig));
				BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
			}
		}

		[HarmonyPatch(typeof(Db), "Initialize")]
        public class BriskBottlerDbPatch
		{
			private static void Prefix()
			{
                List<string> ls = new List<string>(Techs.TECH_GROUPING["AdvancedResearch"]) { BriskDupeFabConfig.ID };
                Techs.TECH_GROUPING["AdvancedResearch"] = ls.ToArray();
			}
		}

              }
	}
