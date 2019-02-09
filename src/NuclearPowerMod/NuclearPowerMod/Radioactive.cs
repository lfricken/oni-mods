// Klei.AI.Radioactive
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

[HarmonyPatch(typeof(InitializeCheck), "Awake")]
internal static class UraniumElement_InitializeCheck_Awake
{
	private static void Prefix()
	{
		Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.NAME", UI.FormatAsLink("Radioactive", "RADIOACTIVE"));
		Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.DESCRIPTION", "\nThis Duplicant's chest congestion is making it difficult to breathe");
		Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.LEGEND_HOVERTEXT", "Radioactivity Present\n");
		Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.COUGH_SYMPTOM", "Coughing");
		Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.COUGH_SYMPTOM_TOOLTIP", "Duplicants periodically cough up Polluted Oxygen, producing additional germs");
		Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.DESCRIPTIVE_SYMPTOMS", "Lethal without medical care. Duplicants experience coughing and shortness of breath.");
		Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.DISEASE_SOURCE_DESCRIPTOR", "Currently infected with {2}.\n\nThis Duplicant will produce {1} when coughing.");
		Strings.Add("STRINGS.DUPLICANTS.DISEASES.RADIOACTIVE.DISEASE_SOURCE_DESCRIPTOR_TOOLTIP", "This Duplicant will cough approximately every {0}" + UI.HORIZONTAL_BR_RULE + "Each time they cough, they will release {1}");
	}
}

[HarmonyPatch(typeof(ProcGenGame.WorldGen), MethodType.Constructor)]
[HarmonyPatch(new Type[] { })]
internal static class UraniumElement_WorldGen_ctor
{
	private static void Postfix()
	{
		Debug.Log(" === WorldGen.ctor Postfix === ");
		//UraniumElement_Prop.Radio = new Radioactivity();
		//var diseases = new Database.Diseases(((ModifierSet)__instance).Root);
		ProcGenGame.WorldGen.diseaseIds = new List<string>
			{
				"FoodPoisoning",
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				"SlimeLung",
				"Radioactive"
			};


		List<string> dlist = ProcGenGame.WorldGen.diseaseIds;
			string layerhelper = "";
			foreach (var i in dlist)
			{
				layerhelper += dlist.IndexOf(i);
				layerhelper += " : ";
				layerhelper += (byte)dlist.IndexOf(i);
				layerhelper += " : ";
				layerhelper += i;
				layerhelper += System.Environment.NewLine;
			}
			Debug.Log("diseasehelp " + layerhelper);
		

	}

}
[HarmonyPatch(typeof(Db), "Initialize")]
internal static class UraniumElement_Db_Initialize
{
	private static bool Prefix(Db __instance)
	{
		Debug.Log(" === Db.Initialize Prefix === ");
		//UraniumElement_Prop.Radio = new Radioactivity();
		//var diseases = new Database.Diseases(((ModifierSet)__instance).Root);
		return true;
	}

	private static void Postfix(Db __instance)
	{
		Debug.Log(" === Db.Initialize Postfix === ");
		/*
		List<string> ls = new List<string>((string[])Database.Techs.TECH_GROUPING["Combustion"]);
		ls.Add(InverseElectrolyzerConfig.ID);
		Database.Techs.TECH_GROUPING["Combustion"] = (string[])ls.ToArray();
		*/
		//return true;
		__instance.Diseases = new Database.Diseases(((ModifierSet)__instance).Root);
	}
}
/*
[HarmonyPatch(typeof(Klei.AI.Disease), MethodType.Constructor)]
internal static class Diseasecolor
{
	private static void Postfix(Disease __instance)
	{
		__instance.overlayColour = new Color32(byte.MaxValue, (byte)0, (byte)200, byte.MaxValue);
	}
}*/
namespace Klei.AI
{
    public class Radioactive : Disease
    {
		public class RadioactiveComponent : DiseaseComponent
        {
            public class StatesInstance : GameStateMachine<States, StatesInstance, DiseaseInstance, object>.GameInstance
            {
                public float lastCoughtTime;

                public StatesInstance(DiseaseInstance master)
                    : base(master)
                {
                }
                public Reactable GetReactable()
                {
                    return new SelfEmoteReactable(base.master.gameObject, (HashedString)"RadioactiveLungCough", Db.Get().ChoreTypes.Cough, "anim_sneeze_kanim").AddStep(new EmoteReactable.EmoteStep
                    {
                        anim = "sneeze",
                        finishcb = this.ProduceSlime
                    }).AddStep(new EmoteReactable.EmoteStep
                    {
                        anim = "sneeze_pst"
                    }).AddStep(new EmoteReactable.EmoteStep
                    {
                        startcb = this.FinishedCoughing
                    });
                }

