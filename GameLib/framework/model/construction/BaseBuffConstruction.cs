using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class BaseBuffConstruction : BaseConstruction
    {
        private readonly String buffId;

        public BaseBuffConstruction(String prototypeId, String id, String buffId) : base(prototypeId, id)
        {
            this.buffId = buffId;
        }

        override public void onLogicFrame()
        {
            // do nothing
        }

        override public void onClick()
        {
            if (!canUpgrade())
            {
                return;
            }
            doEnhanceBuff();
        }

        private void doEnhanceBuff()
        {
            List<ResourcePair> upgradeCostRule = upgradeComponent.upgradeCostPack.modifiedValues;
            gameContext.storageManager.modifyAllResourceNum(upgradeCostRule, false);
            saveData.level = (saveData.level + 1);
            gameContext.buffManager.addBuffAmout(buffId, 1);
            updateModifiedValues();
        }

        override public Boolean canClickEffect()
        {
            return canUpgrade();
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
