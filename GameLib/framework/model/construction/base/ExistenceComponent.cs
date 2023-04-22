using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class ExistenceComponent
    {
        private readonly BaseConstruction construction;

        /**
        * Nullable
        */
        public ResourcePack destoryCostPack;
        /**
        * Nullable
        */
        public ResourcePack destoryGainPack;

        public bool allowAnyProficiencyDestory;

        public ExistenceComponent(BaseConstruction construction)
        {
            this.construction = construction;

        }

        internal void lazyInitDescription()
        {
            if (destoryGainPack != null)
            {
                this.destoryGainPack.descriptionStart = construction.descriptionPackage.destroyGainDescriptionStart;
            }
            if (destoryCostPack != null)
            {
                this.destoryCostPack.descriptionStart = construction.descriptionPackage.destroyCostDescriptionStart;
            }
        }

        internal void updateModifiedValues()
        {
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

        public Boolean canDestory()
        {
            if (!allowAnyProficiencyDestory && !construction.proficiencyComponent.isMaxProficiency())
            {
                return false;
            }
            return destoryCostPack != null && construction.gameplayContext.storageManager.isEnough(destoryCostPack.modifiedValues);
        }

        internal void destoryInstanceAndNotify(String constructionPrototypeIdOfEmpty)
        {
            construction.gameplayContext.constructionManager.removeInstance(construction);
            if (construction.existenceComponent.destoryCostPack != null)
            {
                construction.gameplayContext.storageManager.modifyAllResourceNum(construction.existenceComponent.destoryCostPack.modifiedValues, false);
            }
            if (construction.existenceComponent.destoryGainPack != null)
            {
                construction.gameplayContext.storageManager.modifyAllResourceNum(construction.existenceComponent.destoryGainPack.modifiedValues, true);
            }
            if (constructionPrototypeIdOfEmpty != null)
            {
                construction.gameplayContext.constructionManager.createInstanceOfPrototype(constructionPrototypeIdOfEmpty, construction.position);
            }
            construction.gameplayContext.eventManager.notifyConstructionCollectionChange();
        }

    }
}

