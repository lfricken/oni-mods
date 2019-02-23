using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;


namespace BuildingExtension
{

	[HarmonyPatch(typeof(ConduitFlow), MethodType.Constructor, new Type[] { typeof(ConduitType), typeof(int), typeof(IUtilityNetworkMgr), typeof(float), typeof(float) })]
	public static class PipesMass
	{
		public static void Prefix(ConduitType conduit_type, int num_cells, IUtilityNetworkMgr network_mgr, ref float max_conduit_mass, float initial_elapsed_time)
		{ max_conduit_mass = 400; }
	}


	[HarmonyPatch(typeof(LiquidLogicValveConfig), "ConfigureBuildingTemplate")]
	public static class LiquidLogicValveConfigDynamicPatch
	{
		public static void Postfix(LiquidLogicValveConfig __instance, ref GameObject go)
		{
			OperationalValve operationalValve = go.AddOrGet<OperationalValve>();
			operationalValve.maxFlow = 400;
		}
	}

	[HarmonyPatch(typeof(GasLogicValveConfig), "ConfigureBuildingTemplate")]
	public static class GasLogicValveConfigDynamicPatch
	{
		public static void Postfix(GasLogicValveConfig __instance, ref GameObject go)
		{
			OperationalValve operationalValve = go.AddOrGet<OperationalValve>();
			operationalValve.maxFlow = 400;
		}
	}


}
