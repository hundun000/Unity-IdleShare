using hundun.unitygame.adapters;
using hundun.unitygame.enginecorelib;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public abstract class BaseIdleMenuScreen<T_GAME, T_SAVE> : BaseHundunScreen<T_GAME, T_SAVE> where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        private JRunable buttonContinueGameInputListener;
        private JRunable buttonNewGameInputListener;


        protected GameObject UiRoot { get; private set; }


        protected Text title;
        protected Button buttonContinueGame;
        protected Button buttonNewGame;
        

        virtual public void postMonoBehaviourInitialization(T_GAME game,
            JRunable buttonContinueGameInputListener,
            JRunable buttonNewGameInputListener
            )
        {
            base.postMonoBehaviourInitialization(game);

            this.buttonContinueGameInputListener = buttonContinueGameInputListener;
            this.buttonNewGameInputListener = buttonNewGameInputListener;
        }

        override public void show()
        {
            UiRoot = gameObject.transform.Find("_uiRoot").gameObject;

            this.title = this.UiRoot.transform.Find("title").gameObject.GetComponent<Text>();
            this.buttonContinueGame = this.UiRoot.transform.Find("buttonContinueGame").gameObject.GetComponent<Button>();
            this.buttonNewGame = this.UiRoot.transform.Find("buttonNewGame").gameObject.GetComponent<Button>();

            List<String> memuScreenTexts = game.idleGameplayExport.gameDictionary.getMemuScreenTexts(game.idleGameplayExport.language);

            title.text = JavaFeatureForGwt.stringFormat("[     %s     ]", memuScreenTexts[0]);

            buttonContinueGame.transform.Find("text").GetComponent<Text>().text = memuScreenTexts[2];
            buttonContinueGame.onClick.AddListener(buttonContinueGameInputListener.Invoke);

            buttonNewGame.transform.Find("text").GetComponent<Text>().text = memuScreenTexts[1];
            buttonNewGame.onClick.AddListener(buttonNewGameInputListener.Invoke);

            if (!game.saveHandler.gameHasSave())
            {
                buttonContinueGame.gameObject.SetActive(false);
            } 
            else
            {
                buttonContinueGame.gameObject.SetActive(true);
            }
        }
    }
}
