using Assets.Scripts.DemoGameCore.logic;
using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using hundun.unitygame.enginecorelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.EventSystems.EventTrigger;
using System.Xml.Linq;
using UnityEngine;

namespace hundun.idleshare.enginecore
{
    public class GameEntityManager<T_GAME, T_SAVE> where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        private BaseIdleGame<T_GAME, T_SAVE> game;
        
        public Dictionary<String, List<GameEntity>> gameEntitiesOfConstructionIds = new Dictionary<String, List<GameEntity>>();

        public Dictionary<String, List<GameEntity>> gameEntitiesOfResourceIds = new Dictionary<String, List<GameEntity>>();

        public Dictionary<String, List<String>> areaShowEntityByOwnAmountConstructionIds;

        public Dictionary<String, List<String>> areaShowEntityByOwnAmountResourceIds;

        public Dictionary<String, List<String>> areaShowEntityByChangeAmountResourceIds;


        public void lazyInit(BaseIdleGame<T_GAME, T_SAVE> game, Dictionary<String, List<String>> areaShowEntityByOwnAmountConstructionIds,
                Dictionary<String, List<String>> areaShowEntityByOwnAmountResourceIds,
                Dictionary<String, List<String>> areaShowEntityByChangeAmountResourceIds)
        {
            this.game = game;
            this.areaShowEntityByOwnAmountConstructionIds = areaShowEntityByOwnAmountConstructionIds;
            this.areaShowEntityByOwnAmountResourceIds = areaShowEntityByOwnAmountResourceIds;
            this.areaShowEntityByChangeAmountResourceIds = areaShowEntityByChangeAmountResourceIds;
        }

        public void allEntityMoveForFrame()
        {
            foreach (KeyValuePair<String, List<GameEntity>> entry in gameEntitiesOfConstructionIds)
            {
                List<GameEntity> queue = entry.Value;
                queue.ForEach(entity => {
                    entity.frameLogic();
                    positionChange(entity);
                });
            }

            foreach (KeyValuePair<String, List<GameEntity>> entry in gameEntitiesOfResourceIds) {
                List<GameEntity> queue = entry.Value;
                queue.RemoveAll(entity => {
                    Boolean remove = entity.checkRemove();
                    if (remove)
                    {
                        game.frontend.log(this.getClass().getSimpleName(), "entity removed by self check");
                        UnityEngine.Object.Destroy(entity.gameObject);
                    }
                    return remove;
                });
                queue.ForEach(entity => {
                    entity.frameLogic();
                    positionChange(entity);
                });
            }
        }

        public void areaEntityCheckByOwnAmount(String gameArea, BaseGameEntityFactory<T_GAME, T_SAVE> gameEntityFactory)
        {
            List<String> shownConstructionIds = this.areaShowEntityByOwnAmountConstructionIds.get(gameArea);
            if (shownConstructionIds != null)
            {
                foreach (String shownConstructionId in shownConstructionIds) {
                    checkConstructionEntityByOwnAmount(shownConstructionId, gameEntityFactory);
                }
            }

            List<String> shownResourceIds = this.areaShowEntityByOwnAmountResourceIds.get(gameArea);
            if (shownResourceIds != null)
            {
                foreach (String resourceId in shownResourceIds)
                {
                    checkResourceEntityByOwnAmount(resourceId, gameEntityFactory);
                }
            }
        }

        public void areaEntityCheckByChangeAmount(String gameArea, BaseGameEntityFactory<T_GAME, T_SAVE> gameEntityFactory, Dictionary<String, long> changeMap)
        {

            List<String> shownResourceIds = this.areaShowEntityByChangeAmountResourceIds.get(gameArea);
            if (shownResourceIds != null)
            {
                foreach (String resourceId in shownResourceIds) {
                    if (changeMap.getOrDefault(resourceId, 0L) > 0)
                    {
                        addResourceEntityByChangeAmount(resourceId, gameEntityFactory, ((int)changeMap.get(resourceId)));
                    }
                }
            }
        }

