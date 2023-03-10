using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Unity.VisualScripting.Icons;

namespace hundun.idleshare.gamelib
{
    public interface IGameDictionary
    {
        String constructionIdToShowName(Language language, String constructionId);
        String constructionIdToDetailDescroptionConstPart(Language language, String constructionId);
        List<String> getMemuScreenTexts(Language language);

        Dictionary<Language, String> getLanguageShowNameMap();
    }
}
