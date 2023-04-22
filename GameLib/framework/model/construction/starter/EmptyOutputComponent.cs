using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hundun.idleshare.gamelib
{
    public class EmptyOutputComponent : OutputComponent
    {
        public EmptyOutputComponent(BaseConstruction construction) : base(construction)
        {
        }

        public override void onSubLogicFrame()
        {

        }


        override public long calculateModifiedOutputGain(long baseValue, int level, int proficiency)
        {
            return baseValue;
        }

        override public long calculateModifiedOutputCost(long baseValue, int level, int proficiency)
        {
            return baseValue;
        }
    }
}
