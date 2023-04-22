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
    public abstract class BaseConstruction : ITileNode<BaseConstruction>
    {

        public IdleGameplayContext gameplayContext;

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

        /**
         * NotNull
         */
        public ExistenceComponent existenceComponent;

        private Dictionary<TileNeighborDirection, BaseConstruction> _neighbors;
        internal bool allowPositionOverwrite = false;

        public GridPosition position { get => this.saveData.position; set => this.saveData.position = value; }
        public Dictionary<TileNeighborDirection, BaseConstruction> neighbors { get => _neighbors; set => _neighbors = value; }

        public void lazyInitDescription(IdleGameplayContext gameContext, Language language)
        {
            this.gameplayContext = gameContext;

            this.name = gameContext.gameDictionary.constructionPrototypeIdToShowName(language, prototypeId);
            this.detailDescroptionConstPart = gameContext.gameDictionary.constructionPrototypeIdToDetailDescroptionConstPart(language, prototypeId);

            outputComponent.lazyInitDescription();
            upgradeComponent.lazyInitDescription();
            existenceComponent.lazyInitDescription();
            

            updateModifiedValues();
        }

        public BaseConstruction(String prototypeId, String id)
        {

            this.saveData = new ConstructionSaveData(prototypeId);
            this.id = id;
        }



        /**
         * 重新计算各个数值的加成后的结果
         */
        public void updateModifiedValues()
        {
            //gameContext.frontend.log(this.name, "updateCurrentCache called");
            // --------------
            
            upgradeComponent.updateModifiedValues();
            outputComponent.updateModifiedValues();
            existenceComponent.updateModifiedValues();
            

        }


        public void onSubLogicFrame()
        {
            outputComponent.onSubLogicFrame();
            proficiencyComponent.onSubLogicFrame();
        }
    }
}
