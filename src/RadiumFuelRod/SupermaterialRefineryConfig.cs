using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace RadiumFuelRod
{
	/// <summary>
	/// Add Radium to the Super refinery.
	/// </summary>
	public class SupermaterialRefineryConfigPatches
	{
		public const string ID = "SupermaterialRefinery";

		[HarmonyPatch(typeof(SupermaterialRefineryConfig))]
		[HarmonyPatch(nameof(SupermaterialRefineryConfig.ConfigureBuildingTemplate))]
		public static class ConfigureBuildingTemplate_Patch
		{
			static void Postfix(GameObject go, Tag prefab_tag)
			{
				// add recipe
				Radium();
			}
		}

		public static void Radium()
		{
			ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[3]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Niobium.CreateTag(), 5f),
				new ComplexRecipe.RecipeElement(SimHashes.Tungsten.CreateTag(), 95f),
				new ComplexRecipe.RecipeElement(SimHashes.RefinedCarbon.CreateTag(), 300f),
			};

			ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Radium.CreateTag(), 400f)
			};

			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", ingredients, results), ingredients, results)
			{
				time = 80f,
				description = (string)STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION,
				useResultAsDescription = true
			}.fabricators = new List<Tag>()
			{
				TagManager.Create("SupermaterialRefinery")
			};
		}
	}
}
