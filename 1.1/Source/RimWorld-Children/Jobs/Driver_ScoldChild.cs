using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;

namespace RimWorldChildren
{
	public class JobDriver_ScoldChild : JobDriver
	{
		//
		// Static Fields
		//
		private int scoldDuration = 300;
		private int num_times_scolded = 0;

		//
		// Properties
		//
		protected Pawn Victim {
			get {
				return (Pawn)TargetA.Thing;
			}
		}
		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Victim, this.job, 1, -1, null);
		}

		public override void ExposeData ()
		{
			Scribe_Values.Look<int> (ref num_times_scolded, "num_times_scolded", 0);
			Scribe_Values.Look<int> (ref scoldDuration, "scoldDuration", 0);
			base.ExposeData ();
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull (TargetIndex.A);
			this.FailOnDowned (TargetIndex.A);
			yield return Toils_Goto.GotoThing (TargetIndex.A, PathEndMode.Touch);
			Toil prepare = new Toil();
			prepare.initAction = delegate
			{
				scoldDuration += Rand.Range(0, 300);
				PawnUtility.ForceWait(Victim, scoldDuration, Victim);
			};
			prepare.tickAction = delegate {
				if (pawn.IsHashIntervalTick (150) && (Rand.Value > 0.5f || num_times_scolded <= 0)) {
					pawn.interactions.TryInteractWith (Victim, DefDatabase<InteractionDef>.GetNamed ("ScoldChild"));
					num_times_scolded++;
				}
			};
			prepare.defaultCompleteMode = ToilCompleteMode.Delay;
			prepare.defaultDuration = scoldDuration;
			prepare.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return prepare;
			yield return new Toil
			{
				initAction = delegate
				{
					//Victim.needs.mood.thoughts.memories.TryGainMemoryThought (ThoughtDef.Named ("GotToldOff"), pawn);
					pawn.MentalState.RecoverFromState();
					AddEndCondition (() => JobCondition.Succeeded);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
		
	public class InteractionWorker_ScoldChild : InteractionWorker
	{
		public override float RandomSelectionWeight (Pawn initiator, Pawn recipient)
		{
			//TODO: Make people who hate the kid randomly scold them
			return 0;
			/*if (initiator.ageTracker.CurLifeStageIndex > 2 && recipient.ageTracker.CurLifeStageIndex <= 2) {
				return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor (initiator, recipient);
			} else {
				return 0;
			}*/
		}
	}
}