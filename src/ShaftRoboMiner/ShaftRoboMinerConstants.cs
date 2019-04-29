/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

namespace ShaftRoboMiner
{
	public partial class ShaftRoboMiner
	{
		public static readonly string Id = nameof(ShaftAutoMiner);

		public static readonly string DisplayName = $"Shaft-{STRINGS.BUILDINGS.PREFABS.AUTOMINER.NAME}";
		public static readonly string Description = $"{STRINGS.BUILDINGS.PREFABS.AUTOMINER.DESC}";
		public static readonly LocString Effect = $"{STRINGS.BUILDINGS.PREFABS.AUTOMINER.EFFECT}";

		public static readonly string TechGroup = "SolidTransport";
		public static readonly string BuildTab = "Conveyance";

		public static readonly byte MaxHardness = 220;
		public static readonly float EnergyConsumption = 480f;
		public static readonly float HeatProduction = 2;

		public partial class Range
		{
			public static readonly int width = 4;
			public static readonly int height = 36;

			public static readonly int xOffset = (-width / 2) + 1;
			public static readonly int yOffset = 0;

			public static readonly CellOffset VisionOffset = new CellOffset(0, 1);
		}
	}
}
