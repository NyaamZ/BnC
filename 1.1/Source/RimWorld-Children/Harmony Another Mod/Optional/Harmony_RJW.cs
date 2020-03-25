using System;
using RimWorld;
using Verse;
using rjw;
using HarmonyLib;
using System.Collections.Generic;

namespace RimWorldChildren.Optional
{
    [StaticConstructorOnStartup]
    internal static class Harmony_RJW
    {
        static Harmony_RJW()
        {
            Harmony harmony = new Harmony("rimworld.baby_and_children.rjw_patch");

            try
            {
                ((Action)(() =>
                {
                    if ((AccessTools.Method(
                            typeof(xxx),
                            nameof(xxx.is_human)) == null))
                    {
                        //Log.Message("[From BnC] RJW Not Detected");
                        return;
                    }

                    Log.Message("[From BnC] RimJobWorld Detected");
                    AnotherModCheck.RJW_On = true;

                    //Patch
                    harmony.Patch(
                        typeof(Hediff_SimpleBaby).GetMethod("PostMake"),                        
                        new HarmonyMethod(typeof(rjw_Patches).GetMethod(nameof(rjw_Patches.PostMake_Pre))));

                    HediffDef PostPregnancy = DefDatabase<HediffDef>.GetNamed("BnC_RJW_PostPregnancy");
                    PawnCapacityModifier cap = new PawnCapacityModifier();
                    cap.capacity = DefDatabase<PawnCapacityDef>.GetNamed("Reproduction");
                    cap.setMax = 0.1f;

                    PostPregnancy.stages[0].capMods.Add(cap);
                }))();
            }
            catch (TypeLoadException)
            {
            }
        }
    }

    public static class rjw_Patches
    {
        public static bool PostMake_Pre(Hediff __instance)
        {
            Hediff_SimpleBaby h = (Hediff_SimpleBaby)__instance;
            Pawn pawn = h.pawn;            
            pawn.health.AddHediff(HediffDef.Named("BabyState"), null, null);
            Hediff_Baby babystate = (Hediff_Baby)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("BabyState"));
            if (babystate != null)
            { babystate.GrowUpTo(0, true); }            
            pawn.health.AddHediff(HediffDef.Named("NoManipulationFlag"), null, null);
            return false;            
         }         
    }
}
