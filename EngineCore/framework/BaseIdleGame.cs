using hundun.idleshare.gamelib;
using hundun.unitygame.enginecorelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace hundun.idleshare.enginecore
{
    public abstract class BaseIdleGame<T_GAME, T_SAVE> : BaseHundunGame<T_GAME, T_SAVE> where T_GAME : BaseHundunGame<T_GAME, T_SAVE>
    {
        public AbstractIdleScreenContext<T_GAME, T_SAVE> screenContext { get; protected set; }
        public AudioPlayManager<T_GAME, T_SAVE> audioPlayManager { get; protected set; }
        protected AbstractTextureManager textureManager;

        protected IdleGameplayExport idleGameplayExport;
        protected ChildGameConfig childGameConfig;


        public BaseIdleGame(int viewportWidth, int viewportHeight) : base(viewportWidth, viewportHeight)
        {
        }

        override protected void createStage2()
        {
            textureManager.lazyInitOnGameCreateStage2();
            screenContext.lazyInit();
        }

        override protected void createStage3()
        {

            //managerContext.lazyInitOnGameCreate(childGameConfig);
            audioPlayManager.lazyInit(childGameConfig.screenIdToFilePathMap);

        }
    }
}
