using RimWorld;
using Verse;
using Verse.AI;
using System.Reflection;
using System.Collections.Generic;
using HugsLib;
using HarmonyLib;
using System.Reflection.Emit;
using System.Linq;
using System;
using RimWorldChildren.babygear;

namespace RimWorldChildren
{

    //public class ChildrenBase : ModBase
    //{
    //    public static BnC_Settings settings;

    //    public override string ModIdentifier
    //    {
    //        get
    //        {
    //            return "BabyAndChildren";
    //        }
    //    }

    //    public override void DefsLoaded()
    //    {
    //        settings = new BnC_Settings(Settings);
    //    }
    //}

    [StaticConstructorOnStartup]
    internal static class HarmonyPatches_Children
    {

        static HarmonyPatches_Children()
        {

            Harmony harmonyInstance = new Harmony("rimworld.baby_and_children");
            Harmony.DEBUG = false;

            //MethodInfo jobdriver_lovin_m4_transpiler = AccessTools.Method (typeof(Lovin_Override), "JobDriver_Lovin_M4_Transpiler");
            //harmonyInstance.Patch (typeof(JobDriver_Lovin).GetNestedTypes (AccessTools.all) [0].GetMethod ("<>m__4", AccessTools.all), null, null, new HarmonyMethod (jobdriver_lovin_m4_transpiler));

            MethodInfo jobdriver_wear_transpiler = AccessTools.Method(typeof(Wear_Override), "JobDriver_Wear_MoveNext_Transpiler");
            harmonyInstance.Patch(typeof(JobDriver_Wear).GetNestedTypes(AccessTools.all)[0].GetMethod("MoveNext"), null, null, new HarmonyMethod(jobdriver_wear_transpiler));

            AnotherModCheck.AnotherModPatchRun(harmonyInstance);
        }
    }

    // Handy for more readable code when age-checking
    public static class AgeStage
    {
        public const int Baby = 0;
        public const int Toddler = 1;
        public const int Child = 2;
        public const int Teenager = 3;
        public const int Adult = 4;
    }

    public static class ChildrenUtility
    {
        public static readonly HashSet<String> SupportAlienRaces = new HashSet<string>(DefDatabase<SupportAlienDef>.GetNamed("makeAlienChild").supportAlienRaces);
        public static HashSet<String> CurrentAlienRaces = new HashSet<string>();

        // Returns the maximum possible mass of a weapon the specified child can use
        public static float ChildMaxWeaponMass(Pawn pawn)
        {
            if (pawn.ageTracker.CurLifeStageIndex >= AgeStage.Teenager)
                return 999;
            // const float baseMass = 2.5f;
            return (pawn.ageTracker.AgeBiologicalYearsFloat * 0.1f) + (float)BnC_Settings.option_child_max_weapon_mass;
        }
        // Determines if a pawn is capable of currently breastfeeding
        public static bool CanBreastfeed(Pawn pawn)
        {
            return pawn.health.hediffSet.HasHediff(HediffDef.Named("Lactating"));
        }

        public static Building_Bed FindCribFor(Pawn baby, Pawn traveler)
        {
            Building_Bed crib = null;
            // Is a crib already assigned to the baby?
            if (baby.ownership != null && baby.ownership.OwnedBed != null && ChildrenUtility.IsBedCrib(baby.ownership.OwnedBed))
            {
                Building_Bed bedThing = baby.ownership.OwnedBed;
                if (RestUtility.IsValidBedFor(bedThing, baby, traveler, false, false))
                {
                    crib = baby.ownership.OwnedBed;
                }
            }
            // If not, let's look for one
            else
            {
                foreach (var thingDef in RestUtility.AllBedDefBestToWorst)
                {
                    if (RestUtility.CanUseBedEver(baby, thingDef) && thingDef.building.bed_maxBodySize <= 0.6f)
                    {
                        Building_Bed find_crib = (Building_Bed)GenClosest.ClosestThingReachable(baby.Position, baby.Map, ThingRequest.ForDef(thingDef), PathEndMode.OnCell, TraverseParms.For(traveler), 9999f, (Thing b) => (RestUtility.IsValidBedFor(b, baby, traveler, false, false)), null);
                        if (find_crib != null) crib = find_crib;
                    }
                }
            }
            return crib;
        }

