using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace Autosweeperignoretiles
{
	[HarmonyPatch(typeof(Grid), "IsPhysicallyAccessible", null)]
	internal class AutoSweeperThroughWallsMod_Grid_IsPhysicallyAccessible
	{
		private static bool Prefix(ref bool __result)
		{
			__result = true;
			return false;
		}
	}
}
