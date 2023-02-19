using hundun.unitygame.adapters;
using hundun.unitygame.enginecorelib;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace hundun.idleshare.enginecore
{
    public abstract class AbstractMenuScreen<T_GAME, T_SAVE> : BaseHundunScreen<T_GAME, T_SAVE> where T_GAME : BaseHundunGame<T_GAME, T_SAVE>
    {
        private readonly String titleText;
        private readonly OnTouchUp buttonContinueGameInputListener;
        private readonly OnTouchUp buttonNewGameInputListener;

        public Text title;
        public Button buttonContinueGame;
        public Button buttonNewGame;

        protected AbstractMenuScreen(T_GAME game,
            String titleText,
            OnTouchUp buttonContinueGameInputListener,
            OnTouchUp buttonNewGameInputListener
            ) : base(game)
        {
            this.titleText = titleText;
            this.buttonContinueGameInputListener = buttonContinueGameInputListener;
            this.buttonNewGameInputListener = buttonNewGameInputListener;
        }

        override public void show()
        {

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
