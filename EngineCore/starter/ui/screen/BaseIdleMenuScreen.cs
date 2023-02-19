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
    public abstract class BaseIdleMenuScreen<T_GAME, T_SAVE> : BaseHundunScreen<T_GAME, T_SAVE> where T_GAME : BaseHundunGame<T_GAME, T_SAVE>
    {
        private String titleText;
        private JRunable buttonContinueGameInputListener;
        private JRunable buttonNewGameInputListener;


        protected GameObject UiRoot { get; private set; }


        protected Text title;
        protected Button buttonContinueGame;
        protected Button buttonNewGame;
        

        virtual public void postMonoBehaviourInitialization(T_GAME game,
            String titleText,
            JRunable buttonContinueGameInputListener,
            JRunable buttonNewGameInputListener
            )
        {
            base.postMonoBehaviourInitialization(game);

            this.titleText = titleText;
            this.buttonContinueGameInputListener = buttonContinueGameInputListener;
            this.buttonNewGameInputListener = buttonNewGameInputListener;
        }

        override public void show()
        {
            UiRoot = gameObject.transform.Find("_uiRoot").gameObject;

            this.title = this.UiRoot.transform.Find("title").gameObject.GetComponent<Text>();
            this.buttonContinueGame = this.UiRoot.transform.Find("buttonContinueGame").gameObject.GetComponent<Button>();
            this.buttonNewGame = this.UiRoot.transform.Find("buttonNewGame").gameObject.GetComponent<Button>();


            title.text = JavaFeatureForGwt.stringFormat("[     %s     ]", titleText);

            buttonContinueGame.transform.Find("text").GetComponent<Text>().text = "Countine";
            buttonContinueGame.onClick.AddListener(buttonContinueGameInputListener.Invoke);

            buttonNewGame.transform.Find("text").GetComponent<Text>().text = "New";
            buttonNewGame.onClick.AddListener(buttonNewGameInputListener.Invoke);

            if (game.saveHandler.gameHasSave())
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
