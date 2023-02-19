using Assets.Scripts.DemoGameCore.ui.screen;
using hundun.idleshare.enginecore;
using hundun.unitygame.adapters;
using System.Collections.Generic;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using hundun.unitygame.enginecorelib;

namespace hundun.idleshare.enginecore
{
    public class StorageInfoBoardVM<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {

        private BackgroundVM backgroundVM;

        private static int NODE_HEIGHT = 25;
        private static int NODE_WIDTH = 120;

        public static int NUM_NODE_PER_ROW = 5;

        List<String> shownOrders;
        HashSet<String> shownTypes = new HashSet<String>();
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;

        List<ResourceAmountPairNode<T_GAME, T_SAVE>> nodes = new List<ResourceAmountPairNode<T_GAME, T_SAVE>>();

        public void lazyInit(List<String> shownOrders)
        {
            this.shownOrders = shownOrders;
            rebuildCells();
        }

        //Label mainLabel;


        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent)
        {
            this.parent = parent;
            backgroundVM.update(parent.game.textureManager.defaultBoardNinePatchTexture);

        }



        private void rebuildCells()
        {
            //this.clearChildren();
            //nodes.clear();

            //for (int i = 0; i < shownOrders.size(); i++)
            //{
            //    String resourceType = shownOrders.get(i);
            //    if (shownTypes.contains(resourceType))
            //    {
            //        ResourceAmountPairNode<T_GAME> node = new ResourceAmountPairNode<>(parent.getGame(), resourceType);
            //        nodes.add(node);
            //        shownTypes.add(resourceType);
            //        Cell<ResourceAmountPairNode<T_GAME>> cell = this.add(node).width(NODE_WIDTH).height(NODE_HEIGHT);
            //        if ((i + 1) % NUM_NODE_PER_ROW == 0)
            //        {
            //            cell.row();
            //        }
            //    }
            //}

        }



        public void updateViewData()
        {
            //        List<ResourceType> shownResources = areaShownResources.get(parent.getArea());
            //        if (shownResources == null) {
            //            mainLabel.setText("Unkonwn area");
            //            return;
            //        }

            //        String text = shownResources.stream()
            //                .map(shownResource -> parent.game.getModelContext().getStorageManager().getResourceDescription(shownResource))
            //                .collect(Collectors.joining("    "));
            //        text += "\nBuffs = " + parent.game.getModelContext().getBuffManager().getBuffAmounts();
            //        mainLabel.setText(text);
            //boolean needRebuildCells = !shownTypes.equals(parent.getGame().getIdleGameplayExport().getUnlockedResourceTypes());
            //if (needRebuildCells)
            //{
            //    shownTypes.clear();
            //    shownTypes.addAll(parent.getGame().getIdleGameplayExport().getUnlockedResourceTypes());
            //    rebuildCells();
            //}
            //nodes.stream().forEach(node->node.update(parent.getGame().getIdleGameplayExport().getResourceNumOrZero(node.getResourceType())));
        }


    }
}