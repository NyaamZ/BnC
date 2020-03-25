/* using RimWorld;
using Verse;
using System;
using System.Text;

namespace RimWorldChildren
{
	public class Hediff_PostPregnancy : Verse.HediffWithComps
	{
		//
		// Fields
		//
		//int age = 0;

		//
		// Static Fields
		//

		//
		// Methods
		//

		public override string DebugString ()
		{
			StringBuilder stringBuilder = new StringBuilder ();
			stringBuilder.Append (base.DebugString ());
			return stringBuilder.ToString ();
		}

		/*public override void PostMake (){

		}*/

		/*public override void Tick ()
		{
			if (Severity == 1) {
				this.pawn.health.RemoveHediff (this);
			}

			age++;
			Severity = Math.Min((age / 2500) / 24, 1);
		}*/

	/*	public override bool Visible {
			get { return true; }
		}
	}
}

*/