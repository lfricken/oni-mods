using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class BriskAirWickConfig : IBuildingConfig   
{

    private static readonly LogicPorts.Port[] INPUT_PORTS = new LogicPorts.Port[1]
      {
       LogicPorts.Port.InputPort(LogicOperationalController.PORT_ID, new CellOffset(0, 0), UI.LOGIC_PORTS.CONTROL_OPERATIONAL, false)
      };
    public const string ID = "BRISKAIRWICK";

    public override BuildingDef CreateBuildingDef()
    {
        string id = "BRISKAIRWICK";
        int width = 1;
        int height = 2;
        string anim = "lava_lamp_kanim";
        int hitpoints = 30;
        float construction_time = 30f;
        float[] tieR2 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
        string[] rawMetals = MATERIALS.RAW_METALS;
        float melting_point = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
        EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, rawMetals, melting_point, build_location_rule, new EffectorValues { amount = 2, radius = 5 }, tieR0, 0.2f);
        buildingDef.InputConduitType = ConduitType.Liquid;
        buildingDef.RequiresPowerInput = true;
        buildingDef.EnergyConsumptionWhenActive = 2f;
        buildingDef.ViewMode = SimViewMode.LiquidVentMap;
        buildingDef.AudioCategory = "Metal";
        buildingDef.UtilityInputOffset = new CellOffset(0, 0);
        buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration);
        Storage storage1 = go.AddOrGet<Storage>();
        storage1.showInUI = true;

        ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType = ConduitType.Liquid;
        conduitConsumer.consumptionRate = 1.5f;
        conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.DirtyWater);
        conduitConsumer.capacityKG = 10f;
        conduitConsumer.forceAlwaysSatisfied = true;
        conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

        ManualDeliveryKG manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
        manualDeliveryKg1.SetStorage(storage1);
        manualDeliveryKg1.requestedItemTag = "BasicFabric";
        manualDeliveryKg1.capacity = 2f;
        manualDeliveryKg1.refillMass = 1f;
        manualDeliveryKg1.minimumMass = 1f;
        manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;
        go.AddOrGet<WaterPurifier>();

        ElementConverter elementConverter = go.AddComponent<ElementConverter>();
        elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
			{
				new ElementConverter.ConsumedElement("BasicFabric", 0.000375f),
				new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.02f)
			};
        elementConverter.outputElements = new ElementConverter.OutputElement[1]
			{
				new ElementConverter.OutputElement(0.02f, SimHashes.ContaminatedOxygen, 303.15f, false, 0.0f, 1f, true),
			};

        Prioritizable.AddRef(go);
    }

        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
         {
          GeneratedBuildings.RegisterLogicPorts(go, BriskAirWickConfig.INPUT_PORTS);
        }

          public override void DoPostConfigureUnderConstruction(GameObject go)
         {
           GeneratedBuildings.RegisterLogicPorts(go, BriskAirWickConfig.INPUT_PORTS);
         }

    public override void DoPostConfigureComplete(GameObject go)
    {
            GeneratedBuildings.RegisterLogicPorts(go, BriskAirWickConfig.INPUT_PORTS);
            go.AddOrGet<LogicOperationalController>();
            go.AddOrGetDef<PoweredActiveController.Def>();
            BuildingTemplates.DoPostConfigure(go);

    }
}