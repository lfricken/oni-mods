using STRINGS;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

public class BriskDupeFabConfig : IBuildingConfig
{
    public const string ID = "DUPEFABRICATOR";

        public override BuildingDef CreateBuildingDef()
    {
        string id = "DUPEFABRICATOR";
        int width = 3;
        int height = 4;
       string anim = "oxylite_refinery_kanim";
        int hitpoints = 100;
        float construction_time = 100f;
        float[] tieR4 = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
        string[] rawMetals = MATERIALS.RAW_METALS;
        float melting_point = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
        EffectorValues tieR0 = NOISE_POLLUTION.NOISY.TIER0;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR4, rawMetals, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.BONUS.TIER1, tieR0, 0.2f);
        buildingDef.ViewMode = SimViewMode.LiquidVentMap;
        buildingDef.AudioCategory = "Metal";
        buildingDef.UtilityInputOffset = new CellOffset(0, 0);
        return buildingDef;
    }

  public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
  {
    go.AddOrGet<DropAllWorkable>();
    Prioritizable.AddRef(go);
    go.AddOrGet<BuildingComplete>().isManuallyOperated = false;
    Fabricator fabricator = go.AddOrGet<Fabricator>();
    BuildingTemplates.CreateFabricatorStorage(go, fabricator);



 }


    public override void DoPostConfigureComplete(GameObject go)
    {
        BuildingTemplates.DoPostConfigure(go);
        
    new Recipe(SimHashes.Water.ToString(), 2500f, (SimHashes)0, "Water", "Gives water in turn of life... Dupe life", 1).SetFabricator("DUPEFABRICATOR", 5f).AddIngredient(new Recipe.Ingredient(GameTags.Minion.ToString(), 1f));
    new Recipe("FieldRation", 30f, (SimHashes)0, "Food", "Not Dupes", 1).SetFabricator("DUPEFABRICATOR", 5f).AddIngredient(new Recipe.Ingredient(GameTags.Minion.ToString(), 1f));
    new Recipe(SimHashes.Carbon.ToString(), 2000f, (SimHashes)0, "Carbon", "Turns Dupes turned to Carbon", 1).SetFabricator("DUPEFABRICATOR", 5f).AddIngredient(new Recipe.Ingredient(GameTags.Minion.ToString(), 1f));
    }
}