using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{

    public abstract class AbstractAchievement
    {
        protected IdleGameplayContext gameplayContext;

        public String id;
        public String name;
        public String description;
        public String congratulationText;

        public AbstractAchievement(String id, string name, string description, string congratulationText)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.congratulationText = congratulationText;
        }

        public abstract bool checkUnloack();

        public void lazyInitDescription(IdleGameplayContext gameplayContext)
        {
            this.gameplayContext = gameplayContext;
        }
    }

    public interface IBuiltinAchievementsLoader
    {
        public Dictionary<String, AbstractAchievement> getProviderMap(Language language);
    }
}
