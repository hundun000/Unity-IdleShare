using Assets.Scripts.DemoGameCore.ui.screen;
using hundun.idleshare.enginecore;
using hundun.unitygame.adapters;
using System.Collections.Generic;
using System;
using UnityEngine;
using hundun.unitygame.enginecorelib;
using Unity.VisualScripting;
using UnityEngine.UI;
using hundun.unitygame.gamelib;

namespace hundun.idleshare.enginecore
{
    public class StorageInfoBoardVM<T_GAME, T_SAVE> : MonoBehaviour, ILogicFrameListener where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {

        private Image background;
        protected GameObject nodesRoot;
        protected GameObject nodePrefab;


        List<String> shownOrders;
        HashSet<String> shownTypes = new HashSet<String>();
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;

        List<ResourceAmountPairNode> nodes = new List<ResourceAmountPairNode>();



        //Label mainLabel;
        private void Awake()
        {
            this.background = this.transform.Find("background").GetComponent<Image>();
            this.nodesRoot = this.transform.Find("_nodesRoot").gameObject;
            this.nodePrefab = this.transform.Find("_templates/nodePrefab").gameObject;
        }

        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent, List<String> shownOrders)
        {
            this.parent = parent;
            background.sprite = (parent.game.textureManager.defaultBoardNinePatchTexture);

            this.shownOrders = shownOrders;
        }



        private void rebuildCells()
        {
            nodesRoot.transform.AsTableClear();
            nodes.Clear();

            for (int i = 0; i < shownOrders.size(); i++)
            {
                String resourceType = shownOrders.get(i);
                if (shownTypes.Contains(resourceType))
                {
                    ResourceAmountPairNode node = nodesRoot.transform.AsTableAdd<ResourceAmountPairNode>(nodePrefab);
                    node.postPrefabInitialization(parent.game.textureManager, resourceType);
                    nodes.Add(node);
                    shownTypes.Add(resourceType);
                }
            }

        }



        private void updateViewData()
        {
            Boolean needRebuildCells = !shownTypes.Equals(parent.game.idleGameplayExport.gameplayContext.storageManager.unlockedResourceTypes);
            if (needRebuildCells)
            {
                shownTypes.Clear();
                shownTypes.AddRange(parent.game.idleGameplayExport.gameplayContext.storageManager.unlockedResourceTypes);
                rebuildCells();
            }
            nodes.ForEach(
                node => node.update(parent.game.idleGameplayExport.gameplayContext.storageManager.getResourceNumOrZero(node.getResourceType()))
                );
        }

        public void onLogicFrame()
        {
            updateViewData();
        }
    }
}