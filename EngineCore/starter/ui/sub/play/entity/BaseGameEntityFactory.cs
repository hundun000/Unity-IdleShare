using hundun.unitygame.adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace hundun.idleshare.enginecore
{
    public abstract class BaseGameEntityFactory<T_GAME, T_SAVE> : MonoBehaviour where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        public abstract int calculateResourceDrawNum(String resourceId, long logicAmount);
        public abstract int calculateConstructionDrawNum(String constructionid, long logicAmount, int maxDrawNum);
        public abstract GameEntity newResourceEntity(String resourceId, int index);
        public abstract GameEntity newConstructionEntity(String constructionid, int index);

        protected double DEFAULT_CONSTRUCTION_WIDTH_SCALE = 2.0;
        protected double DEFAULT_CONSTRUCTION_HEIGHT_SCALE = 2.0;
        protected double DEFAULT_RESOURCE_WIDTH_SCALE = 1.0;
        protected double DEFAULT_RESOURCE_HEIGHT_SCALE = 1.0;

        protected int COLUMN_STABLE_FIRST_COL_X = 80;
        protected int COLUMN_STABLE_COL_PADDING_X = 90;
        protected int COLUMN_STABLE_FIRST_COL_Y = 150;
        protected int COLUMN_STABLE_INDEX_PADDING_X = 0;
        protected int COLUMN_STABLE_INDEX_PADDING_Y = 30;

        protected int ROW_STABLE_FIRST_COL_X = 32;
        protected int ROW_STABLE_ROW_PADDING_Y = 100;
        protected int ROW_STABLE_FIRST_COL_Y = 50;
        protected int ROW_STABLE_INDEX_PADDING_X = 48;
        protected int ROW_STABLE_INDEX_PADDING_Y = 0;

        protected BaseIdlePlayScreen<T_GAME, T_SAVE> parent;

        private FailingEntity failingEntityTempalte;
        private GameEntity stableEntityTempalte;

        private Transform drawContaioner;

        void Awake()
        {
            this.failingEntityTempalte = this.transform.Find("_templates/FailingEntity").GetComponent<FailingEntity>();
            this.stableEntityTempalte = this.transform.Find("_templates/GameEntity").GetComponent<GameEntity>();
        }

        virtual public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent, Transform drawContaioner)
        {
            this.parent = parent;
            this.drawContaioner = drawContaioner;
        }

        protected GameEntity failingResourcEntity(
                String resourceId,
                int MIN_X, int MAX_X,
                int START_Y, int REMOVE_Y,
                double BASE_FAILING_SPEED,
                double FAILING_SPEED_RANDOM_RANGE,
                int HIDEN_FRAME_RANGE
                )
        {
            Sprite sprite = parent.game.textureManager.getResourceEntity(resourceId);
            MAX_X = (int)(MAX_X - DEFAULT_RESOURCE_WIDTH_SCALE * sprite.textureRect.width);
            REMOVE_Y = (int)(REMOVE_Y + DEFAULT_RESOURCE_HEIGHT_SCALE * sprite.textureRect.height);
            int randX = (int)(MIN_X + Random.Range(0, 1.0f) * (MAX_X - MIN_X));
            double speed = BASE_FAILING_SPEED + Random.Range(0, 1.0f) * FAILING_SPEED_RANDOM_RANGE;
            int hidenFrame = (int)(Random.Range(0, 1.0f) * HIDEN_FRAME_RANGE);

            FailingEntity entity = drawContaioner.AsTableAdd<FailingEntity>(failingEntityTempalte.gameObject);
            entity.postPrefabInitialization(REMOVE_Y, hidenFrame);
            entity.texture = (sprite);
            entity.x = (randX);
            entity.y = (START_Y);
            entity.drawWidth = ((int)(DEFAULT_RESOURCE_WIDTH_SCALE * entity.texture.textureRect.width));
            entity.drawHeight = ((int)(DEFAULT_RESOURCE_HEIGHT_SCALE * entity.texture.textureRect.height));
            entity.moveable = (true);
            entity.speedY = ((float)(-1.0 * speed));
            entity.frameLogic();
            return entity;
        }



        protected GameEntity randomPositionStableConstructionEntity(String constructionId, int MIN_X, int MAX_X, int MIN_Y, int MAX_Y)
        {
            Sprite sprite = parent.game.textureManager.getConstructionEntity(constructionId);
            MAX_X = (int)(MAX_X - DEFAULT_CONSTRUCTION_WIDTH_SCALE * sprite.textureRect.width);
            MAX_Y = (int)(MAX_Y - DEFAULT_CONSTRUCTION_HEIGHT_SCALE * sprite.textureRect.height);
            int randX = (int)(MIN_X + Random.Range(0, 1) * (MAX_X - MIN_X));
            int randY = (int)(MIN_Y + Random.Range(0, 1) * (MAX_Y - MIN_Y));

            return stableAnyEntity(
                    sprite,
                    randX,
                    randY,
                    DEFAULT_CONSTRUCTION_WIDTH_SCALE,
                    DEFAULT_CONSTRUCTION_HEIGHT_SCALE,
                    constructionId);
        }

        protected GameEntity columnStableConstructionEntity(String constructionId, int index, int col)
        {
            int x = COLUMN_STABLE_FIRST_COL_X + col * COLUMN_STABLE_COL_PADDING_X + COLUMN_STABLE_INDEX_PADDING_X * index;
            int y = COLUMN_STABLE_FIRST_COL_Y + COLUMN_STABLE_INDEX_PADDING_Y * index;
            return stableAnyEntity(
                    parent.game.textureManager.getConstructionEntity(constructionId),
                    x,
                    y,
                    DEFAULT_CONSTRUCTION_WIDTH_SCALE,
                    DEFAULT_CONSTRUCTION_HEIGHT_SCALE,
                    constructionId);
        }

        protected GameEntity rowStableConstructionEntity(String constructionId, int index, int row)
        {
            int x = ROW_STABLE_FIRST_COL_X + ROW_STABLE_INDEX_PADDING_X * index;
            int y = ROW_STABLE_FIRST_COL_Y + row * ROW_STABLE_ROW_PADDING_Y + ROW_STABLE_INDEX_PADDING_Y * index;
            return stableAnyEntity(
                    parent.game.textureManager.getConstructionEntity(constructionId),
                    x,
                    y,
                    DEFAULT_CONSTRUCTION_WIDTH_SCALE,
                    DEFAULT_CONSTRUCTION_HEIGHT_SCALE,
                    constructionId);
        }



        protected GameEntity stableAnyEntity(Sprite sprite, int x, int y, double WIDTH_SCALE, double HEIGHT_SCALE, String constructionId)
        {
            GameEntity entity = drawContaioner.AsTableAdd<GameEntity>(stableEntityTempalte.gameObject);
            entity.texture = (sprite);
            entity.x = (x);
            entity.y = (y);
            entity.drawWidth = ((int)(WIDTH_SCALE * entity.texture.textureRect.width));
            entity.drawHeight = ((int)(HEIGHT_SCALE * entity.texture.textureRect.height));
            entity.moveable = (false);
            return entity;
        }
    }
}
