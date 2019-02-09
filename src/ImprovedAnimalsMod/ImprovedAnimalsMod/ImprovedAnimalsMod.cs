using Harmony;
using TUNING;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Klei;

namespace AnimalsMod
{
	#region animals
	
	[HarmonyPatch(typeof(PacuConfig), "CreatePacu")]
	public static class PacuMod
	{
		public static void Postfix(GameObject __result)
		{
			Diet diet = new Diet(
				new Diet.Info(new HashSet<Tag>() { SimHashes.SlimeMold.CreateTag() }, SimHashes.CrudeOil.CreateTag(), 200, 1, Db.Get().Diseases.FoodPoisoning.Id, 0f),
				new Diet.Info(new HashSet<Tag>() { SimHashes.Algae.CreateTag() }, SimHashes.ToxicSand.CreateTag(), 200, 1, Db.Get().Diseases.FoodPoisoning.Id, 0f)
				);
			CreatureCalorieMonitor.Def cmon = __result.AddOrGetDef<CreatureCalorieMonitor.Def>();
			cmon.diet = diet;
			cmon.minPoopSizeInCalories = 30;
			__result.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

		//	Diet diet = __result.AddOrGetDef<SolidConsumerMonitor.Def>().diet;
		//	diet.infos.Add (new Diet.Info(new HashSet<Tag>() { SimHashes.SlimeMold.CreateTag() }, SimHashes.CrudeOil.CreateTag(), 50, 0.5f, Db.Get().Diseases.FoodPoisoning.Id, 2000f));
		}
	}
	[HarmonyPatch(typeof(PacuCleanerConfig), "CreatePacu")]
	public static class PacucleanerMod
	{
		public static void Postfix(GameObject __result)
		{
			Diet diet = new Diet(
				new Diet.Info(new HashSet<Tag>() { SimHashes.Water.CreateTag() }, SimHashes.ToxicSand.CreateTag(), 50, 1, Db.Get().Diseases.FoodPoisoning.Id, 0f),
				new Diet.Info(new HashSet<Tag>() { SimHashes.DirtyWater.CreateTag() }, SimHashes.SlimeMold.CreateTag(), 50, 1, Db.Get().Diseases.FoodPoisoning.Id, 0f)
				);
			CreatureCalorieMonitor.Def cmon = __result.AddOrGetDef<CreatureCalorieMonitor.Def>();
			cmon.diet = diet;
			cmon.minPoopSizeInCalories = 30;
			__result.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;

		}
	}
	/*
	[HarmonyPatch(typeof(PacuTropicalConfig), "CreatePacu")]
	public static class PacutropicalMod
	{
		public static void Postfix(GameObject __result)
		{
			Diet diet = new Diet(
				new Diet.Info(new HashSet<Tag>() { SimHashes.Algae.CreateTag() }, SimHashes.DirtyWater.CreateTag(), 200, 1, Db.Get().Diseases.FoodPoisoning.Id, 0f),
				new Diet.Info(new HashSet<Tag>() { SimHashes.Clay.CreateTag() }, SimHashes.Mercury.CreateTag(), 200, 1, Db.Get().Diseases.FoodPoisoning.Id, 0f),
				new Diet.Info(new HashSet<Tag>() { SimHashes.BleachStone.CreateTag() }, SimHashes.Mercury.CreateTag(), 200, 1, Db.Get().Diseases.FoodPoisoning.Id, 0f)
				//new Diet.Info(new HashSet<Tag>() { SimHashes.ToxicSand.CreateTag() }, SimHashes.Mercury.CreateTag(), 50, 1, Db.Get().Diseases.FoodPoisoning.Id, 0f)
				);
			CreatureCalorieMonitor.Def cmon = __result.AddOrGetDef<CreatureCalorieMonitor.Def>();
			cmon.diet = diet;
			cmon.minPoopSizeInCalories = 30;
			__result.AddOrGetDef<SolidConsumerMonitor.Def>().diet = diet;
		}
	}
	*/
	[HarmonyPatch(typeof(BaseHatchConfig), "BasicRockDiet", null)]
	public static class HatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{
			foreach (Diet.Info resu in __result)
			{
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Diamond.CreateTag() }, SimHashes.FoolsGold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Regolith.CreateTag() }, SimHashes.Carbon.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				break;
			}
		}
	}
	[HarmonyPatch(typeof(BaseHatchConfig), "MetalDiet", null)]
	public static class MetalHatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{
			foreach (Diet.Info resu in __result)
			{
				//__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.SolidCarbonDioxide.CreateTag() }, SimHashes.MoltenTungsten.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				//__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Clay.CreateTag() }, SimHashes.Mercury.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Brick.CreateTag() }, SimHashes.Unobtanium.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Aerogel.CreateTag() }, SimHashes.Brick.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Katairite.CreateTag() }, SimHashes.Aerogel.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Tungsten.CreateTag() }, SimHashes.Katairite.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Steel.CreateTag() }, SimHashes.Wolframite.CreateTag(), resu.caloriesPerKg, 2, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.BleachStone.CreateTag() }, SimHashes.Cuprite.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.MaficRock.CreateTag() }, SimHashes.GoldAmalgam.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 0f));
				break;
			}
			__result = new List<Diet.Info>();
		}
	}
	[HarmonyPatch(typeof(BaseHatchConfig), "HardRockDiet", null)]
	public static class RockHatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{

			Diet.Info resu = null;
			foreach (Diet.Info resua in __result)
			{
				resu = resua;
				break;
			}

			__result.Clear();
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Carbon.CreateTag() }, SimHashes.SlimeMold.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.IgneousRock.CreateTag() }, SimHashes.Obsidian.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.SedimentaryRock.CreateTag() }, SimHashes.Granite.CreateTag(), resu.caloriesPerKg, resu.producedConversionRate, Db.Get().Diseases.FoodPoisoning.Id, 100000f));
			
		}
	}

	[HarmonyPatch(typeof(BaseHatchConfig), "VeggieDiet", null)]
	public static class SageHatchMod
	{
		public static void Postfix(List<Diet.Info> __result)
		{

			Diet.Info resu = null;
			foreach (Diet.Info resua in __result)
			{
				resu = resua;
				break;
			}
			__result.Clear();
			__result.Add(new Diet.Info(new HashSet<Tag>() { SimHashes.Steel.CreateTag() }, SimHashes.Lime.CreateTag(), resu.caloriesPerKg, 0.4f, Db.Get().Diseases.FoodPoisoning.Id, 1000000));
		}
	}
}
#endregion