using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public abstract class BaseConstruction : ILogicFrameListener, IBuffChangeListener {

        public static readonly int DEFAULT_MAX_LEVEL = 99;

        public int maxLevel = DEFAULT_MAX_LEVEL;


        public static readonly int DEFAULT_MAX_DRAW_NUM = 5;

        public int maxDrawNum = DEFAULT_MAX_DRAW_NUM;

        public static readonly DescriptionPackage WORKING_LEVEL_AUTO_DESCRIPTION_PACKAGE = new DescriptionPackage(
            "AutoCost", "AutoGain", "UpgradeCost", "(max level)", "Upgrade",
                ILevelDescroptionProviders.WORKING_LEVEL_IMP);

        public static readonly DescriptionPackage MAX_LEVEL_AUTO_DESCRIPTION_PACKAGE = new DescriptionPackage(
            "AutoCost", "AutoGain", "UpgradeCost", "(max level)", "Upgrade",
                ILevelDescroptionProviders.ONLY_LEVEL_IMP);

        public static readonly DescriptionPackage SELLING_DESCRIPTION_PACKAGE = new DescriptionPackage(
            "Sell", "Gain", "UpgradeCost", "(max level)", "Upgrade",
                ILevelDescroptionProviders.WORKING_LEVEL_IMP);
        public static readonly DescriptionPackage GATHER_DESCRIPTION_PACKAGE = new DescriptionPackage(
            "Pay", "Gain", null, "(max level)", "Gather",
                ILevelDescroptionProviders.EMPTY_IMP);

        public static readonly DescriptionPackage WIN_DESCRIPTION_PACKAGE = new DescriptionPackage(
            null, null, "Pay", "(max level)", "Unlock",
                ILevelDescroptionProviders.LOCK_IMP);

        protected Random random = new Random();

        public IdleGameplayContext gameContext;

        /**
         * NotNull
         */
        public ConstructionSaveData saveData;

        public String name;

        public String id;

        public String detailDescroptionConstPart;

        public DescriptionPackage descriptionPackage;


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


        public void lazyInitDescription(IdleGameplayContext gameContext)
        {
            this.gameContext = gameContext;

            this.name = gameContext.gameDictionary.constructionIdToShowName(this.id);

            outputComponent.lazyInitDescription();
            upgradeComponent.lazyInitDescription();

            updateModifiedValues();
        }

        public BaseConstruction(String id)
        {

            this.saveData = new ConstructionSaveData();
            this.id = id;
        }

        public abstract void onClick();

        public abstract Boolean canClickEffect();

        public String getButtonDescroption()
        {
            return descriptionPackage.buttonDescroption;
        }

        //protected abstract long calculateModifiedUpgradeCost(long baseValue, int level);
        public abstract long calculateModifiedOutput(long baseValue, int level);
        public abstract long calculateModifiedOutputCost(long baseValue, int level);



        /**
         * 重新计算各个数值的加成后的结果
         */
        public void updateModifiedValues()
        {
            gameContext.frontend.log(this.name, "updateCurrentCache called");
            // --------------
            Boolean reachMaxLevel = this.saveData.level == this.maxLevel;
            upgradeComponent.updateModifiedValues(reachMaxLevel);
            outputComponent.updateModifiedValues();

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


        protected Boolean canUpgrade()
        {
            return upgradeComponent.canUpgrade();
        }

        public String getSaveDataKey()
        {
            return id;
        }

        public abstract void onLogicFrame();
    }
}
