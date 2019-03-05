/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace RocketOverhaul
{
	/// <summary>
	/// 
	/// </summary>
	public class RocketStatsOverhaul : RocketStats
	{
		#region Properties / Constructors / Misc
		/// <summary>
		/// 
		/// </summary>
		public CommandModule CommandModule { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public RocketStatsOverhaul(CommandModule commandModule) : base(commandModule)
		{
			var field = typeof(RocketStats).GetField("commandModule", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
			CommandModule = (CommandModule)field.GetValue(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<GameObject> BuildingNetworkEnumerable()
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(CommandModule.GetComponent<AttachableBuilding>()))
			{
				yield return gameObject;
			}
		}
		#endregion

		#region Oxidizer
		/// <summary>
		/// 
		/// </summary>
		public new float GetAverageOxidizerEfficiency()
		{
			float lox = 0;
			float oxyRock = 0;

			foreach (GameObject gameObject in BuildingNetworkEnumerable())
			{
				OxidizerTank component = gameObject.GetComponent<OxidizerTank>();
				if (component != null)
				{
					foreach (KeyValuePair<Tag, float> oxidizer in component.GetOxidizersAvailable())
					{
						if (oxidizer.Key == SimHashes.LiquidOxygen.CreateTag())
						{
							lox += oxidizer.Value;
						}
						else if (oxidizer.Key == SimHashes.OxyRock.CreateTag())
						{
							oxyRock += oxidizer.Value;
						}
					}
				}
			}

			return 100.0f * GetEfficiency(lox, oxyRock);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lox"></param>
		/// <param name="oxyRock"></param>
		public float GetEfficiency(float lox, float oxyRock)
		{
			float totalOxidizer = lox + oxyRock;

			float mixedAmount = Mathf.Min(lox, oxyRock) * 2;
			float extraLox = Mathf.Max(0, lox - oxyRock);
			float extraOxyRock = Mathf.Max(0, oxyRock - lox);
			Assert.AreApproximatelyEqual(totalOxidizer, mixedAmount + extraLox + extraOxyRock);

			float sum = OxidizerEfficiency.Mixed * mixedAmount + OxidizerEfficiency.Lox * extraLox + OxidizerEfficiency.OxyRock * extraOxyRock;
			return sum / totalOxidizer;
		}
		#endregion

		#region Boosters
		/// <summary>
		/// Returns how much do boosters contribute to rocket distance in km.
		/// </summary>
		public new float GetBoosterThrust()
		{
			float fuel = 0;
			float oxidizer = 0;

			foreach (GameObject gameObject in BuildingNetworkEnumerable())
			{
				SolidBooster component = gameObject.GetComponent<SolidBooster>();
				if (component != null)
				{
					fuel += component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.OxyRock).tag);
					oxidizer += component.fuelStorage.GetAmountAvailable(ElementLoader.FindElementByHash(SimHashes.Iron).tag);
				}
			}

			fuel /= SolidBoosterStats.MaxFuelStorage;
			oxidizer /= SolidBoosterStats.MaxOxyStorage;
			float numEffectiveBoosters = Mathf.Min(fuel, oxidizer);
			float thrust = GetBoosterContribution(numEffectiveBoosters);

			return thrust;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="numEffectiveBoosters">How many boosters worth of booster fuel do we have?</param>
		public static float GetBoosterContribution(float numEffectiveBoosters)
		{
			List<float> ranges = SolidBoosterStats.Ranges;

			if (numEffectiveBoosters <= 0)
				return 0;

			float totalRange = 0;
			for (int i = 0; numEffectiveBoosters > 0 && i < ranges.Count; i++)
			{
				if (numEffectiveBoosters >= 1)
					totalRange += 1 * ranges[i];
				else
					totalRange += numEffectiveBoosters * ranges[i];
				numEffectiveBoosters -= 1;
			}

			return totalRange;
		}
		#endregion

		#region Thrust
		/// <summary>
		/// 
		/// </summary>
		public new float GetRocketMaxDistance()
		{
			float thrust = GetTotalThrust() - GetRangePenalty();
			float dist = Mathf.Max(0, thrust);
			return dist;
		}

		/// <summary>
		/// All contributions to distance in km summed.
		/// </summary>
		public new float GetTotalThrust()
		{
			return GetEngineThrust() + GetBoosterThrust();
		}

		/// <summary>
		/// 
		/// </summary>
		public float GetEngineThrust()
		{
			return GetAverageOxidizerEfficiency() * GetTotalOxidizableFuel() * Scalars.Efficiency;
		}
		#endregion

		#region Penalty
		public float GetRangePenalty()
		{
			return 0;
		}

		public void GetModuleCount(out int cargoBays, out int seeingModules, out int scienceBays)
		{
			cargoBays = 0;
			seeingModules = 0;
			scienceBays = 0;
		}
		#endregion
	}
}
