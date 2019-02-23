using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using TUNING;

public class OnePackConfig : IEquipmentConfig
{

    public const string ID = "OnePack";
    public const string Recipe_Desc = "This is an all in one pack"; 
    public const int decorMod = 5;
    public const float LifeAmount = 15f;
    public const float CarryAmount = 800f;
    public const float SpeedAmount = 10f;
    public const float BreathAmount = 80f;
    public float stressModificationValue;

    public EquipmentDef CreateEquipmentDef()
    {
        Dictionary<string, float> InputElementMassMap = new Dictionary<string, float>();
        InputElementMassMap.Add(LightPackConfig.ID, 1);
        InputElementMassMap.Add(BackPackConfig.ID, 1);
        InputElementMassMap.Add(ReBreatherConfig.ID, 1);
        InputElementMassMap.Add(SlicksterShoesConfig.ID, 1);
        InputElementMassMap.Add(StressPackConfig.ID, 1);
        InputElementMassMap.Add("BasicFabric", 1);
        ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo((string) ID, decorMod, 0f, 0f);
        List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>()
        {
         new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, (float) CarryAmount, (string) ID, false, false, true),
         new AttributeModifier(Db.Get().Attributes.CarryAmount.Id, (float) CarryAmount, (string) ID, true, false, true),
         new AttributeModifier(Db.Get().Attributes.Athletics.Id, (float) CarryAmount, (string) ID, false, false, true),
         new AttributeModifier(Db.Get().Amounts.Breath.maxAttribute.Id, (float) CarryAmount, (string) ID, false, false, true),
        };
        EquipmentDef equipmentDef1 = EquipmentTemplates.CreateEquipmentDef("OnePack", TUNING.EQUIPMENT.TOOLS.TOOLSLOT, TUNING.EQUIPMENT.VESTS.FABRICATOR, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_FABTIME, SimHashes.Carbon, InputElementMassMap, (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, "vacillator_charge_kanim", (string)null, "body_water_slow_kanim", 4, AttributeModifiers, (string)null, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, (Tag[])null);
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
        BackPackConfig.SetupVest(go);
    }
}