using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Harmony;
using Klei.AI;
using Klei.AI.DiseaseGrowthRules;
using STRINGS;
using UnityEngine;
using System.ComponentModel;
using static Klei.AI.Disease;
using VoronoiTree;
using ProcGenGame;
using static ProcGenGame.TerrainCell;


namespace NuclearPowerMod
{
	public class ElementsAdd
	{
	}
	class RadiumOreElement
	{
		public const SimHashes ID = (SimHashes)1231231110;
		public const string name = "RadiumOre";
		public const int idx = 1000001;
	}
	class MoltenRadiumElement
	{
		public const SimHashes ID = (SimHashes)1231231111;
		public const string name = "MoltenRadium";
		public const int idx = 1000001;
	}
	class RadiumGasElement
	{
		public const SimHashes ID = (SimHashes)1231232222;
		public const string name = "RadiumGas";
		public const int idx = 1000002;
	}

	[HarmonyPatch(typeof(ElementLoader), "SetupElementsTable", MethodType.Constructor)]
	internal static class SetupElementsTable_InitializeCheck_Awake
	{
		private static void Prefix()
		{
			Debug.Log(" === InitializeCheck.Awake Prefix === ");
			//Strings.Add("STRINGS.ELEMENTS." + (int)RadiumOreElement.ID + ".NAME", UI.FormatAsLink("Radium Ore", "RADORE"));
			//Strings.Add("STRINGS.ELEMENTS." + (int)RadiumOreElement.ID + ".DESC", "Molten Radium is a molten radioactive element.");
			//Strings.Add("STRINGS.ELEMENTS." + (int)RadiumOreElement.ID + ".BUILD_DESC", "");
			Strings.Add("STRINGS.ELEMENTS.HELIUM"+".NAME", UI.FormatAsLink("Molten Radium", "MOLTENRAD"));
			Strings.Add("STRINGS.ELEMENTS.HELIUM"+".DESC", "Molten Radium is a molten radioactive element.");
			Strings.Add("STRINGS.ELEMENTS.HELIUM" + ".BUILD_DESC", "");
			Strings.Add("STRINGS.ELEMENTS.LIQUIDHELIUM" + ".NAME", UI.FormatAsLink("Radium Gas", "RADGAS"));
			Strings.Add("STRINGS.ELEMENTS.LIQUIDHELIUM" + ".DESC", "Radium Gas is a gaseous radioactive element.");
			Strings.Add("STRINGS.ELEMENTS.LIQUIDHELIUM" + ".BUILD_DESC", "");

			//UraniumElement_Prop.MyEnum = Utils.ExtendEnum(typeof(SimHashes), UraniumElement.name, (int)UraniumElement.ID);

		}
	}

	[HarmonyPatch(typeof(GeneratorConfig), "CreateBuildingDef", null)]
	static class elementfinish
	{
		static void Prefix()
		{
			Element radiumgas = ElementLoader.FindElementByHash(SimHashes.Helium);
			Element radiumliquid = ElementLoader.FindElementByHash(SimHashes.LiquidHelium);
			Element radium = ElementLoader.FindElementByHash(SimHashes.Radium);
			Element naphta = ElementLoader.FindElementByHash(SimHashes.SolidNaphtha);
			Element prop = ElementLoader.FindElementByHash(SimHashes.LiquidPropane);
			radiumgas.name = "Radium Gas";
			radiumliquid.name = "Liquid Radium";
			prop.name = "Radium Precursor";
			radiumgas.lowTemp = 4500f;
			radiumgas.defaultValues.temperature = 9000f;
			radiumliquid.defaultValues.temperature = 5000f;
			radiumliquid.lowTemp = 500f;
			radiumliquid.highTemp = 4500f;
			naphta.highTemp = 1000f;
			radium.highTemp = 3000f;
			radium.highTempTransitionTarget = radiumliquid.id;
			prop.lowTempTransitionTarget = radium.id;
			radiumliquid.lowTempTransitionTarget = naphta.id;
			radiumliquid.attributeModifiers = radium.attributeModifiers;
			radiumgas.attributeModifiers = radium.attributeModifiers;
		}
	}
}