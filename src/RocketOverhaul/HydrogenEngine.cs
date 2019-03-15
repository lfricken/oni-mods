﻿/// <summary>
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
	public class HydrogenEngine
	{
		[HarmonyPatch(typeof(HydrogenEngineConfig))]
		[HarmonyPatch(nameof(HydrogenEngineConfig.DoPostConfigureComplete))]
		public static class DoPostConfigureComplete
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(GameObject go)
			{
				RocketEngineImproved rocketEngine = go.AddOrGet<RocketEngineImproved>();
				rocketEngine.ExhaustVelocity = HydrogenEngineStats.ExhaustVelocity;
				rocketEngine.RangePenalty = HydrogenEngineStats.RangePenalty;
				rocketEngine.MinFuel = HydrogenEngineStats.MinFuel;
				rocketEngine.MaxFuel = HydrogenEngineStats.MaxFuel;

				rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.LiquidHydrogen).tag;
				rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
				rocketEngine.exhaustElement = SimHashes.Steam;
				rocketEngine.exhaustTemperature = 2000f;
				EntityTemplates.ExtendBuildingToRocketModule(go);
				go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim("rocket_hydrogen_engine_bg_kanim"));

			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class LoadGeneratedBuildings
		{
			public static void Postfix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{HydrogenEngineConfig.ID.ToUpperInvariant()}.NAME", HydrogenEngineStats.NAME);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{HydrogenEngineConfig.ID.ToUpperInvariant()}.DESC", HydrogenEngineStats.DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{HydrogenEngineConfig.ID.ToUpperInvariant()}.EFFECT", HydrogenEngineStats.EFFECT);
			}
		}
	}
}
