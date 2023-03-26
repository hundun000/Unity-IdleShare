using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace hundun.idleshare.gamelib
{
    public class BaseClickGatherConstruction : BaseConstruction
    {

        public BaseClickGatherConstruction(String prototypeId, String id
                    ) : base(prototypeId, id)
            {
        }

        override public void onClick()
        {
            if (!canClickEffect())
            {
                return;
            }
            doGather();
        }

        private void doGather()
        {
            if (outputComponent.hasCost())
            {
                gameContext.storageManager.modifyAllResourceNum(outputComponent.outputCostPack.modifiedValues, false);
            }
            gameContext.storageManager.modifyAllResourceNum(outputComponent.outputGainPack.modifiedValues, true);
        }

        override public Boolean canClickEffect()
        {
            return canOutput();
        }


        override public void onLogicFrame()
        {
            // do nothing
        }

        override public long calculateModifiedOutput(long baseValue, int level, int proficiency)
        {
            return baseValue;
        }

        override public long calculateModifiedOutputCost(long baseValue, int level, int proficiency)
        {
            return baseValue;
        }


    }
}
