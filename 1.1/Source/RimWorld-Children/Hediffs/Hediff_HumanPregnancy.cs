//using RimWorld;
//using RimWorld.Planet;
using Verse;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;
//using System.Linq;

namespace RimWorldChildren
{
	public class Hediff_HumanPregnancy : HediffWithComps
	{
 	//	// Static Fields
	//	//
	//	private const int TicksPerDay = 60000;
	//	private const int MiscarryCheckInterval = 1000;

	//	private const float MTBMiscarryStarvingDays = 0.5f;

	//	private const float MTBMiscarryWoundedDays = 0.5f;

	//	//
	//	// Fields
	//	//
	//	public Pawn father;

	//	private bool is_discovered;

	//	//
	//	// Properties
	//	//
	//	public float GestationProgress {
	//		get {
	//			return Severity;
	//		}
	//		private set {
	//			Severity = value;
	//		}
	//	}

	//	public static Hediff_HumanPregnancy Create(Pawn mother, Pawn father)
	//	{
	//		var torso = mother.RaceProps.body.AllParts.Find(x => x.def == BodyPartDefOf.Torso);
	//		var hediff = (Hediff_HumanPregnancy) HediffMaker.MakeHediff(HediffDef.Named("HumanPregnancy"), mother, torso);
	//		hediff.is_discovered = false;

	//		// Todo: Capture genetics info about father/mother and save on hediff instead of ref. 
	//		// If father leaves or is killed, info is still accessible when child is eventually born
	//		hediff.father = father;
	//		return hediff;
	//	}

	//	private bool IsSeverelyWounded {
	//		get {
	//			float num = 0;
	//			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
	//			for (int i = 0; i < hediffs.Count; i++) {
	//				if (hediffs [i] is Hediff_Injury && !hediffs [i].IsPermanent ()) {
	//					num += hediffs [i].Severity;
	//				}
	//			}
	//			List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors ();
	//			for (int j = 0; j < missingPartsCommonAncestors.Count; j++) {
	//				if (missingPartsCommonAncestors [j].IsFresh) {
	//					num += missingPartsCommonAncestors [j].Part.def.GetMaxHealth (this.pawn);
	//				}
	//			}
	//			return num > 38 * this.pawn.RaceProps.baseHealthScale;
	//		}
	//	}

	//	//
	//	// Static Methods
	//	//

	//	public override void PostMake ()
	//	{
	//		// Ensure the hediff always applies to the torso, regardless of incorrect directive
	//		base.PostMake ();
	//		var torso = pawn.RaceProps.body.AllParts.Find (x => x.def.defName == "Torso");
	//		if (base.Part != torso)
	//			Part = torso;
	//	}


	//	public override bool Visible
	//	{
	//		get
	//		{
	//			return is_discovered;
	//		}
	//	}

	//	public void DiscoverPregnancy (){
	//		is_discovered = true;
	//		Find.LetterStack.ReceiveLetter ("WordHumanPregnancy".Translate(), "MessageIsPregnant".Translate (new object[] { pawn.LabelIndefinite () }), LetterDefOf.PositiveEvent, pawn, null);		 
	//	}

	//	static List<TraitDef> genetic_traits = new List<TraitDef> {
	//		TraitDefOf.Psychopath,
	//		TraitDefOf.Gay,
	//		TraitDefOf.Beauty,
	//		TraitDefOf.Industriousness,
	//		TraitDef.Named("TooSmart"),
	//		TraitDef.Named("Greedy"),
	//		TraitDef.Named("Jealous"),
	//		TraitDef.Named("DrugDesire"),
	//		TraitDef.Named("NaturalMood"),
	//		TraitDef.Named("Nerves"),
	//		TraitDef.Named("PsychicSensitivity")
	//	};

