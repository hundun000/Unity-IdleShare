using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{ 
    public interface IOneFrameResourceChangeListener
    {
        void onResourceChange(Dictionary<String, long> changeMap, Dictionary<string, List<long>> deltaHistoryMap);
    }
}
