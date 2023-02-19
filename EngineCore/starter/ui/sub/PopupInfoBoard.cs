using hundun.idleshare.enginecore;
using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public class PopupInfoBoard<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;

        Image background;
        GameObject childrenRoot;

        Text detailDescroptionConstPartTextTemplate;
        GameObject onePackTemplate;
        ResourceAmountPairNode resourceAmountPairNodeTemplate;
        GameObject maxLevelGroupTemplate;

        void Awake()
        {
            this.background = this.transform.Find("background").GetComponent<Image>();
            this.childrenRoot = this.transform.Find("childrenRoot").gameObject;

            this.detailDescroptionConstPartTextTemplate = this.transform.Find("_templates/detailDescroptionConstPartTextTemplate").GetComponent<Text>();
            this.onePackTemplate = this.transform.Find("_templates/onePackTemplate").gameObject;
            this.resourceAmountPairNodeTemplate = this.transform.Find("_templates/resourceAmountPairNodeTemplate").GetComponent<ResourceAmountPairNode>();
            this.maxLevelGroupTemplate = this.transform.Find("_templates/maxLevelGroupTemplate").gameObject;

        }

        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent)
        {
            //super("GUIDE_TEXT", parent.game.getButtonSkin());
            this.parent = parent;
            this.background.sprite = parent.game.textureManager.defaultBoardNinePatchTexture;
        }


        private void rebuildCells(ConstructionExportData model)
        {
            childrenRoot.transform.AsTableClear();

            Text detailDescroptionConstPartText = childrenRoot.transform.AsTableAdd<Text>(detailDescroptionConstPartTextTemplate.gameObject);
            detailDescroptionConstPartText.text = model.detailDescroptionConstPart;


            buildOnePack(model.outputCostPack);

            buildOnePack(model.outputGainPack);

            if (model.upgradeState == UpgradeState.HAS_NEXT_UPGRADE)
            {
                buildOnePack(model.upgradeCostPack);
            }
            else if (model.upgradeState == UpgradeState.REACHED_MAX_UPGRADE)
            {
                GameObject maxLevelGroup = childrenRoot.transform.AsTableAddGameobject(maxLevelGroupTemplate.gameObject);
                Text maxLevelGroupLabel_0 = maxLevelGroup.transform.Find("label_0").GetComponent<Text>();
                Text maxLevelGroupLabel_1 = maxLevelGroup.transform.Find("label_1").GetComponent<Text>();

                maxLevelGroupLabel_0.text = model.upgradeCostPack.descriptionStart;
                maxLevelGroupLabel_1.text = model.descriptionPackage.upgradeMaxLevelDescription;
            }

        }

        private void buildOnePack(ResourcePack pack)
        {
            if (pack != null && pack.modifiedValues != null)
            {
                GameObject onepackVM = childrenRoot.transform.AsTableAddGameobject(onePackTemplate.gameObject);
                Text onepackVMLabel = onepackVM.transform.Find("label").GetComponent<Text>();
                GameObject onepackVMNodesRoot = onepackVM.transform.Find("nodes").gameObject;


                onepackVMLabel.text = pack.descriptionStart;
                foreach (ResourcePair entry in pack.modifiedValues)
                {
                    ResourceAmountPairNode resourceAmountPairNode = onepackVMNodesRoot.transform.AsTableAdd<ResourceAmountPairNode>(resourceAmountPairNodeTemplate.gameObject);
                    resourceAmountPairNode.postPrefabInitialization(parent.game.textureManager, entry.type);
                    resourceAmountPairNode.update(entry.amount);
                }
            }
        }


        public void update(ConstructionExportData model)
        {
            rebuildCells(model);
        }



    }
}
