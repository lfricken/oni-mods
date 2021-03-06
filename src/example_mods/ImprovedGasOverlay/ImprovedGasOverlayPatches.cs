﻿using Harmony;
using UnityEngine;

namespace ImprovedGasOverlay
{
	public static class ImprovedGasOverlayPatches
	{
		[HarmonyPatch(typeof(SimDebugView))]
		[HarmonyPatch("GetOxygenMapColour")]
		public static class SimDebugView_GetOxygenMapColour_Patch
		{
			public static bool Prefix(int cell, ref Color __result)
			{
				var element = Grid.Element[cell];

				if (!element.IsGas)
				{
					__result = ImprovedGasOverlayConfig.NotGasColor;
					return false;
				}

				var mass = Grid.Mass[cell];

				__result = ColorUtils.GetGasColor(element.id, ColorUtils.GetCellOverlayColor(cell),
					ColorUtils.GetPressureFraction(mass, ImprovedGasOverlayConfig.GasPressureEnd), mass);

				return false;
			}
		}
	}
}
