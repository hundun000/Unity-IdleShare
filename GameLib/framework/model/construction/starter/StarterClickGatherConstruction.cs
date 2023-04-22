using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace hundun.idleshare.gamelib
{
    public class StarterClickGatherConstruction : BaseConstruction
    {

        public StarterClickGatherConstruction(String prototypeId, String id
                    ) : base(prototypeId, id)
            {
        }


        override public void onSubLogicFrame()
        {
            // do nothing
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
