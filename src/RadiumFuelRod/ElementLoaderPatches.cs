/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;

namespace RadiumFuelRod
{
	public class ElementLoaderPatches
	{
		[HarmonyPatch(typeof(ElementLoader))]
		[HarmonyPatch(nameof(ElementLoader.Load))]
		public static partial class LoadUserElementData_Patch
		{
			static void Postfix()
			{
				var carbon = new ElementModifier(Carbon.MeltingPoint, Carbon.BoilingPoint, Carbon.SpecificHeatCapacity, SimHashes.RefinedCarbon, SimHashes.MoltenCarbon, SimHashes.CarbonGas);

				Element radium;
				radium = ElementLoader.FindElementByHash(SimHashes.Radium);
				radium.specificHeatCapacity = Radium.SpecificHeatCapacity;
				radium.highTemp = Radium.MeltingPoint;
			}
		}
	}
}
