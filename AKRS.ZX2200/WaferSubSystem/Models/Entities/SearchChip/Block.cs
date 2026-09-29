using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.Infrastructure.EventBus;
//using AKRS.Galaxy2.PR.Models.MatchResults;
using DevExpress.XtraEditors;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using System.Windows.Media.Media3D;
    using Accord.Statistics.Testing;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CustomControls.Components;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.WM;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Controls;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Carrier;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Structs;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using AKRSAKRS.Galaxy2.PR.Models.Algs;
    using DevExpress.Utils.Win.Hook;
    using log4net.Core;
    using OfficeOpenXml;

    /// <summary>
    /// 芯片搜索类
    /// </summary>
    public partial class Block : SingletonNoSave<Block>
    {
        /// <summary>
        /// 外框偏差
        /// </summary>
        public AKRSPoint3D FrameOffset = new AKRSPoint3D();

        /// <summary>
        /// 芯片分析
        /// </summary>
        /// <param name="isSpringBoard">是否是跳板晶片(只用来定位但不取的晶片)</param>
        /// <param name="isSkip">是否跳过</param>
        /// <returns>相机输出结果</returns>
        private MatchResult AnalyseDie(bool isSpringBoard, out bool isSkip, string matchTemplateName)
        {
        RetryAnalyseDie:

            isSkip = false;

            bool isOK;

            // 拍照定位并获取相机结果
            (bool isSucceed, MatchResult[] matchResults) result = this.MatchResult(matchTemplateName);

            isOK = result.isSucceed;

            if (this.currentCarrierConfig.IsFrameSearch)
            {
                (bool isSucceed, MatchResult[] matchResults) resultFrame = this.MatchFrameResult();

                if (result.isSucceed && resultFrame.isSucceed)
                {
                    isOK = true;

                    AKRSPoint3D point1 = MachineCoordinateSystem.GetInstance().WaferCameraCoordinateSystem.ForwardConvertCoordinate(new AKRSPoint3D(result.matchResults[0].CenterX, result.matchResults[0].CenterY, 0));
                    AKRSPoint3D point2 = MachineCoordinateSystem.GetInstance().WaferCameraCoordinateSystem.ForwardConvertCoordinate(new AKRSPoint3D(resultFrame.matchResults[0].CenterX, resultFrame.matchResults[0].CenterY, 0));
                    this.FrameOffset = point2 - point1;
                
                }
                else
                {
                    isOK = false;
                }
            }
            else
            {
                this.FrameOffset = new AKRSPoint3D();
            }

            // 不为空
            if (isOK/*result.isSucceed*/)
            {
                // 墨点检测
                if (this.IsInkDotSearch && !isSpringBoard)
                {
                    if (this.BlobResult(result.matchResults[0]))
                    {
                        // 有墨点
                        if (this.IsBlobAutoSkip)
                        {
                            isSkip = true;
                        }
                        else
                        {
                            this.currentCarrierConfig.AddCountOfPositionError();

                            //this.res = AKRSMessageBoxExt.Show($"Ink spot detected, determine whether to skip, click \r\n" + "OK：Don't skip\r\n" + "Skip：skip\r\n" + "Abort: terminates thread;", "Prompt", new string[] { "OK", "Skip", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Ignore, DialogResult.Abort });
                            this.res = AKRSMessageBoxExt.Show($"检测到墨点, 是否跳过？, 点击 \r\n" + "OK：不跳过\r\n" + "Skip：跳过\r\n" + "Abort: 停止;", "Prompt", new string[] { "OK", "Skip", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Ignore, DialogResult.Abort });
                            switch (this.res)
                            {
                                case DialogResult.OK:
                                    isSkip = false;
                                    break;
                                case DialogResult.Ignore:
                                    isSkip = true;
                                    break;

                                case DialogResult.Abort:
                                    // 终止
                                    //throw new Exception($"Ink spot detected!");
                                    throw new Exception($"检测到墨点！");
                            }
                        }

                        if (isSkip)
                        {
                            this.currentCarrierConfig.AddCountOfInkDot();
                            this.currentCarrierConfig.AddCountOfTotal();
                            this.countBlankNum = 0;
                            return this.CameraCenterPixel;
                        }
                    }
                }

            //// 与计算的偏差距离
            //double disOffset = Math.Sqrt(offset[0].X * offset[0].X + offset[0].Y * offset[0].Y);
            //double maxAllowColOffset = 0.33 * this.DisColSize();
            //double maxAllowRowOffset = 0.33 * this.DisRowSize();
            //if (this.DisColSize() < 600)
            //{
            //    maxAllowColOffset = 0.4 * this.DisColSize();
            //}

            //if (this.DisRowSize() < 600)
            //{
            //    maxAllowRowOffset = 0.4 * this.DisRowSize();
            //}

            //// 使用晶圆图定位的偏差大于1/3的行间距或列间距可能定位到错误芯片了
            //if (disOffset > maxAllowColOffset || disOffset > maxAllowRowOffset)
            //{
            //    // 定位位置与计算位置偏差过大报警
            //    DialogResult res = AKRSMessageBoxExt.Show($"定位位置与计算位置偏差过大，确定是否跳过, 点击 \r\n" + "继续：使用定位位置\r\n" + "跳过：不使用定位位置，并跳过该芯片\r\n", "提示", new string[] { "继续", "跳过" }, new DialogResult[] { DialogResult.OK, DialogResult.Ignore });
            //    switch (res)
            //    {
            //        case DialogResult.OK:
            //            break;
            //        case DialogResult.Ignore:
            //            isSkip = true;
            //            break;
            //    }

            //    this.countBlankNum = 0;
            //    return true;
            //}

            RetryTimesLocation:

                // 开启多次定位
                if (this.IsTimesLocation && !isSpringBoard)
                {
                    (bool isSucceed, MatchResult matchResult) resultTimesLocation = this.ToRequiredAccuracy();
                    if (!resultTimesLocation.isSucceed)
                    {
                        // 多次定位报警
                        //this.res = AKRSMessageBoxExt.Show($"The accuracy does not meet the requirements after multiple positioning! , click \r\n" + "Ignore：Skip the chip\r\n" + "Retry：retry match", "Prompt", new string[] { "Ignore", "Retry" }, new DialogResult[] { DialogResult.Ignore, DialogResult.Retry });
                        this.res = AKRSMessageBoxExt.Show($"多次定位后，未达到目标精度！ , 点击 \r\n" + "Ignore：跳过当前芯片\r\n" + "Retry：重新检测", "Prompt", new string[] { "Ignore", "Retry" }, new DialogResult[] { DialogResult.Ignore, DialogResult.Retry });
                        switch (this.res)
                        {
                            case DialogResult.Ignore:
                                break;
                            case DialogResult.Retry:
                                goto RetryTimesLocation;
                        }

                        isSkip = true;
                        this.countBlankNum = 0;
                    }

                    return resultTimesLocation.matchResult;
                }

                return result.matchResults[0];
            }
            else
            {
                if (!this.IsBlankDieAutoSkip || this.countBlankNum + 1 >= this.BlankDieAutoSkipMaxTimes)
                {
                    if (!isSpringBoard)
                    {
                        this.currentCarrierConfig.AddCountOfPositionError();

                        if (WaferTableDevicePara.IsAheadReachEnd && this.IsBlankDieAutoSkip)
                        {
                            if (this.GetSearchMode() == SearchMode.Single)
                            {
                                if (this.countBlankNum + 1 < this.BlankDieAutoSkipMaxTimes * 3)
                                {
                                    if (this.countBlankNum + 1 == this.BlankDieAutoSkipMaxTimes)
                                    {
                                        // 单独小功能，进行嵌入
                                        this.AheadReachEnd();
                                        this.countBlankNum++;

                                        goto RetryAnalyseDie;
                                    }

                                    this.countBlankNum++;
                                    isSkip = true;
                                    return this.CameraCenterPixel;
                                }
                            }
                        }

                    RetrySelectPoint:

                        PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(matchTemplateName);

                        string message;
                        if (Block.GetInstance().GetSearchMode() == SearchMode.Single)
                        {
                            message = $"晶圆视觉模板：芯片模板{prEntity.GetName()}定位失败，当前方向为{Block.GetInstance().GetSingleSearchDirection()}";
                        }
                        else
                        {
                            message = $"晶圆视觉模板：芯片模板{prEntity.GetName()}定位失败";
                        }

                        (DialogResult dialog, BaseAlgResult matchResult) result1 = UcMainSystem.VisionAlarmFunc(
                                prEntity,
                                message,
                                "搜晶失败");

                        switch (result1.dialog)
                        {
                            case DialogResult.OK:
                                if (this.SearchMode != SearchMode.Box & !this.IsInSearchRange(this.WaferTableRealPositionToG0))
                                {
                                    // 如果选取的位置在搜索范围外，则重新选择点位
                                    AKRSMessageBoxExt.Show($"选取的位置在搜索范围外，则重新选择点位!", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                                    goto RetrySelectPoint;
                                }

                                // 相当于重试
                                goto RetryAnalyseDie;
                            case DialogResult.Cancel:
                                // 相当于重试
                                goto RetryAnalyseDie;
                            case DialogResult.Abort:
                                // 相当于终止
                                throw new Exception($"定位并移动到搜晶相机中心失败, 晶圆线程将停止！");
                            case DialogResult.Retry:
                                // 相当于提前更换
                                isSkip = true;
                                this.isNeedAheadChangeTablet = true;
                                break;
                            case DialogResult.Ignore:
                                // 相当于跳过
                                this.countBlankNum = 0;
                                isSkip = true;
                                break;
                        }

                        return this.CameraCenterPixel;
                    }
                    else
                    {
                        //AKRSXtraMessageBox.Show("It's a springboard chip. It skips the search for the next one!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.countBlankNum = 0;
                        isSkip = true;
                        return this.CameraCenterPixel;
                    }
                }
                else
                {
                    this.countBlankNum++;
                    isSkip = true;
                }

                // 未检测到
                return this.CameraCenterPixel;
            }
        }

        /// <summary>
        /// 芯片分析
        /// </summary>
        /// <param name="isSpringBoard">是否是跳板晶片(只用来定位但不取的晶片)</param>
        /// <param name="isSkip">是否跳过</param>
        /// <returns>相机输出结果</returns>
        private MatchResult AnalyseDie(double visionPos,bool isSpringBoard, out bool isSkip, string matchTemplateName)
        {
           
            return this.AnalyseDie(isSpringBoard, out isSkip, matchTemplateName);
        }

        /// <summary>
        /// 下一颗搜晶方式
        /// </summary>
        /// <returns>result</returns>
        public bool NextDie()
        {
            if (this.SearchMode == SearchMode.Single)
            {
                return this.SearchSingle();
            }
            else if (this.SearchMode == SearchMode.Nine)
            {
                return this.SearchNine();
            }
            else if (this.SearchMode == SearchMode.WaferMap)
            {
                return this.SearchMapping();
            }
            else if (this.SearchMode == SearchMode.Box)
            {
                return this.SearchDieBox();
            }
            else
            {
                throw new Exception("No exist the Search Mode!");
            }
        }

        /// <summary>
        /// 定位时是否需要开真空
        /// </summary>
        /// <returns>result</returns>
        private bool IsMatchWithVacuum()
        {
            if (this.currentCarrierConfig.IsMatchWithVacuum)
            {
                return true;                
            }
            else
            {
                return false;
            }

            //if (this.currentCarrierConfig is CarrierWithWaferConfig carrierWithWaferConfig)
            //{
            //    if (carrierWithWaferConfig.IsMatchWithVacuum)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    return false;
            //}
        }

        /// <summary>
        /// 移动到下一颗芯片
        /// </summary>
        /// <param name="direction">direction</param>
        public void NextDie(DirectionEnum direction)
        {
            AKRSPoint3D position = this.WaferTableRealPositionToG0;
            switch (direction)
            {
                case DirectionEnum.North:
                    {
                        position.Y += this.CarrierRowSpacing.Y;
                    }

                    break;
                case DirectionEnum.South:
                    {
                        position.Y -= this.CarrierRowSpacing.Y;
                    }

                    break;
                case DirectionEnum.West:
                    {
                        position.X += this.CarrierColSpacing.X;
                    }

                    break;
                case DirectionEnum.East:
                    {
                        position.X -= this.CarrierColSpacing.X;
                    }

                    break;
                case DirectionEnum.Centre:
                    break;
            }

            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(position);
            this.MoveToCameraCenterWithThread();
        }

        /// <summary>
        /// 是否是对称模板
        /// </summary>
        private bool isSymmetricModel;

        /// <summary>
        /// Wafer相机视觉定位芯片-得到像素结果
        /// </summary>
        /// <param name="matchTemplateName">matchTemplateName</param>
        /// <returns>定位结果</returns>
        public (bool isSucceed, MatchResult[] matchResults) MatchResult(string matchTemplateName, bool isSaveImg = false, bool isCheckBondPos = true)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || MachineStateModel.GetInstance().IsDryCycle)
            {                
                return (true, new MatchResult[1] { this.CameraCenterPixel });
            }  
            
            MatchResult[] results = null;
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(matchTemplateName);
            if (prEntity != null)
            {
                if (prEntity.Alg.AlgFlowType == AlgFlowTypeEnum.SymmetricModeleAlg)
                {
                    AKRSPoint3D pixcelResult = new AKRSPoint3D(prEntity.Alg.SymmetricCenterX, prEntity.Alg.SymmetricCenterY, 0);
                    AKRSPoint3D posResult = this.GetOffsetWorld(pixcelResult);
                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.WaferTableRealPositionToG0 + posResult);

                    this.isSymmetricModel = true;
                }
                else
                {
                    this.isSymmetricModel = false;
                }

                if (this.IsMatchWithVacuum())
                {
                    if (this.currentCarrierConfig is CarrierWithWaferConfig)
                    {
                        WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();
                    }
                    else if (this.currentCarrierConfig is CarrierWithStaticWaffleConfig)
                    {
                        WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
                    }
                    else if (this.currentCarrierConfig is CarrierWithWaffleConfig)
                    {
                        WaferSubController.GetInstance().WaferTableController.OpenWaffleVacuum();
                    }
                }
                else
                {
                    if (this.currentCarrierConfig is CarrierWithWaferConfig)
                    {
                        WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                    }
                    else if (this.currentCarrierConfig is CarrierWithStaticWaffleConfig)
                    {
                        WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
                    }
                    else if (this.currentCarrierConfig is CarrierWithWaffleConfig)
                    {
                        WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                    }
                }

                Static.RecordTime("晶圆线程", "定位前真空操作完成");

                // 相机拍照延时
                Thread.Sleep(this.DelayPhoto);

                Stopwatch sw = Stopwatch.StartNew();

                if (isCheckBondPos)
                {
                    if (this.currentCarrierConfig.SearchCamera == SearchCameraEnum.WaferCamera)
                    {
                        // 判断能否拍照，根据bond位置，不符合就一直while
                        while (WaferSubController.GetInstance().WaferTableController.IsNotAllowWaferCameraAction(this.currentCarrierConfig.AdjustCamera))
                        {
                            if (sw.ElapsedMilliseconds > 8000)
                            {
                                LogHelper.Post(Level.Error, $"晶圆相机定位-等Bond模组到避让位超时{sw.ElapsedMilliseconds}ms!", LogCategory.Component);

                                sw.Restart();

                                //this.res = AKRSMessageBoxExt.Show(
                                //    $"Wait Wafer camera action out of time!\r\n Continue:continue;\r\n Abort:terminates thread;",
                                //    "Alarm",
                                //    new string[] { "Continue", "Abort" },
                                //    new DialogResult[] { DialogResult.OK, DialogResult.Abort, },
                                //    AlarmLevel.SecondLevel);
                                //switch (this.res)
                                //{
                                //    case DialogResult.OK:
                                //        sw.Restart();
                                //        continue;

                                //    case DialogResult.Abort:
                                //        // 终止
                                //        throw new Exception($"Wait Wafer camera action out of time!");
                                //}
                            }

                            this.StopSearch();

                            continue;
                        }
                    }
                    else
                    {
                        // 判断Bond是否到位，不符合就一直while
                        while (true)
                        {
                            this.StopSearch();

                            if (System2RunTimeProvider.IsBondArriveSearchPos)
                            {
                                // 位置检查
                                if (System2Domain.GetInstance().BondModuleController
                                        .IsBondModuleAtG0Pos(this.currentCarrierConfig.BondCameraSearchPosition)
                                    == false)
                                {
                                    LogHelper.Post(Level.Error, $"搜晶失败：Bond模组不在搜晶位置!", LogCategory.Component);

                                    throw new Exception($"搜晶失败：Bond模组不在搜晶位置!");
                                }

                                break;
                            }

                            Thread.Sleep(10);
                        }
                    }
                }

                Static.RecordTime("晶圆线程", "等Bond模组位置检查完成");

                //sw.Stop();
                //sw.Reset();

                ExcuteResult prResult = prEntity.DoWork();
                if (prResult == ExcuteResult.Success)
                {
                    List<BaseAlgResult> algResults = prEntity.AlgResults;
                    results = new MatchResult[algResults.Count];

                    for (int i = 0; i < algResults.Count; i++)
                    {
                        results[i] = (MatchResult)algResults[i];
                        if (isSaveImg)
                        {
                            Directory.CreateDirectory("C://搜晶相机存图");
                            results[i].OutPutImg1.Save($"C://搜晶相机存图//{DateTime.Now.ToString("yyMMddhhmmss")}.bmp");
                        }
                    }

                    Static.RecordTime("晶圆线程", $"模板：{this.DieMatchName}定位成功");

                    return (true, results);
                }
                else
                {
                    if (this.currentCarrierConfig is CarrierWithWaferConfig)
                    {
                        WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                    }
                    else if (this.currentCarrierConfig is CarrierWithStaticWaffleConfig)
                    {
                        //WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
                    }
                    else if (this.currentCarrierConfig is CarrierWithWaffleConfig)
                    {
                        //WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                    }
                    
                    if (prEntity.Alg.AlgFlowType == AlgFlowTypeEnum.SymmetricModeleAlg)
                    {
                        AKRSPoint3D pixcelResult = new AKRSPoint3D(prEntity.Alg.SymmetricCenterX, prEntity.Alg.SymmetricCenterY, 0);
                        AKRSPoint3D posResult = this.GetOffsetWorld(pixcelResult);
                        WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.WaferTableRealPositionToG0 - posResult);
                    }

                    Static.RecordTime("晶圆线程", $"模板：{this.DieMatchName}定位失败");

                    return (false, null);
                }
            }
            else
            {
                //AKRSXtraMessageBox.Show($"The template-{matchTemplateName} is not exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AKRSXtraMessageBox.Show($"视觉模板-{matchTemplateName} 不存在！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return (false, null);
            }
        }

        /// <summary>
        /// 视觉定位芯片-得到像素结果（Block专用）
        /// </summary>
        /// <returns>定位结果</returns>
        public (bool isSucceed, MatchResult[] matchResults) MatchResult()
        {
            return this.MatchResult(this.DieMatchName);
        }

        /// <summary>
        /// 视觉定位芯片框架-得到像素结果（Block专用）
        /// </summary>
        /// <returns>定位结果</returns>
        public (bool isSucceed, MatchResult[] matchResults) MatchFrameResult()
        {
            return this.MatchResult(this.DieFrameName);
        }

        /// <summary>
        /// 将像素offset转到worldOffset(G0)
        /// </summary>
        /// <param name="results">定位结果</param>
        /// <returns>像素差值对应的物理差值</returns>
        public AKRSPoint3D[] MatchResultToWorld(MatchResult[] results)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || MachineStateModel.GetInstance().IsDryCycle)
            {
                return new AKRSPoint3D[1] { new AKRSPoint3D() };
            }

            if (results != null && results.Length > 0)
            {
                AKRSPoint3D[] offsetWorld = new AKRSPoint3D[results.Length];
                for (int i = 0; i < results.Length; i++)
                {
                    offsetWorld[i] = this.GetOffsetWorld(new AKRSPoint3D(results[i].CenterX, results[i].CenterY, 0));
                }

                return offsetWorld;
            }

            return null;
        }

        /// <summary>
        /// 将像素offset转到worldOffset(G0)
        /// </summary>
        /// <param name="results">定位结果</param>
        /// <returns>像素差值对应的物理差值</returns>
        public AKRSPoint3D MatchResultToWorld(MatchResult results)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || MachineStateModel.GetInstance().IsDryCycle)
            {
                return new AKRSPoint3D();
            }

            AKRSPoint3D offsetWorld = this.GetOffsetWorld(new AKRSPoint3D(results.CenterX, results.CenterY, 0));

            return offsetWorld;
        }

        /// <summary>
        /// 移动到相机中心(Block专用)
        /// </summary>
        /// <param name="results">results</param>
        public void MoveToCameraCenterWithThread(MatchResult[] results)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || !this.IsPositionSearch || MachineStateModel.GetInstance().IsDryCycle)
            {
                return;
            }

            AKRSPoint3D result = MachineCoordinateSystem.GetInstance().WaferCameraCoordinateSystem.SelfPosToG0(new AKRSPoint3D(results[0].CenterX, results[0].CenterY, 0), this.WaferTableRealPosition);
            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(result);
        }

        /// <summary>
        /// 定位并移动到相机中心(Block专用)
        /// </summary>
        /// <returns>result</returns>
        public bool MoveToCameraCenterWithThread()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || !this.IsPositionSearch || MachineStateModel.GetInstance().IsDryCycle)
            {
                return true;
            }

            (bool isSucceed, MatchResult[] matchResults) result = this.MatchResult();
            if (result.isSucceed)
            {
                this.MoveToCameraCenterWithThread(result.matchResults);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 定位并移动到相机中心
        /// </summary>
        /// <param name="matchTemplateName">matchTemplateName</param>
        /// <returns>result</returns>
        public bool MoveToCameraCenter(string matchTemplateName, bool isSaveImg = false, bool isCheckBondPos = true)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || MachineStateModel.GetInstance().IsDryCycle)
            {
                return true;
            }

            (bool isSucceed, MatchResult[] matchResults) result = this.MatchResult(matchTemplateName, isSaveImg, isCheckBondPos);
            if (result.isSucceed)
            {
                AKRSPoint3D point = MachineCoordinateSystem.GetInstance().WaferCameraCoordinateSystem.SelfPosToG0(new AKRSPoint3D(result.matchResults[0].CenterX, result.matchResults[0].CenterY, 0), this.WaferTableRealPosition);
                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(point, true);

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 墨点定位芯片
        /// </summary>
        /// <param name="blobTemplateName">blobTemplateName</param>
        /// <param name="matchResult">matchResult</param>
        /// <returns>定位结果</returns>
        public bool BlobResult(string blobTemplateName, MatchResult matchResult)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork || MachineStateModel.GetInstance().IsDryCycle)
            {
                return false;
            }

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(blobTemplateName);
            if (prEntity != null)
            {
                // 设置光源
                prEntity.SetLight();

                // 采图
                Bitmap bmp = prEntity.Photograph(prEntity.CameraName);

                // 设置位置修正系数
                prEntity.Alg.SetFix((float)matchResult.CenterX, (float)matchResult.CenterY, (float)matchResult.Angle);

                // 执行墨点检测
                ExcuteResult prResult = prEntity.DoWork(bmp);
                if (prResult == ExcuteResult.Success)
                {
                    List<BaseAlgResult> matchResults = prEntity.AlgResults;
                    if (matchResults[0].IsSuccess)
                    {
                        bmp.Dispose();

                        //// 有墨点
                        //return true;

                        // 姚裕---墨点数量需求
                        if (this.currentCarrierConfig.IsCheckBlobNum)
                        {
                            int num = ((BlobResult)matchResults[0]).Num;
                            if (num <= this.currentCarrierConfig.CheckBlobNumUpperLimit && num >= this.currentCarrierConfig.CheckBlobNumLowerLimit)
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        bmp.Dispose();

                        // 无墨点
                        return false;
                    }
                }
                else
                {
                    bmp.Dispose();

                    // 执行不成功当墨点处理
                    return true;
                }

                //ExcuteResult prResult = prEntity.DoWork();
                //if (prResult == ExcuteResult.Success)
                //{
                //    BlobResult algResults = (BlobResult)prEntity.AlgResult;

                //    return algResults.IsSuccess;
                //}
                //else
                //{
                //    return false;
                //}
            }
            else
            {
                //AKRSXtraMessageBox.Show($"The template-{blobTemplateName} is not exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AKRSXtraMessageBox.Show($"视觉模板-{blobTemplateName} 不存在！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }

        /// <summary>
        /// 墨点定位芯片
        /// </summary>
        /// <param name="matchResult">matchResult</param>
        /// <returns>定位结果</returns>
        public bool BlobResult(MatchResult matchResult)
        {
            return this.BlobResult(this.DieBlobName, matchResult);
        }

        /// <summary>
        /// 刷新显示信息
        /// </summary>
        /// <param name="isDebugMode">是否调试模式</param>
        public void RefreshMap(bool isDebugMode = false)
        {
            LogHelper.Post(Level.Info, $"刷新操作开始", LogCategory.Component);
            if (this.currentCarrierConfig.CarrierType != CarrierTypeEnum.StaticWaffle && !MachineStateModel.GetInstance().IsOffLineWork)
            {
                double tar = this.GetPickPosZ();
                double postionZ = System2Domain.GetInstance().BondHeadController.GetAxisZRealPos();

                // 抬高mm处进行回芯片中心操作
                while (postionZ - tar < this.RaiseDistanceToRefreshMapAfterBondPicked)
                {
                    this.StopSearch();

                    postionZ = System2Domain.GetInstance().BondHeadController.GetAxisZRealPos();

                    continue;
                }
            }
                
            if (this.currentCarrierConfig is CarrierWithWaferConfig)
            {
                System2RunTimeProvider.EjectStopwatch.Restart();

                WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();

                System2RunTimeProvider.EjectToReadyLiftTime.Add(System2RunTimeProvider.EjectStopwatch.ElapsedMilliseconds);
                LogHelper.Post(Level.Info, $" 取料流程顶针回预顶起位完成，顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos}", LogCategory.Component);
            }

            // 刷新Map信息
            if (this.SearchMode == SearchMode.WaferMap)
            {
                this.RemeberPickedDieInfo();
                this.RefreshPickedDieStatus();
                //this.ReMoveDieToWaferCameraCenter();
            }
            else if (this.SearchMode == SearchMode.Single)
            {
                //this.ReMoveDieToWaferCameraCenter();
            }
            else if (this.SearchMode == SearchMode.Box)
            {
                this.currentWafflePlateSlotState.CurrentIndex = this.currentWaffleSlot.Index + 1;
                this.AsyncSaveWaferSubDevicePara();

                this.RemeberWafflePickedDieInfo();
                this.RefreshWafflePickedDieStatus();
            }
            else if (this.SearchMode == SearchMode.Nine)
            {
                this.ReMoveDieToWaferCameraCenter();
            }

            LogHelper.Post(Level.Info, $"刷新操作结束", LogCategory.Component);
        }

        /// <summary>
        /// 获取取料位置
        /// </summary>
        /// <returns>result</returns>
        private double GetPickPosZ()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return 0;
            }

            if (this.currentCarrierConfig.CarrierType == CarrierTypeEnum.Wafer)
            {
                EjectionConfig ejectionConfig = EjectDevicePara.CurrentSlotConfig.EjectionConfig;

                double readyBondPickPosition =
                    System2Domain.GetInstance().BondModuleController.ConvertG0ToMachinePos(ejectionConfig.ReadyBondPickPosition).Z;

                double pickupDistance = this.currentCarrierConfig.PickupForceMode == ForceModeEnum.Distance ? this.currentCarrierConfig.PickupDistance : 0;

                // 计算最终取晶高度:顶针测高高度+顶针台工作位置-顶针台测高高度+吸嘴相对标准吸嘴补偿+芯片和蓝膜厚度+取料补偿
                double res = readyBondPickPosition + (((CarrierWithWaferConfig)this.currentCarrierConfig).EjectionTableWorkPosition
                                                      - ejectionConfig.EjectionTableMeasureHeightPosition).Z
                                                   + this.currentCarrierConfig.ComponentThickness
                                                   + this.currentCarrierConfig.CarrierThickness
                                                   + pickupDistance;

                return res;
            }
            else if (this.currentCarrierConfig.CarrierType == CarrierTypeEnum.Waffle)
            {
                // 获取华夫盒对象
                CarrierWithWaffleConfig carrierWithWaffleConfig =
                    (CarrierWithWaffleConfig)CarrierConfigRepository.GetInstance().Find(this.currentCarrierConfig.Name);

                // 力控模式不用这个硬补偿
                double pickUpDistance =
                    this.currentCarrierConfig.PickupForceMode == ForceModeEnum.Distance ? this.currentCarrierConfig.PickupDistance : 0;

                // 华夫盒示教高度转Bond坐标系
                double readyBondPickPosition =
                   System2Domain.GetInstance().BondModuleController.ConvertG0ToMachinePos(carrierWithWaffleConfig.ReadyBondPickPosition).Z;

                // 计算最终取晶高度=华夫盒示教高度+取料补偿+芯片厚度/*+吸嘴测高补偿*/
                // 华夫盒用吸嘴测高
                double pickPosZ = readyBondPickPosition + pickUpDistance
                                                      + carrierWithWaffleConfig.ComponentThickness
                    /* + nozzle.MeasureHeightOffset*/;

                return pickPosZ;
            }
            else
            {
                // 获取静态华夫盒对象
                CarrierWithStaticWaffleConfig carrierWithStaticWaffleConfig =
                    (CarrierWithStaticWaffleConfig)CarrierConfigRepository.GetInstance().Find(this.currentCarrierConfig.Name);

                // 力控模式不用这个硬补偿
                double pickUpDistance =
                    this.currentCarrierConfig.PickupForceMode == ForceModeEnum.Distance ? this.currentCarrierConfig.PickupDistance : 0;

                // 华夫盒示教高度转Bond坐标系
                double readyBondPickPosition =
                  System2Domain.GetInstance().BondModuleController.ConvertG0ToMachinePos(carrierWithStaticWaffleConfig.ReadyBondPickPosition).Z;

                // 计算最终取晶高度=华夫盒示教高度+取料补偿+芯片厚度+/*吸嘴测高补偿*/
                // 华夫盒用吸嘴测高

                double pickPosZ = readyBondPickPosition + pickUpDistance
                                                      + carrierWithStaticWaffleConfig.ComponentThickness
                                                     /* + nozzle.MeasureHeightOffset*/;
                return pickPosZ;
            }
        }

        /// <summary>
        /// 获取当前芯片中心的G0位置(包括空芯片)
        /// </summary>
        /// <returns>当前芯片中心的G0位置</returns>
        private AKRSPoint3D GetCurrentDieMarkCenterG0Pos()
        {
            if (this.isFirstSearch)
            {
                return this.WaferTableRealPositionToG0;
            }
            else
            {
                if (this.matchResult.CenterX == this.CameraCenterPixel.CenterX && this.matchResult.CenterY == this.CameraCenterPixel.CenterY)
                {
                    // 当未检测到芯片时，单颗搜晶需要以当前位置为起点进行下一颗偏移，这里直接传回晶圆台当前位置或者转换一下都可以，值相等
                    return this.GetDieMarkCenterG0Pos(this.matchResult);                    
                }

                WaferTablet wt = (WaferTablet)this.WaferTableDevicePara.CurrentTablet;
                return wt.CurrentPosition;                
            }
        }

        /// <summary>
        /// 让芯片重新回到晶圆相机中心
        /// </summary>
        private void ReMoveDieToWaferCameraCenter()
        {
            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.WaferTableRealPositionToG0 - this.GetPickOffset1());           
        }

        /// <summary>
        /// 获取芯片Mark中心G0位置
        /// </summary>
        /// <param name="matchResult">pr结果</param>
        /// <returns>芯片Mark中心G0位置</returns>
        public AKRSPoint3D GetDieMarkCenterG0Pos(MatchResult matchResult)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork/* || MachineStateModel.GetInstance().IsDryCycle*/)
            {
                return new AKRSPoint3D();
            }

            return MachineCoordinateSystem.GetInstance().WaferCameraCoordinateSystem.SelfPosToG0(new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0), this.WaferTableRealPosition);
        }

        private AKRSPoint3D dieMarkCenterG0Pos = new AKRSPoint3D();

        private AKRSPoint3D leftUpPoint = new AKRSPoint3D();

        private AKRSPoint3D rightDownPoint = new AKRSPoint3D();

        /// <summary>
        /// 让芯片去取料中心
        /// </summary>
        private void MoveDieToPickCenter()
        {
            this.countBlankNum = 0;
            this.bondSpinAngle = this.matchResult.Angle;
            if (this.IsTwoPointSearch)
            {
                this.dieMarkCenterG0Pos = (this.leftUpPoint + this.rightDownPoint) / 2;
            }
            else
            {
                this.dieMarkCenterG0Pos = this.GetDieMarkCenterG0Pos(this.matchResult);
            }

            if (this.SearchMode == SearchMode.Box)
            {
                if (this.currentCarrierConfig.CarrierType != CarrierTypeEnum.StaticWaffle)
                {
                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(dieMarkCenterG0Pos + this.GetPickOffset1(), false);
                }
            }
            else
            {
                if (SearchMode == SearchMode.WaferMap)
                {
                    AKRSPoint3D updatePos = dieMarkCenterG0Pos;

                    //if (this.isSymmetricModel)
                    //{
                    //    updatePos = this.WaferTableRealPositionToG0 + this.GetOffsetWorld(new AKRSPoint3D(this.matchResult.CenterX, this.matchResult.CenterY, 0));
                    //}                      
                }
                else if (SearchMode == SearchMode.Nine)
                {
                    // 芯片Mark中心对准相机中心
                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(dieMarkCenterG0Pos);
                    this.CalculateDiesPosition();
                    dieMarkCenterG0Pos = this.WaferTableRealPositionToG0;
                }

                // 设置起点记忆
                this.SetCurrentPosition(dieMarkCenterG0Pos);

                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(dieMarkCenterG0Pos + this.GetPickOffset1());
                //Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 获取取料补偿
        /// </summary>
        /// <returns>取料补偿</returns>
        public AKRSPoint3D GetPickOffset()
        {
            AKRSPoint3D offset = new AKRSPoint3D();
            if (this.SearchMode == SearchMode.Box)
            {
                offset = new AKRSPoint3D();
            }
            else
            {
                offset = this.EjectDevicePara.CurrentSlotConfig.EjectionConfig
                             .DeviationWithEjectionCenterAndWaferCameraCenter;
            }

            return offset;
        }

        /// <summary>
        /// 获取取料补偿
        /// </summary>
        /// <returns>取料补偿</returns>
        public AKRSPoint3D GetPickOffset1()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return new AKRSPoint3D();
            }

            AKRSPoint3D offset = new AKRSPoint3D();
            if (this.SearchMode == SearchMode.Box)
            {
                if (!this.currentCarrierConfig.IsTwoPointSearch)
                {
                    offset = this.currentCarrierConfig.RelativeDistanceMarkWithCenter
                       - this.currentCarrierConfig.CenterOffset;

                    //offset = new AKRSPoint3D();
                }
            }
            else
            {
                offset = this.currentCarrierConfig.RelativeDistanceMarkWithCenter - this.currentCarrierConfig.CenterOffset + this.EjectDevicePara.CurrentSlotConfig.EjectionConfig
                             .DeviationWithEjectionCenterAndWaferCameraCenter;

                //offset = new AKRSPoint3D() - this.currentCarrierConfig.CenterOffset + this.EjectDevicePara.CurrentSlotConfig.EjectionConfig
                //             .DeviationWithEjectionCenterAndWaferCameraCenter;
            }

            return offset;
        }

        /// <summary>
        /// 初始化map信息
        /// </summary>
        public void InitMapInfo()
        {
            if (this.SearchMode == SearchMode.WaferMap)
            {
                this.pickingRefDieInfo = null;
            }
        }

        /// <summary>
        /// 是否需要更换料片
        /// </summary>
        /// <returns>result</returns>
        public bool IsNeedChangeCarrier()
        {
            if (!this.isNeedChangeCarrier)
            {
                return false;
            }

            if (this.SearchMode == SearchMode.Single)
            {
                return true;
            }
            else if (this.SearchMode == SearchMode.Nine)
            {
                return true;
            }
            else if (this.SearchMode == SearchMode.WaferMap)
            {
                return this.pickingRefDieInfo == null || this.pickingRefDieInfo.Count == 0;
            }
            else if (this.SearchMode == SearchMode.Box)
            {
                return true;
            }
            else
            {
                throw new Exception("No exist the Search Mode!");
            }
        }

        /// <summary>
        /// 初始化搜晶
        /// </summary>
        public void StartInit()
        {
            this.isFirstSearch = true;
            this.countBlankNum = 0;
            this.isNeedAheadChangeTablet = false;

            this.StartInitWaferMap();
            this.StartInitWaffleMap();
            this.StartInitSingle();
            this.StartInitNine();
        }

        /// <summary>
        /// 加载map
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="isDownload">isDownload</param>
        public void LoadMap(string fileName, bool isDownload = false)
        {
            MainForm.SendUiAction(() =>
            {
                if (this.currentCarrierConfig is CarrierWithWaferConfig)
                {
                    string filePath =
                        System.IO.Path.Combine(WaferSubDevicePara.GetInstance().WaferTableDevicePara.WaferMapFilePath,
                            fileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    if (!System.IO.File.Exists(filePath) || isDownload)
                    {
                        string sourceFileName = System.IO.Path.Combine(this.WaferMapRepositoryDirPath,
                            this.currentCarrierConfig.Name);
                        string[] strArr = Directory.GetFiles(this.WaferMapRepositoryDirPath);
                        foreach (var a in strArr)
                        {
                            string str = Path.GetFileNameWithoutExtension(a);
                            if (str == this.currentCarrierConfig.Name)
                            {
                                sourceFileName = a;
                                filePath = Path.GetFileName(a);
                                break;
                            }
                        }

                        System.IO.File.Copy(sourceFileName, filePath, true);
                    }

                    this.WaferMapII.OpenFile(filePath);
                    //this.SetWaferMapDataConfig(this.WaferMapDataConfig);
                }
                else if (this.currentCarrierConfig is CarrierWithWaffleConfig)
                {
                    throw new Exception("The waffle no exist map");
                }
                else
                {
                    throw new Exception("The currentCarrier is null");
                }
            });
        }

        /// <summary>
        /// 显示map
        /// </summary>
        /// <param name="isWaffleMap">是否是华夫盒图</param>
        public void ShowMapping(bool isWaffleMap = false)
        {
            if (isWaffleMap)
            {
                this.StartInitWaffleMap();
                this.FrmWaffleMap.Show();
            }
            else
            {
                this.StartInitWaferMap();
                this.WaferMapII.Show();
            }
        }

        public void HideMapping()
        {
            this.WaferMapII.Hide();
        }

        /// <summary>
        /// 置位需要更换料片信号
        /// </summary>
        public void SetChangeCarrierSignal()
        {
            this.isNeedChangeCarrier = true;
        }

        /// <summary>
        /// 复位需要更换料片信号
        /// </summary>
        public void ReSetChangeCarrierSignal()
        {
            this.isNeedChangeCarrier = false;
            this.isNeedChangeCarrierMap = false;
        }

        /// <summary>
        /// 设置当前料片
        /// </summary>
        /// <param name="carrierConfig">carrier</param>
        public void SetCurrentCarrier(BaseCarrierConfig carrierConfig)
        {
            this.currentCarrierConfig = carrierConfig;
        }

        /// <summary>
        /// 设置当前华夫盘槽位状态
        /// </summary>
        /// <param name="wafflePlateSlotState">wafflePlateSlotState</param>
        public void SetCurrentWafflePlateSlotState(AdapterSlotEntity wafflePlateSlotState)
        {
            this.currentWafflePlateSlotState = wafflePlateSlotState;
            //Task.Run(() => this.FrmWaffleMap.LoadCarrierMapping(this.currentWafflePlateSlotState));

            //Action<AdapterSlotEntity> temp = this.FrmWaffleMap.LoadCarrierMapping;
            //temp.BeginInvoke(this.currentWafflePlateSlotState, null, "succeed");
            this.FrmWaffleMap.LoadCarrierMapping(this.currentWafflePlateSlotState);
        }

        /// <summary>
        /// 获取当前华夫盘槽位状态
        /// </summary>
        /// <returns>result</returns>
        public AdapterSlotEntity GetCurrentWafflePlateSlotState()
        {
            return this.currentWafflePlateSlotState;
        }

        /// <summary>
        /// 搜晶范围限制
        /// </summary>
        /// <param name="diePos">芯片位置</param>
        /// <returns>是否在范围内</returns>
        public bool IsInSearchRange(AKRSPoint3D diePos)
        {
            switch (this.CarrierShapeType)
            {
                case CarrierShapeTypeEnum.Circular:
                    return this.IsInCircleRange(diePos, this.WaferCenter, this.WaferRadius);

                case CarrierShapeTypeEnum.Rectangular:
                    return this.IsInRectRange(diePos, this.TopEdge, this.BottomEdge, this.LeftEdge, this.RightEdge);

                case CarrierShapeTypeEnum.CircularAndRectangular:
                default:
                    return this.IsInCircleRange(diePos, this.WaferCenter, this.WaferRadius) && this.IsInRectRange(diePos, this.TopEdge, this.BottomEdge, this.LeftEdge, this.RightEdge);
            }
        }

        /// <summary>
        /// 搜晶范围限制
        /// </summary>
        /// <param name="diePos">芯片位置</param>
        /// <returns>是否在范围内</returns>
        public bool IsInSearchRange(AKRSPoint3D diePos, CarrierWithWaferConfig waferCarrier) 
        {
            switch (waferCarrier.CarrierShapeType)
            {
                case CarrierShapeTypeEnum.Circular:
                    return this.IsInCircleRange(diePos, waferCarrier.WaferCenter, waferCarrier.WaferRadius + waferCarrier.WaferRadiusOffset);

                case CarrierShapeTypeEnum.Rectangular:
                    return this.IsInRectRange(diePos, waferCarrier.TopEdge, waferCarrier.BottomEdge, waferCarrier.LeftEdge, waferCarrier.RightEdge);

                case CarrierShapeTypeEnum.CircularAndRectangular:
                default:
                    return this.IsInCircleRange(diePos, waferCarrier.WaferCenter, waferCarrier.WaferRadius + waferCarrier.WaferRadiusOffset) && this.IsInRectRange(diePos, waferCarrier.TopEdge, waferCarrier.BottomEdge, waferCarrier.LeftEdge, waferCarrier.RightEdge);
            }
        }

        /// <summary>
        /// 获取bond取料角度
        /// </summary>
        /// <returns>result</returns>
        public double GetBondSpinAngle()
        {
            return this.bondSpinAngle + this.currentCarrierConfig.BondSpinAngleOffset;
        }

        /// <summary>
        /// 获取搜晶方式
        /// </summary>
        /// <returns>result</returns>
        public SearchMode GetSearchMode()
        {
            return this.SearchMode;
        }

        /// <summary>
        /// 多次校正直到达到要求的精度
        /// </summary>
        /// <returns>达到精度</returns>
        private (bool isSucceed, MatchResult matchResult) ToRequiredAccuracy()
        {
        RetryCommand:

            int repeatSearchNum = 1;
            do
            {
                // 拍照定位并获取相机结果
                (bool isSucceed, MatchResult[] matchResults) result = this.MatchResult();

                // 有晶片
                if (result.isSucceed)
                {
                    AKRSPoint3D[] offset = this.MatchResultToWorld(result.matchResults);

                    double angle = result.matchResults[0].Angle;

                    //LogHelper.Post(Level.Info, $"TarAccuracyX：{Math.Abs(offset[0].X)} + TarAccuracyY：{Math.Abs(offset[0].Y)} + TarAngle：{Math.Abs(angle)}", LogCategory.Global, ViewType.InUI);
                    LogHelper.Post(Level.Info, $"芯片目标精度X：{Math.Abs(offset[0].X)} + 芯片目标精度Y：{Math.Abs(offset[0].Y)} + 芯片目标精度Angle：{Math.Abs(angle)}", LogCategory.Global, ViewType.InUI);

                    if (Math.Abs(angle) <= this.TarAngle && Math.Abs(offset[0].X) <= this.TarAccuracy.X && Math.Abs(offset[0].Y) <= this.TarAccuracy.Y)
                    {
                        // 已经达到所要精度返回
                        return (true, result.matchResults[0]);
                    }
                    else
                    {
                        // 未达到就再校正
                        if (repeatSearchNum >= this.MaxLocationTimes)
                        {
                            // 次数已达到,但是未校正到要求精度
                            return (false, this.CameraCenterPixel);
                        }

                        // 移动到相机中心
                        this.MoveToCameraCenterWithThread(result.matchResults);
                    }
                }
                else
                {
                    //this.res = AKRSMessageBoxExt.Show(
                    //    $"Whether to retake the picture!\r\n Retry:Re-photograph positioning;\r\n OK: continue;\r\n Skip: skip;\r\n Abort:terminates thread;",
                    //    "Alarm",
                    //    new string[] { "Retry", "OK", "Skip", "Abort" },
                    //    new DialogResult[] { DialogResult.Retry, DialogResult.OK, DialogResult.Ignore, DialogResult.Abort, },
                    //    AlarmLevel.SecondLevel);
                    this.res = AKRSMessageBoxExt.Show(
                        $"未找到芯片，是否重新拍照检测！\r\n Retry:重新拍照定位;\r\n OK: 忽略定位结果，直接取片;\r\n Skip: 跳过当前芯片;\r\n Abort:停止;",
                        "Alarm",
                        new string[] { "Retry", "OK", "Skip", "Abort" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.OK, DialogResult.Ignore, DialogResult.Abort, },
                        AlarmLevel.SecondLevel);
                    switch (this.res)
                    {
                        case DialogResult.Retry:
                            // 重新执行检测流程
                            goto RetryCommand;

                        case DialogResult.OK:
                            // 忽略视觉定位结果，继续执行动作,将定位结果置为相机中心
                            return (true, result.matchResults[0]);

                        case DialogResult.Ignore:
                            // 跳过
                            return (false, this.CameraCenterPixel);

                        case DialogResult.Abort:
                            // 终止
                            //throw new Exception($"Take a picture and move to the center of the camera failed, the upper wafer thread terminated!");
                            throw new Exception($"拍照并定位到晶圆相机中心失败, 晶圆线程停止！");
                    }
                }

                // 多一次校正
                repeatSearchNum++;
            }
            while (repeatSearchNum <= this.MaxLocationTimes);

            return (false, this.CameraCenterPixel);
        }

        /// <summary>
        /// 获取晶圆相机内两点差值
        /// </summary>
        /// <param name="result">resultCamera</param>
        /// <returns>resultOffsetG0</returns>
        public AKRSPoint3D GetOffsetWorld(AKRSPoint3D result)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return result;
            }

            AKRSPoint3D point1 = MachineCoordinateSystem.GetInstance().WaferCameraCoordinateSystem.ForwardConvertCoordinate(result);
            AKRSPoint3D point2 = MachineCoordinateSystem.GetInstance().WaferCameraCoordinateSystem.ForwardConvertCoordinate(new AKRSPoint3D(this.CameraCenterPixel.CenterX, this.CameraCenterPixel.CenterY, 0));
            return point2 - point1;
        }

        /// <summary>
        /// 停止搜晶
        /// </summary>
        private void StopSearch(bool isNeedEjectZero = true)
        {
            if (Machine.GetInstance().IsStop())
            {
                if (isNeedEjectZero)
                {
                    WaferSubController.GetInstance().EjectController.OpenEjectionTableBlow();
                    Thread.Sleep(EjectDevicePara.DelayEjectBlowHold);
                    WaferSubController.GetInstance().EjectController.CloseEjectionTableBlow();
                    WaferSubController.GetInstance().EjectController.MoveEjectToSafePosition();
                }

                //throw new Exception("Machine is stop!");
                throw new Exception("正在搜晶过程中，检测到设备已停止！");
            }
        }

        /// <summary>
        /// 是否是圆形区域
        /// </summary>
        /// <param name="point">point</param>
        /// <param name="circleCentrePoint">circleCentrePoint</param>
        /// <param name="circleRadius">circleRadius</param>
        /// <returns>result</returns>
        private bool IsInCircleRange(AKRSPoint3D point, AKRSPoint3D circleCentrePoint, double circleRadius)
        {
            if (Math.Pow(point.X - circleCentrePoint.X, 2) + Math.Pow(point.Y - circleCentrePoint.Y, 2) <= Math.Pow(circleRadius, 2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否在矩形区域内
        /// </summary>
        /// <param name="point">point</param>
        /// <returns>result</returns>
        private bool IsInRectRange(AKRSPoint3D point, AKRSPoint3D topEdge, AKRSPoint3D bottomEdge, AKRSPoint3D leftEdge, AKRSPoint3D rightEdge)
        {
            if (point.X < leftEdge.X && point.X > rightEdge.X && point.Y < bottomEdge.Y && point.Y > topEdge.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 初始化晶圆图
        /// </summary>
        private void StartInitWaferMap()
        {
            MainForm.SendUiAction(
                () =>
                    {
                        if (this.WaferMapII == null)
                        {
                            this.WaferMapII = new WaferMapInterfaceImplementer();
                            this.WaferMapII.CreateForm("WaferMap", this.WaferMapRepositoryDirPath);
                            this.WaferMapII.Show();
                            this.WaferMapII.Hide();
                            // this.WaferMapII.Lock();      
                        }
                    });
        }

        /// <summary>
        /// 初始化华夫盒图
        /// </summary>
        private void StartInitWaffleMap()
        {
            MainForm.SendUiAction(
                () =>
                    {
                        if (this.FrmWaffleMap == null)
                        {
                            this.FrmWaffleMap = new FrmWaffleMap();
                            this.FrmWaffleMap.Show();
                            this.FrmWaffleMap.Hide();
                        }
                    });
        }

        /// <summary>
        /// 初始化单颗
        /// </summary>
        private void StartInitSingle()
        {
            
        }

        /// <summary>
        /// 初始化九点搜晶
        /// </summary>
        private void StartInitNine()
        {
            this.CurrentRowDirection = RowDirection.LeftToRight;
            this.currentRowNum = 0;
            this.turnUpRowNum = 0;
            this.tDie.DieInfo.Exist = false;
            this.BiggestRowStartDie.DieInfo.Exist = false;
            this.NextRowStartDie.DieInfo.Exist = false;
            this.nextDie.Exist = false;
            this.ResetNineDiesData();
        }

        /// <summary>
        /// 获取晶圆图路径
        /// </summary>
        /// <returns>result</returns>
        private string GetWaferMapRepositoryDirPath()
        {
            string strDirPath;
            string strConfigDirPath;
            strDirPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            strConfigDirPath = System.IO.Path.Combine(strDirPath, "Config1");
            return strConfigDirPath;
        }

        /// <summary>
        /// 获取当前载具
        /// </summary>
        /// <returns>result</returns>
        public BaseCarrierConfig GetCurrentCarrier()
        {
            return this.currentCarrierConfig;
        }

        /// <summary>
        /// 获取晶圆图检查结果
        /// </summary>
        /// <returns>result</returns>
        public bool GetCheckPreparedResult()
        {
            if (this.WaferMapII.CheckPrepared(out CheckPreparedResultDetail detail) == CheckPreparedResult.OK && !this.isNeedChangeCarrierMap)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region NOtUse

        /// <summary>
        /// 是否多次定位
        /// </summary>
        [JsonIgnore]
        private bool IsTimesLocation => this.currentCarrierConfig.IsTimesLocation;

        /// <summary>
        /// 最大定位次数
        /// </summary>
        [JsonIgnore]
        private int MaxLocationTimes => this.currentCarrierConfig.MaxLocationTimes;

        /// <summary>
        /// 目标角度
        /// </summary>
        [JsonIgnore]
        private double TarAngle => this.currentCarrierConfig.TarAngle;

        /// <summary>
        /// 目标精度
        /// </summary>
        [JsonIgnore]
        private AKRSPoint3D TarAccuracy => this.currentCarrierConfig.TarAccuracy;

        /// <summary>
        /// 是否取用参考点
        /// </summary>
        [JsonIgnore] 
        public bool IsWaferMapPickRefInEnd { get; set; } = false;

        #endregion

        #region 模组与模组参数

        /// <summary>
        /// WaferTableModule
        /// </summary>
        [JsonIgnore]
        private WaferTableModule WaferTableModule => WaferSubModule.GetInstance().WaferTable;

        /// <summary>
        /// WaferTableDevicePara
        /// </summary>
        [JsonIgnore]
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        /// <summary>
        /// EjectDevicePara
        /// </summary>
        [JsonIgnore]
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        #endregion

        /// <summary>
        /// SearchMode
        /// </summary>

        [JsonIgnore] 
        private SearchMode SearchMode => this.currentCarrierConfig.SearchMode;

        /// <summary>
        /// 是否是PositionSearch
        /// </summary>
        [JsonIgnore] 
        private bool IsPositionSearch => this.currentCarrierConfig.IsPositionSearch;

        /// <summary>
        /// PositionSearchMode
        /// </summary>
        [JsonIgnore] 
        private PositionSearchModeEnum PositionSearchMode => this.currentCarrierConfig.PositionSearchMode;

        /// <summary>
        /// IsTwoPointLocation
        /// </summary>
        [JsonIgnore] 
        private bool IsTwoPointSearch => this.currentCarrierConfig.IsTwoPointSearch;

        /// <summary>
        /// 定位模板名称
        /// </summary>
        [JsonIgnore] 
        private string DieMatchName => this.currentCarrierConfig.DieMatchName;

        /// <summary>
        /// 定位模板名称
        /// </summary>
        [JsonIgnore]
        private string DieMatchNameP2 => this.currentCarrierConfig.DieMatchNameP2;

        /// <summary>
        /// 定位框架模板名称
        /// </summary>
        [JsonIgnore]
        private string DieFrameName => this.currentCarrierConfig.DieFrameName;

        /// <summary>
        /// 芯片P1点相对中心距离
        /// </summary>
        [JsonIgnore]
        private AKRSPoint3D DistanceDieP1RelativeCenter => this.currentCarrierConfig.DistanceDieP1RelativeCenter;

        /// <summary>
        /// 芯片P2点相对中心距离
        /// </summary>
        [JsonIgnore]
        private AKRSPoint3D DistanceDieP2RelativeCenter => this.currentCarrierConfig.DistanceDieP2RelativeCenter;

        /// <summary>
        /// 墨点检测模板名称
        /// </summary>
        [JsonIgnore] 
        private string DieBlobName => this.currentCarrierConfig.DieBlobName;

        /// <summary>
        /// 是否执行墨点检测
        /// </summary>
        [JsonIgnore] 
        private bool IsInkDotSearch => this.currentCarrierConfig.IsInkDotSearch;

        /// <summary>
        /// 检测到墨点时是否自动跳过
        /// </summary>
        [JsonIgnore] 
        private bool IsBlobAutoSkip => this.currentCarrierConfig.NumberOfInkDot > 0;

        /// <summary>
        /// 空晶是否自动跳过
        /// </summary>
        [JsonIgnore] 
        private bool IsBlankDieAutoSkip => this.BlankDieAutoSkipMaxTimes > 0;

        /// <summary>
        /// 空晶最大跳过次数
        /// </summary>
        [JsonIgnore] 
        private int BlankDieAutoSkipMaxTimes => this.currentCarrierConfig.BlankDieAutoSkipMaxTimes;

        /// <summary>
        /// 载具行间距
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D CarrierRowSpacing => this.currentCarrierConfig.CarrierRowSpacing;

        /// <summary>
        /// 载具列间距
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D CarrierColSpacing => this.currentCarrierConfig.CarrierColSpacing;

        /// <summary>
        /// RaiseDistanceToRefreshMapAfterBondPicked
        /// </summary>
        [JsonIgnore]
        private double RaiseDistanceToRefreshMapAfterBondPicked => this.currentCarrierConfig.RaiseDistanceToRefreshMapAfterBondPicked;

        /// <summary>
        /// 晶圆中心坐标
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D WaferCenter => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.WaferCenter : new AKRSPoint3D();

        /// <summary>
        /// 晶圆半径
        /// </summary>
        [JsonIgnore] 
        private double WaferRadius => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.WaferRadius + waferCarrier.WaferRadiusOffset : new double();

        /// <summary>
        /// 载具形状
        /// </summary>
        [JsonIgnore] 
        private CarrierShapeTypeEnum CarrierShapeType => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.CarrierShapeType : CarrierShapeTypeEnum.Circular;

        /// <summary>
        /// 矩形上边位置
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D TopEdge => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.TopEdge : new AKRSPoint3D();

        /// <summary>
        /// 矩形下边位置
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D BottomEdge => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.BottomEdge : new AKRSPoint3D();

        /// <summary>
        /// 矩形左边位置
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D LeftEdge => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.LeftEdge : new AKRSPoint3D();

        /// <summary>
        /// 矩形右边位置
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D RightEdge => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.RightEdge : new AKRSPoint3D();

        /// <summary>
        /// 间隔行个数
        /// </summary>
        [JsonIgnore] 
        private double AfterNumberOfRows => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.AfterNumberOfRows : new double();

        /// <summary>
        /// 间隔列个数
        /// </summary>
        [JsonIgnore] 
        private double AfterNumberOfColumns => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.AfterNumberOfColumns : new double();

        /// <summary>
        /// 单颗搜晶方式的搜索方向
        /// </summary>
        [JsonIgnore] 
        private SingleSearchDirection SingleSearchDirection => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? ((WaferTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet).SingleSearchDirectionCurrent : SingleSearchDirection.ToDownRight;

        /// <summary>
        /// 九颗搜晶方式的搜索方向
        /// </summary>
        [JsonIgnore] 
        private NineSearchDirection NineSearchDirection => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.NineSearchDirection : NineSearchDirection.NineUpToDown;

        /// <summary>
        /// 晶圆图数据配置
        /// </summary>
        [JsonIgnore] 
        private WaferMapDataConfig WaferMapDataConfig => this.currentCarrierConfig is CarrierWithWaferConfig waferCarrier ? waferCarrier.WaferMapDataConfig : null;

        /// <summary>
        /// 获取晶圆台当前位置
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D WaferTableRealPosition => WaferSubController.GetInstance().WaferTableController.WaferTableMachinePos;

        /// <summary>
        /// 获取晶圆台当前位置
        /// </summary>
        [JsonIgnore] 
        private AKRSPoint3D WaferTableRealPositionToG0 => WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

        /// <summary>
        /// 相机拍照延时
        /// </summary>
        [JsonIgnore]
        private int DelayPhoto => WaferSubDevicePara.GetInstance().WaferTableDevicePara.DelayPhoto;

        /// <summary>
        /// 是否第一次搜晶
        /// </summary>
        [JsonIgnore] 
        private bool isFirstSearch = true;

        /// <summary>
        /// 标记Map图是否已经搜索完成
        /// </summary>
        [JsonIgnore] 
        private bool isNeedChangeCarrier;

        /// <summary>
        /// Bond旋转角度
        /// </summary>
        [JsonIgnore] 
        private double bondSpinAngle;

        /// <summary>
        /// 当前载具
        /// </summary>
        [JsonIgnore] 
        private BaseCarrierConfig currentCarrierConfig = new BaseCarrierConfig();

        /// <summary>
        /// 当前的华夫盒槽位状态
        /// </summary>
        [JsonIgnore] 
        private AdapterSlotEntity currentWafflePlateSlotState;

        /// <summary>
        /// 相机定位结果
        /// </summary>
        private MatchResult matchResult = new MatchResult();

        /// <summary>
        /// 相机中心
        /// </summary>
        private MatchResult CameraCenterPixel => new MatchResult(1224, 1024, 0);

        /// <summary>
        /// 晶圆图路径
        /// </summary>
        [JsonIgnore] 
        private string WaferMapRepositoryDirPath => this.GetWaferMapRepositoryDirPath();

        /// <summary>
        /// res
        /// </summary>
        [JsonIgnore] 
        private DialogResult res;
    }

    /// <summary>
    /// BaseMap
    /// </summary>
    public class BaseMap
    {
        /// <summary>
        /// MapID
        /// </summary>
        public string MapID { get; set; } = string.Empty;
    }

    /// <summary>
    /// WaferMap
    /// </summary>
    public class WaferMap : BaseMap
    {
        /// <summary>
        /// ReferencePoint
        /// </summary>
        public List<AKRSPoint3D> ReferencePoint { get; set; } = new List<AKRSPoint3D>();
    }

    /// <summary>
    /// WaffleMap
    /// </summary>
    public class WaffleMap : BaseMap
    {
        
    }
}