	//	internal static void GiveRandomBirthDefect(Pawn baby){
	//		int r = Rand.Range (1, 9);
	//		// Bad back defect
	//		if (r == 1)
	//			baby.health.AddHediff (HediffDefOf.BadBack, ChildrenUtility.GetPawnBodyPart(baby, "Spine"), null);
	//		// cataract defect
	//		if (r == 2) {
	//			int r1 = Rand.Range (1, 3);
	//			if(r1 == 1 || r1 == 3)
	//				baby.health.AddHediff (HediffDefOf.Cataract, ChildrenUtility.GetPawnBodyPart(baby, "LeftEye"), null);
	//			if(r1 == 2 || r1 == 3)
	//				baby.health.AddHediff (HediffDefOf.Cataract, ChildrenUtility.GetPawnBodyPart(baby, "RightEye"), null);
	//		}
	//		// frail defect
	//		if (r == 3)
	//			baby.health.AddHediff (HediffDefOf.Frail, null, null);
	//		if (r == 4)
	//			baby.health.AddHediff (HediffDef.Named ("HearingLoss"), ChildrenUtility.GetPawnBodyPart(baby, "Head"), null);
	//		if (r == 5) {
	//			baby.health.AddHediff (HediffDef.Named ("DefectBlind"), ChildrenUtility.GetPawnBodyPart(baby, "LeftEye"), null);
	//			baby.health.AddHediff (HediffDef.Named ("DefectBlind"), ChildrenUtility.GetPawnBodyPart(baby, "RightEye"), null);
	//		}
	//		if (r == 6) {
	//			baby.health.AddHediff (HediffDef.Named ("DefectHeart"), ChildrenUtility.GetPawnBodyPart (baby, "Heart"), null);
	//		}
	//		if (r == 7) {
	//			baby.health.AddHediff (HediffDef.Named ("DefectStillborn"), null, null);
	//		}
	//	}

	//	//
	//	// Methods
	//	//
	//	public void DoBirthSpawn (Pawn mother, Pawn father, float chance_successful = 1.0f)
	//	{
	//		if (mother == null) {
	//			Log.Error ("No mother defined");
	//			return;
	//		}
	//		if (father == null) {
	//			Log.Warning ("No father defined");
	//		}

	//		float birthing_quality = mother.health.hediffSet.GetFirstHediffOfDef (HediffDef.Named ("GivingBirth")).TryGetComp<HediffComp_TendDuration> ().tendQuality;

	//		mother.health.AddHediff (HediffDef.Named ("PostPregnancy"), null, null);
	//		mother.health.AddHediff (HediffDef.Named ("Lactating"), ChildrenUtility.GetPawnBodyPart(pawn, "Torso"), null);

	//		int num = (mother.RaceProps.litterSizeCurve == null) ? 1 : Mathf.RoundToInt (Rand.ByCurve (mother.RaceProps.litterSizeCurve));
	//		if (num < 1) {
	//			num = 1;
	//		}

	//		// Make sure the pawn looks like mommy and daddy
	//		float skin_whiteness = Rand.Range(0,1);
	//		// Pool of "genetic traits" the baby can inherit
	//		List<Trait> traitpool = new List<Trait>();

	//		if (mother.RaceProps.Humanlike) {
	//			// Add mom's traits to the pool
	//			foreach (Trait momtrait in mother.story.traits.allTraits) { traitpool.Add (momtrait); }
	//			if (father != null) {
	//				// Add dad's traits to the pool
	//				foreach (Trait dadtrait in father.story.traits.allTraits) { traitpool.Add (dadtrait); }
	//				// Blend skin colour between mom and dad
	//				skin_whiteness = Rand.Range (mother.story.melanin, father.story.melanin);
	//			}
	//			else {
	//				// If dad doesn't exist, just use mom's skin colour
	//				skin_whiteness = mother.story.melanin;
	//			}

	//			// Clear out any traits that aren't genetic from the list 
	//			if (traitpool.Count > 0) {
	//				foreach (Trait trait in traitpool.ToArray()) {
	//					bool is_genetic = false;
	//					foreach (TraitDef gentrait in genetic_traits) {
	//						if (gentrait.defName == trait.def.defName){
	//							is_genetic = true;
	//						} 
	//					}
	//					if (!is_genetic) {
	//						traitpool.Remove (trait);
	//					}
	//				}
	//			}
	//		}

