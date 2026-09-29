namespace AKRS.ZX2200.BondSystem.BondForce.Services
{
    using AKRS.Galaxy2.Log;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using DevExpress.CodeParser;
    using DevExpress.DataAccess.Native.Excel;
    using DevExpress.XtraEditors;
    using log4net.Core;
    using OfficeOpenXml;
    using SqlSugar;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// 力控服务类
    /// </summary>
    public static class ForceCalibrationService
    {
        /// <summary>
        /// 实际力转输入力(GT机台用)
        /// </summary>
        /// <param name="force">实际力(g)</param>
        /// <param name="initialValue">初始值</param>
        /// <param name="angle">当前角度</param>
        /// <returns>结果(N)</returns>
        public static double ActualForceToInputForce(double force, double initialValue, double angle)
        {
            // 判定是否小力
            bool isSmallForce = JudgeIsSmallForce(force);

            if (isSmallForce == false)
            {
                // 如果是大力：通过角度找力控初始值
                initialValue = GetInitialValByAngle(angle);
            }

            List<ForceRelateAngleItem> forceRelateAngleList = isSmallForce
                                                                  ? ForceCalibrationData.GetInstance().SmallForceRelationList
                                                                  : ForceCalibrationData.GetInstance().LargeForceRelationList;

            // 找到离得最近的角度
            ForceRelateAngleItem forceRelateAngle = forceRelateAngleList.Aggregate((minItem, nextItem) =>
                Math.Abs(nextItem.Angle - angle) < Math.Abs(minItem.Angle - angle)
                    ? nextItem
                    : minItem);

            List<TheoreticalForceAndActualForce> theoreticalForceAndActualForceList =
                              forceRelateAngle.TheoreticalForceAndActualForceList.ToList();

            // 默认一个初始值
            double forceReference = 0;

            int count = theoreticalForceAndActualForceList.Count;

            double lowerLimit = theoreticalForceAndActualForceList[0].CaliTableForce;
            double upperLimit = theoreticalForceAndActualForceList[theoreticalForceAndActualForceList.Count - 1].CaliTableForce;

            // 如果列表中的实际力为0，说明没有进行标定
            if ((ForceCalibrationData.GetInstance().SmallForceRelationList.Count <= 0 && isSmallForce)
                || (ForceCalibrationData.GetInstance().LargeForceRelationList.Count <= 0 && isSmallForce == false))
            {
                throw new Exception("力控标定未完成，请先进行力控标定!");
            }
            else
            {
                // 如果输入的力值超出存储范围，则返回最小力
                if (force < lowerLimit
                    || force > upperLimit)
                {
                    AKRSXtraMessageBox.Show(
                        $"输入力值:{force}g,isSmallForce:{isSmallForce},在{angle}°超出力控范围,最大力值 ：{upperLimit}g, 最小力值:{lowerLimit}g!",
                        "异常",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    throw new Exception("输入力值超出范围!");
                    // return actualForce;
                }

                // 如果只有一条数据
                if (count == 1)
                {
                    forceReference = theoreticalForceAndActualForceList[0].CaliTableForce / 100.0;
                    return forceReference;
                }

                // 根据输入的力值先找到实际力值存储范围，计算系数，再计算对应理论力值
                for (int i = 0; i < count - 1; i++)
                {
                    if (force <= theoreticalForceAndActualForceList[i + 1].CaliTableForce && force >= theoreticalForceAndActualForceList[i].CaliTableForce)
                    {
                        if (theoreticalForceAndActualForceList[i + 1].ForceIncrement == 0)
                        {
                            throw new Exception("力控模拟量增量为0，请重新进行力控标定!");
                        }

                        double k =
                            (theoreticalForceAndActualForceList[i + 1].ForceIncrement
                             - theoreticalForceAndActualForceList[i].ForceIncrement)
                            / (theoreticalForceAndActualForceList[i + 1].CaliTableForce
                               - theoreticalForceAndActualForceList[i].CaliTableForce);

                        double b = theoreticalForceAndActualForceList[i].ForceIncrement
                                   - k * theoreticalForceAndActualForceList[i].CaliTableForce;

                        // 这里除以100是把g转成N
                        forceReference = (k * force + b + initialValue) / 100.0;

                        break;
                    }
                }

                if (isSmallForce)
                {
                    if (forceReference > ForceConfig.GetInstance().SmallForceConfigItemList.Last().ForceUpperLimit)
                    {
                        throw new Exception("力控输入模拟量超出量程，请联系设备供应商!");
                    }
                }

                return forceReference;
            }
        }

        ///// <summary>
        ///// 实际力转输入力(Etel机台用)
        ///// 已跟上面的方法合并
        ///// </summary>
        ///// <param name="force">实际力(g)</param>
        ///// <returns>力基准(N)</returns>
        //public static double ActualForceToInputForce(double force, double angle)
        //{
        //    List<ForceRelateAngleItem> forceRelateAngleList = ForceCalibrationData.GetInstance().LargeForceRelationList;

        //    // 找到离得最近的角度
        //    ForceRelateAngleItem forceRelateAngle = forceRelateAngleList.Aggregate((minItem, nextItem) =>
        //        Math.Abs(nextItem.Angle - angle) < Math.Abs(minItem.Angle - angle)
        //            ? nextItem
        //            : minItem);

        //    List<TheoreticalForceAndActualForce> theoreticalForceAndActualForceList =
        //        forceRelateAngle.TheoreticalForceAndActualForceList.ToList();

        //    // 默认一个初始值
        //    double forceReference = 0;

        //    // 如果列表中的实际力为0，说明没有进行标定，此时下压力等于力基准参数
        //    if (ForceCalibrationData.GetInstance().LargeForceRelationList.Count == 0)
        //    {
        //        throw new Exception("力控标定未完成，请先进行力控标定!");
        //    }
        //    else
        //    {
        //        // 如果输入的力值超出存储范围，则返回最小力
        //        if (force < theoreticalForceAndActualForceList[0].CaliTableForce
        //            || force > theoreticalForceAndActualForceList[theoreticalForceAndActualForceList.Count - 1].CaliTableForce)
        //        {
        //            AKRSXtraMessageBox.Show(
        //                $"输入力值超出范围,最大力值 ：{theoreticalForceAndActualForceList[theoreticalForceAndActualForceList.Count - 1].CaliTableForce}g, 最小力值:{theoreticalForceAndActualForceList[0].CaliTableForce}g!",
        //                "异常",
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Warning);

        //            throw new Exception("输入力值超出范围!");
        //            // return actualForce;
        //        }

        //        // 根据输入的力值先找到实际力值存储范围，计算系数，再计算对应理论力值
        //        for (int i = 0; i < theoreticalForceAndActualForceList.Count - 1; i++)
        //        {
        //            if (force <= theoreticalForceAndActualForceList[i + 1].CaliTableForce && force >= theoreticalForceAndActualForceList[i].CaliTableForce)
        //            {
        //                double k =
        //                    (theoreticalForceAndActualForceList[i + 1].InputForce
        //                     - theoreticalForceAndActualForceList[i].InputForce)
        //                    / (theoreticalForceAndActualForceList[i + 1].CaliTableForce
        //                       - theoreticalForceAndActualForceList[i].CaliTableForce);

        //                double b = theoreticalForceAndActualForceList[i].InputForce
        //                           - k * theoreticalForceAndActualForceList[i].CaliTableForce;

        //                forceReference = (k * force + b) / 100.0;
        //                break;
        //            }
        //        }

        //        return forceReference;
        //    }
        //}

        /// <summary>
        /// 力控增量转实际力
        /// </summary>
        /// <param name="forceIncrement">力控增量</param>
        /// <param name="isSmallForce">是否是小力</param>
        /// <param name="angle">角度</param>
        /// <returns>实际力(g)</returns>
        public static double ForceIncrementToActualForce(double forceIncrement, bool isSmallForce, double angle)
        {
            List<ForceRelateAngleItem> forceRelateAngleList = isSmallForce
                                                                  ? ForceCalibrationData.GetInstance().SmallForceRelationList
                                                                  : ForceCalibrationData.GetInstance().LargeForceRelationList;

            try
            {
                // 找到离得最近的角度
                ForceRelateAngleItem forceRelateAngle = forceRelateAngleList.Aggregate((minItem, nextItem) =>
                    Math.Abs(nextItem.Angle - angle) < Math.Abs(minItem.Angle - angle)
                        ? nextItem
                        : minItem);

                List<TheoreticalForceAndActualForce> theoreticalForceAndActualForceList =
                    forceRelateAngle.TheoreticalForceAndActualForceList.ToList();

                // 默认一个初始值
                double forceReference = 0;

                int count = theoreticalForceAndActualForceList.Count;

                // 如果列表中的实际力为0，说明没有进行标定
                if (count == 0) 
                {
                    return 0;
                }

                // 如果只有一条数据
                if (count == 1)
                {
                    return theoreticalForceAndActualForceList[0].CaliTableForce;
                }

                // 增量低于2默认焊头没有接触 这个是为了让力控实时曲线好看一点
                if (forceIncrement < 2)
                {
                    return 0;
                }

                // 增量的最大最小值
                double lowerLimit = theoreticalForceAndActualForceList.Select(it => it.ForceIncrement).Min();
                double upperLimit = theoreticalForceAndActualForceList.Select(it => it.ForceIncrement).Max();
                {
                    // 如果输入的力值小于标定的增量的最小值
                    if (forceIncrement < lowerLimit)
                    {
                        return theoreticalForceAndActualForceList[0].CaliTableForce / theoreticalForceAndActualForceList[0].ForceIncrement * forceIncrement;
                    }

                    // 如果输入的力值大于标定的最大值,按照最后一段去计算
                    if (forceIncrement > upperLimit)
                    {
                        double k =
                            (theoreticalForceAndActualForceList[count - 1].CaliTableForce
                             - theoreticalForceAndActualForceList[count - 2].CaliTableForce)
                            / (theoreticalForceAndActualForceList[count - 1].ForceIncrement
                               - theoreticalForceAndActualForceList[count - 2].ForceIncrement);

                        double b = theoreticalForceAndActualForceList[count - 1].CaliTableForce - k * theoreticalForceAndActualForceList[count - 1].ForceIncrement;

                        forceReference = k * forceIncrement + b;

                        return forceReference;
                    }

                    // 根据输入的力值先找到实际力值存储范围，计算系数，再计算对应理论力值
                    for (int i = 0; i < count - 1; i++)
                    {
                        if (forceIncrement <= theoreticalForceAndActualForceList[i + 1].ForceIncrement && forceIncrement >= theoreticalForceAndActualForceList[i].ForceIncrement)
                        {
                            double k = (theoreticalForceAndActualForceList[i + 1].CaliTableForce - theoreticalForceAndActualForceList[i].CaliTableForce)
                                       / (theoreticalForceAndActualForceList[i + 1].ForceIncrement - theoreticalForceAndActualForceList[i].ForceIncrement);

                            double b = theoreticalForceAndActualForceList[i].CaliTableForce - k * theoreticalForceAndActualForceList[i].ForceIncrement;

                            forceReference = k * forceIncrement + b;
                            break;
                        }
                    }

                    return forceReference;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Post(
            Level.Error,
            $"焊头力控增量转实际力异常：" + ex.ToString,
            LogCategory.Bond,
            ViewType.InFileAndUI);
               
               // 直接返回异常数据
                return 999;
            }
        }

        /// <summary>
        /// 获取小力切换阈值
        /// </summary>
        /// <param name="inputValue">输入的模拟量</param>
        /// <param name="initialValue">当前初始值初始值</param>
        /// <returns>切换阈值</returns>
        public static double GetSmallForceModelChange(double inputValue, double initialValue)
        {
            double modelChangeForce = default;

            // 上界
            double upperLimit = ForceConfig.GetInstance().SmallForceConfigItemList.Select(item => item.ForceUpperLimit).Max();

            // 下界
            double lowerLimit = ForceConfig.GetInstance().SmallForceConfigItemList.Select(item => item.ForceLowerLimit).Min();

            if (inputValue > upperLimit || inputValue <= 0)
            {
                throw new Exception($"力控输入的模拟量{inputValue}超过阈值0~{upperLimit}");
            }

            if (initialValue > inputValue)
            {
                throw new Exception($"力控输入的模拟量{inputValue}小于当前初始值{initialValue}");
            }

            // 分成三份每一份的间隔
            double pitch = (upperLimit - lowerLimit) / 3.0;

            if (inputValue < lowerLimit + pitch)
            {
                // 小力
                modelChangeForce = inputValue - (inputValue - initialValue) * 0.5;
            }
            else if (inputValue >= lowerLimit + pitch && inputValue < lowerLimit + pitch * 2)
            {
                // 中力
                modelChangeForce = inputValue - (inputValue - initialValue) * 0.1;
            }
            else if (inputValue >= lowerLimit + pitch * 2 && inputValue <= upperLimit)
            {
                // 大力
                modelChangeForce = inputValue - (inputValue - initialValue) * 0.01;
            }

            // 单位是kg这里要除以1000
            return modelChangeForce / 1000.0;
        }

        /// <summary>
        /// 获取大力切换阈值
        /// </summary>
        /// <param name="inputValue">输入的模拟量</param>
        /// <param name="forceConfigItem">配置项</param>
        /// <returns>切换阈值</returns>
        public static double GetForceModelChange(double inputValue, ForceConfigItem forceConfigItem)
        {
            double modelChangeForce =
                inputValue - (forceConfigItem.ForceLowerLimit - forceConfigItem.ChangeForceLowerLimit);

            //double modelChangeForce = inputValue * forceConfigItem.ChangeForceK + forceConfigItem.ChangeForceB;

            //if ((inputValue - modelChangeForce) > 10)
            //{
            //    modelChangeForce = inputValue - 10;
            //}

            // 单位是kg这里要除以1000
            return modelChangeForce / 1000.0;
        }

        /// <summary>
        /// 判断是否是小力
        /// </summary>
        /// <param name="inputValue">实际力值</param>
        /// <returns>结果</returns>
        public static bool JudgeIsSmallForce(double inputValue)
        {
            if (inputValue < 0) 
            {
                throw new Exception($"输入力值为负数,大小力判断失败！");
            }

            if (inputValue < ForceConfig.GetInstance().ForceBoundary)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 通过角度获取力控初始值
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns></returns>
        public static double GetInitialValByAngle(double angle)
        {
            return
                ForceCalibrationData.GetInstance().ForceInitialValDic.ToList().Aggregate(
                    (minItem, nextItem) =>
                        Math.Abs(nextItem.Key - angle) < Math.Abs(minItem.Key - angle) ? nextItem : minItem).Value;
        }

        /// <summary>
        ///  打印标定数据
        /// </summary>
        /// <param name="path">路径</param>
        public static void ExportCaliData(string path)
        {
            bool isSmallForceEmpty = !ForceCalibrationData.GetInstance().SmallForceRelationList.Any();
            bool isLargeForceEmpty = !ForceCalibrationData.GetInstance().LargeForceRelationList.Any();

            if (isSmallForceEmpty && isLargeForceEmpty)
            {
                return;
            }

            var source1 = isSmallForceEmpty == false ? ForceCalibrationData.GetInstance().SmallForceRelationList.SelectMany(
relation => relation.TheoreticalForceAndActualForceList,
(relation, item) => new
{
    Angle = relation.Angle,
    InputForce = item.InputForce,
    CaliTableForce = item.CaliTableForce,
    BondHeadForce = item.BondHeadForce,
    InitialValue = item.InitialValue,
    ForceIncrement = item.ForceIncrement,
}).ToList() : null;

            var source2 = isLargeForceEmpty == false ? ForceCalibrationData.GetInstance().LargeForceRelationList.SelectMany(
               relation => relation.TheoreticalForceAndActualForceList,
               (relation, item) => new
               {
                   Angle = relation.Angle,
                   InputForce = item.InputForce,
                   CaliTableForce = item.CaliTableForce,
                   BondHeadForce = item.BondHeadForce,
                   InitialValue = item.InitialValue,
                   ForceIncrement = item.ForceIncrement,
               }).ToList() : null;

            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path));

            ExcelWorksheet worksheet1 = isSmallForceEmpty ? null
                : excelPackage.Workbook.Worksheets.Add("小力");

            ExcelWorksheet worksheet2 = isLargeForceEmpty ? null
                : excelPackage.Workbook.Worksheets.Add("大力");

            // 设置列宽
            for (int i = 1; i <= 7; i++)
            {
                if (worksheet1 != null)
                {
                    worksheet1.Column(i).Width = 15;
                }

                if (worksheet2 != null)
                {
                    worksheet2.Column(i).Width = 15;
                }
            }

            // 添加标题行
            if (worksheet1 != null)
            {
                if (worksheet1.Dimension == null)
                {
                    worksheet1.Cells[1, 1].Value = "角度";

                    worksheet1.Cells[1, 2].Value = "输入（模拟量）";

                    worksheet1.Cells[1, 3].Value = "模拟量初始值";

                    worksheet1.Cells[1, 4].Value = "模拟量增量";

                    worksheet1.Cells[1, 5].Value = "应变片力值";

                    worksheet1.Cells[1, 6].Value = "标定台力值";
                }

                int lastUsedRow1 = worksheet1.Dimension != null ? worksheet1.Dimension.End.Row : 0;

                for (int i = 0; i < source1.Count; i++)
                {
                    worksheet1.Cells[lastUsedRow1 + 1 + i, 1].Value = source1[i].Angle;

                    worksheet1.Cells[lastUsedRow1 + 1 + i, 2].Value = source1[i].InputForce;

                    worksheet1.Cells[lastUsedRow1 + 1 + i, 3].Value = source1[i].InitialValue;

                    worksheet1.Cells[lastUsedRow1 + 1 + i, 4].Value = source1[i].ForceIncrement;

                    worksheet1.Cells[lastUsedRow1 + 1 + i, 5].Value =
                        source1[i].BondHeadForce;

                    worksheet1.Cells[lastUsedRow1 + 1 + i, 6].Value =
                        source1[i].CaliTableForce;
                }
            }


            if (worksheet2 != null)
            {
                if (worksheet2.Dimension == null)
                {
                    worksheet2.Cells[1, 1].Value = "角度";

                    worksheet2.Cells[1, 2].Value = "输入（模拟量）";

                    worksheet2.Cells[1, 3].Value = "模拟量初始值";

                    worksheet2.Cells[1, 4].Value = "模拟量增量";

                    worksheet2.Cells[1, 5].Value = "应变片力值";

                    worksheet2.Cells[1, 6].Value = "标定台力值";
                }

                int lastUsedRow2 = worksheet2.Dimension != null ? worksheet2.Dimension.End.Row : 0;


                for (int i = 0; i < source2.Count; i++)
                {
                    worksheet2.Cells[lastUsedRow2 + 1 + i, 1].Value = source2[i].Angle;

                    worksheet2.Cells[lastUsedRow2 + 1 + i, 2].Value = source2[i].InputForce;

                    worksheet2.Cells[lastUsedRow2 + 1 + i, 3].Value = source2[i].InitialValue;

                    worksheet2.Cells[lastUsedRow2 + 1 + i, 4].Value = source2[i].ForceIncrement;

                    worksheet2.Cells[lastUsedRow2 + 1 + i, 5].Value =
                        source2[i].BondHeadForce;

                    worksheet2.Cells[lastUsedRow2 + 1 + i, 6].Value =
                        source2[i].CaliTableForce;
                }
            }


            excelPackage.Save();
        }
    }
}
