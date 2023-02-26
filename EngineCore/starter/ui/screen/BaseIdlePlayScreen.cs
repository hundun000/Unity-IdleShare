using Assets.Scripts.DemoGameCore.ui.screen;
using Assets.Scripts.DemoGameCore.ui.sub;
using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using hundun.unitygame.enginecorelib;
using hundun.unitygame.gamelib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hundun.idleshare.enginecore
{
    public abstract class BaseIdlePlayScreen<T_GAME, T_SAVE> : 
        BaseHundunScreen<T_GAME, T_SAVE>, IAchievementUnlockCallback, ISecondaryInfoBoardCallback<ConstructionExportProxy> 
        where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        // ----- unity adapter ------
        protected GameObject Contrainer { get; private set; }
        protected GameObject PopoupRoot { get; private set; }
        protected GameObject UiRoot { get; private set; }
        protected GameObject Templates { get; private set; }
        protected AudioSource audioSource;

        // ----- ui ------
        protected IdleScreenBackgroundVM screenBackgroundVM;
        protected StorageInfoBoardVM<T_GAME, T_SAVE> storageInfoBoardVM;
        protected FixedConstructionControlBoardVM<T_GAME, T_SAVE> constructionControlBoardVM;
        protected GameImageDrawer<T_GAME, T_SAVE> gameImageDrawer;
        protected GameAreaControlBoardVM<T_GAME, T_SAVE> gameAreaControlBoardVM;
        
        // ----- popup ui ------
        protected AchievementMaskBoard<T_GAME, T_SAVE> achievementMaskBoard;
        protected PopupInfoBoardVM<T_GAME, T_SAVE> popupInfoBoardVM;

        // ----- not ui ------
        public GameEntityManager<T_GAME, T_SAVE> gameEntityManager { get; protected set; }
        public String area { get; private set; }
        private String startArea;

        protected List<ILogicFrameListener> logicFrameListeners = new List<ILogicFrameListener>();
        protected List<IGameAreaChangeListener> gameAreaChangeListeners = new List<IGameAreaChangeListener>();

        virtual protected void Awake()
        {
            Contrainer = this.gameObject;
            PopoupRoot = this.transform.Find("_popupRoot").gameObject;
            UiRoot = this.transform.Find("_uiRoot").gameObject;
            Templates = this.transform.Find("_templates").gameObject;
            audioSource = this.transform.Find("_audioSource").GetComponent<AudioSource>();

            this.screenBackgroundVM = this.Contrainer.transform.Find("ScreenBackgroundVM").gameObject.GetComponent<IdleScreenBackgroundVM>();
            this.storageInfoBoardVM = this.UiRoot.transform.Find("cell_0/StorageInfoBoardVM").gameObject.GetComponent<StorageInfoBoardVM<T_GAME, T_SAVE>>();
            this.constructionControlBoardVM = this.UiRoot.transform.Find("cell_1/FixedConstructionControlBoardVM").gameObject.GetComponent<FixedConstructionControlBoardVM<T_GAME, T_SAVE>>();
            this.gameAreaControlBoardVM = this.UiRoot.transform.Find("cell_2/GameAreaControlBoardVM").gameObject.GetComponent<GameAreaControlBoardVM<T_GAME, T_SAVE>>();
            
            this.popupInfoBoardVM = this.PopoupRoot.transform.Find("PopupInfoBoardVM").gameObject.GetComponent<PopupInfoBoardVM<T_GAME, T_SAVE>>();
            this.achievementMaskBoard = this.PopoupRoot.transform.Find("AchievementMaskBoard").gameObject.GetComponent<AchievementMaskBoard<T_GAME, T_SAVE>>();
        }

        virtual public void postMonoBehaviourInitialization(T_GAME game, String startArea,
                int LOGIC_FRAME_PER_SECOND
                )
        {
            base.postMonoBehaviourInitialization(game);
            this.startArea = startArea;
            this.logicFrameHelper = new LogicFrameHelper(LOGIC_FRAME_PER_SECOND);
        }

        public void setAreaAndNotifyChildren(String current)
        {
            String last = this.area;
            this.area = current;

            foreach (IGameAreaChangeListener gameAreaChangeListener in gameAreaChangeListeners)
            {
                gameAreaChangeListener.onGameAreaChange(last, current);
            }

        }


        override public void show()
        {


            lazyInitBackUiAndPopupUiContent();

            lazyInitUiRootContext();

            lazyInitLogicContext();

            // start area
            setAreaAndNotifyChildren(startArea);
            game.frontend.log(this.getClass().getSimpleName(), "show done");

            // unity adapter
            game.audioPlayManager.intoScreen(audioSource, this.getClass().getSimpleName());
        }

        virtual protected void lazyInitLogicContext()
        {
            this.gameImageDrawer = new GameImageDrawer<T_GAME, T_SAVE>();
            this.gameEntityManager = new GameEntityManager<T_GAME, T_SAVE>();
            gameEntityManager.lazyInit(this.game, 
                game.childGameConfig.areaShowEntityByOwnAmountConstructionIds, 
                game.childGameConfig.areaShowEntityByOwnAmountResourceIds, 
                game.childGameConfig.areaShowEntityByChangeAmountResourceIds);

            logicFrameListeners.Add(constructionControlBoardVM);
            logicFrameListeners.Add(game.idleGameplayExport);

            gameAreaChangeListeners.Add(screenBackgroundVM);
            gameAreaChangeListeners.Add(constructionControlBoardVM);
            gameAreaChangeListeners.Add(gameAreaControlBoardVM);

            this.game.idleGameplayExport.eventManagerRegisterListener(this);
            this.game.idleGameplayExport.eventManagerRegisterListener(gameImageDrawer);
        }


        protected abstract void lazyInitUiRootContext();

        protected abstract void lazyInitBackUiAndPopupUiContent();

        override protected void onLogicFrame()
        {
            base.onLogicFrame();

            foreach (ILogicFrameListener logicFrameListener in logicFrameListeners)
            {
                logicFrameListener.onLogicFrame();
            }
        }

        public void showAndUpdateGuideInfo(ConstructionExportProxy model)
        {
            popupInfoBoardVM.gameObject.SetActive(true);
            popupInfoBoardVM.update(model);
        }

        public void hideAndCleanGuideInfo()
        {
            popupInfoBoardVM.gameObject.SetActive(false);
        }

        override protected void gameObjectDraw(float delta)
        {
            gameImageDrawer.allEntitiesMoveForFrameAndDraw();
        }

        public void hideAchievementMaskBoard()
        {
            game.frontend.log(this.getClass().getSimpleName(), "hideAchievementMaskBoard called");
            achievementMaskBoard.gameObject.SetActive(false);
        }

        public void onAchievementUnlock(AchievementPrototype prototype)
        {
            game.frontend.log(this.getClass().getSimpleName(), "onAchievementUnlock called");
            achievementMaskBoard.gameObject.SetActive(true);
            achievementMaskBoard.setAchievementPrototype(prototype);
        }
    }

}


