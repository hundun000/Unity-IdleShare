using hundun.idleshare.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public abstract class ChildGameConfig
    {
        public Dictionary<String, List<String>> areaControlableConstructionIds;
        public Dictionary<String, List<String>> areaShowEntityByOwnAmountConstructionIds;
        public Dictionary<String, List<String>> areaShowEntityByOwnAmountResourceIds;
        public Dictionary<String, List<String>> areaShowEntityByChangeAmountResourceIds;
        public Dictionary<String, String> screenIdToFilePathMap;
        public List<AchievementPrototype> achievementPrototypes;
    }
}
