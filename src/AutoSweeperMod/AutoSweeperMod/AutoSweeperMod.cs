using Harmony;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Reflection;

namespace AutoSweeperMod
{
	[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigureComplete", null)]
	public static class AutoSweeperMod
	{
		public static int RANGE = 22;
		public static void Postfix(GameObject go)
		{
			go.AddOrGet<SolidTransferArm>().pickupRange = RANGE;
			StationaryChoreRangeVisualizer choreRangeVisualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
			choreRangeVisualizer.x = -RANGE;
			choreRangeVisualizer.y = -RANGE;
			choreRangeVisualizer.width = (RANGE * 2) + 1;
			choreRangeVisualizer.height = (RANGE * 2) + 1;
			//configfile._streamwriter.Write("Load autosweeper");
			//Debug.Log("Autosweepermod");
			//configfile.moddebuglog("Autosweeper Loaded");
		}
	}

	
	[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigurePreview", null)]
	public static class AutoSweeperModb
	{
		public static int RANGE = AutoSweeperMod.RANGE;
		public static void Postfix(ref GameObject go)
		{
			StationaryChoreRangeVisualizer choreRangeVisualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
			choreRangeVisualizer.x = -RANGE;
			choreRangeVisualizer.y = -RANGE;
			choreRangeVisualizer.width = (RANGE * 2) + 1;
			choreRangeVisualizer.height = (RANGE * 2) + 1;
		}
	}
	[HarmonyPatch(typeof(SolidTransferArmConfig), "CreateBuildingDef", null)]
	public static class AutoSweeperModc
	{
		public static void Postfix(BuildingDef __result)
		{
			__result.RequiresPowerInput = false;
			__result.EnergyConsumptionWhenActive = 0;
		}
	}




	
	[HarmonyPatch(typeof(SolidTransferArmConfig), "DoPostConfigureUnderConstruction", null)]
	public static class AutoSweeperModd
	{
		public static int RANGE = AutoSweeperMod.RANGE;
		public static void Postfix(GameObject go)
		{
		StationaryChoreRangeVisualizer choreRangeVisualizer = go.AddOrGet<StationaryChoreRangeVisualizer>();
		choreRangeVisualizer.x = -RANGE;
		choreRangeVisualizer.y = -RANGE;
		choreRangeVisualizer.width = (RANGE * 2) + 1;
		choreRangeVisualizer.height = (RANGE * 2) + 1;
		}
	}
}


