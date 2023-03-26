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
        BaseConstruction model;

        Text constructionNameLabel;
        TextButton upWorkingLevelButton;
        TextButton downWorkingLevelButton;
        Text workingLevelLabel;
        Text proficiencyLabel;
        Text positionLabel;

        TextButton clickEffectButton;
        TextButton destoryButton;
        TextButton transformButton;
        Image background;
        
        virtual protected void OnMouseEnter()
        {
            if (model != null)
            {
                parent.showAndUpdateGuideInfo(model);
            }
            parent.game.frontend.log(this.getClass().getSimpleName(), "exit event");

        }


        virtual protected void OnMouseExit()
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
            this.proficiencyLabel = this.transform.Find("proficiencyLabel").GetComponent<Text>();
            this.positionLabel = this.transform.Find("positionLabel").GetComponent<Text>();
            this.destoryButton = this.transform.Find("destoryButton").GetComponent<TextButton>();
            this.transformButton = this.transform.Find("transformButton").GetComponent<TextButton>();
        }

        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent)
        {

            this.parent = parent;


            clickEffectButton.button.onClick.AddListener(() => {
            
                parent.game.frontend.log(this.getClass().getSimpleName(), "clickEffectButton clicked");
                parent.game.idleGameplayExport.constructionOnClick(model.id);

            });
            destoryButton.button.onClick.AddListener(() => {

                parent.game.frontend.log(this.getClass().getSimpleName(), "destoryButton clicked");
                parent.game.idleGameplayExport.destoryConstruction(model.id, null);

            });
            transformButton.button.onClick.AddListener(() => {

                parent.game.frontend.log(this.getClass().getSimpleName(), "transformButton clicked");
                parent.game.idleGameplayExport.transformConstruction(model.id);

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

        public void setModel(BaseConstruction constructionExportData)
        {
            this.model = constructionExportData;
            if (constructionExportData != null)
            {
                if (constructionExportData.levelComponent.workingLevelChangable)
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
            clickEffectButton.label.text = (model.descriptionPackage.buttonDescroption);
            workingLevelLabel.text = (model.levelComponent.getWorkingLevelDescroption());
            proficiencyLabel.text = (model.proficiencyComponent.getProficiencyDescroption());
            positionLabel.text = (model.saveData.position.toShowText());
            destoryButton.label.text = (model.descriptionPackage.destroyButtonDescroption);

            // ------ update clickable-state ------
            if (model.canClickEffect())
            {
                clickEffectButton.button.interactable = (true);
            }
            else
            {
                clickEffectButton.button.interactable = (false);
            }
            if (model.canDestory())
            {
                destoryButton.button.interactable = (true);
            }
            else
            {
                destoryButton.button.interactable = (false);
            }
            if (model.upgradeComponent.canTransfer())
            {
                transformButton.button.interactable = (true);
            }
            else
            {
                transformButton.button.interactable = (false);
            }

            Boolean canUpWorkingLevel = parent.game.idleGameplayExport.constructionCanChangeWorkingLevel(model.id, 1);
            if (canUpWorkingLevel)
            {
                upWorkingLevelButton.button.interactable = (true);
            }
            else
            {
                upWorkingLevelButton.button.interactable = (false);
            }

            Boolean canDownWorkingLevel = parent.game.idleGameplayExport.constructionCanChangeWorkingLevel(model.id, -1);
            if (canDownWorkingLevel)
            {
                downWorkingLevelButton.button.interactable = (true);
            }
            else
            {
                downWorkingLevelButton.button.interactable = (false);
            }

        }
    }
}
