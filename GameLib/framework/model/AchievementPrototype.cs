using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class AchievementPrototype
    {
        public String name;
        public String description;
        public Dictionary<String, int> requiredBuffs;
        public Dictionary<String, int> requiredResources;

        public AchievementPrototype(string name, string description, Dictionary<string, int> requiredBuffs, Dictionary<string, int> requiredResources)
        {
            this.name = name;
            this.description = description;
            this.requiredBuffs = requiredBuffs;
            this.requiredResources = requiredResources;
        }
    }
}
