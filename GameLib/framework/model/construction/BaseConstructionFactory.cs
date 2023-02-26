using hundun.idleshare.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEditor.Progress;

namespace hundun.idleshare.gamelib
{
    public class BaseConstructionFactory
    {
        Dictionary<String, BaseConstruction> constructions = new Dictionary<String, BaseConstruction>();

        public BaseConstruction getConstruction(String id)
        {
            BaseConstruction result = constructions[id];
            if (result == null)
            {
                throw new SystemException("getConstruction " + id + " not found");
            }
            return result;
        }

        public List<BaseConstruction> getConstructions()
        {
            return constructions.Values.ToList();
        }

        public void lazyInit(IdleGameplayContext gameContext, List<BaseConstruction> constructionsList)
        {
            constructionsList.ForEach(item => this.constructions.Add(item.id, item));
            foreach (KeyValuePair<String, BaseConstruction> entry in constructions)
            {
                var it = entry.Value;
                it.lazyInitDescription(gameContext);
                gameContext.eventManager.registerListener(it);
            }
        }
    }
}
