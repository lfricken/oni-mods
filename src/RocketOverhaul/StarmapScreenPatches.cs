/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using System;
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
				bool isNull = __instance == null;
				Debug.Log(isNull);
				__instance.Get(out BreakdownList rocketDetailsRange, nameof(rocketDetailsRange));
				__instance.Get(out RocketThrustWidget rocketThrustWidget, nameof(rocketThrustWidget));
				__instance.Get(out CommandModule currentCommandModule, nameof(currentCommandModule));
				RocketStatsOverhaul stats = (RocketStatsOverhaul)currentCommandModule.rocketStats;
				RocketEngineImproved engine = (RocketEngineImproved)stats.GetMainEngine();

				string engineEfficiency = string.Format(StarmapScreenOverhaul.FormatNoDecimalAndCommas, engine.ExhaustVelocity) + "m/s";
				string engineThrust = StarmapScreenOverhaul.FormatDistance(stats.GetEngineThrust());
				string boosterThrust = StarmapScreenOverhaul.FormatDistance(stats.GetBoosterThrust());
				string modulePenalty = StarmapScreenOverhaul.FormatDistance(-stats.GetModulePenalty());
				string totalDistance = StarmapScreenOverhaul.FormatDistance(stats.GetRocketMaxDistance());
				Color orange = new Color(1f, 0.55f, 0f);

				rocketDetailsRange.ClearRows();

				rocketDetailsRange.AddRow().ShowData(StarmapScreenOverhaul.Caption.TotalOxidizableFuel, GameUtil.GetFormattedMass(stats.GetTotalOxidizableFuel(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				rocketDetailsRange.AddRow().ShowData(StarmapScreenOverhaul.Caption.EngineExhaustVelocity, engineEfficiency);
				rocketDetailsRange.AddRow().ShowData(StarmapScreenOverhaul.Caption.AverageOxidizerEfficiency, GameUtil.GetFormattedPercent(stats.GetAverageOxidizerEfficiency(), GameUtil.TimeSlice.None));

				BreakdownListRow row;
				row = rocketDetailsRange.AddRow();
				row.ShowStatusData(StarmapScreenOverhaul.Caption.TotalEngineThrust, engineThrust, Color.green);
				row.SetImportant(true);

				row = rocketDetailsRange.AddRow();
				row.ShowStatusData(StarmapScreenOverhaul.Caption.TotalBoosterThrust, boosterThrust, Color.green);
				row.SetImportant(true);

				row = rocketDetailsRange.AddRow();
				row.ShowStatusData(StarmapScreenOverhaul.Caption.Payload, modulePenalty, Color.red);
				row.SetImportant(true);

				row = rocketDetailsRange.AddRow();
				row.ShowStatusData(StarmapScreenOverhaul.Caption.TotalDistance, totalDistance, orange);
				row.SetImportant(true);
			}
		}

		[HarmonyPatch(typeof(StarmapScreen))]
		[HarmonyPatch("UpdateStorageDisplay")]
		public static class UpdateStorageDisplay
		{
			static bool Prefix() { return true; } // skip original method
			static void Postfix(StarmapScreen __instance)
			{
				__instance.Get(out BreakdownList rocketDetailsStorage, nameof(rocketDetailsStorage));
				__instance.Get(out CommandModule currentCommandModule, nameof(currentCommandModule));
				RocketStatsOverhaul stats = (RocketStatsOverhaul)currentCommandModule.rocketStats;

				rocketDetailsStorage.SetTitle(StarmapScreenOverhaul.Caption.Payload);
				rocketDetailsStorage.ClearRows();

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
						rocketDetailsStorage.AddRow().ShowData(moduleName, penalty);
					}
				}
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
