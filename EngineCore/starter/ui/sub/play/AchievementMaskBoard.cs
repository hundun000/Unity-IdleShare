using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public class AchievementMaskBoard<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;
        Text congratulationLabel;
        TextButton backTextButton;
        Image background;

        void Awake()
        {
            this.background = this.transform.Find("background").GetComponent<Image>();
            this.congratulationLabel = this.transform.Find("congratulationLabel").GetComponent<Text>();
            this.backTextButton = this.transform.Find("button").GetComponent<TextButton>();
        }

        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent)
        {
            this.parent = parent;
            this.background.sprite = parent.game.textureManager.achievementMaskBoardTexture;

            this.backTextButton.label.text = parent.game.idleGameplayExport.gameDictionary.getAchievementTexts(parent.game.idleGameplayExport.language)[2];
            this.backTextButton.button.onClick.AddListener(() => {
                parent.hideAchievementMaskBoard();
            });

        }
        public void setAchievementPrototype(AbstractAchievement prototype)
        {
            congratulationLabel.text = prototype.congratulationText;
        }
    }
}
