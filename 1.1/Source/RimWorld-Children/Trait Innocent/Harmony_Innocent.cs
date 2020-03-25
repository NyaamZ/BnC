using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using HarmonyLib;
using System;

namespace RimWorldChildren.Optional
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LogPerformanceAttribute : System.Attribute
    {
    }

    [HarmonyPatch(typeof(ThoughtUtility), nameof(ThoughtUtility.GiveThoughtsForPawnExecuted))]
    public static class ThoughtUtility_ExecutedPatch
    {
        [LogPerformance]
        [HarmonyPostfix]
        public static void InnocentThoughts_ExecutedPatch(Pawn victim, PawnExecutionKind kind)
        {
            if (!victim.RaceProps.Humanlike)
            {
                return;
            }
            int forcedStage = 1;
            if (victim.guilt.IsGuilty)
            {
                forcedStage = 0;
            }
            else
            {
                switch (kind)
                {
                    case PawnExecutionKind.GenericBrutal:
                        forcedStage = 2;
                        break;
                    case PawnExecutionKind.GenericHumane:
                        forcedStage = 1;
                        break;
                    case PawnExecutionKind.OrganHarvesting:
                        forcedStage = 3;
                        break;
                }
            }
            ThoughtDef def;
            if (victim.IsColonist)
            {
                def = ThoudhtDef_Innocent.KnowGuestExecutedInnocent;
            }
            else
            {
                def = ThoudhtDef_Innocent.KnowGuestExecutedInnocent;
            }
            foreach (Pawn current in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                current.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(def, forcedStage), null);
            }
        }
    }

    [HarmonyPatch(typeof(ThoughtUtility), nameof(ThoughtUtility.GiveThoughtsForPawnOrganHarvested))]
    public static class ThoughtUtility_OrganHarvestedPatch
    {
        [LogPerformance]
        [HarmonyPostfix]
        public static void InnocentThoughts_OrganHarvestedPatch(Pawn victim)
        {
            if (!victim.RaceProps.Humanlike)
            {
                return;
            }
            ThoughtDef thoughtDef = null;
            if (victim.IsColonist)
            {
                thoughtDef = ThoudhtDef_Innocent.KnowColonistOrganHarvestedInnocent;
            }
            else if (victim.HostFaction == Faction.OfPlayer)
            {
                thoughtDef = ThoudhtDef_Innocent.KnowGuestOrganHarvestedInnocent;
            }
            foreach (Pawn current in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
            {
                if (current == victim)
                {
                    current.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.MyOrganHarvested, null);
                }
                else if (thoughtDef != null)
                {
                    current.needs.mood.thoughts.memories.TryGainMemory(thoughtDef, null);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Pawn), nameof(Pawn.PreTraded))]
    public static class Pawn_PreTradedPatch
    {
        [HarmonyPostfix]
        public static void InnocentThought_PreTradedPatch(Pawn __instance, TradeAction action, Pawn playerNegotiator, ITrader trader)
        {
            if (action == TradeAction.PlayerSells)
            {
                if (__instance.RaceProps.Humanlike)
                {
                    foreach (Pawn current in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
                    {
                        current.needs.mood.thoughts.memories.TryGainMemory(ThoudhtDef_Innocent.KnowPrisonerSoldInnocent, null);
                    }
                }
            }
        }
    }

    public static class DeleteInnocent
    {
        public static void RemoveInnocence(ref Pawn pawn, ref List<IndividualThoughtToAdd> outIndividualThoughts)
        {
            if (!pawn.story.traits.HasTrait(TraitDef_BnC.Innocent)) return;
            // Remove Innocence Humankills*3+2% chance                                    
            float Humankills = pawn.records.GetValue(RecordDefOf.KillsHumanlikes);
            float Trait_remove_chance = ((Humankills * 3) + 2) / 100;
            if (Rand.Value < Trait_remove_chance)
            {
                //pawn.story.traits.allTraits.RemoveRange(0, (pawn.story.traits.allTraits.Count));
                //pawn.story.traits.allTraits.Remove(new Trait(TraitDef.Named("Innocent")));
                ReplaceTraitInnocent(ref pawn);
                outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoudhtDef_Innocent.LostInnocence, pawn, null, 1f, 1f));
            }
        }

        public static void ReplaceTraitInnocent(ref Pawn pawn)
        {
            List<Trait> traitpool = new List<Trait>();
            Pawn mother = pawn.GetMother();
            Pawn father = pawn.GetFather();
            if (mother != null && mother.RaceProps.Humanlike)
            { foreach (Trait momtrait in mother.story.traits.allTraits) traitpool.Add(momtrait); }
            if (father != null && mother.RaceProps.Humanlike)
            { foreach (Trait fatrait in father.story.traits.allTraits) traitpool.Add(fatrait); }
            traitpool.Add(new Trait(TraitDef.Named("Kind")));
            traitpool.Add(new Trait(TraitDef.Named("Bloodlust")));
            traitpool.Add(new Trait(TraitDef.Named("Psychopath")));
            traitpool.Add(new Trait(TraitDef.Named("Nimble")));
            traitpool.Add(new Trait(TraitDef.Named("FastLearner")));
            traitpool.Add(new Trait(TraitDef.Named("Tough")));
            traitpool.Add(new Trait(TraitDef.Named("ShootingAccuracy"), 1));
            traitpool.Add(new Trait(TraitDef.Named("ShootingAccuracy"), -1));

            List<Trait> hadtrait = new List<Trait>();
            int trait_count_before = pawn.story.traits.allTraits.Count;

            foreach (Trait intrait in pawn.story.traits.allTraits)
            { if (intrait.def != TraitDef_BnC.Innocent) hadtrait.Add(intrait); }
            pawn.story.traits.allTraits.RemoveRange(0, (pawn.story.traits.allTraits.Count));
            foreach (Trait intoit in hadtrait) pawn.story.traits.GainTrait(intoit);

            int r;
            do
            {
                r = new Random().Next(0, traitpool.Count);
                Trait trait = traitpool[r];
                if (trait != null && !pawn.story.traits.HasTrait(trait.def) && !pawn.story.traits.allTraits.Any(x => x.def.ConflictsWith(trait)))
                {
                    pawn.story.traits.GainTrait(trait);
                }
            } while (pawn.story.traits.allTraits.Count < trait_count_before);
        }
    }

    [HarmonyPatch(typeof(PawnDiedOrDownedThoughtsUtility), "AppendThoughts_ForHumanlike")]
    public static class PawnDiedOrDownedThoughtUtility_AppendThoughtsPatch
    {
        [HarmonyPostfix]
        public static void InnocentThoughts_AppendThoughtsPatch(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, ref List<IndividualThoughtToAdd> outIndividualThoughts, ref List<ThoughtDef> outAllColonistsThoughts)
        {
            bool flag = dinfo.HasValue && dinfo.Value.Def.execution;
            bool flag2 = victim.IsPrisonerOfColony && !victim.guilt.IsGuilty && !victim.InAggroMentalState;
            bool flag3 = dinfo.HasValue && dinfo.Value.Def.ExternalViolenceFor(victim) && dinfo.Value.Instigator != null && dinfo.Value.Instigator is Pawn;
            if (flag3)
            {
                Pawn pawn = (Pawn)dinfo.Value.Instigator;
                if (!pawn.Dead && pawn.needs.mood != null && pawn.story != null && pawn != victim)
                {
                    if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.HostileTo(pawn))
                    {
                        if (victim.Faction != null && victim.Faction.HostileTo(pawn.Faction) && !flag2 && pawn.story.traits.HasTrait(TraitDef_BnC.Innocent))
                        {
                            outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoudhtDef_Innocent.KilledHumanlikeEnemyInnocent, pawn, victim, 1f, 1f));
                            //remove innocence
                            DeleteInnocent.RemoveInnocence(ref pawn, ref outIndividualThoughts);
                        }
                    }
                }
            }
            if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.Spawned)
            {
                List<Pawn> allPawnsSpawned = victim.Map.mapPawns.AllPawnsSpawned;
                for (int i = 0; i < allPawnsSpawned.Count; i++)
                {
                    Pawn pawn2 = allPawnsSpawned[i];
                    if (pawn2 != victim && pawn2.needs.mood != null)
                    {
                        if (!flag && (pawn2.MentalStateDef != MentalStateDefOf.SocialFighting || ((MentalState_SocialFighting)pawn2.MentalState).otherPawn != victim))
                        {
                            if (pawn2.Position.InHorDistOf(victim.Position, 12f) && GenSight.LineOfSight(victim.Position, pawn2.Position, victim.Map, false) && pawn2.Awake() && pawn2.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
                            {
                                if (pawn2.Faction == victim.Faction)
                                {
                                    outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoudhtDef_Innocent.WitnessedDeathAllyInnocent, pawn2, null, 1f, 1f));
                                    //remove innocence
                                    DeleteInnocent.RemoveInnocence(ref pawn2, ref outIndividualThoughts);
                                }
                                else if (victim.Faction == null || (!victim.Faction.HostileTo(pawn2.Faction) || pawn2.story.traits.HasTrait(TraitDef_BnC.Innocent)))
                                {
                                    outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoudhtDef_Innocent.WitnessedDeathNonAllyInnocent, pawn2, null, 1f, 1f));
                                    //remove innocence
                                    DeleteInnocent.RemoveInnocence(ref pawn2, ref outIndividualThoughts);
                                }

                            }
                            else if (victim.Faction == Faction.OfPlayer && victim.Faction == pawn2.Faction && victim.HostFaction != pawn2.Faction)
                            {
                                outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoudhtDef_Innocent.KnowColonistDiedInnocent, pawn2, null, 1f, 1f));
                                //remove innocence
                                DeleteInnocent.RemoveInnocence(ref pawn2, ref outIndividualThoughts);
                            }
                            if (flag2 && pawn2.Faction == Faction.OfPlayer && !pawn2.IsPrisoner)
                            {
                                outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoudhtDef_Innocent.KnowPrisonerDiedInnocentInnocent, pawn2, null, 1f, 1f));
                                //remove innocence
                                DeleteInnocent.RemoveInnocence(ref pawn2, ref outIndividualThoughts);
                            }
                        }
                    }
                }
            }
            if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && victim.IsColonist)
            {
                outAllColonistsThoughts.Add(ThoudhtDef_Innocent.ColonistAbandonedInnocent);
            }
            if (thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie)
            {
                if (victim.IsColonist)
                {
                    outAllColonistsThoughts.Add(ThoudhtDef_Innocent.ColonistAbandonedToDieInnocent);
                }
                else if (victim.IsPrisonerOfColony)
                {
                    outAllColonistsThoughts.Add(ThoudhtDef_Innocent.PrisonerAbandonedToDieInnocent);
                }
            }
        }
    }

    [HarmonyPatch(typeof(Recipe_Surgery), "CheckSurgeryFail")]
    public static class Recipe_Surgery_FailPatch
    {
        [HarmonyPostfix]
        public static void Recipe_Surgery_Fail_InnocentThought(bool __result, Pawn surgeon, Pawn patient)
        {
            if (surgeon.needs.mood != null && __result && patient.Dead)
            {
                surgeon.needs.mood.thoughts.memories.TryGainMemory(ThoudhtDef_Innocent.KilledPatientInnocent, patient);
            }
        }
    }
}