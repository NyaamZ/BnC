using System;
using Verse;
namespace RimWorldChildren.babygear
{
    public class CompBabyGear : ThingComp
    {
        public CompProperties_BabyGear Props
        {
            get
            {
                return (CompProperties_BabyGear)this.props;
            }
        }
        public CompBabyGear()
        {
        }
    }
}
