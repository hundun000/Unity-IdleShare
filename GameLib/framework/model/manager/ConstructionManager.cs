using hundun.idleshare.gamelib;
using hundun.unitygame.adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEditor.Progress;

namespace hundun.idleshare.gamelib
{
    public class ConstructionManager
    {
        IdleGameplayContext gameContext;


        public ConstructionManager(IdleGameplayContext gameContext)
        {
            this.gameContext = gameContext;
        }


        /**
         * 运行中的设施集合。key: constructionId
         */
        Dictionary<String, BaseConstruction> runningConstructionModelMap = new Dictionary<String, BaseConstruction>();

        /**
         * 根据GameArea显示不同的Construction集合
         */
        Dictionary<String, List<BaseConstruction>> areaControlableConstructions;

        public void lazyInit(Dictionary<String, List<String>> areaControlableConstructionIds)
        {
            areaControlableConstructions = new Dictionary<String, List<BaseConstruction>>();
            if (areaControlableConstructionIds != null)
            {
                foreach (KeyValuePair<String, List<String>> entry in areaControlableConstructionIds)
                {
                    areaControlableConstructions.Add(
                            entry.Key,
                            entry.Value
                                .Select(id => gameContext.constructionFactory.getConstruction(id))
                                .ToList()
                    );
                }
            }

            foreach (KeyValuePair<String, List<BaseConstruction>> entry in areaControlableConstructions)
            {
                var items = entry.Value;
                items.ForEach(item => runningConstructionModelMap.TryAdd(item.id, item));
            }

                
        }
        public void onSubLogicFrame()
        {
            foreach (KeyValuePair<String, BaseConstruction> entry in runningConstructionModelMap)
            {
                var item = entry.Value;
                item.onLogicFrame();
            }
        }

        public List<BaseConstruction> getAreaShownConstructionsOrEmpty(String gameArea)
        {
            areaControlableConstructions.computeIfAbsent(gameArea, gameArea2 => new List<BaseConstruction>());
            List<BaseConstruction> constructions = areaControlableConstructions.get(gameArea);
            return constructions;
        }

        public int? getConstructionLevelOrNull(String constructionId)
        {
            if (!runningConstructionModelMap.containsKey(constructionId))
            {
                return null;
            }
            return runningConstructionModelMap.get(constructionId).saveData.level;
        }
    }
}
