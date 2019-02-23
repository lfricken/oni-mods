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
	internal class SuperWireRefinedBridgeHighWattageConfig_LoadGeneratedBuildings
	{
		private static void Postfix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDBRIDGEHIGHWATTAGE.NAME", "Heavi Conductive Joint Plate");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDBRIDGEHIGHWATTAGE.DESC", "Joint plates can run Heavi wires through walls without leaking gas or liquid.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDBRIDGEHIGHWATTAGE.EFFECT", "Carries more Wattage than a regular Heavi-Watt Joint Plate without overloading.\n\nAllows Heavi-Watt Wire to be run through wall and floor tiles.\n\nAnd is less ugly.Functions as a regular tile.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)BUILDINGS.PLANORDER.First(po => po.category == PlanScreen.PlanCategory.Power).data;
			category.Remove("WireRefinedBridgeHighWattage");
			category.Add(SuperWireRefinedBridgeHighWattageConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(SuperWireRefinedBridgeHighWattageConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class SuperWireRefinedBridgeHighWattageConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["PrettyGoodConductors"]);
			ls.Remove("WireRefinedBridgeHighWattage");
			ls.Add(SuperWireRefinedBridgeHighWattageConfig.ID);
			Database.Techs.TECH_GROUPING["PrettyGoodConductors"] = (string[])ls.ToArray();
		}
	}
	public class SuperWireRefinedBridgeHighWattageConfig : WireBridgeHighWattageConfig
	{
		public new const string ID = "SuperWireRefinedBridgeHighWattage";

		protected override string GetID()
		{
			return "SuperWireRefinedBridgeHighWattage";
		}

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = base.CreateBuildingDef();
			buildingDef.AnimFiles = new KAnimFile[1]
			{
	  Assets.GetAnim((HashedString) "heavywatttile_conductive_kanim")
			};
			buildingDef.Mass = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
			buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
			buildingDef.SceneLayer = Grid.SceneLayer.WireBridges;
			buildingDef.ForegroundLayer = Grid.SceneLayer.TileFront;
			buildingDef.BaseDecor = -4f;
			buildingDef.BaseDecorRadius = 4f;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridgeHighWattage");
			return buildingDef;
		}

		protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
		{
			WireUtilityNetworkLink utilityNetworkLink = base.AddNetworkLink(go);
			utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max50000;
			return utilityNetworkLink;
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			go.GetComponent<Constructable>().requiredRolePerk = RoleManager.rolePerks.CanPowerTinker.id;
		}
	}

}
