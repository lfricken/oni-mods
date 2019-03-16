/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace RocketOverhaul
{
	public class SteamEngine
	{
		[HarmonyPatch(typeof(KeroseneEngineConfig))]
		[HarmonyPatch(nameof(KeroseneEngineConfig.DoPostConfigureComplete))]
		public static class DoPostConfigureComplete
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(GameObject go)
			{
				RocketEngine rocketEngine = go.AddOrGet<RocketEngine>();
				rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
				rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
				rocketEngine.requireOxidizer = false;
				rocketEngine.exhaustElement = SimHashes.Steam;
				rocketEngine.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
				FuelTank fuelTank = go.AddOrGet<FuelTank>();
				fuelTank.capacityKg = fuelTank.minimumLaunchMass;
				fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
				var list = new List<Storage.StoredItemModifier>()
				{
					Storage.StoredItemModifier.Hide,
					Storage.StoredItemModifier.Seal,
					Storage.StoredItemModifier.Insulate
				};
				fuelTank.SetDefaultStoredItemModifiers(list);
				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.conduitType = ConduitType.Gas;
				conduitConsumer.consumptionRate = 10f;
				conduitConsumer.capacityTag = fuelTank.FuelType;
				conduitConsumer.capacityKG = fuelTank.capacityKg;
				conduitConsumer.forceAlwaysSatisfied = true;
				conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
				go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim("rocket_steam_engine_bg_kanim"));
				EntityTemplates.ExtendBuildingToRocketModule(go);
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
