/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace RocketOverhaul
{
	public class SteamEngineConfigPatches
	{
		[HarmonyPatch(typeof(SteamEngineConfig))]
		[HarmonyPatch(nameof(SteamEngineConfig.DoPostConfigureComplete))]
		public static class DoPostConfigureComplete
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(GameObject go)
			{
				RocketEngineImproved rocketEngine = go.AddOrGet<RocketEngineImproved>();
				rocketEngine.ExhaustVelocity = SteamEngineStats.ExhaustVelocity;
				rocketEngine.RangePenalty = SteamEngineStats.RangePenalty;
				rocketEngine.MinFuel = SteamEngineStats.MinFuel;
				rocketEngine.MaxFuel = SteamEngineStats.MaxFuel;
				rocketEngine.OnlyFuel = true;

				rocketEngine.fuelTag = ElementLoader.FindElementByHash(SimHashes.Steam).tag;
				rocketEngine.explosionEffectHash = SpawnFXHashes.MeteorImpactDust;
				rocketEngine.requireOxidizer = false;
				rocketEngine.exhaustElement = SimHashes.Steam;
				rocketEngine.exhaustTemperature = ElementLoader.FindElementByHash(SimHashes.Steam).lowTemp + 50f;
				FuelTank fuelTank = go.AddOrGet<FuelTank>();
				fuelTank.capacityKg = SteamEngineStats.MaxStorage;
				fuelTank.FuelType = ElementLoader.FindElementByHash(SimHashes.Steam).tag;

				var list = new List<Storage.StoredItemModifier>();
				list.Add(Storage.StoredItemModifier.Hide);
				list.Add(Storage.StoredItemModifier.Seal);
				list.Add(Storage.StoredItemModifier.Insulate);
				fuelTank.SetDefaultStoredItemModifiers(list);

				ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
				conduitConsumer.conduitType = ConduitType.Gas;
				conduitConsumer.consumptionRate = 10f;
				conduitConsumer.capacityTag = fuelTank.FuelType;
				conduitConsumer.capacityKG = SteamEngineStats.MaxStorage;
				conduitConsumer.forceAlwaysSatisfied = true;
				conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
				go.AddOrGet<RocketModule>().SetBGKAnim(Assets.GetAnim("rocket_steam_engine_bg_kanim"));
				EntityTemplates.ExtendBuildingToRocketModule(go);
			}
		}

		[HarmonyPatch(typeof(FuelTank))]
		[HarmonyPatch(nameof(FuelTank.MaxCapacity), MethodType.Getter)]
		public static class FuelTank_MaxCapacity
		{
			static void Postfix(FuelTank __instance, ref float __result)
			{
				if (__instance.FuelType == ElementLoader.FindElementByHash(SimHashes.Steam).tag)
				{
					__result = SteamEngineStats.MaxStorage;
				}
				else
				{
					__result = 900;
				}
			}
		}

		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class LoadGeneratedBuildings
		{
			static void Postfix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteamEngineConfig.ID.ToUpperInvariant()}.NAME", SteamEngineStats.NAME);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteamEngineConfig.ID.ToUpperInvariant()}.DESC", SteamEngineStats.DESC);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{SteamEngineConfig.ID.ToUpperInvariant()}.EFFECT", SteamEngineStats.EFFECT);
			}
		}
	}
}
