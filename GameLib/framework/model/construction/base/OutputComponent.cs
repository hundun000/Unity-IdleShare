using hundun.idleshare.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public class OutputComponent
    {
        private readonly BaseConstruction construction;

        /**
         * 对于Click型，即为基础点击收益；对于Auto型，即为基础自动收益；
         */
        public ResourcePack outputGainPack;


        /**
            * output行为所需要支付的费用; 无费用时为null
            */
        public ResourcePack outputCostPack;


        private static readonly int DEFAULT_AUTO_OUPUT_SECOND_MAX = 1;
        public int autoOutputSecondCountMax = DEFAULT_AUTO_OUPUT_SECOND_MAX;

        public OutputComponent(BaseConstruction construction)
        {
            this.construction = construction;
        }

        public void lazyInitDescription()
        {
            if (outputCostPack != null)
            {
                outputCostPack.descriptionStart = (construction.descriptionPackage.outputCostDescriptionStart);
            }
            if (outputGainPack != null)
            {
                outputGainPack.descriptionStart = (construction.descriptionPackage.outputGainDescriptionStart);
            }
        }

        public void updateModifiedValues()
        {
            // --------------
            if (outputGainPack != null)
            {
                outputGainPack.modifiedValues = (
                        outputGainPack.baseValues
                                .Select(pair => {
                                    long newAmout = construction.calculateModifiedOutput(pair.amount, 
                                        construction.saveData.workingLevel,
                                        construction.saveData.proficiency
                                        );
                                    return new ResourcePair(pair.type, newAmout);
                                })
                                .ToList()
                );
                this.outputGainPack.modifiedValuesDescription = (String.Join(", ",
                        outputGainPack.modifiedValues
                                .Select(pair => pair.type + "x" + pair.amount)
                                .ToList())
                                + "; "
                );
            }
            // --------------
            if (outputCostPack != null)
            {
                    outputCostPack.modifiedValues = (
                        outputCostPack.baseValues
                                .Select(pair => {
                                    long newAmout = construction.calculateModifiedOutput(pair.amount, 
                                        construction.saveData.workingLevel,
                                        construction.saveData.proficiency);
                                    return new ResourcePair(pair.type, newAmout);
                                })
                                .ToList()
                );
                this.outputCostPack.modifiedValuesDescription = (String.Join(", ",
                        outputCostPack.modifiedValues
                                .Select(pair => pair.type + "x" + pair.amount)
                                .ToList())
                                + "; "
                );
            }
        }

        public Boolean hasCost()
        {
            return outputCostPack != null;
        }

        public Boolean canOutput()
        {
            if (!hasCost())
            {
                return true;
            }

            List<ResourcePair> compareTarget = outputCostPack.modifiedValues;
            return construction.gameContext.storageManager.isEnough(compareTarget);
        }
    }
}
