using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Harmony;
using KSerialization;
using STRINGS;
using BUILDINGS = TUNING.BUILDINGS;

namespace BriskAirWick
{
    public class BriskAirWickMod
    {
        [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
        public class BriskAirWickBuildingsPatch
        {
            private static void Prefix()
            {
                Strings.Add("STRINGS.BUILDINGS.PREFABS.BRISKAIRWICK.NAME", "Air Wick");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.BRISKAIRWICK.DESC", "A Wick abosrbs the Polluted Water then releases Polluted Oxygen. Smell the Freshness");
                Strings.Add("STRINGS.BUILDINGS.PREFABS.BRISKAIRWICK.EFFECT", "Pushes " + (string) ELEMENTS.CONTAMINATEDOXYGEN.NAME + " into " + (string) ELEMENTS.DIRTYWATER.NAME + "With use of" + (string) STRINGS.CREATURES.SPECIES.BASICFABRICMATERIALPLANT.NAME + ".");



                List<string> category = (List<string>)BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Oxygen).data;
                category.Add(BriskAirWickConfig.ID);

            }

            private static void Postfix()
            {
                object obj = Activator.CreateInstance(typeof(BriskAirWickConfig));
                BuildingConfigManager.Instance.RegisterBuilding(obj as IBuildingConfig);
            }
        }

        [HarmonyPatch(typeof(Db), "Initialize")]
        public class BriskAirWickDbPatch
        {
            private static void Prefix()
            {

                List<string> ls = new List<string>(Techs.TECH_GROUPING["ImprovedOxygen"]) { BriskAirWickConfig.ID };
                Techs.TECH_GROUPING["ImprovedOxygen"] = ls.ToArray();
                
            }
        }

    }
}