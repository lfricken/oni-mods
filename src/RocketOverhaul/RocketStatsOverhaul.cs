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
		/// Adds a public copy of CommandModule.
		/// </summary>
		public RocketStatsOverhaul(CommandModule commandModule) : base(commandModule)
		{
			var field = typeof(RocketStats).GetField("commandModule", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);
			CommandModule = (CommandModule)field.GetValue(this);
		}

		/// <summary>
		/// Iterates over game objects in AttachableBuilding.GetAttachedNetwork
		/// </summary>
		public IEnumerable<GameObject> BuildingNetworkEnumerable()
		{
			foreach (GameObject gameObject in AttachableBuilding.GetAttachedNetwork(CommandModule.GetComponent<AttachableBuilding>()))
			{
				yield return gameObject;
			}
		}

		/// <summary>
		/// Given 100 returns 1.0
		/// </summary>
		public static float PercentageToFraction(float percentage)
		{
			return percentage / 100f;
		}
		#endregion

		#region Oxidizer
		/// <summary>
		/// Return a value representing the oxidizer efficiency.
		/// Returns 100 for 100% effective.
		/// </summary>
		public new float GetAverageOxidizerEfficiency()
		{
			return 100f * GetEfficiency();
		}

		public float GetOxyMultiplier()
		{
			return PercentageToFraction(GetAverageOxidizerEfficiency());
		}

		public float GetOxidizerAmounts(out float oxyrock, out float lox, out float mixed)
		{
			GetOxidizerAmounts(out float totalOxyrock, out float totalLox);
			GetEfficiencyAmounts(totalOxyrock, totalLox, out oxyrock, out lox, out mixed);
			return totalOxyrock + totalLox;
		}

		public void GetOxidizerAmounts(out float oxyrock, out float lox)
		{
			oxyrock = 0;
			lox = 0;

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
							oxyrock += oxidizer.Value;
						}
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void GetEfficiencyAmounts(float oxyRock, float lox, out float oxyRockAmount, out float loxAmount, out float mixedAmount)
		{
			oxyRockAmount = Mathf.Max(0, oxyRock - lox);
			loxAmount = Mathf.Max(0, lox - oxyRock);
			mixedAmount = Mathf.Min(lox, oxyRock) * 2;
		}

		/// <summary>
		/// 
		/// </summary>
		public float GetEfficiency()
		{
			float totalOxidizer = GetOxidizerAmounts(out float oxyrockAmount, out float loxAmount, out float mixedAmount);

			float sum = OxidizerEfficiency.OxyRock * oxyrockAmount + OxidizerEfficiency.Lox * loxAmount + OxidizerEfficiency.Mixed * mixedAmount;
			return sum / totalOxidizer;
		}

		public float GetEfficiencyContribution()
		{
			return (GetEfficiency() - 1) * GetEngineThrust();
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
		/// Return how much boosters increase our range by.
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
		/// Expected distance of this rocket.
		/// </summary>
		public new float GetRocketMaxDistance()
		{
			float thrust = GetTotalThrust() - GetModulePenalty();
			float dist = Mathf.Max(0, thrust);
			return dist;
		}

		/// <summary>
		/// All contributions to distance in km summed. Includes engine penalty.
		/// </summary>
		public new float GetTotalThrust()
		{
			return GetEngineThrust() * GetOxyMultiplier() + GetBoosterThrust();
		}

		/// <summary>
		/// Returns the total contribution by the engine. This includes the engine penalty.
		/// </summary>
		public float GetEngineThrust()
		{
			float fuel = GetTotalOxidizableFuel();
			return GetEngineContribution(fuel);
		}


		public new RocketEngineImproved GetMainEngine()
		{
			RocketEngineImproved rocketEngine = null;
			foreach (GameObject gameObject in BuildingNetworkEnumerable())
			{
				rocketEngine = gameObject.GetComponent<RocketEngineImproved>();
				if (rocketEngine != null)
				{
					if (rocketEngine.mainEngine)
						break;
				}
			}
			return rocketEngine;
		}

		public float GetEngineContribution(float fuel)
		{
			RocketEngineImproved engine = GetMainEngine() as RocketEngineImproved;
			if (engine == null)
			{
				Debug.Log($"A rocket engine does not implement the {nameof(RocketEngineImproved)}.");
				return 0;
			}

			fuel /= DistanceEquationScalars.FuelPerXTick;
			float exponentPenalty = -GetFuelPenalty(fuel);
			float linearBenefit = fuel * engine.ExhaustVelocity * DistanceEquationScalars.Exhaust;

			float thrust = exponentPenalty + linearBenefit;
			float max_range = DistanceEquationScalars.Range * thrust - engine.RangePenalty;

			return max_range;
		}

		#endregion

		#region Penalty
		/// <summary>
		/// Computes the diminishing return value of fuel.
		/// </summary>
		public float GetFuelPenalty(float fuel)
		{
			return fuel * fuel;
		}

		/// <summary>
		/// Returns the total penalty from modules.
		/// </summary>
		public float GetModulePenalty()
		{
			GetModuleCount(out int cargoBays, out int touristModules, out int researchModules);
			float penalty = ModulePenalties.CargoPenalty * cargoBays + ModulePenalties.TouristPenalty * touristModules + ModulePenalties.ResearchPenalty * researchModules;
			return penalty;
		}

		/// <summary>
		/// Outputs the number of each module on this rocket.
		/// </summary>
		public void GetModuleCount(out int cargoBays, out int touristModules, out int researchModules)
		{
			cargoBays = 0;
			touristModules = 0;
			researchModules = 0;

			foreach (GameObject gameObject in BuildingNetworkEnumerable())
			{
				if (gameObject.GetComponent<CargoBay>() != null)
				{
					cargoBays += 1;
				}
				else if (gameObject.GetComponent<TouristModule>() != null)
				{
					touristModules += 1;
				}
				else if (gameObject.GetComponent<ResearchModule>() != null)
				{
					researchModules += 1;
				}
			}
		}
		#endregion
	}
}
