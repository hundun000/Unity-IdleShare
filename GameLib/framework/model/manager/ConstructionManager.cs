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

        private List<BaseConstruction> removeQueue = new List<BaseConstruction>();
        private List<BaseConstruction> createQueue = new List<BaseConstruction>();

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

            removeQueue.ForEach(it => {
                runningConstructionModelMap.Remove(it.id);
                TileNodeUtils.updateNeighborsAllStep(it, this);
            });

            createQueue.ForEach(it => {
                removeInstanceAt(it.position);
                runningConstructionModelMap.put(it.id, it);
                TileNodeUtils.updateNeighborsAllStep(it, this);
            });

            if (removeQueue.Count > 0 || createQueue.Count > 0)
            {
                this.gameContext.eventManager.notifyConstructionCollectionChange();
            }
            removeQueue.Clear();
            createQueue.Clear();

            foreach (KeyValuePair<String, BaseConstruction> entry in runningConstructionModelMap)
            {
                var construction = entry.Value;
                construction.onSubLogicFrame();
            }

        }

        public List<BaseConstruction> getAreaControlableConstructionsOrEmpty(String gameArea)
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

        


        

        private void removeInstanceAt(GridPosition position)
        {
            runningConstructionModelMap
                         .Where(pair => pair.Value.position.Equals(position))
                         .ToList()
                         .ForEach(pair => removeQueue.Add(pair.Value));
 
        }


        public void addToRemoveQueue(BaseConstruction construction)
        {
            removeQueue.Add(construction);
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

        

        internal void buyInstanceOfPrototype(string prototypeId, GridPosition position)
        {
            AbstractConstructionPrototype prototype = gameContext.constructionFactory.getPrototype(prototypeId);
            this.gameContext.storageManager.modifyAllResourceNum(prototype.buyInstanceCostPack.modifiedValues, false);
            addToCreateQueue(prototypeId, position);
        }

        public void addToCreateQueue(string prototypeId, GridPosition position)
        {
            BaseConstruction construction = gameContext.constructionFactory.getInstanceOfPrototype(prototypeId, position);

            createQueue.Add(construction);
        }

        

        BaseConstruction ITileNodeMap<BaseConstruction>.getValidNodeOrNull(GridPosition position)
        {
            return getConstructionAt(position);
        }

        internal void AddToRemoveQueue(BaseConstruction construction)
        {
            throw new NotImplementedException();
        }
    }
}
