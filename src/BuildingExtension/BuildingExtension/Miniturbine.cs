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
	internal class MiniSteamTurbineConfig_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MINISTEAMTURBINE.NAME", "Mini Steam Turbine");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MINISTEAMTURBINE.DESC", "The MiniSteamTurbine provides small quantities of energie and is satisfied with any steam.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MINISTEAMTURBINE.EFFECT", "Use it when you have a little heat in abundance its strongly cooling.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Power").Equals(po.category)).data;

			category.Add(MiniSteamTurbineConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(MiniSteamTurbineConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class MiniSteamTurbineConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["RenewableEnergy"]);
			ls.Add(MiniSteamTurbineConfig.ID);
			Database.Techs.TECH_GROUPING["RenewableEnergy"] = (string[])ls.ToArray();
		}
	}
	public class MiniSteamTurbineConfig : IBuildingConfig
	{
		public const string ID = "MiniSteamTurbine";
		private static readonly List<Storage.StoredItemModifier> StoredItemModifiers;
		private static readonly LogicPorts.Port[] INPUT_PORTS;
		public override BuildingDef CreateBuildingDef()
		{
			string id = "MiniSteamTurbine";
			int width = 1;
			int height = 2;
			string anim = "minigaspump_kanim";
			int hitpoints = 30;
			float construction_time = 60f;
			string[] construction_materials = new string[2]
			{
	  "RefinedMetal",
	  "Plastic"
			};
			EffectorValues none = TUNING.NOISE_POLLUTION.NONE;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, new float[2]
			{
	  TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3[0],
	  TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2[0]
			}, construction_materials, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.NONE, none, 1f);
			buildingDef.GeneratorWattageRating = 400f;
			buildingDef.GeneratorBaseCapacity = 400f;
			buildingDef.Entombable = true;
			buildingDef.IsFoundation = false;
			buildingDef.PermittedRotations = PermittedRotations.FlipH;
			buildingDef.ViewMode = OverlayModes.Power.ID;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PowerOutputOffset = new CellOffset(0, 0);
			buildingDef.OverheatTemperature = 5273.15f;
			return buildingDef;
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, MiniSteamTurbineConfig.INPUT_PORTS);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			base.DoPostConfigureUnderConstruction(go);
			GeneratedBuildings.RegisterLogicPorts(go, MiniSteamTurbineConfig.INPUT_PORTS);
			go.GetComponent<Constructable>().requiredRolePerk = RoleManager.rolePerks.CanPowerTinker.id;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, MiniSteamTurbineConfig.INPUT_PORTS);
			go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(MiniSteamTurbineConfig.StoredItemModifiers);
			Turbine turbine = go.AddOrGet<Turbine>();
			turbine.srcElem = SimHashes.Steam;
			turbine.pumpKGRate = 1f;
			turbine.requiredMassFlowDifferential = 0.01f;
			turbine.minEmitMass = 1f;
			turbine.maxRPM = 4000f;
			turbine.rpmAcceleration = turbine.maxRPM / 30f;
			turbine.rpmDeceleration = turbine.maxRPM / 20f;
			turbine.minGenerationRPM = 3000f;
			turbine.minActiveTemperature = 352.15f;
			turbine.emitTemperature = 252.15f;
			go.AddOrGet<Generator>();
			go.AddOrGet<LogicOperationalController>();
			Prioritizable.AddRef(go); 
			KPrefabID component = go.GetComponent<KPrefabID>();
		}

		static MiniSteamTurbineConfig()
		{
			List<Storage.StoredItemModifier> storedItemModifierList = new List<Storage.StoredItemModifier>();
			storedItemModifierList.Add(Storage.StoredItemModifier.Hide);
			storedItemModifierList.Add(Storage.StoredItemModifier.Insulate);
			storedItemModifierList.Add(Storage.StoredItemModifier.Seal);
			MiniSteamTurbineConfig.StoredItemModifiers = storedItemModifierList;
			MiniSteamTurbineConfig.INPUT_PORTS = new LogicPorts.Port[1]
			{
	  LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), "Logic Port", false)
			};
		}

	}
}
