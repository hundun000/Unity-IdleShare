using hundun.unitygame.adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class BuffManager
    {
        private IdleGameplayContext gameContext;

        public Dictionary<String, int> buffAmounts = new Dictionary<String, int>();

        public BuffManager(IdleGameplayContext gameContext)
        {
            this.gameContext = gameContext;
        }

        public int getBuffAmoutOrDefault(String id)
        {
            return buffAmounts.getOrDefault(id, 0);
        }

        public void addBuffAmout(String id, int amount)
        {
            int oldValue = getBuffAmoutOrDefault(id);
            buffAmounts.put(id, oldValue + amount);
            gameContext.eventManager.notifyBuffChange();
        }


        public int modifyResourceGain(String resourceType, int oldAmout)
        {
            int newAmout = oldAmout;
            // TODO
            return newAmout;
        }
    }
}