	//		// Todo: Perhaps add a way to pass on the parent's body build
	//		// Best way to do it might be to represent thin/fat/normal/hulk
	//		// as a pair of two values, strength and weight
	//		// For example, if the mother has an average body type, she would
	//		// have a strength of .5f and a weight of .5f. A fat pawn would have
	//		// a strength of .5f and a weight of .75f. A thin pawn would have a
	//		// strength of .25f and a weight of .25f. A hulk pawn would have 
	//		// strength of .75f and weight of .75f
	//		//List<float> strength_pool = new List<float>();
	//		//List<float> weight_pool = new List<float>();

	//		//// Get mother and fathers info here if possible

	//		//float avg_strength = strength_pool.Average();
	//		//float avg_weight = weight_pool.Average();

	//		// Surname passing
	//		string last_name = null;
	//		if (mother.RaceProps.Humanlike) {
	//			if (father == null) {
	//				last_name = NameTriple.FromString (mother.Name.ToStringFull).Last;
	//			} 
	//			else {
	//				last_name = NameTriple.FromString (father.Name.ToStringFull).Last;
	//			}
	//			//Log.Message ("Debug: Newborn is born to the " + last_name + " family.");
	//		}

	//		//A16//PawnGenerationRequest request = new PawnGenerationRequest (mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, mother.Map, false, true, false, false, true, false, 1, false, true, true, null, 0, 0, null, skin_whiteness, last_name);
	//		//A17//PawnGenerationRequest request = new PawnGenerationRequest (mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, mother.Map.Tile, false, true, false, false, false, false, 1, false, true, true, false, false, null, 0, 0, null, skin_whiteness, last_name);
	//		//A18//PawnGenerationRequest request = new PawnGenerationRequest (mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, mother.Map.Tile, true, true, false, false, false, false, 0f, false, true, false, false, false, false, false, null, 0f, 0f, 0f, default(Gender?), skin_whiteness, last_name);
	//		PawnGenerationRequest request = new PawnGenerationRequest (mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, mother.Map.Tile, true, true, false, false, false, false, 0f, false, true, false, false, false, false, false, null, null, 0f, 0f, 0f, default(Gender?), skin_whiteness, last_name);

	//		Pawn baby = null;
	//		for (int i = 0; i < num; i++) {
	//			baby = PawnGenerator.GeneratePawn (request);
	//			if (PawnUtility.TrySpawnHatchedOrBornPawn (baby, mother)) {
	//				if (baby.playerSettings != null && mother.playerSettings != null) {
	//					baby.playerSettings.AreaRestriction = mother.playerSettings.AreaRestriction;
	//				}
	//				if (baby.RaceProps.IsFlesh) {
	//					baby.relations.AddDirectRelation (PawnRelationDefOf.Parent, mother);
	//					if (father != null) {
	//						baby.relations.AddDirectRelation (PawnRelationDefOf.Parent, father);
	//					}
	//				}

	//				// Good until otherwise proven bad
	//				bool successful_birth = true;
	//				var disabledBaby = BackstoryDatabase.allBackstories ["CustomBackstory_NA_Childhood_Disabled"];
	//				if (disabledBaby != null) {
	//					baby.story.childhood = disabledBaby;
	//				}
	//				else {
	//					Log.Error ("Couldn't find the required Backstory: CustomBackstory_NA_Childhood_Disabled!");
	//					baby.story.childhood = null;
	//				}
	//				baby.story.adulthood = null;
	//				baby.workSettings.Disable (WorkTypeDefOf.Hunting); //hushes up the "has no ranged weapon" alert
	//				// remove all traits
	//				baby.story.traits.allTraits.Clear ();

