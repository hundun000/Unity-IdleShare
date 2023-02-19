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
    public class ConstructionControlNodeVM<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;
        ConstructionExportData model;

        Text constructionNameLabel;
        TextButton upWorkingLevelButton;
        TextButton downWorkingLevelButton;
        Text workingLevelLabel;

        TextButton clickEffectButton;
        Image background;

        void OnMouseEnter()
        {
            if (model != null)
            {
                parent.showAndUpdateGuideInfo(model);
            }
            parent.game.frontend.log(this.getClass().getSimpleName(), "exit event");

        }


        void OnMouseExit()
        {
            parent.hideAndCleanGuideInfo();
        }



        void Awake()
        {
            this.background = this.transform.Find("background").GetComponent<Image>();
            this.constructionNameLabel = this.transform.Find("constructionNameLabel").GetComponent<Text>();
            this.clickEffectButton = this.transform.Find("clickEffectButton").GetComponent<TextButton>();
            this.upWorkingLevelButton = this.transform.Find("group/upWorkingLevelButton").GetComponent<TextButton>();
            this.workingLevelLabel = this.transform.Find("group/workingLevelLabel").GetComponent<Text>();
            this.downWorkingLevelButton = this.transform.Find("group/downWorkingLevelButton").GetComponent<TextButton>();
            
        }

        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent, int index)
        {

            this.parent = parent;


            clickEffectButton.button.onClick.AddListener(() => {
            
                parent.game.frontend.log(this.getClass().getSimpleName(), "clicked");
                parent.game.idleGameplayExport.constructionOnClick(model.id);

            });


            // ------ changeWorkingLevelGroup ------
            downWorkingLevelButton.button.onClick.AddListener(() => {
                parent.game.frontend.log(this.getClass().getSimpleName(), "level down clicked");
                parent.game.idleGameplayExport.constructionChangeWorkingLevel(model.id, -1);
            });


            
            upWorkingLevelButton.button.onClick.AddListener(() => {

                parent.game.frontend.log(this.getClass().getSimpleName(), "level up clicked");
                parent.game.idleGameplayExport.constructionChangeWorkingLevel(model.id, 1);

            });



            background.sprite = parent.game.textureManager.defaultBoardNinePatchTexture;
        }


        private void initAsNormalStyle()
        {

            this.upWorkingLevelButton.gameObject.SetActive(false);
            this.downWorkingLevelButton.gameObject.SetActive(false);

            //changeWorkingLevelGroup.setVisible(false);

            //this.debug();
        }


        private void initAsChangeWorkingLevelStyle()
        {
            //clearStyle();

            //changeWorkingLevelGroup.setVisible(true);
            this.upWorkingLevelButton.gameObject.SetActive(true);
            this.downWorkingLevelButton.gameObject.SetActive(true);



        }

        public void setModel(ConstructionExportData constructionExportData)
        {
            this.model = constructionExportData;
            if (constructionExportData != null)
            {
                if (constructionExportData.workingLevelChangable)
                {
                    initAsChangeWorkingLevelStyle();
                }
                else
                {
                    initAsNormalStyle();
                }
            }
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
            constructionNameLabel.text = (model.name);
            clickEffectButton.label.text = (model.buttonDescroption);
            workingLevelLabel.text = (model.workingLevelDescroption);

            // ------ update clickable-state ------
            Boolean canClickEffect = parent.game.idleGameplayExport.constructionCanClickEffect(model.id);
            //clickEffectButton.setTouchable(clickable ? Touchable.enabled : Touchable.disabled);


            if (canClickEffect)
            {
                clickEffectButton.button.interactable = (true);
                //clickEffectButton.SetColor(Color.white);
            }
            else
            {
                clickEffectButton.button.interactable = (false);
                //clickEffectButton.SetColor(Color.red);
            }

            Boolean canUpWorkingLevel = parent.game.idleGameplayExport.constructionCanChangeWorkingLevel(model.id, 1);
            if (canUpWorkingLevel)
            {
                upWorkingLevelButton.button.interactable = (true);
                //upWorkingLevelButton.getLabel().setColor(Color.WHITE);
            }
            else
            {
                upWorkingLevelButton.button.interactable = (false);
                //upWorkingLevelButton.getLabel().setColor(Color.RED);
            }

            Boolean canDownWorkingLevel = parent.game.idleGameplayExport.constructionCanChangeWorkingLevel(model.id, -1);
            if (canDownWorkingLevel)
            {
                downWorkingLevelButton.button.interactable = (true);
                //downWorkingLevelButton.getLabel().setColor(Color.WHITE);
            }
            else
            {
                downWorkingLevelButton.button.interactable = (false);
                //downWorkingLevelButton.getLabel().setColor(Color.RED);
            }
            // ------ update model ------
            //model.onLogicFrame();

        }
    }
}
