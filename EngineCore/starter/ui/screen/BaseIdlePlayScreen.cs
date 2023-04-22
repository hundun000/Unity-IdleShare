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
        BaseHundunScreen<T_GAME, T_SAVE>, 
        IAchievementUnlockCallback, 
        INotificationBoardCallerAndCallback, 
        ISecondaryInfoBoardCallback<BaseConstruction> 
        where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        // ----- unity adapter ------
        protected GameObject Contrainer { get; private set; }
        protected GameObject PopupRoot { get; private set; }
        protected GameObject UiRoot { get; private set; }
        protected GameObject Templates { get; private set; }
        protected AudioSource audioSource;

        // ----- ui ------
        //protected IdleScreenBackgroundVM screenBackgroundVM;
        
        // ----- popup ui ------
        protected AchievementMaskBoard<T_GAME, T_SAVE> achievementMaskBoard;
        protected NotificationMaskBoard<T_GAME, T_SAVE> notificationMaskBoard;
        protected PopupInfoBoardVM<T_GAME, T_SAVE> popupInfoBoardVM;

        // ----- not ui ------
        public GameEntityManager<T_GAME, T_SAVE> gameEntityManager { get; protected set; }
        public String area { get; private set; }
        private String startArea;

        protected List<ILogicFrameListener> logicFrameListeners;
        protected List<IGameAreaChangeListener> gameAreaChangeListeners;

        virtual protected void Awake()
        {
            Contrainer = this.gameObject;
            PopupRoot = this.transform.Find("_popupRoot").gameObject;
            UiRoot = this.transform.Find("_uiRoot").gameObject;
            Templates = this.transform.Find("_templates").gameObject;
            audioSource = this.transform.Find("_audioSource").GetComponent<AudioSource>();

            //this.screenBackgroundVM = this.Contrainer.transform.Find("ScreenBackgroundVM").gameObject.GetComponent<IdleScreenBackgroundVM>();
            
            this.popupInfoBoardVM = this.PopupRoot.transform.Find("PopupInfoBoardVM").gameObject.GetComponent<PopupInfoBoardVM<T_GAME, T_SAVE>>();
            this.achievementMaskBoard = this.PopupRoot.transform.Find("AchievementMaskBoard").gameObject.GetComponent<AchievementMaskBoard<T_GAME, T_SAVE>>();
            this.notificationMaskBoard = this.PopupRoot.transform.Find("NotificationMaskBoard").gameObject.GetComponent<NotificationMaskBoard<T_GAME, T_SAVE>>();
        }

        virtual public void postMonoBehaviourInitialization(T_GAME game, String startArea,
                int LOGIC_FRAME_PER_SECOND
                )
        {
            base.postMonoBehaviourInitialization(game);
            this.startArea = startArea;
            this.logicFrameHelper = new LogicFrameHelper(LOGIC_FRAME_PER_SECOND);

            this.logicFrameListeners = new List<ILogicFrameListener>();
            this.gameAreaChangeListeners = new List<IGameAreaChangeListener>();
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
            this.gameEntityManager = new GameEntityManager<T_GAME, T_SAVE>();
            gameEntityManager.lazyInit(this.game, 
                game.childGameConfig.areaShowEntityByOwnAmountConstructionPrototypeIds, 
                game.childGameConfig.areaShowEntityByOwnAmountResourceIds, 
                game.childGameConfig.areaShowEntityByChangeAmountResourceIds);

            
            logicFrameListeners.Add(game.idleGameplayExport);

            //gameAreaChangeListeners.Add(screenBackgroundVM);

            this.game.idleGameplayExport.eventManagerRegisterListener(this);
        }

        virtual protected void dispose()
        {
            this.game.idleGameplayExport.eventManagerUnregisterListener(this);
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

            if (logicFrameHelper.clockCount % logicFrameHelper.secondToFrameNum(10) == 0)
            {
                game.saveHandler.gameSaveCurrent();
            }
        }

        public void showAndUpdateGuideInfo(BaseConstruction model)
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

        }

        virtual public void hideAchievementMaskBoard()
        {
            game.frontend.log(this.getClass().getSimpleName(), "hideAchievementMaskBoard called");
            achievementMaskBoard.gameObject.SetActive(false);
            logicFrameHelper.logicFramePause = false;
        }

        virtual public void showAchievementMaskBoard(AbstractAchievement prototype)
        {
            game.frontend.log(this.getClass().getSimpleName(), "showAchievementMaskBoard called");
            achievementMaskBoard.gameObject.SetActive(true);
            achievementMaskBoard.setAchievementPrototype(prototype);
            logicFrameHelper.logicFramePause = true;
        }

        public void hideNotificationMaskBoard()
        {
            game.frontend.log(this.getClass().getSimpleName(), "hideNotificationMaskBoard called");
            notificationMaskBoard.gameObject.SetActive(false);
            logicFrameHelper.logicFramePause = false;
        }

        public void showNotificationMaskBoard(String data)
        {
            game.frontend.log(this.getClass().getSimpleName(), "showNotificationMaskBoard called");
            notificationMaskBoard.gameObject.SetActive(true);
            notificationMaskBoard.setData(data);
            logicFrameHelper.logicFramePause = true;
        }

    }

}


