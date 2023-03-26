using Assets.Scripts.DemoGameCore.logic;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{

    

    public abstract class AbstractConstructionPrototype {

        public readonly String prototypeId;
        public readonly Language language;
        public ResourcePack buyInstanceCostPack;

        protected DescriptionPackage descriptionPackage;

        protected AbstractConstructionPrototype(String prototypeId, 
            Language language, 
            ResourcePack buyInstanceCostPack
            )
        {
            this.prototypeId = prototypeId;
            this.language = language;
            this.buyInstanceCostPack = buyInstanceCostPack;

            if (buyInstanceCostPack != null)
            {
                buyInstanceCostPack.modifiedValues = buyInstanceCostPack.baseValues;
                buyInstanceCostPack.modifiedValuesDescription = (String.Join(", ",
                            buyInstanceCostPack.modifiedValues
                                    .Select(pair => pair.type + "x" + pair.amount)
                                    .ToList())
                                    + "; "
                    );
            }
        }

        public abstract BaseConstruction getInstance(GridPosition position);

        internal void lazyInitDescription(IdleGameplayContext gameContext, Language language)
        {
            if (buyInstanceCostPack != null)
            {
                buyInstanceCostPack.descriptionStart = gameContext.gameDictionary.getPlayScreenTexts(language)[1];
            }
        }
    }


    public interface IBuiltinConstructionsLoader
    {
        public Dictionary<String, AbstractConstructionPrototype> getProviderMap(Language language);
    }
}
