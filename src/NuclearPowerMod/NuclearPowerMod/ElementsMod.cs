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
	public class RadioActiveElements
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


	public static class SimHashesextension
	{
		public static void AddTo(this SimHashes add)
		{
			SimHashes s = new SimHashes();
			s = s | add;
		}
	}
	

	[HarmonyPatch(typeof(ElementLoader), "SetupElementsTable")]
	internal static class UraniumElement_InitializeCheck_Awake
	{
		private static void Prefix()
		{
			Debug.Log(" === InitializeCheck.Awake Prefix === ");
			Strings.Add("STRINGS.ELEMENTS." + (int)RadiumOreElement.ID + ".NAME", UI.FormatAsLink("Radium Ore", "RADORE"));
			Strings.Add("STRINGS.ELEMENTS." + (int)RadiumOreElement.ID + ".DESC", "Molten Radium is a molten radioactive element.");
			Strings.Add("STRINGS.ELEMENTS." + (int)RadiumOreElement.ID + ".BUILD_DESC", "");
			Strings.Add("STRINGS.ELEMENTS." + (int)MoltenRadiumElement.ID + ".NAME", UI.FormatAsLink("Molten Radium", "MOLTENRAD"));
			Strings.Add("STRINGS.ELEMENTS." + (int)MoltenRadiumElement.ID + ".DESC", "Molten Radium is a molten radioactive element.");
			Strings.Add("STRINGS.ELEMENTS." + (int)MoltenRadiumElement.ID + ".BUILD_DESC", "");
			Strings.Add("STRINGS.ELEMENTS." + (int)RadiumGasElement.ID + ".NAME", UI.FormatAsLink("Radium Gas", "RADGAS"));
			Strings.Add("STRINGS.ELEMENTS." + (int)RadiumGasElement.ID + ".DESC", "Radium Gas is a gaseous radioactive element.");
			Strings.Add("STRINGS.ELEMENTS." + (int)RadiumGasElement.ID + ".BUILD_DESC", "");

			//UraniumElement_Prop.MyEnum = Utils.ExtendEnum(typeof(SimHashes), UraniumElement.name, (int)UraniumElement.ID);

		}
	}

	[HarmonyPatch(typeof(Assets), "SubstanceListHookup")]
	internal static class UraniumElementAssetsSubstanceListHookup
	{
		private static void Prefix(Assets __instance)
		{
			Debug.Log(" === Assets.SubstanceListHookup Prefix === ");

			Substance radium = __instance.substanceTable.GetSubstance(SimHashes.Radium);
			Substance chlorine = __instance.substanceTable.GetSubstance(SimHashes.Chlorine);
			Substance chlorinegas = __instance.substanceTable.GetSubstance(SimHashes.ChlorineGas);
			Substance MoltenRadium = new Substance();
			MoltenRadium.name = MoltenRadiumElement.name;
			//MoltenRadium.elementID = (SimHashes) SimHashesMod.MoltenRadium;
			MoltenRadium.elementID = (SimHashes)MoltenRadiumElement.ID;
			MoltenRadium.colour = chlorine.colour;
			//MoltenRadium.colour.r = 0;
			//MoltenRadium.colour.g = 0;
			//MoltenRadium.colour.b = 255;
			MoltenRadium.debugColour = chlorine.debugColour;
			MoltenRadium.overlayColour = chlorine.overlayColour;
			MoltenRadium.colourMap = chlorine.colourMap;
			//MoltenRadium.colourMap = (Texture2D)Resources.Load(pt);
			//UraniumElement_Prop.Print(MoltenRadium.colourMap);
			MoltenRadium.shineMask = chlorine.shineMask;
			MoltenRadium.normalMap = chlorine.normalMap;
			MoltenRadium.hitEffect = chlorine.hitEffect;
			MoltenRadium.fallingStartSound = chlorine.fallingStartSound;
			MoltenRadium.fallingStopSound = chlorine.fallingStopSound;
			MoltenRadium.renderedByWorld = chlorine.renderedByWorld;

			MoltenRadium.idx = MoltenRadiumElement.idx;//chlorine.idx;  // ElementLoader.elements.Count + 2;
			MoltenRadium.material = chlorine.material;
			//UraniumElement_Prop.Print(MoltenRadium.material);
			//string pt = "Mods" + Path.DirectorySeparatorChar + "NuclearMod" + Path.DirectorySeparatorChar + "iron.png";
			MoltenRadium.material.mainTexture = chlorine.material.mainTexture;
			//MoltenRadium.material.mainTexture = (Texture2D)Resources.Load(pt);
			//UraniumElement_Prop.Print(MoltenRadium.material);
			MoltenRadium.anim = chlorine.anim;
			//UraniumElement_Prop.Print(MoltenRadium.anim);
			MoltenRadium.anims = chlorine.anims;
			MoltenRadium.hue = chlorine.hue;
			MoltenRadium.saturation = chlorine.saturation;
			MoltenRadium.propertyBlock = radium.propertyBlock;
			MoltenRadium.audioConfig = chlorine.audioConfig;
			MoltenRadium.showInEditor = chlorine.showInEditor;
			MoltenRadium.saturation = chlorine.saturation;

			__instance.substanceTable.GetList().Add(MoltenRadium);
			elementadder.addliquid(MoltenRadiumElement.ID, 200f, 2220f);


			Debug.Log(" === Added(MoltenRadium === ");

			Substance RadiumGas = new Substance();
			RadiumGas.name = RadiumGasElement.name;
			//RadiumGas.elementID = (SimHashes) SimHashesMod.RadiumGas;
			RadiumGas.elementID = (SimHashes)RadiumGasElement.ID;
			RadiumGas.colour = chlorinegas.colour;
			//RadiumGas.colour.r = 0;
			//RadiumGas.colour.g = 0;
			//RadiumGas.colour.b = 255;
			RadiumGas.debugColour = chlorinegas.debugColour;
			RadiumGas.overlayColour = chlorinegas.overlayColour;
			RadiumGas.colourMap = chlorinegas.colourMap;
			//RadiumGas.colourMap = (Texture2D)Resources.Load(pt);
			//UraniumElement_Prop.Print(RadiumGas.colourMap);
			RadiumGas.shineMask = chlorinegas.shineMask;
			RadiumGas.normalMap = chlorinegas.normalMap;
			//RadiumGas.hitEffect = chlorinegas.hitEffect;
			RadiumGas.fallingStartSound = chlorinegas.fallingStartSound;
			RadiumGas.fallingStopSound = chlorinegas.fallingStopSound;
			RadiumGas.renderedByWorld = chlorinegas.renderedByWorld;

			RadiumGas.idx = RadiumGasElement.idx;// chlorinegas.idx;// ElementLoader.elements.Count + 3;
			RadiumGas.material = chlorinegas.material;
			//UraniumElement_Prop.Print(RadiumGas.material);
			//string pt = "Mods" + Path.DirectorySeparatorChar + "NuclearMod" + Path.DirectorySeparatorChar + "iron.png";
			//RadiumGas.material.mainTexture = chlorinegas.material.mainTexture;
			//RadiumGas.material.mainTexture = (Texture2D)Resources.Load(pt);
			//UraniumElement_Prop.Print(RadiumGas.material);
			RadiumGas.anim = chlorinegas.anim;
			//UraniumElement_Prop.Print(RadiumGas.anim);
			RadiumGas.anims = chlorinegas.anims;
			RadiumGas.hue = chlorinegas.hue;
			RadiumGas.saturation = chlorinegas.saturation;
			RadiumGas.propertyBlock = radium.propertyBlock;
			RadiumGas.audioConfig = chlorinegas.audioConfig;
			RadiumGas.showInEditor = chlorinegas.showInEditor;
			RadiumGas.saturation = chlorinegas.saturation;

			__instance.substanceTable.GetList().Add(RadiumGas);
			elementadder.addgas(RadiumGasElement.ID, 200f, 8220f);
			//Debug.Log(" === Assets.SubstanceListHookup Prefix === " + __instance.substanceTable.GetList().Count);
			//for (int i = 0; i < __instance.substanceTable.GetList().Count; i++)
			//{
			//	Debug.Log(" === Assets.SubstanceListHookup Prefix === "+ __instance.substanceTable.GetList().ElementAt(i).name+" "+ __instance.substanceTable.GetList().ElementAt(i).idx);
			//}

			Debug.Log(" === substance list success === ");
		}
	}

	static class elementadder
	{
		public static List<ElementLoader.SolidEntry> sollist = new List<ElementLoader.SolidEntry>();
		public static List<ElementLoader.LiquidEntry> lilist = new List<ElementLoader.LiquidEntry>();
		public static List<ElementLoader.GasEntry> gaslist = new List<ElementLoader.GasEntry>();
		public static ElementLoader.SolidEntry addsolid(SimHashes elementid, float defaultMass, float defaultTemperature)
		{
			ElementLoader.SolidEntry entr = new ElementLoader.SolidEntry();
			entr.elementId = elementid;
			entr.defaultMass = defaultMass;
			entr.defaultTemperature = defaultTemperature;
			sollist.Add(entr);
			return entr;
		}
		public static ElementLoader.LiquidEntry addliquid(SimHashes elementid, float defaultMass, float defaultTemperature)
		{
			ElementLoader.LiquidEntry entr = new ElementLoader.LiquidEntry();
			entr.elementId = elementid;
			entr.defaultMass = defaultMass;
			entr.defaultTemperature = defaultTemperature;
			lilist.Add(entr);
			return entr;
		}
		public static ElementLoader.GasEntry addgas(SimHashes elementid, float defaultMass, float defaultTemperature)
		{
			ElementLoader.GasEntry entr = new ElementLoader.GasEntry();
			entr.elementId = elementid;
			entr.defaultMass = defaultMass;
			entr.defaultTemperature = defaultTemperature;
			gaslist.Add(entr);
			return entr;
		}


	}


	[HarmonyPatch(typeof(ElementLoader), "Load")]
	internal static class UraniumElementElementLoad
	{
		private static void Prefix(ref Hashtable substanceList, ElementLoader.SolidEntry[] solid_entries, ElementLoader.LiquidEntry[] liquid_entries, ElementLoader.GasEntry[] gas_entries, SubstanceTable substanceTable)
		{
			Debug.Log(" === ElementLoader.Load Prefix === ");
			//"UraniumOre","0.129","4","1","1","1","0.9","1808","MoltenIron","","","242.15","800","1840","3","25","159.6882","Metal","Ore | BuildableAny","0","# U02 - Uranium Ore, advanced smelter can get the O2","",
			string line = "UraniumOre,0.129,4,1,1,1,0.9,1808,MoltenIron,,,242.15,800,1840,3,25,159.6882,Metal,Ore | BuildableAny,0,\"# U02 - Uranium Ore, advanced smelter can get the O2\"";
			//textSolid( "\n" + line);
			foreach (var l in elementadder.sollist) solid_entries.Add(l);
			foreach (var l in elementadder.lilist) liquid_entries.Add(l);
			foreach (var l in elementadder.gaslist) gas_entries.Add(l);

		}
	}

	[HarmonyPatch(typeof(GeneratorConfig), "CreateBuildingDef", null)]
	static class elementfinish
	{
		static void Prefix()
		{
			Element radium = ElementLoader.FindElementByHash(SimHashes.Radium);
			Element radiumgas = ElementLoader.FindElementByHash(SimHashes.Helium);
			Element radiumliquid = ElementLoader.FindElementByHash(SimHashes.LiquidHelium);
			radiumgas.name = "Radium Gas";
			radiumliquid.name = "Liquid Radium";
			radiumgas.lowTemp = 6500f;
			radiumgas.defaultValues.temperature = 9000f;
			radiumliquid.defaultValues.temperature = 5000f;
			radiumliquid.lowTemp = 500f;
			radiumliquid.highTemp = 8000f;
			radium.highTemp = 1200f;
			radium.highTempTransitionTarget = radiumliquid.id;
			radiumliquid.lowTempTransitionTarget = radium.id;
			radiumliquid.attributeModifiers = radium.attributeModifiers;
			radiumgas.attributeModifiers = radium.attributeModifiers;

		}
	}
}