using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using TUNING;

public class StressPackConfig : IEquipmentConfig
{

    public const string ID = "StressPack";
    public const string Recipe_Desc = "Gives a high level of Morale"; 
    public const int decorMod = 2;
    public const float CarryAmount = 20f;
    public float stressModificationValue;

    public EquipmentDef CreateEquipmentDef()
    {
        Dictionary<string, float> InputElementMassMap = new Dictionary<string, float>();
        InputElementMassMap.Add("GOLD", 2);
        InputElementMassMap.Add("DIAMOND", 1);
        ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo((string) ID, decorMod, 0f, 0f);
        List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>()
        {
         new AttributeModifier(Db.Get().Attributes.QualityOfLife.Id, (float) CarryAmount, (string) ID, false, false, true),
        };
        EquipmentDef equipmentDef1 = EquipmentTemplates.CreateEquipmentDef("StressPack", TUNING.EQUIPMENT.TOOLS.TOOLSLOT, TUNING.EQUIPMENT.VESTS.FABRICATOR, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_FABTIME, SimHashes.Carbon, InputElementMassMap, (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, "vacillator_charge_kanim", (string)null, "body_water_slow_kanim", 4, AttributeModifiers, (string)null, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, (Tag[])null);
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

        BackPackConfig.SetupVest(go);
    }
}