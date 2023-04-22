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
        REACHED_MAX_UPGRADE_NO_TRANSFER,
        REACHED_MAX_UPGRADE_HAS_TRANSFER

    }

    public class UpgradeComponent
    {
        private readonly BaseConstruction construction;

        public ResourcePack upgradeCostPack { get; set; }

        public ResourcePack transformCostPack { get; set; }
        public string transformConstructionPrototypeId;
        public UpgradeState upgradeState { get; private set; }


        /**
         * 影响升级后下一级费用，详见具体公式
         */
        private static readonly double upgradeCostLevelUpArg = 1.00;
        private static readonly Func<long, int, long> DEFAULT_CALCULATE_COST_FUNCTION = (baseValue, level) =>
        {
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
                upgradeCostPack.descriptionStart = construction.descriptionPackage.upgradeCostDescriptionStart;
            }
            if (transformCostPack != null)
            {
                transformCostPack.descriptionStart = construction.descriptionPackage.transformCostDescriptionStart;
            }
        }
        public void updateModifiedValues()
        {
            Boolean reachMaxLevel = construction.levelComponent.isReachMaxLevel();
            if (upgradeCostPack != null)
            {
                if (reachMaxLevel)
                {
                    
                    this.upgradeCostPack.modifiedValues = (null);
                    this.upgradeCostPack.modifiedValuesDescription = (null);
                    if (transformCostPack != null)
                    {
                        upgradeState = UpgradeState.REACHED_MAX_UPGRADE_HAS_TRANSFER;

                        this.transformCostPack.modifiedValues = transformCostPack.baseValues;
                        this.transformCostPack.modifiedValuesDescription = (String.Join(", ",
                                transformCostPack.modifiedValues
                                        .Select(pair => pair.type + "x" + pair.amount)
                                        .ToList())
                                        + "; "
                        );
                    } 
                    else
                    {
                        upgradeState = UpgradeState.REACHED_MAX_UPGRADE_NO_TRANSFER;
                    }
                }
                else
                {
                    this.upgradeCostPack.modifiedValues = (
                            upgradeCostPack.baseValues
                                    .Select(pair =>
                                    {
                                        long newAmout = calculateCostFunction.Invoke(pair.amount, construction.saveData.level);
                                        return new ResourcePair(pair.type, newAmout);
                                    })
                                    .ToList()
                    );
                    this.upgradeCostPack.modifiedValuesDescription = (String.Join(", ",
                            upgradeCostPack.modifiedValues
                                    .Select(pair => pair.type + "x" + pair.amount)
                                    .ToList())
                                    + "; "
                    );
                }
            }
            
        }

        public Boolean canUpgrade()
        {
            if (construction.levelComponent.isReachMaxLevel() || upgradeCostPack == null)
            {
                return false;
            }
            if (!construction.proficiencyComponent.isMaxProficiency())
            {
                return false;
            }

            List<ResourcePair> compareTarget = upgradeCostPack.modifiedValues;
            return construction.gameplayContext.storageManager.isEnough(compareTarget);
        }

        public void doUpgrade()
        {
            List<ResourcePair> upgradeCostRule = this.upgradeCostPack.modifiedValues;
            construction.gameplayContext.storageManager.modifyAllResourceNum(upgradeCostRule, false);
            construction.saveData.level = (construction.saveData.level + 1);
            if (!construction.levelComponent.workingLevelChangable)
            {
                construction.saveData.workingLevel = (construction.saveData.level);
            }
            construction.proficiencyComponent.afterUpgrade();
            construction.updateModifiedValues();
            construction.gameplayContext.eventManager.notifyConstructionCollectionChange();
        }

        public Boolean canTransfer()
        {
            if (!construction.levelComponent.isReachMaxLevel() || transformCostPack == null)
            {
                return false;
            }
            if (!construction.proficiencyComponent.isMaxProficiency())
            {
                return false;
            }

            List<ResourcePair> compareTarget = transformCostPack.modifiedValues;
            return construction.gameplayContext.storageManager.isEnough(compareTarget);
        }

        public void doTransfer()
        {
            if (construction.upgradeComponent.transformCostPack != null)
            {
                construction.gameplayContext.storageManager.modifyAllResourceNum(construction.upgradeComponent.transformCostPack.modifiedValues, false);
            }
            construction.gameplayContext.constructionManager.addToRemoveQueue(construction);
            construction.gameplayContext.constructionManager.addToCreateQueue(construction.upgradeComponent.transformConstructionPrototypeId, construction.position);
        }
    }
}
