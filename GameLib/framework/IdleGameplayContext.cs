using Assets.Scripts.Unity_IdleShare.GameLib.framework.model.manager;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class IdleGameplayContext
    {
        public readonly int LOGIC_FRAME_PER_SECOND;



        public readonly IFrontend frontend;

        public readonly EventManager eventManager;
        public readonly StorageManager storageManager;
        public readonly BuffManager buffManager;
        public readonly AchievementManager achievementManager;
        public readonly BaseConstructionFactory constructionFactory;
        public readonly ConstructionManager constructionManager;
        public readonly IGameDictionary gameDictionary;
    
        public IdleGameplayContext(
                IFrontend frontEnd,
                IGameDictionary gameDictionary,
                BaseConstructionFactory constructionFactory,
                int LOGIC_FRAME_PER_SECOND, ChildGameConfig childGameConfig)
            {
                this.LOGIC_FRAME_PER_SECOND = LOGIC_FRAME_PER_SECOND;

                this.frontend = frontEnd;

                this.eventManager = new EventManager(this);
                this.storageManager = new StorageManager(this);
                this.buffManager = new BuffManager(this);
                this.achievementManager = new AchievementManager(this);
                this.constructionFactory = constructionFactory;
                this.constructionManager = new ConstructionManager(this);
                this.gameDictionary = gameDictionary;

                this.constructionFactory.lazyInit(this);
                this.constructionManager.lazyInit(childGameConfig.areaControlableConstructionIds);
                this.achievementManager.lazyInit(childGameConfig.achievementPrototypes);
            }
        }
}