	//				// Add some genetic traits
	//				if (traitpool.Count > 0) {
	//					for(int j = 0; j != 2; j++){
	//						Trait gentrait = traitpool.RandomElement ();
	//						if(!baby.story.traits.HasTrait(gentrait.def)){
	//							baby.story.traits.GainTrait (gentrait);
	//						}
	//					}
	//				}
	//				// Move the baby in front of the mother, rather than on top
	//				if (mother.CurrentBed() != null) {
	//					baby.Position = baby.Position + new IntVec3 (0, 0, 1).RotatedBy (mother.CurrentBed().Rotation);
	//				}
	//				// else {
	//				//	 baby.Position = baby.Position + new IntVec3 (0, 0, 1).RotatedBy (mother.Rotation);
	//				// }
					
	//				// The baby died from bad chance of success
	//				if (Rand.Value > chance_successful || chance_successful == 0) {
	//					successful_birth = false;
	//				}

	//				// Birth defects via drugs or alcohol
	//				if (mother.health.hediffSet.HasHediff(HediffDef.Named("BirthDefectTracker"))){
	//					Hediff_BirthDefectTracker tracker = (Hediff_BirthDefectTracker)mother.health.hediffSet.GetFirstHediffOfDef (HediffDef.Named ("BirthDefectTracker"));
	//					// The baby died in utero from chemical affect
	//					if (tracker.stillbirth){
	//						successful_birth = false;
	//					}
	//					// The baby lived! So far, anyways
	//					else {
	//						// Should the baby get fetal alcohol syndrome?
	//						if (tracker.fetal_alcohol) {
	//							baby.health.AddHediff (HediffDef.Named ("FetalAlcoholSyndrome"));
	//						}
	//						// If the mother got high while pregnant, crongrats retard
	//						// now your baby is addicted to crack
	//						if (tracker.drug_addictions.Count > 0) {
	//							foreach (HediffDef addiction in tracker.drug_addictions) {
	//								baby.health.AddHediff (addiction, null, null);
	//							}
	//						}
	//					}
	//				}
					
	//				// Inbred?
	//				if (father != null && mother.relations.FamilyByBlood.Contains<Pawn> (father)) {
	//					// 50% chance to get a birth defect from inbreeding
	//					if (Rand.Range(0,1) == 1){
	//						GiveRandomBirthDefect (baby);
	//						if (baby.health.hediffSet.HasHediff (HediffDef.Named ("DefectStillborn"))) {
	//							successful_birth = false;
	//						}
	//					}
	//				}

					

	//				// The baby was born! Yay!
	//				if (successful_birth == true) {
	//					// Give father a happy memory if the birth was successful and he's not dead
	//					if (father != null && !father.health.Dead) {
	//						// The father is happy the baby was born
	//						father.needs.mood.thoughts.memories.TryGainMemory(ThoughtDef.Named("PartnerGaveBirth"));
	//					}

	//					// Send a message that the baby was born
	//					if (mother.health.hediffSet.GetFirstHediffOfDef (HediffDef.Named ("HumanPregnancy")).Visible && PawnUtility.ShouldSendNotificationAbout (mother)) {
	//						//Messages.Message ("MessageGaveBirth".Translate (new object[] {mother.LabelIndefinite ()}).CapitalizeFirst (), mother, MessageSound.Benefit);
	//						Find.LetterStack.ReceiveLetter ("LabelGaveBirth".Translate (new object[] { baby.LabelIndefinite () }), "MessageHumanBirth".Translate (new object[] {
	//							mother.LabelIndefinite (),
	//							baby.Name.ToStringShort
	//						}), LetterDefOf.PositiveEvent, baby, null);
	//					}

