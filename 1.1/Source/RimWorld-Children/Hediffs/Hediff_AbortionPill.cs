/*using RimWorld;
using Verse;

namespace RimWorldChildren
{
	public class Hediff_AbortionPill : HediffWithComps
	{
		//
		// Fields
		//

		int hediff_age = 0;

		//
		// Methods
		//

		public override void Tick ()
		{
			// Drug takes time to take effect
			BodyPartRecord torso = pawn.RaceProps.body.AllParts.Find (x => x.def == BodyPartDefOf.Torso);
			if (hediff_age == 2500) {
				if (pawn.health.hediffSet.HasHediff(HediffDef.Named("HumanPregnancy"), torso)) {
					Hediff preggo = pawn.health.hediffSet.GetFirstHediffOfDef (HediffDef.Named("HumanPregnancy"));
					if (preggo.Severity < 0.3) {
						pawn.health.hediffSet.hediffs.Remove (preggo);
					}
				}
			}

			if (hediff_age > 16000) {
				if (Rand.Range(1, 1000) == 1) {
					// Bleeding from taking the pill
					pawn.health.DropBloodFilth ();
				}
			}
			hediff_age += 1;
		}

		public override bool Visible {
			get { return true; }
		}
	}
}*/