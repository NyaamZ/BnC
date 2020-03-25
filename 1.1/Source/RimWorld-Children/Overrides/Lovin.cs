/*using RimWorld;
using System;
using Verse;
using Verse.AI;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;


namespace RimWorldChildren
{


	internal static class Lovin_Override
	{
		static IEnumerable<CodeInstruction> JobDriver_Lovin_M4_Transpiler(IEnumerable<CodeInstruction> instructions){
			List<CodeInstruction> ILs = instructions.ToList ();
			Type iterator = typeof(JobDriver_Lovin).GetNestedType ("<MakeNewToils>c__Iterator0", AccessTools.all);
			int injectIndex = ILs.FindIndex (IL => IL.opcode == OpCodes.Ret);
			
			List<CodeInstruction> injection = new List<CodeInstruction> {
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(iterator, "$this")), //get JobDriver_Lovin object
				new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(typeof(JobDriver_Lovin), "pawn")),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldfld, AccessTools.Field(iterator, "$this")),
				new CodeInstruction(OpCodes.Call, AccessTools.Property(typeof(JobDriver_Lovin), "Partner").GetGetMethod(true)),
				new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Lovin_Override), "TryToImpregnate")),
			};
			ILs.InsertRange (injectIndex, injection);

			foreach (CodeInstruction IL in ILs) {
				yield return IL;
			}
		}

		internal static void TryToImpregnate(Pawn initiator, Pawn partner){
            // Lesbian/gay couples. Those cases should never result in pregnancy
            if(initiator.gender == partner.gender)
                return;
            
            // One of the two is sterile, so don't continue
            foreach(Pawn pawn in new List<Pawn>{initiator,partner}){
            	if(pawn.health.hediffSet.HasHediff(HediffDef.Named("Sterile"))){
            		return;
            	}
            }

            Pawn male = initiator.gender == Gender.Male? initiator: partner;
			Pawn female = initiator.gender == Gender.Female ? initiator : partner;

			// Only humans can be impregnated for now
			if (!ChildrenUtility.RaceUsesChildren(female))
				return;

			BodyPartRecord torso = female.RaceProps.body.AllParts.Find (x => x.def == BodyPartDefOf.Torso);
			HediffDef contraceptive = HediffDef.Named ("Contraceptive");

			// Make sure the woman is not pregnanct and not using a contraceptive
			if(female.health.hediffSet.HasHediff(HediffDefOf.Pregnant, torso) || female.health.hediffSet.HasHediff(contraceptive, null) || male.health.hediffSet.HasHediff(contraceptive, null)){
				return;
			}
			// Check the pawn's age to see how likely it is she can carry a fetus
			// 25 and below is guaranteed, 50 and above is impossible, 37.5 is 50% chance
			float preg_chance = Math.Max (1 - (Math.Max (female.ageTracker.AgeBiologicalYearsFloat - 25, 0) / 25), 0) * 0.33f;
			// For debug testing
			//float preg_chance = 1;
			if (preg_chance < Rand.Value) {
				if(Prefs.DevMode) Log.Message ("Impregnation failed. Chance was " + preg_chance);
				return;
			}
			if(Prefs.DevMode) Log.Message ("Impregnation succeeded. Chance was " + preg_chance);
			// Spawn a bunch of hearts. Sharp eyed players may notice this means impregnation occurred.
			for(int i = 0; i <= 3; i++){
				MoteMaker.ThrowMetaIcon(male.Position, male.MapHeld, ThingDefOf.Mote_Heart);
				MoteMaker.ThrowMetaIcon(female.Position, male.MapHeld, ThingDefOf.Mote_Heart);
			}

			// Do the actual impregnation. We apply it to the torso because Remove_Hediff in operations doesn't work on WholeBody (null body part)
			// for whatever reason.
			female.health.AddHediff (Hediff_HumanPregnancy.Create(female, male), torso);
		}
	}

	[HarmonyPatch(typeof(JobGiver_DoLovin), "TryGiveJob")]
	public static class JobGiver_DoLovin_TryGiveJob_Patch{
		[HarmonyPostfix]
		internal static void TryGiveJob_Patch(ref Job __result, ref Pawn pawn){
			if (pawn.ageTracker != null && pawn.ageTracker.CurLifeStageIndex <= AgeStage.Child) {
				__result = null;
			}
		}
	}
}

*/