//using Verse;
//using HugsLib;
//using HugsLib.Settings;
//using System;
//using UnityEngine;
//using System.Collections.Generic;

//namespace RimWorldChildren
//{
//    public class BnC_Settings : ModBase
//    {
//        public override string ModIdentifier
//        {
//            get
//            {
//                return "BabyAndChildren";
//            }
//        }

//        public override void Initialize()
//        {
//            base.Initialize();
//        }

//        public override Version GetVersion()
//        {
//            return base.GetVersion();
//        }
        
//        protected override bool HarmonyAutoPatch { get => false; }

//        public static Dictionary<ThingDef, float> RaceOriGestPeriod = new Dictionary<ThingDef, float>();

//        public static SettingHandle<bool> option_hostile_children_raider;
//        public static SettingHandle<int> option_child_max_weapon_mass;
//        public static SettingHandle<bool> option_accelerated_growth;
//        public static SettingHandle<int> option_accelerated_growth_end_age;
//        public static SettingHandle<int> option_baby_accelerated_factor;
//        public static SettingHandle<int> option_toddler_accelerated_factor;
//        public static SettingHandle<int> option_child_accelerated_factor;
//        public static SettingHandle<int> option_gestation_period_factor;

//        public static SettingHandle<float> option_texture_scale_X;
//        public static SettingHandle<float> option_texture_scale_Y;
//        public static SettingHandle<float> option_texture_offset_X;
//        public static SettingHandle<float> option_texture_offset_Y;
//        public static SettingHandle<float> option_texture_offset_westeast_X;

//        //public static SettingHandle<float> option_debug_scale_X;
//        //public static SettingHandle<float> option_debug_scale_Y;
//        //public static SettingHandle<float> option_debug_offset_X;
//        //public static SettingHandle<float> option_debug_offset_Y;
//        //public static SettingHandle<float> option_debug_loc_X;
//        //public static SettingHandle<float> option_debug_loc_Y;
//        //public static SettingHandle<float> option_debug_loc_Z;
//        //public static SettingHandle<float> option_hair_offset_Y;

//        public override void DefsLoaded()
//        {
//            ThingDef rc = DefDatabase<ThingDef>.GetNamed("Human");
//            RaceOriGestPeriod.Add(rc, rc.race.gestationPeriodDays);
//            foreach (string ar in ChildrenUtility.CurrentAlienRaces)
//            {
//                rc = DefDatabase<ThingDef>.GetNamed(ar);
//                RaceOriGestPeriod.Add(rc, rc.race.gestationPeriodDays);
//            }   

//            option_hostile_children_raider = Settings.GetHandle<bool>("hostile_children_raider", "HostileChildrenRaider".Translate(), "HostileChildrenRaider_desc".Translate(), true);
//            option_child_max_weapon_mass = Settings.GetHandle("child_max_weapon_mas", "ChildMaxWeaponMass".Translate(), "ChildMaxWeaponMass_desc".Translate(), 2, Validators.IntRangeValidator(0, 4));
//            option_child_max_weapon_mass.SpinnerIncrement = 1;
//            option_accelerated_growth = Settings.GetHandle<bool>("accelerated_growth", "AcceleratedGrowth".Translate(), "AcceleratedGrowth_desc".Translate(), true);
//            option_accelerated_growth_end_age = Settings.GetHandle("accelerated_growth_end_age", "AcceleratedGrowthEndAge".Translate(), "AcceleratedGrowthEndAge_desc".Translate(), 8, Validators.IntRangeValidator(0, 12));
//            option_accelerated_growth_end_age.SpinnerIncrement = 1;
//            option_baby_accelerated_factor = Settings.GetHandle("baby_accelerated_factor", "BabyAcceleratedFactor".Translate(), "BabyAcceleratedFactor_desc".Translate(), 4, Validators.IntRangeValidator(1, 12));
//            option_baby_accelerated_factor.SpinnerIncrement = 1;
//            option_toddler_accelerated_factor = Settings.GetHandle("toddler_accelerated_factor", "ToddlerAcceleratedFactor".Translate(), "ToddlerAcceleratedFactor_desc".Translate(), 4, Validators.IntRangeValidator(1, 12));
//            option_toddler_accelerated_factor.SpinnerIncrement = 1;
//            option_child_accelerated_factor = Settings.GetHandle("child_accelerated_factor", "ChildAcceleratedFactor".Translate(), "ChildAcceleratedFactor_desc".Translate(), 4, Validators.IntRangeValidator(1, 12));
//            option_child_accelerated_factor.SpinnerIncrement = 1;
//            option_gestation_period_factor = Settings.GetHandle("gestation_period_factor", "GestationPeriodFactor".Translate(), "GestationPeriodFactor_desc".Translate(), 100, Validators.IntRangeValidator(10, 200));
//            option_gestation_period_factor.SpinnerIncrement = 10;

