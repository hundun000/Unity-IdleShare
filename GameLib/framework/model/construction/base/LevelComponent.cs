using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class LevelComponent
    {
        private readonly BaseConstruction construction;

        public readonly Boolean workingLevelChangable;
        public static readonly int DEFAULT_MIN_WORKING_LEVEL = 0;
        public int minWorkingLevel = DEFAULT_MIN_WORKING_LEVEL;
        public static readonly int DEFAULT_MAX_LEVEL = 5;
        public int maxLevel = DEFAULT_MAX_LEVEL;
        public LevelComponent(BaseConstruction construction, Boolean workingLevelChangable)
        {
            this.construction = construction;
            this.workingLevelChangable = workingLevelChangable;
        }

        public String getWorkingLevelDescroption()
        {
            Boolean reachMaxLevel = construction.saveData.level == this.maxLevel;
            return construction.descriptionPackage.levelDescroptionProvider.Invoke(construction.saveData.level, construction.saveData.workingLevel, reachMaxLevel);
        }

        public Boolean canChangeWorkingLevel(int delta)
        {
            if (!workingLevelChangable)
            {
                return false;
            }
            int next = construction.saveData.workingLevel + delta;
            if (next > construction.saveData.level || next < this.minWorkingLevel)
            {
                return false;
            }
            return true;
        }

        public void changeWorkingLevel(int delta)
        {
            if (canChangeWorkingLevel(delta))
            {
                construction.saveData.workingLevel = (construction.saveData.workingLevel + delta);
                construction.updateModifiedValues();
                construction.gameplayContext.frontend.log(construction.name, "changeWorkingLevel delta = " + delta + ", success to " + construction.saveData.workingLevel);
            }
            else
            {
                construction.gameplayContext.frontend.log(construction.name, "changeWorkingLevel delta = " + delta + ", but cannot!");
            }
        }

        internal bool isReachMaxLevel()
        {
            return construction.saveData.level >= this.maxLevel;
        }
    }
}

