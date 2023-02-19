using hundun.idleshare.enginecore;
using hundun.unitygame.adapters;
using hundun.unitygame.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace hundun.idleshare.enginecore
{
    public class GameAreaControlBoardVM<T_GAME, T_SAVE> : MonoBehaviour, IGameAreaChangeListener where T_GAME : BaseIdleGame<T_GAME, T_SAVE>
    {
        BaseIdlePlayScreen<T_GAME, T_SAVE> parent;
        Dictionary<String, GameAreaControlNodeVM<T_GAME, T_SAVE>> nodes = new Dictionary<String, GameAreaControlNodeVM<T_GAME, T_SAVE>>();

        GameObject nodesRoot;
        GameAreaControlNodeVM<T_GAME, T_SAVE> nodeTemplate;

        void Awake()
        {
            this.nodesRoot = this.transform.Find("nodesRoot").gameObject;
            this.nodeTemplate = this.transform.Find("_templates/nodeTemplate").GetComponent<GameAreaControlNodeVM<T_GAME, T_SAVE>>();
        }


        public void postPrefabInitialization(BaseIdlePlayScreen<T_GAME, T_SAVE> parent, List<String> gameAreas)
        {
            this.parent = parent;

            foreach (String gameArea in gameAreas)
            {
                initButtonMap(gameArea, false);
            }

            rebuildChild(null);
        }

        private void initButtonMap(String gameArea, Boolean longVersion)
        {
            GameAreaControlNodeVM<T_GAME, T_SAVE> node = nodesRoot.transform.AsTableAdd<GameAreaControlNodeVM<T_GAME, T_SAVE>>(nodeTemplate.gameObject);
            node.postPrefabInitialization(parent, gameArea, longVersion);

            nodes.put(gameArea, node);
        }




        public void onGameAreaChange(String last, String current)
        {
            rebuildChild(current);
        }

        private void rebuildChild(String current)
        {

            nodes.ToList().ForEach(entry => {
                if (entry.Key == current)
                {
                    entry.Value.changeVersion(true);
                }
                else
                {
                    entry.Value.changeVersion(false);
                }

            });

        }

    }
}
