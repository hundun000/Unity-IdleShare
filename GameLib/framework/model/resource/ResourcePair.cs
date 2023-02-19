using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class ResourcePair
    {
        public String type;
        public long amount;

        public ResourcePair(string type, long amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}
