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

				rocketDetailsRange.ClearRows();

				rocketDetailsRange.AddRow().ShowData(STRINGS.UI.STARMAP.ROCKETSTATS.TOTAL_OXIDIZABLE_FUEL, GameUtil.GetFormattedMass(currentCommandModule.rocketStats.GetTotalOxidizableFuel(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}"));
				rocketDetailsRange.AddRow().ShowData(STRINGS.UI.STARMAP.ROCKETSTATS.ENGINE_EFFICIENCY, GameUtil.GetFormattedEngineEfficiency(currentCommandModule.rocketStats.GetEngineEfficiency()));
				rocketDetailsRange.AddRow().ShowData(STRINGS.UI.STARMAP.ROCKETSTATS.OXIDIZER_EFFICIENCY, GameUtil.GetFormattedPercent(currentCommandModule.rocketStats.GetAverageOxidizerEfficiency(), GameUtil.TimeSlice.None));

				float meters = currentCommandModule.rocketStats.GetBoosterThrust() * 1000f;
				if (meters != 0.0)
					rocketDetailsRange.AddRow().ShowData(STRINGS.UI.STARMAP.ROCKETSTATS.SOLID_BOOSTER, GameUtil.GetFormattedDistance(meters));

				BreakdownListRow breakdownListRow1 = rocketDetailsRange.AddRow();
				breakdownListRow1.ShowStatusData(STRINGS.UI.STARMAP.ROCKETSTATS.TOTAL_THRUST, GameUtil.GetFormattedDistance(currentCommandModule.rocketStats.GetTotalThrust() * 1000f), Color.green);
				breakdownListRow1.SetImportant(true);

				rocketThrustWidget.gameObject.SetActive(true);

				BreakdownListRow breakdownListRow2 = rocketDetailsRange.AddRow();
				breakdownListRow2.ShowStatusData((string)STRINGS.UI.STARMAP.ROCKETSTATUS.WEIGHTPENALTY, "hi", Color.red);
				breakdownListRow2.SetHighlighted(true);

				// graph
				rocketDetailsRange.AddCustomRow(rocketThrustWidget.gameObject);
				rocketThrustWidget.Draw(currentCommandModule);

				BreakdownListRow breakdownListRow3 = rocketDetailsRange.AddRow();
				breakdownListRow3.ShowData(StarmapScreenOverhaul.Caption.TotalDistance, StarmapScreenOverhaul.FormatDistance(currentCommandModule.rocketStats.GetRocketMaxDistance()));
				breakdownListRow3.SetImportant(true);
			}
		}

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
