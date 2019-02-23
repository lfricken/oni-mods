
using System;
using Harmony;
using KSerialization;
using TUNING;

namespace BriskRubberSuit
{
    public class RubberBootsMod
    {
        [HarmonyPatch(typeof(GeneratedEquipment), "LoadGeneratedEquipment")]
        public class GeneratedEquipmentConfigManagerPatch
        {

            private static void Prefix()
            {
                Strings.Add("STRINGS.EQUIPMENT.PREFABS.RUBBERBOOTS.NAME", "Rubber Suit");
                Strings.Add("STRINGS.EQUIPMENT.PREFABS.RUBBERBOOTS.GENERICNAME", "Rubber Suit");
                Strings.Add("STRINGS.EQUIPMENT.PREFABS.RUBBERBOOTS.RECIPE_DESC", "Protection from wetness and heat/cold");
                Strings.Add("STRINGS.EQUIPMENT.PREFABS.RUBBERBOOTS.EFFECT", "Protection from wetness and heat/cold");
                Strings.Add("STRINGS.EQUIPMENT.PREFABS.RUBBERBOOTS.DESC", "Rubber Suit that protects you from getting drity");
            }

            private static void Postfix()
            {
                object obj = Activator.CreateInstance(typeof(RubberBootsConfig));
                EquipmentConfigManager.Instance.RegisterEquipment(obj as IEquipmentConfig);
            }
        }
        
    }
}