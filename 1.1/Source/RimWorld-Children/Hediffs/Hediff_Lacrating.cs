using RimWorld;
using Verse;
using HarmonyLib;
using System.Reflection;
using rjw;
using System;

namespace RimWorldChildren
{
    //public static class BNC_BodyPartHelper
    //{
    //    public static bool has_bodypart(Pawn pawn, string Bodypart)
    //    {
    //        BodyPartRecord Part = pawn.RaceProps.body.AllParts.Find(bpr => bpr.def.defName == Bodypart);
    //        if (Part is null) return false;

    //        return pawn.health.hediffSet.hediffs.Any((Hediff hed) =>
    //            (hed.Part == Part) &&
    //            (hed is Hediff_Implant || hed is Hediff_AddedPart));
    //    }
    //}

    // give mother Lacrating hediff
    [HarmonyPatch(typeof(Pawn_RelationsTracker), "AddDirectRelation")]
    static class Give_Hediff_Lacrating
    {
        private static FieldInfo PawnFI = typeof(Pawn_RelationsTracker).GetField("pawn", BindingFlags.NonPublic | BindingFlags.Instance);

        [HarmonyPostfix]
        static void AddDirectRelation_GiveLacrating(Pawn_RelationsTracker __instance, PawnRelationDef def, Pawn otherPawn)
        {
            if (def == PawnRelationDefOf.Parent )
            {
                Pawn pawn = (Pawn)PawnFI.GetValue(__instance);
                Pawn mother = pawn.GetMother();                
                //Log.Message("mother : " + pawn.LabelIndefinite());
                if (pawn.ageTracker.CurLifeStageIndex == AgeStage.Baby && mother == otherPawn)
                {                                    
                    if (mother.RaceProps.Humanlike && !mother.health.hediffSet.HasHediff(HediffDef.Named("Lactating")))
                    {
                        if (AnotherModCheck.RJW_On)
                        {
                            try
                            {
                                ((Action)(() =>
                                {
                                    if (Genital_Helper.has_breasts(mother))
                                    {
                                        mother.health.AddHediff(HediffDef.Named("Lactating"), ChildrenUtility.GetPawnBodyPart(mother, "Chest"), null);
                                    }
                                    if (Genital_Helper.has_vagina(mother))
                                    {
                                        mother.health.AddHediff(HediffDef.Named("BnC_RJW_PostPregnancy"), ChildrenUtility.GetPawnBodyPart(mother, "Genitals"), null);
                                    }
                                }))();
                            }
                            catch (TypeLoadException)
                            {
                            }
                        }
                        else
                        {
                            mother.health.AddHediff(HediffDef.Named("Lactating"), ChildrenUtility.GetPawnBodyPart(mother, "Torso"), null);
                        }
                    }
                }
            }    
        }
    }
}