/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Database;
using Harmony;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace RocketOverhaul
{
	public partial class MethaneEngineConfig : HydrogenEngineConfig
	{
		public const string Id = nameof(MethaneEngineConfig);

		public override BuildingDef CreateBuildingDef()
		{
			int width = 7;
			int height = 5;
			string anim = "rocket_hydrogen_engine_kanim";
			int hitpoints = 1000;
			float construction_time = 60f;
			float[] engineMassLarge = BUILDINGS.ROCKETRY_MASS_KG.ENGINE_MASS_LARGE;
			string[] construction_materials = new string[1]
			{
				SimHashes.Steel.ToString()
			};
			float melting_point = 9999f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues tieR2 = NOISE_POLLUTION.NOISY.TIER2;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(nameof(MethaneEngineConfig), width, height, anim, hitpoints, construction_time, engineMassLarge, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tieR2, 0.2f);
			BuildingTemplates.CreateRocketBuildingDef(buildingDef);
			buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
			buildingDef.OverheatTemperature = 2273.15f;
			buildingDef.Floodable = false;
			buildingDef.AttachmentSlotTag = GameTags.Rocket;
			buildingDef.ObjectLayer = ObjectLayer.Building;
			buildingDef.attachablePosition = new CellOffset(0, 0);
			buildingDef.RequiresPowerInput = false;
			buildingDef.CanMove = true;
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			RocketEngineImproved engine = go.AddOrGet<RocketEngineImproved>();
			engine.ExhaustVelocity = MethaneEngineStats.ExhaustVelocity;
			engine.RangePenalty = MethaneEngineStats.DistancePenalty;
			engine.fuelTag = ElementLoader.FindElementByHash(MethaneEngineStats.FuelType).tag;
			engine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
			engine.exhaustElement = SimHashes.Steam;
			engine.exhaustTemperature = 2000f;
			EntityTemplates.ExtendBuildingToRocketModule(go);
			go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim("rocket_hydrogen_engine_bg_kanim"));
		}
	}

	/// <summary>
	/// Adds the <see cref="MethaneEngine"/> to the:
	/// Buildings Tab,
	/// Tech Tree,
	/// </summary>
	public class MethaneEngine_AddToBuildings
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MethaneEngineConfig.Id.ToUpperInvariant()}.NAME", MethaneEngineStats.NAME);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MethaneEngineConfig.Id.ToUpperInvariant()}.DESC", MethaneEngineStats.DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{MethaneEngineConfig.Id.ToUpperInvariant()}.EFFECT", MethaneEngineStats.EFFECT);

				ModUtil.AddBuildingToPlanScreen(MethaneEngineStats.BuildTab, MethaneEngineConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddTech(MethaneEngineConfig.Id, MethaneEngineStats.TechGroup);
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
				if (type_name == typeof(MethaneEngineConfig).AssemblyQualifiedName)
				{
					__result = typeof(MethaneEngineConfig);
				}
			}
		}
	}
}
