/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

namespace AdvancedRoboMiner
{
	public partial class AdvancedRoboMiner
	{
		public static readonly string Id = AutoMinerConfig.ID;
		public static readonly byte MaxHardness = 220;
		public static readonly float EnergyConsumption = 240f;
		public static readonly float HeatProduction = 2;

		public partial class Range
		{
			public static readonly int width = 16;
			public static readonly int height = 9;

			public static readonly int xOffset = (-width / 2) + 1;
			public static readonly int yOffset = 0;

			public static readonly CellOffset VisionOffset = new CellOffset(0, 1);
		}
	}
}
