using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hundun.idleshare.gamelib
{
    public abstract class BaseAutoOutputComponent : OutputComponent
    {
        protected int autoOutputProgress = 0;

        public BaseAutoOutputComponent(BaseConstruction construction) : base(construction)
        {
        }

        public override void onSubLogicFrame()
        {
            autoOutputProgress++;
            int outputFrameCountMax = this.autoOutputSecondCountMax * construction.gameplayContext.LOGIC_FRAME_PER_SECOND;
            if (autoOutputProgress >= outputFrameCountMax)
            {
                autoOutputProgress = 0;
                tryAutoOutputOnce();
            }
        }

        private void tryAutoOutputOnce()
        {
            if (!this.canOutput())
            {
                //gameContext.frontend.log(this.id, "canOutput");
                return;
            }
            //gameContext.frontend.log(this.id, "AutoOutput");
            this.doOutput();
        }
    }
}
