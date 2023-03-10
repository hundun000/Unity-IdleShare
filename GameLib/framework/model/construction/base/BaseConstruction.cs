using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unity.VisualScripting.Icons;

namespace hundun.idleshare.gamelib
{
    public abstract class BaseConstruction : ILogicFrameListener, IBuffChangeListener {

        public static readonly int DEFAULT_MAX_LEVEL = 99;

        public int maxLevel = DEFAULT_MAX_LEVEL;


        public static readonly int DEFAULT_MAX_DRAW_NUM = 5;

        public int maxDrawNum = DEFAULT_MAX_DRAW_NUM;

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


        public void lazyInitDescription(IdleGameplayContext gameContext, Language language)
        {
            this.gameContext = gameContext;

            this.name = gameContext.gameDictionary.constructionIdToShowName(language, id);
            this.detailDescroptionConstPart = gameContext.gameDictionary.constructionIdToDetailDescroptionConstPart(language, id);

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
            //gameContext.frontend.log(this.name, "updateCurrentCache called");
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