	//					// Try to give PPD. If not, give "New baby" thought
	//					float chance = 0.2f;
	//					if (mother.story.traits.HasTrait (TraitDefOf.Psychopath)) {
	//						   chance -= 1;
	//					}
	//					if (mother.story.traits.HasTrait (TraitDef.Named ("Nerves"))) {
	//						chance -= 0.2f * mother.story.traits.GetTrait (TraitDef.Named ("Nerves")).Degree;
	//					} else if (mother.story.traits.HasTrait (TraitDef.Named ("NaturalMood"))) {
	//						if (mother.story.traits.GetTrait (TraitDef.Named ("NaturalMood")).Degree == 2) {
	//							chance -= 1;
	//						}
	//						if (mother.story.traits.GetTrait (TraitDef.Named ("NaturalMood")).Degree == -2) {
	//							chance += 0.6f;
	//						}
	//						// For some reason this is broken
	//						/*} else if (mother.story.traits.HasTrait (TraitDef.Named ("Neurotic"))) {
	//						if (mother.story.traits.GetTrait (TraitDef.Named ("Neurotic")).Degree == 1) {
	//							chance += 0.2f;
	//						} else
	//							chance += 0.4f;*/ /*
	//					}

	//					// Because for whatever dumb reason the Math class doesn't have a Clamp method
	//					if (chance < 0) {
	//						chance = 0;
	//					}

	//					if (chance > 1) {
	//						chance = 1;
	//					}
	//					Log.Message ("Debugging: Chance of PPD is " + chance * 100 + "%");


	//					// Try to give PPD
	//					if (Rand.Value < chance) {
	//						mother.needs.mood.thoughts.memories.TryGainMemory (ThoughtDef.Named ("PostPartumDepression"), null);
	//						bool verybad = false;
	//						if (mother.story.traits.HasTrait (TraitDef.Named ("NaturalMood"))) {
	//							if (mother.story.traits.GetTrait (TraitDef.Named ("NaturalMood")).Degree == -2) {
	//								verybad = true;
	//							}
	//						} else if (mother.story.traits.HasTrait (TraitDef.Named ("Neurotic"))) {
	//							if (mother.story.traits.GetTrait (TraitDef.Named ("Neurotic")).Degree == 2) {
	//								verybad = true;
	//							}
	//						}
	//						// This pawn gets an exceptionally bad case of PPD
	//						if (verybad) {
	//							foreach (Thought_Memory thought in mother.needs.mood.thoughts.memories.Memories) {
	//								if (thought.def.defName == "PostPartumDepression") {
	//									thought.SetForcedStage (thought.CurStageIndex + 1);
	//								}
	//							}
	//						}
	//					} else {
	//						// If we didn't get PPD, then the pawn gets a mood buff
	//						if (mother.health.hediffSet.HasHediff (HediffDef.Named ("GaveBirthFlag")) == false) {
	//							mother.needs.mood.thoughts.memories.TryGainMemory (ThoughtDef.Named ("IGaveBirthFirstTime"));
	//						} 
	//						else {
	//							mother.needs.mood.thoughts.memories.TryGainMemory (ThoughtDef.Named ("IGaveBirth"));
	//						}
	//					}
	//				}
	//				else
	//				{
	//					bool aborted = false;
	//					if (chance_successful < 0f)
	//					{
	//						aborted = true;
	//					}
	//					if (baby != null)
	//					{
	//						Miscarry(baby, aborted);
	//					}
	//				}
	//			}
	//			else {
	//				Find.WorldPawns.PassToWorld (baby, PawnDiscardDecideMode.Discard);
	//			}
	//		}
	//		// Post birth
	//		if (mother.Spawned) {
	//			// Spawn guck
	//			FilthMaker.MakeFilth (mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite (), 5);
	//			if (mother.caller != null) {
	//				mother.caller.DoCall ();
	//			}
	//			if(baby != null) {
	//				if (baby.caller != null) {
	//					baby.caller.DoCall ();
	//				}
	//			}

