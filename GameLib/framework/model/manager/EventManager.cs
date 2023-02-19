using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class EventManager
    {
        List<IBuffChangeListener> buffChangeListeners = new List<IBuffChangeListener>();
        List<IAchievementUnlockCallback> achievementUnlockListeners = new List<IAchievementUnlockCallback>();
        List<IOneFrameResourceChangeListener> oneFrameResourceChangeListeners = new List<IOneFrameResourceChangeListener>();

        IdleGameplayContext gameContext;


        public EventManager(IdleGameplayContext gameContext)
        {
            this.gameContext = gameContext;
        }

        public void registerListener(Object listener)
        {
            if (listener is IBuffChangeListener && !buffChangeListeners.Contains(listener)) {
                buffChangeListeners.Add((IBuffChangeListener)listener);
            }
            if (listener is IAchievementUnlockCallback && !achievementUnlockListeners.Contains(listener)) {
                achievementUnlockListeners.Add((IAchievementUnlockCallback)listener);
            }
            if (listener is IOneFrameResourceChangeListener && !oneFrameResourceChangeListeners.Contains(listener)) {
                oneFrameResourceChangeListeners.Add((IOneFrameResourceChangeListener)listener);
            }
        }

        public void notifyBuffChange()
        {
            gameContext.frontend.log(this.getClass().getSimpleName(), "notifyBuffChange");
            foreach (IBuffChangeListener listener in buffChangeListeners)
            {
                listener.onBuffChange();
            }
        }

        //    public void notifyResourceAmountChange(boolean fromLoad) {
        //        Gdx.app.log(this.getClass().getSimpleName(), "notifyResourceAmountChange");
        //        for (IAmountChangeEventListener listener : amountChangeEventListeners) {
        //            listener.onResourceChange(fromLoad);
        //        }
        //    }

        public void notifyOneFrameResourceChange(Dictionary<String, long> changeMap)
        {
            //Gdx.app.log(this.getClass().getSimpleName(), "notifyOneFrameResourceChange");
            foreach (IOneFrameResourceChangeListener listener in oneFrameResourceChangeListeners)
            {
                listener.onResourceChange(changeMap);
            }
        }

        public void notifyAchievementUnlock(AchievementPrototype prototype)
        {
            gameContext.frontend.log(this.getClass().getSimpleName(), "notifyAchievementUnlock");
            foreach (IAchievementUnlockCallback listener in achievementUnlockListeners)
            {
                listener.onAchievementUnlock(prototype);
            }
        }
    }
}