        public static bool IsBedCrib(Building_Bed bed)
        {
            return (bed.def.building.bed_humanlike && bed.def.building.bed_maxBodySize <= 0.6f);
        }

        // Returns whether a race can become pregnant/have kids etc.
        public static bool RaceUsesChildren(Pawn pawn)
        {
            if (!pawn.RaceProps.Humanlike) return false;
            if (pawn.def.defName == "Human" || pawn.def.defName == "Ratkin")
            { return true; }

            // alien races
            if (CurrentAlienRaces.Contains(pawn.def.defName))
            //if (pawn.def.defName == "AFerian"
            //    || pawn.def.defName == "Alien_Callistan"
            //    || pawn.def.defName == "Alien_Cassowary"
            //    || pawn.def.defName == "Alien_Chicken"
            //    || pawn.def.defName == "Alien_Dodo"
            //    || pawn.def.defName == "Alien_Engie"
            //    || pawn.def.defName == "Alien_Equium"
            //    || pawn.def.defName == "Alien_Ferrex"
            //    || pawn.def.defName == "Alien_Gnoll"
            //    || pawn.def.defName == "Alien_Mantis"
            //    || pawn.def.defName == "Alien_NiHal"
            //    || pawn.def.defName == "Alien_Orassan"
            //    || pawn.def.defName == "Alien_Owl"
            //    || pawn.def.defName == "Alien_Parrot"
            //    || pawn.def.defName == "Alien_Penguin"
            //    || pawn.def.defName == "Alien_Racc"
            //    || pawn.def.defName == "Alien_Rockman"
            //    || pawn.def.defName == "Alien_Slug"
            //    || pawn.def.defName == "Alien_Vulture"
            //    || pawn.def.defName == "Alien_Wolvx"
            //    || pawn.def.defName == "Alien_Xenn"
            //    || pawn.def.defName == "Alien_Zabrak"
            //    || pawn.def.defName == "Alien_Zoltan"
            //    || pawn.def.defName == "Avali"
            //    || pawn.def.defName == "BearMan"
            //    || pawn.def.defName == "CamelMan"
            //    || pawn.def.defName == "ChjAndroid"
            //    || pawn.def.defName == "ElephantMan"
            //    || pawn.def.defName == "ElkMan"
            //    || pawn.def.defName == "Fantasy_Goblin"
            //    || pawn.def.defName == "FoxMan"
            //    || pawn.def.defName == "GazelleMan"
            //    || pawn.def.defName == "LynxMan"
            //    || pawn.def.defName == "LotRE_ElfStandardRace"
            //    || pawn.def.defName == "LotRH_HobbitStandardRace"
            //    || pawn.def.defName == "LotRD_DwarfStandardRace"
            //    || pawn.def.defName == "PigMan"
            //    || pawn.def.defName == "RaccoonMan"
            //    || pawn.def.defName == "WolfMan"
            //    )
            { return true; }
            return false;
        }

        public static bool IsHumanlikeChild(Pawn pawn)
        {
            if (pawn.def.defName == "Human" || pawn.def.defName == "Ratkin")
            { return true; }
            return false;
        }

        public static int Setting_Accelerated_Factor(int grown_to)
        {
            switch (grown_to)
            {
                case 0: return BnC_Settings.option_baby_accelerated_factor - 1;
                case 1: return BnC_Settings.option_toddler_accelerated_factor - 1;
                case 2: return BnC_Settings.option_child_accelerated_factor - 1;
            }
            return 1;
        }

        public static void GiveBackstory(ref Pawn pawn)
        {
            FactionDef faction = pawn.Faction.def;
            Name Namebefore = pawn.Name;
            string LastName = pawn.Name.ToStringShort;
            PawnBioAndNameGenerator.GiveAppropriateBioAndNameTo(pawn, LastName, faction);
            pawn.Name = Namebefore;
        }

