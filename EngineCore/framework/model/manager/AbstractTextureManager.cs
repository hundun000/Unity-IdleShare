using hundun.unitygame.adapters;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hundun.idleshare.enginecore
{
    public abstract class AbstractTextureManager
    {


        public Sprite winTexture;


        public Sprite menuTexture;


        public Sprite defaultBoardNinePatchTexture;


        protected Dictionary<String, Sprite> resourceIconMap = new Dictionary<String, Sprite>();
        protected Dictionary<String, Sprite> resourceEntityMap = new Dictionary<String, Sprite>();
        protected Dictionary<String, Sprite> constructionEntityMap = new Dictionary<String, Sprite>();
        protected Dictionary<String, Sprite> gameAreaLeftPartRegionMap = new Dictionary<String, Sprite>();
        protected Dictionary<String, Sprite> gameAreaRightPartRegionMap = new Dictionary<String, Sprite>();
        protected Dictionary<String, Sprite> gameAreaBackMap = new Dictionary<String, Sprite>();

        protected Sprite defaultIcon;
        protected Sprite defaultAreaBack;



        public Sprite getBackgroundTexture(String gameArea)
        {
            return gameAreaBackMap.getOrDefault(gameArea, defaultAreaBack);
        }

        public Sprite getResourceIcon(String resourceType)
        {
            return resourceIconMap.getOrDefault(resourceType, defaultIcon);
        }

        public Sprite getResourceEntity(String resourceType)
        {
            return resourceEntityMap.getOrDefault(resourceType, defaultIcon);
        }

        public Sprite getConstructionEntity(String constructionId)
        {
            return constructionEntityMap.getOrDefault(constructionId, defaultIcon);
        }

        public Sprite getGameAreaTexture(String key, Boolean longVersion)
        {
            if (longVersion)
            {
                return gameAreaLeftPartRegionMap.getOrDefault(key, defaultIcon);
            }
            else
            {
                return gameAreaRightPartRegionMap.getOrDefault(key, defaultIcon);
            }

        }

        public abstract void lazyInitOnGameCreateStage2();
    }
}
