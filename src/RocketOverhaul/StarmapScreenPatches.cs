/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RocketOverhaul
{
	/// <summary>
	/// Replaces methods on <see cref="RocketStats"/> since it isn't virtual.
	/// </summary>
	public static class StarmapScreenPatches
	{
		[HarmonyPatch(typeof(StarmapScreen))]
		[HarmonyPatch("UpdateRangeDisplay")]
		public static class UpdateRangeDisplay
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(StarmapScreen __instance)
			{
				__instance.Get(out BreakdownList rocketDetailsRange, nameof(rocketDetailsRange));
				BreakdownList list = rocketDetailsRange;
				__instance.Get(out RocketThrustWidget rocketThrustWidget, nameof(rocketThrustWidget));
				__instance.Get(out CommandModule currentCommandModule, nameof(currentCommandModule));
				RocketStatsOverhaul stats = (RocketStatsOverhaul)currentCommandModule.rocketStats;

				list.ClearRows();

				AddImportant(list, StarmapScreenOverhaul.Caption.TotalEngineThrust, stats.GetEngineThrust());
				AddImportantOxidizer(list, StarmapScreenOverhaul.Caption.TotalOxidizerEfficiency, stats.GetAverageOxidizerEfficiency(), stats.GetEfficiencyContribution());
				AddImportant(list, StarmapScreenOverhaul.Caption.TotalBoosterThrust, stats.GetBoosterThrust());
				AddImportant(list, StarmapScreenOverhaul.Caption.TotalPayload, -stats.GetModulePenalty());
				AddImportant(list, StarmapScreenOverhaul.Caption.TotalDistance, stats.GetRocketMaxDistance());
			}
		}

		public static void AddImportantOxidizer(BreakdownList list, string caption, float percent, float value)
		{
			AddImportant(list, caption + ": " + (int)percent + "%", value);
		}

		public static void AddImportant(BreakdownList list, string caption, float value)
		{
			Color color;
			if (value > 0)
				color = Color.green;
			else if (value == 0)
				color = Color.yellow;
			else
				color = Color.red;

			BreakdownListRow row;
			row = list.AddRow();
			row.ShowStatusData(caption, StarmapScreenOverhaul.FormatDistance(value), color);
			row.SetImportant(true);
		}

		[HarmonyPatch(typeof(StarmapScreen))]
		[HarmonyPatch("UpdateMassDisplay")]
		public static class MainEngine
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(StarmapScreen __instance)
			{
				__instance.Get(out BreakdownList rocketDetailsMass, nameof(rocketDetailsMass));
				BreakdownList list = rocketDetailsMass;
				__instance.Get(out RocketThrustWidget rocketThrustWidget, nameof(rocketThrustWidget));
				__instance.Get(out CommandModule currentCommandModule, nameof(currentCommandModule));
				RocketStatsOverhaul stats = (RocketStatsOverhaul)currentCommandModule.rocketStats;
				RocketEngineImproved engine = (RocketEngineImproved)stats.GetMainEngine();
				list.SetTitle(StarmapScreenOverhaul.Caption.TotalEngineThrust);
				list.ClearRows();

				string fuelRange = StarmapScreenOverhaul.FormatDistance(engine.MinFuel, "", "kg") + " to " + StarmapScreenOverhaul.FormatDistance(engine.MaxFuel, "", "kg");

				list.AddRow().ShowData(StarmapScreenOverhaul.Caption.EngineExhaustVelocity, StarmapScreenOverhaul.FormatDistance(engine.ExhaustVelocity, "", "m/s"));
				list.AddRow().ShowData(StarmapScreenOverhaul.Caption.RecommendedOxidizableFuel, fuelRange);
				list.AddRow().ShowData(StarmapScreenOverhaul.Caption.TotalOxidizableFuel, StarmapScreenOverhaul.FormatDistance(stats.GetTotalOxidizableFuel(), "", "kg"));
				AddImportant(list, StarmapScreenOverhaul.Caption.TotalEngineThrust, stats.GetEngineThrust());
			}
		}

		[HarmonyPatch(typeof(StarmapScreen))]
		[HarmonyPatch("UpdateStorageDisplay")]
		public static class OxidizerEfficiency
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(StarmapScreen __instance)
			{
				__instance.Get(out BreakdownList rocketDetailsStorage, nameof(rocketDetailsStorage));
				BreakdownList list = rocketDetailsStorage;
				__instance.Get(out CommandModule currentCommandModule, nameof(currentCommandModule));
				RocketStatsOverhaul stats = (RocketStatsOverhaul)currentCommandModule.rocketStats;

				list.SetTitle(StarmapScreenOverhaul.Caption.TotalOxidizerEfficiency);
				list.SetIcon(__instance.rocketDetailsOxidizerIcon);
				list.gameObject.name = nameof(rocketDetailsStorage);
				list.ClearRows();

				stats.GetOxidizerAmounts(out float oxyrockAmount, out float loxAmount, out float mixedAmount);
				float totalEfficiency = stats.GetAverageOxidizerEfficiency();

				list.AddRow().ShowData(StarmapScreenOverhaul.Caption.OxyRock, StarmapScreenOverhaul.FormatDistance(oxyrockAmount, "", "kg"));
				list.AddRow().ShowData(StarmapScreenOverhaul.Caption.LiquidOxygen, StarmapScreenOverhaul.FormatDistance(loxAmount, "", "kg"));
				list.AddRow().ShowData(StarmapScreenOverhaul.Caption.Mixed, StarmapScreenOverhaul.FormatDistance(mixedAmount, "", "kg"));

				AddImportantOxidizer(list, StarmapScreenOverhaul.Caption.TotalOxidizerEfficiency, totalEfficiency, stats.GetEfficiencyContribution());
			}
		}

		[HarmonyPatch(typeof(StarmapScreen))]
		[HarmonyPatch("UpdateFuelDisplay")]
		public static class SolidBoosters
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(StarmapScreen __instance)
			{
				var ranges = SolidBoosterStats.Ranges;
				List<string> rangeDivisors = new List<string>();
				foreach (var range in ranges)
				{
					rangeDivisors.Add(StarmapScreenOverhaul.FormatDistance(range, ""));
				}

				__instance.Get(out BreakdownList rocketDetailsFuel, nameof(rocketDetailsFuel));
				BreakdownList list = rocketDetailsFuel;
				__instance.Get(out CommandModule currentCommandModule, nameof(currentCommandModule));
				RocketStatsOverhaul stats = (RocketStatsOverhaul)currentCommandModule.rocketStats;

				list.SetTitle(StarmapScreenOverhaul.Caption.TotalBoosterThrust);
				list.SetIcon(__instance.rocketDetailsFuelIcon);
				list.gameObject.name = nameof(rocketDetailsFuel);
				list.ClearRows();

				string moduleName = "";
				int boosterCount = 0;
				foreach (GameObject gameObject in stats.BuildingNetworkEnumerable())
				{
					SolidBooster component = gameObject.GetComponent<SolidBooster>();
					if (component != null)
					{
						moduleName = gameObject.gameObject.GetProperName();
						boosterCount++;
					}
				}

				float boosterKmRemaining = stats.GetBoosterThrust();
				for (int i = 0; i < boosterCount; ++i)
				{
					if (boosterKmRemaining <= GetRange(ranges, i))
					{
						list.AddRow().ShowData(moduleName, StarmapScreenOverhaul.FormatDistance(boosterKmRemaining, "+", "") + "/" + rangeDivisors[i]);
						boosterKmRemaining -= boosterKmRemaining;
					}
					else
					{
						list.AddRow().ShowData(moduleName, StarmapScreenOverhaul.FormatDistance(GetRange(ranges, i), "+", "") + "/" + rangeDivisors[i]);
						boosterKmRemaining -= GetRange(ranges, i);
					}
				}

				AddImportant(list, StarmapScreenOverhaul.Caption.TotalBoosterThrust, stats.GetBoosterThrust());
			}

			public static float GetRange(List<float> ranges, int index)
			{
				if (index < ranges.Count)
					return ranges[index];
				else
					return 0;
			}
		}

		[HarmonyPatch(typeof(StarmapScreen))]
		[HarmonyPatch("UpdateOxidizerDisplay")]
		public static class Payload
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(StarmapScreen __instance)
			{
				__instance.Get(out BreakdownList rocketDetailsOxidizer, nameof(rocketDetailsOxidizer));
				BreakdownList list = rocketDetailsOxidizer;
				__instance.Get(out CommandModule currentCommandModule, nameof(currentCommandModule));
				RocketStatsOverhaul stats = (RocketStatsOverhaul)currentCommandModule.rocketStats;

				list.SetTitle(StarmapScreenOverhaul.Caption.TotalPayload);
				list.SetIcon(__instance.rocketDetailsStorageIcon);
				list.gameObject.name = nameof(rocketDetailsOxidizer);
				list.ClearRows();

				// add row for each module
				foreach (GameObject gameObject in stats.BuildingNetworkEnumerable())
				{
					string penalty = null;
					if (gameObject.GetComponent<CargoBay>() != null)
						penalty = StarmapScreenOverhaul.FormatDistance(-ModulePenalties.CargoPenalty);
					else if (gameObject.GetComponent<TouristModule>() != null)
						penalty = StarmapScreenOverhaul.FormatDistance(-ModulePenalties.TouristPenalty);
					else if (gameObject.GetComponent<ResearchModule>() != null)
						penalty = StarmapScreenOverhaul.FormatDistance(-ModulePenalties.ResearchPenalty);

					if (penalty != null) // was this a module that we are counting?
					{
						string moduleName = gameObject.gameObject.GetProperName();
						list.AddRow().ShowData(moduleName, penalty);
					}
				}

				AddImportant(list, StarmapScreenOverhaul.Caption.TotalPayload, -stats.GetModulePenalty());
			}
		}

		/// <summary>
		/// Easily get a private instance field of type T from StarmapScreen.
		/// Note that something like an int can't be set via this.
		/// </summary>
		public static void Get<T>(this StarmapScreen screen, out T value, string fieldName)
		{
			Type type = screen.GetType();
			FieldInfo field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
			if (field == null)
			{
				field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public);
			}

			value = (T)field.GetValue(screen);
		}
	}
}
