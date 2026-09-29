using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;

using ch.etel.edi.dsa.v40;
using log4net.Core;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Services
{
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.LogicHardware.Hardwares.DispenseControllers;
    using AKRS.Galaxy2.LogicHardware.Services;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Utils;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 点胶动作静态执行类，主要包含点胶重复性的动作
    /// </summary>
    public static class DispenserMoveHelper
    {
        /// <summary>
        /// 锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 执行点胶、画胶动作、双轴插补，目前值支持双轴插补
        /// 只能画连续的曲线
        /// </summary>
        /// <param name="epoxyApplication">图形</param>
        /// <param name="axisX">X轴</param>
        /// <param name="axisY">Y轴</param>ObjectHelper.Clone
        /// <param name="axisZ">Z轴</param>
        /// <param name="dripElectric">点胶IO</param>
        /// <param name="point">点胶点</param>
        /// <param name="isDrip">是否出胶</param>
        /// <param name="isPrePlant">是否在预点胶板上出胶</param>
        /// <returns>是否成功</returns>
        public static ExcuteResult Interpolation(EpoxyApplication epoxyApplication, Axis axisX, Axis axisY, Axis axisZ, Electric dripElectric, AKRSPoint3D point, double safeHeight, bool isDrip = true, bool isPrePlant = false)
        {
            //DsaIpolGroup iGroup = System1Domain.GetInstance().DispenseController.GetIpolGroup();

            //try
            //{
            //    #region 数据预处理
            //    epoxyApplication = EpoxyPretreatment(epoxyApplication, point);
            //    #endregion

            //    // 点胶的高度抬升
            //    double offSetZ = 0;

            //    if (isPrePlant)
            //    {
            //        offSetZ = epoxyApplication.PrePlantOffSetZ;
            //    }
            //    else
            //    {
            //        offSetZ = epoxyApplication.OffsetZ;
            //    }

            //    point.Z += offSetZ;

            //    // 对图形进行遍历
            //    for (int i = 0; i < epoxyApplication.DispensePatternParas.Count; i++)
            //    {
            //        #region 移动到点胶的位置，提前开关胶

            //        if (i == 0)
            //        {
            //            axisZ.AbsoluteMove(safeHeight);
            //        }
            //        else
            //        {
            //            axisZ.AbsoluteMove(point.Z);
            //        }
                    
            //        // 移动到绝对安全的位置
            //        // axisZ.AbsoluteMove(point.Z + epoxyApplication.SecurityHeightForEpoxyApplication);

            //        // 移动到开始点胶的位置
            //        MotionService.MoveAxesToTargetPosition(
            //            (axisX, false, epoxyApplication.DispensePatternParas[i][0].X, AccuracyMode.HighSpeed),
            //            (axisY, false, epoxyApplication.DispensePatternParas[i][0].Y, AccuracyMode.HighSpeed));

            //        axisZ.AbsoluteMove(point.Z + epoxyApplication.SecurityHeightForEpoxyApplication);

            //        // 移动参数，这个目前不知道需不需要考虑分辨率
            //        MovePara movePara = new MovePara()
            //                                {
            //                                    // 缓慢下降的速度
            //                                    Vel = epoxyApplication.SlowTravelSpeedBeforeDispensing / 100
            //                                          * axisZ.GetVel(),

            //                                    // 加速度
            //                                    Acc = axisZ.GetAcc(),

            //                                    // 减速度
            //                                    Dec = axisZ.GetDec(),

            //                                    // 高度补偿
            //                                    TargetPosition = point.Z + offSetZ,
            //                                };

            //        // 提前开胶
            //        if (epoxyApplication.AdvanceOpenDistance < 0)
            //        {
            //            // 提前开胶，这个理论上是按时间去处理的，但是以前都是这样写的，先按照这个去写
            //            axisZ.SendAbsoluteMoveCommand(movePara);

            //            while (true)
            //            {
            //                // 啥意思 ？ 
            //                if (axisZ.GetRealPosition() <= point.Z + offSetZ - epoxyApplication.AdvanceOpenDistance)
            //                {
            //                    //  dripElectric.SetOutputValue(true);
            //                    break;
            //                }

            //                Thread.Sleep(5);
            //            }

            //            axisZ.WaitForArrival();

            //            // 判断轴有没有停止，停止之后才执行后续的步骤
            //            //while (true)
            //            //{
            //            //    if (axisZ.GetAxisState().InPosition)
            //            //    {
            //            //        break;
            //            //    }

            //            //    Thread.Sleep(5);
            //            //}
            //        }
            //        else
            //        {
            //            // 滞后开胶
            //            axisZ.AbsoluteMove(movePara);
            //            Thread.Sleep(epoxyApplication.AdvanceOpenTime);
            //            //  dripElectric.SetOutputValue(true);
            //        }

            //        #endregion

            //        // 如果画胶里面只有两个数且XY相等，则认为是点胶，不开启插补
            //        if (epoxyApplication.DispensePatternParas[i].Length == 2
            //            && epoxyApplication.DispensePatternParas[i][0].X
            //                .CompareTo(epoxyApplication.DispensePatternParas[i][1].X) == 0
            //            && epoxyApplication.DispensePatternParas[i][0].Y
            //                .CompareTo(epoxyApplication.DispensePatternParas[i][1].Y) == 0)
            //        {
            //            #region 点胶

            //            // 开启点胶阀
            //            dripElectric.SetOutputValue(isDrip);


            //            //DelayHelper.Delay(300);
            //            // 保持一定时间,这个在画胶软件里面去设置
            //            DelayHelper.Delay(epoxyApplication.DispensePatternParas[i][0].DripTime);

            //            // 点胶完成，关闭点胶阀IO
            //            dripElectric.SetOutputValue(false);

            //            if (epoxyApplication.DispensePatternParas[i][0].PostDripDelay > 0)
            //            {
            //                // 关胶后延迟  应该是断尾时间
            //                DelayHelper.Delay(epoxyApplication.DispensePatternParas[i][0].PostDripDelay);
            //            }

            //            #endregion
            //        }
            //        else
            //        {
            //            #region 画胶

            //            // 开始插补
            //            iGroup.ipolBegin();

            //            // 设置为绝对坐标系 ，不设置绝对坐标系
            //            iGroup.ipolSetAbsMode(true, -1);

            //            // 连续插补
            //            for (int j = 0; j < epoxyApplication.DispensePatternParas[i].Length; j++)
            //            {
            //                // 由于第一段是到这个位置的参数，所以需要默认值
            //                if (j == 0)
            //                {
            //                    iGroup.ipolTanVelocity(0.01);
            //                    iGroup.ipolTanAcceleration(0.01);
            //                    iGroup.ipolTanDeceleration(0.01);

            //                    // 设置抖动时间，不知道什么意思
            //                    iGroup.ipolTanJerkTime(0.01);
            //                }
            //                else
            //                {
            //                    // 设置速度,Etel的单位为M/s，外接传入为mm/s,需要转换
            //                    iGroup.ipolTanVelocity(epoxyApplication.DispensePatternParas[i][j - 1].Speed / 1000.0);

            //                    // 设置加速度
            //                    iGroup.ipolTanAcceleration(
            //                        epoxyApplication.DispensePatternParas[i][j - 1].Acc / 1000.0);
            //                    iGroup.ipolTanDeceleration(
            //                        epoxyApplication.DispensePatternParas[i][j - 1].Acc / 1000.0);

            //                    // 设置抖动时间，不知道什么意思
            //                    iGroup.ipolTanJerkTime(0.01);
            //                }

            //                if (!epoxyApplication.DispensePatternParas[i][j].OpenGlueFlag)
            //                {
            //                 //   iGroup.ipolMark2Param(5, 5, 0x0000001, 1000);
            //                    // 点胶完成，关闭点胶阀IO
            //                    dripElectric.SetOutputValue(true);
            //                }
            //                else
            //                {
            //                  //  iGroup.ipolMark2Param(5, 5, 0x0010000, 1000);
            //                    // 点胶完成，关闭点胶阀IO
            //                    dripElectric.SetOutputValue(false);
            //                }

            //                iGroup.ipolLine(
            //                    epoxyApplication.DispensePatternParas[i][j].X / 1000,
            //                    epoxyApplication.DispensePatternParas[i][j].Y / 1000);

            //                // 等待插补结束
            //                iGroup.ipolWaitMovement(100000);
            //            }

            //            // 最后关闭插补的IO
            //           // iGroup.ipolMark2Param(5, 5, 0x0010000, 1000);
            //            // 点胶完成，关闭点胶阀IO
            //            dripElectric.SetOutputValue(false);

            //            // 退出插补模式
            //            iGroup.ipolEnd();                 
            //        }

            //        #endregion

            //        #region 一段断尾

            //        if (epoxyApplication.TearOff1Open)
            //        {
            //            // 移动参数，这个目前不知道需不需要考虑分辨率
            //            MovePara tearOff1 = new MovePara()
            //                                    {
            //                                        Vel = epoxyApplication.TearOff1Speed / 100 * axisZ.GetVel(),
            //                                        Acc = axisZ.GetAcc(),
            //                                        Dec = axisZ.GetDec(),
            //                                        TargetPosition = point.Z + epoxyApplication.TearOff1Height,
            //                                    };

            //            // 一段断尾
            //            axisZ.AbsoluteMove(tearOff1);
            //            DelayHelper.Delay(epoxyApplication.TearOff1Delay);
            //        }

            //        #endregion

            //        #region 二段断尾

            //        if (epoxyApplication.TearOff2Open)
            //        {
            //            // 移动参数，这个目前不知道需不需要考虑分辨率
            //            MovePara tearOff2 = new MovePara()
            //                                    {
            //                                        Vel = epoxyApplication.TearOff2Speed / 100 * axisZ.GetVel(),
            //                                        Acc = axisZ.GetAcc(),
            //                                        Dec = axisZ.GetDec(),
            //                                        TargetPosition = point.Z + epoxyApplication.TearOff2Height,
            //                                    };

            //            // 二段短尾
            //            axisZ.AbsoluteMove(tearOff2);
            //            DelayHelper.Delay(epoxyApplication.TearOff2Delay);
            //        }

            //        #endregion
            //    }

            //    axisZ.AbsoluteMove(point.Z + epoxyApplication.SecurityHeightForEpoxyApplication);

            //    return ExcuteResult.Success;
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Post(Level.Error, $"系统1画胶功能异常，请联系管理员", ex, LogCategory.Dispense);
            //    AKRSMessageBoxExt.Show(
            //        ex.Message,
            //        "异常",
            //        new string[] { "异常" },
            //        new DialogResult[] { DialogResult.Yes });
            //    return ExcuteResult.Exception;
            //}
            //finally
            //{
             
            //}

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 画胶数据预处理，主要针对角度和大小
        /// </summary>
        /// <param name="epoxyApplication">画胶的图像</param>
        /// <param name="point3D">位置</param>
        /// <param name="angle">角度</param>
        /// <returns>处理完成的画胶图形</returns>
        public static EpoxyApplication EpoxyPretreatment(EpoxyApplication epoxyApplication, AKRSPoint3D point3D, double angle)
        {
            // 先克隆一个，避免后续将原数据更改掉了
            EpoxyApplication epoxy = ObjectHelper.Clone(epoxyApplication);

            double sizeMaxX = 0;
            double sizeMinX = 0;
            double sizeMaxY = 0;
            double sizeMinY = 0;

            // 找出最大值和最小值，为归一化做准备
            for (int i = 0; i < epoxy.DispensePatternParas.Count; i++)
            {
                for (int j = 0; j < epoxy.DispensePatternParas[i].Length; j++)
                {
                    EpoxyApplicationLocation location = epoxy.DispensePatternParas[i][j];
                    if (i == 0 && j == 0)
                    {
                        sizeMaxX = location.X;
                        sizeMinX = location.X;
                        sizeMaxY = location.Y;
                        sizeMinY = location.Y;
                    }
                    else
                    {
                        sizeMaxX = Math.Max(sizeMaxX, location.X);
                        sizeMaxY = Math.Max(sizeMaxY, location.Y);
                        sizeMinX = Math.Min(sizeMinX, location.X);
                        sizeMinY = Math.Min(sizeMinY, location.Y);
                    }
                }
            }

            double offsetX = (sizeMaxX + sizeMinX) / 2.0;
            double offsetY = (sizeMaxY + sizeMinY) / 2.0;
            sizeMaxX = sizeMaxX - offsetX;
            sizeMaxY = sizeMaxY - offsetY;

            // 归一化
            for (int i = 0; i < epoxy.DispensePatternParas.Count; i++)
            {
                for (int j = 0; j < epoxy.DispensePatternParas[i].Length; j++)
                {
                    EpoxyApplicationLocation location = epoxy.DispensePatternParas[i][j];

                    location.X = (location.X - offsetX) * epoxy.SizeX / sizeMaxX / 2.0;

                    location.Y = (location.Y - offsetY) * epoxy.SizeY / sizeMaxY / 2.0;
                }

                if (epoxy.DispensePatternParas[i].Length == 2 
                    && epoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.SingleDotOnly)
                {
                    epoxy.DispensePatternParas[i][0].X =
                        (epoxy.DispensePatternParas[i][0].X + epoxy.DispensePatternParas[i][1].X) / 2.0;

                    epoxy.DispensePatternParas[i][0].Y =
                        (epoxy.DispensePatternParas[i][0].Y + epoxy.DispensePatternParas[i][1].Y) / 2.0;
                }
            }

            // 速度初始化
            for (int i = 0; i < epoxy.DispensePatternParas.Count; i++)
            {
                for (int j = 0; j < epoxy.DispensePatternParas[i].Length; j++)
                {
                    if (epoxy.DispensePatternParas[i][j].Speed <= 0)
                    {
                        epoxy.DispensePatternParas[i][j].Speed = 100;
                    }
                    
                    if (epoxy.DispensePatternParas[i][j].Acc <= 0)
                    {
                        epoxy.DispensePatternParas[i][j].Acc = 1000;
                    }
                    
                    if (epoxy.DispensePatternParas[i][j].Dec <= 0)
                    {
                        epoxy.DispensePatternParas[i][j].Dec = 1000;
                    }

                    epoxy.DispensePatternParas[i][j].Speed =
                        epoxy.DispensePatternParas[i][j].Speed * epoxy.SpindleSpeedPercentage / 100.0;
                    epoxy.DispensePatternParas[i][j].Acc =
                        epoxy.DispensePatternParas[i][j].Acc * epoxy.SpindleSpeedPercentage / 100.0;
                    epoxy.DispensePatternParas[i][j].Dec =
                        epoxy.DispensePatternParas[i][j].Dec * epoxy.SpindleSpeedPercentage / 100.0;

                    if (double.IsNaN(epoxy.DispensePatternParas[i][j].X))
                    {
                        epoxy.DispensePatternParas[i][j].X = 0;
                    }

                    if (double.IsNaN(epoxy.DispensePatternParas[i][j].Y))
                    {
                        epoxy.DispensePatternParas[i][j].Y = 0;
                    }
                }
            }

            // 添加旋转和偏移
            for (int i = 0; i < epoxy.DispensePatternParas.Count; i++)
            {
                for (int j = 0; j < epoxy.DispensePatternParas[i].Length; j++)
                {
                    EpoxyApplicationLocation location = epoxy.DispensePatternParas[i][j];

                    double x = location.X;
                    double y = location.Y;
                    // 转化成弧度
                    double degree = (epoxy.Angle + angle) * Math.PI / 180.0;

                    location.X = x * Math.Cos(degree) - y * Math.Sin(degree) + /*epoxy.OffsetX +*/ point3D.X;
                    location.Y = x * Math.Sin(degree) + y * Math.Cos(degree) + /*epoxy.OffsetY*/ + point3D.Y;
                    location.Z = location.Z + point3D.Z;
                }
            }

            return epoxy;
        }


        /// <summary>
        /// 打开安全门
        /// </summary>
        public static void OpenSafeDoor()
        {
            return;
        }

        /// <summary>
        /// 关闭安全门
        /// </summary>
        public static void CloseSafeDoor()
        {
            return;
        }

        /// <summary>
        /// 检测画家图形数据的合理性
        /// 例如速度是否为0
        /// </summary>
        /// <param name="epoxyApplication">画家的图形</param>
        /// <returns>结果</returns>
        public static bool CheckEpoxyApplication(EpoxyApplication epoxyApplication)
        {
            if (epoxyApplication == null)
            {
                return false;
            }

            for (int i = 0; i < epoxyApplication.DispensePatternParas.Count; i++)
            {
                for (int j = 0; j < epoxyApplication.DispensePatternParas[i].Length; j++)
                {
                    if (j != epoxyApplication.DispensePatternParas[i].Length - 1)
                    {
                        if (!IsEpoxyDataReasonable(epoxyApplication.DispensePatternParas[i][j].Acc))
                        {
                            AKRSXtraMessageBox.Show($"第{i + 1}条线 第{j + 1}段 加速度数据不合理");
                            return false;
                        }

                        if (!IsEpoxyDataReasonable(epoxyApplication.DispensePatternParas[i][j].Dec))
                        {
                            AKRSXtraMessageBox.Show($"第{i + 1}条线 第{j + 1}段 减速度数据不合理");
                            return false;
                        }

                        if (!IsEpoxyDataReasonable(epoxyApplication.DispensePatternParas[i][j].Speed))
                        {
                            AKRSXtraMessageBox.Show($"第{i + 1}条线 第{j + 1}段 速度数据不合理");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 判断单个数据是否合格
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        public static bool IsEpoxyDataReasonable(double data)
        {
            if (data == 0)
            {
                data = 10;
            }

            if (data < 0 || data > 10000)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断单个数据是否合格
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        public static bool IsEpoxyDataReasonableSpeed(double data)
        {
            if (data == 0)
            {
                data = 10;
            }

            if (data < 0 || data > 1000)
            {
                return false;
            }

            return true;
        }
    }
}
