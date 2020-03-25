using System;
using System.Collections.Generic;
using Verse;
using HarmonyLib;

namespace RimWorldChildren
{  
    public class AnotherModCheck
    {
        public static bool Alien_Races_On = false;
        public static bool BnC_Al_On = false;
        public static bool RJW_On = false;
        public static bool ShowHair_On = false;

        private static bool IsLoaded(string mod)
        {
            return LoadedModManager.RunningModsListForReading.Any(x => x.Name == mod);
        }

        public static void AnotherModPatchRun(Harmony harmony)
        {
            if (IsLoaded("[KV] Show Hair With Hats or Hide All Hats - 1.0"))
            {
                Log.Message("[From BnC] Show Hair 1.0 Detected");
                ShowHair_On = true;
            }

            if (IsLoaded("Humanoid Alien Races 2.0"))
            {
                Log.Message("[From BnC] Humanoid Alien Races 2.0 Detected");
                Alien_Races_On = true;
            }

            if (IsLoaded("BnC Alien Support"))
            {
                Log.Message("[From BnC] BnC Alien Support Detected");
                BnC_Al_On = true;
            }

            if (Alien_Races_On && !BnC_Al_On)
            {
                Log.Error("[From BnC] ** If you use Alien Race, plz use - BnC Alien Support - together.\n" +
                                                "Or If you don't use Alien Race, take it out. Now that's not necessary anymore. **");
            }
        }


        // replace RJW heddiff -> CnP heddiff
        //public static void RJW_Heddiff_Replace(Pawn pawn)
        //{
        //    if (pawn.Spawned && pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_BabyState")))
        //    {
        //        pawn.health.AddHediff(HediffDef.Named("BabyState"), null, null);
        //        pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_BabyState")));
                                
        //        Hediff_Baby babystate = (Hediff_Baby)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("BabyState"));
        //        for (int i = 0; i != pawn.ageTracker.CurLifeStageIndex + 1; i++)
        //        { babystate.GrowUpTo(i, true); }

        //        Pawn mother = pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Parent, x => x.gender == Gender.Female);
        //        if (mother != null)
        //        { mother.health.AddHediff(HediffDef.Named("Lactating"), ChildrenUtility.GetPawnBodyPart(pawn, "Chest"), null); }

        //        if (pawn.health.hediffSet.HasHediff(HediffDef.Named("RJW_NoManipulationFlag")))
        //        {
        //            pawn.health.AddHediff(HediffDef.Named("NoManipulationFlag"), null, null);
        //            pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("RJW_NoManipulationFlag")));                    
        //        }
        //    }           
        //}

    }
        
}
