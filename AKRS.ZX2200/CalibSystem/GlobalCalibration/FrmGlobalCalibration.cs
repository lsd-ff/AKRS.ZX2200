using AKRS.ZX2200.Infrastructure.Service;
using DataAnalysis.Acquisition;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate;

    using ch.etel.edi.dsa.v40;

    using DevExpress.XtraEditors;

    using global::GlobalCalibration.GlobalCalibration;

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// 标定窗体
    /// </summary>
    public partial class FrmGlobalCalibration : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 全局标定
        /// </summary>
        private GlobalCalibrationDomain GlobalCalibrationDomain =>
            CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance();

        /// <summary>
        /// 固晶补偿标定
        /// </summary>
        private BondCompensate BondCompensate => BondCompensate.GetInstance();

        /// <summary>
        /// Bond模组
        /// </summary>
        private readonly BondModule bondModule = new BondModule();

        /// <summary>
        /// 全局标定线程
        /// </summary>
        private Task globalCalibrationTask;

        /// <summary>
        /// 2D补偿的X
        /// </summary>
        private readonly List<double> stageAxisXMapping = new List<double>();

        /// <summary>
        /// 2D补偿的Y
        /// </summary>
        private readonly List<double> stageAxisYMapping = new List<double>();

        /// <summary>
        /// 是否停止
        /// </summary>
        private bool isStop = false;

        /// <summary>
        /// 定位结果
        /// </summary>
        private readonly List<List<double>> result = new List<List<double>>();

        /// <summary>
        /// 方向
        /// </summary>
        private readonly double direct = 1;

        /// <summary>
        /// 标定窗体
        /// </summary>
        public FrmGlobalCalibration()
        {
            this.InitializeComponent();

            if (this.bondModule.BondAxisX.AxisDrive is ETELAxis)
            {
                this.direct = -1;
            }
        }

        /// <summary>
        /// 获取轴的坐标
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtGetStartPos_Click(object sender, EventArgs e)
        {
            this.MoveToCameraCenter(this.GlobalCalibrationDomain.PrName);
            this.SpStartX.EditValue = this.bondModule.BondAxisX.GetCmdPosition();
            this.SpStartY.EditValue = this.bondModule.BondAxisY.GetCmdPosition();
            this.SpStartZ.EditValue = this.bondModule.BondHead.AxisZ.GetRealPosition();
            this.Save();
        }

        /// <summary>
        /// 制作PR
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEditPR_Click(object sender, EventArgs e)
        {
            string name = this.TxPrName.Text;

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Substrate;


                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            //prEntity.SetHardware(System2Domain.GetInstance().System2Controller.GetHardware(CameraTypeEnum.BondCamera));

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStart_Click(object sender, EventArgs e)
        {
            this.Save();
            if (this.globalCalibrationTask == null || this.globalCalibrationTask.IsCompleted)
            {
                this.isStop = false;
                this.globalCalibrationTask = new Task(this.GlobalCalibration);
                this.globalCalibrationTask.Start();
            }
        }

        /// <summary>
        /// 全局标定方法
        /// </summary>
        private void GlobalCalibration()
        {
            // 跑数据
            if (this.CmbChooseType.SelectedIndex == 0)
            {
                this.RecordData(999);
            }
            else if (this.CmbChooseType.SelectedIndex == 1)
            {
                this.InitData();
                this.PreliminaryCompensationX();
                this.PreliminaryCompensationY();
                this.Mapping2DCalibration(999);
            }
            else if (this.CmbChooseType.SelectedIndex == 2)
            {
                this.ScaleX();
            }
            else if (this.CmbChooseType.SelectedIndex == 3)
            {
                this.ScaleY();
            }
            else if (this.CmbChooseType.SelectedIndex == 4)
            {
                this.StageCalibrationTest();
            }
            else if (this.CmbChooseType.SelectedIndex == 5)
            {
                this.InitBondCompensateCalibration();
                this.BondCompensateCalibration(999);
            }
            else if (this.CmbChooseType.SelectedIndex == 6)
            {
                this.InitData();
                this.PreliminaryCompensationX();
                this.PreliminaryCompensationY();
                this.InitBondCompensateCalibration();

                while (!this.isStop)
                {
                    this.Mapping2DCalibration(1);
                    this.BondCompensateCalibration(1);
                }
            }
            else if (this.CmbChooseType.SelectedIndex == 7)
            {
                while (!this.isStop)
                {
                    this.RecordData(1);
                    this.BondCompensateRecordData(1);
                }
               
            }
        }

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStop_Click(object sender, EventArgs e)
        {
            this.isStop = true;

            if (this.CmbChooseType.SelectedText == "Z-XY")
            {
                return;
            }

            AxisCompensateProgram.GetInstance().Save();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmGlobalCalibration_Load(object sender, EventArgs e)
        {
            this.SpStartX.EditValue = this.GlobalCalibrationDomain.StartPoint3D.X;
            this.SpStartY.EditValue = this.GlobalCalibrationDomain.StartPoint3D.Y;
            this.SpStartZ.EditValue = this.GlobalCalibrationDomain.StartPoint3D.Z;

            this.SpColumnSpacing.EditValue = this.GlobalCalibrationDomain.ColumnSpacing;
            this.SpRowSpacing.EditValue = this.GlobalCalibrationDomain.RowSpacing;
            this.SpColumnCount.EditValue = this.GlobalCalibrationDomain.ColumnCount;
            this.SpRowCount.EditValue = this.GlobalCalibrationDomain.RowCount;
            this.CkAutoScale.Checked = this.GlobalCalibrationDomain.AutoScale;
            this.TxPrName.Text = this.GlobalCalibrationDomain.PrName;
            this.SpPrDelay.EditValue = this.GlobalCalibrationDomain.Delay;

            this.SpColumnSpacingNew.EditValue = this.BondCompensate.ColumnSpacing;
            this.SpRowSpacingNew.EditValue = this.BondCompensate.RowSpacing;
            this.SpColumnCountNew.EditValue = this.BondCompensate.ColumnCount;
            this.SpRowCountNew.EditValue = this.BondCompensate.RowCount;

            this.SpStartNewX.EditValue = this.BondCompensate.StartPoint?.X;
            this.SpStartNewY.EditValue = this.BondCompensate.StartPoint?.Y;
            this.SpStartNewZ.EditValue = this.BondCompensate.StartPoint?.Z;
            this.TxBondPrName.Text = this.BondCompensate.PrName;

            this.CmbChooseType.Properties.Items.Add("验证数据");
            this.CmbChooseType.Properties.Items.Add("2维补偿");
            this.CmbChooseType.Properties.Items.Add("单轴补偿X");
            this.CmbChooseType.Properties.Items.Add("单轴补偿X");
            this.CmbChooseType.Properties.Items.Add("测试贴片精度");
            this.CmbChooseType.Properties.Items.Add("固晶补偿标定");
            this.CmbChooseType.Properties.Items.Add("2维补偿和固晶补偿标定");
            this.CmbChooseType.Properties.Items.Add("2维补偿和固晶补偿验证");
            this.CmbChooseType.SelectedIndex = 0;

            this.CmbMoveCenterModule.Properties.Items.Add("相机");
            this.CmbMoveCenterModule.Properties.Items.Add("轴");
            this.CmbMoveCenterModule.SelectedIndex = 0;
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmGlobalCalibration_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (this.globalCalibrationTask == null)
            {
                return;
            }

            if (!this.globalCalibrationTask.IsCompleted)
            {
                AKRSXtraMessageBox.Show("Please click Stop first");
                e.Cancel = true;
                return;
            }

            this.globalCalibrationTask.Dispose();
        }

        /// <summary>
        /// 移动到相机中心
        /// </summary>
        /// <param name="prName">视觉模板名称</param>
        private void MoveToCameraCenter(string prName)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(prName);

        RetryCommand:

            Thread.Sleep(200);

            // 执行定位
            ExcuteResult p1Result = pREntity.DoWork();

            // 定位失败，直接返回
            if (p1Result != ExcuteResult.Success)
            {
                AKRSXtraMessageBox.Show("Vision fail");
                return;
            }

            // 定位结果
            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            if (Math.Abs(matchResult.CenterX - 1224) < 0.1 && Math.Abs(matchResult.CenterY - 1024) < 0.1)
            {
                return;
            }

            // 相机中心移动
            double x = this.bondModule.BondAxisX.GetCmdPosition()
                       - (1224 - matchResult.CenterX) * (double)this.SpinX.Value;
            double y = this.bondModule.BondAxisY.GetCmdPosition()
                       + (1024 - matchResult.CenterY) * (double)this.SpinY.Value;

            // 移动到相机中心
            this.bondModule.BondAxisX.AbsoluteMove(x);
            this.bondModule.BondAxisY.AbsoluteMove(y);

            Thread.Sleep(100);

            goto RetryCommand;
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.GlobalCalibrationDomain.StartPoint3D = new AKRSPoint3D(
                (double)this.SpStartX.Value,
                (double)this.SpStartY.Value,
                (double)this.SpStartZ.Value);

            this.GlobalCalibrationDomain.ColumnSpacing = (double)this.SpColumnSpacing.Value;
            this.GlobalCalibrationDomain.RowSpacing = (double)this.SpRowSpacing.Value;
            this.GlobalCalibrationDomain.ColumnCount = (int)this.SpColumnCount.Value;
            this.GlobalCalibrationDomain.RowCount = (int)this.SpRowCount.Value;

            this.BondCompensate.ColumnSpacing = (double)this.SpColumnSpacingNew.Value;
            this.BondCompensate.RowSpacing = (double)this.SpRowSpacingNew.Value;
            this.BondCompensate.ColumnCount = (int)this.SpColumnCountNew.Value;
            this.BondCompensate.RowCount = (int)this.SpRowCountNew.Value;

            this.GlobalCalibrationDomain.PrName = this.TxPrName.Text;
            this.GlobalCalibrationDomain.Delay = (int)this.SpPrDelay.Value;

            this.BondCompensate.PrName = this.TxBondPrName.Text;

            this.BondCompensate.StartPoint = new AKRSPoint3D(
                (double)this.SpStartNewX.Value,
                (double)this.SpStartNewY.Value,
                (double)this.SpStartNewZ.Value);
            this.GlobalCalibrationDomain.Save();
            this.BondCompensate.Save();
        }

        /// <summary>
        /// 改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbChooseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CmbChooseType.SelectedText == "固晶补偿标定" || this.CmbChooseType.SelectedText == "2维补偿和固晶补偿标定" || this.CmbChooseType.SelectedIndex == 7)
            {
                this.GpBondCompensate.Enabled = true;
            }
            else
            {
                this.GpBondCompensate.Enabled = false;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// 关闭2D图
        /// </summary>
        private void CloseStage()
        {
            if (this.bondModule.BondAxisX.AxisDrive is ETELAxis)
            {
                ETelDrive.Close2DCompensate(this.bondModule.BondAxisX, this.bondModule.BondAxisY);
            }
            else
            {
                GuGaoDrive.Close2DCompensate();
            }

            #region MyRegion

            this.bondModule.BondAxisY.ResetError();

            this.bondModule.BondAxisY.ServoOn();

            this.bondModule.BondAxisY.GoHome();

            this.bondModule.BondAxisY.ServoOn();

            this.bondModule.BondAxisX.ResetError();

            this.bondModule.BondAxisX.ServoOn();

            this.bondModule.BondAxisX.GoHome();

            this.bondModule.BondAxisX.ServoOn();

            #endregion
        }

        /// <summary>
        /// 开启2D图
        /// </summary>
        private void OpenStage()
        {
            if (this.bondModule.BondAxisX.AxisDrive is ETELAxis)
            {
                ETelDrive.Close2DCompensate(this.bondModule.BondAxisX, this.bondModule.BondAxisY);

                #region 回零

                this.bondModule.BondAxisX.ResetError();

                this.bondModule.BondAxisX.ServoOn();

                this.bondModule.BondAxisX.GoHome();

                this.bondModule.BondAxisX.ServoOn();

                this.bondModule.BondAxisY.ResetError();

                this.bondModule.BondAxisY.ServoOn();

                this.bondModule.BondAxisY.GoHome();

                this.bondModule.BondAxisY.ServoOn();

                #endregion

                ETelDrive.Open2DCompensate(
                    this.bondModule.BondAxisX,
                    this.bondModule.BondAxisY,
                    this.GlobalCalibrationDomain.TextName);
            }
            else if (this.bondModule.BondAxisX.AxisDrive is GTAxis)
            {
                GuGaoDrive.Close2DCompensate();

                #region 回零

                this.bondModule.BondAxisY.ResetError();

                this.bondModule.BondAxisY.ServoOn();

                this.bondModule.BondAxisY.GoHome();

                this.bondModule.BondAxisX.ResetError();

                this.bondModule.BondAxisX.ServoOn();

                this.bondModule.BondAxisX.GoHome();

                #endregion

                GuGaoDrive.Open2DCompensate();
            }
        }

        /// <summary>
        /// 开启单轴补偿
        /// </summary>
        private void OpenXOffset()
        {
            // 获取ETel轴的两个驱动器对象
            DsaDrive moveDriveX = ((ETELAxis)this.bondModule.BondAxisX.AxisDrive).GetDrive();

            // 后续更改
            moveDriveX.scaleMappingActivate(Dsa.SCALE_MAPPING_LINEAR_ACTIVATION);

            this.bondModule.BondAxisX.ResetError();

            this.bondModule.BondAxisX.ServoOn();

            this.bondModule.BondAxisX.GoHome();
        }

        /// <summary>
        /// 关闭单轴补偿
        /// </summary>
        private void CloseXOffset()
        {
            // 获取ETel轴的两个驱动器对象
            DsaDrive moveDriveX = ((ETELAxis)this.bondModule.BondAxisX.AxisDrive).GetDrive();

            // 后续更改
            moveDriveX.scaleMappingDeactivate();

            this.bondModule.BondAxisX.ResetError();

            this.bondModule.BondAxisX.ServoOn();

            this.bondModule.BondAxisX.GoHome();
        }

        /// <summary>
        /// 开启单轴补偿
        /// </summary>
        private void OpenYOffset()
        {
            // 获取ETel轴的两个驱动器对象
            DsaDrive moveDriveY = ((ETELAxis)this.bondModule.BondAxisY.AxisDrive).GetDrive();

            // 后续更改
            moveDriveY.scaleMappingActivate(Dsa.SCALE_MAPPING_LINEAR_ACTIVATION);

            this.bondModule.BondAxisY.ResetError();

            this.bondModule.BondAxisY.ServoOn();

            this.bondModule.BondAxisY.GoHome();
        }

        /// <summary>
        /// 关闭单轴补偿
        /// </summary>
        private void CloseYOffset()
        {
            // 获取ETel轴的两个驱动器对象
            DsaDrive moveDriveY = ((ETELAxis)this.bondModule.BondAxisY.AxisDrive).GetDrive();

            // 后续更改
            moveDriveY.scaleMappingDeactivate();

            this.bondModule.BondAxisY.ResetError();

            this.bondModule.BondAxisY.ServoOn();

            this.bondModule.BondAxisY.GoHome();
        }

        /// <summary>
        ///  2DMapping标定
        /// </summary>
        /// <param name="count">次数</param>
        private void Mapping2DCalibration(int count)
        {
            this.bondModule.MoveBondXYZ(this.GlobalCalibrationDomain.StartPoint3D);

            for (int k = 0; k < count; k++)
            {
                for (int i = 0; i < this.GlobalCalibrationDomain.RowCount; i++)
                {
                    for (int j = 0; j < this.GlobalCalibrationDomain.ColumnCount; j++)
                    {
                        if (this.isStop)
                        {
                            return;
                        }

                        AKRSPoint3D point3D = new AKRSPoint3D(
                            this.GlobalCalibrationDomain.StartPoint3D.X
                            + j * this.GlobalCalibrationDomain.ColumnSpacing,
                            this.GlobalCalibrationDomain.StartPoint3D.Y + i * this.GlobalCalibrationDomain.RowSpacing,
                            this.GlobalCalibrationDomain.StartPoint3D.Z);

                        Stopwatch stopwatch = new Stopwatch();

                        this.MoveXY(point3D.X, point3D.Y);

                        Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                        // 寻找Pr模板
                        PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                            .Find(this.GlobalCalibrationDomain.PrName);

                        ExcuteResult re = pREntity.DoWork(this.bondModule.GetHardware(), false);

                        if (re == ExcuteResult.Success)
                        {
                            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                            double x = -(1224.0 - matchResult.CenterX) * (double)this.SpinX.Value * this.direct;

                            double y = (1024.0 - matchResult.CenterY) * (double)this.SpinY.Value * this.direct;

                            this.stageAxisXMapping[i * this.GlobalCalibrationDomain.ColumnCount + j] += x;
                            this.stageAxisYMapping[i * this.GlobalCalibrationDomain.ColumnCount + j] += y;

                            this.result.Add(
                                new List<double>()
                                    {
                                        matchResult.CenterX,
                                        matchResult.CenterY,
                                        point3D.X,
                                        point3D.Y,
                                        x,
                                        y,
                                        this.stageAxisXMapping
                                            [i * this.GlobalCalibrationDomain.ColumnCount + j],
                                        this.stageAxisYMapping[
                                            i * this.GlobalCalibrationDomain.ColumnCount + j],
                                        this.bondModule.BondAxisX.GetRealPosition(),
                                        this.bondModule.BondAxisY.GetRealPosition()
                                    });
                        }
                        else
                        {
                            this.result.Add(
                                new List<double>()
                                    {
                                        999,
                                        999,
                                        999,
                                        999,
                                        999,
                                        999,
                                        999,
                                        999
                                    });

                            this.stageAxisXMapping[i * this.GlobalCalibrationDomain.ColumnCount + j] += 0;
                            this.stageAxisYMapping[i * this.GlobalCalibrationDomain.ColumnCount + j] += 0;
                        }
                    }
                }

                this.SaveData(k);
            }
        }

        /// <summary>
        /// 2DMapping标定
        /// </summary>
        private void StageCalibrationTest()
        {
            this.result.Clear();
            this.MoveToCameraCenter(this.GlobalCalibrationDomain.PrName);

            // 获取当前位置
            AKRSPoint3D point = this.bondModule.Get3DRealPosition();

            for (int j = 0; j < this.GlobalCalibrationDomain.ColumnCount - 30; j++)
            {
                if (this.isStop)
                {
                    this.result.Clear();
                    return;
                }

                AKRSPoint3D point3D = new AKRSPoint3D(
                    point.X + j * this.GlobalCalibrationDomain.ColumnSpacing,
                    point.Y + 5,
                    this.GlobalCalibrationDomain.StartPoint3D.Z);

                this.MoveXY(point3D.X, point3D.Y);

                this.MoveToCameraCenter(this.GlobalCalibrationDomain.PrName);

                AKRSPoint3D point3D1 = this.bondModule.Get3DRealPosition();

                this.MoveXY(point3D1.X + 50, point3D1.Y);

                Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                // 寻找Pr模板
                PREntity pREntity =
                    (PREntity)VisionEntityRepository.GetInstance().Find(this.GlobalCalibrationDomain.PrName);

                ExcuteResult re = pREntity.DoWork(this.bondModule.GetHardware(), false);

                if (re == ExcuteResult.Success)
                {
                    MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                    double x = -(1224.0 - matchResult.CenterX) * (double)this.SpinX.Value * this.direct;

                    double y = (1024.0 - matchResult.CenterY) * (double)this.SpinY.Value * this.direct;

                    this.result.Add(
                        new List<double>()
                            {
                                matchResult.CenterX,
                                matchResult.CenterY,
                                point3D.X,
                                point3D.Y,
                                x,
                                y,
                                0,
                                0
                            });
                }
                else
                {
                    throw new Exception("定位失败");
                }
            }

            string path = FileHelper.CreateFileByDate("全局标定");
            FileHelper.SaveDoubleExcel(this.result, Path.Combine(path, DateTime.Now.ToFileTime().ToString() + ".xlsx"));
            this.result.Clear();
        }

        /// <summary>
        /// 跑数据
        /// </summary>
        /// <param name="count">次数</param>
        private void RecordData(int count)
        {
            this.result.Clear();

            this.bondModule.MoveBondXYZ(this.GlobalCalibrationDomain.StartPoint3D);

            // 寻找Pr模板
            PREntity pREntity =
                (PREntity)VisionEntityRepository.GetInstance().Find(this.GlobalCalibrationDomain.PrName);

            for (int k = 0; k < count; k++)
            {
                for (int i = 0; i < this.GlobalCalibrationDomain.RowCount; i++)
                {
                    for (int j = 0; j < this.GlobalCalibrationDomain.ColumnCount; j++)
                    {
                        if (this.isStop)
                        {
                            return;
                        }

                        AKRSPoint2D point2D = new AKRSPoint2D(
                            this.GlobalCalibrationDomain.StartPoint3D.X
                            + j * this.GlobalCalibrationDomain.ColumnSpacing,
                            this.GlobalCalibrationDomain.StartPoint3D.Y + i * this.GlobalCalibrationDomain.RowSpacing);
                        
                        this.MoveXY(point2D.X, point2D.Y);
                        
                        Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                        // 执行定位
                        ExcuteResult p1Result = pREntity.DoWork(this.bondModule.GetHardware(), false);

                        // 定位失败，直接返回
                        if (p1Result == ExcuteResult.Success)
                        {
                            // 定位结果
                            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                            this.result.Add(
                                new List<double>()
                                    {
                                        matchResult.CenterX,
                                        matchResult.CenterY,
                                        point2D.X,
                                        point2D.Y,
                                        this.bondModule.BondAxisX.GetRealPosition(),
                                        this.bondModule.BondAxisY.GetRealPosition()
                                    });
                        }
                        else
                        {
                            this.result.Add(
                                new List<double>()
                                    {
                                        1224,
                                        1024,
                                        point2D.X,
                                        point2D.Y,
                                        99999.9999,
                                        99999.9999
                                    });
                        }
                    }
                }

                // 数据处理
                string path = FileHelper.CreateFileByDate("全局标定");

                FileHelper.SaveDoubleExcel(
                    this.result,
                    Path.Combine(path, DateTime.Now.ToFileTime().ToString() + ".xlsx"));

                this.result.Clear();
            }
        }

        /// <summary>
        /// 移动XY
        /// </summary>
        /// <param name="x">x轴</param>
        /// <param name="y">Y轴</param>
        private void MoveXY(double x, double y)
        {
            if (this.bondModule.BondAxisX.AxisDrive is ETELAxis)
            {
                // 设置群组
                DsaIpolGroup iGroup = System2Domain.GetInstance().BondModuleController.GetXYIpolGroup();


                // 开始插补
                iGroup.ipolBegin();

                // 设置为绝对坐标系 ，不设置绝对坐标系
                iGroup.ipolSetAbsMode(true, -1);

                iGroup.ipolTanVelocity(0.05);
                iGroup.ipolTanAcceleration(0.5);
                iGroup.ipolTanDeceleration(0.5);

                // 设置抖动时间，不知道什么意思
                iGroup.ipolTanJerkTime(0.01);

                iGroup.ipolLine(x / 1000.0, y / 1000.0);

                // 等待插补结束
                iGroup.ipolWaitMovement(100000);

                // 退出插补模式
                iGroup.ipolEnd();
            }
            else
            {
                this.bondModule.MoveBondXY(x, y);
            }
        }

        /// <summary>
        /// X轴单轴补偿
        /// </summary>
        private void ScaleX()
        {
            this.CloseXOffset();

            bool isFirst = true;

            List<double> result = new List<double>();

            for (int i = 0; i < this.SpCycleTimes.Value; i++)
            {
                for (int j = 0; j < this.GlobalCalibrationDomain.ColumnCount; j++)
                {
                    if (this.isStop)
                    {
                        return;
                    }

                    AKRSPoint3D point3D = new AKRSPoint3D(
                        this.GlobalCalibrationDomain.StartPoint3D.X + j * this.GlobalCalibrationDomain.ColumnSpacing,
                        this.GlobalCalibrationDomain.StartPoint3D.Y,
                        this.GlobalCalibrationDomain.StartPoint3D.Z);

                    this.MoveXY(point3D.X, point3D.Y);

                    Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                    // 寻找Pr模板
                    PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                        .Find(this.GlobalCalibrationDomain.PrName);

                    ExcuteResult re = pREntity.DoWork();

                    if (re != ExcuteResult.Success)
                    {
                        return;
                    }

                    MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                    double x = 0;
                    double y = 0;

                    if (this.CmbMoveCenterModule.SelectedIndex == 0)
                    {
                        x = (1224.0 - matchResult.CenterX) * (double)this.SpinX.Value;

                        y = -(1024.0 - matchResult.CenterY) * (double)this.SpinY.Value;
                    }
                    else
                    {
                        this.MoveToCameraCenter(this.GlobalCalibrationDomain.PrName);
                        double positionX = this.bondModule.BondAxisX.GetCmdPosition();
                        double positionY = this.bondModule.BondAxisY.GetCmdPosition();
                        x = point3D.X - positionX;
                        y = point3D.Y - positionY;
                    }


                    if (isFirst)
                    {
                        result.Add(x);
                    }
                    else
                    {
                        result[j] += x;
                    }
                }

                // 保存到本地
                ETelDrive.SaveMappingDataX("BondX", 0, this.GlobalCalibrationDomain.StartPoint3D.X, result, "X");

                ETelDrive.DownloadMapping(this.bondModule.BondAxisX, "X");

                if (isFirst)
                {
                    isFirst = false;
                    this.OpenXOffset();
                }
            }
        }

        /// <summary>
        /// Y轴单轴补偿
        /// </summary>
        private void ScaleY()
        {
            this.CloseYOffset();
            bool isFirst = true;
            List<double> resultY = new List<double>();
            for (int i = 0; i < this.SpCycleTimes.Value; i++)
            {
                for (int j = 0; j < this.GlobalCalibrationDomain.RowCount; j++)
                {
                    if (this.isStop)
                    {
                        return;
                    }

                    AKRSPoint3D point3D = new AKRSPoint3D(
                        this.GlobalCalibrationDomain.StartPoint3D.X,
                        this.GlobalCalibrationDomain.StartPoint3D.Y + j * this.GlobalCalibrationDomain.RowSpacing,
                        this.GlobalCalibrationDomain.StartPoint3D.Z);

                    this.MoveXY(point3D.X, point3D.Y);

                    Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                    // 寻找Pr模板
                    PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                        .Find(this.GlobalCalibrationDomain.PrName);

                    ExcuteResult re = pREntity.DoWork();

                    if (re != ExcuteResult.Success)
                    {
                        return;
                    }

                    MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                    double x = 0;
                    double y = 0;

                    if (this.CmbMoveCenterModule.SelectedIndex == 0)
                    {
                        x = (1224.0 - matchResult.CenterX) * (double)this.SpinX.Value;

                        y = -(1024.0 - matchResult.CenterY) * (double)this.SpinY.Value;
                    }
                    else
                    {
                        this.MoveToCameraCenter(this.GlobalCalibrationDomain.PrName);
                        double positionX = this.bondModule.BondAxisX.GetCmdPosition();
                        double positionY = this.bondModule.BondAxisY.GetCmdPosition();
                        x = point3D.X - positionX;
                        y = point3D.Y - positionY;
                    }

                    if (isFirst)
                    {
                        resultY.Add(y);
                    }
                    else
                    {
                        resultY[j] += y;
                    }
                }

                // 保存到本地
                ETelDrive.SaveMappingDataX("BondY", 1, this.GlobalCalibrationDomain.StartPoint3D.Y, resultY, "Y");

                ETelDrive.DownloadMapping(this.bondModule.BondAxisY, "Y");
                if (isFirst)
                {
                    isFirst = false;
                    this.OpenYOffset();
                }
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            this.CloseStage();
            this.bondModule.MoveBondXYZ(this.GlobalCalibrationDomain.StartPoint3D);
            this.MoveToCameraCenter(this.GlobalCalibrationDomain.PrName);
            this.Invoke(
                new Action(
                    () =>
                        {
                            this.SpStartX.EditValue = this.bondModule.BondAxisX.GetRealPosition();
                            this.SpStartY.EditValue = this.bondModule.BondAxisY.GetRealPosition();
                            this.SpStartZ.EditValue = this.bondModule.BondHead.AxisZ.GetRealPosition();
                        }));

            this.Save();

            this.stageAxisXMapping.Clear();
            this.stageAxisYMapping.Clear();

            for (int i = 0; i < this.GlobalCalibrationDomain.RowCount; i++)
            {
                for (int j = 0; j < this.GlobalCalibrationDomain.ColumnCount; j++)
                {
                    this.stageAxisXMapping.Add(0.0001);
                    this.stageAxisYMapping.Add(0.0001);
                }
            }

            this.result.Clear();
        }

        /// <summary>
        /// 初步补偿X
        /// </summary>
        private void PreliminaryCompensationX()
        {
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < this.GlobalCalibrationDomain.RowCount; i++)
                {
                    for (int j = 0; j < this.GlobalCalibrationDomain.ColumnCount; j++)
                    {
                        if (this.isStop)
                        {
                            return;
                        }

                        if (i == 0)
                        {
                            AKRSPoint3D point3D = new AKRSPoint3D(
                                this.GlobalCalibrationDomain.StartPoint3D.X
                                + j * this.GlobalCalibrationDomain.ColumnSpacing,
                                this.GlobalCalibrationDomain.StartPoint3D.Y
                                + i * this.GlobalCalibrationDomain.RowSpacing,
                                this.GlobalCalibrationDomain.StartPoint3D.Z);

                            this.MoveXY(point3D.X, point3D.Y);

                            Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                            // 寻找Pr模板
                            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                                .Find(this.GlobalCalibrationDomain.PrName);

                            ExcuteResult re = pREntity.DoWork(this.bondModule.GetHardware(), false);

                            if (re == ExcuteResult.Success)
                            {
                                MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                                double x = -(1224.0 - matchResult.CenterX) * (double)this.SpinX.Value * this.direct;

                                double y = (1024.0 - matchResult.CenterY) * (double)this.SpinY.Value * this.direct;

                                this.stageAxisXMapping[i * this.GlobalCalibrationDomain.ColumnCount + j] += x;

                                this.result.Add(
                                    new List<double>()
                                        {
                                            matchResult.CenterX,
                                            matchResult.CenterY,
                                            point3D.X,
                                            point3D.Y,
                                            x,
                                            y,
                                            this.stageAxisXMapping[
                                                i * this.GlobalCalibrationDomain.ColumnCount + j],
                                            this.stageAxisYMapping[
                                                i * this.GlobalCalibrationDomain.ColumnCount + j]
                                        });
                            }
                            else
                            {
                                throw new Exception("定位失败，请检查标定板是否有问题");
                            }
                        }
                        else
                        {
                            this.stageAxisXMapping[i * this.GlobalCalibrationDomain.ColumnCount + j] =
                                this.stageAxisXMapping[j];
                        }
                    }
                }

                this.SaveData(k);
            }
        }

        /// <summary>
        /// 初步补偿Y
        /// </summary>
        private void PreliminaryCompensationY()
        {
            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < this.GlobalCalibrationDomain.RowCount; i++)
                {
                    for (int j = 0; j < this.GlobalCalibrationDomain.ColumnCount; j++)
                    {
                        if (this.isStop)
                        {
                            return;
                        }

                        if (j == 0)
                        {
                            AKRSPoint3D point3D = new AKRSPoint3D(
                                this.GlobalCalibrationDomain.StartPoint3D.X
                                + j * this.GlobalCalibrationDomain.ColumnSpacing,
                                this.GlobalCalibrationDomain.StartPoint3D.Y
                                + i * this.GlobalCalibrationDomain.RowSpacing,
                                this.GlobalCalibrationDomain.StartPoint3D.Z);

                            this.MoveXY(point3D.X, point3D.Y);

                            Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                            // 寻找Pr模板
                            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                                .Find(this.GlobalCalibrationDomain.PrName);

                            ExcuteResult re = pREntity.DoWork(this.bondModule.GetHardware(), false);

                            if (re == ExcuteResult.Success)
                            {
                                MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                                double x = -(1224.0 - matchResult.CenterX) * (double)this.SpinX.Value * this.direct;

                                double y = (1024.0 - matchResult.CenterY) * (double)this.SpinY.Value * this.direct;

                                this.stageAxisYMapping[i * this.GlobalCalibrationDomain.ColumnCount + j] += y;

                                this.result.Add(
                                    new List<double>()
                                        {
                                            matchResult.CenterX,
                                            matchResult.CenterY,
                                            point3D.X,
                                            point3D.Y,
                                            x,
                                            y,
                                            this.stageAxisXMapping[
                                                i * this.GlobalCalibrationDomain.ColumnCount + j],
                                            this.stageAxisYMapping[
                                                i * this.GlobalCalibrationDomain.ColumnCount + j]
                                        });
                            }
                            else
                            {
                                throw new Exception("定位失败，请检查标定板是否有问题");
                            }
                        }
                        else
                        {
                            this.stageAxisYMapping[i * this.GlobalCalibrationDomain.ColumnCount + j] =
                                this.stageAxisYMapping[i * this.GlobalCalibrationDomain.ColumnCount];
                        }
                    }
                }

                this.SaveData(k);
            }
        }

        /// <summary>
        /// 打开补偿
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtOpenStage_Click(object sender, EventArgs e)
        {
            this.OpenStage();
        }

        /// <summary>
        /// 关闭补偿
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCloseStage_Click(object sender, EventArgs e)
        {
            this.CloseStage();
        }

        /// <summary>
        /// 保存固高数据
        /// </summary>
        private void SaveGuGaoData()
        {
            int[,] compensateX = new int[CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().RowCount,
                CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().ColumnCount];

            int[,] compensateY = new int[CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().RowCount,
                CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().ColumnCount];

            for (int i = 0; i < this.stageAxisXMapping.Count; i++)
            {
                int row = i / CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().ColumnCount;

                int column = i % CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().ColumnCount;

                compensateX[row, column] = (int)(this.stageAxisXMapping[i] * 10000.0);

                compensateY[row, column] = (int)(this.stageAxisYMapping[i] * 10000.0);
            }

            CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().CompensateX = compensateX;

            CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().CompensateY = compensateY;

            CalibSystem.GlobalCalibration.GlobalCalibrationDomain.GetInstance().Save();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="index">索引</param>
        private void SaveData(int index)
        {
            if (this.bondModule.BondAxisX.AxisDrive is ETELAxis)
            {
                this.GlobalCalibrationDomain.TextName = $"StageXY{index}";

                // 保存到本地
                ETelDrive.SaveMappingData(this.stageAxisXMapping, this.stageAxisYMapping, $"StageXY{index}");

                Thread.Sleep(100);

                ETelDrive.DownloadMapping(this.bondModule.BondAxisX, this.bondModule.BondAxisY, $"StageXY{index}");
                this.OpenStage();
            }
            else
            {
                this.SaveGuGaoData();
                this.OpenStage();
            }

            string path = FileHelper.CreateFileByDate("全局标定");
            FileHelper.SaveDoubleExcel(this.result, Path.Combine(path, DateTime.Now.ToFileTime().ToString() + ".xlsx"));
            this.result.Clear();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="index">索引</param>
        private void SaveBondCompensateData()
        {
            string path = FileHelper.CreateFileByDate("全局标定");
            FileHelper.SaveDoubleExcel(
                this.result,
                Path.Combine(path, DateTime.Now.ToFileTime().ToString() + "固晶相机标定" + ".xlsx"));
            this.result.Clear();
            this.Save();
        }

        /// <summary>
        /// 导出补偿数据
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnExportCompensateData_Click(object sender, EventArgs e)
        {
            string fileName = $"D:\\二维补偿数据\\二维补偿数据{DateTime.Now:HHmmss}";
            IDataLog dataLog = DataLogManager.Instance.CreateDataLog($"二维补偿数据{DateTime.Now:HHmmss}")
                .ConfigureFileOutput($"{fileName}.json");
            int rowCount = this.GlobalCalibrationDomain.RowCount;
            int colCount = this.GlobalCalibrationDomain.ColumnCount;
            double rowSpace = this.GlobalCalibrationDomain.RowSpacing;
            double colSpace = this.GlobalCalibrationDomain.ColumnSpacing;
            double startX = this.GlobalCalibrationDomain.StartPoint3D.X;
            double startY = this.GlobalCalibrationDomain.StartPoint3D.Y;
            int[,] compensationValX = this.GlobalCalibrationDomain.CompensateX;
            int[,] compensationValY = this.GlobalCalibrationDomain.CompensateY;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    double axisXPos = startX + j * colSpace;
                    double axisYPos = startY + i * rowSpace;
                    AKRSPoint2D axisPos = new AKRSPoint2D(axisXPos, axisYPos);
                    double compensateX = compensationValX[i, j] / 10000.0;
                    double compensateY = compensationValY[i, j] / 10000.0;
                    AKRSPoint2D compensateVal = new AKRSPoint2D(compensateX, compensateY);
                    dataLog.AddData("轴坐标", axisPos.Export());
                    dataLog.AddData("补偿值", compensateVal.Export());
                    dataLog.CommitData();
                }

                dataLog.Flush();
            }
        }

        /// <summary>
        /// 编辑焊点补偿标定相机模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnEditPRZWithXY_Click(object sender, EventArgs e)
        {
            string name = this.BondCompensate.PrName;

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Substrate;


                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            // 设置相机硬件

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 移动到中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnGetStartNewPos_Click(object sender, EventArgs e)
        {
            this.MoveToCameraCenter(this.BondCompensate.PrName);
            this.SpStartNewX.EditValue = this.bondModule.BondAxisX.GetCmdPosition();
            this.SpStartNewY.EditValue = this.bondModule.BondAxisY.GetCmdPosition();
            this.SpStartNewZ.EditValue = this.bondModule.BondHead.AxisZ.GetRealPosition();
            this.Save();
        }

        /// <summary>
        /// 自动拓展
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CkAutoScale_CheckedChanged(object sender, EventArgs e)
        {
            this.GlobalCalibrationDomain.AutoScale = this.CkAutoScale.Checked;
            this.GlobalCalibrationDomain.Save();
        }

        /// <summary>
        /// Bond相机补偿标定
        /// </summary>
        /// <param name="count">次数</param>
        private void BondCompensateCalibration(int count)
        {
            #region 数据处理

            this.bondModule.MoveBondXYZ(this.BondCompensate.StartPoint);
            this.MoveToCameraCenter(this.BondCompensate.PrName);
            this.Invoke(
                new Action(
                    () =>
                    {
                        this.SpStartNewX.EditValue = this.bondModule.BondAxisX.GetCmdPosition();
                        this.SpStartNewY.EditValue = this.bondModule.BondAxisY.GetCmdPosition();
                        this.SpStartNewZ.EditValue = this.bondModule.BondHead.AxisZ.GetCmdPosition();
                    }));

            this.Save();

            this.result.Clear();

            #endregion

            #region 工作补偿

            for (int k = 0; k < count; k++)
            {
                for (int i = 0; i < this.BondCompensate.RowCount; i++)
                {
                    for (int j = 0; j < this.BondCompensate.ColumnCount; j++)
                    {
                        if (this.isStop)
                        {
                            return;
                        }

                        AKRSPoint3D point3D = new AKRSPoint3D(
                            this.BondCompensate.StartPoint.X
                            + j * this.BondCompensate.ColumnSpacing,
                            this.BondCompensate.StartPoint.Y + i * this.BondCompensate.RowSpacing,
                            this.BondCompensate.StartPoint.Z);

                        AKRSPoint3D compensate = this.BondCompensate.GetBondCompensate(point3D.X, point3D.Y);

                        this.MoveXY(point3D.X + compensate.X, point3D.Y + compensate.Y);

                        Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                        // 寻找Pr模板
                        PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                            .Find(this.BondCompensate.PrName);

                        ExcuteResult re = pREntity.DoWork(this.bondModule.GetHardware(), false);

                        if (re == ExcuteResult.Success)
                        {
                            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                            double x = -(1224.0 - matchResult.CenterX) * (double)this.SpinX.Value * this.direct;

                            double y = (1024.0 - matchResult.CenterY) * (double)this.SpinY.Value * this.direct;

                            this.BondCompensate.BondCompensateX[i, j] += x;
                            this.BondCompensate.BondCompensateY[i, j] += y;

                            this.result.Add(
                                new List<double>()
                                    {
                                        matchResult.CenterX,
                                        matchResult.CenterY,
                                        point3D.X,
                                        point3D.Y,
                                        x,
                                        y,
                                        this.BondCompensate.BondCompensateX[i, j],
                                        this.BondCompensate.BondCompensateY[i, j],
                                        this.bondModule.BondAxisX.GetRealPosition(),
                                        this.bondModule.BondAxisY.GetRealPosition()
                                    });
                        }
                        else
                        {
                            this.result.Add(
                                new List<double>()
                                    {
                                        99999,
                                        99999,
                                        99999,
                                        99999,
                                        99999,
                                        99999,
                                        99999,
                                        99999
                                    });

                            this.BondCompensate.BondCompensateX[i, j] += 0;
                            this.BondCompensate.BondCompensateY[i, j] += 0;
                        }
                    }
                }

                this.SaveBondCompensateData();
            }

            #endregion
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitBondCompensateCalibration()
        {
            this.BondCompensate.BondCompensateX =
                new double[this.BondCompensate.RowCount, this.BondCompensate.ColumnCount];

            this.BondCompensate.BondCompensateY =
                new double[this.BondCompensate.RowCount, this.BondCompensate.ColumnCount];

            this.Save();
        }

        /// <summary>
        /// 验证补偿数据
        /// </summary>
        /// <param name="count">数量</param>
        private void BondCompensateRecordData(int count)
        {
            this.result.Clear();

            this.bondModule.MoveBondXYZ(this.BondCompensate.StartPoint);

            // 寻找Pr模板
            PREntity pREntity =
                (PREntity)VisionEntityRepository.GetInstance().Find(this.BondCompensate.PrName);

            for (int k = 0; k < count; k++)
            {
                for (int i = 0; i < this.BondCompensate.RowCount; i++)
                {
                    for (int j = 0; j < this.BondCompensate.ColumnCount; j++)
                    {
                        if (this.isStop)
                        {
                            return;
                        }

                        AKRSPoint2D point2D = new AKRSPoint2D(
                            this.BondCompensate.StartPoint.X
                            + j * this.BondCompensate.ColumnSpacing,
                            this.BondCompensate.StartPoint.Y + i * this.BondCompensate.RowSpacing);

                        AKRSPoint3D compensate = this.BondCompensate.GetBondCompensate(point2D.X, point2D.Y);

                        this.MoveXY(point2D.X + compensate.X, point2D.Y + compensate.Y);

                        Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                        // 执行定位
                        ExcuteResult p1Result = pREntity.DoWork(this.bondModule.GetHardware(), false);

                        // 定位失败，直接返回
                        if (p1Result == ExcuteResult.Success)
                        {
                            // 定位结果
                            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                            this.result.Add(
                                new List<double>()
                                    {
                                        matchResult.CenterX,
                                        matchResult.CenterY,
                                        point2D.X,
                                        point2D.Y,
                                        this.bondModule.BondAxisX.GetRealPosition(),
                                        this.bondModule.BondAxisY.GetRealPosition()
                                    });
                        }
                        else
                        {
                            this.result.Add(
                                new List<double>()
                                    {
                                        1224,
                                        1024,
                                        point2D.X,
                                        point2D.Y,
                                        99999.9999,
                                        99999.9999
                                    });
                        }
                    }
                }

                // 数据处理
                string path = FileHelper.CreateFileByDate("全局标定");

                FileHelper.SaveDoubleExcel(
                    this.result,
                    Path.Combine(path, DateTime.Now.ToFileTime().ToString() + "固晶相机标定" + ".xlsx"));

                this.result.Clear();
            }
        }
    }
}