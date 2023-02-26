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
    public class ConstructionExportProxy
    {
        private BaseConstruction model;

        public String id;
        public String name;
        public DescriptionPackage descriptionPackage;
        public ResourcePack outputGainPack;
        public ResourcePack outputCostPack;
        public UpgradeState upgradeState;
        public ResourcePack upgradeCostPack;
        // ------- need runtime calculate ------
        public String buttonDescroption { 
            get 
            {
                return model.getButtonDescroption();
            } 
        }
        public String workingLevelDescroption
        {
            get
            {
                return model.levelComponent.getWorkingLevelDescroption();
            }
        }
        public String detailDescroptionConstPart
        {
            get
            {
                return model.detailDescroptionConstPart;
            }
        }

        public Boolean workingLevelChangable
        {
            get
            {
                return model.levelComponent.workingLevelChangable;
            }
        }

        public static ConstructionExportProxy fromModel(BaseConstruction model)
        {
            ConstructionExportProxy result = new ConstructionExportProxy();
            result.model = model;
            result.id = (model.id);
            result.name = (model.name);
            result.outputCostPack = (model.outputComponent.outputCostPack);
            result.outputGainPack = (model.outputComponent.outputGainPack);
            result.upgradeState = (model.upgradeComponent.upgradeState);
            result.upgradeCostPack = (model.upgradeComponent.upgradeCostPack);
            result.descriptionPackage = (model.descriptionPackage);
            return result;
        }


    }

    public class IdleGameplayExport : ILogicFrameListener, ISubGameplaySaveHandler<GameplaySaveData>, ISubSystemSettingSaveHandler<SystemSettingSaveData>
    {
        private IdleGameplayContext gameplayContext;
        private IBuiltinConstructionsLoader builtinConstructionsLoader;
        private ChildGameConfig childGameConfig;
        public IGameDictionary gameDictionary;
        public Language language;

        public IdleGameplayExport(
                IFrontend frontEnd,
                IGameDictionary gameDictionary,
                IBuiltinConstructionsLoader builtinConstructionsLoader,
                int LOGIC_FRAME_PER_SECOND, ChildGameConfig childGameConfig)
        {
            this.gameDictionary = gameDictionary;
            this.childGameConfig = childGameConfig;
            this.builtinConstructionsLoader = builtinConstructionsLoader;
            this.gameplayContext = new IdleGameplayContext(frontEnd, gameDictionary, LOGIC_FRAME_PER_SECOND);
        }

        public long getResourceNumOrZero(String resourceId)
        {
            return gameplayContext.storageManager.getResourceNumOrZero(resourceId);
        }

        public BaseConstruction getConstruction(String id)
        {
            return gameplayContext.constructionFactory.getConstruction(id);
        }

        public void onLogicFrame()
        {
            gameplayContext.constructionManager.onSubLogicFrame();
            gameplayContext.storageManager.onSubLogicFrame();
        }

        public List<ConstructionExportProxy> getAreaShownConstructionsOrEmpty(String current)
        {
            return gameplayContext.constructionManager.getAreaShownConstructionsOrEmpty(current)
                    .Select(it => ConstructionExportProxy.fromModel(it))
                    .ToList();
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
            BaseConstruction model = gameplayContext.constructionFactory.getConstruction(id);
            model.levelComponent.changeWorkingLevel(delta);
        }

        public void constructionOnClick(String id)
        {
            BaseConstruction model = gameplayContext.constructionFactory.getConstruction(id);
            model.onClick();
        }

        public Boolean constructionCanClickEffect(String id)
        {
            BaseConstruction model = gameplayContext.constructionFactory.getConstruction(id);
            return model.canClickEffect();
        }

        public Boolean constructionCanChangeWorkingLevel(String id, int delta)
        {
            BaseConstruction model = gameplayContext.constructionFactory.getConstruction(id);
            return model.levelComponent.canChangeWorkingLevel(delta);
        }

        public void applyGameplaySaveData(GameplaySaveData gameplaySaveData)
        {
            List<BaseConstruction> constructions = gameplayContext.constructionFactory.getConstructions();
            foreach (BaseConstruction construction in constructions)
            {
                if (gameplaySaveData.constructionSaveDataMap.ContainsKey(construction.id))
                {
                    construction.saveData = (gameplaySaveData.constructionSaveDataMap.get(construction.id));
                    construction.updateModifiedValues();
                }
            }
            gameplayContext.storageManager.unlockedResourceTypes = (gameplaySaveData.unlockedResourceTypes);
            gameplayContext.storageManager.ownResoueces = (gameplaySaveData.ownResoueces);
            gameplayContext.achievementManager.unlockedAchievementNames = (gameplaySaveData.unlockedAchievementNames);
        }

        public void currentSituationToGameplaySaveData(GameplaySaveData gameplaySaveData)
        {
            List<BaseConstruction> constructions = gameplayContext.constructionFactory.getConstructions();
            gameplaySaveData.constructionSaveDataMap = (constructions
                    .ToDictionary(
                            it => it.id,
                            it => it.saveData
                            )
                    );
            gameplaySaveData.unlockedResourceTypes = (gameplayContext.storageManager.unlockedResourceTypes);
            gameplaySaveData.ownResoueces = (gameplayContext.storageManager.ownResoueces);
            gameplaySaveData.unlockedAchievementNames = (gameplayContext.achievementManager.unlockedAchievementNames);
        }

        public void applySystemSetting(SystemSettingSaveData systemSettingSave)
        {
            this.language = (systemSettingSave.language);
            gameplayContext.allLazyInit(
                    systemSettingSave.language,
                    childGameConfig,
                    builtinConstructionsLoader.provide(systemSettingSave.language)
                    );
            gameplayContext.frontend.log(this.getClass().getSimpleName(), "applySystemSetting done");
        }

        public void currentSituationToSystemSetting(SystemSettingSaveData systemSettingSave)
        {
            systemSettingSave.language = (this.language);
        }

    }
}
