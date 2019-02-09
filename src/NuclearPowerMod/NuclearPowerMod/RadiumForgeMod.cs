using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;
using UnityEngine;

[HarmonyPatch(typeof(SupermaterialRefineryConfig), "ConfigureBuildingTemplate", null)]
public static class RadiumForge
{
	public static void Postfix( )
	{
		float num1 = 0.01f;
		ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[2]
		{ 
	  new ComplexRecipe.RecipeElement(SimHashes.SolidMercury.CreateTag(), 50f),
	  new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 50f )
		};
		ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
		{
	  new ComplexRecipe.RecipeElement(SimHashes.Radium.CreateTag(), 100f)
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

		/*
		ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[3]
	{
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Steel).tag, 25f),
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Glass).tag, 25f),
		new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.SolidNaphtha).tag, 100f)
	};
		ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
		{
	  new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.LiquidPropane).tag, 100f)
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
		ComplexRecipeManager.Get().AddObsoleteIDMapping(obsolete_id, str);*/
	}
}