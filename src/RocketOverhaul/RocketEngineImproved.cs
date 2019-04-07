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
		public float RangePenalty;

		/// <summary>
		/// 
		/// </summary>
		public float ExhaustVelocity;

		/// <summary>
		/// 
		/// </summary>
		public float MinFuel;

		/// <summary>
		/// 
		/// </summary>
		public float MaxFuel;

		/// <summary>
		/// True if this engine only uses fuel.
		/// </summary>
		public bool OnlyFuel;
	}
}
