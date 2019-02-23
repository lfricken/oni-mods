using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using TUNING;

public class ReBreatherConfig : IEquipmentConfig
{

    public const string ID = "ReBreather";
    public const string Recipe_Desc = "Expands the breath of the Dupe"; 
    public const int decorMod = 0;
    public const float CarryAmount = 100f;

    public EquipmentDef CreateEquipmentDef()
    {
        Dictionary<string, float> InputElementMassMap = new Dictionary<string, float>();
        InputElementMassMap.Add("Puft", 1);
        List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>()
        {
         new AttributeModifier(Db.Get().Amounts.Breath.maxAttribute.Id, (float) CarryAmount, (string) BackPackConfig.ID, false, false, true),
        };
        ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo((string) ID, decorMod, 0f, 0f);
        EquipmentDef equipmentDef1 = EquipmentTemplates.CreateEquipmentDef("ReBreather", TUNING.EQUIPMENT.TOOLS.TOOLSLOT, TUNING.EQUIPMENT.VESTS.FABRICATOR, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_FABTIME, SimHashes.Carbon, InputElementMassMap, (float)TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, "rotfood_kanim", (string)null, "rotfood_kanim", 4, AttributeModifiers, (string)null, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, (Tag[])null);
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