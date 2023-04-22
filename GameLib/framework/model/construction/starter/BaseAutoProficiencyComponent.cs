using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hundun.idleshare.gamelib
{

    public abstract class BaseAutoProficiencyComponent : ProficiencyComponent
    {
        protected readonly int? upgradeLostProficiency;
        protected int autoProficiencyProgress = 0;
        protected readonly int? AUTO_PROFICIENCY_SECOND_MAX; // n秒生长一次

        public BaseAutoProficiencyComponent(BaseConstruction construction, int? second, int? upgradeLostProficiency) : base(construction)
        {
            this.AUTO_PROFICIENCY_SECOND_MAX = second;
            this.upgradeLostProficiency = upgradeLostProficiency;
        }

        public override void onSubLogicFrame()
        {
            if (AUTO_PROFICIENCY_SECOND_MAX != null)
            {
                autoProficiencyProgress++;
                int proficiencyFrameCountMax = AUTO_PROFICIENCY_SECOND_MAX.Value * construction.gameplayContext.LOGIC_FRAME_PER_SECOND;
                if (autoProficiencyProgress >= proficiencyFrameCountMax)
                {
                    autoProficiencyProgress = 0;
                    tryProficiencyOnce();
                }
            }
            
        }

        protected abstract void tryProficiencyOnce();

        public override void afterUpgrade()
        {
            if (upgradeLostProficiency != null)
            {
                construction.saveData.proficiency -= this.upgradeLostProficiency.Value;
            }
        }
    }
}
