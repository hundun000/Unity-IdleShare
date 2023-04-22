using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace hundun.idleshare.enginecore
{
    public abstract class AbstractConstructionPrototypeControlBoardVM<T_GAME, T_SAVE> : MonoBehaviour, ILogicFrameListener, IGameAreaChangeListener where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        protected BaseIdlePlayScreen<T_GAME, T_SAVE> parent;
        /**
         * 显示在当前screen的Construction集合。以ConstructionView形式存在。
         */
        protected List<ConstructionPrototypeControlNodeVM<T_GAME, T_SAVE>> constructionControlNodes = new List<ConstructionPrototypeControlNodeVM<T_GAME, T_SAVE>>();



        virtual public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent)
        {
            this.parent = parent;
        }
        public void onLogicFrame()
        {
            constructionControlNodes.ForEach(item => item.update());
        }

        public void onGameAreaChange(String last, String current)
        {


            List<AbstractConstructionPrototype> newConstructions = parent.game.idleGameplayExport.gameplayContext.constructionManager.getAreaShownConstructionPrototypesOrEmpty(current);

            int childrenSize = initChild(newConstructions.size());

            for (int i = 0; i < childrenSize && i < newConstructions.size(); i++)
            {
                constructionControlNodes.get(i).setModel(newConstructions.get(i));
            }
            for (int i = newConstructions.size(); i < childrenSize; i++)
            {
                constructionControlNodes.get(i).setModel(null);
            }
            parent.game.frontend.log(this.getClass().getSimpleName(), "ConstructionPrototypes change to: " + String.Join(",", 
                newConstructions.Select(construction => construction.prototypeId))
            );

        }

        /**
         * 初始化某个数量的Children。该数量不一定等于areaShownConstructionsSize，由实现决定。
         * @return childrenSize
         */
        protected abstract int initChild(int areaShownConstructionsSize);
    }
}
