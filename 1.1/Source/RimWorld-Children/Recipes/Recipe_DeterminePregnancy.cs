/*using RimWorld;
using System;
using Verse;
using System.Collections.Generic;

namespace RimWorldChildren
{
	public class Recipe_DeterminePregnancy : RecipeWorker
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
			if(part != null && ChildrenUtility.RaceUsesChildren(pawn) && pawn.gender == Gender.Female &&
			   pawn.ageTracker.CurLifeStageIndex >= AgeStage.Teenager){
				yield return part;
			}
		}

		public override void ApplyOnPawn (Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if(pawn.health.hediffSet.HasHediff(HediffDef.Named("HumanPregnancy"))){
				Hediff_HumanPregnancy preggo = (Hediff_HumanPregnancy)pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("HumanPregnancy"));
				preggo.DiscoverPregnancy();				
			}
			else{
				Messages.Message (billDoer.Name.ToStringShort + " has determined " + pawn.Name.ToStringShort + " is not pregnant.", MessageTypeDefOf.NeutralEvent);
			}
		}
		
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}
	}
}*/
