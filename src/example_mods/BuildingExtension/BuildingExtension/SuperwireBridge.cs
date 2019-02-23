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
	internal class SuperWireRefinedBridgeConfig_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDBRIDGE.NAME", "Super Wire Bridge");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDBRIDGE.DESC", "The Super Wire Bridge can transport 20000 Watts.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERWIREREFINEDBRIDGE.EFFECT", "A very strong but expensive wire to connect in lategame.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Power").Equals(po.category)).data;
			category.Add(SuperWireRefinedBridgeConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(SuperWireRefinedBridgeConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class SuperWireRefinedBridgeConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["PrettyGoodConductors"]);
			ls.Add(SuperWireRefinedBridgeConfig.ID);
			Database.Techs.TECH_GROUPING["PrettyGoodConductors"] = (string[])ls.ToArray();
		}
	}
	public class SuperWireRefinedBridgeConfig : WireBridgeConfig
	{
		public new const string ID = "SuperWireRefinedBridge";

		protected override string GetID()
		{
			return "SuperWireRefinedBridge";
		}

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = base.CreateBuildingDef();
			buildingDef.AnimFiles = new KAnimFile[1]
			{
	  Assets.GetAnim((HashedString) "utilityelectricbridgeconductive_kanim")
			};
			float[] construction_mass = new float[3] { 60, 20, 20 };
			string[] construction_materials = new string[3]
			{
				SimHashes.Steel.ToString(),
				SimHashes.TempConductorSolid.ToString(),
				"Plastic"
			};
			buildingDef.Mass = construction_mass;
			buildingDef.MaterialCategory = construction_materials;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.WireIDs, "WireRefinedBridge");
			return buildingDef;
		}

		protected override WireUtilityNetworkLink AddNetworkLink(GameObject go)
		{
			WireUtilityNetworkLink utilityNetworkLink = base.AddNetworkLink(go);
			utilityNetworkLink.maxWattageRating = Wire.WattageRating.Max20000;
			return utilityNetworkLink;
		}
	}
}
