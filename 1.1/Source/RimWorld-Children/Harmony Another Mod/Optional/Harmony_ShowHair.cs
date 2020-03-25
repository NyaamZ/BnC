//using System;
//using Verse;
//using UnityEngine;
//using HarmonyLib;
//using ShowHair;
//using System.Reflection;


//namespace RimWorldChildren.Harmony.Optional
//{
//    [StaticConstructorOnStartup]
//    internal static class Harmony_ShowHair
//    {
//        static Harmony_ShowHair()
//        {
//            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.baby_and_children.showhair_patch");

//            try
//            {
//                ((Action)(() =>
//                {
//                    if ((AccessTools.Method(
//                            typeof(Patch_PawnRenderer_RenderPawnInternal),
//                            nameof(Patch_PawnRenderer_RenderPawnInternal.Postfix)) == null))
//                    {
//                        Log.Message("[From BnC] Show Hair 1.0 Not Detected");
//                        return;
//                    }

//                    Log.Message("[From BnC] Show Hair 1.0 Detected");

//                    //Patch: PawnRenderer.RenderPawnInternal as Prefix
//                    harmony.Patch(typeof(PawnRenderer).GetMethod("RenderPawnInternal", BindingFlags.NonPublic | BindingFlags.Instance,
//                        Type.DefaultBinder, CallingConventions.Any,
//                        new Type[] { typeof(Vector3), typeof(Quaternion), typeof(bool), typeof(Rot4), typeof(Rot4), typeof(RotDrawMode), typeof(bool), typeof(bool) }, null),
//                        null, new HarmonyMethod(typeof(ShowHair_Patches), nameof(ShowHair_Patches.ShowHairPatches_Postfix)));


//                    //harmony.Patch(
//                    //    AccessTools.Method(
//                    //        typeof(Patch_PawnRenderer_RenderPawnInternal),
//                    //        nameof(Patch_PawnRenderer_RenderPawnInternal.Postfix)),
//                    //    new HarmonyMethod(typeof(ShowHair_Patches), nameof(ShowHair_Patches.Postfix_Prefix)));
//                }))();
//            }
//            catch (TypeLoadException)
//            {
//            }
//        }
//    }

//    public static class ShowHair_Patches
//    {
//        private static FieldInfo PawnFI = typeof(PawnRenderer).GetField("pawn", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance);

//        [HarmonyBefore(new string[] { "com.showhair.rimworld.mod" })]
//        public static void ShowHairPatches_Postfix(PawnRenderer __instance, ref Vector3 rootLoc, Quaternion quat, bool renderBody, Rot4 bodyFacing, Rot4 headFacing, RotDrawMode bodyDrawType, bool portrait, bool headStump)
//        {
//            if (__instance != null)
//            {
//                Pawn pawn = (Pawn)PawnFI.GetValue(__instance);
//                // change rootLoc for child
//                rootLoc = Children_Drawing.ModifyChildYPosOffset(rootLoc, pawn, portrait);
//            }
//        }
//    }

//}





//    //[HarmonyAfter(new string[] { "com.showhair.rimworld.mod" })]
//    //public static void ShowHairPatches_Prefix(PawnRenderer __instance, ref Vector3 rootLoc, float angle, bool renderBody, Rot4 bodyFacing, Rot4 headFacing, RotDrawMode bodyDrawType, bool portrait, bool headStump)
//    //{
//    //    Pawn pawn = PawnFI.GetValue(__instance) as Pawn;
//    //    rootLoc = Children_Drawing.ModifyChildYPosOffset(rootLoc, pawn, portrait);
//    //}

//    //    var original = AccessTools.Method(typeof(OriginalClass), "OriginalMethod", new[] { typeof(ParamClass).MakeByRefType() });
//    //    var prefix = AccessTools.Method(typeof(PatchClass), "PrefixMethod", new[] { typeof(OriginalClass), typeof(ParamClass).MakeByRefType() });
//    //if(original != null && prefix != null)
//    //    harmony.Patch(original, new HarmonyMethod(prefix), null);
