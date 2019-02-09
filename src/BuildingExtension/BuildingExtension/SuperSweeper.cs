using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;
using System.Reflection.Emit;

namespace BuildingExtension
{
	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class SuperTransferArm_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERTRANSFERARM.NAME", "Super-Sweeper");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERTRANSFERARM.DESC", "The Super-Sweeper transports solids over Long distances.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.SUPERTRANSFERARM.EFFECT", "Perfect to supply ladders or buildings that have much space inbetween.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Conveyance").Equals(po.category)).data;
			category.Add(SuperTransferArmConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(SuperTransferArmConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class SuperTransferArm_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["SolidTransport"]);
			ls.Add(SuperTransferArmConfig.ID);
			Database.Techs.TECH_GROUPING["SolidTransport"] = (string[])ls.ToArray();
		}
	}
	public class SuperTransferArmConfig : IBuildingConfig
	{
		public const string ID = "SuperTransferArm";
		private const int RANGE = 14;
		private static readonly LogicPorts.Port[] INPUT_PORTS;

		public override BuildingDef CreateBuildingDef()
		{
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("SuperTransferArm", 3, 1, "conveyor_transferarm_kanim", 10, 10f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3, new string[1] { SimHashes.Steel.ToString() }, 1600f, BuildLocationRule.Anywhere, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, TUNING.NOISE_POLLUTION.NOISY.TIER0, 0.2f);
			buildingDef.Floodable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.ExhaustKilowattsWhenActive = 0.0f;
			buildingDef.SelfHeatKilowattsWhenActive = 0.0f;
			buildingDef.PermittedRotations = PermittedRotations.R360;
			GeneratedBuildings.RegisterWithOverlay(OverlayScreen.SolidConveyorIDs, "SolidTransferArm");
			return buildingDef;
		}

		public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
		{
			go.AddOrGet<Operational>();
			go.AddOrGet<LoopingSounds>();
		}

		public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, SuperTransferArmConfig.INPUT_PORTS);
			SuperTransferArmConfig.AddVisualizer(go, true);
		}

		public override void DoPostConfigureUnderConstruction(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, SuperTransferArmConfig.INPUT_PORTS);
			SuperTransferArmConfig.AddVisualizer(go, false);
			Constructable component = go.GetComponent<Constructable>();
			component.choreTags = GameTags.ChoreTypes.ConveyorChores;
			component.requiredRolePerk = RoleManager.rolePerks.ConveyorBuild.id;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			GeneratedBuildings.RegisterLogicPorts(go, SuperTransferArmConfig.INPUT_PORTS);
			go.AddOrGet<LogicOperationalController>();
			go.AddOrGet<SolidTransferArm>().pickupRange = RANGE;
			SuperTransferArmConfig.AddVisualizer(go, false);
		}

		private static void AddVisualizer(GameObject prefab, bool movable)
		{
			StationaryChoreRangeVisualizer choreRangeVisualizer = prefab.AddOrGet<StationaryChoreRangeVisualizer>();
			choreRangeVisualizer.x = -RANGE;
			choreRangeVisualizer.y = -RANGE;
			choreRangeVisualizer.width = (RANGE * 2) + 1;
			choreRangeVisualizer.height = (RANGE * 2) + 1;
			choreRangeVisualizer.movable = movable;
		}

		static SuperTransferArmConfig()
		{
			SuperTransferArmConfig.INPUT_PORTS = new LogicPorts.Port[1]
			{
	  LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), "Logic Port", false)
			};
		}

		/*
		[HarmonyPatch(typeof(SolidTransferArm), "RefreshReachableCells")]
		internal class SolidTransferArmMod_RefreshReachableCells
		{

			private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
			{
				Debug.Log(" === SolidTransferArmMod_RefreshReachableCells Transpiler === ");
				bool start = false;
				List<CodeInstruction> code = instr.ToList();
				int counter = 0;
				foreach (CodeInstruction codeInstruction in code) {
					if (codeInstruction.opcode == OpCodes.Brfalse) {
						codeInstruction.opcode = OpCodes.Nop;
					}
					/*	start = true;
						continue;
					}
					if(start){	if (counter > 6) break;
						Debug.Log(" === Transpiler applied === ");
						//codeInstruction.opcode = OpCodes.Nop;
						//codeInstruction.operand =  "IL_005a";
						codeInstruction.opcode = OpCodes.Nop;
						counter++;
					}
					yield return codeInstruction;
				}
			}
		}*/
	}

}
