using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hundun.idleshare.gamelib
{

    public class ConstProficiencyComponent : ProficiencyComponent
    {

        public ConstProficiencyComponent(BaseConstruction construction) : base(construction)
        {

        }

        public override void onSubLogicFrame()
        {
            // do nothing
        }

        public override void afterUpgrade()
        {
            // do nothing
        }
    }
}
