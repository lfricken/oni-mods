using Database;
using Harmony;
using STRINGS;
using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;
using BUILDINGS = TUNING.BUILDINGS;

//////////////////////////
// see Preview.md
//////////////////////////

namespace AlgaeGrower
{
    /// <summary>
    /// Animations, 
    /// </summary>
    public class AlgaeGrower : StateMachineComponent<AlgaeGrower.SMInstance>
    {
        /// <summary>
        /// Probably where gas is created by this object?
        /// </summary>
        [SerializeField]
        public CellOffset PressureSampleOffset = CellOffset.none;

        [MyCmpGet]
        private readonly Operational _operational;

        public AlgaeGrower(Operational operational)
        {
            _operational = operational;
        }

        protected override void OnPrefabInit()
        {
            GetComponent<KBatchedAnimController>().randomiseLoopedOffset = true;
            base.OnPrefabInit();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();
            smi.StartSM();
        }

        public class SMInstance : GameStateMachine<States, SMInstance, AlgaeGrower, object>.GameInstance
        {
            private readonly Operational _operational;
            private readonly ElementConverter _converter;

            public SMInstance(AlgaeGrower master) : base(master)
            {
                _operational = master.GetComponent<Operational>();
                _converter = master.GetComponent<ElementConverter>();
            }

            public bool HasEnoughMass(Tag tag) => _converter.HasEnoughMass(tag);

            public bool IsOperational => _operational.IsOperational;

            public bool HasLight()
            {
                var cell = Grid.PosToCell(smi.master.transform.GetPosition());
                return Grid.LightCount[cell] > 0;
            }
        }

        public class States : GameStateMachine<States, SMInstance, AlgaeGrower>
        {
            public State GeneratingOxygen;
            public State StoppedGeneratingOxygen;
            public State StoppedGeneratingOxygenTransition;
            public State NoWater;
            public State NoFert;
            public State GotFert;
            public State GotWater;
            public State LostFert;
            public State NotOperational;
            public State NoLight;

            public override void InitializeStates(out BaseState defaultState)
            {
                defaultState = NotOperational;

                root
                    .EventTransition(GameHashes.OperationalChanged, NotOperational, smi => !smi.IsOperational);

                NotOperational
                    .QueueAnim("off")
                    .EventTransition(GameHashes.OperationalChanged, NoLight, smi => smi.IsOperational);

                NoLight
                    .QueueAnim("off")
                    .Enter(smi => smi.master._operational.SetActive(false))
                    .Update("NoLight", (smi, dt) => { if (smi.HasLight() && smi.HasEnoughMass(GameTags.Fertilizer)) smi.GoTo(GotFert); }, UpdateRate.SIM_1000ms);

                GotFert
                    .PlayAnim("on_pre")
                    .OnAnimQueueComplete(NoWater);

                LostFert
                    .PlayAnim("on_pst")
                    .OnAnimQueueComplete(NoFert);

                NoFert
                    .QueueAnim("off")
                    .EventTransition(GameHashes.OnStorageChange, GotFert, smi => smi.HasEnoughMass(GameTags.Fertilizer))
                    .Enter(smi => smi.master._operational.SetActive(false));

                NoWater
                    .QueueAnim("on")
                    .Enter(smi => smi.master.GetComponent<PassiveElementConsumer>().EnableConsumption(true))
                    .EventTransition(GameHashes.OnStorageChange, LostFert, smi => !smi.HasEnoughMass(GameTags.Fertilizer))
                    .EventTransition(GameHashes.OnStorageChange, GotWater, smi => smi.HasEnoughMass(GameTags.Fertilizer) && smi.HasEnoughMass(GameTags.Water));

                GotWater
                    .PlayAnim("working_pre")
                    .OnAnimQueueComplete(GeneratingOxygen);

                GeneratingOxygen
                    .Enter(smi => smi.master._operational.SetActive(true))
                    .Exit(smi => smi.master._operational.SetActive(false))
                    .QueueAnim("working_loop", true)
                    .EventTransition(GameHashes.OnStorageChange, StoppedGeneratingOxygen,
                        smi => !smi.HasEnoughMass(GameTags.Water) || !smi.HasEnoughMass(GameTags.Fertilizer))
                    .Update("GeneratingOxygen", (smi, dt) => { if (!smi.HasLight()) smi.GoTo(StoppedGeneratingOxygen); }, UpdateRate.SIM_1000ms);

                StoppedGeneratingOxygen
                    .PlayAnim("working_pst")
                    .OnAnimQueueComplete(StoppedGeneratingOxygenTransition);

                StoppedGeneratingOxygenTransition
                    .Update("StoppedGeneratingOxygenTransition", (smi, dt) => { if (!smi.HasLight()) smi.GoTo(NoLight); }, UpdateRate.SIM_200ms)
                    .EventTransition(GameHashes.OnStorageChange, NoWater, smi => !smi.HasEnoughMass(GameTags.Water) && smi.HasLight())
                    .EventTransition(GameHashes.OnStorageChange, LostFert, smi => !smi.HasEnoughMass(GameTags.Fertilizer) && smi.HasLight())
                    .EventTransition(GameHashes.OnStorageChange, GotWater, smi => smi.HasEnoughMass(GameTags.Water) && smi.HasLight() &&
                                                                                  smi.HasEnoughMass(GameTags.Fertilizer));
            }
        }
    }

