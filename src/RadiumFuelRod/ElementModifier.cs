namespace RadiumFuelRod
{
	public class ElementModifier
	{
		public Names names;
		public Temperatures temps;

		public ElementModifier(float melting, float boiling, float specificHeatCapacity, SimHashes solidName, SimHashes liquidName, SimHashes gasName)
		{
			names = new Names();
			temps = new Temperatures();

			names.Solid = solidName;
			names.Liquid = liquidName;
			names.Gas = gasName;

			temps.Melting = melting;
			temps.Boiling = boiling;
			temps.SpecificHeatCapacity = specificHeatCapacity;
		}

		public void ApplyChanges()
		{
			Element element;

			// solid
			element = ElementLoader.FindElementByHash(names.Solid);
			element.specificHeatCapacity = temps.SpecificHeatCapacity;

			element.highTemp = temps.Melting;
			element.highTempTransitionTarget = names.Liquid;
			element.highTempTransition = ElementLoader.FindElementByHash(element.highTempTransitionTarget);

			// liquid
			element = ElementLoader.FindElementByHash(names.Liquid);
			element.specificHeatCapacity = temps.SpecificHeatCapacity;

			element.lowTemp = temps.Melting;
			element.lowTempTransitionTarget = names.Solid;
			element.lowTempTransition = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);

			element.highTemp = temps.Boiling;
			element.highTempTransitionTarget = names.Gas;
			element.highTempTransition = ElementLoader.FindElementByHash(element.highTempTransitionTarget);

			// gas
			element = ElementLoader.FindElementByHash(names.Gas);
			element.specificHeatCapacity = temps.SpecificHeatCapacity;

			element.lowTemp = temps.Boiling;
			element.lowTempTransitionTarget = names.Liquid;
			element.lowTempTransition = ElementLoader.FindElementByHash(element.lowTempTransitionTarget);
		}
	}

	public class Names
	{
		public SimHashes Solid = SimHashes.RefinedCarbon;
		public SimHashes Liquid = SimHashes.MoltenCarbon;
		public SimHashes Gas = SimHashes.CarbonGas;
	}

	public class Temperatures
	{
		public float Boiling = 8900.24f;
		public float Melting = 9421.56f;
		public float SpecificHeatCapacity = 2.85f;
	}
}
