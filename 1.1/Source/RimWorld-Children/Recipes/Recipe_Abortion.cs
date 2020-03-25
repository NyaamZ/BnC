/*using RimWorld;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;

namespace RimWorldChildren
{
	public class Recipe_Abortion : Recipe_RemoveHediff
	{
		public override void ApplyOnPawn (Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			// We don't check if the surgery fails, because even if the surgery is a failure the fetus will still die

			if (billDoer != null) {

				TaleRecorder.RecordTale (TaleDefOf.DidSurgery, new object[] {
					billDoer,
					pawn
				});
				//if (base.CheckSurgeryFail (billDoer, pawn, ingredients, part) == false) {
				if (base.CheckSurgeryFail (billDoer, pawn, ingredients, part, bill) == false){
					if (PawnUtility.ShouldSendNotificationAbout (pawn) || PawnUtility.ShouldSendNotificationAbout (billDoer)) {
						string text;
						if (!this.recipe.successfullyRemovedHediffMessage.NullOrEmpty ()) {
							text = string.Format (this.recipe.successfullyRemovedHediffMessage, billDoer.LabelShort, pawn.LabelShort);
						}
						else {
							text = "MessageSuccessfullyRemovedHediff".Translate (new object[] {
								billDoer.LabelShort,
								pawn.LabelShort,
								this.recipe.removesHediff.label
							});
						}
						Messages.Message (text, pawn, MessageTypeDefOf.TaskCompletion);
					}
				}
			}

			Hediff_HumanPregnancy preggo = (Hediff_HumanPregnancy)pawn.health.hediffSet.GetFirstHediffOfDef (HediffDef.Named ("HumanPregnancy"));
			if (preggo.IsLateTerm()) {
				// Give bad thoughts related to late abortions
				pawn.needs.mood.thoughts.memories.TryGainMemory (ThoughtDef.Named ("LateTermAbortion"), null);
				if (preggo.GestationProgress >= 0.97f) {

					Thought_Memory abortion_thought = pawn.needs.mood.thoughts.memories.OldestMemoryOfDef (ThoughtDef.Named ("LateTermAbortion"));
					if (abortion_thought == null) {
						Log.Error ("ChildrenMod: Failed to add late term abortion thought!");
					}
					// Very late term abortion
					abortion_thought.SetForcedStage (abortion_thought.CurStageIndex + 1);
				}

				// Give bad thoughts related to late abortions
				pawn.needs.mood.thoughts.memories.TryGainMemory (ThoughtDef.Named ("LateTermAbortion"), null);
				if (preggo.GestationProgress >= 0.97f) {

					Thought_Memory abortion_thought = pawn.needs.mood.thoughts.memories.OldestMemoryOfDef (ThoughtDef.Named ("LateTermAbortion"));
					// Very late term abortion
					abortion_thought.SetForcedStage (abortion_thought.CurStageIndex + 1);
				}
				// Remove the fetus
				preggo.Miscarry (true);
			}
			else
				pawn.health.RemoveHediff (preggo);
		}
	}
}*/