	//			Log.Message ("Birth quality = " + birthing_quality);
	//			// Possible tearing from pregnancy
	//			if (birthing_quality < 0.75f) {
	//				if (birthing_quality < Rand.Value) {
	//					// Add a tear from giving birth
	//					if (birthing_quality < Rand.Value) {
	//						mother.health.AddHediff (HediffDef.Named ("PregnancyTearMajor"), ChildrenUtility.GetPawnBodyPart (mother, "Torso"), null);
	//					}
	//					else {
	//						mother.health.AddHediff (HediffDef.Named ("PregnancyTear"), ChildrenUtility.GetPawnBodyPart (mother, "Torso"), null);
	//					}
	//				}
	//			}
	//		}
	//		pawn.health.RemoveHediff (pawn.health.hediffSet.GetFirstHediffOfDef (HediffDef.Named ("GivingBirth")));
	//		pawn.health.RemoveHediff (this);
	//	}

	//	public override string DebugString ()
	//	{
	//		StringBuilder stringBuilder = new StringBuilder ();
	//		stringBuilder.Append (base.DebugString ());
	//		stringBuilder.AppendLine ("Gestation progress: " + this.GestationProgress.ToStringPercent ());
	//		stringBuilder.AppendLine ("Time left: " + ((int)((1 - this.GestationProgress) * this.pawn.RaceProps.gestationPeriodDays * TicksPerDay)).ToStringTicksToPeriod ()); //todo: to check
	//		return stringBuilder.ToString ();
	//	}

	//	public override void ExposeData ()
	//	{
	//		base.ExposeData ();
	//		Scribe_References.Look<Pawn> (ref this.father, "father", false);
	//		Scribe_Values.Look<bool> (ref this.is_discovered, "is_discovered", false, false);
	//	}

	//	public bool IsLateTerm()
	//	{
	//		return GestationProgress > 0.75f;
	//	}

	//	public void Miscarry (bool aborted)
	//	{
	//		Miscarry (null, aborted);
	//	}
	//	public void Miscarry (Pawn baby, bool aborted)
	//	{
	//		Pawn mother = pawn;
	//		if (baby != null) {
	//			baby.Name = new NameSingle ("Unnamed", false);
	//			baby.SetFaction (null, null);
	//			if (aborted)
	//				baby.health.AddHediff (HediffDef.Named ("BabyAborted"));
	//			else
	//				baby.health.AddHediff (HediffDef.Named ("DefectStillborn"));
	//		}

	//		if (!aborted && IsLateTerm()) {
	//			if (father != null)
	//				father.needs.mood.thoughts.memories.TryGainMemory (ThoughtDef.Named ("BabyStillborn"), baby);
	//			mother.needs.mood.thoughts.memories.TryGainMemory (ThoughtDef.Named ("BabyStillborn"), baby);
	//		}

	//		// Creates a dead fetus
	//		if (IsLateTerm () && baby == null) {
	//			// TODO: Make a "dead baby" thing object which is not actually a pawn?
	//			//DoBirthSpawn (mother, father, -1);
	//		}
	//		mother.health.RemoveHediff (this);
	//	}

	//	public override void Tick ()
	//	{
	//		// Something has gone horribly wrong
	//		if (pawn.health.hediffSet.HasHediff (HediffDef.Named ("PostPregnancy"))){
	//			Log.Error ("HumanPregnancy Hediff was not properly removed when pawn " + pawn.Name.ToStringShort + " gave birth.");
	//			// delet this
	//			if (pawn.health.hediffSet.HasHediff (HediffDef.Named ("GivingBirth")))
	//				pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(HediffDef.Named("GivingBirth")));
	//			pawn.health.RemoveHediff (this);
	//		}
			
