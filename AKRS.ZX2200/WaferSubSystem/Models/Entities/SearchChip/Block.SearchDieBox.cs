using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.DispenseSystem.Models.DispenseAction;
using AKRS.ZX2200.WaferSubSystem.Controls;
using AKRS.ZX2200.WaferSubSystem.Models.Structs;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.ZX2200.Infrastructure.EventBus;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.Main.Controls;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using DevExpress.CodeParser;
    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
    using System.Diagnostics;
    using System.Threading.Tasks;

    /// <summary>
    /// 芯片搜索类
    /// </summary>
    public partial class Block
    {
        /// <summary>
        /// 华夫盒搜索
        /// </summary>
        /// <returns>result</returns>
        private bool SearchDieBox()
        {
            bool isSkip = false;

            do
            {
                this.currentWaffleSlot = this.GetWaffleNextPositionInfo();
                if (this.currentWaffleSlot == null)
                {
                    this.isNeedChangeCarrier = true;
                    this.currentWafflePlateSlotState.SlotState = SlotStatuEnum.Bad;
                    this.AsyncSaveWaferSubDevicePara();
                    return false;
                }

                this.currentDiePosition = this.currentWaffleSlot.SlotPosition;

                if (this.IsPositionSearch)
                {
                    if (this.PositionSearchMode == PositionSearchModeEnum.StandardSearch)
                    {
                        if (this.IsTwoPointSearch)
                        {
                            //throw new Exception("you can you no, IsTwoPointLocation!");

                            // TODO 两点定位 偏移正负号已验证
                            leftUpPoint = this.currentDiePosition - this.DistanceDieP1RelativeCenter;
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(leftUpPoint, false);
                            this.matchResult = this.AnalyseDie(false, out isSkip, this.DieMatchName);
                            if (!isSkip)
                            {
                                double leftUpPointAngle = this.matchResult.Angle;
                                leftUpPoint = this.GetDieMarkCenterG0Pos(this.matchResult);
                                rightDownPoint = this.currentDiePosition - this.DistanceDieP2RelativeCenter;
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(rightDownPoint, false);
                                this.matchResult = this.AnalyseDie(false, out isSkip, this.DieMatchNameP2);
                                if (!isSkip)
                                {
                                    // success
                                    double rightDownPointAngle = this.matchResult.Angle;
                                    rightDownPoint = this.GetDieMarkCenterG0Pos(this.matchResult);
                                    this.matchResult.Angle = (leftUpPointAngle + rightDownPointAngle) / 2;
                                }
                                else
                                {
                                    // skip
                                    this.currentWafflePlateSlotState.CurrentIndex = this.currentWaffleSlot.Index + 1;
                                    this.AsyncSaveWaferSubDevicePara();
                                }
                            }
                            else
                            {
                                // skip
                                this.currentWafflePlateSlotState.CurrentIndex = this.currentWaffleSlot.Index + 1;
                                this.AsyncSaveWaferSubDevicePara();
                            }
                        }
                        else
                        {
                            if (this.currentCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                            {
                                // 只提供拍照位
                                this.resultDieG0Pos = this.currentWaffleSlot.SlotPosition + this.currentCarrierConfig.RelativeDistanceMarkWithCenter;
                                this.resultDieG0Pos.Z = this.currentCarrierConfig.WaferAxisZPosition.Z;
                            }
                            else
                            {
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.currentWaffleSlot.SlotPosition - this.currentCarrierConfig.RelativeDistanceMarkWithCenter, false);
                                this.matchResult = this.AnalyseDie(false, out isSkip, this.DieMatchName);
                                if (isSkip)
                                {
                                    this.currentWafflePlateSlotState.CurrentIndex = this.currentWaffleSlot.Index + 1;
                                    this.AsyncSaveWaferSubDevicePara();
                                }
                            }
                        }
                    }
                    else if (PositionSearchMode == PositionSearchModeEnum.MultipleSearch)
                    {
                        throw new Exception("you can you no, PositionSearchMode!");
                    }
                    else
                    {
                        throw new Exception("you can you no, PositionSearchMode!");
                    }
                }
                else
                {
                    throw new Exception("you can you no, not PositionSearch!");
                }

                this.StopSearch();
            }
            while (isSkip);
            this.MoveDieToPickCenter();

            return true;
        }

        /// <summary>
        /// 获取华夫盒下一颗晶片的信息
        /// </summary>
        /// <returns>下一颗晶片的信息</returns>
        private AcupointEntity GetWaffleNextPositionInfo()
        {
            RetryCommand:
            AcupointEntity waffleSlot = this.currentWafflePlateSlotState.GetWaffleSlot();
            if (waffleSlot != null)
            {
                if (waffleSlot.SlotState == SlotStatuEnum.Good)
                {
                    return waffleSlot;
                }
                else
                {
                    this.currentWafflePlateSlotState.CurrentIndex++;
                    this.AsyncSaveWaferSubDevicePara();
                    goto RetryCommand;
                }
            }

            return null;
        }

        /// <summary>
        /// 异步保存设备参数
        /// </summary>
        public void AsyncSaveWaferSubDevicePara()
        {
            // 异步的原因在于保存文件内容也在实时刷新，否则只有等待线程结束后，本地文件内容才会刷新；
            MainForm.SendUiAction(() =>
                {
                    //string path = ZX2200PathConfig.WaferSubDevicePara + ".bak";
                    //JsonFormatHelper<WaferSubDevicePara>.SaveGenericObject(WaferSubDevicePara.GetInstance(), path);
                    //Thread.Sleep(50);
                    //System.IO.File.Copy(path, ZX2200PathConfig.WaferSubDevicePara, true);
                    WaferSubDevicePara.GetInstance().Save();
                });
            //Action save = () => WaferSubDevicePara.GetInstance().Save();
            //IAsyncResult str = save.BeginInvoke(null, "save");
            //save.EndInvoke(str);
        }

        /// <summary>
        /// 晶片信息转移
        /// </summary>
        private void RemeberWafflePickedDieInfo()
        {
            this.previousWaffleSlot = this.currentWaffleSlot;
        }

        /// <summary>
        /// 刷新晶片状态
        /// </summary>
        private void RefreshWafflePickedDieStatus()
        {
            this.currentWafflePlateSlotState.SetSlotState(this.previousWaffleSlot.RowIndex, this.previousWaffleSlot.ColumnIndex, SlotStatuEnum.Bad);
            this.currentWafflePlateSlotState.CurrentIndex = this.previousWaffleSlot.Index + 1;

            this.AsyncSaveWaferSubDevicePara();
            this.FrmWaffleMap.RefreshMap();
        }

        /// <summary>
        /// 上一个晶片的信息
        /// </summary>
        private AcupointEntity previousWaffleSlot;

        /// <summary>
        /// 当前槽的信息
        /// </summary>
        private AcupointEntity currentWaffleSlot;

        /// <summary>
        /// 华夫盒图
        /// </summary>
        [JsonIgnore]
        private FrmWaffleMap FrmWaffleMap { get; set; }

        /// <summary>
        /// 获取结果芯片的g0坐标
        /// </summary>
        /// <returns>result</returns>
        public AKRSPoint3D GetResultDieG0Pos()
        {
            return this.resultDieG0Pos;
        }

        /// <summary>
        /// 结果芯片的g0坐标
        /// </summary>
        private AKRSPoint3D resultDieG0Pos = new AKRSPoint3D();
    }
}
