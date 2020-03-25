using RimWorld;
using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using HarmonyLib;

namespace RimWorldChildren
{
	// Creates a blacklist of thoughts Toddlers cannot have
	[HarmonyPatch(typeof(ThoughtUtility), "CanGetThought")]
	public static class ThoughtUtility_CanGetThought_Patch{
		[HarmonyPostfix]
		internal static void CanGetThought_Patch(ref Pawn pawn, ref ThoughtDef def, ref bool __result)
		{
			// Toddlers and younger can't get these thoughts
			if (pawn.ageTracker.CurLifeStageIndex <= AgeStage.Toddler && ChildrenUtility.RaceUsesChildren(pawn)) {
				List<ThoughtDef> thoughtlist = new List<ThoughtDef>{
					ThoughtDefOf.AteWithoutTable,
					ThoughtDefOf.KnowPrisonerDiedInnocent,
					ThoughtDefOf.KnowPrisonerSold,
					ThoughtDefOf.Naked,
					ThoughtDefOf.SleepDisturbed,
					ThoughtDefOf.SleptOnGround,
					ThoughtDef.Named("NeedOutdoors"),
					//ThoughtDef.Named("SharedBedroom")
				};

				foreach (ThoughtDef thought in thoughtlist) {
					if (def == thought)
						__result = false;
				}
			}
		}
	}

	// Reroutes social fighting to account for children
	// I think B18 broke this somehow. ;_;
	[HarmonyPatch(typeof(JobGiver_SocialFighting), "TryGiveJob")]
	public static class JobGiver_SocialFighting_TryGiveJob_Patch
	{
		[HarmonyPostfix]
		internal static void TryGiveJob_Postfix(ref Pawn pawn, ref Job __result){
			Pawn other = ((MentalState_SocialFighting)pawn.MentalState).otherPawn;
			if (__result != null) {
				// Make sure kids don't start social fights with adults
				if (other.ageTracker.CurLifeStageIndex > 2 && pawn.ageTracker.CurLifeStageIndex <= 2) {
					Log.Message ("Debug: Child starting social fight with adult");
					// Adult will "start" the fight, following the code below
					other.interactions.StartSocialFight (pawn);
					__result = null;
				}

				// Make sure adults don't start social fights with kids (unless psychopaths)
				if (other.ageTracker.CurLifeStageIndex <= 2 && pawn.ageTracker.CurLifeStageIndex > 2 && !pawn.story.traits.HasTrait (TraitDefOf.Psychopath)) {
					//Log.Message ("Debug: Adult starting social fight with child");
					// If the pawn is not in a bad mood or is kind, they'll just tell them off
					if (pawn.story.traits.HasTrait (TraitDefOf.Kind) || pawn.needs.mood.CurInstantLevel > 0.45f || pawn.story.DisabledWorkTagsBackstoryAndTraits.HasFlag(WorkTags.Violent)) {
						//Log.Message ("Debug: Adult has decided to tell off the child");
						JobDef chastise = DefDatabase<JobDef>.GetNamed ("ScoldChild", true);
						__result = new Job (chastise, other);
					}
					// Otherwise the adult will smack the child around
					else if (other.health.summaryHealth.SummaryHealthPercent > 0.93f) {
						//Log.Message ("Debug: Adult has decided to smack the child around, child health at " + other.health.summaryHealth.SummaryHealthPercent);
						JobDef paddlin = DefDatabase<JobDef>.GetNamed ("DisciplineChild", true);
						__result = new Job (paddlin, other);
					}

					pawn.MentalState.RecoverFromState ();
					__result = null;
				}
			}
		}
	}
}

