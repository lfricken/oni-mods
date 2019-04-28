/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

namespace ShaftRoboMiner
{
	public partial class ShaftRoboMiner
	{
		public static readonly string Id = nameof(ShaftRoboMiner);

		public static readonly string DisplayName = $"Shaft Robo-Miner";
		public static readonly string Description = $"asdf";
		public static readonly LocString Effect = $"bbbb";

		public static readonly string TechGroup = "ValveMiniaturization";
		public static readonly string BuildTab = "Plumbing";

		public static readonly byte MaxHardness = 220;
		public static readonly float EnergyConsumption = 240f;
		public static readonly float HeatProduction = 2;

		public partial class Range
		{
			public static readonly int width = 36;
			public static readonly int height = 4;

			public static readonly int xOffset = (-width / 2) + 1;
			public static readonly int yOffset = 0;

			public static readonly CellOffset VisionOffset = new CellOffset(0, 1);
		}
	}
}
