using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using AKRS.ZX2200.WaferSubSystem.Services;
using DevExpress.XtraEditors;
using log4net.Core;
using System;
using System.Threading;
using System.Windows.Forms;


namespace AKRS.ZX2200.WaferSubSystem.Tasks
{
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.PR.Models.Algs;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.EventBus;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Controls;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Carrier;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using DevExpress.Utils.Extensions;
    using Newtonsoft.Json;
    using OfficeOpenXml;
    //using System.ComponentModel;
    //using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

    /// <summary>
    /// 晶圆上料子系统线程
    /// </summary>
    public class WaferSubSystemTask
    {
        /// <summary>
        /// 线程方法
        /// </summary>
        private void Dowork()
        {
            try
            {
                WaitBondNeed:
                while (Signal.WaitStart())
                {
                    // 单独空跑
                    if (this.IsAloneDryCycle)
                    {
                        Thread.Sleep(WaferSubDevicePara.GetInstance().WaferTableDevicePara.DelaySearchDieContinue);
                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();
                    }

                    Static.RecordTime("取片信号交互", $"等待Bond要料中");

                    if (!SignalPool.GetInstance().IsBondNeedChipSignal.Wait())
                    {
                        goto WaitBondNeed;
                    }

                    Static.RecordTime("取片信号交互", $"晶圆台已接受到Bond要料信号");

                    // 单独空跑
                    if (this.IsAloneDryCycle)
                    {
                        //if (this.times > 60 && this.times <= 80)
                        //{
                        //    this.CurrentNeedChipName = "wt-1";
                        //}
                        //else if (this.times > 40 && this.times <= 60)
                        //{
                        //    this.CurrentNeedChipName = "wt-1";
                        //}
                        //else if (this.times > 20 && this.times <= 40)
                        //{
                        //    this.CurrentNeedChipName = "wt-1";
                        //}
                        //else if (this.times <= 20)
                        //{
                        //    this.CurrentNeedChipName = "wt-1";
                        //}
                        //else if (this.times > 80)
                        //{
                        //    this.times = 0;
                        //    this.CurrentNeedChipName = "wt-1";
                        //}
                    }

                    Static.RecordTime("取片信号交互", $"Bond需要的芯片名称- {this.CurrentNeedChipName}");

                    if (WaferSystemProgram.GetInstance().GetCarriers().Exists(a => a.Name == this.CurrentNeedChipName))
                    {
                        this.currentNeedCarrierConfig =
                            (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(this.CurrentNeedChipName);
                    }
                    else
                    {
                        //throw new Exception($"The required pieces do not exist, die name is -{this.CurrentNeedChipName}");
                        throw new Exception($"Bond需要的料片不存在, 芯片名称是 -{this.CurrentNeedChipName}");
                    }

                    if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is NullTablet
                        || this.CurrentNeedChipName != this.PreviousNeedChipName)
                    {
                        Block.GetInstance().SetChangeCarrierSignal();
                    }

                    NeedChange:
                    // 如果是换料片，则需要循环判断整个magazine的芯片类型
                    if (this.IsNeedChange)
                    {
                        AlarmLogEntity alarm = new AlarmLogEntity();
                        alarm.StartTime = DateTime.Now;
                        alarm.Message = "请更换料片";

                        if (!this.ChangeTabletAuto())
                        {
                            alarm.HandleTime = DateTime.Now;
                            //alarm.HandleType = dr.ToString();
                            //alarm.AlarmCode = alarmCode;
                            alarm.Category = "更换料片";
                            DBService.Insert(alarm);

                            //throw new Exception("change sheet failed!");
                            throw new Exception("更换料片失败！");
                        }

                        alarm.HandleTime = DateTime.Now;
                        //alarm.HandleType = dr.ToString();
                        //alarm.AlarmCode = alarmCode;
                        alarm.Category = "更换料片";
                        DBService.Insert(alarm);
                    }

                    Static.RecordTime("晶圆线程", $"准备开始搜晶，芯片名称- {this.CurrentNeedChipName}");

                    // 搜晶
                    if (!Block.GetInstance().NextDie())
                    {
                        // 搜晶结果不一定都是搜到的，如果是换料片，则需要循环判断整个magazine的芯片类型
                        if (this.IsNeedChange)
                        {
                            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet wt)
                            {
                                if (wt.CarrierConfigWithWafer.SearchMode == SearchMode.WaferMap)
                                {
                                    goto NeedChange;
                                }
                                else
                                {
                                    // 旧版
                                    //this.res = AKRSMessageBoxExt.Show($"找不到芯片并且晶圆台位置超出限制 , 点击 \r\n" + "OK：继续\r\n" + "Retry：请先移动晶圆台位置，再点击按钮重新搜晶！", "Prompt", new string[] { "OK", "Retry" }, new DialogResult[] { DialogResult.OK, DialogResult.Retry });
                                    //switch (this.res)
                                    //{
                                    //    case DialogResult.OK:
                                    //        goto NeedChange;

                                    //    case DialogResult.Retry:
                                    //        Block.GetInstance().ReSetChangeCarrierSignal();
                                    //        WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet.SlotState = SlotStatuEnum.Good;
                                    //        Block.GetInstance().AsyncSaveWaferSubDevicePara();
                                    //        goto NeedChange;
                                    //}

                                    if (MachineStateModel.GetInstance().IsDryCycle)
                                    {
                                        // 如果是空跑直接去自动换料
                                        goto NeedChange;
                                    }

                                    // 新版
                                    RetrySelectPoint:

                                    PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance()
                                        .Find(wt.CarrierConfigWithWafer.DieMatchName);

                                    string message;
                                    if (Block.GetInstance().GetSearchMode() == SearchMode.Single)
                                    {
                                        message =
                                            $"晶圆视觉模板：芯片模板{prEntity.GetName()}定位失败，当前方向为{Block.GetInstance().GetSingleSearchDirection()}";
                                    }
                                    else
                                    {
                                        message = $"晶圆视觉模板：芯片模板{prEntity.GetName()}定位失败";
                                    }

                                    message += $"\r\n找不到芯片并且晶圆台位置超出限制 , 点击 \r\n" + "OK：继续\r\n"
                                               + "Retry：请先移动晶圆台位置，再点击按钮重新搜晶！";

                                    (DialogResult dialog, BaseAlgResult matchResult) result1 =
                                        UcMainSystem.VisionAlarmFunc(prEntity, message, "晶圆线程");

                                    switch (result1.dialog)
                                    {
                                        case DialogResult.OK:
                                            goto NeedChange;
                                        case DialogResult.Retry:
                                            if (Block.GetInstance().GetSearchMode() != SearchMode.Box
                                                & !Block.GetInstance().IsInSearchRange(
                                                    WaferSubController.GetInstance().WaferTableController
                                                        .WaferTableG0Pos))
                                            {
                                                // 如果选取的位置在搜索范围外，则重新选择点位
                                                AKRSMessageBoxExt.Show(
                                                    $"选取的位置在搜索范围外，则重新选择点位!",
                                                    "Prompt",
                                                    new string[] { "OK" },
                                                    new DialogResult[] { DialogResult.OK });
                                                goto RetrySelectPoint;
                                            }

                                            Block.GetInstance().ReSetChangeCarrierSignal();
                                            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet
                                                .SlotState = SlotStatuEnum.Good;
                                            Block.GetInstance().AsyncSaveWaferSubDevicePara();
                                            goto NeedChange;
                                    }
                                }
                            }
                            else
                            {
                                goto NeedChange;
                            }
                        }

                        //throw new Exception("Search failed!");
                        throw new Exception("搜晶失败！");
                    }

                    // 记录上一次的芯片名称
                    this.PreviousNeedChipName = this.CurrentNeedChipName;

                    // 开真空
                    if (currentNeedCarrierConfig.IsOpenWaferTableVacuumBeforePick)
                    {
                        switch (currentNeedCarrierConfig.CarrierType)
                        {
                            case CarrierTypeEnum.Wafer:
                                WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();
                                break;
                                //case CarrierTypeEnum.StaticWaffle:
                                //    WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
                                break;
                            case CarrierTypeEnum.Waffle:
                                WaferSubController.GetInstance().WaferTableController.OpenWaffleVacuum();
                                break;
                        }

                        // 开真空延时
                        Thread.Sleep(currentNeedCarrierConfig.OpenWaferTableVacuumBeforePickDelay);
                    }
                    else
                    {
                        switch (currentNeedCarrierConfig.CarrierType)
                        {
                            case CarrierTypeEnum.Wafer:
                                WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                                break;
                            //case CarrierTypeEnum.StaticWaffle:
                            //    WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
                            //    break;
                            case CarrierTypeEnum.Waffle:
                                WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                                break;
                        }
                    }

                    // 发送可取料信号
                    if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.Wafer)
                    {
                        if (!this.IsAloneDryCycle)
                        {
                            //WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();
                        }
                    }
                    else
                    {
                        // 单独空跑
                        if (this.IsAloneDryCycle)
                        {
                            if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                            {
                                if (!MachineStateModel.GetInstance().IsOffLineWork)
                                {
                                    AKRSPoint3D result = Block.GetInstance().GetResultDieG0Pos();
                                    result = System2Domain.GetInstance().BondModuleController
                                        .ConvertG0ToMachinePos(result);

                                    System2Domain.GetInstance().BondHeadController.MoveZAxis(result.Z);
                                    System2Domain.GetInstance().BondModuleController.MoveBondXY(result.X, result.Y);
                                    Thread.Sleep(10);
                                }
                            }
                        }
                    }

                    SignalPool.GetInstance().IsBondNeedChipSignal.ReSet();

                    SignalPool.GetInstance().IsWaferAllowPickSignal.Set();
                    Static.RecordTime("取片信号交互", $"晶圆台已发送允许取料信号");


                    LogHelper.Post(Level.Info, $"已发送允许取料信号", LogCategory.Component);

                    // 单独空跑
                    if (this.IsAloneDryCycle)
                    {
                        Block.GetInstance().RefreshMap(true);
                        this.times++;
                    }

                    // 记录数据
                    if (IsRecordSearchData & IsAloneDryCycle)
                    {
                        (bool isSucceed, MatchResult[] matchResults) result =
                            Block.GetInstance().MatchResult("芯片位置检测pr");
                        if (result.isSucceed)
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            ExcelPackage package = new ExcelPackage(new FileInfo($"D:\\连续搜索实验.xlsx"));
                            ExcelWorksheet worksheet;
                            if (package.Workbook.Worksheets.Exists(a => a.Name == "sheet"))
                            {
                                worksheet = package.Workbook.Worksheets.ToList().Find(a => a.Name == "sheet");
                            }
                            else
                            {
                                worksheet = package.Workbook.Worksheets.Add("sheet");
                            }

                            worksheet.Cells[1, 1].Value = "VisionPos_X";
                            worksheet.Cells[1, 2].Value = "VisionPos_Y";
                            worksheet.Cells[1, 3].Value = "time";

                            int rowNum = worksheet.Rows.Count();

                            worksheet.Cells[rowNum + 1, 1].Value = result.matchResults[0].CenterX;
                            worksheet.Cells[rowNum + 1, 2].Value = result.matchResults[0].CenterY;
                            worksheet.Cells[rowNum + 1, 3].Value = DateTime.Now.ToString();
                            package.Save();
                        }
                    }

