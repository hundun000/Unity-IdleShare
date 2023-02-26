using hundun.idleshare.gamelib;
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
        protected AudioSource audioSource;

        private IdleScreenBackgroundVM screenBackgroundVM;
        protected Text title;
        protected Button buttonContinueGame;
        protected Button buttonNewGame;
        protected LanguageSwitchBoardVM<T_GAME, T_SAVE> languageSwitchBoardVM;

        virtual public void postMonoBehaviourInitialization(T_GAME game,
            JRunable buttonContinueGameInputListener,
            JRunable buttonNewGameInputListener
            )
        {
            base.postMonoBehaviourInitialization(game);
            screenBackgroundVM.postPrefabInitialization(this.game.textureManager);
            this.buttonContinueGameInputListener = buttonContinueGameInputListener;
            this.buttonNewGameInputListener = buttonNewGameInputListener;
        }

        void Awake()
        {
            UiRoot = gameObject.transform.Find("_uiRoot").gameObject; 
            audioSource = this.transform.Find("_audioSource").GetComponent<AudioSource>();

            this.title = this.UiRoot.transform.Find("title").gameObject.GetComponent<Text>();
            this.buttonContinueGame = this.UiRoot.transform.Find("buttonContinueGame").gameObject.GetComponent<Button>();
            this.buttonNewGame = this.UiRoot.transform.Find("buttonNewGame").gameObject.GetComponent<Button>();
            this.languageSwitchBoardVM = this.UiRoot.transform.Find("languageSwitchBoardVM").gameObject.GetComponent<LanguageSwitchBoardVM<T_GAME, T_SAVE>>();
            this.screenBackgroundVM = gameObject.transform.transform.Find("ScreenBackgroundVM").gameObject.GetComponent<IdleScreenBackgroundVM>();

        }

        override public void show()
        {
            
            List<String> memuScreenTexts = game.idleGameplayExport.gameDictionary.getMemuScreenTexts(game.idleGameplayExport.language);

            title.text = JavaFeatureForGwt.stringFormat("[     %s     ]", memuScreenTexts[0]);

            buttonContinueGame.transform.Find("text").GetComponent<Text>().text = memuScreenTexts[2];
            buttonContinueGame.onClick.AddListener(buttonContinueGameInputListener.Invoke);

            buttonNewGame.transform.Find("text").GetComponent<Text>().text = memuScreenTexts[1];
            buttonNewGame.onClick.AddListener(buttonNewGameInputListener.Invoke);

            if (!game.saveHandler.hasContinuedGameplaySave())
            {
                buttonContinueGame.gameObject.SetActive(false);
            } 
            else
            {
                buttonContinueGame.gameObject.SetActive(true);
            }

            languageSwitchBoardVM.postPrefabInitialization(
                this,
                Enum.GetValues(typeof(Language)).OfType<Language>().ToArray(),
                game.idleGameplayExport.language,
                memuScreenTexts.get(3),
                memuScreenTexts.get(4),
                it => game.idleGameplayExport.language = it
                );

            // unity adapter
            game.audioPlayManager.intoScreen(audioSource, this.getClass().getSimpleName());
        }
    }
}
