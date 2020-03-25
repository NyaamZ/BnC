/*using RimWorld;
using Verse;
using System.Collections.Generic;

namespace RimWorldChildren
{
	public class Hediff_BirthDefectTracker : Verse.HediffWithComps
	{
		//
		// Fields
		//
		
		public int tickrare = 0;

		public float fetal_alcohol_ticker = 0;
		public bool fetal_alcohol = false;
		public bool stillbirth = false;
		public List<HediffDef> drug_addictions = new List<HediffDef>{};

		//
		// Methods
		//

		public void TickRare()
		{
			// Track if the person has drunk booze
			if (pawn.health.hediffSet.HasHediff (HediffDefOf.AlcoholHigh)) {
				fetal_alcohol_ticker += pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.AlcoholHigh).Severity;
			}
			if(fetal_alcohol == false){
				if (fetal_alcohol_ticker >= 5000 && fetal_alcohol_ticker < 10000) {
					fetal_alcohol = true;
				}
				else {
					stillbirth = true;
				}
			}
			// Track drugs taken
			foreach (Hediff hediff in pawn.health.hediffSet.hediffs) {
				if (hediff.sourceHediffDef.defName.EndsWith ("High")
					&& !drug_addictions.Contains(hediff.sourceHediffDef)
					&& !hediff.sourceHediffDef.defName.Contains("SmokeLeaf"))
					drug_addictions.Add (hediff.sourceHediffDef);
			}
		}


		public override void Tick ()
		{
			// don't judge my shitty code
			tickrare++;
			if (tickrare == 60) {
				tickrare = 0;
				TickRare ();
			}
		}

		public override bool Visible {
			get { return false; }
		}
	}
}
*/