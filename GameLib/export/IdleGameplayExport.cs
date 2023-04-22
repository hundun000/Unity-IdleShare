using hundun.unitygame.adapters;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;


namespace hundun.idleshare.gamelib
{

    public class IdleGameplayExport : ILogicFrameListener, ISubGameplaySaveHandler<GameplaySaveData>, ISubSystemSettingSaveHandler<SystemSettingSaveData>
    {
        public IdleGameplayContext gameplayContext;
        private IBuiltinConstructionsLoader builtinConstructionsLoader;
        private IBuiltinAchievementsLoader builtinAchievementsLoader;
        private ChildGameConfig childGameConfig;
        public IGameDictionary gameDictionary;
        public Language language;
        public String stageId;

        public IdleGameplayExport(
                IFrontend frontEnd,
                IGameDictionary gameDictionary,
                IBuiltinConstructionsLoader builtinConstructionsLoader,
                IBuiltinAchievementsLoader builtinAchievementsLoader,
                int LOGIC_FRAME_PER_SECOND, ChildGameConfig childGameConfig)
        {
            this.gameDictionary = gameDictionary;
            this.childGameConfig = childGameConfig;
            this.builtinConstructionsLoader = builtinConstructionsLoader;
            this.builtinAchievementsLoader = builtinAchievementsLoader;
            this.gameplayContext = new IdleGameplayContext(frontEnd, gameDictionary, LOGIC_FRAME_PER_SECOND);
        }

        void ILogicFrameListener.onLogicFrame()
        {
            gameplayContext.constructionManager.onSubLogicFrame();
            gameplayContext.storageManager.onSubLogicFrame();
        }



        void ISubGameplaySaveHandler<GameplaySaveData>.applyGameplaySaveData(GameplaySaveData gameplaySaveData)
        {
            this.stageId = gameplaySaveData.stageId;

            gameplaySaveData.constructionSaveDataMap.Values.ToList().ForEach(it => {
                gameplayContext.constructionManager.loadInstance(it);
            });
            
            gameplayContext.storageManager.unlockedResourceTypes = (gameplaySaveData.unlockedResourceTypes);
            gameplayContext.storageManager.ownResoueces = (gameplaySaveData.ownResoueces);
            gameplayContext.achievementManager.unlockedAchievementIds = (gameplaySaveData.unlockedAchievementIds);
        }

        void ISubGameplaySaveHandler<GameplaySaveData>.currentSituationToGameplaySaveData(GameplaySaveData gameplaySaveData)
        {
            gameplaySaveData.stageId = this.stageId;

            List<BaseConstruction> constructions = gameplayContext.constructionManager.getConstructions();
            gameplaySaveData.constructionSaveDataMap = (constructions
                    .ToDictionary(
                            it => it.id,
                            it => it.saveData
                            )
                    );
            gameplaySaveData.unlockedResourceTypes = (gameplayContext.storageManager.unlockedResourceTypes);
            gameplaySaveData.ownResoueces = (gameplayContext.storageManager.ownResoueces);
            gameplaySaveData.unlockedAchievementIds = (gameplayContext.achievementManager.unlockedAchievementIds);
        }

        void ISubSystemSettingSaveHandler<SystemSettingSaveData>.applySystemSetting(SystemSettingSaveData systemSettingSave)
        {
            this.language = (systemSettingSave.language);
            gameplayContext.allLazyInit(
                    systemSettingSave.language,
                    childGameConfig,
                    builtinConstructionsLoader.getProviderMap(language),
                    builtinAchievementsLoader.getProviderMap(language)
                    );
            gameplayContext.frontend.log(this.getClass().getSimpleName(), "applySystemSetting done");
        }

        void ISubSystemSettingSaveHandler<SystemSettingSaveData>.currentSituationToSystemSetting(SystemSettingSaveData systemSettingSave)
        {
            systemSettingSave.language = (this.language);
        }



        internal GridPosition getConnectedRandonPosition()
        {
            if (gameplayContext.constructionManager.runningConstructionModelMap.Count == 0)
            {
                return new GridPosition(0, 0);
            }
            else
            {
                foreach (var construction in gameplayContext.constructionManager.runningConstructionModelMap.Values)
                {
                    foreach (var neighborEntry in construction.neighbors)
                    {
                        if (neighborEntry.Value == null)
                        {
                            return TileNodeUtils.tileNeighborPosition(construction, gameplayContext.constructionManager, neighborEntry.Key);
                        }
                    }
                }
            }
            throw new Exception("getConnectedRandonPosition fail");
        }

        
    }
}
