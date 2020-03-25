using System;
using RimWorld;
using Verse;
using Verse.AI;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Linq;

namespace RimWorldChildren
{
	// This makes it so only children are possible candidates to sleep in a crib-style bed (1x1)
	[HarmonyPatch(typeof(Building_Bed))]
	[HarmonyPatch("AssigningCandidates", MethodType.Getter)]
    //[HarmonyPatch("AssigningCandidates", PropertyMethod.Getter)]
    public static class BedCandidateOverride
	{
		[HarmonyTranspiler]
		static IEnumerable<CodeInstruction> AssigningCandidates_Transpiler(IEnumerable<CodeInstruction> instructions)
		{
			foreach (CodeInstruction instruction in instructions) {
				if (instruction.opcode == OpCodes.Callvirt) {
					MethodInfo bedCandidates = typeof(BedPatchMethods).GetMethod ("BedCandidates");
					yield return new CodeInstruction (OpCodes.Ldarg_0);
					yield return new CodeInstruction (OpCodes.Callvirt, bedCandidates);
				}
			}
		}
	}
	// This patches the FindBedFor method to ensure adult pawns don't find cribs to sleep in
	// It also makes cribs prioritized for children and younger
	[HarmonyPatch(typeof(RestUtility), "FindBedFor", new []{typeof(Pawn), typeof(Pawn), typeof(bool),typeof(bool),typeof(bool)})]
	public static class FindBedForOverride
	{
		[HarmonyPostfix]
		internal static void FindBedFor_Patch(ref Pawn sleeper, ref Building_Bed __result, ref Pawn traveler, ref bool ignoreOtherReservations, ref bool sleeperWillBePrisoner, ref bool checkSocialProperness)
		{
			if (__result == null)
				return;
			if(sleeper.ageTracker.CurLifeStageIndex <= 2 && !__result.def.defName.Contains("Crib")){
				Pawn sleeper3 = sleeper;
				Pawn traveler3 = traveler;
				bool ignore3 = ignoreOtherReservations;
				bool sleeperPris3 = sleeperWillBePrisoner;
				bool checkProper3 = checkSocialProperness;
				Predicate<Thing> validator = delegate (Thing b) {
					bool flag;
					if (((Building_Bed)b).Medical) {
						flag = RestUtility.IsValidBedFor (b, sleeper3, traveler3, sleeperPris3, checkProper3, false, ignore3);
					}
					else {
						flag = false;
					}
					return flag;
				};
				Building_Bed crib = (Building_Bed)GenClosest.ClosestThingReachable(sleeper.Position, sleeper.Map, ThingRequest.ForDef(ThingDef.Named("Crib")), PathEndMode.OnCell,  TraverseParms.For (traveler), 9999, validator);
				//if (crib != null && sleeper.Position.DistanceTo(__result.Position) * 0.25f > sleeper.Position.DistanceTo(crib.Position))
				if (crib != null) __result = crib;
			}
		}
	}

	// Fixes vanilla A17 bug; gone in B18
	// This is just dummy code as it's no longer injected
	internal static class BedHarmonyPatches{
		internal static IEnumerable<CodeInstruction> GetFloatMenuOptions_Transpiler(IEnumerable<CodeInstruction> instructions){
			var ILs = instructions.ToList ();
			int index = ILs.FindIndex (x => x.opcode == OpCodes.Brfalse);
			List<CodeInstruction> injection = new List<CodeInstruction> {
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldfld, typeof(Building_Bed).GetNestedType("<GetFloatMenuOptions>c__Iterator2", AccessTools.all).GetField("myPawn", AccessTools.all)),
				new CodeInstruction(OpCodes.Ldarg_0),
				new CodeInstruction(OpCodes.Ldfld, typeof(Building_Bed).GetNestedType("<GetFloatMenuOptions>c__Iterator2", AccessTools.all).GetField("$this", AccessTools.all)),
				new CodeInstruction(OpCodes.Ldfld, typeof(Building_Bed).GetField("def")),
				new CodeInstruction(OpCodes.Call, typeof(RestUtility).GetMethod("CanUseBedEver")),
				new CodeInstruction(OpCodes.Brfalse, ILs[index].operand),
			};
			ILs.InsertRange (index + 1, injection);
			foreach (CodeInstruction IL in ILs) {
				yield return IL;
			}
		}
	}
	
	// Holds methods used by the above patches
	public static class BedPatchMethods
	{
		public static IEnumerable<Pawn> BedCandidates(Building_Bed bed){
			if (bed.def.defName.Contains("Crib") ){
				IEnumerable<Pawn> candidates = bed.Map.mapPawns.FreeColonists.Where (x => x.ageTracker.CurLifeStageIndex <= 2 && x.Faction == Faction.OfPlayer && ChildrenUtility.RaceUsesChildren(x));
				return candidates;
			}
			else
				return bed.Map.mapPawns.FreeHumanlikesOfFaction (Faction.OfPlayer);
		}
		public static bool IsCrib(Building_Bed bed){
			return bed.def.defName.Contains ("Crib");
		}
	}
}