        // hostile child growup
        public static void GrowupHostileChild(ref PawnGenerationRequest request, ref Pawn pawn)
        {           
            long num = (long)Rand.Range(0, 30) + 20;
            pawn.ageTracker.AgeBiologicalTicks = pawn.ageTracker.AgeBiologicalTicks + (num*3600000);
            GiveBackstory(ref pawn);

            // Update the Colonist Bar
            // PortraitsCache.SetDirty(pawn);
            //LongEventHandler.ExecuteWhenFinished(delegate
            //{
            //    pawn.Drawer.renderer.graphics.ResolveAllGraphics();
            //});
        }

        // Give innocent trait
        public static void Give_Innocent_trait(ref Pawn pawn)
        {
            int tcount = pawn.story.traits.allTraits.Count;
            if (pawn.story.traits.HasTrait(TraitDef_BnC.BleedingHeart)) return;
            if (pawn.ageTracker.CurLifeStageIndex == AgeStage.Baby && Rand.Value < 0.55f)
            {
                if (tcount >= 3) 
                { pawn.story.traits.allTraits.RemoveRange(0, tcount - 2); }
                pawn.story.traits.GainTrait(new Trait(TraitDef_BnC.Innocent));
            }
            if (pawn.ageTracker.CurLifeStageIndex == AgeStage.Child && Rand.Value < 0.35f)
            {
                if (tcount >= 3)
                { pawn.story.traits.allTraits.RemoveRange(0, tcount - 2); }
                pawn.story.traits.GainTrait(new Trait(TraitDef_BnC.Innocent));
            }
        }

        // Quick method to simply return a body part instance by a given part name
        internal static BodyPartRecord GetPawnBodyPart(Pawn pawn, String bodyPart)
        {
            return pawn.RaceProps.body.AllParts.Find(x => x.def == DefDatabase<BodyPartDef>.GetNamed(bodyPart, true));
        }
    }

    internal static class TranspilerHelper
    {

        public static T FailOnBaby<T>(this T f) where T : JobDriver
        {
            f.AddEndCondition(delegate
            {

                Pawn personDressing = f.GetActor();
                if (f.GetActor().ageTracker.CurLifeStageIndex <= 1 && f.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompBabyGear>() == null && ChildrenUtility.RaceUsesChildren(f.GetActor()))
                {
                    Messages.Message("MessageAdultClothesOnBaby".Translate(personDressing.Name.ToStringShort), personDressing, MessageTypeDefOf.CautionInput);
                    return JobCondition.Incompletable;
                }
                //tried testing for bool "isBabyGear" but that caused a null error exception, so I switched to just testing if comp was null, will look into later
                if (f.GetActor().ageTracker.CurLifeStageIndex > 1 && f.job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompBabyGear>() != null)
                {
                    Messages.Message("MessageBabyClothesOnAdult".Translate(personDressing.LabelShort), personDressing, MessageTypeDefOf.CautionInput);
                    return JobCondition.Incompletable;
                }
                else
                    return JobCondition.Ongoing;
            });
            return f;
        }

        [HarmonyPatch(typeof(JobGiver_OptimizeApparel), "TryGiveJob")]
        static class JobDriver_OptimizeApparel_Patch
        {
            [HarmonyPostfix]
            static void TryGiveJob_Patch(JobGiver_OptimizeApparel __instance, ref Job __result, Pawn pawn)
            {
                if (__result != null)
                {
                    // Stop the game from automatically allocating pawns Wear jobs they cannot fulfil
                    if ((pawn.ageTracker.CurLifeStageIndex <= AgeStage.Toddler && __result.targetA.Thing.TryGetComp<CompBabyGear>() == null && ChildrenUtility.RaceUsesChildren(pawn))
                            || (pawn.ageTracker.CurLifeStageIndex >= AgeStage.Child && __result.targetA.Thing.TryGetComp<CompBabyGear>() != null))
                    {
                        __result = null;
                    }
                }
                else
                {
                    // makes pawn to remove baby clothes when too old for them.
                    List<Apparel> wornApparel = pawn.apparel.WornApparel;
                    if (pawn.ageTracker.CurLifeStageIndex >= AgeStage.Child)
                    {
                        for (int i = wornApparel.Count - 1; i >= 0; i--)
                        {
                            CompBabyGear compBabyGear = wornApparel[i].TryGetComp<CompBabyGear>();
                            if (compBabyGear != null)
                            {
                                __result = new Job(JobDefOf.RemoveApparel, wornApparel[i])
                                {
                                    haulDroppedApparel = true
                                };
                                return;
                            }
                        }
                    }
                }
            }
        }

