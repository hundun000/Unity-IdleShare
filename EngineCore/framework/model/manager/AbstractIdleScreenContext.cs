using hundun.unitygame.enginecorelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.enginecore
{
    public abstract class AbstractIdleScreenContext<T_GAME, T_SAVE> where T_GAME : BaseHundunGame<T_GAME, T_SAVE>
    {
        protected T_GAME game;

        public AbstractIdleScreenContext(T_GAME game)
        {
            this.game = game;
        }

        public abstract void lazyInit();

    }
}
