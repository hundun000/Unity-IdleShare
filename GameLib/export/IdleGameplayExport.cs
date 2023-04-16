using Assets.Scripts.DemoGameCore.logic;
using hundun.unitygame.adapters;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;


namespace hundun.idleshare.gamelib
{

    public class IdleGameplayExport : ILogicFrameListener, ISubGameplaySaveHandler<GameplaySaveData>, ISubSystemSettingSaveHandler<SystemSettingSaveData>
    {
        private IdleGameplayContext gameplayContext;
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

        public long getResourceNumOrZero(String resourceId)
        {
            return gameplayContext.storageManager.getResourceNumOrZero(resourceId);
        }


        public List<BaseConstruction> getConstructionsOfPrototype(String prototypeId)
        {
            return gameplayContext.constructionManager.getConstructionsOfPrototype(prototypeId)
                
                ;
        }

        public void onLogicFrame()
        {
            gameplayContext.constructionManager.onSubLogicFrame();
            gameplayContext.storageManager.onSubLogicFrame();
        }

        public List<BaseConstruction> getAreaShownConstructionsOrEmpty(String current)
        {
            return gameplayContext.constructionManager.getAreaShownConstructionsOrEmpty(current)
                    ;
        }

        public List<AbstractConstructionPrototype> getAreaShownConstructionPrototypesOrEmpty(String current)
        {
            return gameplayContext.constructionManager.getAreaShownConstructionPrototypesOrEmpty(current)
                    ;
            ;
        }

        public void eventManagerRegisterListener(Object objecz)
        {
            gameplayContext.eventManager.registerListener(objecz);
        }

        public HashSet<String> getUnlockedResourceTypes()
        {
            return gameplayContext.storageManager.unlockedResourceTypes;
        }

        public void constructionChangeWorkingLevel(String id, int delta)
        {
            BaseConstruction model = gameplayContext.constructionManager.getConstruction(id);
            model.levelComponent.changeWorkingLevel(delta);
        }

        public void constructionOnClick(String id)
        {
            BaseConstruction model = gameplayContext.constructionManager.getConstruction(id);
            model.onClick();
        }

        public BaseConstruction getConstructionAt(GridPosition position)
        {
            return gameplayContext.constructionManager.getConstructionAt(position);
        }

        public Boolean constructionCanClickEffect(String id)
        {
            BaseConstruction model = gameplayContext.constructionManager.getConstruction(id);
            return model.canClickEffect();
        }

        public Boolean constructionCanChangeWorkingLevel(String id, int delta)
        {
            BaseConstruction model = gameplayContext.constructionManager.getConstruction(id);
            return model.levelComponent.canChangeWorkingLevel(delta);
        }

        public void destoryConstruction(String id, String constructionPrototypeIdOfEmpty)
        {
            gameplayContext.constructionManager.destoryInstanceAndNotify(id, constructionPrototypeIdOfEmpty);
        }

        internal AbstractConstructionPrototype previewPrototype(string prototypeId)
        {
            return gameplayContext.constructionFactory.getPrototype(prototypeId);
        }

        public void transformConstruction(String id)
        {
            gameplayContext.constructionManager.transformInstanceAndNotify(id);
        }

        public AchievementInfoPackage getAchievementInfoPackage()
        {
            return gameplayContext.achievementManager.getAchievementInfoPackage();
        }


        public void applyGameplaySaveData(GameplaySaveData gameplaySaveData)
        {
            this.stageId = gameplaySaveData.stageId;

            gameplaySaveData.constructionSaveDataMap.Values.ToList().ForEach(it => {
                gameplayContext.constructionManager.loadInstance(it);
            });
            
            gameplayContext.storageManager.unlockedResourceTypes = (gameplaySaveData.unlockedResourceTypes);
            gameplayContext.storageManager.ownResoueces = (gameplaySaveData.ownResoueces);
            gameplayContext.achievementManager.unlockedAchievementIds = (gameplaySaveData.unlockedAchievementIds);
        }

        public void currentSituationToGameplaySaveData(GameplaySaveData gameplaySaveData)
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

        public void applySystemSetting(SystemSettingSaveData systemSettingSave)
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

        public void currentSituationToSystemSetting(SystemSettingSaveData systemSettingSave)
        {
            systemSettingSave.language = (this.language);
        }


        internal bool canBuyInstanceOfPrototype(string prototypeId, GridPosition position)
        {
            return gameplayContext.constructionManager.canBuyInstanceOfPrototype(prototypeId, position);
        }
        internal void buyInstanceOfPrototype(string prototypeId, GridPosition position)
        {
            gameplayContext.constructionManager.buyInstanceOfPrototype(prototypeId, position);
            gameplayContext.eventManager.notifyConstructionCollectionChange();
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
