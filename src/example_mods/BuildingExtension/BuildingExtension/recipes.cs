using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using TUNING;
using UnityEngine;
/*todo 
void engine
Super cooler
*/
namespace BuildingExtension
{/*
	[HarmonyPatch(typeof(WaterPurifierConfig), "ConfigureBuildingTemplate", null)]
	public static class WaterDirtier
	{
		public static void Postfix(ref GameObject go)
		{
			ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
			elementConverter.consumedElements = new ElementConverter.ConsumedElement[2]
		{
	  new ElementConverter.ConsumedElement(new Tag("Filter"), 1f),
	  new ElementConverter.ConsumedElement(new Tag("Water"), 5f)
		};
			elementConverter.outputElements = new ElementConverter.OutputElement[2]
			{
	  new ElementConverter.OutputElement(5f, SimHashes.DirtyWater, 313.15f, true, 0.0f, 0.5f, false, 0.75f, byte.MaxValue, 0),
	  new ElementConverter.OutputElement(1f, SimHashes.ToxicSand, 313.15f, true, 0.0f, 0.5f, false, 0.25f, byte.MaxValue, 0)
			};
			ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
			conduitDispenser.conduitType = ConduitType.Liquid;
			conduitDispenser.invertElementFilter = true;
			conduitDispenser.elementFilter = new SimHashes[2]
			{
	  SimHashes.DirtyWater,
	  SimHashes.Water
			};
		}
	}
	*/
 //[HarmonyPatch(typeof(WaterPurifierConfig), "ConfigureBuildingTemplate", null)]




	[HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate", null)]
	public static class SupermaterialRefineryConfigrecipes
	{
		public static void Postfix()
		{
			float num1 = 0.01f;
			ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[2]
			{
	  new ComplexRecipe.RecipeElement(SimHashes.SlimeMold.CreateTag(), 100f),
	  new ComplexRecipe.RecipeElement(SimHashes.BleachStone.CreateTag(), 1f )
			};
			ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
			{
	  new ComplexRecipe.RecipeElement(SimHashes.Dirt.CreateTag(), 100f)
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
	[HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate", null)]
	public static class SupermaterialRefineryConfigrecipes2
	{
		public static void Postfix()
		{
			float num1 = 0.01f;
			ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[1]
			{
	  new ComplexRecipe.RecipeElement(SimHashes.Steel.CreateTag(), 100f )
			};
			ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
			{
	  new ComplexRecipe.RecipeElement(SimHashes.Lime.CreateTag(), 20f)
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
	[HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate", null)]
	public static class SupermaterialRefineryConfigrecipes3
	{
		public static void Postfix()
		{
			ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[3]
			{
	  new ComplexRecipe.RecipeElement(SimHashes.MaficRock.CreateTag(), 70f ),
	  new ComplexRecipe.RecipeElement(SimHashes.Sulfur.CreateTag(), 10f ),
	  new ComplexRecipe.RecipeElement(SimHashes.IgneousRock.CreateTag(), 20f )

			};
			ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[2]
			{
	  new ComplexRecipe.RecipeElement(SimHashes.Wolframite.CreateTag(), 50f),
	  new ComplexRecipe.RecipeElement(SimHashes.Sand.CreateTag(), 50f)
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
}