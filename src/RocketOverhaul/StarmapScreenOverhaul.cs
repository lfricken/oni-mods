/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>


namespace RocketOverhaul
{
	public class StarmapScreenOverhaul
	{
		public class Caption
		{
			public static string TotalDistance = "Total Range";
		}

		/// <summary>
		/// Returns 
		/// </summary>
		public static string FormatDistance(float distanceInKm)
		{
			return distanceInKm + Unit;
		}
		private static string Unit = "km";
	}
}
