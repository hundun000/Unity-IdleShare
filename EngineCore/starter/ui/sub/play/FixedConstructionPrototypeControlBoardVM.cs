using hundun.unitygame.adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public class FixedConstructionPrototypeControlBoardVM<T_GAME, T_SAVE> : AbstractConstructionPrototypeControlBoardVM<T_GAME, T_SAVE> where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        public static int FIXED_NODE_NUM = 5;

        GameObject nodesRoot;
        Image background;
        ConstructionPrototypeControlNodeVM<T_GAME, T_SAVE> nodePrefab;

        void Awake()
        {
            this.background = this.transform.Find("background").GetComponent<Image>();
            this.nodesRoot = this.transform.Find("_nodesRoot").gameObject;
            this.nodePrefab = this.transform.Find("_templates/nodePrefab").GetComponent<ConstructionPrototypeControlNodeVM<T_GAME, T_SAVE>>();
        }

        override public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent)
        {

            base.postPrefabInitialization(parent);
            this.background.sprite = (parent.game.textureManager.defaultBoardNinePatchTexture);

        }

        override protected int initChild(int areaShownConstructionsSize)
        {
            int childrenSize = FIXED_NODE_NUM;

            constructionControlNodes.Clear();
            nodesRoot.transform.AsTableClear();

            for (int i = 0; i < childrenSize; i++)
            {
                ConstructionPrototypeControlNodeVM<T_GAME, T_SAVE> constructionView = nodesRoot.transform.AsTableAdd<ConstructionPrototypeControlNodeVM<T_GAME, T_SAVE>>(nodePrefab.gameObject);
                constructionView.postPrefabInitialization(parent);
                constructionControlNodes.Add(constructionView);
            }

            return childrenSize;

        }
    }
}
