/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

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
		public const string Id = SupermaterialRefineryConfig.ID;

		[HarmonyPatch(typeof(SupermaterialRefineryConfig))]
		[HarmonyPatch(nameof(SupermaterialRefineryConfig.ConfigureBuildingTemplate))]
		public static class ConfigureBuildingTemplate_Patch
		{
			static void Postfix(GameObject go, Tag prefab_tag)
			{
				AddRadium();
			}
		}

		public static void AddRadium()
		{
			ComplexRecipe.RecipeElement[] ingredients = new ComplexRecipe.RecipeElement[3]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Niobium.CreateTag(), Radium.Recipe.Niobium),
				new ComplexRecipe.RecipeElement(SimHashes.Tungsten.CreateTag(), Radium.Recipe.Tungsten),
				new ComplexRecipe.RecipeElement(SimHashes.RefinedCarbon.CreateTag(), Radium.Recipe.RefinedCarbon),
			};

			ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Radium.CreateTag(), Radium.Recipe.AmountProduced)
			};

			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(Id, ingredients, results), ingredients, results)
			{
				time = Radium.Recipe.Time,
				description = Radium.Recipe.Description,
				useResultAsDescription = true
			}.fabricators = new List<Tag>()
			{
				TagManager.Create(Id)
			};
		}
	}
}
