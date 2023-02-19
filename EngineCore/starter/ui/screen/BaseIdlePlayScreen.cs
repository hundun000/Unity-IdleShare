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
    public abstract class BaseIdlePlayScreen<T_GAME, T_SAVE> : BaseHundunScreen<T_GAME, T_SAVE> where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {




        protected GameObject Contrainer { get; private set; }
        protected GameObject PopoupRoot { get; private set; }
        protected GameObject UiRoot { get; private set; }
        protected GameObject Templates { get; private set; }

        protected IdleScreenBackgroundVM screenBackgroundVM;
        protected StorageInfoBoardVM<T_GAME, T_SAVE> storageInfoBoardVM;
        protected FixedConstructionControlBoardVM<T_GAME, T_SAVE> constructionControlBoardVM;
        protected PopupInfoBoard<T_GAME, T_SAVE> secondaryInfoBoard;

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
        }

        virtual protected void lazyInitLogicContext()
        {
            logicFrameListeners.Add(constructionControlBoardVM);
            logicFrameListeners.Add(game.idleGameplayExport);

            gameAreaChangeListeners.Add(screenBackgroundVM);
            gameAreaChangeListeners.Add(constructionControlBoardVM);
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

        internal void showAndUpdateGuideInfo(ConstructionExportData model)
        {
            secondaryInfoBoard.gameObject.SetActive(true);
            secondaryInfoBoard.update(model);
        }

        internal void hideAndCleanGuideInfo()
        {
            secondaryInfoBoard.gameObject.SetActive(false);
        }
    }
}


