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
        public Dictionary<String, List<String>> areaControlableConstructionVMPrototypeIds;
        public Dictionary<String, List<String>> areaControlableConstructionPrototypeVMPrototypeIds;
        public Dictionary<String, List<String>> areaShowEntityByOwnAmountConstructionPrototypeIds;
        public Dictionary<String, List<String>> areaShowEntityByOwnAmountResourceIds;
        public Dictionary<String, List<String>> areaShowEntityByChangeAmountResourceIds;
        public Dictionary<String, String> screenIdToFilePathMap;
        public List<String> achievementPrototypeIds;
    }
}