	//		this.ageTicks++;
	//		if (this.pawn.IsHashIntervalTick (1000)) {
	//			if (this.pawn.needs.food != null && this.pawn.needs.food.CurCategory == HungerCategory.Starving && Rand.MTBEventOccurs (0.5f, TicksPerDay, MiscarryCheckInterval)) {
	//				if (this.Visible && PawnUtility.ShouldSendNotificationAbout (this.pawn)) {
	//					Messages.Message ("MessageMiscarriedStarvation".Translate (new object[] {
	//						this.pawn.LabelIndefinite ()
	//					}).CapitalizeFirst (), this.pawn, MessageTypeDefOf.NegativeHealthEvent);
	//				}
	//				Miscarry (false);
	//				return;
	//			}
	//			if (this.IsSeverelyWounded && Rand.MTBEventOccurs (0.5f, TicksPerDay, MiscarryCheckInterval)) {
	//				if (this.Visible && PawnUtility.ShouldSendNotificationAbout (this.pawn)) {
	//					Messages.Message ("MessageMiscarriedPoorHealth".Translate (new object[] {
	//						this.pawn.LabelIndefinite ()
	//					}).CapitalizeFirst (), this.pawn, MessageTypeDefOf.NegativeHealthEvent);
	//				}
	//				Miscarry (false);
	//				return;
	//			}
	//		}

	//		GestationProgress += (1.0f) / (pawn.RaceProps.gestationPeriodDays * TicksPerDay);

	//		// Check if pregnancy is far enough along to "show" for the body type
	//		if (!is_discovered) {
	//			if (
	//				(pawn.story.bodyType == BodyTypeDefOf.Female && GestationProgress > 0.389f) ||
	//				(pawn.story.bodyType == BodyTypeDefOf.Thin && GestationProgress > 0.375f) ||
	//				((pawn.story.bodyType == BodyTypeDefOf.Fat || pawn.story.bodyType == BodyTypeDefOf.Hulk) &&
	//					GestationProgress > 0.50f)) {
	//				// Got the numbers by dividing the average show time (in weeks) per body type by 36
	//				// (36 weeks being the real human gestation period)
	//				// https://www.momtricks.com/pregnancy/when-do-you-start-showing-in-pregnancy/

	//				DiscoverPregnancy();
	//			}
	//		}

	//		// Final stage of pregnancy
	//		if (CurStageIndex == 3){
	//			if (!pawn.health.hediffSet.HasHediff (HediffDef.Named ("GivingBirth"))) {
	//				// Notify the player birth is near
	//				if (Visible && PawnUtility.ShouldSendNotificationAbout (pawn)) {
	//					Messages.Message ("MessageHavingContractions".Translate (new object[] {	pawn.LabelIndefinite () }).CapitalizeFirst (), pawn, MessageTypeDefOf.NeutralEvent);
	//				}
	//				// Give the mother the GivingBirth hediff
	//				pawn.health.AddHediff (HediffDef.Named ("GivingBirth"), ChildrenUtility.GetPawnBodyPart (pawn, "Torso"), null);
	//			}
	//			// We're having contractions now
	//			else {
	//				// Has the pregnancy been tended to?
	//				if (pawn.health.hediffSet.GetFirstHediffOfDef (HediffDef.Named ("GivingBirth")).IsTended ()) {
	//					// Then we get a safe pregnancy
	//					DoBirthSpawn (pawn, father, 1);
	//				}
	//				// natural birth (probably not a good idea!)
	//				else if (GestationProgress >= 1) {
	//					// Do risky pregnancy
	//					DoBirthSpawn (pawn, father, 0.9f);
	//					if (Rand.Value <= 0.1f) {
	//						pawn.health.AddHediff (HediffDef.Named ("PlacentaBleed"), ChildrenUtility.GetPawnBodyPart (pawn, "Torso"), null);
	//					}
	//				}
	//			}
	//		}


	//		/*if (this.GestationProgress >= 1) {
	//			if (this.Visible && PawnUtility.ShouldSendNotificationAbout (this.pawn)) {
	//				Messages.Message ("MessageGaveBirth".Translate (new object[] {
	//					this.pawn.LabelIndefinite ()
	//				}).CapitalizeFirst (), this.pawn, MessageSound.Benefit);
	//			}
	//			Hediff_Pregnant.DoBirthSpawn (this.pawn, this.father);
	//		}
	//	}
	}
}