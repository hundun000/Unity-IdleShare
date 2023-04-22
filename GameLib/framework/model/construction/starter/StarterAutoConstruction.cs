﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class StarterAutoConstruction : BaseConstruction
    {
        protected int autoOutputProgress = 0;


        public StarterAutoConstruction(String prototypeId, String id
                ) : base(prototypeId, id)
        {
        }

        override protected void printDebugInfoAfterConstructed()
        {
            base.printDebugInfoAfterConstructed();
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < maxLevel; i++)
            {
                builder.Append(i).Append("->").Append(upgradeComponent.calculateCostFunction.Invoke(1L, i)).Append(",");
            }
            gameContext.frontend.log(this.id, "getUpgradeCost=[" + builder.ToString() + "]");
        }

        override public void onSubLogicFrame()
        {
            autoOutputProgress++;
            int logicFrameCountMax = outputComponent.autoOutputSecondCountMax * gameContext.LOGIC_FRAME_PER_SECOND;
            if (autoOutputProgress >= logicFrameCountMax)
            {
                autoOutputProgress = 0;
                tryAutoOutputOnce();
            }
        }

        private void tryAutoOutputOnce()
        {
            if (!canOutput())
            {
                //gameContext.frontend.log(this.id, "canOutput");
                return;
            }
            //gameContext.frontend.log(this.id, "AutoOutput");
            if (outputComponent.hasCost())
            {
                gameContext.storageManager.modifyAllResourceNum(outputComponent.outputCostPack.modifiedValues, false);
            }
            if (outputComponent.outputGainPack != null)
            {
                gameContext.storageManager.modifyAllResourceNum(outputComponent.outputGainPack.modifiedValues, true);
            }
        }

        override public long calculateModifiedOutputGain(long baseValue, int level, int proficiency)
        {
            return baseValue * level;
        }

        override public long calculateModifiedOutputCost(long baseValue, int level, int proficiency)
        {
            return baseValue * level;
        }
    }
}