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
        List<INotificationBoardCallerAndCallback> notificationBoardCallerAndCallbacks = new List<INotificationBoardCallerAndCallback>();
        List<IOneFrameResourceChangeListener> oneFrameResourceChangeListeners = new List<IOneFrameResourceChangeListener>();
        List<IConstructionCollectionListener> constructionCollectionListeners = new List<IConstructionCollectionListener>();

        
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
            if (listener is INotificationBoardCallerAndCallback && !notificationBoardCallerAndCallbacks.Contains(listener))
            {
                notificationBoardCallerAndCallbacks.Add((INotificationBoardCallerAndCallback)listener);
            }
            if (listener is IOneFrameResourceChangeListener && !oneFrameResourceChangeListeners.Contains(listener)) {
                oneFrameResourceChangeListeners.Add((IOneFrameResourceChangeListener)listener);
            }
            if (listener is IConstructionCollectionListener && !constructionCollectionListeners.Contains(listener))
            {
                constructionCollectionListeners.Add((IConstructionCollectionListener)listener);
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

        public void notifyAchievementUnlock(AbstractAchievement prototype)
        {
            gameContext.frontend.log(this.getClass().getSimpleName(), "notifyAchievementUnlock");
            foreach (IAchievementUnlockCallback listener in achievementUnlockListeners)
            {
                listener.showAchievementMaskBoard(prototype);
            }
        }

        public void notifyNotification(String data)
        {
            gameContext.frontend.log(this.getClass().getSimpleName(), "notifyNotification");
            foreach (INotificationBoardCallerAndCallback listener in notificationBoardCallerAndCallbacks)
            {
                listener.showNotificationMaskBoard(data);
            }
        }

        public void notifyConstructionCollectionChange()
        {
            //Gdx.app.log(this.getClass().getSimpleName(), "notifyOneFrameResourceChange");
            foreach (IConstructionCollectionListener listener in constructionCollectionListeners)
            {
                listener.onConstructionCollectionChange();
            }
        }
    }
}
