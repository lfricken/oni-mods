/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Database;
using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace BuildableSteamGeyser
{
	/// <summary>
	/// Animations, 
	/// </summary>
	public class BuildableSteamGeyser : StateMachineComponent<BuildableSteamGeyser.SMInstance>
	{
		public const float CelciusOutputTemperature = 175f;
		public const float EmissionRate = 100f;

		[MyCmpGet]
		private readonly Operational _operational;

		public BuildableSteamGeyser(Operational operational)
		{
			_operational = operational;
		}

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class SMInstance : GameStateMachine<States, SMInstance, BuildableSteamGeyser, object>.GameInstance
		{
			private readonly Operational _operational;

			public SMInstance(BuildableSteamGeyser master) : base(master)
			{
				_operational = master.GetComponent<Operational>();
			}

			public bool IsOperational => true;

			public bool HasLight()
			{
				var cell = Grid.PosToCell(smi.master.transform.GetPosition());
				return Grid.LightCount[cell] > 0;
			}
		}

		public class States : GameStateMachine<States, SMInstance, BuildableSteamGeyser>
		{
			public override void InitializeStates(out BaseState defaultState)
			{
				defaultState = root;

				root
					.QueueAnim("working_loop", true);
			}
		}
	}

	/// <summary>
	/// Description, stats, functionality.
	/// </summary>
	public class BuildableSteamGeyserConfig : IBuildingConfig
	{
		public static readonly string Id = "BuildableSteamGeyser";
		public static readonly string DisplayName = "Geothermal Steam Pump";
		public static readonly string Description = $"Pumps large amounts of " + UI.FormatAsLink("Steam", "STEAM") + $" at {BuildableSteamGeyser.CelciusOutputTemperature}°C out of the ground. Can't be turned off once active.";
		public static readonly LocString Effect = (LocString)("Produces " + UI.FormatAsLink("Steam", "STEAM") + ".");

		/// <summary>
		/// Define construction costs, health, size, noises, random stats.
		/// </summary>
		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 5,
				height: 4,
				anim: "geyser_oil_cap_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER4,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER6,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER7,
				construction_materials: MATERIALS.REFINED_METALS,
				melting_point: 0,
				build_location_rule: BuildLocationRule.OnFloor,
				decor: BUILDINGS.DECOR.PENALTY.TIER4,
				noise: NOISE_POLLUTION.NOISY.TIER4);

			buildingDef.Overheatable = false;
			buildingDef.Floodable = false;
			buildingDef.MaterialCategory = MATERIALS.REFINED_METALS;
			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;

			SoundEventVolumeCache.instance.AddVolume("geyser_liquid_water_slush_kanim", "Geyser_erupt_LP", NOISE_POLLUTION.NOISY.TIER0);

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			DefineEmitter(go);
			go.AddOrGet<AnimTileable>();
			var waterGeyser = go.AddOrGet<BuildableSteamGeyser>();

			Prioritizable.AddRef(go);
		}

		public void DefineEmitter(GameObject go)
		{

			float kelvinOffset = 273.15f;
			var emitter = go.AddComponent<ElementEmitter>();
			emitter.emitRange = 2;
			emitter.maxPressure = 2761;
			emitter.outputElement = new ElementConverter.OutputElement(BuildableSteamGeyser.EmissionRate, SimHashes.Steam, BuildableSteamGeyser.CelciusOutputTemperature + kelvinOffset, false, 0f, 0f, false, 1f);
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}
	}

	/// <summary>BuildableSteamGeyser
	/// Set Name, Description, Effect description, Tech Grouping, buildscreen.
	/// </summary>
	public class BuildableSteamGeyserConfigPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{BuildableSteamGeyserConfig.Id.ToUpperInvariant()}.NAME", BuildableSteamGeyserConfig.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{BuildableSteamGeyserConfig.Id.ToUpperInvariant()}.DESC", BuildableSteamGeyserConfig.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{BuildableSteamGeyserConfig.Id.ToUpperInvariant()}.EFFECT", BuildableSteamGeyserConfig.Effect);

				ModUtil.AddBuildingToPlanScreen("Plumbing", BuildableSteamGeyserConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				var tech = new List<string>(Techs.TECH_GROUPING["FarmingTech"]) { BuildableSteamGeyserConfig.Id };
				Techs.TECH_GROUPING["FarmingTech"] = tech.ToArray();
			}
		}

		[HarmonyPatch(typeof(KSerialization.Manager))]
		[HarmonyPatch(nameof(KSerialization.Manager.GetType))]
		[HarmonyPatch(new[] { typeof(string) })]
		public static class KSerializationManager_GetType_Patch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == "BuildableSteamGeyser.BuildableSteamGeyser")
				{
					__result = typeof(BuildableSteamGeyser);
				}
			}
		}
	}
}
