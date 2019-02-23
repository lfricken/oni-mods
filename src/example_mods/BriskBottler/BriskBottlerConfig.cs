using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class BriskBottlerConfig : IBuildingConfig
{
    public const string ID = "BRISKBOTTLER";

    public override BuildingDef CreateBuildingDef()
    {
        string id = "BRISKBOTTLER";
        int width = 2;
        int height = 3;
        string anim = "wash_sink_kanim";
        int hitpoints = 30;
        float construction_time = 30f;
        float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        string[] rawMetals = MATERIALS.RAW_METALS;
        float melting_point = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
        EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tieR0, 0.2f);
        buildingDef.InputConduitType = ConduitType.Liquid;
        buildingDef.ViewMode = SimViewMode.LiquidVentMap;
        buildingDef.AudioCategory = "Metal";
        buildingDef.UtilityInputOffset = new CellOffset(0, 0);
        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        Storage defaultStorage = BuildingTemplates.CreateDefaultStorage(go, false);
        defaultStorage.showDescriptor = true;
        defaultStorage.storageFilters = STORAGEFILTERS.LIQUIDS;
        defaultStorage.capacityKg = 2500f;
        defaultStorage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
        defaultStorage.allowItemRemoval = true;
        go.AddOrGet<DropAllWorkable>();
        GasBottler gasBottler = go.AddOrGet<GasBottler>();
        gasBottler.storage = defaultStorage;
        gasBottler.overrideAnims = new KAnimFile[1]
    {
      Assets.GetAnim((HashedString) "anim_interacts_washbasin_kanim")
    };
        gasBottler.workTime = 5f;
        ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.storage = defaultStorage;
        conduitConsumer.conduitType = ConduitType.Liquid;
        conduitConsumer.ignoreMinMassCheck = true;
        conduitConsumer.forceAlwaysSatisfied = true;
        conduitConsumer.alwaysConsume = true;
        conduitConsumer.capacityKG = defaultStorage.capacityKg;
        conduitConsumer.keepZeroMassObject = false;
        go.AddOrGet<LoopingSounds>();
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        BuildingTemplates.DoPostConfigure(go);
    }
}