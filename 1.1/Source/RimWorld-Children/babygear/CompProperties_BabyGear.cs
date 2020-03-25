using System;
using Verse;

namespace RimWorldChildren.babygear
{
    public class CompProperties_BabyGear : CompProperties
    {
        public bool isBabyGear;
        public CompProperties_BabyGear()
        {
            this.compClass = typeof(CompBabyGear);
        }
    }
}
