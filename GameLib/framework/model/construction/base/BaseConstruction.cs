using hundun.unitygame.gamelib;
using Map;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public abstract class BaseConstruction : ILogicFrameListener, IBuffChangeListener, ITileNode<BaseConstruction>
    {
        public int maxProficiency = 100;
        public int upgradeLostProficiency = 0;
        internal bool allowAnyProficiencyDestory = true;

        public static readonly int DEFAULT_MAX_LEVEL = 5;
        public int maxLevel = DEFAULT_MAX_LEVEL;

        public static readonly int DEFAULT_MAX_DRAW_NUM = 5;
        public int maxDrawNum = DEFAULT_MAX_DRAW_NUM;

        public static readonly int DEFAULT_MIN_WORKING_LEVEL = 0;
        public int minWorkingLevel = DEFAULT_MIN_WORKING_LEVEL;

        protected Random random = new Random();

        public IdleGameplayContext gameContext;

        /**
         * NotNull
         */
        public ConstructionSaveData saveData;

        public String name;

        public String id;

        public String prototypeId { get => saveData.prototypeId; }

        public String detailDescroptionConstPart;

        public DescriptionPackage descriptionPackage;

        /**
        * Nullable
        */
        public ResourcePack destoryCostPack;
        /**
        * Nullable
        */
        public ResourcePack destoryGainPack;
        /**
         * NotNull
         */
        public UpgradeComponent upgradeComponent;


        /**
         * NotNull
         */
        public OutputComponent outputComponent;

        /**
         * NotNull
         */
        public LevelComponent levelComponent;

        /**
         * NotNull
         */
        public ProficiencyComponent proficiencyComponent;

        private Dictionary<TileNeighborDirection, BaseConstruction> _neighbors;
        internal bool allowPositionOverwrite = false;

        public GridPosition position { get => this.saveData.position; set => this.saveData.position = value; }
        public Dictionary<TileNeighborDirection, BaseConstruction> neighbors { get => _neighbors; set => _neighbors = value; }

        public void lazyInitDescription(IdleGameplayContext gameContext, Language language)
        {
            this.gameContext = gameContext;

            this.name = gameContext.gameDictionary.constructionPrototypeIdToShowName(language, prototypeId);
            this.detailDescroptionConstPart = gameContext.gameDictionary.constructionPrototypeIdToDetailDescroptionConstPart(language, prototypeId);

            outputComponent.lazyInitDescription();
            upgradeComponent.lazyInitDescription();
            if (destoryGainPack != null)
            {
                this.destoryGainPack.descriptionStart = descriptionPackage.destroyGainDescriptionStart;
                this.destoryCostPack.descriptionStart = descriptionPackage.destroyCostDescriptionStart;
            }

            updateModifiedValues();
        }

        public BaseConstruction(String prototypeId, String id)
        {

            this.saveData = new ConstructionSaveData(prototypeId);
            this.id = id;
        }

        public abstract void onClick();

        public abstract Boolean canClickEffect();

        //protected abstract long calculateModifiedUpgradeCost(long baseValue, int level);
        public abstract long calculateModifiedOutputGain(long baseValue, int level, int proficiency);
        public abstract long calculateModifiedOutputCost(long baseValue, int level, int proficiency);



        /**
         * 重新计算各个数值的加成后的结果
         */
        public void updateModifiedValues()
        {
            //gameContext.frontend.log(this.name, "updateCurrentCache called");
            // --------------
            Boolean reachMaxLevel = this.saveData.level == this.maxLevel;
            upgradeComponent.updateModifiedValues(reachMaxLevel);
            outputComponent.updateModifiedValues();

            if (destoryGainPack != null)
            {
                destoryCostPack.modifiedValues = destoryCostPack.baseValues;
                destoryCostPack.modifiedValuesDescription = (String.Join(", ",
                        destoryCostPack.modifiedValues
                                .Select(pair => pair.type + "x" + pair.amount)
                                .ToList())
                                + "; "
                );
                destoryGainPack.modifiedValues = destoryGainPack.baseValues;
                destoryGainPack.modifiedValuesDescription = (String.Join(", ",
                        destoryGainPack.modifiedValues
                                .Select(pair => pair.type + "x" + pair.amount)
                                .ToList())
                                + "; "
                );
            }

        }

       
        public void onBuffChange()
        {
            updateModifiedValues();
        }


        virtual protected void printDebugInfoAfterConstructed()
        {
            // default do nothing
        }

        protected Boolean canOutput()
        {
            return outputComponent.canOutput();
        }
        protected void doOutput()
        {
            if (outputComponent.hasCost())
            {
                gameContext.storageManager.modifyAllResourceNum(outputComponent.outputCostPack.modifiedValues, false);
            }
            if (outputComponent.outputGainPack != null)
            {
                gameContext.storageManager.modifyAllResourceNum(outputComponent.outputGainPack.modifiedValues, true);
            }
        }

        public Boolean canUpgrade()
        {
            return upgradeComponent.canUpgrade();
        }

        public Boolean canTransfer()
        {
            return upgradeComponent.canTransfer();
        }

        public void doUpgrade()
        {
            List<ResourcePair> upgradeCostRule = upgradeComponent.upgradeCostPack.modifiedValues;
            gameContext.storageManager.modifyAllResourceNum(upgradeCostRule, false);
            saveData.level = (saveData.level + 1);
            if (!levelComponent.workingLevelChangable)
            {
                saveData.workingLevel = (saveData.level);
            }
            saveData.proficiency -= upgradeLostProficiency;
            updateModifiedValues();
            gameContext.eventManager.notifyConstructionCollectionChange();
        }

        public Boolean canDestory() 
        {
            if (!allowAnyProficiencyDestory && this.saveData.proficiency < this.maxProficiency)
            {
                return false;
            }
            return destoryCostPack != null && gameContext.storageManager.isEnough(destoryCostPack.modifiedValues);
        }

        public String getSaveDataKey()
        {
            return id;
        }

        public abstract void onLogicFrame();
    }
}
