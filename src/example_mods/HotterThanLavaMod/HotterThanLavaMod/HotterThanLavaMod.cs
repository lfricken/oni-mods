using Harmony;
using UnityEngine;
using System.Collections.Generic;

//todo add new elements to get from melting or cooling algae-> water, -> propane -> mercury , liquid iron ->  electrum  , liquid gold -> pyrite edit electrum to superheatable
//bonus: pacu in algae out polluted water
//bonus: tropical pacu in algae out water
//bonus: last pacu in algae out copper
//bonus: increase heavy wire power to 50k
// electrum-> liquid gold pyrite -> iron

//wishlist: add trait tag in settings mod 
//Wishlist: change air cooler to work with electricity
//Wishlist: a power sensor that tells a building how much power it should consume for heating / cooling / else

namespace HotterThanLavaMod
{
	#region lavaheater
	[HarmonyPatch(typeof(LogicTemperatureSensorConfig), "DoPostConfigureComplete", new System.Type[] { typeof(GameObject) })]
	public static class LogicTemperatureSensormod
	{
		public static void Postfix(LogicTemperatureSensorConfig __instance, GameObject go)
		{
			AccessTools.Field(typeof(LogicTemperatureSensor), "maxTemp").SetValue((object)go.AddOrGet<LogicTemperatureSensor>(), (object)3358.15f);

		}
	}
	[HarmonyPatch(typeof(LiquidHeaterConfig), "ConfigureBuildingTemplate", null)]
	public static class LiquidHeaterLogicMod
	{
		public static void Postfix(LiquidHeaterConfig __instance, GameObject go)
		{
			AccessTools.Field(typeof(SpaceHeater), "targetTemperature").SetValue((object)go.AddOrGet<SpaceHeater>(), (object)10000358.15f);
		}
	}
	[HarmonyPatch(typeof(LiquidHeaterConfig), "CreateBuildingDef", null)]
	public static class LiquidHeaterMod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.BaseMeltingPoint = 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
			__result.EnergyConsumptionWhenActive = 1950f;
			__result.ExhaustKilowattsWhenActive = 10000f;
			__result.SelfHeatKilowattsWhenActive = 64f;
		}
	}
	[HarmonyPatch(typeof(SpaceHeaterConfig), "ConfigureBuildingTemplate", null)]
	public static class SpaceHeaterLogicMod
	{
		public static void Postfix(SpaceHeaterConfig __instance, GameObject go)
		{
			AccessTools.Field(typeof(SpaceHeater), "targetTemperature").SetValue((object)go.AddOrGet<SpaceHeater>(), (object)10000358.15f);
		}
	}
	[HarmonyPatch(typeof(SpaceHeaterConfig), "CreateBuildingDef", null)]
	public static class SpaceHeaterMod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.BaseMeltingPoint= 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
			__result.EnergyConsumptionWhenActive = 1950f;
			__result.ExhaustKilowattsWhenActive = 10000f;
			__result.SelfHeatKilowattsWhenActive = 64f;
			__result.Floodable = true;
		}
	}
	[HarmonyPatch(typeof(PolymerizerConfig), "CreateBuildingDef", null)]
	public static class PolimerizerMod
	{
		public static void Postfix(BuildingDef __result)
		{
			__result.BaseMeltingPoint = 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
			__result.Floodable = true;
		}
	}
	/*
	[HarmonyPatch(typeof(WireHighWattageConfig), "DoPostConfigureComplete", null)]
	public static class WireHighWattageMod
	{
		public static void Postfix(SpaceHeaterConfig __instance, GameObject go)
		{
			AccessTools.Field(typeof(WireHighWattageConfig), "DoPostConfigureComplete").SetValue((object)go.AddOrGet<WireHighWattageConfig>(), (object)Wire.WattageRating.Max50000);
		}
	}*/
	[HarmonyPatch(typeof(LiquidPumpConfig), "CreateBuildingDef", null)]
	public static class LiquidPumpmod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.BaseMeltingPoint = 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
		}
	}

	[HarmonyPatch(typeof(GasPumpConfig), "CreateBuildingDef", null)]
	public static class GasPumpMod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.BaseMeltingPoint = 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
		}
	}
	[HarmonyPatch(typeof(LiquidConditionerConfig), "CreateBuildingDef", null)]
	public static class ConditionerMod
	{
		public static void Postfix(BuildingDef __result)
		{
			//__result.MaterialCategory = MATERIALS.ANY_BUILDABLE;
			__result.BaseMeltingPoint = 1000398.15f;
			__result.OverheatTemperature = 1000398.15f;
		}
	}
	[HarmonyPatch(typeof(MetalRefineryConfig), "ConfigureBuildingTemplate", null)]
	public static class RefinerypyriteMod
	{
		public static void Postfix()
		{
			Element pyrite = ElementLoader.FindElementByHash(SimHashes.FoolsGold);
			pyrite.thermalConductivity = ElementLoader.FindElementByHash(SimHashes.Ceramic).thermalConductivity;
	
			ComplexRecipe.RecipeElement recipeElement3 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.IronOre).tag, 50f);
			ComplexRecipe.RecipeElement recipeElement4 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.Gold).tag, 20f);
			ComplexRecipe.RecipeElement recipeElement6 = new ComplexRecipe.RecipeElement(ElementLoader.FindElementByHash(SimHashes.FoolsGold).tag, 50f);

			ComplexRecipe.RecipeElement[] ingredients1 = new ComplexRecipe.RecipeElement[2] { recipeElement3, recipeElement4 };
			ComplexRecipe.RecipeElement[] results1 = new ComplexRecipe.RecipeElement[1] { recipeElement6 };
			string str1 = ComplexRecipeManager.MakeRecipeID("MetalRefinery", (IList<ComplexRecipe.RecipeElement>)results1, (IList<ComplexRecipe.RecipeElement>)ingredients1);
			new ComplexRecipe(str1, ingredients1, results1)
			{
				time = 1000f,
				useResultAsDescription = true,
				description = string.Format((string)STRINGS.BUILDINGS.PREFABS.METALREFINERY.RECIPE_DESCRIPTION, (object)ElementLoader.GetElement(recipeElement3.material).name, (object)ElementLoader.GetElement(recipeElement6.material).name)
			}.fabricators = new List<Tag>()
	{
	  TagManager.Create("MetalRefinery")
	};

		}
	}
	#endregion

}
