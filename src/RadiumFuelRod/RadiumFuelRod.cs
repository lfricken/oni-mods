/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Database;
using Harmony;
using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

namespace RadiumFuelRod
{
	/// <summary>
	/// Animations, 
	/// </summary>
	public partial class RadiumFuelRod : StateMachineComponent<RadiumFuelRod.SMInstance>
	{
		[MyCmpGet]
		private readonly Operational _operational;

		public RadiumFuelRod(Operational operational)
		{
			_operational = operational;
		}

		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		public class SMInstance : GameStateMachine<States, SMInstance, RadiumFuelRod, object>.GameInstance
		{
			private readonly Operational _operational;

			public SMInstance(RadiumFuelRod master) : base(master)
			{
				_operational = master.GetComponent<Operational>();
			}

			public bool IsOperational => true;

			public bool HasLight()
			{
				var cell = Grid.PosToCell(smi.master.transform.GetPosition());
				return Grid.LightCount[cell] > 0;
			}
		}

		public class States : GameStateMachine<States, SMInstance, RadiumFuelRod>
		{
			public override void InitializeStates(out BaseState defaultState)
			{
				defaultState = root;

				root
					.QueueAnim("working_loop", true);
			}
		}
	}

	/// <summary>
	/// Description, stats, functionality.
	/// </summary>
	public class RadiumFuelRodConfig : IBuildingConfig
	{
		public static readonly string Id = nameof(RadiumFuelRod);
		/// <summary>
		/// Define construction costs, health, size, noises, random stats.
		/// </summary>
		public override BuildingDef CreateBuildingDef()
		{
			var buildingDef = BuildingTemplates.CreateBuildingDef(
				id: Id,
				width: 1,
				height: 1,
				anim: "thermalblock_kanim",
				hitpoints: BUILDINGS.HITPOINTS.TIER4,
				construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER6,
				construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER7,
				construction_materials: new string[1] { "Radium" },
				melting_point: Radium.MeltingPoint,
				build_location_rule: BuildLocationRule.Anywhere,
				decor: BUILDINGS.DECOR.PENALTY.TIER4,
				noise: NOISE_POLLUTION.NOISY.TIER0);

			buildingDef.Overheatable = true;
			buildingDef.OverheatTemperature = Radium.MeltingPoint;
			buildingDef.RequiresPowerInput = false;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.BaseTimeUntilRepair = -1f;

			buildingDef.AudioCategory = "HollowMetal";
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.ExhaustKilowattsWhenActive = 0f;
			buildingDef.SelfHeatKilowattsWhenActive = RadiumFuelRod.HeatOutput;

			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			DefineHeater(go);
			go.AddOrGet<AnimTileable>();
			var fuelRod = go.AddOrGet<RadiumFuelRod>();

			Prioritizable.AddRef(go);
		}

		public void DefineHeater(GameObject go)
		{
			go.AddOrGet<LoopingSounds>();
			var heater = go.AddOrGet<SuperHeater>();
			heater.targetTemperature = Radium.MeltingPoint + 5;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			BuildingTemplates.DoPostConfigure(go);
		}
	}

	/// <summary>RadiumFuelRod
	/// Set Name, Description, Effect description, Tech Grouping, buildscreen.
	/// </summary>
	public class RadiumFuelRodConfigPatches
	{
		[HarmonyPatch(typeof(GeneratedBuildings))]
		[HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
		public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
		{
			public static void Prefix()
			{
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{RadiumFuelRodConfig.Id.ToUpperInvariant()}.NAME", RadiumFuelRod.DisplayName);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{RadiumFuelRodConfig.Id.ToUpperInvariant()}.DESC", RadiumFuelRod.Description);
				Strings.Add($"STRINGS.BUILDINGS.PREFABS.{RadiumFuelRodConfig.Id.ToUpperInvariant()}.EFFECT", RadiumFuelRod.Effect);

				ModUtil.AddBuildingToPlanScreen(RadiumFuelRod.BuildTab, RadiumFuelRodConfig.Id);
			}
		}

		[HarmonyPatch(typeof(Db))]
		[HarmonyPatch(nameof(Db.Initialize))]
		public static class Db_Initialize_Patch
		{
			public static void Prefix()
			{
				AddTech(RadiumFuelRodConfig.Id, RadiumFuelRod.TechGroup);
			}

