using hundun.idleshare.gamelib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace hundun.idleshare.gamelib
{
    public class DescriptionPackageFactory
    {

        public static ILevelDescroptionProvider ANY_EMPTY_LEVEL_IMP = (level, workingLevel, reachMaxLevel) => "";
        public static IProficiencyDescroptionProvider ANY_EMPTY_PROFICIENCY_IMP = (proficiency, reachMaxProficiency) => "";


        public static ILevelDescroptionProvider ONLY_LEVEL_IMP = (level, workingLevel, reachMaxLevel) => {
            return "lv." + level + (reachMaxLevel ? "(max)" : "");
        };
        public static ILevelDescroptionProvider WORKING_LEVEL_IMP = (level, workingLevel, reachMaxLevel) => {
            return "lv." + workingLevel + "/" + level + (reachMaxLevel ? "(max)" : "");
        };
        public static ILevelDescroptionProvider LOCK_IMP = (level, workingLevel, reachMaxLevel) => {
            return (reachMaxLevel ? "Unlocked" : "");
        };
        public static IProficiencyDescroptionProvider EN_PROFICIENCY_IMP = (proficiency, reachMaxProficiency) => {
            return "efficiency: " + proficiency;
        };


        public static ILevelDescroptionProvider CN_ONLY_LEVEL_IMP = (level, workingLevel, reachMaxLevel) => {
            return "等级" + level + (reachMaxLevel ? "(最大)" : "");
        };
        public static ILevelDescroptionProvider CN_WORKING_LEVEL_IMP = (level, workingLevel, reachMaxLevel) => {
            return "等级" + workingLevel + "/" + level + (reachMaxLevel ? "(最大)" : "");
        };
        public static ILevelDescroptionProvider CN_LOCK_IMP = (level, workingLevel, reachMaxLevel) => {
            return (reachMaxLevel ? "已解锁" : "");
        };
        public static IProficiencyDescroptionProvider CN_PROFICIENCY_IMP = (proficiency, reachMaxProficiency) => {
            return "效率" + proficiency;
        };




        //public static DescriptionPackage getWorkingLevelAutoDescriptionPackage(Language language)
        //{
        //    switch (language)
        //    {
        //        case Language.CN:
        //            return new DescriptionPackage(
        //                    "自动消耗", "自动产出", "升级费用", "(已达到最大等级)", "升级",
        //                    CN_WORKING_LEVEL_IMP,
        //                    CN_PROFICIENCY_IMP);
        //        default:
        //            return new DescriptionPackage(
        //                    "AutoCost", "AutoGain", "UpgradeCost", "(max level)", "Upgrade",
        //                    WORKING_LEVEL_IMP,
        //                    CN_PROFICIENCY_IMP);
        //    }
        //}

        //public static DescriptionPackage getMaxLevelAutoDescriptionPackage(Language language)
        //{
        //    switch (language)
        //    {
        //        case Language.CN:
        //            return new DescriptionPackage(
        //                    "自动消耗", "自动产出", "升级费用", "(已达到最大等级)", "升级",
        //                    CN_ONLY_LEVEL_IMP,
        //                    CN_PROFICIENCY_IMP);
        //        default:
        //            return new DescriptionPackage(
        //                    "AutoCost", "AutoGain", "UpgradeCost", "(max level)", "Upgrade",
        //                    ONLY_LEVEL_IMP,
        //                    CN_PROFICIENCY_IMP);
        //    }
        //}

        //public static DescriptionPackage getSellingDescriptionPackage(Language language)
        //{
        //    switch (language)
        //    {
        //        case Language.CN:
        //            return new DescriptionPackage(
        //                    "自动出售", "自动获得", "升级费用", "(已达到最大等级)", "升级",
        //                    CN_WORKING_LEVEL_IMP,
        //                    CN_PROFICIENCY_IMP);
        //        default:
        //            return new DescriptionPackage(
        //                    "Sell", "Gain", "UpgradeCost", "(max level)", "Upgrade",
        //                    WORKING_LEVEL_IMP,
        //                    CN_PROFICIENCY_IMP);
        //    }
        //}


        //public static DescriptionPackage getGatherDescriptionPackage(Language language)
        //{
        //    switch (language)
        //    {
        //        case Language.CN:
        //            return new DescriptionPackage(
        //                    "消耗", "获得", null, null, "采集",
        //                    ANY_EMPTY_LEVEL_IMP,
        //                    CN_PROFICIENCY_IMP);
        //        default:
        //            return new DescriptionPackage(
        //                    "Pay", "Gain", null, null, "Gather",
        //                    ANY_EMPTY_LEVEL_IMP,
        //                    CN_PROFICIENCY_IMP);
        //    }
        //}


        //public static DescriptionPackage getWinDescriptionPackage(Language language)
        //{
        //    switch (language)
        //    {
        //        case Language.CN:
        //            return new DescriptionPackage(
        //                    null, null, "支付", null, "解锁",
        //                    CN_LOCK_IMP,
        //                    CN_PROFICIENCY_IMP);
        //        default:
        //            return new DescriptionPackage(
        //                    null, null, "Pay", null, "Unlock",
        //                    LOCK_IMP,
        //                    CN_PROFICIENCY_IMP);
        //    }
        //}
    }
}
