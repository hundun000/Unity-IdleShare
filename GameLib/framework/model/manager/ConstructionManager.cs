using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace hundun.idleshare.gamelib
{
    public class ConstructionManager : ITileNodeMap<BaseConstruction>
    {
        IdleGameplayContext gameContext;

        

        public ConstructionManager(IdleGameplayContext gameContext)
        {
            this.gameContext = gameContext;
        }


        /**
         * 运行中的设施集合。key: constructionId
         */
        public Dictionary<String, BaseConstruction> runningConstructionModelMap = new Dictionary<String, BaseConstruction>();

        /**
         * 根据GameArea显示不同的ConstructionVM集合
         */
        Dictionary<String, List<String>> areaControlableConstructionVMPrototypeIds;

        /**
         * 根据GameArea显示不同的ConstructionPrototypeVM集合
         */
        Dictionary<string, List<string>> areaControlableConstructionPrototypeVMPrototypeIds;

        public void lazyInit(Dictionary<String, List<String>> areaControlableConstructionVMPrototypeIds, Dictionary<string, List<string>> areaControlableConstructionPrototypeVMPrototypeIds)
        {
            this.areaControlableConstructionVMPrototypeIds = areaControlableConstructionVMPrototypeIds;
            this.areaControlableConstructionPrototypeVMPrototypeIds = areaControlableConstructionPrototypeVMPrototypeIds;
            //if (areaControlableConstructionPrototypeIds != null)
            //{
            //    foreach (KeyValuePair<String, List<String>> entry in areaControlableConstructionPrototypeIds)
            //    {
            //        areaControlableConstructions.Add(
            //                entry.Key,
            //                entry.Value
            //                    .SelectMany(id => gameContext.constructionFactory.getConstructionsOfPrototype(id))
            //                    .ToList()
            //        );
            //    }
            //}

            //foreach (KeyValuePair<String, List<BaseConstruction>> entry in areaControlableConstructions)
            //{
            //    var items = entry.Value;
            //    items.ForEach(item => runningConstructionModelMap.TryAdd(item.id, item));
            //}


        }
        public void onSubLogicFrame()
        {

            // 迭代期间不应修改runningConstructionModelMap
            List<BaseConstruction> promoteList = new List<BaseConstruction>();
            List<BaseConstruction> demoteList = new List<BaseConstruction>();

            foreach (KeyValuePair<String, BaseConstruction> entry in runningConstructionModelMap)
            {
                var construction = entry.Value;
                construction.onLogicFrame();

                if (construction.proficiencyComponent.canPromote())
                {
                    promoteList.Add(construction);
                } 
                else if (construction.proficiencyComponent.canDemote())
                {
                    demoteList.Add(construction);
                }
            }

            promoteList.ForEach(it => promoteInstanceAndNotify(it.id));
            demoteList.ForEach(it => demoteInstanceAndNotify(it.id));
        }

        public List<BaseConstruction> getAreaShownConstructionsOrEmpty(String gameArea)
        {
            return runningConstructionModelMap.Values
                .Where(it => areaControlableConstructionVMPrototypeIds.ContainsKey(gameArea) && 
                        areaControlableConstructionVMPrototypeIds.get(gameArea).Contains(it.prototypeId))
                .ToList();
        }

        public List<AbstractConstructionPrototype> getAreaShownConstructionPrototypesOrEmpty(String gameArea)
        {
            return areaControlableConstructionPrototypeVMPrototypeIds.get(gameArea)
                .Select(it => gameContext.constructionFactory.getPrototype(it))
                .ToList();
        }

        public BaseConstruction getConstruction(String id)
        {
            BaseConstruction result = runningConstructionModelMap[id];
            if (result == null)
            {
                throw new SystemException("getConstruction " + id + " not found");
            }
            return result;
        }

        public BaseConstruction getConstructionAt(GridPosition target)
        {
            return runningConstructionModelMap.Values
                .Where(it => it.saveData.position.Equals(target))
                .FirstOrDefault();
        }

        public List<BaseConstruction> getConstructions()
        {
            return runningConstructionModelMap.Values.ToList();
        }

        internal List<BaseConstruction> getConstructionsOfPrototype(string prototypeId)
        {
            return runningConstructionModelMap.Values
                .Where(it => it.prototypeId.Equals(prototypeId))
                .ToList();
        }

        internal void promoteInstanceAndNotify(String id)
        {
            BaseConstruction construction = runningConstructionModelMap[id];
            removeInstance(construction);
            createInstanceOfPrototype(construction.proficiencyComponent.promoteConstructionPrototypeId, construction.position);
            gameContext.eventManager.notifyConstructionCollectionChange();
        }

        internal void demoteInstanceAndNotify(String id)
        {
            BaseConstruction construction = runningConstructionModelMap[id];
            removeInstance(construction);
            createInstanceOfPrototype(construction.proficiencyComponent.demoteConstructionPrototypeId, construction.position);
            gameContext.eventManager.notifyConstructionCollectionChange();
        }

        internal void transformInstanceAndNotify(String id)
        {
            BaseConstruction construction = runningConstructionModelMap[id];
            removeInstance(construction);
            createInstanceOfPrototype(construction.upgradeComponent.transformConstructionPrototypeId, construction.position);
            gameContext.eventManager.notifyConstructionCollectionChange();
        }

        internal void destoryInstanceAndNotify(String id, String constructionPrototypeIdOfEmpty)
        {
            BaseConstruction construction = runningConstructionModelMap[id];
            removeInstance(construction);
            if (construction.destoryCostPack != null)
            {
                gameContext.storageManager.modifyAllResourceNum(construction.destoryCostPack.modifiedValues, false);
            }
            if (construction.destoryGainPack != null)
            {
                gameContext.storageManager.modifyAllResourceNum(construction.destoryGainPack.modifiedValues, true);
            }
            if (constructionPrototypeIdOfEmpty != null)
            {
                createInstanceOfPrototype(constructionPrototypeIdOfEmpty, construction.position);
            }
            gameContext.eventManager.notifyConstructionCollectionChange();
        }

        private void removeInstanceAt(GridPosition position)
        {
            var toRemove = runningConstructionModelMap
                         .Where(pair => pair.Value.position.Equals(position))
                         .ToList();

            foreach (var pair in toRemove)
            {
                runningConstructionModelMap.Remove(pair.Key);
                TileNodeUtils.updateNeighborsAllStep(pair.Value, this);
            }
        }


        private void removeInstance(BaseConstruction construction)
        {
            runningConstructionModelMap.Remove(construction.id);
            TileNodeUtils.updateNeighborsAllStep(construction, this);
        }

        internal void loadInstance(ConstructionSaveData saveData)
        {
            string prototypeId = saveData.prototypeId;
            GridPosition position = saveData.position;
            BaseConstruction construction = gameContext.constructionFactory.getInstanceOfPrototype(prototypeId, position);
            construction.saveData = saveData;
            construction.updateModifiedValues();

            runningConstructionModelMap.put(construction.id, construction);
            TileNodeUtils.updateNeighborsAllStep(construction, this);
        }

        internal bool canBuyInstanceOfPrototype(string prototypeId, GridPosition position)
        {
            AbstractConstructionPrototype prototype = gameContext.constructionFactory.getPrototype(prototypeId);
            bool isCostEnough = this.gameContext.storageManager.isEnough(prototype.buyInstanceCostPack.modifiedValues);
            bool positionAllow = runningConstructionModelMap
                         .Where(pair => pair.Value.position.Equals(position))
                         .Count() == 0
                         || runningConstructionModelMap
                         .Where(pair => pair.Value.position.Equals(position) && pair.Value.allowPositionOverwrite)
                         .Count() == 1
                         ;
            return isCostEnough && positionAllow;

        }

        

        internal void buyInstanceOfPrototypeAndNotify(string prototypeId, GridPosition position)
        {
            AbstractConstructionPrototype prototype = gameContext.constructionFactory.getPrototype(prototypeId);
            this.gameContext.storageManager.modifyAllResourceNum(prototype.buyInstanceCostPack.modifiedValues, false);
            createInstanceOfPrototype(prototypeId, position);
            this.gameContext.eventManager.notifyConstructionCollectionChange();
        }

        private void createInstanceOfPrototype(string prototypeId, GridPosition position)
        {
            removeInstanceAt(position);
            
            BaseConstruction construction = gameContext.constructionFactory.getInstanceOfPrototype(prototypeId, position);
            
            runningConstructionModelMap.put(construction.id, construction);
            TileNodeUtils.updateNeighborsAllStep(construction, this);

            
        }

        

        BaseConstruction ITileNodeMap<BaseConstruction>.getValidNodeOrNull(GridPosition position)
        {
            return getConstructionAt(position);
        }
    }
}
