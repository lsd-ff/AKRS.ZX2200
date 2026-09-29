using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip
{
    using System.IO;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.WM;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using Newtonsoft.Json;
    using System.Linq;
    using System.Threading.Tasks;

    using AKRS.Base;

    using ZedGraph;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using HalconDotNet;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using DevExpress.XtraEditors;
    using System.Windows.Forms;

    using DevExpress.XtraReports.UI;
    using OfficeOpenXml;
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 芯片搜索类
    /// </summary>
    public partial class Block
    {
        /// <summary>
        /// 晶圆图搜索
        /// </summary>
        /// <returns>result</returns>
        private bool SearchMapping()
        {            
            try
            {
                bool isSkip = false;                

                do
                {                    
                    this.currentDieInfo = this.GetNextPositionInfoAndForwardTo();
                    AKRSPoint3D currentDieRealPosition = WaferSubModule.GetInstance().PointConvertAkrsPoint3D(this.currentDieInfo.PhysicalCoordinates);                    
                    if (this.IsPositionSearch)
                    {
                        if (this.PositionSearchMode == PositionSearchModeEnum.StandardSearch)
                        {
                            if (this.IsTwoPointSearch)
                            {
                                throw new Exception("you can you no, IsTwoPointLocation!");

                                // TODO 两点定位 偏移正负号待验证
                                //AKRSPoint3D leftUpPoint = currentDieRealPosition + this.CarrierRowSpacing / 2 + this.CarrierColSpacing / 2;
                                //WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(leftUpPoint);
                                //this.matchResult = this.AnalyseDie(this.currentDieInfo.IsSpringboard, out isSkip);
                                //if (!isSkip)
                                //{
                                //    leftUpPoint = this.WaferTableRealPositionToG0;
                                //    AKRSPoint3D rightDownPoint = currentDieRealPosition - this.CarrierRowSpacing / 2 - this.CarrierColSpacing / 2;
                                //    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(rightDownPoint);
                                //    this.matchResult = this.AnalyseDie(this.currentDieInfo.IsSpringboard, out isSkip);
                                //    if (!isSkip)
                                //    {
                                //        // success
                                //        rightDownPoint = this.WaferTableRealPositionToG0;
                                //        WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch((leftUpPoint + rightDownPoint) / 2);
                                //    }
                                //    else
                                //    {
                                //        this.WaferMapII.SetPhysicalCoordinate(this.currentDieInfo.Position, WaferSubModule.GetInstance().AkrsPoint3DConvertPoint(currentDieRealPosition));
                                //    }
                                //}
                                //else
                                //{
                                //    this.WaferMapII.SetPhysicalCoordinate(this.currentDieInfo.Position, WaferSubModule.GetInstance().AkrsPoint3DConvertPoint(currentDieRealPosition));
                                //}
                            }
                            else
                            {
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(currentDieRealPosition);
                                this.matchResult = this.AnalyseDie(this.currentDieInfo.IsSpringboard, out isSkip, this.DieMatchName);
                                if (WaferTableDevicePara.IsOpenPreGetWaferMapNextDiePos)
                                {
                                    if (isSkip && !this.currentDieInfo.IsSpringboard)
                                    {
                                        this.currentDieInfo = this.WaferMapII.GetNextPositionInfoAndForwardTo();
                                    }
                                }

                                this.WaferMapII.SetPhysicalCoordinate(this.currentDieInfo.Position, WaferSubModule.GetInstance().AkrsPoint3DConvertPoint(currentDieRealPosition));
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
                while (this.currentDieInfo.IsSpringboard || isSkip);
                this.MoveDieToPickCenter();

                return true;
            }
            catch (ThreadAbortException e)
            {
                throw;
            }
            catch (AKRS.WM.NoApproachableChipException e)
            {
                #region 当前批次已经做完

                if (this.IsWaferMapPickRefInEnd)
                {
                    this.pickingRefDieInfo = new List<AKRS.WM.Info>();
                    AKRS.WM.ReferencePoint[] referencePoints = this.WaferMapII.GetCopyOfReferencePoints();
                    if (referencePoints != null && referencePoints.Length > 0)
                    {
                        for (int i = 0; i < referencePoints.Length; i++)
                        {
                            AKRS.WM.ReferencePoint refPoint = referencePoints[i];
                            AKRS.WM.Chip chip = this.WaferMapII.WaferMapData.WaferMap.GetChip(refPoint.Position);
                            System.Collections.Generic.HashSet<AKRS.WM.ChipType> types =
                                this.WaferMapII.GetSelectedChipTypeSet(0);
                            if (types.Contains(chip.Type))
                            {
                                Point point = this.WaferMapII.GetReferenceCoordinates(refPoint.Position);
                                int hCoordinate, vCoordinate;
                                this.WaferMapII.PositionToCoordinates(refPoint.Position,out hCoordinate, out vCoordinate);
                                AKRS.WM.Info info = new AKRS.WM.Info(
                                    refPoint.Position,
                                    point.X,
                                    point.Y,
                                    false,
                                    chip.Type,
                                    hCoordinate,
                                    vCoordinate);
                                this.pickingRefDieInfo.Add(info);
                            }
                        }

                        if (this.pickingRefDieInfo.Count > 0)
                        {
                            return this.NextDie();
                        }
                    }
                }

                this.isNeedChangeCarrier = true;
                this.isNeedChangeCarrierMap = true;
                WaferTableDevicePara.CurrentTablet.SlotState = SlotStatuEnum.Bad;
                WaferSubDevicePara.GetInstance().Save();
                this.WaferMapII.Unlock();
                return false;

                #endregion
            }
            catch (PickRefEnd e)
            {
                this.isNeedChangeCarrier = true;
                this.isNeedChangeCarrierMap = true;
                WaferTableDevicePara.CurrentTablet.SlotState = SlotStatuEnum.Bad;
                WaferSubDevicePara.GetInstance().Save();
                this.WaferMapII.Unlock();
                return false;
            }
            catch (AKRS.WM.NotFoundException e)
            {
                //throw new Exception($"Wafer chart cannot find path, please reset current point!");
                throw new Exception($"晶圆图未找到路径, 请重置当前点！");
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private bool isNeedChangeCarrierMap = false;

        /// <summary>
        /// 开启晶圆图异步寻路
        /// </summary>
        public void StartAsyncSeekingPath()
        {
            if (!this.NeedPickRefDie())
            {
                this.WaferMapII.WaferMapData.SetStrategyCompleted(0, false);
                this.WaferMapII.StartAsyncSeekingPath(false, UsableType.ValidAndVoid, ReferablePointType.LocatedChip);
            }
        }

        /// <summary>
        /// 停止异步寻路
        /// </summary>
        public void StopAsyncSeekingPath()
        {
            this.WaferMapII.StopAsyncSeekingPath();
        }

        /// <summary>
        /// 设置参考点1
        /// </summary>
        public void SetReferencePoint1()
        {
            this.SetWaferMapSpacing();
            AKRS.WM.ReferencePoint[] refArr = this.WaferMapII.GetCopyOfReferencePoints();
            if (refArr == null || refArr.Length == 0)
            {
                //throw new Exception($"The list of reference points is empty!");
                throw new Exception($"参考点列表为空！");
            }

            // 设置参考点1的物理位置
            AKRSPoint3D point = new AKRSPoint3D() { X = this.WaferTableRealPositionToG0.X * 1000, Y = this.WaferTableRealPositionToG0.Y * 1000 };
            this.WaferMapII.SetReferenceCoordinates(refArr[0], point.ToPoint());
        }

        /// <summary>
        /// 设置参考点1
        /// </summary>
        /// <param name="point">point</param>
        public void SetReferencePoint1(Point point)
        {
            this.SetWaferMapSpacing();
            AKRS.WM.ReferencePoint[] refArr = this.WaferMapII.GetCopyOfReferencePoints();
            if (refArr == null || refArr.Length == 0)
            {
                //throw new Exception($"The list of reference points is empty!");
                throw new Exception($"参考点列表为空！");
            }

            this.WaferMapII.SetReferenceCoordinates(refArr[0], point);
        }

        /// <summary>
        /// 查找参考点
        /// </summary>
        public void FindReferencePoint()
        {
            bool bFlag = false;
            AKRS.WM.Position lastRefPos = new AKRS.WM.Position();
            AKRS.WM.Position currentPosition = this.WaferMapII.CurrentPosition;
            this.SetWaferMapSpacing();

            AKRS.WM.ReferencePoint[] referencePoints = this.WaferMapII.GetCopyOfReferencePoints();
            if (referencePoints.Length == 0)
            {
                //throw new Exception($"The list of reference points is empty!");
                throw new Exception($"参考点列表为空！");
            }

            // 晶圆图上前进到参考点1的位置
            this.WaferMapII.ForwardToPosition(referencePoints[0].Position);
            for (int i = 0; i < referencePoints.Length; i++)
            {
                if (i == 0)
                {
                    this.WaferMapII.StartAsyncSeekingPathBetweenTwoPoints(referencePoints[0].Position, referencePoints[i].Position);
                }
                else
                {
                    this.WaferMapII.StartAsyncSeekingPathBetweenTwoPoints(referencePoints[i - 1].Position, referencePoints[i].Position);
                }

                // 等待查找路径结束
                AKRS.WM.SearchResult wmSearchResult;
                while (!this.WaferMapII.WaitForAsyncResultBetweenTwoPoints(0, out wmSearchResult))
                {
                }

                if (wmSearchResult != AKRS.WM.SearchResult.Found)
                {
                    if (wmSearchResult == AKRS.WM.SearchResult.NoResource)
                    {
                        //throw new Exception($"Failed to find reference point, wafer chart could not find path!");
                        throw new Exception($"查找参考点失败, 晶圆图没有发现路径！");
                    }
                    else
                    {
                        //throw new Exception($"Failed to find reference point!");
                        throw new Exception($"查找参考点失败！");
                    }
                }

                AKRS.WM.Info info;
                do
                {
                    #region 逐个搜索到参考点

                    info = this.WaferMapII.GetNextPositionInfoAndForwardToBetweenTwoPoints();
                    AKRSPoint3D currentDiePos = WaferSubModule.GetInstance().PointConvertAkrsPoint3D(info.PhysicalCoordinates);
                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(currentDiePos);

                    // 拍照定位并获取相机结果
                    (bool isSucceed, MatchResult[] matchResults) result = this.MatchResult();
                    if (!result.isSucceed && info.IsSpringboard)
                    {
                        currentDiePos = this.WaferTableRealPositionToG0;
                    }
                    else if (result.isSucceed)
                    {
                        AKRSPoint3D[] offset = this.MatchResultToWorld(result.matchResults);
                        currentDiePos.X = this.WaferTableRealPositionToG0.X - offset[0].X;
                        currentDiePos.Y = this.WaferTableRealPositionToG0.Y - offset[0].Y;

                        WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(currentDiePos);
                    }

                    // 非跳板是参考点
                    if (!info.IsSpringboard)
                    {
                        currentDiePos.X *= 1000;
                        currentDiePos.Y *= 1000;
                        this.WaferMapII.SetReferenceCoordinates(referencePoints[i], currentDiePos.ToPoint());
                        Position wtMap = info.Position;
                        AKRSPoint3D wt = this.WaferTableRealPositionToG0;
                        //AKRSXtraMessageBox.Show("参考点坐标(" + wtMap.Row + "," + wtMap.Column + ") 电机位置：X:" + wt.X + " Y:" + wt.Y);
                    }
                    else
                    {
                        currentDiePos.X *= 1000;
                        currentDiePos.Y *= 1000;
                        this.WaferMapII.SetPhysicalCoordinate(info.Position, currentDiePos.ToPoint());
                    }
                    #endregion
                }
                while (info.IsSpringboard);
            }

            bFlag = true;
            lastRefPos = referencePoints.Last().Position;

            this.WaferMapII.ForwardToPosition(currentPosition);

            // 动作完成标志
            if (bFlag)
            {
                this.WaferMapII.SetCursorPosition(lastRefPos);
            }
        }

        /// <summary>
        /// 移动到起点
        /// </summary>
        public void MoveToStartPoint()
        {
            AKRS.WM.Strategy[] strategies = this.WaferMapII.GetCopyOfStrategies();
            AKRS.WM.Position currentPosition = this.WaferMapII.GetStrategyContext(this.WaferMapII.CurrentStrategyIndex).CurrentPoint;
            this.SetWaferMapSpacing();

            AKRS.WM.ReferencePoint[] referencePoints = this.WaferMapII.GetCopyOfReferencePoints();
            if (referencePoints.Length == 0)
            {
                //throw new Exception($"The list of reference points is empty!");
                throw new Exception($"参考点列表为空！");
            }

            AKRS.WM.Info info;
            #region 带多批次功能的晶圆图

            AKRS.WM.Position referablePoint = this.WaferMapII.FindNearestReferablePoint(
                currentPosition,
                out int xReferablePoint,
                out int yReferablePoint);
            this.WaferMapII.StartAsyncSeekingPathBetweenTwoPoints(referablePoint, currentPosition);

            #endregion

            // 等待查找路径结束
            AKRS.WM.SearchResult wmSearchResult;
            while (!this.WaferMapII.WaitForAsyncResultBetweenTwoPoints(0, out wmSearchResult))
            {
            }

            if (wmSearchResult != AKRS.WM.SearchResult.Found)
            {
                if (wmSearchResult == AKRS.WM.SearchResult.NoResource)
                {
                    //throw new Exception($"Failed to find reference point, wafer chart could not find path!");
                    throw new Exception($"查找参考点失败, 晶圆图没有发现路径！");
                }
                else
                {
                    //throw new Exception($"Failed to find reference point!");
                    throw new Exception($"查找参考点失败！");
                }
            }

            do
            {
                #region 逐个搜索到参考点

                info = this.WaferMapII.GetNextPositionInfoAndForwardToBetweenTwoPoints();

                this.WaferMapII.SetCursorPosition(info.Position);

                AKRSPoint3D currentDiePos = WaferSubModule.GetInstance().PointConvertAkrsPoint3D(info.PhysicalCoordinates);

                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(currentDiePos);

                // 拍照定位并获取相机结果
                (bool isSucceed, MatchResult[] matchResults) result = this.MatchResult();
                if (!result.isSucceed && info.IsSpringboard)
                {
                    currentDiePos = this.WaferTableRealPositionToG0;
                }
                else if (result.isSucceed)
                {
                    AKRSPoint3D[] offset = this.MatchResultToWorld(result.matchResults);
                    currentDiePos.X = this.WaferTableRealPositionToG0.X - offset[0].X;
                    currentDiePos.Y = this.WaferTableRealPositionToG0.Y - offset[0].Y;

                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0PosWithSearch(currentDiePos);
                }

                currentDiePos.X *= 1000;
                currentDiePos.Y *= 1000;
                this.WaferMapII.SetPhysicalCoordinate(info.Position, currentDiePos.ToPoint());
                #endregion
            }
            while (info.Position != currentPosition);

            Position wtMap = info.Position;
            AKRSPoint3D wt = this.WaferTableRealPositionToG0;
            //AKRSXtraMessageBox.Show("起点坐标(" + wtMap.Row + "," + wtMap.Column + ") 电机位置：X:" + wt.X + " Y:" + wt.Y);
        }

        /// <summary>
        /// 在晶圆图上前进到下一个位置，并且返回该位置的信息
        /// </summary>
        /// <returns>返回信息</returns>
        private Info GetNextPositionInfoAndForwardTo()
        {
            if (this.pickingRefDieInfo != null)
            {
                // 说明剩余的参考点也已经取完
                if (this.pickingRefDieInfo.Count == 0)
                {
                    throw new PickRefEnd();
                }

                this.currentDieInfo = this.pickingRefDieInfo[0];
            }
            else
            {
                this.SetWaferMapSpacing();

                if (WaferTableDevicePara.IsOpenPreGetWaferMapNextDiePos)
                {
                    try
                    {
                        this.currentDieInfo = this.WaferMapII.PreGetNextPositionInfo();
                        if (this.currentDieInfo.IsSpringboard)
                        {
                            this.currentDieInfo = this.WaferMapII.GetNextPositionInfoAndForwardTo();
                        }
                    }
                    catch (AKRS.WM.NoApproachableChipException e)
                    {
                        throw;
                    }
                }
                else
                {
                    this.currentDieInfo = this.WaferMapII.GetNextPositionInfoAndForwardTo();
                }
            }

            return this.currentDieInfo;
        }

        /// <summary>
        /// 当前要取参考点（晶圆图已经做到底了，回头取参考点）
        /// </summary>
        /// <returns>取参考点</returns>
        private bool NeedPickRefDie()
        {
            if (this.pickingRefDieInfo == null || this.pickingRefDieInfo.Count == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 记下取走的晶片的信息
        /// </summary>
        private void RemeberPickedDieInfo()
        {
            if (WaferTableDevicePara.IsOpenPreGetWaferMapNextDiePos)
            {
                this.currentDieInfo = this.WaferMapII.GetNextPositionInfoAndForwardTo();

                // todo:这句可能要挪出来
                this.WaferMapII.SetPhysicalCoordinate(this.currentDieInfo.Position, WaferSubModule.GetInstance().AkrsPoint3DConvertPoint(dieMarkCenterG0Pos));
            }
            
            this.lastPickedDieInfo = this.currentDieInfo;
        }

        /// <summary>
        /// 更新取走晶片的状态
        /// </summary>
        private void RefreshPickedDieStatus()
        {
            if (this.NeedPickRefDie())
            {
                this.pickingRefDieInfo.RemoveAt(0);
            }
            else
            {
                try
                {
                    this.WaferMapII.SetChipType(this.lastPickedDieInfo.Position);
                }
                catch (Exception exception)
                {
                    //AKRSXtraMessageBox.Show("Set chip type error for wafer map," + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AKRSXtraMessageBox.Show("设置晶圆图芯片类型失败！," + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            this.WaferMapII.AsyncSaveFile();
        }

        /// <summary>
        /// 设置晶圆图行列间距
        /// </summary>
        private void SetWaferMapSpacing()
        {
            AKRSPoint3D dieRowSize = new AKRSPoint3D() { X = -this.CarrierRowSpacing.X * 1000, Y = -this.CarrierRowSpacing.Y * 1000 };
            AKRSPoint3D dieColSize = new AKRSPoint3D() { X = -this.CarrierColSpacing.X * 1000, Y = -this.CarrierColSpacing.Y * 1000 };
            this.WaferMapII.ChipOffset = new PhysicalChipOffset(dieColSize.ToPoint(), dieRowSize.ToPoint());
        }

        /// <summary>
        /// 当前晶片的信息
        /// </summary>
        private Info currentDieInfo;

        /// <summary>
        /// 取走晶片的信息
        /// </summary>
        private Info lastPickedDieInfo;

        /// <summary>
        /// 需要取走的参考点列表
        /// </summary>
        private List<Info> pickingRefDieInfo;

        /// <summary>
        /// 空晶粒个数
        /// </summary>
        private int countBlankNum;

        /// <summary>
        /// 是否是空晶
        /// </summary>
        private bool isDieBlank;

        /// <summary>
        /// waferMapIILock
        /// </summary>
        private object waferMapIILock = new object();

        /// <summary>
        /// 晶圆图
        /// </summary>
        [JsonIgnore]
        private WaferMapInterfaceImplementer WaferMapII { get; set; }

        /// <summary>
        /// 获取晶圆图文件路径
        /// </summary>
        /// <returns>return</returns>
        public string GetWaferMapFilePath()
        {
            DirectoryInfo directoryInfo = System.IO.Directory.GetParent(this.WaferMapII.WaferMapData.LastWaferMapFilePath);
            return System.IO.Directory.GetParent(directoryInfo.FullName).FullName;
        }

        /// <summary>
        /// 设置晶圆图数据配置
        /// </summary>
        /// <param name="waferMapDataConfig">waferMapDataConfig</param>
        public void SetWaferMapDataConfig(WaferMapDataConfig waferMapDataConfig)
        {
            this.SetJumpStep(waferMapDataConfig.JumpStep);
            this.SetReferencePoints(waferMapDataConfig.ReferencePoints);
            this.SetReferencePoint1(waferMapDataConfig.StepPosition);
            this.SetLastWaferMapFilePath(waferMapDataConfig.LastWaferMapFilePath);
            this.SetStrategyUserDirection(waferMapDataConfig.UserDirection);
            this.SetStrategyInitialStartPoint(waferMapDataConfig.StartPoint);
            this.SetSelectedChipTypes(waferMapDataConfig.SelectedChipTypes);
        }

        /// <summary>
        /// 清除参考点物理坐标
        /// </summary>
        public void ClearReferencePointCoordinates()
        {
            this.WaferMapII.ClearReferencePointCoordinates();
        }

        /// <summary>
        /// 获取参考点1
        /// </summary>
        /// <returns>result</returns>
        /// <exception cref="Exception">Exception</exception>
        public Point GetReferencePoint1()
        {
            try
            {
                return this.WaferMapII.GetReferenceCoordinates(this.WaferMapII.GetCopyOfReferencePoints()[0].Position);
            }
            catch (Exception e)
            {
                //throw new Exception("Coordinates of reference point 1 not set!");
                throw new Exception("未设置参考点1物理位置！");
            }
        }

        /// <summary>
        /// 获取晶圆图数据配置
        /// </summary>
        /// <returns>result</returns>
        public WaferMapDataConfig GetWaferMapDataConfig()
        {
            if (this.WaferMapII.GetCopyOfReferencePoints() == null || this.WaferMapII.GetCopyOfReferencePoints().Length == 0)
            {
                //throw new Exception($"The list of reference points is empty!");
                throw new Exception($"参考点列表为空！");
            }

            WM.ReferencePoint referencePointWM = this.WaferMapII.GetCopyOfReferencePoints()[0];
            ReferencePoint referencePoint = new ReferencePoint() { Row = referencePointWM.Row, Column = referencePointWM.Column, IntLabel = referencePointWM.IntLabel };

            Point stepPosition = this.GetReferencePoint1();

            List<ReferencePoint> referencePoints = new List<ReferencePoint>();
            foreach (var item in this.WaferMapII.GetCopyOfReferencePoints())
            {
                referencePoints.Add(new ReferencePoint() { Row = item.Row, Column = item.Column, IntLabel = item.IntLabel });
            }

            ChipTypeSet selectedChipTypes = this.WaferMapII.WaferMapData.GetSelectedChipTypeSet(0);
            List<string> list = new List<string>();
            foreach (var item in selectedChipTypes)
            {
                list.Add(item.CharValue.ToString());
            }

            Position starPosition = this.WaferMapII.WaferMapData.GetStrategyCurrentStartPoint(0);
            Point startPoint = new Point(starPosition.Row, starPosition.Column);

            return new WaferMapDataConfig()
                       {
                           JumpStep = this.WaferMapII.WaferMapData.JumpInterval,
                           Reference = referencePoint,
                           StepPosition = stepPosition,
                           LastWaferMapFilePath = this.WaferMapII.WaferMapData.LastWaferMapFilePath,
                           ReferencePoints = referencePoints,
                           UserDirection = this.WaferMapII.WaferMapData.GetStrategyUserDirection(0),
                           StartPoint = startPoint,
                           SelectedChipTypes = list,
                       };
        }

        /// <summary>
        /// 设置参考点
        /// </summary>
        /// <param name="referencePoints">referencePoints</param>
        private void SetReferencePoints(List<ReferencePoint> referencePoints)
        {
            try
            {
                this.WaferMapII.WaferMapData.ClearReferencePoints();
                foreach (var item in referencePoints)
                {
                    this.WaferMapII.WaferMapData.AddReferencePoint(new WM.ReferencePoint(item.Row, item.Column, item.IntLabel));
                }
            }
            catch (Exception e)
            {
                // TODO 暂时无处理
            }
        }

        /// <summary>
        /// 设置最后访问的晶圆图文件路径
        /// </summary>
        /// <param name="filePath">filePath</param>
        private void SetLastWaferMapFilePath(string filePath)
        {
            this.WaferMapII.WaferMapData.LastWaferMapFilePath = filePath;
            this.WaferMapII.AsyncSaveFile();
        }

        /// <summary>
        /// 设置跳过个数
        /// </summary>
        /// <param name="i">i</param>
        private void SetJumpStep(int i)
        {
            this.WaferMapII.WaferMapData.JumpInterval = i;
            this.WaferMapII.AsyncSaveFile();
        }

        /// <summary>
        /// SetStrategyUserDirection
        /// </summary>
        /// <param name="userDirection">userDirection</param>
        private void SetStrategyUserDirection(UserDirection userDirection)
        {
            this.WaferMapII.WaferMapData.SetStrategyUserDirection(0, userDirection);
        }

        /// <summary>
        /// SetStrategyInitialStartPoint
        /// </summary>
        /// <param name="point">point</param>
        private void SetStrategyInitialStartPoint(Point point)
        {
            this.WaferMapII.WaferMapData.SetStrategyFirstPoint(0, new Position(point.X, point.Y), SettingSource.Manual);
        }

        /// <summary>
        /// SetSelectedChipTypes
        /// </summary>
        /// <param name="list">list</param>
        private void SetSelectedChipTypes(List<string> list)
        {
            List<ChipType> chipTypeList = new List<ChipType>();
            foreach (var item in list)
            {
                chipTypeList.Add(new ChipType(char.Parse(item)));
            }

            this.WaferMapII.WaferMapData.SetSelectedChipTypes(0, chipTypeList);
        }

        /// <summary>
        /// SetCurrentPoint
        /// </summary>
        /// <param name="point">point</param>
        public void SetCurrentPoint(Point point)
        {
            this.WaferMapII.SetCurrentChip(new Position(point.X, point.Y));
        }
    }
}
