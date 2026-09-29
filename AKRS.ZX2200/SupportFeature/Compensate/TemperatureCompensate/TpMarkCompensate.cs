
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.AdjustActionNode;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.SupportFeature.Statistics;

    using DevExpress.XtraEditors;

    using LanguageExt;

    using MathNet.Numerics;

    using PostSharp.Aspects.Advices;

    /// <summary>
    /// Mark温漂补偿
    /// </summary>
    public static class TpMarkCompensate
    {
        /// <summary>
        /// 上视补偿值
        /// </summary>
        public static AKRSPoint2D UpLookCompensation { get; set; }

        /// <summary>
        /// 取片补偿值
        /// </summary>
        public static AKRSPoint2D PickUpCompensation { get; set; }

        /// <summary>
        /// 最后一次上视补偿的时间
        /// </summary>
        public static DateTime LastUpLookVisionTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后一次取片补偿的时间
        /// </summary>
        public static DateTime LastPickUpVisionTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 温漂补偿
        /// </summary>
        /// <param name="firstTu">是否为第一次</param>
        /// <returns>结果</returns>
        public static bool TemperatureCompensation(bool firstTu)
        {
            //// 如果不开启温漂补偿，直接返回
            //if (!System2Configuration.GetInstance().IsActiveDriftCompensate)
            //{
            //    return true;
            //}

            //if ((DateTime.Now - LastUpLookVisionTime).Minutes >= System2Configuration.GetInstance().DriftCompensateIntervalTime
            //    || firstTu)
            {
                AKRSPoint3D visionPos = System2Domain.GetInstance().BondModuleController.ConvertMachineToG0Pos(BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);

                // 温漂定位
                MatchResult driftMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                    visionPos,
                    "温漂定位",
                    "上视温漂测试Mark");

                if (driftMatchResult == null)
                {
                    AKRSXtraMessageBox.Show("温度补偿失败,请检查温漂Mark模板");

                    Machine.GetInstance().Stop();
                    return false;
                }

                AKRSPoint3D point = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(driftMatchResult.CenterX, driftMatchResult.CenterY, 0));

                TemperatureCompensationEntity entity = new TemperatureCompensationEntity(
                    DateTime.Now,
                    Math.Round(driftMatchResult.CenterX, 4),
                    Math.Round(driftMatchResult.CenterY, 4),
                    Math.Round(driftMatchResult.Angle, 4),
                    Math.Round(point.X, 4),
                    Math.Round(point.Y, 4));

                if (System2Configuration.GetInstance().DriftCompensateMarkNumber == 2)
                {
                    AKRSPoint3D visionPos2 = System2Domain.GetInstance().BondModuleController
                        .ConvertMachineToG0Pos(BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos2);

                    // 温漂定位
                    MatchResult driftMatchResult2 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos2,
                        "温漂定位",
                        "上视温漂测试Mark2",true);

                    if (driftMatchResult2 == null)
                    {
                        AKRSXtraMessageBox.Show("温度补偿失败,请检查温漂Mark模板");

                        Machine.GetInstance().Stop();
                        return false;
                    }

                    point = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                        new AKRSPoint3D(
                            (driftMatchResult.CenterX + driftMatchResult2.CenterX) / 2.0,
                            (driftMatchResult.CenterY + driftMatchResult2.CenterY) / 2.0,
                            0));

                     entity = new TemperatureCompensationEntity(
                        DateTime.Now,
                        Math.Round((driftMatchResult.CenterX + driftMatchResult2.CenterX) / 2.0, 4),
                        Math.Round((driftMatchResult.CenterY + driftMatchResult2.CenterY) / 2.0, 4),
                        Math.Round((driftMatchResult.Angle + driftMatchResult2.Angle) / 2.0, 4),
                        Math.Round(point.X, 4),
                        Math.Round(point.Y, 4));
                }

                UpLookCompensation = new AKRSPoint2D(entity.AxisResultX, entity.AxisResultY);

                StatisticsDomain.GetInstance().SaveDataToDb(entity);

                LastUpLookVisionTime = DateTime.Now;

                return true;
            }

            return true;
        }

        /// <summary>
        /// 取片补偿
        /// </summary>
        /// <param name="firstTu">是否为第一次</param>
        /// <returns>结果</returns>
        public static bool PickUpCompensationVision(bool firstTu)
        {
            // 如果不开启温漂补偿，直接返回
            if (!System2Configuration.GetInstance().IsActivePickUpCompensate)
            {
                return true;
            }

            if ((DateTime.Now - LastPickUpVisionTime).Minutes >= System2Configuration.GetInstance().PickUpCompensateIntervalTime
                || firstTu)
            {
                AKRSPoint3D visionPos = BondDevicePara.GetInstance().BondHeadParam.PickMarkVisionPos1;

                // 温漂定位
                MatchResult pickUpMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                    visionPos,
                    "取片参考点Mark1",
                    "取片参考点Mark1");

                if (pickUpMatchResult == null)
                {
                    AKRSXtraMessageBox.Show("取片参考点Mark定位给失败,请检查取片参考点Mark模板");

                    Machine.GetInstance().Stop();
                    return false;
                }

                AKRSPoint3D point = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(pickUpMatchResult.CenterX, pickUpMatchResult.CenterY, 0));

                TemperatureCompensationEntity entity = new TemperatureCompensationEntity(
                    DateTime.Now,
                    Math.Round(pickUpMatchResult.CenterX, 4),
                    Math.Round(pickUpMatchResult.CenterY, 4),
                    Math.Round(pickUpMatchResult.Angle, 4),
                    Math.Round(point.X, 4),
                    Math.Round(point.Y, 4));

                if (System2Configuration.GetInstance().PickUpCompensateMarkNumber == 2)
                {
                    AKRSPoint3D visionPos2 = BondDevicePara.GetInstance().BondHeadParam.PickMarkVisionPos2;

                    // 温漂定位
                    MatchResult pickUpMatchResult2 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                        visionPos2,
                        "取片参考点Mark2",
                        "取片参考点Mark2");

                    if (pickUpMatchResult2 == null)
                    {
                        AKRSXtraMessageBox.Show("取片参考点Mark2定位失败,请检查取片参考点Mark模板");

                        Machine.GetInstance().Stop();
                        return false;
                    }

                    point = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                        new AKRSPoint3D(
                            (pickUpMatchResult.CenterX + pickUpMatchResult2.CenterX) / 2.0,
                            (pickUpMatchResult.CenterY + pickUpMatchResult2.CenterY) / 2.0,
                            0));

                    entity = new TemperatureCompensationEntity(
                       DateTime.Now,
                       Math.Round((pickUpMatchResult.CenterX + pickUpMatchResult2.CenterX) / 2.0, 4),
                       Math.Round((pickUpMatchResult.CenterY + pickUpMatchResult2.CenterY) / 2.0, 4),
                       Math.Round((pickUpMatchResult.Angle + pickUpMatchResult2.Angle) / 2.0, 4),
                       Math.Round(point.X, 4),
                       Math.Round(point.Y, 4));
                }

                PickUpCompensation = new AKRSPoint2D(entity.AxisResultX, entity.AxisResultY);

                StatisticsDomain.GetInstance().SaveDataToDb(entity);

                LastPickUpVisionTime = DateTime.Now;

                return true;
            }

            return true;
        }

        /// <summary>
        /// 获取温漂补偿
        /// </summary>
        /// <returns>结果</returns>
        public static AKRSPoint3D GetTemperatureCompensation()
        {
            if (!System2Configuration.GetInstance().IsActiveDriftCompensate || UpLookCompensation == null)
            {
                return new AKRSPoint3D();
            }

            if (Math.Abs(-UpLookCompensation.X) > 0.2 || Math.Abs(-UpLookCompensation.Y) > 0.2)
            {
                AKRSXtraMessageBox.Show("温漂补偿值过大，请检查设备是否存在问题");
                throw new Exception("温漂补偿值过大，请检查设备是否存在问题");
            }

            // 获取补偿值
            return new AKRSPoint3D(-UpLookCompensation.X, -UpLookCompensation.Y, 0);
        }

        /// <summary>
        /// 获取取片补偿
        /// </summary>
        /// <returns>结果</returns>
        public static AKRSPoint3D GetPickCompensation()
        {
            if (!System2Configuration.GetInstance().IsActivePickUpCompensate || PickUpCompensation == null)
            {
                return new AKRSPoint3D();
            }

            if (Math.Abs(PickUpCompensation.X) > 0.2 || Math.Abs(PickUpCompensation.Y) > 0.2)
            {
                AKRSXtraMessageBox.Show("取片补偿值过大，请检查设备是否存在问题");
            }

            // 获取补偿值
            return new AKRSPoint3D(PickUpCompensation.X, PickUpCompensation.Y, 0);
        }
    }
}
