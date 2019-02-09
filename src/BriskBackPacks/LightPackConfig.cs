using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using TUNING;

public class LightPackConfig : IEquipmentConfig
{

    public const string ID = "LightPack";
    public const string Recipe_Desc = "Produces Light for a cost"; 
    public const int decorMod = 5;

    public float stressModificationValue;

    public EquipmentDef CreateEquipmentDef()
    {
        Dictionary<string, float> InputElementMassMap = new Dictionary<string, float>();
        InputElementMassMap.Add("GLASS", 2);
        InputElementMassMap.Add("LIGHTBUG", 1);
        ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo((string) ID, decorMod, 0f, 0f);
        List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
        EquipmentDef equipmentDef1 = EquipmentTemplates.CreateEquipmentDef("LightPack", TUNING.EQUIPMENT.TOOLS.TOOLSLOT, TUNING.EQUIPMENT.VESTS.FABRICATOR, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_FABTIME, SimHashes.Carbon, InputElementMassMap, (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, "vacillator_charge_kanim", (string)null, "vacillator_charge_kanim", 4, AttributeModifiers, (string)null, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, (Tag[])null);
        equipmentDef1.OnEquipCallBack = (System.Action<Equippable>)(eq => CoolVestConfig.OnEquipVest(eq, clothingInfo));
        equipmentDef1.RecipeDescription = Recipe_Desc;
        return equipmentDef1;
    }

    public static void SetupVest(GameObject go)
    {
        go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes);
        Equippable equippable = go.GetComponent<Equippable>();
        if ((Object)equippable == (Object)null)
            equippable = go.AddComponent<Equippable>();
        equippable.SetQuality(QualityLevel.Poor);
        go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;

    }

    public void DoPostConfigure(GameObject go)
    {
        Light2D light2D = go.AddOrGet<Light2D>();
        light2D.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
        light2D.Color = new Color(0.5f, 0.5f, 0);
        light2D.Range = 4f;
        light2D.Angle = 0f;
        light2D.Direction = LIGHT2D.FLOORLAMP_DIRECTION;
        light2D.Offset = new Vector2(0f, 0.5f);
        light2D.shape = LightShape.Circle;
        light2D.drawOverlay = true;
        light2D.Lux = 1000;
        LightPackConfig.SetupVest(go);
    }
}