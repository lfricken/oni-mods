
using Klei.AI;
using STRINGS;
using System.Collections.Generic;
using UnityEngine;
using TUNING;


public class RubberBootsConfig : IEquipmentConfig
{

  public const string ID = "RUBBERBOOTS";
  public const int decorMod = 10;

  public EquipmentDef CreateEquipmentDef()
  {
    Dictionary<string, float> InputElementMassMap = new Dictionary<string, float>();
    InputElementMassMap.Add("BasicFabric", 1);
    InputElementMassMap.Add("POLYPROPYLENE", 2);
    ClothingWearer.ClothingInfo clothingInfo = new ClothingWearer.ClothingInfo((string)RubberBootsConfig.ID, decorMod, 1f / 400f, -1.25f);
    List<AttributeModifier> AttributeModifiers = new List<AttributeModifier>();
    EquipmentDef equipmentDef1 = EquipmentTemplates.CreateEquipmentDef("RUBBERBOOTS", TUNING.EQUIPMENT.CLOTHING.SLOT, TUNING.EQUIPMENT.VESTS.FABRICATOR, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_FABTIME, SimHashes.Carbon, InputElementMassMap, (float) TUNING.EQUIPMENT.VESTS.FUNKY_VEST_MASS, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_ICON0, TUNING.EQUIPMENT.VESTS.SNAPON0, TUNING.EQUIPMENT.VESTS.FUNKY_VEST_ANIM0, 4, AttributeModifiers, TUNING.EQUIPMENT.VESTS.SNAPON1, true, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 0.4f, (Tag[]) null);
    Descriptor descriptor1 = new Descriptor(string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, (object) GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)), string.Format("{0}: {1}", (object) DUPLICANTS.ATTRIBUTES.THERMALCONDUCTIVITYBARRIER.NAME, (object) GameUtil.GetFormattedDistance(ClothingWearer.ClothingInfo.FANCY_CLOTHING.conductivityMod)), Descriptor.DescriptorType.Effect, false);
    Descriptor descriptor2 = new Descriptor(string.Format("{0}: {1}", (object)DUPLICANTS.ATTRIBUTES.DECOR.NAME, (object)RubberBootsConfig.decorMod), string.Format("{0}: {1}", (object)DUPLICANTS.ATTRIBUTES.DECOR.NAME, (object)RubberBootsConfig.decorMod), Descriptor.DescriptorType.Effect, false);
    equipmentDef1.additionalDescriptors.Add(descriptor1);
    equipmentDef1.additionalDescriptors.Add(descriptor2);
    equipmentDef1.EffectImmunites.Add(Db.Get().effects.Get("SoakingWet"));
    equipmentDef1.EffectImmunites.Add(Db.Get().effects.Get("WetFeet"));
    equipmentDef1.OnEquipCallBack = (System.Action<Equippable>) (eq => CoolVestConfig.OnEquipVest(eq, clothingInfo));
    equipmentDef1.RecipeDescription = (string) STRINGS.EQUIPMENT.PREFABS.FUNKY_VEST.RECIPE_DESC;
    return equipmentDef1;
  }

  public static void SetupVest(GameObject go)
  {
    go.GetComponent<KPrefabID>().AddTag(GameTags.Clothes);
    Equippable equippable = go.GetComponent<Equippable>();
    if ((Object) equippable == (Object) null)
      equippable = go.AddComponent<Equippable>();
    equippable.SetQuality(QualityLevel.Poor);
    go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
  }

  public void DoPostConfigure(GameObject go)
  {
    FunkyVestConfig.SetupVest(go);
  }
}