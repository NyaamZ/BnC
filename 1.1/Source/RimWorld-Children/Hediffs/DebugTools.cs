using RimWorld;
using Verse.AI;
using Verse;
using HarmonyLib;

namespace RimWorldChildren
{
	// Makes the pawn do Lovin' if they're in bed with their partner
	public class Hediff_GetFuckin : HediffWithComps
	{
		public override void PostMake ()
		{
			base.PostMake ();
			if (pawn.InBed ()) {
				Pawn partner = LovePartnerRelationUtility.GetPartnerInMyBed (pawn);
				Building_Bed bed = pawn.CurrentBed();
				if (partner != null) {
					//pawn.mindState.awokeVoluntarily = true;
					//partner.mindState.awokeVoluntarily = true;
					Job lovin = new Job (JobDefOf.Lovin, partner, bed);
					pawn.jobs.StartJob (lovin, JobCondition.InterruptForced, null, false, true, null);
				}
			}
			pawn.health.RemoveHediff (this);
			return;
		}
	}

	// Makes the pawn pregnant (if not already) and sets the pregnancy to near its end
	/*public class Hediff_MakePregnateLate :HediffWithComps
	{
		public override void Tick ()
		{
			if (!pawn.health.hediffSet.HasHediff(HediffDef.Named("HumanPregnancy"))) {
				pawn.health.AddHediff (Hediff_HumanPregnancy.Create(pawn, null), pawn.RaceProps.body.AllParts.Find(x => x.def == BodyPartDefOf.Torso), null);
			}
			pawn.health.hediffSet.GetFirstHediffOfDef (HediffDef.Named("HumanPregnancy")).Severity = 0.995f;

			pawn.health.RemoveHediff (this);
		}
	}*/

	// Play with baby
	public class Hediff_GiveJobTest : HediffWithComps
	{
		public override void Tick ()
		{
			Pawn baby = null;
			// Try to find a baby
			foreach(Pawn colonist in pawn.Map.mapPawns.FreeColonists){
				if (colonist.ageTracker.CurLifeStageIndex == 0)
					baby = colonist;
			}
			if (baby != null) {
				Job playJob = new Job (DefDatabase<JobDef>.GetNamed ("BreastFeedBaby"), baby);
				//pawn.QueueJob (playJob);
				pawn.jobs.StartJob (playJob, JobCondition.InterruptForced, null, true, true, null, null);
				Log.Message ("Found baby " + baby.Name.ToStringShort + " and proceeding to breastfeed.");
			} else
				Log.Message ("Failed to find any baby.");

			pawn.health.RemoveHediff (this);
		}
	}
}