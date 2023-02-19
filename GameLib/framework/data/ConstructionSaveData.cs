using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class ConstructionSaveData
    {
        public int level;
        public int workingLevel;

        public ConstructionSaveData()
        {
        }

        public ConstructionSaveData(int level, int workingLevel)
        {
            this.level = level;
            this.workingLevel = workingLevel;
        }

        public static Builder builder()
        {
            return new Builder();
        }

        public class Builder
        {
            public int _level;
            public int _workingLevel;

            public ConstructionSaveData build()
            {
                return new ConstructionSaveData(_level, _workingLevel);
            }

            public Builder level(int _level)
            {
                this._level = _level;
                return this;
            }

            public Builder workingLevel(int _workingLevel)
            {
                this._workingLevel = _workingLevel;
                return this;
            }
        }
    }
}
