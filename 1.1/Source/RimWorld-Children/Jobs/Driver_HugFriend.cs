using System;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;

namespace RimWorldChildren
{
	// >implying any of this actually works
	public class JobGiver_HugFriend : ThinkNode_JobGiver
	{
		//
		// Methods
		//
		protected override Job TryGiveJob (Pawn pawn)
		{
			Pawn friend = null;
			List<Pawn> friends = new List<Pawn>();
			foreach(Pawn mapPawn in pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawned){
				if (!mapPawn.Downed && mapPawn.CanCasuallyInteractNow (true) && !mapPawn.Faction.HostileTo (pawn.Faction) && pawn.relations.OpinionOf (mapPawn) >= 50) {
					friends.Add (mapPawn);
				}
			};
			if (friends.Count > 0)
				friend = friends.RandomElement ();
			if (friend == null) {
				return null;
			}
			return new Job (DefDatabase<JobDef>.GetNamed("HugFriend"), friend);
		}
	}

	public class JobDriver_HugFriend : JobDriver
	{
		//
		// Static Fields
		//
		private const int hugDuration = 150;

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
			return this.pawn.Reserve(this.Victim, this.job, 1, 01, null);
		}

		public override void ExposeData ()
		{
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
				PawnUtility.ForceWait(Victim, hugDuration, Victim);
			};
			prepare.defaultCompleteMode = ToilCompleteMode.Delay;
			prepare.defaultDuration = hugDuration;
			yield return prepare;
			yield return new Toil
			{
				initAction = delegate
				{
					pawn.interactions.TryInteractWith (Victim, DefDatabase<InteractionDef>.GetNamed ("HugFriend", true));
					this.AddEndCondition (() => JobCondition.Succeeded);
				},
				defaultCompleteMode = ToilCompleteMode.Instant
			};
		}
	}
		
	public class InteractionWorker_HugFriend : InteractionWorker
	{
		public override float RandomSelectionWeight (Pawn initiator, Pawn recipient)
		{
			// The interaction is done only through the JobDriver_HugFriend
			return 0;
		}
	}

	/*float weight = 0;
	// Chance for family
	foreach (Pawn relative in initiator.relations.RelatedPawns) {
		if (relative == recipient)
			weight += 0.25f;
	}
	if (recipient.ageTracker.CurLifeStageIndex >= 3)
		weight += 0.25f;
	if (initiator.gender == Gender.Female)
		weight += 0.25f;
	if (initiator.story.traits.HasTrait (TraitDefOf.Kind))
		weight += 0.25f;
	if (initiator.story.traits.HasTrait (TraitDefOf.Abrasive))
		weight -= 1f;
	if (initiator.story.traits.HasTrait (TraitDefOf.Psychopath))
		weight -= 0.5f;
	weight *= System.Math.Min(initiator.relations.OpinionOf (recipient) / 50, 1);
	return 0.007f * weight;*/
}