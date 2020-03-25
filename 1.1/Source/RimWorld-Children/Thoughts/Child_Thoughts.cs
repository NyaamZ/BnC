using System;
using RimWorld;
using Verse;

namespace RimWorldChildren
{
	public class ThoughtWorker_ScaredOfTheDark : ThoughtWorker_Dark
	{
		//
		// Methods
		//
		protected override ThoughtState CurrentStateInternal (Pawn p)
		{
			// Make sure it only gets applied to kids
			if (p.ageTracker.CurLifeStageIndex != AgeStage.Child)
				return false;
			// Psychopath kids doesn't afraid of anything
			if (p.story.traits.HasTrait (TraitDefOf.Psychopath))
				return false;
			return p.Awake () && p.needs.mood.recentMemory.TicksSinceLastLight > 800;
		}
	}

	public class ThoughtWorker_NearParents : ThoughtWorker
	{
		const int maxDist = 8;
		//
		// Methods
		//
		protected override ThoughtState CurrentStateInternal (Pawn p)
		{
			if (p.ageTracker.CurLifeStageIndex > AgeStage.Toddler || !ChildrenUtility.RaceUsesChildren(p))
				return false;
			Pawn mother = p.relations.GetFirstDirectRelationPawn (PawnRelationDefOf.Parent, x => x.gender == Gender.Female);
			Pawn father = p.relations.GetFirstDirectRelationPawn (PawnRelationDefOf.Parent, x => x.gender == Gender.Male);
			if (ArePawnsNear(p, mother) && ArePawnsNear(p, father)){
                return ThoughtState.ActiveAtStage(2);
            }
            else if(ArePawnsNear(p, mother))
            {
                return ThoughtState.ActiveAtStage(0);
            }
            else if(ArePawnsNear(p, father))
            {
                return ThoughtState.ActiveAtStage(1);
            }
            else
            {
                return false;
            }
		}

        protected bool ArePawnsNear(Pawn a, Pawn b)
        {
            if (a == null || b == null) return false;

            return (a.GetRoom() == b.GetRoom() && 
                a.Position.DistanceTo(b.Position) < maxDist);
        }
	}
}