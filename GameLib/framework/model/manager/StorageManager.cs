using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Assets.Scripts.Unity_IdleShare.GameLib.framework.model.manager
{
    public class StorageManager
    {
        IdleGameplayContext gameContext;


        public Dictionary<String, long> ownResoueces = new Dictionary<String, long>();

        public HashSet<String> unlockedResourceTypes = new HashSet<String>();


        private Dictionary<String, long> oneFrameDeltaResoueces = new Dictionary<String, long>();

        public StorageManager(IdleGameplayContext gameContext)
        {
            this.gameContext = gameContext;
        }

        public String getResourceDescription(String key)
        {
            long amount = getResourceNumOrZero(key);
            return key + ": " + amount;
        }

        public long getResourceNumOrZero(String key)
        {
            return ownResoueces.getOrDefault(key, 0L);
        }



        /**
         * @param plus ture: plus the map; false: minus the map;
         */
        public void modifyAllResourceNum(Dictionary<String, long> map, Boolean plus)
        {
            //Gdx.app.log(this.getClass().getSimpleName(), (plus ? "plus" : "minus") + ": " + map);
            foreach (KeyValuePair<String, long> entry in map)
            {
                unlockedResourceTypes.Add(entry.Key);
                ownResoueces.merge(entry.Key, (plus ? 1 : -1) * entry.Value, (oldValue, newValue) => oldValue + newValue);
                oneFrameDeltaResoueces.merge(entry.Key, (plus ? 1 : -1) * entry.Value, (oldValue, newValue) => oldValue + newValue);
            }
            //game.getEventManager().notifyResourceAmountChange(false);
        }

        public void modifyAllResourceNum(List<ResourcePair> packs, Boolean plus)
        {
            //Gdx.app.log(this.getClass().getSimpleName(), (plus ? "plus" : "minus") + ": " + packs);
            foreach (ResourcePair pack in packs)
            {
                unlockedResourceTypes.Add(pack.type);
                ownResoueces.merge(pack.type, (plus ? 1 : -1) * pack.amount, (oldValue, newValue) => oldValue + newValue);
                oneFrameDeltaResoueces.merge(pack.type, (plus ? 1 : -1) * pack.amount, (oldValue, newValue) => oldValue + newValue);
            }
            //game.getEventManager().notifyResourceAmountChange(false);
        }

        public Boolean isEnough(List<ResourcePair> pairs)
        {
            foreach (ResourcePair pair in pairs)
            {
                long own = this.getResourceNumOrZero(pair.type);
                if (own < pair.amount)
                {
                    return false;
                }
            }
            return true;
        }


        public void onSubLogicFrame()
        {
            // ------ frameDeltaAmountClear ------
            Dictionary<String, long> temp = new Dictionary< String, long> (oneFrameDeltaResoueces);
            oneFrameDeltaResoueces.Clear();
            //Gdx.app.log(this.getClass().getSimpleName(), "frameDeltaAmountClear: " + temp);
            gameContext.eventManager.notifyOneFrameResourceChange(temp);
        }
    }
}
