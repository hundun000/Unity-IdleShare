using hundun.idleshare.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace hundun.idleshare.gamelib
{

    public class DescriptionPackageBuilder
    {
        private String buttonDescroption;

        private String outputCostDescriptionStart;
        private String outputGainDescriptionStart;

        private String upgradeCostDescriptionStart;
        private String upgradeMaxLevelNoTransferDescription;

        private String transformButtonDescroption;
        private String transformCostDescriptionStart;
        private String upgradeMaxLevelHasTransferDescription;

        private String destroyButtonDescroption;
        private String destroyGainDescriptionStart;
        private String destroyCostDescriptionStart;

        private ILevelDescroptionProvider levelDescroptionProvider;

        private IProficiencyDescroptionProvider proficiencyDescroptionProvider;

        public DescriptionPackageBuilder button(String buttonDescroption)
        {
            this.buttonDescroption = buttonDescroption;
            return this;
        }

        public DescriptionPackageBuilder output(String outputCostDescriptionStart, String outputGainDescriptionStart)
        {
            this.outputCostDescriptionStart = outputCostDescriptionStart;
            this.outputGainDescriptionStart = outputGainDescriptionStart;
            return this;
        }

        public DescriptionPackageBuilder upgrade(String upgradeCostDescriptionStart, String upgradeMaxLevelNoTransferDescription, ILevelDescroptionProvider levelDescroptionProvider)
        {
            this.upgradeCostDescriptionStart = upgradeCostDescriptionStart;
            this.upgradeMaxLevelNoTransferDescription = upgradeMaxLevelNoTransferDescription;
            this.levelDescroptionProvider = levelDescroptionProvider;
            return this;
        }

        public DescriptionPackageBuilder transform(String transformButtonDescroption,
                String transformCostDescriptionStart,
                String upgradeMaxLevelHasTransferDescription)
        {
            this.transformButtonDescroption = transformButtonDescroption;
            this.transformCostDescriptionStart = transformCostDescriptionStart;
            this.upgradeMaxLevelHasTransferDescription = upgradeMaxLevelHasTransferDescription;
            return this;
        }

        public DescriptionPackageBuilder destroy(String destroyButtonDescroption,
                String destroyGainDescriptionStart,
                String destroyCostDescriptionStart)
        {
            this.destroyButtonDescroption = destroyButtonDescroption;
            this.destroyGainDescriptionStart = destroyGainDescriptionStart;
            this.destroyCostDescriptionStart = destroyCostDescriptionStart;
            return this;
        }

        public DescriptionPackageBuilder proficiency(
            IProficiencyDescroptionProvider proficiencyDescroptionProvider)
        {

            this.proficiencyDescroptionProvider = proficiencyDescroptionProvider;
            return this;
        }

        public DescriptionPackage build()
        {
            return new DescriptionPackage(
                    buttonDescroption,

                    outputCostDescriptionStart,
                    outputGainDescriptionStart,

                    upgradeCostDescriptionStart,
                    upgradeMaxLevelNoTransferDescription,

                    transformButtonDescroption,
                    transformCostDescriptionStart,
                    upgradeMaxLevelHasTransferDescription,

                    destroyButtonDescroption,
                    destroyGainDescriptionStart,
                    destroyCostDescriptionStart,

                    levelDescroptionProvider,

                    proficiencyDescroptionProvider
                );
        }

    }
    public class DescriptionPackage
    {
        public String buttonDescroption;

        public String outputCostDescriptionStart;
        public String outputGainDescriptionStart;

        public String upgradeCostDescriptionStart;
        public String upgradeMaxLevelNoTransferDescription;

        public String transformButtonDescroption;
        public String transformCostDescriptionStart;
        public String upgradeMaxLevelHasTransferDescription;

        public String destroyButtonDescroption;
        public String destroyGainDescriptionStart;
        public String destroyCostDescriptionStart;

        public ILevelDescroptionProvider levelDescroptionProvider;

        public IProficiencyDescroptionProvider proficiencyDescroptionProvider;

        public DescriptionPackage(
            string buttonDescroption, 
            string outputCostDescriptionStart, 
            string outputGainDescriptionStart, 
            string upgradeCostDescriptionStart, 
            string upgradeMaxLevelNoTransferDescription, 
            string transformButtonDescroption, 
            string transformCostDescriptionStart, 
            string upgradeMaxLevelHasTransferDescription, 
            string destroyButtonDescroption, 
            string destroyGainDescriptionStart, 
            string destroyCostDescriptionStart, 
            ILevelDescroptionProvider levelDescroptionProvider,
            IProficiencyDescroptionProvider proficiencyDescroptionProvider)
        {
            this.buttonDescroption = buttonDescroption;
            this.outputCostDescriptionStart = outputCostDescriptionStart;
            this.outputGainDescriptionStart = outputGainDescriptionStart;
            this.upgradeCostDescriptionStart = upgradeCostDescriptionStart;
            this.upgradeMaxLevelNoTransferDescription = upgradeMaxLevelNoTransferDescription;
            this.transformButtonDescroption = transformButtonDescroption;
            this.transformCostDescriptionStart = transformCostDescriptionStart;
            this.upgradeMaxLevelHasTransferDescription = upgradeMaxLevelHasTransferDescription;
            this.destroyButtonDescroption = destroyButtonDescroption;
            this.destroyGainDescriptionStart = destroyGainDescriptionStart;
            this.destroyCostDescriptionStart = destroyCostDescriptionStart;
            this.levelDescroptionProvider = levelDescroptionProvider;
            this.proficiencyDescroptionProvider = proficiencyDescroptionProvider;
        }
    }

    public delegate String ILevelDescroptionProvider(int level, int workingLevel, Boolean reachMaxLevel);
    public delegate String IProficiencyDescroptionProvider(int proficiency, Boolean reachMaxProficiency);





}
