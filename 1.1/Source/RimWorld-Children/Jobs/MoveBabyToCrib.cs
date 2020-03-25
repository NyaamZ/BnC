using Verse;
using Verse.AI;
using RimWorld;

namespace RimWorldChildren
{

    public class WorkGiver_TakeBabyToCrib : WorkGiver_Scanner
    {
        //
        // Properties
        //
        public override PathEndMode PathEndMode
        {
            get
            {
                return PathEndMode.Touch;
            }
        }
        public override ThingRequest PotentialWorkThingRequest
        {
            get
            {
                return ThingRequest.ForGroup(ThingRequestGroup.Pawn);
            }
        }

        //
        // Methods
        //
        public override bool HasJobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn baby = t as Pawn;
            if (baby == null || baby == pawn)
            {
                return false;
            }
            if (!baby.RaceProps.Humanlike || baby.ageTracker.CurLifeStageIndex > AgeStage.Toddler)
            {
                return false;
            }
            if (!pawn.CanReserveAndReach(t, PathEndMode.ClosestTouch, Danger.Deadly, 1, -1, null, forced))
            {
                return false;
            }
            // Baby is in a crib already
            if (baby.InBed() && ChildrenUtility.IsBedCrib(baby.CurrentBed()))
            {
                return false;
            }
            int baby_time = GenLocalDate.HourInteger(baby.Map);
            if (baby.ageTracker.CurLifeStageIndex == AgeStage.Toddler && (baby_time > 6 && baby_time < 21))
            {
                return false;
            }
            Building_Bed crib = ChildrenUtility.FindCribFor(baby, pawn);
            if (crib == null)
            {
                JobFailReason.Is("NoCrib".Translate());
                return false;
            }
            return true;
        }

        public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
        {
            Pawn baby = (Pawn)t;
            Building_Bed crib = ChildrenUtility.FindCribFor(baby, pawn);
            if (baby != null && crib != null)
            {
                Job carryBaby = new Job(DefDatabase<JobDef>.GetNamed("TakeBabyToCrib"), baby, crib) { count = 1 };
                return carryBaby;
            }
            return null;
        }
    }
}