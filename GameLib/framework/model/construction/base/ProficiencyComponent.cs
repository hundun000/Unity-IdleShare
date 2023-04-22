using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public abstract class ProficiencyComponent
    {

        public int maxProficiency = 100;
        protected readonly BaseConstruction construction;
        public String promoteConstructionPrototypeId;
        public String demoteConstructionPrototypeId;

        public ProficiencyComponent(BaseConstruction construction)
        {
            this.construction = construction;
        }

        public String getProficiencyDescroption()
        {
            Boolean reachMaxLevel = construction.saveData.proficiency >= this.maxProficiency;
            return construction.descriptionPackage.proficiencyDescroptionProvider.Invoke(construction.saveData.proficiency, reachMaxLevel);
        }

        public Boolean canPromote()
        {
            return isMaxProficiency() && promoteConstructionPrototypeId != null;
        }

        public Boolean canDemote()
        {
            return (construction.saveData.proficiency < 0) && demoteConstructionPrototypeId != null;
        }

        public void changeProficiency(int delta)
        {
            construction.saveData.proficiency = Math.Max(0, Math.Min(construction.saveData.proficiency + delta, this.maxProficiency));
            construction.updateModifiedValues();
            //construction.gameContext.frontend.log(construction.name, "changeProficiency delta = " + delta + ", success to " + construction.saveData.proficiency);
        }

        public void cleanProficiency()
        {

            construction.saveData.proficiency = 0;
            construction.updateModifiedValues();
            construction.gameplayContext.frontend.log(construction.name, "cleanProficiency");

        }

        public abstract void onSubLogicFrame();

        public abstract void afterUpgrade();

        public bool isMaxProficiency()
        {
            return construction.saveData.proficiency >= maxProficiency;
        }
    }
}

