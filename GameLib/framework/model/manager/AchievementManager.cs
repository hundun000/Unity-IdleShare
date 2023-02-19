using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEditor.Progress;
using Unity.VisualScripting;

namespace hundun.idleshare.gamelib
{
    public class AchievementManager : IBuffChangeListener, IOneFrameResourceChangeListener, IGameStartListener
    {
        IdleGameplayContext gameContext;

        Dictionary<String, AchievementPrototype> prototypes = new Dictionary<String, AchievementPrototype>();


        public HashSet<String> unlockedAchievementNames = new HashSet<String>();


        public AchievementManager(IdleGameplayContext gameContext)
        {
            this.gameContext = gameContext;
            gameContext.eventManager.registerListener(this);
        }

        public void addPrototype(AchievementPrototype prototype)
        {
            prototypes.Add(prototype.name, prototype);
        }

        private Boolean checkRequiredResources(Dictionary<String, int> requiredResources)
        {
            if (requiredResources == null)
            {
                return true;
            }
            foreach (KeyValuePair<String, int> entry in requiredResources)
            {
                long own = gameContext.storageManager.getResourceNumOrZero(entry.Key);
                if (own < entry.Value)
                {
                    return false;
                }
            }
            return true;
        }

        private Boolean checkRequiredBuffs(Dictionary<String, int> map)
        {
            if (map == null)
            {
                return true;
            }
            foreach (KeyValuePair<String, int> entry in map)
            {
                int own = gameContext.buffManager.getBuffAmoutOrDefault(entry.Key);
                if (own < entry.Value)
                {
                    return false;
                }
            }
            return true;
        }

        private void checkAllAchievementUnlock()
        {
            //Gdx.app.log(this.getClass().getSimpleName(), "checkAllAchievementUnlock");
            foreach (AchievementPrototype prototype in prototypes.Values)
            {
                if (unlockedAchievementNames.Contains(prototype.name))
                {
                    continue;
                }
                Boolean resourceMatched = checkRequiredResources(prototype.requiredResources);
                Boolean buffMatched = checkRequiredBuffs(prototype.requiredBuffs);
                Boolean allMatched = resourceMatched && buffMatched;
                if (allMatched)
                {
                    unlockedAchievementNames.Add(prototype.name);
                    gameContext.eventManager.notifyAchievementUnlock(prototype);
                }
            }
        }



        public void onBuffChange()
        {
            checkAllAchievementUnlock();
        }

        public void lazyInit(List<AchievementPrototype> achievementPrototypes)
        {
            achievementPrototypes.ForEach(item => addPrototype(item));
        }


        public void onResourceChange(Dictionary<String, long> changeMap)
        {
            checkAllAchievementUnlock();
        }

        public void onGameStart()
        {
            checkAllAchievementUnlock();
        }
    }
}
