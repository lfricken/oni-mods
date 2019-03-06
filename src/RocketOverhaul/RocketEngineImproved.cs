/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using KSerialization;

namespace RocketOverhaul
{
	/// <summary>
	/// 
	/// </summary>
	[SerializationConfig(MemberSerialization.OptIn)]
	public class RocketEngineImproved : RocketEngine
	{
		/// <summary>
		/// 
		/// </summary>
		public float RangePenalty { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public float ExhaustVelocity { get; set; }
	}
}
