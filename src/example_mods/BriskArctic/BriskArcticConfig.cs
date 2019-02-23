using System.Collections.Generic;
using TUNING;
using STRINGS;
using UnityEngine;

namespace BriskArctic
{
	public class BriskArcticConfig : IBuildingConfig
	{
        private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
         {
          LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(1, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, false)
         };
        public const string ID = "ARCTIC";

		public override BuildingDef CreateBuildingDef()
		{
            string id = "ARCTIC";
            int width = 2;
            int height = 2;
            string anim = "spaceheater_kanim";
            int hitpoints = 10;
            float construction_time = 50f;
            float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
            string[] allMetals = MATERIALS.ALL_METALS;
            float melting_point = 2400f;
            BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
            EffectorValues tieR2 = TUNING.NOISE_POLLUTION.NOISY.TIER2;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, allMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tieR2, 0.2f);
            buildingDef.RequiresPowerInput = true;
            buildingDef.EnergyConsumptionWhenActive = 10f;
            buildingDef.ExhaustKilowattsWhenActive = -16f;
            buildingDef.SelfHeatKilowattsWhenActive = -50f;
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.ViewMode = SimViewMode.PowerMap;
            buildingDef.InputConduitType = ConduitType.Gas;
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            return buildingDef;
		}

        public override void DoPostConfigureComplete(GameObject go)
        {
            go.AddOrGet<Storage>().capacityKg = 0.09999999f;
            go.AddOrGet<MassiveHeatSink>();
            go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 120f;
            PrimaryElement component = go.GetComponent<PrimaryElement>();
            component.SetElement(SimHashes.Iron);
            component.Temperature = 294.15f;
            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.conduitType = ConduitType.Gas;
            conduitConsumer.consumptionRate = 1f;
            conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Hydrogen);
            conduitConsumer.capacityKG = 0.09999999f;
            conduitConsumer.forceAlwaysSatisfied = true;
            conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
            go.AddOrGet<ElementConverter>().consumedElements = new ElementConverter.ConsumedElement[1]
             {
               new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Hydrogen).tag, 0.01f)
             };
            ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
            elementConverter.outputElements = new ElementConverter.OutputElement[] { };
            GeneratedBuildings.RegisterLogicPorts(go, BriskArcticConfig.INPUT_PORTS);
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGetDef<PoweredActiveController.Def>();

        }
              public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
         {
          GeneratedBuildings.RegisterLogicPorts(go, BriskArcticConfig.INPUT_PORTS);
        }

          public override void DoPostConfigureUnderConstruction(GameObject go)
         {
           GeneratedBuildings.RegisterLogicPorts(go, BriskArcticConfig.INPUT_PORTS);
         }


      }
	}