                private void ProduceSlime(GameObject cougher)
                {
                    AmountInstance amountInstance = Db.Get().Amounts.Temperature.Lookup(cougher);
                    int gameCell = Grid.PosToCell(cougher);
                    SimMessages.AddRemoveSubstance(gameCell, SimHashes.ContaminatedOxygen, CellEventLogger.Instance.Cough, 0.1f, amountInstance.value, Db.Get().Diseases.GetIndex("Radioactive"), 1000, true, -1);
                    PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Resource, string.Format(DUPLICANTS.DISEASES.ADDED_POPFX, base.master.modifier.Name, 1000), cougher.transform, 1.5f, false);
                }

                private void FinishedCoughing(GameObject cougher)
                {
                    base.sm.coughFinished.Trigger(this);
                }
            }

            public class States : GameStateMachine<States, StatesInstance, DiseaseInstance>
            {
                public class BreathingStates : State
                {
                    public State normal;

                    public State cough;
                }

                public Signal coughFinished;

                public BreathingStates breathing;

                public State notbreathing;

                public override void InitializeStates(out BaseState default_state)
                {
                    default_state = this.breathing;
                    this.breathing.DefaultState(this.breathing.normal).TagTransition(GameTags.NoOxygen, this.notbreathing, false).Enter("SetCoughTime", delegate (StatesInstance smi)
                    {
                        smi.lastCoughtTime = Time.time;
                    })
                        .Update("Cough", delegate (StatesInstance smi, float dt)
                        {
                            if (!smi.master.IsDoctored && Time.time - smi.lastCoughtTime > 20f)
                            {
                                smi.GoTo(this.breathing.cough);
                            }
                        }, UpdateRate.SIM_4000ms, false);
                    this.breathing.cough.ToggleReactable((StatesInstance smi) => smi.GetReactable()).OnSignal(this.coughFinished, this.breathing.normal);
                    this.notbreathing.TagTransition(new Tag[1]
                    {
                        GameTags.NoOxygen
                    }, this.breathing, true);
                }
            }

            public override object OnInfect(GameObject go, DiseaseInstance diseaseInstance)
            {
                StatesInstance statesInstance = new StatesInstance(diseaseInstance);
                statesInstance.StartSM();
                return statesInstance;
            }

            public override void OnCure(GameObject go, object instance_data)
            {
                StatesInstance statesInstance = (StatesInstance)instance_data;
                statesInstance.StopSM("Cured");
            }

            public override List<Descriptor> GetSymptoms() 
            {
                List<Descriptor> list = new List<Descriptor>();
                list.Add(new Descriptor("Coughing", "Duplicants periodically cough up Polluted Oxygen, producing additional germs", Descriptor.DescriptorType.SymptomAidable, false));
                list.Add(new Descriptor(Strings.Get("DUPLICANTS.DISEASES.Radioactive.COUGH_SYMPTOM"), Strings.Get("DUPLICANTS.DISEASES.Radioactive.COUGH_SYMPTOM_TOOLTIP"), Descriptor.DescriptorType.SymptomAidable, false));
                return list;
            }
        }

        private const float COUGH_FREQUENCY = 20f;

        private const float COUGH_MASS = 0.1f;

        private const int DISEASE_AMOUNT = 1000;

        private const float DEATH_TIMER = 4800f;

        public const string ID = "Radioactive";

        public Radioactive()
            : base("Radioactive", DiseaseType.Pathogen, Severity.Critical, 0.00025f, new List<InfectionVector>
            {
                InfectionVector.Inhalation, InfectionVector.Digestion,InfectionVector.Exposure,InfectionVector.Contact
			}, 2400f, 1, RangeInfo.Idempotent(), RangeInfo.Idempotent(), RangeInfo.Idempotent(), RangeInfo.Idempotent())    //new RangeInfo(20f, 593.15f, float.PositiveInfinity, float.PositiveInfinity), new RangeInfo(20f, 1200f, float.PositiveInfinity, float.PositiveInfinity), new RangeInfo(20f, 0f, float.PositiveInfinity, float.PositiveInfinity), RangeInfo.Idempotent())

