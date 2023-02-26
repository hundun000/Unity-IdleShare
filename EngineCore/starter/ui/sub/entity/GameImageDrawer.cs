using Assets.Scripts.DemoGameCore.logic;
using hundun.idleshare.enginecore;
using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace hundun.idleshare.enginecore
{
    public class GameImageDrawer<T_GAME, T_SAVE> : IOneFrameResourceChangeListener where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;
        BaseGameEntityFactory<T_GAME, T_SAVE> gameEntityFactory;


        public void lazyInit(BaseIdlePlayScreen<T_GAME, T_SAVE> parent, BaseGameEntityFactory<T_GAME, T_SAVE> gameEntityFactory)
        {
            this.parent = parent;
            this.gameEntityFactory = gameEntityFactory;
        }


        public void allEntitiesMoveForFrameAndDraw()
        {
            GameEntityManager<T_GAME, T_SAVE> manager = parent.gameEntityManager;

            
            String gameArea = parent.area;

            List<String> needDrawConstructionIds = manager.areaShowEntityByOwnAmountConstructionIds.get(gameArea);

            manager.destoryNoNeedDrawConstructionIds(needDrawConstructionIds);
            manager.allEntityMoveForFrame();
            if (needDrawConstructionIds != null)
            {
                foreach (String id in needDrawConstructionIds)
                {
                    List<GameEntity> queue = manager.gameEntitiesOfConstructionIds.get(id);
                    if (queue == null)
                    {
                        continue;
                    }
                    queue.ForEach(entity => {
                        drawToEngine(entity);
                    });
            }
        }

        List<String> needDrawByOwnAmountResourceIds = manager.areaShowEntityByOwnAmountResourceIds.get(gameArea);
        if (needDrawByOwnAmountResourceIds != null) {
            foreach (String id in needDrawByOwnAmountResourceIds) {
                List<GameEntity> queue = manager.gameEntitiesOfResourceIds.get(id);
                if (queue == null) {
                    continue;
                }
                queue.ForEach(entity => {
                    drawToEngine(entity);
                });
            }
        }

        List<String> needDrawByChangeAmountResourceIds = manager.areaShowEntityByChangeAmountResourceIds.get(gameArea);
        if (needDrawByChangeAmountResourceIds != null)
        {
            foreach (String id in needDrawByChangeAmountResourceIds)
            {
                List<GameEntity> queue = manager.gameEntitiesOfResourceIds.get(id);
                if (queue == null)
                {
                    continue;
                }
                queue.ForEach(entity => {
                    drawToEngine(entity);
                });
            }
        }

    }

        private void drawToEngine(GameEntity entity)
        {
            // 设置RectTransform即可，具体绘制由Unity执行

            entity.image.sprite = entity.texture;
            entity.rectTransform.anchoredPosition = new Vector3(entity.x, entity.y, 0);
            Vector3 newScale = entity.transform.localScale;
            newScale.x = entity.textureFlipX ? -1 : 1;
            entity.rectTransform.localScale = newScale;
            entity.rectTransform.sizeDelta = new Vector2(entity.drawWidth, entity.drawHeight);

            
        }


        public void onResourceChange(Dictionary<String, long> changeMap)
        {
            GameEntityManager<T_GAME, T_SAVE> manager = parent.gameEntityManager;
            String gameArea = parent.area;
            manager.areaEntityCheckByOwnAmount(gameArea, gameEntityFactory);
            manager.areaEntityCheckByChangeAmount(gameArea, gameEntityFactory, changeMap);
        }


    }
}
