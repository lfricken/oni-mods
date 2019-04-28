/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Database;
using Harmony;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace ShaftRoboMiner
{
	/// <summary>
	/// Description, stats, functionality.
	/// </summary>
	public class ShaftRoboMinerConfig : IBuildingConfig
	{
		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ShaftRoboMiner.Id, 2, 2, "auto_miner_kanim", 10, 10f, BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, BUILDINGS.DECOR.PENALTY.TIER2, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = ShaftRoboMiner.EnergyConsumption;
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = ShaftRoboMiner.HeatProduction;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, ShaftRoboMiner.Id);
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<Operational>();
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<MiningSounds>();
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
			AddVisualizer(go, true);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
			AddVisualizer(go, false);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, LogicOperationalController.INPUT_PORTS_0_0);
			go.AddOrGet<LogicOperationalController>();
			ShaftAutoMiner autoMiner = go.AddOrGet<ShaftAutoMiner>();
			autoMiner.width = ShaftRoboMiner.Range.width;
			autoMiner.height = ShaftRoboMiner.Range.height;
			autoMiner.x = ShaftRoboMiner.Range.xOffset;
			autoMiner.y = ShaftRoboMiner.Range.yOffset;
			autoMiner.vision_offset = ShaftRoboMiner.Range.VisionOffset;
			AddVisualizer(go, false);
		}

		private static void AddVisualizer(GameObject go, bool movable)
		{
			StationaryChoreRangeVisualizer choreRangeVisualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
			choreRangeVisualizer.width = ShaftRoboMiner.Range.width;
			choreRangeVisualizer.height = ShaftRoboMiner.Range.height;
			choreRangeVisualizer.x = ShaftRoboMiner.Range.xOffset;
			choreRangeVisualizer.y = ShaftRoboMiner.Range.yOffset;
			choreRangeVisualizer.vision_offset = ShaftRoboMiner.Range.VisionOffset;
			choreRangeVisualizer.movable = movable;
			choreRangeVisualizer.blocking_tile_visible = false;

			go.GetComponent<KPrefabID>().instantiateFn += (prefab =>
			{
				StationaryChoreRangeVisualizer component = prefab.GetComponent<StationaryChoreRangeVisualizer>();
				Func<int, bool> callback = AutoMiner.DigBlockingCB;
				component.blocking_cb = callback;
			});
		}
	}

	/// <summary>BuildableSteamGeyser
	/// Set Name, Description, Effect description, Tech Grouping, buildscreen.
	/// </summary>
	public class ShaftRoboMinerConfigPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ShaftRoboMiner.Id.ToUpperInvariant()}.NAME", ShaftRoboMiner.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ShaftRoboMiner.Id.ToUpperInvariant()}.DESC", ShaftRoboMiner.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{ShaftRoboMiner.Id.ToUpperInvariant()}.EFFECT", ShaftRoboMiner.Effect);

				ModUtil.AddBuildingToPlanScreen(ShaftRoboMiner.BuildTab, ShaftRoboMiner.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddTech(ShaftRoboMiner.Id, ShaftRoboMiner.TechGroup);
			}

			private static void AddTech(string id, string techGroup)
			{
				var tech = new List<string>(Techs.TECH_GROUPING[techGroup]);
				tech.Add(id);
				Techs.TECH_GROUPING[techGroup] = tech.ToArray();
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager))]
		[HarmonyPatch(nameof(KSerialization.Manager.GetType))]
		[HarmonyPatch(new[] { typeof(string) })]
		public static class KSerializationManager_GetType_Patch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == typeof(ShaftAutoMiner).AssemblyQualifiedName)
				{
					__result = typeof(ShaftAutoMiner);
				}
			}
		}
	}
}