		{
            base.doctorRequired = true;
            base.fatalityDuration = 4800f;
            base.AddDiseaseComponent(new CommonSickEffectDisease());
            base.AddDiseaseComponent(new AttributeModifierDisease(new AttributeModifier[5]
            { //add vomitting
                new AttributeModifier("BreathDelta", -1.13636363f, Strings.Get("DUPLICANTS.DISEASES.Radioactive.NAME"), false, false, true),
                new AttributeModifier("Athletics", -3f, Strings.Get("DUPLICANTS.DISEASES.Radioactive.NAME"), false, false, true),
			    new AttributeModifier("StaminaDelta", -0.5f, (string) Strings.Get("DUPLICANTS.DISEASES.Radioactive.NAME"), false, false, true),
			   new AttributeModifier("Sneezyness", 1f, (string) Strings.Get("DUPLICANTS.DISEASES.Radioactive.NAME"), false, false, true),
			  new AttributeModifier(Db.Get().Amounts.Stress.deltaAttribute.Id, -0.03333334f, (string) Strings.Get("DUPLICANTS.DISEASES.Radioactive.NAME"), false, false, true)
	  }));
            base.AddDiseaseComponent(new RadioactiveComponent());
        }

		//rules grow on: Temperature above 1000 degree grow on radium

        protected override void PopulateElemGrowthInfo()
        {
            base.InitializeElemGrowthArray(ref base.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
            base.AddGrowthRule(new GrowthRule
            {
                underPopulationDeathRate = 5.66666675f,
                minCountPerKG = 5f,
                populationHalfLife = 3200f,
                maxCountPerKG = 5000,
                overPopulationHalfLife = 200f,
                minDiffusionCount = 1000,
                diffusionScale = 0.05f,
                minDiffusionInfestationTickCount = 1
            });
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.Oxygen) {
				populationHalfLife = 1200f,
				minCountPerKG = 500f,
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.Mercury)
            {
                populationHalfLife = 30f
            });
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.MercuryGas)
			{
				populationHalfLife = 30f
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.Water)
			{
				populationHalfLife = 10000f
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.Steam)
			{
				populationHalfLife = 10000f
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.Petroleum)
            {
                populationHalfLife = -1000f
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.SourGas)
            {
                populationHalfLife = -1000f
            });

			/*
            base.AddGrowthRule(new StateGrowthRule(Element.State.Solid)
            {
                minCountPerKG = 0.4f,
                populationHalfLife = 3000f,
                overPopulationHalfLife = 1200f,
                diffusionScale = 1E-06f,
                minDiffusionCount = 1000000
            });
            base.AddGrowthRule(new StateGrowthRule(Element.State.Gas)
            {
                minCountPerKG = 250f,
                populationHalfLife = 12000f,
                overPopulationHalfLife = 1200f,
                maxCountPerKG = 10000f,
                minDiffusionCount = 5100,
                diffusionScale = 0.005f
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.ContaminatedOxygen)
            {
                underPopulationDeathRate = 0f,
                populationHalfLife = -300f,
                overPopulationHalfLife = 1200f
            });
            base.AddGrowthRule(new ElementGrowthRule(SimHashes.Oxygen)
            {
                populationHalfLife = 1200f,
                overPopulationHalfLife = 10f
            });
            base.AddGrowthRule(new StateGrowthRule(Element.State.Liquid)
            {
                minCountPerKG = 0.4f,
                populationHalfLife = 1200f,
                overPopulationHalfLife = 300f,
                maxCountPerKG = 100f,
                diffusionScale = 0.01f
            });*/
            base.InitializeElemExposureArray(ref base.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
            base.AddExposureRule(new ExposureRule
            {
                populationHalfLife = 120000f
            });
            base.AddExposureRule(new ElementExposureRule(SimHashes.Petroleum)
            {
                populationHalfLife = -500f
            });
            base.AddExposureRule(new ElementExposureRule(SimHashes.SourGas)
            {
                populationHalfLife = -500f
            });
            base.AddExposureRule(new ElementExposureRule(SimHashes.Mercury)
            {
                populationHalfLife = 30f
            });
			base.AddExposureRule(new ElementExposureRule(SimHashes.MercuryGas)
			{
				populationHalfLife = 30f
			});
		}

        public override List<Descriptor> GetDiseaseSourceDescriptors()
        {
            List<Descriptor> list = new List<Descriptor>();
            list.Add(new Descriptor(string.Format(Strings.Get("DUPLICANTS.DISEASES.Radioactive.DISEASE_SOURCE_DESCRIPTOR"), GameUtil.GetFormattedTime(20f), GameUtil.GetFormattedDiseaseAmount(1000), base.Name), string.Format(Strings.Get("DUPLICANTS.DISEASES.Radioactive.DISEASE_SOURCE_DESCRIPTOR_TOOLTIP"), GameUtil.GetFormattedTime(20f), GameUtil.GetFormattedDiseaseAmount(1000), base.Name), Descriptor.DescriptorType.DiseaseSource, false));
            return list;
        }
    }
}