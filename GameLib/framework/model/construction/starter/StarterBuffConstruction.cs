using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class StarterBuffConstruction : BaseConstruction
    {
        private readonly String buffId;

        public StarterBuffConstruction(String prototypeId, String id, String buffId) : base(prototypeId, id)
        {
            this.buffId = buffId;
        }

        override public void onSubLogicFrame()
        {
            // do nothing
        }

        private void doEnhanceBuff()
        {
            List<ResourcePair> upgradeCostRule = upgradeComponent.upgradeCostPack.modifiedValues;
            gameContext.storageManager.modifyAllResourceNum(upgradeCostRule, false);
            saveData.level = (saveData.level + 1);
            gameContext.buffManager.addBuffAmout(buffId, 1);
            updateModifiedValues();
        }


        public override long calculateModifiedOutputGain(long baseValue, int level, int proficiency)
        {
            throw new NotImplementedException();
        }

        public override long calculateModifiedOutputCost(long baseValue, int level, int proficiency)
        {
            throw new NotImplementedException();
        }
    }
}
