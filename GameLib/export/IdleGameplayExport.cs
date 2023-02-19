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
    public class ConstructionExportData
    {
        public String id;
        public String name;
        public String buttonDescroption;
        public String workingLevelDescroption;
        public String detailDescroptionConstPart;
        public DescriptionPackage descriptionPackage;
        public ResourcePack outputGainPack;
        public ResourcePack outputCostPack;
        public UpgradeState upgradeState;
        public ResourcePack upgradeCostPack;
        public Boolean workingLevelChangable;

        public static ConstructionExportData fromModel(BaseConstruction model)
        {
            ConstructionExportData result = new ConstructionExportData();

            result.id = (model.id);
            result.name = (model.name);
            result.buttonDescroption = (model.getButtonDescroption());
            result.workingLevelDescroption = (model.levelComponent.getWorkingLevelDescroption());
            result.outputCostPack = (model.outputComponent.outputCostPack);
            result.outputGainPack = (model.outputComponent.outputGainPack);
            result.upgradeState = (model.upgradeComponent.upgradeState);
            result.upgradeCostPack = (model.upgradeComponent.upgradeCostPack);
            result.detailDescroptionConstPart = (model.detailDescroptionConstPart);
            result.descriptionPackage = (model.descriptionPackage);
            result.workingLevelChangable = (model.levelComponent.workingLevelChangable);
            return result;
        }


    }

    public class IdleGameplayExport : ILogicFrameListener, ISubGameplaySaveHandler<GameplaySaveData>
    {
        private IdleGameplayContext gameplayContext;

        public IdleGameplayExport(
                IFrontend frontEnd,
                IGameDictionary gameDictionary,
                BaseConstructionFactory constructionFactory,
                int LOGIC_FRAME_PER_SECOND, ChildGameConfig childGameConfig)
        {
            this.gameplayContext = new IdleGameplayContext(frontEnd, gameDictionary, constructionFactory, LOGIC_FRAME_PER_SECOND, childGameConfig);
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

        public List<ConstructionExportData> getAreaShownConstructionsOrEmpty(String current)
        {
            return gameplayContext.constructionManager.getAreaShownConstructionsOrEmpty(current)
                    .Select(it => ConstructionExportData.fromModel(it))
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

        public void applyGameSaveData(GameplaySaveData gameplaySaveData)
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

        public void currentSituationToSaveData(GameplaySaveData gameplaySaveData)
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
    }
}
