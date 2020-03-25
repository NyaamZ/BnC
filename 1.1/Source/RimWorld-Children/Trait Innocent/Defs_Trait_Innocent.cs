using RimWorld;

namespace RimWorldChildren
{
    [DefOf]
    public static class TraitDef_BnC
    {
        public static TraitDef Innocent = TraitDef.Named("Innocent");
        public static TraitDef BleedingHeart = TraitDef.Named("BleedingHeart");
    }

    [DefOf]
    public static class ThoudhtDef_Innocent
    {
        // Innocent thoughts
        public static ThoughtDef KnowGuestExecutedInnocent; //
        public static ThoughtDef KnowColonistExecutedInnocent; //
        public static ThoughtDef KnowGuestOrganHarvestedInnocent; //
        public static ThoughtDef KnowColonistOrganHarvestedInnocent; //
        public static ThoughtDef KnowPrisonerSoldInnocent; //
        public static ThoughtDef KnowPrisonerDiedInnocentInnocent; //
        public static ThoughtDef KnowColonistDiedInnocent; //
        public static ThoughtDef WitnessedDeathAllyInnocent; //
        public static ThoughtDef WitnessedDeathNonAllyInnocent; //
        public static ThoughtDef ColonistAbandonedInnocent; //
        public static ThoughtDef ColonistAbandonedToDieInnocent; //
        public static ThoughtDef PrisonerAbandonedToDieInnocent; //
        public static ThoughtDef KilledPatientInnocent; //

        public static ThoughtDef KilledHumanlikeEnemyInnocent; //
        public static ThoughtDef LostInnocence; //
        
    }
}