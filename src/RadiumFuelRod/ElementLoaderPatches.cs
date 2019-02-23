using Harmony;

namespace RadiumFuelRod
{
	class ElementLoaderPatches
	{
		[HarmonyPatch(typeof(ElementLoader))]
		[HarmonyPatch("LoadUserElementData")]
		public static partial class LoadUserElementData_Patch
		{
			static void Postfix()
			{
				var solidCarbon = ElementLoader.FindElementByHash(SimHashes.RefinedCarbon);
				solidCarbon.highTemp = 9000;
				var liqCarbon = ElementLoader.FindElementByHash(SimHashes.MoltenCarbon);
				liqCarbon.lowTemp = 9000;
				liqCarbon.highTemp = 9500;
				var gasCarbon = ElementLoader.FindElementByHash(SimHashes.CarbonGas);
				gasCarbon.lowTemp = 9500;

				var t = gasCarbon.MemberwiseClone();
			}
		}
	}
}
