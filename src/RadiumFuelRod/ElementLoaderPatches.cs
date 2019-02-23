/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;

namespace RadiumFuelRod
{
	public static class Carbon
	{
		public const float MeltingPoint = 8913.24f;
		public const float BoilingPoint = 9421.56f;
	}

	public static class Radium
	{
		public const float MeltingPoint = 8913.24f;
		public const float SpecificHeatCapacity = 2.758f;
	}

	public class ElementLoaderPatches
	{
		[HarmonyPatch(typeof(ElementLoader))]
		[HarmonyPatch("LoadUserElementData")]
		public static partial class LoadUserElementData_Patch
		{
			static void Postfix()
			{
				ElementModifier carbon = new ElementModifier(Carbon.MeltingPoint, Carbon.BoilingPoint, 2.85f, SimHashes.RefinedCarbon, SimHashes.MoltenCarbon, SimHashes.CarbonGas);
				carbon.ApplyChanges();

				Element radium;
				radium = ElementLoader.FindElementByHash(SimHashes.Radium);
				radium.specificHeatCapacity = Radium.SpecificHeatCapacity;
				radium.highTemp = Radium.MeltingPoint;
			}
		}
	}
}
