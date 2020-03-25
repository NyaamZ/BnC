using RimWorld;
using Verse;
using Verse.Sound;
using Verse.AI;
using System.Collections.Generic;

namespace RimWorldChildren
{
	public class Hediff_UnhappyBaby : HediffWithComps
	{
		private bool CanBabyCry(){
			if (pawn.health.capacities.CapableOf (PawnCapacityDefOf.Breathing) && pawn.health.capacities.CanBeAwake)
				return true;
			else
				return false;
		}

		public void WhineAndCry()
		{
			// Wake up baby if asleep
//			if (!pawn.Awake () && pawn.health.capacities.CanBeAwake) {
//
//			}

			// Baby isn't hungry anymore
			if (!IsBabyHungry() && !IsBabyUnhappy()) {
				pawn.health.RemoveHediff (this);
			} else if(CanBabyCry()){
				// Whine and cry
				MoteMaker.ThrowMetaIcon (pawn.Position, pawn.Map, ThingDefOf.Mote_IncapIcon);
				SoundInfo info = SoundInfo.InMap (new TargetInfo (pawn.PositionHeld, pawn.MapHeld));
				SoundDef.Named ("Pawn_BabyCry").PlayOneShot (info);
//				if (IsBabyHungry ())
//					CryForFood ();
//				else if (IsBabyUnhappy ())
//					CryForFun ();
			}
		}

		private bool IsBabyHungry(){
			if (pawn.needs.food.CurLevelPercentage < pawn.needs.food.PercentageThreshHungry)
				return true;
			return false;
		}

		private bool IsBabyUnhappy(){
			if (pawn.needs.joy.CurLevelPercentage < 0.2f)
				return true;
			return false;
		}

//		private void CryForFood(){
//			Pawn victim = null;
//			List<Pawn> viableWomen = new List<Pawn> ();
//			foreach (Pawn mapPawn in pawn.MapHeld.mapPawns.FreeColonistsSpawned) {
//				if (mapPawn.gender == Gender.Female && //Find women
//					mapPawn.ageTracker.CurLifeStage.reproductive && // of reproductive age
//				    mapPawn.PositionHeld.InHorDistOf (pawn.PositionHeld, 24) && // within earshot
//					mapPawn.relations.OpinionOf(pawn) > -10 && // that don't hate the baby
//					!mapPawn.Downed // and are capable of movement
//				){
//					viableWomen.Add (mapPawn);
//				}
//			}
//			if (viableWomen.Contains (pawn.GetMother ()))
//				victim = pawn.GetMother ();
//			else if (viableWomen.Count > 0)
//				victim = viableWomen.RandomElement ();
//
//			if(victim != null){
//				victim.jobs.StartJob (new Job (DefDatabase<JobDef>.GetNamed ("BreastFeedBaby"), pawn), JobCondition.InterruptForced, null, true);
//			}
//		}

		private void CryForFun(){
			Pawn victim = null;
			List<Pawn> viablePawns = new List<Pawn> ();
			foreach (Pawn mapPawn in pawn.MapHeld.mapPawns.FreeColonistsSpawned) {
				if (mapPawn.ageTracker.CurLifeStageIndex >= 2 &&
				    mapPawn.PositionHeld.InHorDistOf (pawn.PositionHeld, 24) && // within earshot
				    mapPawn.relations.OpinionOf (pawn) > -10 && // that don't hate the baby
				    !mapPawn.Downed // and are capable of movement
				){
					viablePawns.Add(mapPawn);
				}
			}
			if (viablePawns.Count > 0)
				victim = viablePawns.RandomElement();
			if (victim != null) {
				victim.jobs.StartJob (new Job (DefDatabase<JobDef>.GetNamed ("PlayWithBaby"), pawn));
			}
		}

		public override void PostMake ()
		{
			WhineAndCry ();
			base.PostMake ();
		}

		public override void Tick()
		{
			if (pawn.Spawned) {
				if (pawn.IsHashIntervalTick (800)) {
					LongEventHandler.ExecuteWhenFinished (delegate {
						WhineAndCry ();
					});
				}
			}
		}

		// Hide the hediff
		public override bool Visible {
			get {
				return false;
			}
		}
	}
}