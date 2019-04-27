/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using STRINGS;

namespace RadiumFuelRod
{
	public partial class RadiumFuelRod
	{
		public static readonly string DisplayName = "Nuclear Fuel Rod";
		public static readonly string Description = "Uses fission to produce heat.";
		public static readonly LocString Effect = "Produces " + UI.FormatAsLink("Heat", "HEAT") + ".";

		public static readonly string TechGroup = "Catalytics";
		public static readonly string BuildTab = "Power";

		public static readonly float HeatOutput = 8128f;
	}

	public class Radium
	{
		public static readonly float MeltingPoint = 8913.24f;
		public static readonly float SpecificHeatCapacity = 2.758f;
		public static readonly string Name = nameof(Radium);

		public class Recipe
		{
			public static readonly float Niobium = 5;
			public static readonly float Tungsten = 95;
			public static readonly float RefinedCarbon = 300;
			public static readonly float AmountProduced = Niobium + Tungsten + RefinedCarbon;

			public static readonly float Time = 100;
			public static readonly string Description = $"{UI.FormatAsLink(Name, Name.ToUpper())} is a radioactive alkaline earth metal that is used in the construction of nuclear devices.";
		}
	}

	public class Carbon
	{
		public static readonly float MeltingPoint = 8913.24f;
		public static readonly float BoilingPoint = 9421.56f;
		public static readonly float SpecificHeatCapacity = 2.85f;
	}
}