        private void positionChange(GameEntity entity)
        {
            if (entity.moveable)
            {
                entity.x = (entity.x + entity.speedX);
                entity.y = (entity.y + entity.speedY);
            }
        }

        private void checkResourceEntityByOwnAmount(String resourceId, BaseGameEntityFactory<T_GAME, T_SAVE> gameEntityFactory)
        {
            long resourceNum = game.idleGameplayExport.getResourceNumOrZero(resourceId);
            int drawNum = gameEntityFactory.calculateResourceDrawNum(resourceId, resourceNum);

            gameEntitiesOfResourceIds.computeIfAbsent(resourceId, k => new List<GameEntity>());
            List<GameEntity> gameEntities = gameEntitiesOfResourceIds.get(resourceId);
            while (gameEntities.size() > drawNum)
            {
                game.frontend.log(this.getClass().getSimpleName(), "checkResourceEntityByOwnAmount " + resourceId + " remove, current = " + gameEntities.size() + " , target = " + drawNum);
                gameEntities.RemoveAt(gameEntities.size() - 1);
            }
            while (gameEntities.size() < drawNum)
            {
                int newIndex = gameEntities.size();
                GameEntity gameEntity = gameEntityFactory.newResourceEntity(resourceId, newIndex);
                if (gameEntity != null)
                {
                    game.frontend.log(this.getClass().getSimpleName(), "checkResourceEntityByOwnAmount " + resourceId + " new, current = " + gameEntities.size() + " , target = " + drawNum);
                    gameEntities.Add(gameEntity);
                }
                else
                {
                    break;
                }
            }
        }

        private void addResourceEntityByChangeAmount(String resourceId, BaseGameEntityFactory<T_GAME, T_SAVE> gameEntityFactory, int addAmount)
        {
            int drawNum = addAmount;

            gameEntitiesOfResourceIds.computeIfAbsent(resourceId, k => new List<GameEntity>());
            List<GameEntity> gameEntities = gameEntitiesOfResourceIds.get(resourceId);
            for (int i = 0; i < drawNum; i++)
            {
                GameEntity gameEntity = gameEntityFactory.newResourceEntity(resourceId, i);
                if (gameEntity != null)
                {
                    gameEntities.Add(gameEntity);
                    game.frontend.log(this.getClass().getSimpleName(), "addResourceEntityByChangeAmount " + resourceId + " new, target = " + drawNum);
                }
                else
                {
                    break;
                }
            }
        }

        private void checkConstructionEntityByOwnAmount(String id, BaseGameEntityFactory<T_GAME, T_SAVE> gameEntityFactory)
        {
            BaseConstruction construction = game.idleGameplayExport.getConstruction(id);
            int resourceNum = construction.saveData.workingLevel;
            int MAX_DRAW_NUM = construction.maxDrawNum;
            int drawNum = gameEntityFactory.calculateConstructionDrawNum(id, resourceNum, MAX_DRAW_NUM);
            gameEntitiesOfConstructionIds.computeIfAbsent(id, k => new List<GameEntity>());
            List<GameEntity> gameEntities = gameEntitiesOfConstructionIds.get(id);
            while (gameEntities.size() > drawNum)
            {
                game.frontend.log(this.getClass().getSimpleName(), "checkConstructionEntityByOwnAmount " + id + " remove, current = " + gameEntities.size() + " , target = " + drawNum);
                gameEntities.RemoveAt(gameEntities.size() - 1);
            }
            while (gameEntities.size() < drawNum)
            {
                int newIndex = gameEntities.size();
                GameEntity gameEntity = gameEntityFactory.newConstructionEntity(id, newIndex);
                if (gameEntity != null)
                {
                    game.frontend.log(this.getClass().getSimpleName(), "checkConstructionEntityByOwnAmount " + id + " new, current = " + gameEntities.size() + " , target = " + drawNum);
                    gameEntities.Add(gameEntity);
                }
                else
                {
                    //Gdx.app.log(this.getClass().getSimpleName(), "checkConstructionEntityByOwnAmount " + id + " , cannot create new entity.");
                    break;
                }
            }
        }

    }
}
