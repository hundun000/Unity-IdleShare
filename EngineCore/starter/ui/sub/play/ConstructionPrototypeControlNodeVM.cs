using hundun.idleshare.enginecore;
using hundun.idleshare.gamelib;
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
    public class ConstructionPrototypeControlNodeVM<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;
        AbstractConstructionPrototype model;

        Text constructionNameLabel;
        Text costLabel;
        TextButton clickEffectButton;
        InputField xInputField;
        InputField yInputField;
        Image background;





        void Awake()
        {
            this.background = this.transform.Find("background").GetComponent<Image>();
            this.constructionNameLabel = this.transform.Find("constructionNameLabel").GetComponent<Text>();
            this.costLabel = this.transform.Find("costLabel").GetComponent<Text>();
            this.clickEffectButton = this.transform.Find("clickEffectButton").GetComponent<TextButton>();
            this.xInputField = this.transform.Find("xInputField").GetComponent<InputField>();
            this.yInputField = this.transform.Find("yInputField").GetComponent<InputField>();
        }

        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent)
        {

            this.parent = parent;

            clickEffectButton.button.onClick.AddListener(() => {
            
                parent.game.frontend.log(this.getClass().getSimpleName(), "clicked");
                // FIXME 改为拖拽目的地坐标
                int x = int.Parse(xInputField.text);
                int y = int.Parse(yInputField.text);
                //GridPosition position = parent.game.idleGameplayExport.getConnectedRandonPosition();
                GridPosition position = new GridPosition(x, y);
                parent.game.idleGameplayExport.gameplayContext.constructionManager.buyInstanceOfPrototype(model.prototypeId, position);
            });

            background.sprite = parent.game.textureManager.defaultBoardNinePatchTexture;
        }

        private void updateCanCreateInstance()
        {
            bool enable;
            try
            {
                int x = int.Parse(xInputField.text);
                int y = int.Parse(yInputField.text);
                GridPosition position = new GridPosition(x, y);
                enable = parent.game.idleGameplayExport.gameplayContext.constructionManager.canBuyInstanceOfPrototype(model.prototypeId, position);
            }
            catch
            {
                enable = false;
            }

            clickEffectButton.button.interactable = enable;
        }

        public void setModel(AbstractConstructionPrototype constructionExportData)
        {
            this.model = constructionExportData;
            update();
        }

        public void update()
        {
            // ------ update show-state ------
            if (model == null)
            {
                this.gameObject.SetActive(false);
                //textButton.setVisible(false);
                //Gdx.app.log("ConstructionView", this.hashCode() + " no model");
                return;
            }
            else
            {
                this.gameObject.SetActive(true);
                //textButton.setVisible(true);
                //Gdx.app.log("ConstructionView", model.getName() + " set to its view");
            }
            // ------ update text ------
            constructionNameLabel.text = parent.game.idleGameplayExport.gameDictionary.constructionPrototypeIdToShowName(parent.game.idleGameplayExport.language, model.prototypeId);
            costLabel.text = model.buyInstanceCostPack.modifiedValuesDescription;

            // ------ update clickable-state ------

            updateCanCreateInstance();
        }
    }
}
