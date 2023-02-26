using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.enginecore
{
    public class FailingEntity : GameEntity
    {
        int removeY;
        int hidenFrameCount;


        public void postPrefabInitialization(int removeY, int hidenFrameCount)
        {
            this.removeY = removeY;
            this.hidenFrameCount = hidenFrameCount;
            this.hiden = hidenFrameCount > 0;
        }

        override public void frameLogic()
        {
            hidenFrameCount--;
            if (hidenFrameCount < 0)
            {
                hiden = false;
            }
        }

        override public Boolean checkRemove()
        {
            return this.y <= removeY;
        }
    }
}