			private static void AddTech(string id, string techGroup)
			{
				var tech = new List<string>(Techs.TECH_GROUPING[techGroup]);
				tech.Add(id);
				Techs.TECH_GROUPING[techGroup] = tech.ToArray();
			}
		}

		[HarmonyPatch(typeof(Manager))]
		[HarmonyPatch(nameof(Manager.GetType))]
		[HarmonyPatch(new[] { typeof(string) })]
		public static class KSerializationManager_GetType_Patch
		{
			public static void Postfix(string type_name, ref Type __result)
			{
				if (type_name == typeof(RadiumFuelRod).AssemblyQualifiedName)
				{
					__result = typeof(RadiumFuelRod);
				}
			}
		}
	}

	[SerializationConfig(MemberSerialization.OptIn)]
	public class SuperHeater : StateMachineComponent<SuperHeater.StatesInstance>
	{
		public float targetTemperature = 15;
		public int radius = 2;
		[MyCmpReq]
		private Operational operational;

		public float TargetTemperature
		{
			get
			{
				return targetTemperature;
			}
		}

		protected override void OnSpawn()
		{
			base.OnSpawn();
			smi.StartSM();
		}

		private MonitorState MonitorHeating(float dt)
		{
			// always ready because fuel cell!
			return MonitorState.ReadyToHeat;
		}

		public class StatesInstance : GameStateMachine<States, StatesInstance, SuperHeater, object>.GameInstance
		{
			public StatesInstance(SuperHeater master)
			  : base(master)
			{
			}
		}

		public class States : GameStateMachine<States, StatesInstance, SuperHeater>
		{
			public State offline;
			public OnlineStates online;
			private StatusItem statusItemUnderMassLiquid;
			private StatusItem statusItemUnderMassGas;
			private StatusItem statusItemOverTemp;

			public override void InitializeStates(out BaseState default_state)
			{
				default_state = offline;
				serializable = false;
				statusItemUnderMassLiquid = new StatusItem("statusItemUnderMassLiquid", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_LIQUID.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 63486);
				statusItemUnderMassGas = new StatusItem("statusItemUnderMassGas", BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDLOWMASS_GAS.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 63486);
				statusItemOverTemp = new StatusItem("statusItemOverTemp", BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.NAME, BUILDING.STATUSITEMS.HEATINGSTALLEDHOTENV.TOOLTIP, string.Empty, StatusItem.IconType.Info, NotificationType.BadMinor, false, OverlayModes.None.ID, 63486);
				statusItemOverTemp.resolveStringCallback = (str, obj) =>
				{
					StatesInstance statesInstance = (StatesInstance)obj;
					return string.Format(str, GameUtil.GetFormattedTemperature(statesInstance.master.TargetTemperature, GameUtil.TimeSlice.None, GameUtil.TemperatureInterpretation.Absolute, true, false));
				};
				offline.EventTransition(GameHashes.OperationalChanged, online, smi => smi.master.operational.IsOperational);
				online.EventTransition(GameHashes.OperationalChanged, offline, smi => !smi.master.operational.IsOperational).DefaultState(online.heating).Update("SuperHeater_online", (smi, dt) =>
				{
					switch (smi.master.MonitorHeating(dt))
					{
						case MonitorState.ReadyToHeat:
							smi.GoTo(online.heating);
							break;
						case MonitorState.TooHot:
							smi.GoTo(online.overtemp);
							break;
						case MonitorState.NotEnoughLiquid:
							smi.GoTo(online.undermassliquid);
							break;
						case MonitorState.NotEnoughGas:
							smi.GoTo(online.undermassgas);
							break;
					}
				}, UpdateRate.SIM_4000ms, false);
				online.heating.Enter(smi => smi.master.operational.SetActive(true, false)).Exit(smi => smi.master.operational.SetActive(false, false));
				online.undermassliquid.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, statusItemUnderMassLiquid, null);
				online.undermassgas.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, statusItemUnderMassGas, null);
				online.overtemp.ToggleCategoryStatusItem(Db.Get().StatusItemCategories.Heat, statusItemOverTemp, null);
			}

			public class OnlineStates : State
			{
				public State heating;
				public State overtemp;
				public State undermassliquid;
				public State undermassgas;
			}
		}

		private enum MonitorState
		{
			ReadyToHeat,
			TooHot,
			NotEnoughLiquid,
			NotEnoughGas,
		}
	}

}