                    if (IsSearchOne)
                    {
                        Machine.GetInstance().Stop();
                    }
                }
            }
            catch (Exception exception)
            {
                //AKRSMessageBoxExt.Show(exception.Message + "\r\n" + "The wafer thread is about to stop!", "Error", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                AKRSMessageBoxExt.Show(
                    exception.Message + "\r\n" + "晶圆线程即将停止！",
                    "Error",
                    new string[] { "OK" },
                    new DialogResult[] { DialogResult.OK });
                Machine.GetInstance().Stop();
                //this.waferSubSystemWorkThread.Abort();
            }
        }

        ///// <summary>
        ///// 判断所需芯片在哪一层
        ///// </summary>
        ///// <returns>layerIndex</returns>
        //private int JudgeNeedChipAtLayerIndex()
        //{
        //    for (int i = 0; i < WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.MaxUseLayerCount; i++)
        //    {
        //        if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].SlotState == SlotStatuEnum.Good)
        //        {
        //            if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i] is WaferTablet)
        //            {
        //                WaferTablet wt = (WaferTablet)WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i];
        //                if (CarrierConfigRepository.GetInstance().IsExists(wt.Name))
        //                {
        //                    if (wt.Name == this.CurrentNeedChipName)
        //                    {
        //                        return i;
        //                    }
        //                }
        //            }
        //            else if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i] is AdapterTablet)
        //            {
        //                AdapterTablet ad = (AdapterTablet)WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i];
        //                if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
        //                {
        //                    for (int j = 0; j < ad.AdapterSetting.MaxUseWaffleCount; j++)
        //                    {
        //                        if (CarrierConfigRepository.GetInstance().IsExists(ad.AdapterSetting.WaffleArray[j].Name))
        //                        {
        //                            if (ad.AdapterSetting.WaffleArray[j].Name == this.CurrentNeedChipName && ad.AdapterState.WafflePlateSlotsState[j].SlotState == SlotStatuEnum.Good)
        //                            {
        //                                return i;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return -1;
        //}

        ///// <summary>
        ///// 判断所需芯片在适配器哪一盒
        ///// </summary>
        ///// <returns>slotIndex</returns>
        //private int JudgeNeedChipAtAdapterSlotIndex()
        //{
        //    if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
        //    {
        //        if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet is AdapterTablet)
        //        {
        //            AdapterTablet ad = (AdapterTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet;
        //            if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
        //            {
        //                for (int i = 0; i < ad.AdapterSetting.MaxUseWaffleCount; i++)
        //                {
        //                    if (CarrierConfigRepository.GetInstance()
        //                        .IsExists(ad.AdapterSetting.WaffleArray[i].Name))
        //                    {
        //                        if (ad.AdapterSetting.WaffleArray[i].Name == this.CurrentNeedChipName && ad.AdapterState.WafflePlateSlotsState[i].SlotState == SlotStatuEnum.Good)
        //                        {
        //                            return i;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet)
        //        {
        //            AdapterTablet ad = (AdapterTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet;
        //            if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
        //            {
        //                for (int i = 0; i < ad.AdapterSetting.MaxUseWaffleCount; i++)
        //                {
        //                    if (CarrierConfigRepository.GetInstance()
        //                        .IsExists(ad.AdapterSetting.WaffleArray[i].Name))
        //                    {
        //                        if (ad.AdapterSetting.WaffleArray[i].Name == this.CurrentNeedChipName && ad.AdapterState.WafflePlateSlotsState[i].SlotState == SlotStatuEnum.Good)
        //                        {
        //                            return i;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return -1;
        //}

        /// <summary>
        /// 更换料片成功后的操作
        /// </summary>
        /// <returns>是否完成</returns>
        private bool ChangeSucceedOperation()
        {
            Block.GetInstance().StartInit();

            if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet is AdapterTablet ad)
                {
                    if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
                    {
                        for (int i = 0; i < ad.AdapterSetting.MaxUseWaffleCount; i++)
                        {
                            if (CarrierConfigRepository.GetInstance()
                                .IsExists(ad.AdapterSetting.WaffleArray[i].Name))
                            {
                                if (ad.AdapterSetting.WaffleArray[i].Name == this.CurrentNeedChipName && ad.AdapterState.WafflePlateSlotsState[i].SlotState == SlotStatuEnum.Good)
                                {
                                    // 切换模板名称及行列间距
                                    Block.GetInstance().SetCurrentCarrier(ad.AdapterSetting.WaffleArray[i].CarrierWithWaffleConfig);
                                    Block.GetInstance().SetCurrentWafflePlateSlotState(ad.AdapterState.WafflePlateSlotsState[i]);
                                    Block.GetInstance().ReSetChangeCarrierSignal();
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet wt)
                {
                    if (CarrierConfigRepository.GetInstance().IsExists(wt.Name))
                    {
                        if (wt.Name == this.CurrentNeedChipName && wt.SlotState == SlotStatuEnum.Good)
                        {
                            // 切换模板名称及行列间距
                            WaferSubController.GetInstance().WaferTableController.MoveWaferCameraToWorkPosition(wt.CarrierConfigWithWafer);
                            Block.GetInstance().SetCurrentCarrier(wt.CarrierConfigWithWafer);

                            // 换顶针
                            WaferSubController.GetInstance().EjectController.ChangeEjection();

                            if (wt.CarrierConfigWithWafer.AutoRunWithDieMode == AutoRunWithDieModeEnum.Automatic)
                            {
                                if (Block.GetInstance().GetSearchMode() == SearchMode.WaferMap)
                                {
                                    //throw new Exception("you can you no, SearchMode!");

                                    if (!Block.GetInstance().GetCheckPreparedResult())
                                    {
                                        Block.GetInstance().LoadMap(wt.CarrierConfigWithWafer.Name, true);
                                        WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(wt.CarrierConfigWithWafer.ReferencePosition);

                                        int i = 0;
                                        AKRSPoint3D centerPos = new AKRSPoint3D(wt.CarrierConfigWithWafer.ReferencePosition.X, wt.CarrierConfigWithWafer.ReferencePosition.Y, 0);
                                        AKRSPoint3D tempPos = new AKRSPoint3D(centerPos.X, centerPos.Y, centerPos.Z);

                                    Retry:
                                        i++;
                                        (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(wt.CarrierConfigWithWafer.DieMatchName);
                                        if (result.isSucceed)
                                        {
                                            if (wt.CarrierConfigWithWafer.IsAroundLocate)
                                            {
                                                centerPos = tempPos;
                                            }

                                            wt.CarrierConfigWithWafer.ReferencePosition = centerPos;
                                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(wt.CarrierConfigWithWafer.ReferencePosition);
                                            CarrierConfigRepository.GetInstance().Save();

                                            Block.GetInstance().SetReferencePoint1();
                                            Block.GetInstance().FindReferencePoint();
                                            Block.GetInstance().MoveToStartPoint();
                                            Block.GetInstance().StartAsyncSeekingPath();
                                            Block.GetInstance().InitMapInfo();
                                            Block.GetInstance().ReSetChangeCarrierSignal();

                                            if (wt.CarrierConfigWithWafer.IsConfirmStartPointWithManual)
                                            {
                                                this.res = AKRSMessageBoxExt.Show($"请人工确认晶圆起点是否正确！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                                                switch (this.res)
                                                {
                                                    case DialogResult.OK:
                                                        break;

                                                    case DialogResult.Abort:
                                                        throw new Exception("更换晶圆料片后进行一系列操作失败！");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (wt.CarrierConfigWithWafer.IsAroundLocate)
                                            {
                                                tempPos = new AKRSPoint3D(centerPos.X, centerPos.Y, centerPos.Z);
                                                if (i == 1)
                                                {
                                                    tempPos.Y -= wt.CarrierConfigWithWafer.AroundLocateDistance;
                                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(tempPos);
                                                }
                                                else if (i == 2)
                                                {
                                                    tempPos.Y += wt.CarrierConfigWithWafer.AroundLocateDistance;
                                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(tempPos);
                                                }
                                                else if (i == 3)
                                                {
                                                    tempPos.X -= wt.CarrierConfigWithWafer.AroundLocateDistance;
                                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(tempPos);
                                                }
                                                else if (i == 4)
                                                {
                                                    tempPos.X += wt.CarrierConfigWithWafer.AroundLocateDistance;
                                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(tempPos);
                                                }
                                                else
                                                {
                                                    i = 0;
                                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(centerPos);

                                                    DialogResult res = DialogResult.Abort;
                                                    MainForm.SendUiAction(
                                                        () =>
                                                            {
                                                                FrmChangePositionTeach temp = new FrmChangePositionTeach(wt.CarrierConfigWithWafer.DieMatchName);
                                                                temp.ShowDialog();
                                                                temp.Dispose();
                                                                res = temp.DialogResult;
                                                            });

                                                    if (res == DialogResult.OK)
                                                    {
                                                        centerPos = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                                                        tempPos = new AKRSPoint3D(centerPos.X, centerPos.Y, centerPos.Z);
                                                    }
                                                    else if (res == DialogResult.Abort)
                                                    {
                                                        //throw new Exception("Change Succeed Operation failed");
                                                        throw new Exception("更换晶圆料片后进行一系列操作失败！");
                                                    }
                                                }

                                                goto Retry;
                                            }
                                            else
                                            {
                                                DialogResult res = DialogResult.Abort;
                                                MainForm.SendUiAction(
                                                    () =>
                                                        {
                                                            FrmChangePositionTeach temp = new FrmChangePositionTeach(wt.CarrierConfigWithWafer.DieMatchName);
                                                            temp.ShowDialog();
                                                            temp.Dispose();
                                                            res = temp.DialogResult;
                                                        });

                                                if (res == DialogResult.OK)
                                                {
                                                    centerPos = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                                                }
                                                else if (res == DialogResult.Abort)
                                                {
                                                    //throw new Exception("Change Succeed Operation failed");
                                                    throw new Exception("更换晶圆料片后进行一系列操作失败！");
                                                }

                                                goto Retry;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Block.GetInstance().ReSetChangeCarrierSignal();

                                        wt.IsNewTablet = false;
                                        WaferSubDevicePara.GetInstance().Save();
                                    }

                                    //// TODO 自动更换晶圆图、确认位置
                                    //string fileName = wt.Name + "-" + $"{WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo}";
                                    //Static.CurrentWaferMapName = fileName;
                                    //if (!SignalPool.GetInstance().IsLoadWaferMap.Wait())
                                    //{
                                    //    throw new Exception("Wait load wafer map failed!");
                                    //}

                                    //Block.GetInstance().SetReferencePoint1();
                                    //Block.GetInstance().FindReferencePoint();
                                    //Block.GetInstance().MoveToStartPoint();
                                    //Block.GetInstance().StartAsyncSeekingPath();
                                    //Block.GetInstance().InitMapInfo();
                                    //Block.GetInstance().ReSetChangeCarrierSignal();

                                    //WaferSubController.GetInstance().WaferTableController.MoveToReferencePoint1();
                                    //Block.GetInstance().SetCurrentPoint(wt.CurrentPoint);
                                    //(bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(wt.CarrierConfigWithWafer.ReferenceName);
                                    //if (result.isSucceed)
                                    //{
                                    //    // 直接去上次结束的位置进行搜晶
                                    //    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(wt.CurrentPosition);
                                    //}
                                    //else
                                    //{
                                    //    // 进行人工干预，设置第一颗的位置（此过程需要将当前是几行几列也写入），直接进行搜晶
                                    //    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(wt.CurrentPosition);
                                    //Retry:
                                    //    FrmChangePositionTeach temp = new FrmChangePositionTeach(wt.CarrierConfigWithWafer.ReferenceName);
                                    //    temp.ShowDialog();
                                    //    if (this.IsNeedChange)
                                    //    {
                                    //        if (temp.DialogResult == DialogResult.Abort)
                                    //        {
                                    //            throw new Exception("Change Succeed Operation failed");
                                    //        }

                                    //        goto Retry;
                                    //    }
                                    //}
                                }
                                else if (Block.GetInstance().GetSearchMode() == SearchMode.Single)
                                {
                                    if (!Block.GetInstance().IsInSearchRange(wt.CurrentPosition))
                                    {
                                        wt.InitTabletInfo();
                                        WaferSubDevicePara.GetInstance().Save();
                                    }

                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(wt.CurrentPosition, true);

                                    if (MachineStateModel.GetInstance().IsDryCycle)
                                    {
                                        Block.GetInstance().ReSetChangeCarrierSignal();
                                    }
                                    else
                                    {
                                        // 由于需要先屏蔽掉-防止弹出提示框
                                        // (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(wt.CarrierConfigWithWafer.DieMatchName);
                                        if (wt.IsNewTablet/* || !result.isSucceed*/)
                                        {
                                        Retry:
                                            DialogResult res = DialogResult.Abort;
                                            MainForm.SendUiAction(
                                                () =>
                                                {
                                                    FrmChangePositionTeach temp = new FrmChangePositionTeach(wt.CarrierConfigWithWafer.DieMatchName);
                                                    temp.ShowDialog();
                                                    temp.Dispose();
                                                    res = temp.DialogResult;
                                                });

                                            if (this.IsNeedChange)
                                            {
                                                if (res == DialogResult.Abort)
                                                {
                                                    //throw new Exception("Change Succeed Operation failed");
                                                    throw new Exception("更换晶圆料片后进行一系列操作失败！");
                                                }

                                                goto Retry;
                                            }
                                            else
                                            {
                                                wt.IsNewTablet = false;
                                                WaferSubDevicePara.GetInstance().Save();
                                            }
                                        }
                                        else
                                        {
                                            Block.GetInstance().ReSetChangeCarrierSignal();
                                        }
                                    }                                       
                                }
                                else if (Block.GetInstance().GetSearchMode() == SearchMode.Nine)
                                {
                                    if (!Block.GetInstance().IsInSearchRange(wt.CurrentPosition))
                                    {
                                        wt.InitTabletInfo();
                                        WaferSubDevicePara.GetInstance().Save();
                                    }

                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(wt.CurrentPosition, true);

                                    (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(wt.CarrierConfigWithWafer.DieMatchName);
                                    if (wt.IsNewTablet || !result.isSucceed)
                                    {
                                        Retry:
                                        DialogResult res = DialogResult.Abort;
                                        MainForm.SendUiAction(
                                            () =>
                                                {
                                                    FrmChangePositionTeach temp = new FrmChangePositionTeach(wt.CarrierConfigWithWafer.DieMatchName);
                                                    temp.ShowDialog();
                                                    temp.Dispose();
                                                    res = temp.DialogResult;
                                                });
                                        
                                        if (this.IsNeedChange)
                                        {
                                            if (res == DialogResult.Abort)
                                            {
                                                //throw new Exception("Change Succeed Operation failed");
                                                throw new Exception("更换晶圆料片后进行一系列操作失败！");
                                            }

                                            goto Retry;
                                        }
                                        else
                                        {
                                            wt.IsNewTablet = false;
                                            WaferSubDevicePara.GetInstance().Save();
                                        }
                                    }
                                    else
                                    {
                                        Block.GetInstance().ReSetChangeCarrierSignal();
                                    }
                                }
                                else
                                {
                                    throw new Exception("you can you no, SearchMode!");
                                }
                            }
                            else if (wt.CarrierConfigWithWafer.AutoRunWithDieMode == AutoRunWithDieModeEnum.Manual)
                            {
                                if (Block.GetInstance().GetSearchMode() == SearchMode.WaferMap)
                                {
                                    if (!Block.GetInstance().IsInSearchRange(wt.CurrentPosition))
                                    {
                                        wt.InitTabletInfo();
                                        WaferSubDevicePara.GetInstance().Save();
                                    }

                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(wt.CurrentPosition, true);

                                    if (!Block.GetInstance().GetCheckPreparedResult())
                                    {
                                    // 换图
                                    ChangeMap:
                                    DialogResult res = ControlService.ShowDialogForm<FrmChangeWaferMapTeach>();
                                        if (this.IsNeedChange)
                                        {
                                            if (res == DialogResult.Abort)
                                            {
                                                //throw new Exception("Change WaferMap failed");
                                                throw new Exception("更换晶圆图失败！");
                                            }

                                            goto ChangeMap;
                                        }
                                        else
                                        {
                                            wt.IsNewTablet = false;
                                            WaferSubDevicePara.GetInstance().Save();
                                        }
                                    }
                                    else
                                    {
                                        //Block.GetInstance().InitMapInfo();
                                        Block.GetInstance().ReSetChangeCarrierSignal();

                                        wt.IsNewTablet = false;
                                        WaferSubDevicePara.GetInstance().Save();
                                    }
                                }
                                else if (Block.GetInstance().GetSearchMode() == SearchMode.Single)
                                {
                                    if (!Block.GetInstance().IsInSearchRange(wt.CurrentPosition))
                                    {
                                        wt.InitTabletInfo();
                                        WaferSubDevicePara.GetInstance().Save();
                                    }

                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(wt.CurrentPosition, true);

                                    Retry:
                                    DialogResult res = DialogResult.Abort;

                                    res = ControlService.ShowDialogForm<FrmChangePositionTeach>(wt.CarrierConfigWithWafer.DieMatchName);

                                    //PlatformProvider.Current.OnUIThread(
                                    //    () =>
                                    //        {
                                    //            FrmChangePositionTeach temp = new FrmChangePositionTeach(wt.CarrierConfigWithWafer.DieMatchName);
                                    //            temp.ShowDialog();
                                    //            temp.Dispose();
                                    //            res = temp.DialogResult;
                                    //        });

                                    if (this.IsNeedChange)
                                    {
                                        if (res == DialogResult.Abort)
                                        {
                                            //throw new Exception("Change Succeed Operation failed");
                                            throw new Exception("更换晶圆料片后进行一系列操作失败！");
                                        }

                                        goto Retry;
                                    }
                                    else
                                    {
                                        wt.IsNewTablet = false;
                                        WaferSubDevicePara.GetInstance().Save();
                                    }
                                }
                                else if (Block.GetInstance().GetSearchMode() == SearchMode.Nine)
                                {
                                    if (!Block.GetInstance().IsInSearchRange(wt.CurrentPosition))
                                    {
                                        wt.InitTabletInfo();
                                        WaferSubDevicePara.GetInstance().Save();
                                    }

                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(wt.CurrentPosition, true);

                                    Retry:
                                    DialogResult res = DialogResult.Abort;
                                    MainForm.SendUiAction(
                                        () =>
                                            {
                                                FrmChangePositionTeach temp = new FrmChangePositionTeach(wt.CarrierConfigWithWafer.DieMatchName);
                                                temp.ShowDialog();
                                                temp.Dispose();
                                                res = temp.DialogResult;
                                            });
                                    
                                    if (this.IsNeedChange)
                                    {
                                        if (res == DialogResult.Abort)
                                        {
                                            //throw new Exception("Change Succeed Operation failed");
                                            throw new Exception("更换晶圆料片后进行一系列操作失败！");
                                        }

                                        goto Retry;
                                    }
                                    else
                                    {
                                        wt.IsNewTablet = false;
                                        WaferSubDevicePara.GetInstance().Save();
                                    }
                                }
                                else
                                {
                                    throw new Exception("you can you no, SearchMode!");
                                }
                            }
                            else if (wt.CarrierConfigWithWafer.AutoRunWithDieMode == AutoRunWithDieModeEnum.ManualAtStart)
                            {
                                throw new Exception("you can you no, AutoRunWithDieMode!");
                            }
                            else
                            {
                                throw new Exception("you can you no, AutoRunWithDieMode!");
                            }

                            return true;
                        }
                    }
                }

                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet ad)
                {
                    if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
                    {
                        for (int i = 0; i < ad.AdapterSetting.MaxUseWaffleCount; i++)
                        {
                            if (CarrierConfigRepository.GetInstance()
                                .IsExists(ad.AdapterSetting.WaffleArray[i].Name))
                            {
                                if (ad.AdapterSetting.WaffleArray[i].Name == this.CurrentNeedChipName && ad.AdapterState.WafflePlateSlotsState[i].SlotState == SlotStatuEnum.Good)
                                {
                                    //if (ad.AdapterSetting.WaffleArray[i].CarrierWithWaffle.AutoRunWithDieMode == AutoRunWithDieModeEnum.Automatic)
                                    //{
                                    //    throw new Exception("you can you no, AutoRunWithDieMode!");
                                    //}
                                    //else if (ad.AdapterSetting.WaffleArray[i].CarrierWithWaffle.AutoRunWithDieMode == AutoRunWithDieModeEnum.Manual)
                                    //{
                                    //    throw new Exception("you can you no, AutoRunWithDieMode!");
                                    //}
                                    //else if (ad.AdapterSetting.WaffleArray[i].CarrierWithWaffle.AutoRunWithDieMode == AutoRunWithDieModeEnum.ManualAtStart)
                                    //{
                                    //    throw new Exception("you can you no, AutoRunWithDieMode!");
                                    //}
                                    //else
                                    //{
                                    //    throw new Exception("you can you no, AutoRunWithDieMode!");
                                    //}

                                    // 切换模板名称及行列间距
                                    WaferSubController.GetInstance().WaferTableController.MoveWaferCameraToWorkPosition(ad.AdapterSetting.WaffleArray[i].CarrierWithWaffleConfig);
                                    Block.GetInstance().SetCurrentCarrier(ad.AdapterSetting.WaffleArray[i].CarrierWithWaffleConfig);
                                    Block.GetInstance().SetCurrentWafflePlateSlotState(ad.AdapterState.WafflePlateSlotsState[i]);
                                    Block.GetInstance().ReSetChangeCarrierSignal();
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            //XtraMessageBox.Show($"The required pieces do not exist -{this.CurrentNeedChipName}, please retry", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            AKRSXtraMessageBox.Show($"不存在Bond需求的料片 -{this.CurrentNeedChipName}, 请重试！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return false;
        }

        /// <summary>
        /// 手动换料
        /// </summary>
        /// <returns>是否更换完成</returns>
        private bool ChangeTabletManual()
        {
            if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet is AdapterTablet)
                {
                    // 判断是否存在
                    int layerIndex = WaferSubController.GetInstance().MagazineController.JudgeNeedChipAtAdapterSlotIndex(this.CurrentNeedChipName);
                    if (layerIndex >= 0)
                    {
                        goto Operation;
                    }
                }
            }
            else
            {
                // 此种情况无需更换料片
                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet)
                {
                    if (CarrierConfigRepository.GetInstance().IsExists(WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet.Name))
                    {
                        if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet.Name == this.CurrentNeedChipName && WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet.SlotState == SlotStatuEnum.Good)
                        {
                            if (!WaferSubController.GetInstance().WaferTableController.IsWaferSensor)
                            {
                                goto ChangeMap;
                            }

                            // 晶圆夹持气缸抬起
                            WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();
                            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet wt)
                            {
                                // 扩晶到扩晶位
                                WaferSubController.GetInstance().WaferTableController.MoveExpandToUpPosition(wt.CarrierConfigWithWafer);
                            }

                            goto Operation;
                        }
                    }
                }
                else if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet)
                {
                    // 判断是否存在
                    int layerIndex = WaferSubController.GetInstance().MagazineController.JudgeNeedChipAtAdapterSlotIndex(this.CurrentNeedChipName);
                    if (layerIndex >= 0)
                    {
                        if (!WaferSubController.GetInstance().WaferTableController.IsWaferSensor)
                        {
                            goto ChangeMap;
                        }

                        // 晶圆夹持气缸抬起
                        WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();

                        goto Operation;
                    }
                }
            }

        // 换图
        ChangeMap:
            if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                FrmChangeStaticAdapterTeach frmChangeStaticAdapterTeach = new FrmChangeStaticAdapterTeach();
                frmChangeStaticAdapterTeach.ShowDialog();
                frmChangeStaticAdapterTeach.Dispose();
                if (frmChangeStaticAdapterTeach.DialogResult != DialogResult.OK)
                {
                    if (frmChangeStaticAdapterTeach.DialogResult == DialogResult.Abort)
                    {
                        return false;
                    }

                    goto ChangeMap;
                }
            }
            else
            {
                FrmChangeTabletTeach frmChangeTablet = new FrmChangeTabletTeach();
                frmChangeTablet.Dispose();
                if (frmChangeTablet.DialogResult != DialogResult.OK)
                {
                    if (frmChangeTablet.DialogResult == DialogResult.Abort)
                    {
                        return false;
                    }

                    goto ChangeMap;
                }
            }

        Operation:
            // 更换料片成功后的操作
            if (!this.ChangeSucceedOperation())
            {
                goto ChangeMap;
            }

            return true;
        }

        /// <summary>
        /// 循环判断需料芯片在哪一层并更换到晶圆台
        /// </summary>
        /// <returns>是否完成</returns>
        private bool ChangeTabletAuto()
        {
            if (MachineStateModel.GetInstance().IsDryCycle)
            {
                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet waferTablet)
                {
                    waferTablet.SlotState = SlotStatuEnum.Good;
                    waferTablet.InitTabletInfo();
                }

                this.ChangeSucceedOperation();

                return true;
            }

            if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.Wafer)
            {
                // 如果是晶圆料片，先关闭真空
                WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
            }

            if (WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift)
            {
                if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                {
                    if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet is AdapterTablet)
                    {
                        // 判断是否存在
                        int layerIndex = WaferSubController.GetInstance().MagazineController.JudgeNeedChipAtAdapterSlotIndex(this.CurrentNeedChipName);
                        if (layerIndex < 0)
                        {
                        RetryChangeStaticAdapter:
                            DialogResult res = DialogResult.Abort;
                            MainForm.SendUiAction(
                                () =>
                                    {
                                        FrmChangeStaticAdapterTeach frmChangeStaticAdapterTeach = new FrmChangeStaticAdapterTeach();
                                        frmChangeStaticAdapterTeach.ShowDialog();
                                        frmChangeStaticAdapterTeach.Dispose();
                                        res = frmChangeStaticAdapterTeach.DialogResult;
                                    });
                            
                            if (res != DialogResult.OK)
                            {
                                if (res == DialogResult.Abort)
                                {
                                    return false;
                                }

                                goto RetryChangeStaticAdapter;
                            }
                            else
                            {
                                // 更换料片成功后的操作
                                if (!this.ChangeSucceedOperation())
                                {
                                    goto RetryChangeStaticAdapter;
                                }

                                return true;
                            }
                        }
                    }
                }
                else
                {
                    // 归还料片信息
                    if (WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo > 0)
                    {
                        this.PlaceWaferInfo();
                    }

                // 判断是否存在
                JudgeNeedChipAtLayer:
                    int layerIndex = WaferSubController.GetInstance().MagazineController.JudgeNeedChipAtLayerIndex(this.CurrentNeedChipName);
                    if (layerIndex < 0)
                    {
                        // 更换magazine
                        //this.res = XtraMessageBox.Show($"There is no Bond required chip type {this.CurrentNeedChipName} where Magazine!\r\n Wafer table about to move!\r\n" + "Click OK: continue,\r\n" + "Click Cancel: stop", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        this.res = AKRSXtraMessageBox.Show($"Magazine不存在Bond需要的芯片- {this.CurrentNeedChipName}！\r\n 晶圆台即将移动！\r\n" + "点击 OK: 继续,\r\n" + "点击 Cancel: 停止", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        switch (this.res)
                        {
                            case DialogResult.OK:
                                WaferSubController.GetInstance().WaferTableController.PlaceWaferInSlot();
                                WaferSubController.GetInstance().MagazineController.MoveMagazineToMarkPosition();
                                //AKRSMessageBoxExt.Show($"Please change magazine and fill status, click ok when you're done!", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                                AKRSMessageBoxExt.Show($"请更换Magazine并且填充状态, 当你做完这些后点击ok！", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });

                                goto JudgeNeedChipAtLayer;

                            case DialogResult.Cancel:
                                // 终止
                                //throw new Exception($"There is no Bond required chip type {this.CurrentNeedChipName} where Magazine!");
                                throw new Exception($"Magazine不存在Bond需要的芯片- {this.CurrentNeedChipName}！");
                        }
                    }

                    if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet adt)
                    {
                        if (adt.Name == WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig
                        .TabletArray[layerIndex].Name)
                        {
                            goto Operation;
                        }                       
                    }

                    // 判断是否在当前层
                    if (layerIndex == WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo - 1)
                    {
                        if (this.PreviousNeedChipName == string.Empty)
                        {
                            if (!WaferSubController.GetInstance().WaferTableController.IsWaferSensor)
                            {
                                WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet = new NullTablet();
                                WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo = 0;
                                WaferSubDevicePara.GetInstance().Save();
                                goto JudgeNeedChipAtLayer;
                            }

                            // 晶圆夹持气缸抬起
                            WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();
                            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet waferTablet)
                            {
                                // 扩晶到扩晶位
                                WaferSubController.GetInstance().WaferTableController.MoveExpandToUpPosition(waferTablet.CarrierConfigWithWafer);
                            }
                        }

                        goto Operation;
                    }

                    // 归还料片
                    WaferSubController.GetInstance().WaferTableController.PlaceWaferInSlot();

                    // 更换i的料层到晶圆台
                    WaferSubController.GetInstance().WaferTableController.RemoveWaferFromSlot(layerIndex);
                }

            Operation:
                // 更换料片成功后的操作
                if (!this.ChangeSucceedOperation())
                {
                    goto Operation;
                }

                return true;
            }
            else
            {
                if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                {
                    if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet is AdapterTablet)
                    {
                        // 判断是否存在
                        int layerIndex = WaferSubController.GetInstance().MagazineController.JudgeNeedChipAtAdapterSlotIndex(this.CurrentNeedChipName);
                        if (layerIndex >= 0)
                        {
                            goto Operation;
                        }
                    }
                }
                else
                {
                    // 此种情况无需更换料片
                    if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet)
                    {
                        if (CarrierConfigRepository.GetInstance().IsExists(WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet.Name))
                        {
                            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet.Name == this.CurrentNeedChipName && WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet.SlotState == SlotStatuEnum.Good)
                            {
                                if (!WaferSubController.GetInstance().WaferTableController.IsWaferSensor)
                                {
                                    goto ChangeMap;
                                }

                                // 晶圆夹持气缸抬起
                                WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();
                                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet wt)
                                {
                                    // 扩晶到扩晶位
                                    WaferSubController.GetInstance().WaferTableController.MoveExpandToUpPosition(wt.CarrierConfigWithWafer);
                                }

                                goto Operation;
                            }
                        }
                    }
                    else if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet)
                    {
                        // 判断是否存在
                        int layerIndex = WaferSubController.GetInstance().MagazineController.JudgeNeedChipAtAdapterSlotIndex(this.CurrentNeedChipName);
                        if (layerIndex >= 0)
                        {
                            if (!WaferSubController.GetInstance().WaferTableController.IsWaferSensor)
                            {
                                goto ChangeMap;
                            }

                            // 晶圆夹持气缸抬起
                            WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();

                            goto Operation;
                        }
                    }
                }

            // 换图
            ChangeMap:
                if (this.currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                {
                    DialogResult res = DialogResult.Abort;
                    MainForm.SendUiAction(
                        () =>
                            {
                                FrmChangeStaticAdapterTeach frmChangeStaticAdapterTeach = new FrmChangeStaticAdapterTeach();
                                frmChangeStaticAdapterTeach.ShowDialog();
                                frmChangeStaticAdapterTeach.Dispose();
                                res = frmChangeStaticAdapterTeach.DialogResult;
                            });

                    if (res != DialogResult.OK)
                    {
                        if (res == DialogResult.Abort)
                        {
                            return false;
                        }

                        goto ChangeMap;
                    }
                }
                else
                {
                    DialogResult res = DialogResult.Abort;
                    MainForm.SendUiAction(
                        () =>
                            {
                                FrmChangeTabletTeach frmChangeTablet = new FrmChangeTabletTeach();
                                frmChangeTablet.Dispose();
                                res = frmChangeTablet.DialogResult;
                            });
                    
                    if (res != DialogResult.OK)
                    {
                        if (res == DialogResult.Abort)
                        {
                            return false;
                        }

                        goto ChangeMap;
                    }
                }

            Operation:
                // 更换料片成功后的操作
                if (!this.ChangeSucceedOperation())
                {
                    goto ChangeMap;
                }

                return true;
            }
        }

        public Task waferSubTask;

        /// <summary>
        /// 上晶圆启动线程
        /// </summary>
        public void Start()
        {
            try
            {
                if (this.waferSubTask == null || this.waferSubTask.Status != TaskStatus.Running)
                {
                    {
                        Block.GetInstance().StartInit();
                        this.PreviousNeedChipName = string.Empty;
                    }

                    Machine.GetInstance().SetState(MachineStateEnum.Working);

                    this.waferSubTask = Task.Factory.StartNew(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("晶圆工作线程");
                                this.Dowork();
                            },
                        CancellationToken.None,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Default);
                }
                else
                {
                    Machine.GetInstance().SetState(MachineStateEnum.Working);
                }

                //if (this.waferSubSystemWorkThread == null || !this.waferSubSystemWorkThread.IsAlive)
                //{
                //    {
                //        Block.GetInstance().StartInit();
                //        this.PreviousNeedChipName = string.Empty;
                //    }

                //    MachineStateModel.GetInstance().MachineState = MachineStateEnum.Working;
                //    this.waferSubSystemWorkThread = new Thread(this.Dowork)
                //    {
                //        IsBackground = true,
                //        Name = "Upper wafer worker thread"
                //    };
                //    this.waferSubSystemWorkThread.Start();
                //}
                //else
                //{
                //    MachineStateModel.GetInstance().MachineState = MachineStateEnum.Working;
                //}
            }
            catch (Exception e)
            {
                //AKRSMessageBoxExt.Show(e.Message + "\r\n" + "Upper wafer thread failed to open!", "Error", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                AKRSMessageBoxExt.Show(e.Message + "\r\n" + "晶圆线程开启失败！", "Error", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
            }
        }

        /// <summary>
        /// AbortWaferSubThread
        /// </summary>
        public void AbortWaferSubThread()
        {
            //if (this.waferSubSystemWorkThread != null && this.waferSubSystemWorkThread.IsAlive)
            //{
            //    this.waferSubSystemWorkThread.Abort();
            //}
        }

        /// <summary>
        /// SetCurrentNeedChipName
        /// </summary>
        /// <param name="chipName">chipName</param>
        public void SetCurrentNeedChipName(string chipName)
        {
            this.CurrentNeedChipName = chipName;
        }

        /// <summary>
        /// 获取当前需求芯片名称
        /// </summary>
        /// <returns>return</returns>
        public string GetCurrentNeedChipName()
        {
            return this.CurrentNeedChipName;
        }

        /// <summary>
        /// 归还料片信息
        /// </summary>
        private void PlaceWaferInfo()
        {
            int currentLayerNo = WaferSubDevicePara.GetInstance().MagazineDevicePara.CurrentLayerNo;
            if (currentLayerNo > 0)
            {
                WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[currentLayerNo - 1] = WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet;
                MagazineAllocationsConfigRepository.GetInstance().Save();
            }
        }

        /// <summary>
        /// 线程
        /// </summary>
        //public Thread waferSubSystemWorkThread;

        /// <summary>
        /// Bond当前需料名称
        /// </summary>        
        private string CurrentNeedChipName { get; set; }

        /// <summary>
        /// Bond上一个需料名称
        /// </summary>       
        private string PreviousNeedChipName { get; set; }

        /// <summary>
        /// 是否是单独空跑模式
        /// </summary>
        [JsonIgnore]
        public bool IsAloneDryCycle { get; set; }

        /// <summary>
        /// 是否记录数据
        /// </summary>
        [JsonIgnore]
        public bool IsRecordSearchData { get; set; }

        /// <summary>
        /// 是否搜索一次
        /// </summary>
        [JsonIgnore]
        public bool IsSearchOne { get; set; }

        /// <summary>
        /// 需要更换料片
        /// </summary>
        private bool IsNeedChange => Block.GetInstance().IsNeedChangeCarrier();

        /// <summary>
        /// 当前需求芯片的载具配置
        /// </summary>
        private BaseCarrierConfig currentNeedCarrierConfig;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// times
        /// </summary>
        private int times = 0;
    }
}
