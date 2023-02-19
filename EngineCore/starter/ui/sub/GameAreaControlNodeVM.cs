using Assets.Scripts.DemoGameCore.logic;
using hundun.idleshare.enginecore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public class GameAreaControlNodeVM<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;
        //Image image;
        String gameArea;

        Button button;
        Image image;
        RectTransform rectTransform;
        void Awake()
        {
            this.button = this.transform.GetComponent<Button>();
            this.image = this.transform.GetComponent<Image>();

            this.rectTransform = this.image.gameObject.GetComponent<RectTransform>();
        }

        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent, String gameArea, Boolean longVersion)
        {
            this.parent = parent;
            this.gameArea = gameArea;

            rebuildImage(longVersion);
            this.button.onClick.AddListener(() => {
                parent.setAreaAndNotifyChildren(gameArea);
            });

        }

        private Sprite rebuildImage(Boolean longVersion)
        {
            Sprite drawable = parent.game.textureManager.getGameAreaTexture(gameArea, longVersion);
            return drawable;

        }

        public void changeVersion(Boolean longVersion)
        {
            image.sprite = (rebuildImage(longVersion));

            //rectTransform.sizeDelta = new Vector2(longVersion ? 150 : 100, rectTransform.sizeDelta.y);
        }
    }
}
