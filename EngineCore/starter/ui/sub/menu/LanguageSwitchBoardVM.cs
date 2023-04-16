using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using hundun.unitygame.enginecorelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public class LanguageSwitchBoardVM<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        protected BaseHundunScreen<T_GAME, T_SAVE> parent;
        protected Dropdown selectBox;
        protected Text label;
        protected Text restartHintLabel;
        protected Dictionary<Language, string> languageShowNameMap;

        void Awake()
        {
            this.label = this.transform.Find("label").GetComponent<Text>();
            this.selectBox = this.transform.Find("selectBox").GetComponent<Dropdown>();
            this.restartHintLabel = this.transform.Find("restartHintLabel").GetComponent<Text>();
        }

        virtual public void postPrefabInitialization(BaseHundunScreen<T_GAME, T_SAVE> parent)
        {
            this.parent = parent;
            this.GetComponent<Image>().sprite = (parent.game.textureManager.defaultBoardNinePatchTexture);
            this.languageShowNameMap = parent.game.idleGameplayExport.gameDictionary.getLanguageShowNameMap();


        }


    }
}
