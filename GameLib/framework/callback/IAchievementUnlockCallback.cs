using System;

namespace hundun.idleshare.gamelib
{
    /**
 * @author hundun
 * Created on 2022/01/21
 */
    public interface IAchievementUnlockCallback
    {
        void hideAchievementMaskBoard();
        void showAchievementMaskBoard(AbstractAchievement prototype);
    }
}



