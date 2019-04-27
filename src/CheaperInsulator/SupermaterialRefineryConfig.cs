/// <summary>
/// Created by lfricken https://github.com/lfricken/oni-mods
/// </summary>

using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace CheaperInsulator
{
	public class SupermaterialRefineryConfigPatches
	{
		public const string Id = SupermaterialRefineryConfig.ID;

		/// <summary>
		/// Replace <see cref="SupermaterialRefineryConfigPatches.ConfigureBuildingTemplate"/>
		/// </summary>
		[HarmonyPatch(typeof(SupermaterialRefineryConfig))]
		[HarmonyPatch(nameof(SupermaterialRefineryConfig.ConfigureBuildingTemplate))]
		public static class ConfigureBuildingTemplate_Patch
		{
			static void Postfix(GameObject go, Tag prefab_tag)
			{
				RemoveInsulator();
				AddInsulator();
			}
		}

		public static void AddInsulator()
		{
			// ratios
			float amountProduced = 1000f;
			float amount1 = 15f;
			float amount2 = 5f;
			float amount3 = amountProduced - amount1 - amount2;

			// inputs
			List<ComplexRecipe.RecipeElement> ingredients = new List<ComplexRecipe.RecipeElement>();
			ingredients.Add(new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), amount1));
			ingredients.Add(new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), amount2));
			ingredients.Add(new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), amount3));
			var inputs = ingredients.ToArray();

			// outputs
			ComplexRecipe.RecipeElement[] results = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement(SimHashes.SuperInsulator.CreateTag(), amountProduced)
			};

			// remove the original recipe
			string recipeId = ComplexRecipeManager.MakeRecipeID(Id, inputs, results);

			// setup description, add recipe
			new ComplexRecipe(recipeId, inputs, results)
			{
				time = 100f,
				description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERINSULATOR_RECIPE_DESCRIPTION,
				useResultAsDescription = true
			}.fabricators = new List<Tag>()
			{
				TagManager.Create(Id)
			};
		}

		public static void RemoveInsulator()
		{
			float num3 = 0.15f;
			float num4 = 0.05f;
			float num5 = 1f - num4 - num3;
			ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[3]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num3),
				new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 100f * num5),
				new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 100f * num4)
			};
			ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement(SimHashes.SuperInsulator.CreateTag(), 100f)
			};

			string recipeId = ComplexRecipeManager.MakeRecipeID(Id, ingredients2, results2);
			ComplexRecipe recipe = ComplexRecipeManager.Get().GetRecipe(recipeId);
			if (recipe != null)
				ComplexRecipeManager.Get().recipes.Remove(recipe);
			else
				Debug.Log(nameof(ComplexRecipeManager) + " failed to add the new recipe.");
		}
	}
}