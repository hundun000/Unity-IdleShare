
namespace hundun.idleshare.gamelib
{
    /**
     * @author hundun
     * Created on 2023/02/17
     */
    public interface ISecondaryInfoBoardCallback<T> {
        void showAndUpdateGuideInfo(T model);
        void hideAndCleanGuideInfo();
    }

}