        internal static List<ThingStuffPair> FilterChildWeapons(Pawn pawn, List<ThingStuffPair> weapons)
        {
            var weapons_out = new List<ThingStuffPair>();
            if (weapons.Count > 0)
                foreach (ThingStuffPair weapon in weapons)
                {
                    if (weapon.thing.BaseMass < ChildrenUtility.ChildMaxWeaponMass(pawn))
                    {
                        weapons_out.Add(weapon);
                    }
                }
            return weapons_out;
        }

        //	internal static bool RecipeHasNoIngredients(RecipeDef recipe){
        //		if(recipe.ingredients.Count == 0)
        //			return true;
        //		return false;
        //	}
        //}

        // Patches pawn generation to account for the possibility of children
        [HarmonyPatch(typeof(PawnGenerator), "GeneratePawn", new[] { typeof(PawnGenerationRequest) })]
        public static class PawnGenerate_Patch
        {
            [HarmonyPostfix]
            internal static void _GeneratePawn(ref PawnGenerationRequest request, ref Pawn __result)
            {
                Pawn pawn = __result;

                if (pawn.ageTracker.CurLifeStageIndex <= AgeStage.Child && ChildrenUtility.RaceUsesChildren(pawn))
                {
                    // hostile children on/off
                    if (!BnC_Settings.option_hostile_children_raider && Faction.OfPlayerSilentFail != null)
                    {
                        if (pawn.Faction != null && pawn.HostileTo(Faction.OfPlayer))
                        {
                            Log.Message("[From BnC] hostile children growup : " + pawn.LabelIndefinite());
                            ChildrenUtility.GrowupHostileChild(ref request, ref pawn);
                            return;
                        }
                    }                   

                    // give innocence trait                    
                    ChildrenUtility.Give_Innocent_trait(ref pawn);
                    // give backstory
                    if (pawn.ageTracker.CurLifeStageIndex == 2 && pawn.ageTracker.AgeBiologicalYears < 8 && ChildrenUtility.IsHumanlikeChild(pawn))
                    {  pawn.story.childhood = BackstoryDatabase.allBackstories["CustomBackstory_NA_Childhood"];  }
                    
                    if (pawn.ageTracker.CurLifeStageIndex == AgeStage.Baby)
                    {
                        // if rjw is on return
                        if (AnotherModCheck.RJW_On) return;                        
                    }                            

                    // Children hediff being injected
                    pawn.health.AddHediff(HediffDef.Named("BabyState"), null, null);
                    Hediff_Baby babystate = (Hediff_Baby)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("BabyState"));
                    if (babystate != null)
                    {
                        for (int i = 0; i != pawn.ageTracker.CurLifeStageIndex + 1; i++)
                        { babystate.GrowUpTo(i, true); }
                    }                    
                }
            }
        }

        // Children are downed easier
        [HarmonyPatch(typeof(Pawn_HealthTracker), "ShouldBeDowned")]
        public static class Pawn_HealtherTracker_ShouldBeDowned_Patch
        {
            [HarmonyPostfix]
            internal static void SBD(ref Pawn_HealthTracker __instance, ref bool __result)
            {
                Pawn pawn = (Pawn)AccessTools.Field(typeof(Pawn_HealthTracker), "pawn").GetValue(__instance);
                if (ChildrenUtility.RaceUsesChildren(pawn) && pawn.ageTracker.CurLifeStageIndex <= 2)
                {
                    __result = __instance.hediffSet.PainTotal > 0.4f || !__instance.capacities.CanBeAwake || !__instance.capacities.CapableOf(PawnCapacityDefOf.Moving);
                }
            }
        }

