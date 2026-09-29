using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AKRS.Galaxy2.Machine.Models;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using System.Threading;
    using System.Threading.Tasks;

    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Services;

    /// <summary>
    /// 芯片搜索类
    /// </summary>
    public partial class Block
    {
        /// <summary>
        /// 普通搜晶方式
        /// </summary>
        /// <returns>result</returns>
        private bool SearchSingle(bool isOffset = true)
        {
            bool isSkip = false;

            do
            {
                Static.RecordTime("晶圆线程", $"单颗搜晶-准备获取芯片位置");

                this.currentDiePosition = this.GetNextPosition(isOffset);
                isOffset = false;

                Static.RecordTime("晶圆线程", $"单颗搜晶-获取芯片位置完成");

                if (this.currentDiePosition == null || this.isNeedAheadChangeTablet)
                {
                    Static.RecordTime("晶圆线程", $"需要换料");

                    this.isNeedAheadChangeTablet = false;
                    this.isNeedChangeCarrier = true;
                    WaferTableDevicePara.CurrentTablet.SlotState = SlotStatuEnum.Bad;
                    this.AsyncSaveWaferSubDevicePara();
                    return false;
                }

                if (this.IsPositionSearch)
                {
                    if (this.PositionSearchMode == PositionSearchModeEnum.StandardSearch)
                    {
                        if (this.IsTwoPointSearch)
                        {
                            //throw new Exception("you can you no, IsTwoPointLocation!");

                            // TODO 两点定位 偏移正负号待验证
                            leftUpPoint = this.currentDiePosition - this.DistanceDieP1RelativeCenter;
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(leftUpPoint);

                            Static.RecordTime("晶圆线程", $"两点定位-移动到芯片左上角完成，准备定位模板：{this.DieMatchName}");

                            this.matchResult = this.AnalyseDie(false, out isSkip, this.DieMatchName);
                            if (!isSkip)
                            {
                                double leftUpPointAngle = this.matchResult.Angle;
                                leftUpPoint = this.GetDieMarkCenterG0Pos(this.matchResult);
                                //rightDownPoint = this.currentDiePosition - this.DistanceDieP2RelativeCenter;
                                rightDownPoint = leftUpPoint + this.DistanceDieP1RelativeCenter - this.DistanceDieP2RelativeCenter;
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(rightDownPoint);

                                Static.RecordTime("晶圆线程", $"两点定位-移动到芯片右下角完成，准备定位模板：{this.DieMatchNameP2}");

                                this.matchResult = this.AnalyseDie(false, out isSkip, this.DieMatchNameP2);
                                if (!isSkip)
                                {
                                    // success
                                    double rightDownPointAngle = this.matchResult.Angle;
                                    rightDownPoint = this.GetDieMarkCenterG0Pos(this.matchResult);
                                    this.matchResult.Angle = (leftUpPointAngle + rightDownPointAngle) / 2;
                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch((leftUpPoint + rightDownPoint) / 2);
                                }
                                else
                                {
                                    // skip
                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.currentDiePosition);
                                }
                            }
                            else
                            {
                                // skip
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.currentDiePosition);
                            }

                            //AKRSPoint3D leftUpPoint = this.currentDiePosition + this.CarrierRowSpacing / 2 + this.CarrierColSpacing / 2;
                            //WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(leftUpPoint);
                            //this.matchResult = this.AnalyseDie(false, out isSkip);
                            //if (!isSkip)
                            //{
                            //    leftUpPoint = this.WaferTableRealPositionToG0;
                            //    AKRSPoint3D rightDownPoint = this.currentDiePosition - this.CarrierRowSpacing / 2 - this.CarrierColSpacing / 2;
                            //    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(rightDownPoint);
                            //    this.matchResult = this.AnalyseDie(false, out isSkip);
                            //    if (!isSkip)
                            //    {
                            //        // success
                            //        rightDownPoint = this.WaferTableRealPositionToG0;
                            //        WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch((leftUpPoint + rightDownPoint) / 2);
                            //    }
                            //    else
                            //    {
                            //        // skip
                            //        WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.currentDiePosition);
                            //    }
                            //}
                            //else
                            //{
                            //    // skip
                            //    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.currentDiePosition);
                            //}
                        }
                        else
                        {
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(this.currentDiePosition);
                            this.matchResult = this.AnalyseDie(false, out isSkip, this.DieMatchName);
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

            Static.RecordTime("晶圆线程", $"晶圆台准备移动到芯片中心");

            this.MoveDieToPickCenter();

            Static.RecordTime("晶圆线程", $"晶圆台移动到芯片中心完成");

            return true;
        }

        /// <summary>
        /// 是否需要提前更换料片
        /// </summary>
        private bool isNeedAheadChangeTablet = false;

        /// <summary>
        /// 获取下一颗点位信息
        /// </summary>
        /// <returns>result</returns>
        private AKRSPoint3D GetNextPosition(bool isOffset)
        {
            //AKRSPoint3D result = this.GetCurrentDieMarkCenterG0Pos();
            AKRSPoint3D result;

            if (this.isFirstSearch)
            {
                result = this.WaferTableRealPositionToG0;

                this.dieRowSize = this.CarrierRowSpacing * (this.AfterNumberOfRows + 1);
                this.dieColSize = this.CarrierColSpacing * (this.AfterNumberOfColumns + 1);

                if (this.SingleSearchDirection == SingleSearchDirection.ToRightDown || this.SingleSearchDirection == SingleSearchDirection.ToRightUp)
                {
                    this.dieColSize *= -1;
                }

                if (this.SingleSearchDirection == SingleSearchDirection.ToDownRight || this.SingleSearchDirection == SingleSearchDirection.ToDownLeft)
                {
                    this.dieRowSize *= -1;
                }

                this.isFirstSearch = false;

                return result;
            }
            else
            {
                if (isOffset)
                {
                    result = this.WaferTableRealPositionToG0 - this.GetPickOffset1();
                }
                else
                {
                    result = this.WaferTableRealPositionToG0;
                }
            }

            if (this.SingleSearchDirection == SingleSearchDirection.ToRightDown || this.SingleSearchDirection == SingleSearchDirection.ToRightUp || this.SingleSearchDirection == SingleSearchDirection.ToLeftDown || this.SingleSearchDirection == SingleSearchDirection.ToLeftUp)
            {
                // Normal search
                result += this.dieColSize;
                if (this.IsInSearchRange(result))
                {
                    return result;
                }

                // Row feed search
                if (this.SingleSearchDirection == SingleSearchDirection.ToRightDown || this.SingleSearchDirection == SingleSearchDirection.ToLeftDown)
                {
                    result -= this.dieRowSize;
                }

                if (this.SingleSearchDirection == SingleSearchDirection.ToRightUp || this.SingleSearchDirection == SingleSearchDirection.ToLeftUp)
                {
                    result += this.dieRowSize;
                }

                if (this.IsInSearchRange(result))
                {
                    // Probe search (recursion)
                    for (int i = 0; i < this.retryTimes; i++)
                    {
                        result += this.dieColSize;
                        if (!this.IsInSearchRange(result))
                        {
                            this.dieColSize *= -1;

                            if (this.SingleSearchDirection == SingleSearchDirection.ToRightDown)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToLeftDown);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToLeftDown)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToRightDown);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToRightUp)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToLeftUp);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToLeftUp)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToRightUp);
                            }

                            return result += this.dieColSize;
                        }
                    }

                    return null;
                }
                else
                {
                    // Reverse search (recursion)
                    for (int i = 0; i < this.retryTimes; i++)
                    {
                        result -= this.dieColSize;
                        if (this.IsInSearchRange(result))
                        {
                            this.dieColSize *= -1;

                            if (this.SingleSearchDirection == SingleSearchDirection.ToRightDown)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToLeftDown);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToLeftDown)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToRightDown);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToRightUp)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToLeftUp);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToLeftUp)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToRightUp);
                            }

                            return result;
                        }
                    }

                    return null;
                }
            }

            if (this.SingleSearchDirection == SingleSearchDirection.ToDownRight || this.SingleSearchDirection == SingleSearchDirection.ToDownLeft || this.SingleSearchDirection == SingleSearchDirection.ToUpRight || this.SingleSearchDirection == SingleSearchDirection.ToUpLeft)
            {
                // Normal search
                result += this.dieRowSize;
                if (this.IsInSearchRange(result))
                {
                    return result;
                }

                // Column feed search
                if (this.SingleSearchDirection == SingleSearchDirection.ToDownRight || this.SingleSearchDirection == SingleSearchDirection.ToUpRight)
                {
                    result -= this.dieColSize;
                }

                if (this.SingleSearchDirection == SingleSearchDirection.ToDownLeft || this.SingleSearchDirection == SingleSearchDirection.ToUpLeft)
                {
                    result += this.dieColSize;
                }

                if (this.IsInSearchRange(result))
                {
                    // Probe search (recursion)
                    for (int i = 0; i < this.retryTimes; i++)
                    {
                        result += this.dieRowSize;
                        if (!this.IsInSearchRange(result))
                        {
                            this.dieRowSize *= -1;

                            if (this.SingleSearchDirection == SingleSearchDirection.ToDownRight)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToUpRight);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToUpRight)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToDownRight);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToDownLeft)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToUpLeft);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToUpLeft)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToDownLeft);
                            }

                            return result += this.dieRowSize;
                        }
                    }

                    return null;
                }
                else
                {
                    // Reverse search (recursion)
                    for (int i = 0; i < this.retryTimes; i++)
                    {
                        result -= this.dieRowSize;
                        if (this.IsInSearchRange(result))
                        {
                            this.dieRowSize *= -1;

                            if (this.SingleSearchDirection == SingleSearchDirection.ToDownRight)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToUpRight);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToUpRight)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToDownRight);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToDownLeft)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToUpLeft);
                            }
                            else if (this.SingleSearchDirection == SingleSearchDirection.ToUpLeft)
                            {
                                this.SetSingleSearchDirection(SingleSearchDirection.ToDownLeft);
                            }

                            return result;
                        }
                    }

                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// 提前到达末端
        /// </summary>
        private void AheadReachEnd()
        {
            AKRSPoint3D result = this.WaferTableRealPositionToG0;
            if (this.SingleSearchDirection == SingleSearchDirection.ToRightDown || this.SingleSearchDirection == SingleSearchDirection.ToRightUp || this.SingleSearchDirection == SingleSearchDirection.ToLeftDown || this.SingleSearchDirection == SingleSearchDirection.ToLeftUp)
            {
                if (this.SingleSearchDirection == SingleSearchDirection.ToRightDown)
                {
                    this.SetSingleSearchDirection(SingleSearchDirection.ToLeftDown);
                }
                else if (this.SingleSearchDirection == SingleSearchDirection.ToLeftDown)
                {
                    this.SetSingleSearchDirection(SingleSearchDirection.ToRightDown);
                }
                else if (this.SingleSearchDirection == SingleSearchDirection.ToRightUp)
                {
                    this.SetSingleSearchDirection(SingleSearchDirection.ToLeftUp);
                }
                else if (this.SingleSearchDirection == SingleSearchDirection.ToLeftUp)
                {
                    this.SetSingleSearchDirection(SingleSearchDirection.ToRightUp);
                }

                if (this.SingleSearchDirection == SingleSearchDirection.ToRightDown || this.SingleSearchDirection == SingleSearchDirection.ToLeftDown)
                {
                    result -= this.dieRowSize;
                }

                if (this.SingleSearchDirection == SingleSearchDirection.ToRightUp || this.SingleSearchDirection == SingleSearchDirection.ToLeftUp)
                {
                    result += this.dieRowSize;
                }

                this.dieColSize *= -1;
            }
            else
            {
                if (this.SingleSearchDirection == SingleSearchDirection.ToDownRight)
                {
                    this.SetSingleSearchDirection(SingleSearchDirection.ToUpRight);
                }
                else if (this.SingleSearchDirection == SingleSearchDirection.ToUpRight)
                {
                    this.SetSingleSearchDirection(SingleSearchDirection.ToDownRight);
                }
                else if (this.SingleSearchDirection == SingleSearchDirection.ToDownLeft)
                {
                    this.SetSingleSearchDirection(SingleSearchDirection.ToUpLeft);
                }
                else if (this.SingleSearchDirection == SingleSearchDirection.ToUpLeft)
                {
                    this.SetSingleSearchDirection(SingleSearchDirection.ToDownLeft);
                }

                if (this.SingleSearchDirection == SingleSearchDirection.ToDownRight || this.SingleSearchDirection == SingleSearchDirection.ToUpRight)
                {
                    result -= this.dieColSize;
                }

                if (this.SingleSearchDirection == SingleSearchDirection.ToDownLeft || this.SingleSearchDirection == SingleSearchDirection.ToUpLeft)
                {
                    result += this.dieColSize;
                }

                this.dieRowSize *= -1;
            }

            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(result);
        }

        /// <summary>
        /// 设置当前点坐标
        /// </summary>
        /// <param name="dieMarkCenterG0Pos">芯片Mark中心G0坐标</param>
        private void SetCurrentPosition(AKRSPoint3D dieMarkCenterG0Pos)
        {
            WaferTablet wt = (WaferTablet)this.WaferTableDevicePara.CurrentTablet;
            wt.CurrentPosition = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
            this.AsyncSaveWaferSubDevicePara();
        }

        /// <summary>
        /// 设置单颗搜索方向
        /// </summary>
        /// <param name="direction">方向</param>
        private void SetSingleSearchDirection(SingleSearchDirection direction)
        {
            if (this.currentCarrierConfig is CarrierWithWaferConfig wt)
            {
                ((WaferTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet).SingleSearchDirectionCurrent = direction;
                this.AsyncSaveWaferSubDevicePara();
            }
        }

        /// <summary>
        /// 获取单颗搜索方向
        /// </summary>
        /// <returns>单颗搜索方向</returns>
        public string GetSingleSearchDirection()
        {
            return ((WaferTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet).SingleSearchDirectionCurrent.GetDescription();
        }

        /// <summary>
        /// 递归次数
        /// </summary>
        private int retryTimes = 50;

        /// <summary>
        /// 当前晶片的位置
        /// </summary>
        private AKRSPoint3D currentDiePosition;

        /// <summary>
        /// 行间距
        /// </summary>
        private AKRSPoint3D dieRowSize;

        /// <summary>
        /// 列间距
        /// </summary>
        private AKRSPoint3D dieColSize;
    }
}
