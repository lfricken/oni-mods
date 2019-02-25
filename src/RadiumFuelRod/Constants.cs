using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadiumFuelRod
{
	public partial class RadiumFuelRod
	{
		public static readonly string DisplayName = "Nuclear Fuel Rod";
		public static readonly string Description = "Uses fission to produce heat.";
		public static readonly LocString Effect = "Produces " + UI.FormatAsLink("Heat", "HEAT") + ".";

		public const float HeatOutput = 4064f;
	}

	public class Radium
	{
		public const float MeltingPoint = 8913.24f;
		public const float SpecificHeatCapacity = 2.758f;

		public class Recipe
		{
			public const float Niobium = 5;
			public const float Tungsten = 95;
			public const float RefinedCarbon = 300;
			public const float AmountProduced = Niobium + Tungsten + RefinedCarbon;

			public const float Time = 100;
			public const string Description = "Radium is a radioactive alkaline earth metal that is used in the construction of nuclear devices.";
		}
	}

	public class Carbon
	{
		public const float MeltingPoint = 8913.24f;
		public const float BoilingPoint = 9421.56f;
		public const float SpecificHeatCapacity = 2.85f;
	}
}
