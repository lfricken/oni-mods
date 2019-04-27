/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

namespace AdvancedRoboMiner
{
	public partial class AdvancedRoboMiner
	{
		public static readonly byte MaxHardness = 220;
		public static readonly float EnergyConsumption = 2000;
		public static readonly float HeatProduction = 6;

		public partial class Range
		{
			public static readonly int width = 20;
			public static readonly int height = 11;

			public static readonly int xOffset = (-width / 2) + 1;

			public static readonly CellOffset VisionOffset = new CellOffset(0, 1);
		}
	}
}