    /// <summary>
    /// Description, stats, functionality.
    /// </summary>
    public class AlgaeGrowerConfig : IBuildingConfig
    {
        public const string Id = "AlgaeGrower";
        public const string DisplayName = "Algae Grower";
        public const string Description = "Algae colony, Duplicant colony... we're more alike than we are different.";
        public static string Effect =
            $"Consumes {ELEMENTS.FERTILIZER.NAME}, {ELEMENTS.CARBONDIOXIDE.NAME} and {ELEMENTS.WATER.NAME} " +
            $"to grow {ELEMENTS.ALGAE.NAME} and emit some {ELEMENTS.OXYGEN.NAME}.\n\nRequires {UI.FormatAsLink("Light", "LIGHT")}  to grow.";

        private static readonly List<Storage.StoredItemModifier> PollutedWaterStorageModifiers = new List<Storage.StoredItemModifier>
        {
            Storage.StoredItemModifier.Hide,
            Storage.StoredItemModifier.Seal
        };

        /// <summary>
        /// Define construction costs, health, size, noises, random stats.
        /// </summary>
        public override BuildingDef CreateBuildingDef()
        {
            var buildingDef = BuildingTemplates.CreateBuildingDef(
                id: Id,
                width: 1,
                height: 2,
                anim: "algaefarm_kanim",
                hitpoints: BUILDINGS.HITPOINTS.TIER1,
                construction_time: BUILDINGS.CONSTRUCTION_TIME_SECONDS.TIER2,
                construction_mass: BUILDINGS.CONSTRUCTION_MASS_KG.TIER4,
                construction_materials: MATERIALS.FARMABLE,
                melting_point: BUILDINGS.MELTING_POINT_KELVIN.TIER1,
                build_location_rule: BuildLocationRule.OnFloor,
                decor: BUILDINGS.DECOR.PENALTY.TIER1,
                noise: NOISE_POLLUTION.NOISY.TIER0);

            buildingDef.Floodable = false;
            buildingDef.MaterialCategory = MATERIALS.FARMABLE;
            buildingDef.AudioCategory = "HollowMetal";
            buildingDef.UtilityInputOffset = new CellOffset(0, 0);
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
            buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;

            SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_bubbles", NOISE_POLLUTION.NOISY.TIER0);
            SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_in", NOISE_POLLUTION.NOISY.TIER0);
            SoundEventVolumeCache.instance.AddVolume("algaefarm_kanim", "AlgaeHabitat_algae_out", NOISE_POLLUTION.NOISY.TIER0);

            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            DefineStorage(go);
            DefineConverter(go);
            DefineDropers(go);
            DefineCollectors(go);

            go.AddOrGet<AnimTileable>();
            var algaeHabitat = go.AddOrGet<AlgaeGrower>();
            algaeHabitat.PressureSampleOffset = new CellOffset(0, 1);

            Prioritizable.AddRef(go);
        }

        /// <summary>
        /// This building will automatically drop stuff.
        /// </summary>
        protected void DefineDropers(GameObject go)
        {
            var elementDropper = go.AddComponent<ElementDropper>();
            elementDropper.emitMass = 5;
            elementDropper.emitTag = SimHashes.Algae.CreateTag();
        }

        /// <summary>
        /// Collects these elements from the environment automatically.
        /// </summary>
        protected void DefineCollectors(GameObject go)
        {
            var elementCollector = go.AddOrGet<ElementConsumer>();
            elementCollector.elementToConsume = SimHashes.CarbonDioxide;
            elementCollector.consumptionRate = 0.01375f;
            elementCollector.consumptionRadius = 3;
            elementCollector.storeOnConsume = true;
            elementCollector.sampleCellOffset = new Vector3(0.0f, 1f, 0.0f);
            elementCollector.isRequired = true; // isRequired means that an warning will show in game if the element isn't available
            elementCollector.showInStatusPanel = true;

            var passiveElementCollector = go.AddComponent<PassiveElementConsumer>();
            passiveElementCollector.elementToConsume = SimHashes.Water;
            passiveElementCollector.consumptionRate = 1.2f;
            passiveElementCollector.consumptionRadius = 1;
            passiveElementCollector.showDescriptor = false;
            passiveElementCollector.storeOnConsume = true;
            passiveElementCollector.capacityKG = 360f;
            passiveElementCollector.showInStatusPanel = false;
        }

