using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace hundun.idleshare.gamelib
{
    public class GameplaySaveData
    {
        public Dictionary<String, long> ownResoueces;
        public HashSet<String> unlockedResourceTypes;
        public Dictionary<String, ConstructionSaveData> constructionSaveDataMap;
        public HashSet<String> unlockedAchievementIds;

    }
}
