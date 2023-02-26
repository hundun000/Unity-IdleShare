using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public interface IBuiltinConstructionsLoader
    {
        List<BaseConstruction> provide(Language language);
    }
}
