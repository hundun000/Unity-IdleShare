using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
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
        BaseIdleMenuScreen<T_GAME, T_SAVE> parent;
        Dropdown selectBox;
        Text label;
        Text restartHintLabel;


        void Awake()
        {
            this.label = this.transform.Find("label").GetComponent<Text>();
            this.selectBox = this.transform.Find("selectBox").GetComponent<Dropdown>();
            this.restartHintLabel = this.transform.Find("restartHintLabel").GetComponent<Text>();
        }

        public void postPrefabInitialization(BaseIdleMenuScreen<T_GAME, T_SAVE> parent,
                Language[] values,
                Language current,
                String startText,
                String hintText,
                JConsumer<Language> onSelect
                )
        {
            this.parent = parent;
            this.GetComponent<Image>().sprite = (parent.game.textureManager.defaultBoardNinePatchTexture);


            selectBox.options = values
                .Select(it => new Dropdown.OptionData(parent.game.idleGameplayExport.gameDictionary.language(it)))
                .ToList();
            selectBox.value = selectBox.options.FindIndex(0, it => it.text.Equals(parent.game.idleGameplayExport.gameDictionary.language(current)));
            selectBox.onValueChanged.AddListener(it =>
            {
                restartHintLabel.gameObject.SetActive(true);
                onSelect.Invoke(parent.game.idleGameplayExport.gameDictionary.unLanguage(selectBox.options[it].text));
            });

            this.label.text = startText;
            this.restartHintLabel.text = hintText;


        }


    }
}
