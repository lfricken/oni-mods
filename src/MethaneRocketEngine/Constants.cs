/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using STRINGS;

namespace MethaneRocketEngine
{
	public partial class MethaneRocketEngine
	{
		public static readonly string Id = nameof(MethaneRocketEngine);
		public static readonly string DisplayName = "meth rocket";
		public static readonly string Description = "desc";
		public static readonly LocString Effect = "asdf " + UI.FormatAsLink("Heat", "HEAT") + ".";

		public static readonly string TechGroup = "Catalytics";
		public static readonly string BuildTab = "Power";
	}
}
