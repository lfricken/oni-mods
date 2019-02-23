using TUNING;
using UnityEngine;

public class LIQUIDPERMEABLEMRMBRANEConfig : IBuildingConfig
{
    public const string ID = "LIQUIDPERMEABLEMRMBRANE";

    public override BuildingDef CreateBuildingDef()
    {
        string id = "LIQUIDPERMEABLEMRMBRANE";
        int width = 1;
        int height = 1;
        string anim = "floor_gasperm_kanim";
        int hitpoints = 100;
        float construction_time = 30f;
        float[] tieR2 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
        string[] allMetals = MATERIALS.ALL_METALS;
        float melting_point = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.Tile;
        EffectorValues none = NOISE_POLLUTION.NONE;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR2, allMetals, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
        buildingDef.Floodable = false;
        buildingDef.Entombable = false;
        buildingDef.Overheatable = false;
        buildingDef.IsFoundation = true;
        buildingDef.TileLayer = ObjectLayer.FoundationTile;
        buildingDef.ReplacementLayer = ObjectLayer.ReplacementTile;
        buildingDef.AudioCategory = "Metal";
        buildingDef.AudioSize = "small";
        buildingDef.BaseTimeUntilRepair = -1f;
        buildingDef.SceneLayer = Grid.SceneLayer.TileMain;
        buildingDef.isKAnimTile = true;
        buildingDef.BlockTileAtlas = Assets.GetTextureAtlas("tiles_gasmembrane");
        buildingDef.BlockTilePlaceAtlas = Assets.GetTextureAtlas("tiles_gasmembrane_place");
        buildingDef.BlockTileMaterial = Assets.GetMaterial("tiles_solid");
        buildingDef.DecorBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_info");
        buildingDef.DecorPlaceBlockTileInfo = Assets.GetBlockTileDecorInfo("tiles_mesh_tops_decor_place_info");
        buildingDef.ConstructionOffsetFilter = new CellOffset[1]
    {
      new CellOffset(0, -1)
    };
        return buildingDef;
    }
    
    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        GeneratedBuildings.MakeBuildingAlwaysOperational(go);
        BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
        SimCellOccupier simCellOccupier = go.AddOrGet<SimCellOccupier>();
        simCellOccupier.setGasImpermeable = true;
        simCellOccupier.setLiquidImpermeable = false;
        simCellOccupier.doReplaceElement = false;
        go.AddOrGet<KAnimGridTileVisualizer>().blockTileConnectorID = MeshTileConfig.BlockTileConnectorID;
        go.AddOrGet<BuildingHP>().destroyOnDamaged = true;
        go.AddComponent<SimTemperatureTransfer>();
    }
    
    public override void DoPostConfigureComplete(GameObject go)
    {
        BuildingTemplates.DoPostConfigure(go);
    }

    public override void DoPostConfigureUnderConstruction(GameObject go)
    {
        base.DoPostConfigureUnderConstruction(go);
        go.AddOrGet<KAnimGridTileVisualizer>();
    }
}