        // Stops pawns from randomly dying when downed
        // This is a necessary patch for preventing children from dying on pawn generation unfortunately
        [HarmonyPatch(typeof(Pawn_HealthTracker), "CheckForStateChange")]
        public static class Pawn_HealthTracker_CheckForStateChange_Patch
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> CheckForStateChange_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> ILs = instructions.ToList();

                MethodInfo isPlayerFaction = typeof(Faction).GetProperty("IsPlayer").GetGetMethod();
                int index = ILs.FindIndex(IL => IL.opcode == OpCodes.Callvirt && IL.operand == isPlayerFaction) + 2;
                List<CodeInstruction> injection = new List<CodeInstruction> {
                new CodeInstruction(OpCodes.Br, ILs[index-1].operand),
            };
                ILs.InsertRange(index, injection);

                foreach (CodeInstruction instruction in ILs)
                {
                    yield return instruction;
                }
            }
        }

        // Babies wake up if they're unhappy
        [HarmonyPatch(typeof(RestUtility), "WakeThreshold")]
        public static class RestUtility_WakeThreshold_Patch
        {
            [HarmonyPostfix]
            internal static void WakeThreshold_Patch(ref float __result, ref Pawn p)
            {
                if (p.ageTracker.CurLifeStageIndex < AgeStage.Child && p.health.hediffSet.HasHediff(HediffDef.Named("UnhappyBaby")))
                {
                    __result = 0.15f;
                }
            }
        }

        // Causes children to drop too-heavy weapons and potentially hurt themselves on firing
        [HarmonyPatch(typeof(Verb_Shoot), "TryCastShot")]
        public static class VerbShoot_TryCastShot_Patch
        {
            [HarmonyPostfix]
            internal static void TryCastShot_Patch(ref Verb_Shoot __instance)
            {
                Pawn pawn = __instance.CasterPawn;

                if (pawn != null && ChildrenUtility.RaceUsesChildren(pawn) && pawn.ageTracker.CurLifeStageIndex <= AgeStage.Child && pawn.Faction.IsPlayer)
                {
                    // The weapon is too heavy and the child will (likely) drop it when trying to fire
                    if (__instance.EquipmentSource.def.BaseMass > ChildrenUtility.ChildMaxWeaponMass(pawn))
                    {

                        ThingWithComps benis;
                        pawn.equipment.TryDropEquipment(__instance.EquipmentSource, out benis, pawn.Position, false);

                        float recoilForce = (__instance.EquipmentSource.def.BaseMass - 2); //3

                        if (recoilForce > 0)
                        {
                            string[] hitPart = {
                            "Torso",
                            "LeftShoulder",
                            "LeftArm",
                            "LeftHand",
                            "RightShoulder",
                            "RightArm",
                            "RightHand",
                            "Head",
                            "Neck",
                            "LeftEye",
                            "RightEye",
                            "Nose",
                        };
                            int hits = Rand.Range(1, 4);
                            while (hits > 0)
                            {
                                pawn.TakeDamage(new DamageInfo(DamageDefOf.Blunt, (int)((recoilForce + Rand.Range(0f, 3f)) / hits), 0, -1, __instance.EquipmentSource,
                                    ChildrenUtility.GetPawnBodyPart(pawn, hitPart.RandomElement<String>()), null));
                                hits--;
                            }
                        }
                    }
                }
            }
        }


        // Gives a notification that the weapon a child has picked up is dangerous for them to handle
        [HarmonyPatch(typeof(Pawn_EquipmentTracker), "Notify_EquipmentAdded")]
        public static class PawnEquipmentracker_NotifyEquipmentAdded_Patch
        {
            [HarmonyPostfix]
            internal static void Notify_EquipmentAdded_Patch(ref ThingWithComps eq, ref Pawn_EquipmentTracker __instance)
            {
                Pawn pawn = __instance.ParentHolder as Pawn;
                if (pawn != null && ChildrenUtility.RaceUsesChildren(pawn) && eq.def.BaseMass > ChildrenUtility.ChildMaxWeaponMass(pawn) && pawn.ageTracker.CurLifeStageIndex <= AgeStage.Child && pawn.Faction.IsPlayer)
                {
                    Messages.Message("MessageWeaponTooLarge".Translate(eq.def.label, ((Pawn)__instance.ParentHolder).Name.ToStringShort), MessageTypeDefOf.CautionInput);
                }
            }
        }

        // Prevents children from being spawned with weapons too heavy for them
        // (For example in raids a child might otherwise spawn with an auto-shotty and promptly drop it the first time
        // they fire it at you. Which would be silly.)
        /*[HarmonyPatch(typeof(PawnWeaponGenerator), "TryGenerateWeaponFor")]
        public static class PawnWeaponGenerator_TryGenerateWeaponFor_Patch
        {
            [HarmonyTranspiler]
            static IEnumerable<CodeInstruction> TryGenerateWeaponFor_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> ILs = instructions.ToList ();

                int index = ILs.FindIndex (IL => IL.opcode == OpCodes.Ldfld && IL.operand.ToStringSafe().Contains("Pawn_EquipmentTracker")) - 1;

                MethodInfo giveChildWeapons = typeof(TranspilerHelper).GetMethod("FilterChildWeapons", AccessTools.all);
                var injection = new List<CodeInstruction> {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_2),
                    new CodeInstruction(OpCodes.Callvirt, giveChildWeapons),
                    new CodeInstruction(OpCodes.Stloc_2),
                };
                ILs.InsertRange (index, injection);

                foreach (CodeInstruction instruction in ILs) {
                    yield return instruction;
                }
            }
        }*/

        /*	// Fixes null reference exception error if a Bill_Medical has no actual ingredients
            // Remove this code when it is fixed in Vanilla
            [HarmonyPatch(typeof(Bill_Medical), "Notify_DoBillStarted")]
            public static class Bill_Medical_NotifyDoBillStarted_Patch
            {
                [HarmonyTranspiler]
                static IEnumerable<CodeInstruction> Notify_DoBillStarted_Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator ilgen)
                {
                    List<CodeInstruction> ILs = instructions.ToList ();
                    Label jumpToEnd = ILs[ILs.FindIndex(x => x.opcode == OpCodes.Ret)].labels[0];
                    // Add the "jumpToEnd" label onto the last instruction

                    int index = ILs.FindIndex (IL => IL.opcode == OpCodes.Brtrue) + 1;
                    var injection = new List<CodeInstruction> {
                        new CodeInstruction(OpCodes.Ldarg_0),
                        new CodeInstruction(OpCodes.Ldfld, typeof(Bill_Medical).GetField("recipe", AccessTools.all)),
                        new CodeInstruction(OpCodes.Call, typeof(TranspilerHelper).GetMethod("RecipeHasNoIngredients", AccessTools.all)),
                        new CodeInstruction(OpCodes.Brtrue, jumpToEnd),
                    };
                    ILs.InsertRange (index, injection);

                    foreach (CodeInstruction instruction in ILs) {
                        yield return instruction;
                    }
                }
            }
            */

        // Babies and toddlers should not develop a tolerance for social joy since this is currently their only available joy source
        // This can be removed if other joy sources (toys) are added
        [HarmonyPatch(typeof(Need_Joy), "GainJoy")]
        public static class NeedJoy_GainJoy_Patch
        {
            [HarmonyPostfix]
            internal static void GainJoy_Patch(Need_Joy __instance, JoyKindDef joyKind, Pawn ___pawn)
            {
                if (joyKind == JoyKindDefOf.Social && ___pawn.ageTracker.CurLifeStageIndex <= AgeStage.Toddler)
                {
                    // Unlike NeedJoy.GainJoy, Notify_JoyGained does not check that amount > 0
                    // We can abuse this to keep babies' tolerance for social joy at zero.
                    __instance.tolerances.Notify_JoyGained(__instance.tolerances[joyKind] * 0.65f * -1, JoyKindDefOf.Social);
                }
            }
        }

    }
}

