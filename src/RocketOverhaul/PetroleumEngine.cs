/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using UnityEngine;

namespace RocketOverhaul
{
	public class PetroleumEngine
	{
		[HarmonyPatch(typeof(KeroseneEngineConfig))]
		[HarmonyPatch(nameof(KeroseneEngineConfig.DoPostConfigureComplete))]
		public static class DoPostConfigureComplete
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(GameObject go)
			{
				RocketEngineImproved rocketEngine = go.AddOrGet<RocketEngineImproved>();
				rocketEngine.ExhaustVelocity = PetroleumEngineStats.ExhaustVelocity;
				rocketEngine.RangePenalty = PetroleumEngineStats.RangePenalty;
				rocketEngine.MinFuel = PetroleumEngineStats.MinFuel;
				rocketEngine.MaxFuel = PetroleumEngineStats.MaxFuel;

				rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Petroleum).tag;
				rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
				EntityTemplates.ExtendBuildingToRocketModule(go);
				go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim("rocket_petroleum_engine_bg_kanim"));
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class LoadGeneratedBuildings
		{
			static void Postfix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{KeroseneEngineConfig.ID.ToUpperInvariant()}.NAME", PetroleumEngineStats.NAME);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{KeroseneEngineConfig.ID.ToUpperInvariant()}.DESC", PetroleumEngineStats.DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{KeroseneEngineConfig.ID.ToUpperInvariant()}.EFFECT", PetroleumEngineStats.EFFECT);
			}
		}
	}
}
