/*using RimWorld;
using System;
using Verse;
using System.Collections.Generic;
using System.Diagnostics;

namespace RimWorldChildren
{
	public class Recipe_AddHediffToPart : Recipe_Surgery
	{
		//
		// Fields
		//

		//
		// Methods
		//
		
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			
			BodyPartRecord part = pawn.RaceProps.body.corePart;
			if(recipe.appliedOnFixedBodyParts[0] != null)
				part = pawn.RaceProps.body.AllParts.Find(x => x.def == recipe.appliedOnFixedBodyParts[0]);
			if(part != null && !pawn.health.hediffSet.HasHediff(recipe.addsHediff, part, true)){
				yield return part;
			}
		}

		public override void ApplyOnPawn (Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
			}
			pawn.health.AddHediff(recipe.addsHediff, part, null);
		}
	}
}
*/