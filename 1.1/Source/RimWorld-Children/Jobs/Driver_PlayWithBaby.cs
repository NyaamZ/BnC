using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using Verse;
using Verse.AI;

namespace RimWorldChildren
{
	public class WorkGiver_PlayWithBaby : WorkGiver_Scanner
	{
		//
		// Properties
		//
		public override PathEndMode PathEndMode {
			get {
				return PathEndMode.Touch;
			}
		}
		public override ThingRequest PotentialWorkThingRequest {
			get {
				return ThingRequest.ForGroup (ThingRequestGroup.Pawn);
			}
		}

		//
		// Methods
		//
		public override bool HasJobOnThing (Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || pawn2 == pawn) {
				return false;
			}
			if (!pawn2.RaceProps.Humanlike || pawn2.ageTracker.CurLifeStageIndex > AgeStage.Toddler) {
				return false;
			}
			if (!pawn2.InBed() || !pawn2.Awake()){
				return false;
			}
			if (pawn2.needs.joy == null || pawn2.needs.joy.CurLevelPercentage > 0.9f) {
				return false;
			}
			if (!pawn.CanReserveAndReach (t, PathEndMode.ClosestTouch, Danger.None, 1, -1, null, forced)) {
				return false;
			}
			return true;
		}

		public override Job JobOnThing (Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = (Pawn)t;
			if (pawn2 != null) {
				return new Job (DefDatabase<JobDef>.GetNamed ("PlayWithBaby")) {
					targetA = pawn2,
				};
			}
			return null;
		}
	}
	
	public class JobDriver_PlayWithBaby : JobDriver
	{
		const TargetIndex BabyInd = TargetIndex.A;

		const TargetIndex ChairInd = TargetIndex.B;

		Pawn Baby
		{
			get
			{
				return (Pawn)job.GetTarget(TargetIndex.A).Thing;
			}
		}

		Thing Chair
		{
			get
			{
				return job.GetTarget(TargetIndex.B).Thing;
			}
		}
		
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Baby, this.job, 1, -1, null);
		}

		[DebuggerHidden]
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			this.FailOn (() => !Baby.InBed () || !Baby.Awake());
			if (Chair != null)
			{
				this.FailOnDespawnedNullOrForbidden(TargetIndex.B);
			}
			yield return Toils_Reserve.Reserve (TargetIndex.A);
			if (Chair != null)
			{
				yield return Toils_Reserve.Reserve (TargetIndex.B);
				yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			}
			yield return Toils_Interpersonal.WaitToBeAbleToInteract(pawn);
			yield return new Toil
			{
				tickAction = delegate
				{
					Baby.needs.joy.GainJoy(job.def.joyGainRate * 0.000144f, job.def.joyKind);
					if (pawn.IsHashIntervalTick(320))
					{
						InteractionDef intDef = (Rand.Value >= 0.8f) ? InteractionDefOf.DeepTalk : InteractionDefOf.Chitchat;
						pawn.interactions.TryInteractWith(Baby, intDef);
					}
					pawn.rotationTracker.FaceCell(Baby.Position);
					
					pawn.GainComfortFromCellIfPossible();
					JoyUtility.JoyTickCheckEnd (pawn, JoyTickFullJoyAction.None);
					if (pawn.needs.joy.CurLevelPercentage > 0.9999f && Baby.needs.joy.CurLevelPercentage > 0.9999f)
					{
						pawn.jobs.EndCurrentJob (JobCondition.Succeeded);
					}
				},
				socialMode = RandomSocialMode.Off,
				defaultCompleteMode = ToilCompleteMode.Delay,
				defaultDuration = job.def.joyDuration
			};
		}
	}
}