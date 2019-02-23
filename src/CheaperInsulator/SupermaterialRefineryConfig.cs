using Harmony;
using System.Collections.Generic;
using UnityEngine;

namespace CheaperInsulator
{
	public class SupermaterialRefineryConfigPatches
	{
		public const string ID = "SupermaterialRefinery";

		/// <summary>
		/// Replace <see cref="SupermaterialRefineryConfigPatches.ConfigureBuildingTemplate"/>
		/// </summary>
		[HarmonyPatch(typeof(SupermaterialRefineryConfig))]
		[HarmonyPatch(nameof(SupermaterialRefineryConfig.ConfigureBuildingTemplate))]
		public static partial class ConfigureBuildingTemplate_Patch
		{
			static bool Prefix(GameObject go, Tag prefab_tag)
			{
				// add components, setup basic stuff
				Setup(go, prefab_tag);

				// add recipes
				Insulator();
				SuperCoolant();
				ViscoGel();
				Thermium();

				// return false to skip original method
				return false;
			}
		}

		static void Setup(GameObject go, Tag prefab_tag)
		{
			// components
			go.AddOrGet<DropAllWorkable>();
			go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
			go.AddOrGet<FabricatorIngredientStatusManager>();
			go.AddOrGet<CopyBuildingSettings>();

			ComplexFabricator fabricator = go.AddOrGet<ComplexFabricator>();
			fabricator.sideScreenStyle = ComplexFabricatorSideScreen.StyleSetting.ListQueueHybrid;
			fabricator.duplicantOperated = true;

			// storage
			BuildingTemplates.CreateComplexFabricatorStorage(go, fabricator);

			// animation
			ComplexFabricatorWorkable fabricatorWorkable = go.AddOrGet<ComplexFabricatorWorkable>();
			fabricatorWorkable.overrideAnims = new KAnimFile[1]
			{
					Assets.GetAnim((HashedString) "anim_interacts_rockrefinery_kanim")
			};

			// priority
			Prioritizable.AddRef(go);
		}

		public static void Insulator()
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

			// setup description, add recipe
			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", inputs, results), inputs, results)
			{
				time = 100f,
				description = STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERINSULATOR_RECIPE_DESCRIPTION,
				useResultAsDescription = true
			}.fabricators = new List<Tag>()
			{
				TagManager.Create("SupermaterialRefinery")
			};
		}

		public static void SuperCoolant()
		{
			float num1 = 0.01f;
			float num2 = (float)((1.0 - (double)num1) * 0.5);
			ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[3]
			{
				new ComplexRecipe.RecipeElement(SimHashes.Fullerene.CreateTag(), 100f * num1),
				new ComplexRecipe.RecipeElement(SimHashes.Gold.CreateTag(), 100f * num2),
				new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), 100f * num2)
			};
			ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1]
			{
				new ComplexRecipe.RecipeElement(SimHashes.SuperCoolant.CreateTag(), 100f)
			};
			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients1, (IList<ComplexRecipe.RecipeElement>)results1), ingredients1, results1)
			{
				time = 80f,
				description = (string)STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.SUPERCOOLANT_RECIPE_DESCRIPTION,
				useResultAsDescription = true
			}.fabricators = new List<Tag>()
			{
				TagManager.Create("SupermaterialRefinery")
			};
		}

		public static void ViscoGel()
		{
			float num7 = 0.35f;
			ComplexRecipe.RecipeElement[] ingredients4 = new ComplexRecipe.RecipeElement[2]
			{
			new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num7),
			new ComplexRecipe.RecipeElement(SimHashes.Petroleum.CreateTag(), (float) (100.0 * (1.0 - (double) num7)))
			};
			ComplexRecipe.RecipeElement[] results4 = new ComplexRecipe.RecipeElement[1]
			{
			new ComplexRecipe.RecipeElement(SimHashes.ViscoGel.CreateTag(), 100f)
			};
			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients4, (IList<ComplexRecipe.RecipeElement>)results4), ingredients4, results4)
			{
				time = 80f,
				description = (string)STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.VISCOGEL_RECIPE_DESCRIPTION,
				useResultAsDescription = true
			}.fabricators = new List<Tag>()
			{
			TagManager.Create("SupermaterialRefinery")
			};
		}


		public static void Thermium()
		{
			float num6 = 0.05f;
			ComplexRecipe.RecipeElement[] ingredients3 = new ComplexRecipe.RecipeElement[2]
			{
			new ComplexRecipe.RecipeElement(SimHashes.Niobium.CreateTag(), 100f * num6),
			new ComplexRecipe.RecipeElement(SimHashes.Tungsten.CreateTag(), (float) (100.0 * (1.0 - (double) num6)))
			};
			ComplexRecipe.RecipeElement[] results3 = new ComplexRecipe.RecipeElement[1]
			{
			new ComplexRecipe.RecipeElement(SimHashes.TempConductorSolid.CreateTag(), 100f)
			};
			new ComplexRecipe(ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", (IList<ComplexRecipe.RecipeElement>)ingredients3, (IList<ComplexRecipe.RecipeElement>)results3), ingredients3, results3)
			{
			time = 80f,
			description = (string)STRINGS.BUILDINGS.PREFABS.SUPERMATERIALREFINERY.TEMPCONDUCTORSOLID_RECIPE_DESCRIPTION,
			useResultAsDescription = true
			}.fabricators = new List<Tag>()
			{
			TagManager.Create("SupermaterialRefinery")
			};
		}

		//public static string Insulator_OriginalRecipeId
		//{
		//	get
		//	{
		//		float num3 = 0.15f;
		//		float num4 = 0.05f;
		//		float num5 = 1f - num4 - num3;
		//		ComplexRecipe.RecipeElement[] ingredients2 = new ComplexRecipe.RecipeElement[3]
		//		{
		//			new ComplexRecipe.RecipeElement(SimHashes.Isoresin.CreateTag(), 100f * num3),
		//			new ComplexRecipe.RecipeElement(SimHashes.Katairite.CreateTag(), 100f * num5),
		//			new ComplexRecipe.RecipeElement(BasicFabricConfig.ID.ToTag(), 100f * num4)
		//		};
		//		ComplexRecipe.RecipeElement[] results2 = new ComplexRecipe.RecipeElement[1]
		//		{
		//			new ComplexRecipe.RecipeElement(SimHashes.SuperInsulator.CreateTag(), 100f)
		//		};
		//		return ComplexRecipeManager.MakeRecipeID("SupermaterialRefinery", ingredients2, results2);
		//	}
		//}

		//public static void RemoveRecipe(string recipeId)
		//{
		//	List<ComplexRecipe> recipes = ComplexRecipeManager.Get().recipes;
		//	foreach (ComplexRecipe recipe in recipes)
		//	{
		//		if (recipe.id.Equals(recipeId))
		//			recipes.Remove(recipe);
		//	}
		//}
	}
}
