using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class SystemSettingSaveData
    {
        public Language language;

        public SystemSettingSaveData()
        {
        }

        public SystemSettingSaveData(Language language)
        {
            this.language = language;
        }

        public override string ToString()
        {
            return "SystemSettingSaveData(" + "language=" + language + ")";
        }
    }
}
