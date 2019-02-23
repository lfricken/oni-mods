using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;

namespace BuildingExtension
{// Decompiled with JetBrains decompiler
 // Type: PressureDoorConfig
 // Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
 // MVID: C3209CA9-6918-453B-840E-8A1BF32B92D7
 // Assembly location: C:\Users\name\source\lib\Assembly-CSharp.dll
 // Compiler-generated code is shown

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class SuperWireRefinedHighWattageConfig_LoadGeneratedBuildings
	{
		private static void Postfix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDHIGHWATTAGE.NAME", "Heavi Conductive Wire");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDHIGHWATTAGE.DESC", "Higher wattage wire is used to avoid power overloads, particularly for strong generators.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDHIGHWATTAGE.EFFECT", "Carries more Wattage than regular Wire without overloading.\n\nAnd is less ugly.Cannot be run through tiles.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Power).data;
			category.Remove("WireRefinedHighWattage");
			category.Add(SuperWireRefinedBridgeHighWattageConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(SuperWireRefinedBridgeHighWattageConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class SuperWireRefinedHighWattageConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["PrettyGoodConductors"]);
			ls.Remove("WireRefinedHighWattage");
			ls.Add(SuperWireRefinedBridgeHighWattageConfig.ID);
			Database.Techs.TECH_GROUPING["PrettyGoodConductors"] = (string[])ls.ToArray();
		}
	}
	public class SuperWireRefinedHighWattageConfig : BaseWireConfig
	{
		public const string ID = "SuperWireRefinedHighWattage";

		public override BuildingDef CreateBuildingDef()
		{
			string id = "SuperWireRefinedHighWattage";
			string anim = "utilities_electric_conduct_hiwatt_kanim";
			float construction_time = 3f;
			float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
			float insulation = 0.05f;
			EffectorValues none = NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = this.CreateBuildingDef(id, anim, construction_time, tieR2, insulation, BUILDINGS.DECOR.PENALTY.TIER3, none);
			buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
			buildingDef.BuildLocationRule = BuildLocationRule.NotInTiles;
			buildingDef.BaseDecor = -4f;
			buildingDef.BaseDecorRadius = 4f;
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			this.DoPostConfigureComplete(Wire.WattageRating.Max50000, go);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.GetComponent<Constructable>().requiredRolePerk = RoleManager.rolePerks.CanPowerTinker.id;
		}
	}

}
