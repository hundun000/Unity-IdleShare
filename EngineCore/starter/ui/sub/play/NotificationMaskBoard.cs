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
    public class NotificationMaskBoard<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;
        Text label;
        Button button;
        Text buttonText;
        Image background;

        void Awake()
        {
            this.background = this.transform.Find("background").GetComponent<Image>();
            this.label = this.transform.Find("label").GetComponent<Text>();
            this.button = this.transform.Find("button").GetComponent<Button>();
            this.buttonText = this.transform.Find("button/text").GetComponent<Text>();
        }

        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent)
        {
            this.parent = parent;
            this.background.sprite = parent.game.textureManager.achievementMaskBoardTexture;

            this.buttonText.text = "OK";
            this.button.onClick.AddListener(() => {
                parent.hideNotificationMaskBoard();
            });

        }


        internal void setData(string data)
        {
            label.text = data;
        }
    }
}
