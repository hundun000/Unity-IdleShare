using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using hundun.unitygame.adapters;

namespace hundun.idleshare.gamelib
{
    public class AchievementInfoPackage
    {
        public readonly AbstractAchievement firstLockedAchievement;
        public readonly int total;
        public readonly int unLockedSize;

        public readonly List<AbstractAchievement> allAchievementList;
        public readonly HashSet<String> unlockedAchievementIds;

        public AchievementInfoPackage(AbstractAchievement firstLockedAchievement, int total, int unLockedSize,
            List<AbstractAchievement> allAchievementList, HashSet<String> unlockedAchievementIds
            )
        {
            this.firstLockedAchievement = firstLockedAchievement;
            this.total = total;
            this.unLockedSize = unLockedSize;
            this.allAchievementList = allAchievementList;
            this.unlockedAchievementIds = unlockedAchievementIds;
        }
    }

    public class AchievementManager : IBuffChangeListener, IOneFrameResourceChangeListener, IGameStartListener, IConstructionCollectionListener
    {
        IdleGameplayContext gameContext;

        Dictionary<String, AbstractAchievement> prototypes = new Dictionary<String, AbstractAchievement>();


        public HashSet<String> unlockedAchievementIds = new HashSet<String>();
        private List<String> totalAchievementIds = new List<string>();
        private List<String> achievementQueue = new List<string>();

        public AchievementManager(IdleGameplayContext gameContext)
        {
            this.gameContext = gameContext;
            gameContext.eventManager.registerListener(this);
        }

        public void addPrototype(AbstractAchievement prototype)
        {
            prototypes.Add(prototype.id, prototype);
            prototype.lazyInitDescription(gameContext);
        }


        public AchievementInfoPackage getAchievementInfoPackage()
        {
            List<AbstractAchievement> allAchievementList = achievementQueue.Select(it => prototypes.get(it)).ToList();

            AbstractAchievement firstLockedAchievement = allAchievementList
                .Where(it => !unlockedAchievementIds.Contains(it.id))
                .FirstOrDefault();
            return new AchievementInfoPackage(
                firstLockedAchievement,
                totalAchievementIds.Count,
                unlockedAchievementIds.Count,
                allAchievementList,
                unlockedAchievementIds
                );
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
            foreach (AbstractAchievement prototype in prototypes.Values)
            {
                if (unlockedAchievementIds.Contains(prototype.id))
                {
                    continue;
                }
                Boolean resourceMatched = prototype.checkUnloack();
                if (resourceMatched)
                {
                    unlockedAchievementIds.Add(prototype.id);
                    gameContext.eventManager.notifyAchievementUnlock(prototype);
                    if (prototype.awardResourceMap != null)
                    {
                        gameContext.storageManager.modifyAllResourceNum(prototype.awardResourceMap, true);
                    }
                }
            }
        }



        public void onBuffChange()
        {
            checkAllAchievementUnlock();
        }

        public void lazyInit(Dictionary<string, AbstractAchievement> achievementProviderMap, List<String> achievementPrototypeIds)
        {
            achievementPrototypeIds.ForEach(it => addPrototype(achievementProviderMap.get(it)));
            this.totalAchievementIds = achievementPrototypeIds;
            this.achievementQueue = new List<string>(achievementPrototypeIds);
        }

        public void onGameStart()
        {
            checkAllAchievementUnlock();
        }

        public void onConstructionCollectionChange()
        {
            checkAllAchievementUnlock();
        }

        void IOneFrameResourceChangeListener.onResourceChange(Dictionary<string, long> changeMap, Dictionary<string, List<long>> deltaHistoryMap)
        {
            checkAllAchievementUnlock();
        }
    }
}