        /// <summary>
        /// Elements we can store, and elements that need to be delivered.
        /// </summary>
        protected void DefineStorage(GameObject go)
        {
            var storage1 = go.AddOrGet<Storage>();
            storage1.showInUI = true;

            var storage2 = go.AddComponent<Storage>();
            storage2.capacityKg = 5f;
            storage2.showInUI = true;
            storage2.SetDefaultStoredItemModifiers(PollutedWaterStorageModifiers);
            storage2.allowItemRemoval = false;
            storage2.storageFilters = new List<Tag> { SimHashes.Algae.CreateTag(), SimHashes.CarbonDioxide.CreateTag() };

            var manualDeliveryKg1 = go.AddOrGet<ManualDeliveryKG>();
            manualDeliveryKg1.SetStorage(storage1);
            manualDeliveryKg1.requestedItemTag = SimHashes.Fertilizer.CreateTag();
            manualDeliveryKg1.capacity = 90f;
            manualDeliveryKg1.refillMass = 18f;
            manualDeliveryKg1.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;

            var manualDeliveryKg2 = go.AddComponent<ManualDeliveryKG>();
            manualDeliveryKg2.SetStorage(storage1);
            manualDeliveryKg2.requestedItemTag = SimHashes.Water.CreateTag();
            manualDeliveryKg2.capacity = 360f;
            manualDeliveryKg2.refillMass = 72f;
            manualDeliveryKg2.allowPause = true;
            manualDeliveryKg2.choreTypeIDHash = Db.Get().ChoreTypes.OperateFetch.IdHash;
        }

        /// <summary>
        /// Conversion definitions.
        /// </summary>
        public void DefineConverter(GameObject go)
        {
            var elementConverter = go.AddComponent<ElementConverter>();
            elementConverter.consumedElements = new[]
            {
                new ElementConverter.ConsumedElement(SimHashes.CarbonDioxide.CreateTag(), 0.01375f),
                new ElementConverter.ConsumedElement(SimHashes.Fertilizer.CreateTag(), 0.000625f),
                new ElementConverter.ConsumedElement(SimHashes.Water.CreateTag(), 0.005625f)
            };
            elementConverter.outputElements = new[]
            {
                new ElementConverter.OutputElement(1.000f, SimHashes.Oxygen, 303.15f, false, 0.0f, 1f),
                new ElementConverter.OutputElement(0.015f, SimHashes.Algae, 303.15f, true, 0.0f, 1f)
            };
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
            BuildingTemplates.DoPostConfigure(go);
        }
    }

    /// <summary>
    /// Set Name, Description, Effect description, Tech Grouping, buildscreen.
    /// </summary>
    public class AlgaeGrowerPatches
    {
        [HarmonyPatch(typeof(GeneratedBuildings))]
        [HarmonyPatch("LoadGeneratedBuildings")]
        public static class GeneratedBuildings_LoadGeneratedBuildings_Patch
        {
            public static void Prefix()
            {
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AlgaeGrowerConfig.Id.ToUpperInvariant()}.NAME", AlgaeGrowerConfig.DisplayName);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AlgaeGrowerConfig.Id.ToUpperInvariant()}.DESC", AlgaeGrowerConfig.Description);
                Strings.Add($"STRINGS.BUILDINGS.PREFABS.{AlgaeGrowerConfig.Id.ToUpperInvariant()}.EFFECT", AlgaeGrowerConfig.Effect);

                ModUtil.AddBuildingToPlanScreen("Oxygen", AlgaeGrowerConfig.Id);
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                var tech = new List<string>(Techs.TECH_GROUPING["FarmingTech"]) { AlgaeGrowerConfig.Id };
                Techs.TECH_GROUPING["FarmingTech"] = tech.ToArray();
            }
        }

        [HarmonyPatch(typeof(KSerialization.Manager))]
        [HarmonyPatch("GetType")]
        [HarmonyPatch(new[] { typeof(string) })]
        public static class KSerializationManager_GetType_Patch
        {
            public static void Postfix(string type_name, ref Type __result)
            {
                if (type_name == "AlgaeGrower.AlgaeGrower")
                {
                    __result = typeof(AlgaeGrower);
                }
            }
        }
    }
}
