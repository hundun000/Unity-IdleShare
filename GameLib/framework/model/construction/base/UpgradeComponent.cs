using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hundun.idleshare.gamelib
{
    public enum UpgradeState
    {
        NO_UPGRADE,
        HAS_NEXT_UPGRADE,
        REACHED_MAX_UPGRADE

    }

    public class UpgradeComponent
    {
        private readonly BaseConstruction construction;

        public ResourcePack upgradeCostPack { get; set; }
        public UpgradeState upgradeState { get; private set; }


        /**
         * 影响升级后下一级费用，详见具体公式
         */
        private static readonly double upgradeCostLevelUpArg = 1.05;
        private static readonly Func<long, int, long> DEFAULT_CALCULATE_COST_FUNCTION = (baseValue, level) => {
            return (long)(
                    baseValue
                    * (1 + 1 * level)
                    * Math.Pow(upgradeCostLevelUpArg, level)
                    );
        };
        public Func<long, int, long> calculateCostFunction = DEFAULT_CALCULATE_COST_FUNCTION;


        public UpgradeComponent(BaseConstruction construction)
        {
            this.construction = construction;
            // default value
            upgradeState = UpgradeState.NO_UPGRADE;
        }

        public void lazyInitDescription()
        {
            if (upgradeCostPack != null)
            {
                upgradeState = UpgradeState.HAS_NEXT_UPGRADE;
                upgradeCostPack.descriptionStart = (construction.descriptionPackage.upgradeCostDescriptionStart);
            }
        }

        public void updateModifiedValues(Boolean reachMaxLevel)
        {
            if (upgradeCostPack != null)
            {
                if (reachMaxLevel)
                {
                    upgradeState = UpgradeState.REACHED_MAX_UPGRADE;
                    this.upgradeCostPack.modifiedValues = (null);
                    this.upgradeCostPack.modifiedValuesDescription = (null);
                }
                else
                {
                    this.upgradeCostPack.modifiedValues = (
                            upgradeCostPack.baseValues
                                    .Select(pair => {
                                        long newAmout = calculateCostFunction.Invoke(pair.amount, construction.saveData.level);
                                        return new ResourcePair(pair.type, newAmout);
                                    })
                                    .ToList()
                    );
                    this.upgradeCostPack.modifiedValuesDescription = (String.Join(", ",
                            upgradeCostPack.baseValues
                                    .Select(pair => pair.type + "x" + pair.amount)
                                    .ToList())
                                    + "; "
                    );
                }
            }
        }

        public Boolean canUpgrade()
        {
            if (construction.saveData.level >= construction.maxLevel || upgradeCostPack == null)
            {
                return false;
            }

            List<ResourcePair> compareTarget = upgradeCostPack.modifiedValues;
            return construction.gameContext.storageManager.isEnough(compareTarget);
        }
    }
}