//            //option_texture_scale_X = settings.GetHandle<float>("texture_scale_X", "TextureScaleX".Translate(), "TextureScaleX_desc".Translate(), 1.12f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_texture_scale_Y = settings.GetHandle<float>("texture_scale_Y", "TextureScaleY".Translate(), "TextureScaleY_desc".Translate(), 1.32f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_texture_offset_X = settings.GetHandle<float>("texture_offset_X", "TextureOffsetX".Translate(), "TextureOffsetX_desc".Translate(), -0.055f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_texture_offset_Y = settings.GetHandle<float>("texture_offset_Y", "TextureOffsetY".Translate(), "TextureOffsetY_desc".Translate(), -0.2f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_texture_offset_westeast_X = settings.GetHandle<float>("texture_offset_westeast_X", "TextureOffsetWesteast_X".Translate(), "TextureOffsetWesteast_X_desc".Translate(), -0.06f, Validators.FloatRangeValidator(-5f, 5f));

//            //option_debug_scale_X = settings.GetHandle<float>("debug_scale_X", "DebugScaleX".Translate(), "DebugScaleX_desc".Translate(), 1.225f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_debug_scale_Y = settings.GetHandle<float>("debug_scale_Y", "DebugScaleY".Translate(), "DebugScaleY_desc".Translate(), 1.225f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_debug_offset_X = settings.GetHandle<float>("debug_offset_X", "DebugOffsetX".Translate(), "DebugOffsetX_desc".Translate(), -0.105f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_debug_offset_Y = settings.GetHandle<float>("debug_offset_Y", "DebugOffsetY".Translate(), "DebugOffsetY_desc".Translate(), -0.10f, Validators.FloatRangeValidator(-5f, 5f));

//            ////option_debug_loc_X = settings.GetHandle<float>("debug_loc_X", "DebugLocX".Translate(), "DebugLocX_desc".Translate(), 0f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_debug_loc_Y = settings.GetHandle<float>("debug_loc_Y", "DebugLocY".Translate(), "DebugLocY_desc".Translate(), 0f, Validators.FloatRangeValidator(-5f, 5f));
//            //option_debug_loc_Z = settings.GetHandle<float>("debug_loc_Z", "DebugLocZ".Translate(), "DebugLocZ_desc".Translate(), 0f, Validators.FloatRangeValidator(-5f, 5f));

//            //option_hair_offset_Y = settings.GetHandle<float>("option_hair_offset_Y", "HairOffsetY".Translate(), "HairOffsetY_desc".Translate(), 0.114f, Validators.FloatRangeValidator(-5f, 5f));
//            SettingsChanged();
//        }

//        public override void SettingsChanged()
//        {
//            base.SettingsChanged();

//            ThingDef rc = DefDatabase<ThingDef>.GetNamed("Human");
//            float originday = RaceOriGestPeriod[rc];
//            //Log.Message( rc.defName + " before :" + originday);
//            rc.race.gestationPeriodDays = originday * (float)option_gestation_period_factor / 100f;
//            Mathf.Ceil(rc.race.gestationPeriodDays);
//           // Log.Message(rc.defName + " after :" + rc.race.gestationPeriodDays);

//            foreach (string ar in ChildrenUtility.CurrentAlienRaces)
//            {
//                rc = DefDatabase<ThingDef>.GetNamed(ar);
//                originday = RaceOriGestPeriod[rc];
//                //Log.Message(rc.defName + " before :" + originday);
//                rc.race.gestationPeriodDays = originday * (float)option_gestation_period_factor / 100f;
//                Mathf.Ceil(rc.race.gestationPeriodDays);
//                //Log.Message(rc.defName + " after :" + rc.race.gestationPeriodDays);
//            }

//        }

//        public override void MapLoaded(Map map)
//        {
//            base.MapLoaded(map);
//        }
//    }
//}