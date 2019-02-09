using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;

[HarmonyPatch(typeof(GlassForgeConfig), "ConfigureBuildingTemplate", null)]
public static class MercuryMod
{
	public static void Postfix()
	{

		ElementLoader.FindElementByHash(SimHashes.SolidMercury).highTemp = 1000f;
		ElementLoader.FindElementByHash(SimHashes.Mercury).lowTemp = 50f;
		ElementLoader.FindElementByHash(SimHashes.Mercury).highTemp = 1200f;
		ElementLoader.FindElementByHash(SimHashes.Granite).highTempTransitionTarget = SimHashes.Mercury;
		ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[3]
	{
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Tungsten).tag, 50f),
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.MaficRock).tag, 50f),
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Obsidian).tag, 100f)
	};
		ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
		{
	  new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Mercury).tag, 200f)
		};
		string obsolete_id = ComplexRecipeManager.MakeObsoleteRecipeID("GlassForge", ingredients[0].material);
		string str = ComplexRecipeManager.MakeRecipeID("GlassForge", (IList<ComplexRecipe.RecipeElement>)ingredients, (IList<ComplexRecipe.RecipeElement>)results);
		new ComplexRecipe(str, ingredients, results)
		{
			time = 40f,
			useResultAsDescription = true,
			description = string.Format((string)STRINGS.BUILDINGS.PREFABS.GLASSFORGE.RECIPE_DESCRIPTION, (object)ElementLoader.GetElement(results[0].material).name, (object)ElementLoader.GetElement(ingredients[0].material).name)

		}.fabricators = new List<Tag>()
	{
	  TagManager.Create("GlassForge")
	};
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);
	}

	[HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate", null)]
	public static class SupermaterialRefineryConfigrecipes
	{
		public static void Postfix()
		{
			ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[4]
			{
				new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Tungsten).tag, 50f),
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.MaficRock).tag, 50f),
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Obsidian).tag, 100f),
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.SuperCoolant).tag, 50f),
	};
			ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
			{
	  new ComplexRecipe.RecipeElement(SimHashes.SolidMercury.CreateTag(), 100f)
			};
			ComplexRecipe complexRecipe3 = new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients2, (IList<ComplexRecipe.RecipeElement>)results2), ingredients2, results2);
			complexRecipe3.time = 80f;
			complexRecipe3.description = (string)STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERCOOLANT_RECIPE_DESCRIPTION;
			complexRecipe3.useResultAsDescription = true;
			ComplexRecipe complexRecipe4 = complexRecipe3;
			List<Tag> tagList3 = new List<Tag>();
			tagList3.Add(TagManager.Create("SupermaterialRefinery"));
			List<Tag> tagList4 = tagList3;
			complexRecipe4.fabricators = tagList4;
		}
	}

	

	[HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
	internal class MercuryHeatSinkConfig_GeneratedBuildings_LoadGeneratedBuildings
	{
		private static void Prefix()
		{
			Debug.Log(" === GeneratedBuildings Prefix === " + MercuryHeatSinkConfig.ID);
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MERCURYHEATSINK.NAME", "Mercury Cooler");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MERCURYHEATSINK.DESC", "The Mercury Cooler uses mercury for massive cooling.");
			Strings.Add("STRINGS.BUILDINGS.PREFABS.MERCURYHEATSINK.EFFECT", "The Mercury Cooler is very useful for industrial cooling.");

			//List<string> ls = new List<string>((string[])TUNING.BUILDINGS.PLANORDER[10].data);
			//ls.Add(RadiumFuelConfig.ID);


			List<string> category = (List<string>)TUNING.BUILDINGS.PLANORDER.First(po => ((HashedString)"Utilities").Equals(po.category)).data;
			category.Add(MercuryHeatSinkConfig.ID);

			//TUNING.BUILDINGS.PLANORDER[10].data = (string[])ls.ToArray();

			TUNING.BUILDINGS.COMPONENT_DESCRIPTION_ORDER.Add(typeof(MercuryHeatSinkConfig));
		}
	}
	[HarmonyPatch(typeof(Db), "Initialize")]
	internal class MercuryHeatSinkConfig_Db_Initialize
	{
		private static void Prefix(Db __instance)
		{
			List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["LiquidTemperature"]);
			ls.Add(MercuryHeatSinkConfig.ID);
			Database.Techs.TECH_GROUPING["LiquidTemperature"] = (string[])ls.ToArray();
		}
	}
	public class MercuryHeatSinkConfig : IBuildingConfig
	{
		public const string ID = "MercuryHeatSink";
		private const float CONSUMPTION_RATE = 1f;
		private const float STORAGE_CAPACITY = 10.09999999f;
		/*
		public MercuryHeatSinkConfig()
		{
			//base.\u002Ector();
		}*/

		public override BuildingDef CreateBuildingDef()
		{
			string id = "MercuryHeatSink";
			int width = 4;
			int height = 4;
			string anim = "massiveheatsink_kanim";
			int hitpoints = 100;
			float construction_time = 120f;
			float[] tieR5_1 = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
			string[] rawMetals = MATERIALS.RAW_METALS;
			float melting_point = 2400f;
			BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
			EffectorValues tieR5_2 = NOISE_POLLUTION.NOISY.TIER5;
			BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tieR5_1, rawMetals, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER2, tieR5_2, 0.2f);
			buildingDef.RequiresPowerInput = true;
			buildingDef.EnergyConsumptionWhenActive = 1960f;
			buildingDef.ExhaustKilowattsWhenActive = -160f;
			buildingDef.SelfHeatKilowattsWhenActive = -640f;
			buildingDef.Floodable = false;
			buildingDef.Entombable = false;
			buildingDef.AudioCategory = "Metal";
			buildingDef.PowerInputOffset = new CellOffset(2, 0);
			buildingDef.UtilityInputOffset = new CellOffset(0, 0);
			buildingDef.InputConduitType = ConduitType.Liquid;
			buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
			buildingDef.MaterialCategory = new string[] { "RefinedMetal" };
			buildingDef.Mass = new float[] { 2000f };
			return buildingDef;
		}

		public override void DoPostConfigureComplete(GameObject go)
		{
			go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery);
			go.AddOrGet<MassiveHeatSink>();
			go.AddOrGet<MinimumOperatingTemperature>().minimumTemperature = 1f;
			PrimaryElement component = go.GetComponent<PrimaryElement>();
			component.SetElement(SimHashes.Iron);
			component.Temperature = 294.15f;
			go.AddOrGet<LoopingSounds>();
			go.AddOrGet<Storage>().capacityKg = 9.99999999f;
			ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
			conduitConsumer.conduitType = ConduitType.Liquid;
			conduitConsumer.consumptionRate = 10f;
			conduitConsumer.capacityTag = GameTagExtensions.Create(SimHashes.Mercury);
			conduitConsumer.capacityKG = 9.99999999f;
			conduitConsumer.forceAlwaysSatisfied = true;
			conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
			go.AddOrGet<ElementConverter>().consumedElements = new ElementConverter.ConsumedElement[1]
			{
	  new ElementConverter.ConsumedElement(ElementLoader.FindElementByHash(SimHashes.Mercury).tag, 1.0f)
			};
			go.AddOrGetDef<PoweredActiveController.Def>();
		}
	}

}