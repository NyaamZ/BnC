using RimWorld;
using Verse;
using System;
using System.Linq;

namespace RimWorldChildren
{
	public class Thought_Stillborn : Thought_Memory
	{
		public override void Init ()
		{
			base.Init ();
			DeadBabyThoughts.RemoveChildDiedThought (pawn, otherPawn);
		}
	}

	/*public class Thought_Aborted : Thought_Memory
	{
		public override void Init ()
		{
			base.Init ();
			//DeadBabyThoughts.RemoveChildDiedThought (pawn, otherPawn);
		}
	}*/

	internal static class DeadBabyThoughts {

		internal static void RemoveChildDiedThought(Pawn pawn, Pawn child){
			// Does the pawn have a "my child died thought"?
			MemoryThoughtHandler mems = pawn.needs.mood.thoughts.memories;
			if (mems.NumMemoriesOfDef (ThoughtDef.Named ("MySonDied")) > 0 || mems.NumMemoriesOfDef (ThoughtDef.Named ("MyDaughterDied")) > 0) {
				// Let's look through the list of memories
				foreach (Thought_Memory thought in mems.Memories.ToList()) {
					// Check if it's one of the right defs
					if (thought.def == ThoughtDef.Named ("MySonDied") || thought.def == ThoughtDef.Named ("MyDaughterDied")) {
						// We found the thought
						if (thought.otherPawn == child) {
							// Let's remove it
							mems.Memories.Remove (thought);
						}
					}
				}
			}
		}

	}
}