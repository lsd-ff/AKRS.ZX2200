using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Controls.Setting.EpoxyApplication
{
    using Accord.IO;
    using AKRS.Galaxy2.Dispense;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.Controls.ToolControls.Programming;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using DevExpress.XtraEditors;
    using DevExpress.XtraPrinting;
    using LanguageExt;
    using LanguageExt.Pipes;
    using log4net.Core;
    using PostSharp.Extensibility;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// 画胶设置
    /// </summary>
    public partial class UcEpoxyApplication : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 当前所用的画胶设置页面
        /// </summary>
        public EpoxyApplication EpoxyApplication { get; set; }

        /// <summary>
        /// 画胶页面
        /// </summary>
        public EpoxyPatternForm EpoxyPatternForm { get; set; }

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        private readonly DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();    

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="epoxyApplication">画胶应用</param>
        public UcEpoxyApplication(EpoxyApplication epoxyApplication)
        {
            this.InitializeComponent();
            this.EpoxyApplication = epoxyApplication;
        }

        /// <summary>
        /// 选中的基板
        /// </summary>
        public string SelectSubstrate { get; set; }

        /// <summary>
        /// 选中的基岛
        /// </summary>
        public string SelectModule { get; set; }

        /// <summary>
        /// 选中的焊点
        /// </summary>
        public string SelectBondPosition { get; set; }

        /// <summary>
        /// 加载方法
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcEpoxyApplication_Load(object sender, EventArgs e)
        {
            this.Init();
            this.RefreshDispenseLineControl();

            if (MachineHardwareConfiguration.GetInstance().IsSystem1ConfigPrintingTool)
            {
                this.groupControl6.Enabled = true;
            }

            this.SpSizeX.EditValueChanged += this.EditValueChanged;
            this.SpSizeY.EditValueChanged += this.EditValueChanged;
            this.SpComponentX.EditValueChanged += this.EditValueChanged;
            this.SpComponentY.EditValueChanged += this.EditValueChanged;
            this.SpAngle.EditValueChanged += this.EditValueChanged;
        }

        /// <summary>
        /// 显示控件
        /// </summary>
        private EpoxyPatternControl epoxyPatternControl;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (this.epoxyPatternControl == null)
            {
                this.epoxyPatternControl = new EpoxyPatternControl { Dock = DockStyle.Fill };
            }

            this.PnlEpoxyPatternControl.Controls.Add(this.epoxyPatternControl);

            // 这个是不知道什么意思
            this.GpDispenseOptions.Enabled = false;

            this.CkTearOff1On.Checked = this.EpoxyApplication.TearOff1Open;

            if (!this.CkTearOff1On.Checked)
            {
                this.SpTearOff1Height.Enabled = false;
                this.SpTearOff1Delay.Enabled = false;
                this.SpTearOff1Speed.Enabled = false;
            }

            this.CkTearOff2On.Checked = this.EpoxyApplication.TearOff2Open;

            if (!this.CkTearOff2On.Checked)
            {
                this.SpTearOff2Height.Enabled = false;
                this.SpTearOff2Delay.Enabled = false;
                this.SpTearOff2Speed.Enabled = false;
            }

            var dataSource = EnumHelper.ConvertEnumToNameDisplayDto<EpoxyApplicationTypeEnum>();

            if (!MachineHardwareConfiguration.GetInstance().IsSystem1ConfigPrintingTool
               && !MachineHardwareConfiguration.GetInstance().IsSystem2ConfigPrintingTool
               )
            {
                dataSource = dataSource.Where(d => d.Display != "蘸胶").ToList();

                if (this.EpoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting)
                {
                    this.EpoxyApplication.EpoxyApplicationStrategy = EpoxyApplicationTypeEnum.SingleDotOnly;
                }
            }

            this.LueEpoxyType.Properties.DataSource = dataSource;

            this.LueEpoxyType.EditValue = this.EpoxyApplication.EpoxyApplicationStrategy;

            this.SpTearOff1Height.EditValue = this.EpoxyApplication.TearOff1Height;
            this.SpTearOff1Speed.EditValue = this.EpoxyApplication.TearOff1Speed;
            this.SpTearOff1Delay.EditValue = this.EpoxyApplication.TearOff1Delay;

            this.SpTearOff2Height.EditValue = this.EpoxyApplication.TearOff2Height;
            this.SpTearOff2Speed.EditValue = this.EpoxyApplication.TearOff2Speed;
            this.SpTearOff2Delay.EditValue = this.EpoxyApplication.TearOff2Delay;

            this.SpSecurityHeightForEpoxyApplication.EditValue = this.EpoxyApplication.SecurityHeightForEpoxyApplication;
            this.SpSlowTravel.EditValue = this.EpoxyApplication.SlowTravelSpeedBeforeDispensing;
            this.SpWaitingTime.EditValue = this.EpoxyApplication.DispenserLeadTime;
            this.ChWaitIsDispense.Checked = this.EpoxyApplication.IsDispenserLeadTimeOpen;

            this.SpSpindleSpeed.EditValue = this.EpoxyApplication.SpindleSpeedPercentage;
            this.SpOffSetX.EditValue = this.EpoxyApplication.OffsetX;
            this.SpOffSetY.EditValue = this.EpoxyApplication.OffsetY;
            this.SpOffSetZ.EditValue = this.EpoxyApplication.OffsetZ;
            this.SpAdvance.EditValue = this.EpoxyApplication.AdvanceOpenDistanceNew;
            this.SpPrePlantOffSetZ.EditValue = this.EpoxyApplication.PrePlantOffSetZ;

            this.SpSizeX.EditValue = this.EpoxyApplication.SizeX;
            this.SpSizeY.EditValue = this.EpoxyApplication.SizeY;

            this.SpComponentX.EditValue = this.EpoxyApplication.ComponentSizeX;
            this.SpComponentY.EditValue = this.EpoxyApplication.ComponentSizeY;
            this.SpAngle.EditValue = this.EpoxyApplication.Angle;

            this.SpDispensingPressure.EditValue = this.EpoxyApplication.DispensePressure;

            this.SpVacuum.EditValue = this.EpoxyApplication.Vacuum;

            this.SpPrintSlowDownHeight.EditValue = this.EpoxyApplication.PrintSlowDownHeight;
            this.SpPrintSlowDownSpeed.EditValue = this.EpoxyApplication.PrintSlowDownSpeed;
            this.SpPrintDelay.EditValue = this.EpoxyApplication.PrintDelayTime;
            this.SpPrintingOffset.EditValue = this.EpoxyApplication.PrintOffsetZ;

            this.SpPrintSlowUpHeight.EditValue = this.EpoxyApplication.PrintSlowUpHeight;
            this.SpPrintSlowUpSpeed.EditValue = this.EpoxyApplication.PrintSlowUpSpeed;

            this.SpPrintSlowUpDelay.EditValue = this.EpoxyApplication.PrintSlowUpDelayTime;

            this.SpDispensingPressure.Properties.MaxValue = DispenseDevicePara.GetInstance().DispenseModulePara.DispenserMaxValue;

            if (MachineHardwareConfiguration.GetInstance().IsSystem2ConfigPrintingTool)
            {
                foreach (var item in System2Domain.GetInstance().BondProgram.NozzleShelfProgram.GetNozzleNameList())
                {
                    this.CmbNozzleName.Properties.Items.Add(item);
                }

                this.CmbNozzleName.Properties.Items.Add("Null");

                this.CmbNozzleName.SelectedItem = this.EpoxyApplication.PrintNozzleName;
            }
            else
            {
                this.CmbNozzleName.Enabled = false;
            }

            this.InitTransportUnit();
        }

        /// <summary>
        /// 编辑画胶
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtEditPattern_Click(object sender, EventArgs e)
        {
            // 非空判断
            if (this.EpoxyPatternForm == null || this.EpoxyPatternForm.IsDisposed)
            {
                this.EpoxyPatternForm = new EpoxyPatternForm();
            }

            // 数据转换
            List<EPEPointData[]> list = this.ChangeListPosToPatternData(this.EpoxyApplication);
           
            // 图片矩阵，后续再研究怎么去实现
            float[] imageMatrixElements = new float[] { 1, 0, 0, 1, 0, 0 };

            EPEData epeData = new EPEData(
                1,    // 像素比
                3500, // um
                4200, // um
                list, // 胶路集合
                new Bitmap(500, 500), // 背景图片
                imageMatrixElements);

            RetryCommand:
            // 往编辑页面传画胶数据
            this.EpoxyPatternForm.SetContent(epeData);

            DialogResult dialogResult = this.EpoxyPatternForm.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.EpoxyApplication.DispensePatternParas.Clear();

                // 获取数据
                epeData = this.EpoxyPatternForm.GetContent();

                this.InitPatternData(epeData);

                if (!this.EpoxyApplication.EpoxyIsReasonable)
                {
                    goto RetryCommand;
                }
            }

            this.EpoxyPatternForm.Dispose();
            this.EpoxyPatternForm = null;
            this.RefreshDispenseLineControl();
        }

        /// <summary>
        /// 将点位集合转化成画胶图形
        /// </summary>
        /// <param name="epoxyApplication">画胶图形</param>
        /// <returns>是否成功</returns>
        public List<EPEPointData[]> ChangeListPosToPatternData(EpoxyApplication epoxyApplication)
        {
            List<EPEPointData[]> list = new List<EPEPointData[]>();

            if (epoxyApplication.DispensePatternParas != null && epoxyApplication.DispensePatternParas.Count > 0)
            {
                foreach (EpoxyApplicationLocation[] epoxyApplicationLocations in epoxyApplication.DispensePatternParas)
                {
                    EPEPointData[] epePointDatas = new EPEPointData[epoxyApplicationLocations.Length];

                    for (int j = 0; j < epoxyApplicationLocations.Length; j++)
                    {
                        EndpointSetting endpointInfo = new EndpointSetting
                        {
                            Acc = epoxyApplicationLocations[j].Acc,
                            AltitudeCompensation = (int)epoxyApplicationLocations[j].AltitudeCompensation,
                            Dummied = !epoxyApplicationLocations[j].OpenGlueFlag,
                            MaxSpeed = (int)epoxyApplicationLocations[j].Speed,
                            EndSpeed = (int)epoxyApplicationLocations[j].EndSpeed,
                            Time = epoxyApplicationLocations[j].DripTime,
                            PreDelay = epoxyApplicationLocations[j].AdvanceClose,
                            PostDelay = epoxyApplicationLocations[j].LagOpen,
                            HeightMeasurement = false
                        };

                        EPEPointData epePoint = new EPEPointData(
                            (float)epoxyApplicationLocations[j].X * 1000,
                            (float)epoxyApplicationLocations[j].Y * 1000,
                            endpointInfo);
                        epePointDatas[j] = epePoint;
                    }

                    list.Add(epePointDatas);
                }
            }

            return list;
        }

        /// <summary>
        /// 将画胶后的图形转为点集合
        /// </summary>
        /// <param name="epeData">画胶参数</param>
        public void InitPatternData(EPEData epeData)
        {
            // 清空数据
            this.EpoxyApplication.DispensePatternParas.Clear();

            List<EPEPointData[]> list = epeData.EpePointList;

            for (int i = 0; i < list.Count; i++)
            {
                EpoxyApplicationLocation[] dispenseLocations = new EpoxyApplicationLocation[epeData.EpePointList[i].Length];
                for (int j = 0; j < epeData.EpePointList[i].Length; j++)
                {
                    EpoxyApplicationLocation dispenseLocation = new EpoxyApplicationLocation
                        {
                            X = epeData.EpePointList[i][j].X / 1000,
                            Y = epeData.EpePointList[i][j].Y / 1000,
                            Acc = epeData.EpePointList[i][j].Info.Acc,
                            Speed = epeData.EpePointList[i][j].Info.MaxSpeed,
                            AltitudeCompensation = epeData.EpePointList[i][j].Info.AltitudeCompensation,
                            Dec = epeData.EpePointList[i][j].Info.Acc,
                            DripTime = epeData.EpePointList[i][j].Info.Time,
                            AdvanceClose = epeData.EpePointList[i][j].Info.PreDelay,
                            LagOpen = epeData.EpePointList[i][j].Info.PostDelay,
                            OpenGlueFlag = !epeData.EpePointList[i][j].Info.Dummied
                        };

                    dispenseLocations[j] = dispenseLocation;
                }

                this.EpoxyApplication.DispensePatternParas.Add(dispenseLocations);
            }

            DispenserMoveHelper.EpoxyPretreatment(this.EpoxyApplication, new AKRSPoint3D(), 0);

            this.Refresh();
        }

        /// <summary>
        /// 刷新窗体
        /// </summary>
        private void RefreshDispenseLineControl()
        {
            // 数据转换
            List<EPEPointData[]> list = this.ChangeListPosToPatternData(this.EpoxyApplication);

            bool autoSize = this.EpoxyApplication.ComponentSizeX != 0
                && this.EpoxyApplication.ComponentSizeY != 0
                && list.Count != 0
                && list.Count > 1;

            if (autoSize)
            {
                // 找出list里面最大的X和Y
                double sizeMaxX = 0;
                double sizeMinX = 0;
                double sizeMaxY = 0;
                double sizeMinY = 0;

                // 找出最大值和最小值，为归一化做准备
                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].GetLength(0); j++)
                    {
                        Point2D location = list[i][j].Point;
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

                if (sizeMaxX == 0 || sizeMaxY == 0)
                {
                    sizeMaxX = 1;
                    sizeMaxY = 1;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].GetLength(0); j++)
                    {
                        Point2D location = list[i][j].Point;

                        location.X = (float)((location.X - offsetX) * 1.0/*this.EpoxyApplication.SizeX*/ / sizeMaxX / 2.0) * 100F;

                        location.Y = (float)((location.Y - offsetY) * 1.0/*this.EpoxyApplication.SizeY*/ / sizeMaxY / 2.0) * 100F;

                        list[i][j].Point = location;
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    for (int j = 0; j < list[i].GetLength(0); j++)
                    {
                        double x = list[i][j].X * 3 * this.EpoxyApplication.SizeX / this.EpoxyApplication.ComponentSizeX;
                        double y = list[i][j].Y * 3 * this.EpoxyApplication.SizeY / this.EpoxyApplication.ComponentSizeY;
                        list[i][j].Point = new Point2D((float)x, (float)y);
                    }
                }
            }

            // 图片矩阵，后续再研究怎么去实现
            float[] imageMatrixElements = new float[] { 1, 0, 0, 1, 0, 0 };

            EPEData epeData = new EPEData(
                1, // 像素比
                300,
                300,
                list,
                new Bitmap(200, 200),
                imageMatrixElements);

            float diePixelWidth = epeData.DieWidth / epeData.MicronPerPixel;
            float diePixelHeight = epeData.DieHeight / epeData.MicronPerPixel;

            if (autoSize)
            {
                this.epoxyPatternControl.SetDieSize(diePixelWidth, diePixelHeight);
            }
            else 
            {
                this.epoxyPatternControl.SetDieSize(0, 0);
            }

            this.epoxyPatternControl.SetDefaultSegmentEndPointInfo(epeData.DefaultInfo);

            this.epoxyPatternControl.SetFigure(epeData.EpePointList);
            this.epoxyPatternControl.BackgroundImageInfo = epeData.BackgroundImageInfo;
        }

        /// <summary>
        /// 保存现有参数
        /// </summary>
        public void Confirm()
        {
            this.EpoxyApplication.TearOff1Open = this.CkTearOff1On.Checked;

            this.EpoxyApplication.TearOff2Open = this.CkTearOff2On.Checked;

            this.EpoxyApplication.TearOff1Height = (double)this.SpTearOff1Height.Value;
            this.EpoxyApplication.TearOff1Speed = (double)this.SpTearOff1Speed.Value;
            this.EpoxyApplication.TearOff1Delay = (int)this.SpTearOff1Delay.Value;

            this.EpoxyApplication.TearOff2Height = (double)this.SpTearOff2Height.Value;
            this.EpoxyApplication.TearOff2Speed = (double)this.SpTearOff2Speed.Value;
            this.EpoxyApplication.TearOff2Delay = (int)this.SpTearOff2Delay.Value;

            this.EpoxyApplication.SecurityHeightForEpoxyApplication =
                (double)this.SpSecurityHeightForEpoxyApplication.Value;
            this.EpoxyApplication.SlowTravelSpeedBeforeDispensing = (double)this.SpSlowTravel.Value;

            this.EpoxyApplication.DispenserLeadTime = (int)this.SpWaitingTime.Value;
            this.EpoxyApplication.IsDispenserLeadTimeOpen = this.ChWaitIsDispense.Checked;
            
            this.EpoxyApplication.SpindleSpeedPercentage = (int)this.SpSpindleSpeed.Value;
            this.EpoxyApplication.OffsetX = (double)this.SpOffSetX.Value;
            this.EpoxyApplication.OffsetY = (double)this.SpOffSetY.Value;
            this.EpoxyApplication.OffsetZ = (double)this.SpOffSetZ.Value;
            this.EpoxyApplication.AdvanceOpenDistanceNew = (double)this.SpAdvance.Value;
            this.EpoxyApplication.PrePlantOffSetZ = (double)this.SpPrePlantOffSetZ.Value;

            this.EpoxyApplication.SizeX = (double)this.SpSizeX.Value;
            this.EpoxyApplication.SizeY = (double)this.SpSizeY.Value;
            this.EpoxyApplication.Angle = (double)this.SpAngle.Value;

            this.EpoxyApplication.ComponentSizeX = (double)this.SpComponentX.Value;
            this.EpoxyApplication.ComponentSizeY = (double)this.SpComponentY.Value;

            this.EpoxyApplication.DispensePressure = (double)this.SpDispensingPressure.Value;

            this.EpoxyApplication.Vacuum = (double)this.SpVacuum.Value;

            this.EpoxyApplication.EpoxyApplicationStrategy = (EpoxyApplicationTypeEnum)this.LueEpoxyType.EditValue;

            this.EpoxyApplication.PrintSlowDownHeight = (double)this.SpPrintSlowDownHeight.Value;
            this.EpoxyApplication.PrintSlowDownSpeed = (double)this.SpPrintSlowDownSpeed.Value;
            this.EpoxyApplication.PrintDelayTime = (int)this.SpPrintDelay.Value;
            this.EpoxyApplication.PrintOffsetZ = (double)this.SpPrintingOffset.Value;
            this.EpoxyApplication.PrintSlowUpHeight = (double)this.SpPrintSlowUpHeight.Value;
            this.EpoxyApplication.PrintSlowUpSpeed = (double)this.SpPrintSlowUpSpeed.Value;
            this.EpoxyApplication.PrintSlowUpDelayTime = (int)this.SpPrintSlowUpDelay.Value;

            string printNozzleName = this.CmbNozzleName.SelectedItem?.ToString();

            // 判断是否为空
            if (string.IsNullOrEmpty(printNozzleName))
            {
                this.EpoxyApplication.PrintNozzleName = "Null";
            }
            else
            {
                this.EpoxyApplication.PrintNozzleName = printNozzleName;
            }

            EpoxyApplicationRepository.GetInstance().Save();
            System1Program.GetInstance().Save();
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void CkTearOff1On_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.CkTearOff1On.Checked)
            { 
                this.SpTearOff1Height.Enabled = false;
                this.SpTearOff1Delay.Enabled = false;
                this.SpTearOff1Speed.Enabled = false;
            }
            else
            {
                this.SpTearOff1Height.Enabled = true;
                this.SpTearOff1Delay.Enabled = true;
                this.SpTearOff1Speed.Enabled = true;
            }
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void CkTearOff2On_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.CkTearOff2On.Checked)
            {
                this.SpTearOff2Height.Enabled = false;
                this.SpTearOff2Delay.Enabled = false;
                this.SpTearOff2Speed.Enabled = false;
            }
            else
            {
                this.SpTearOff2Height.Enabled = true;
                this.SpTearOff2Delay.Enabled = true;
                this.SpTearOff2Speed.Enabled = true;
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtTest_Click(object sender, EventArgs e)
        {
            try
            {
                this.Confirm();
                bool isDispenseInsystem2 = MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2
              && (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                  || (MachineHardwareConfiguration.GetInstance().IsSystem2ConfigPrintingTool && this.EpoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting));

                if (isDispenseInsystem2)
                {
                    #region 条件验证
                    TransportUnit transportUnit =
                    TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

                    if (this.CmbBondPositon.SelectedItem == null)
                    {
                        AKRSXtraMessageBox.Show("请先选择焊点");
                        return;
                    }

                    // 判断点胶头是否准备好
                    if (this.EpoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting)
                    {
                        Nozzle nozzle = System2Domain.GetInstance().BondProgram.NozzleShelfProgram.GetNozzleList().Find(it => it.Name == this.EpoxyApplication.PrintNozzleName);
                        if (nozzle == null)
                        {
                            AKRSXtraMessageBox.Show("蘸胶吸嘴为空，请重新选择");
                            return;
                        }
                        else if (!nozzle.IsAssistantSucceed)
                        {
                            AKRSXtraMessageBox.Show("蘸胶吸嘴未示教完成，请示教完后重试");
                            return;
                        }
                    }
                    else
                    {
                        // 如果点胶头没有示教完成，退出
                        if (!BondProgram.GetInstance().S2DispenserProgram.IsReady())
                        {
                            return;
                        }
                    }

                    if (transportUnit == null)
                    {
                        AKRSXtraMessageBox.Show("系统2载台上没有载具，请重试");
                        return;
                    }

                    transportUnit = new TransportUnit(CurrentMachineSystemEnum.System2);

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"生产产品完成");

                    #endregion

                    #region 按照配置执行定位和测高

                    #region substrate

                    List<Substrate> substrates = transportUnit.Substrates;
                    Substrate substrate = substrates.Find(it => it.Name == this.CmbSubstrateName.SelectedItem?.ToString());

                    if (substrate == null)
                    {
                        AKRSXtraMessageBox.Show($"未找到:{this.CmbSubstrateName.SelectedItem?.ToString()},请重新选择");
                        return;
                    }

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"寻找{substrate?.Name}");

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"寻找到{substrate.Name}");

                    if (!System2Domain.GetInstance().System2MatterVision(substrate, false))
                    {
                        return;
                    }

                    if (System2Domain.GetInstance().MatterHeightMeasurePoints(substrate) != ExcuteResult.Success)
                    {
                        return;
                    }

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"基板{substrate.Name}矫正完成");

                    #endregion

                    #region module

                    Module module = substrate.Modules.Find(it => it.Name == this.CmbModuleName.SelectedItem.ToString());

                    if (module == null)
                    {
                        AKRSXtraMessageBox.Show($"未找到:{this.CmbModuleName.SelectedItem.ToString()},请重新选择");
                        return;
                    }

                    if (!System2Domain.GetInstance().System2MatterVision(module, false))
                    {
                        return;
                    }

                    if (System2Domain.GetInstance().MatterHeightMeasurePoints(module) != ExcuteResult.Success)
                    {
                        return;
                    }

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"基岛{module.Name}矫正完成");

                    #endregion

                    #region bondPosition

                    BondPosition bondPosition =
                       module.BondPositions.Find(it => it.Name == (string)this.CmbBondPositon.SelectedItem);

                    if (bondPosition == null)
                    {
                        AKRSXtraMessageBox.Show($"未找到:{this.CmbModuleName.SelectedItem.ToString()},请重新选择");
                        return;
                    }

                    if (!System2Domain.GetInstance().System2MatterVision(bondPosition, false))
                    {
                        return;
                    }

                    if (System2Domain.GetInstance().MatterHeightMeasurePoints(bondPosition) != ExcuteResult.Success)
                    {
                        return;
                    }

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"焊点{bondPosition.Name}矫正完成");
                    #endregion

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"对象矫正完成");

                    #endregion

                    #region 预点胶

                    if (this.EpoxyApplication.EpoxyApplicationStrategy != EpoxyApplicationTypeEnum.Printting)
                    {
                        System2Domain.GetInstance().BondActionNodeRepository.S2PreDispenseActionNode.DoWork();
                        System2RunTimeProvider.RecordTime("Bond点胶测试", $"预点胶完成");
                    }

                    #endregion

                    #region 点胶并查看效果

                    AKRSPoint3D point = bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D(this.EpoxyApplication.OffsetX, this.EpoxyApplication.OffsetY, 0));

                    point += bondPosition.GetDispensePositionCompensate();

                    // 空跑模式不出胶
                    bool isDrip = MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle;

                    double angle = bondPosition.CoordinateSystem.DegreeInG0() * 180.0 / Math.PI + bondPosition.GetDispenseRotaryCompensate();

                    // 如果是蘸胶头则更换吸嘴
                    if (this.EpoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting)
                    {
                        // 换吸嘴动作
                        bool ret = System2Domain.GetInstance().System2Controller.JudgeAndChangeNozzle(this.EpoxyApplication.PrintNozzleName);
                        if (!ret)
                        {
                            return;
                        }
                    }

                    // 执行点胶
                    if (System2Domain.GetInstance().S2DispenseController.InterpolationApplication(this
                        .EpoxyApplication, point, isDrip, false, false, angle) != ExcuteResult.Success)
                    {
                        return;
                    }

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"点/画胶胶完成");

                    System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();

                    System2Domain.GetInstance().S2DispenseController.VisionMoveToG0Pos(point);

                    UcMainSystem.VmVisionShow();

                    System2RunTimeProvider.RecordTime("Bond点胶测试", $"Bond点胶完成");

                    #endregion
                }
                else
                {
                    #region 条件验证
                    
                    TransportUnit transportUnit =
                    TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit;

                    if (this.CmbBondPositon.SelectedItem == null)
                    {
                        AKRSXtraMessageBox.Show("请先选择焊点");
                        return;
                    }

                    // 如果点胶头没有示教完成，退出
                    if (!System1Domain.GetInstance().System1Program.DispenserProgram.IsReady())
                    {
                        return;
                    }

                    if (transportUnit == null)
                    {
                        AKRSXtraMessageBox.Show("系统1载台上没有载具，请重试");
                        return;
                    }

                    transportUnit = new TransportUnit(CurrentMachineSystemEnum.System1);

                    transportUnit.SetSystem1OffSet();

                    #endregion

                    #region 按照配置执行定位和测高

                    Substrate substrate = transportUnit.Substrates.Find(it => it.Name == this.CmbSubstrateName.SelectedItem.ToString());

                    // 如果是分离的
                    if (transportUnit.TransportUnitConfig.SubstrateProcessing == SubstrateProcessingEnum.Divide)
                    {
                        // 如果在点胶前半段
                        if (TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.IsTuInDispense1)
                        {
                            // 判断当前是否在前半段
                            if (!ProductConfiguration.GetInstance().SubstrateConfig.GetBackSubstrateIndex().Contains(substrate.Index))
                            {
                                AKRSXtraMessageBox.Show("请先传送载具到点胶2平台", "提示");
                                return;
                            }
                        }
                        else
                        {
                            // 判断当前是否在前半段
                            if (!ProductConfiguration.GetInstance().SubstrateConfig.GetFrontSubstrateIndex().Contains(substrate.Index))
                            {
                                AKRSXtraMessageBox.Show("载具在点胶2平台,无法执行.请先将载具移动到点胶1平台", "提示");
                                return;
                            }

                            transportUnit.System1MoveDistance();
                        }
                    }

                    if (!System1Domain.GetInstance().System1MatterVision(substrate,false))
                    {
                        return;
                    }

                    if (System1Domain.GetInstance().MatterHeightMeasurePoints(substrate, true) != ExcuteResult.Success)
                    {
                        return;
                    }

                    Module module = substrate.Modules.Find(it => it.Name == this.CmbModuleName.SelectedItem.ToString());

                    if (!System1Domain.GetInstance().System1MatterVision(module, false))
                    {
                        return;
                    }

                    if (System1Domain.GetInstance().MatterHeightMeasurePoints(module, true) != ExcuteResult.Success)
                    {
                        return;
                    }

                    BondPosition bondPosition =
                        module.BondPositions.Find(it => it.Name == (string)this.CmbBondPositon.SelectedItem);

                    if (!System1Domain.GetInstance().System1MatterVision(bondPosition, false))
                    {
                        return;
                    }

                    if (System1Domain.GetInstance().MatterHeightMeasurePoints(bondPosition, true) != ExcuteResult.Success)
                    {
                        return;
                    }

                    #endregion

                    #region 预点胶

                    System1Domain.GetInstance().DispenseActionNodes.PreDispenseAction.DoWork();

                    #endregion

                    #region 点胶并查看效果

                    AKRSPoint3D dispensePoint3D = bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D(this.EpoxyApplication.OffsetX, this.EpoxyApplication.OffsetY, 0));

                    dispensePoint3D += bondPosition.GetDispensePositionCompensate() + DispenseDevicePara.GetInstance().DispenseModulePara.DispenseOffset;

                    double angle = bondPosition.CoordinateSystem.DegreeInG0() * 180.0 / Math.PI + bondPosition.GetDispenseRotaryCompensate();

                    // 空跑模式不出胶
                    bool isDrip = MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle;

                    // 执行点胶
                    if (System1Domain.GetInstance().DispenseController.InterpolationApplication(this.EpoxyApplication, dispensePoint3D, isDrip, false, angle) != ExcuteResult.Success)
                    {
                        return;
                    }

                    AKRSPoint3D visionPos = System1Domain.GetInstance().DispenseController.GetG0VisionPos(dispensePoint3D);

                    System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(visionPos);

                    UcMainSystem.VmVisionShow();
                }

                #endregion
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show("Dispense error \r\n" + exception.StackTrace);
                LogHelper.Post(Level.Error, $"Dispense error",exception, LogCategory.MainSoftWare, ViewType.InFileAndUI);
                throw;
            }
        }

        /// <summary>
        /// 初始化产品选项
        /// </summary>
        public void InitTransportUnit()
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                foreach (OppositeSex oppositeSex in ProductConfiguration.GetInstance().OppositeSexConfiguration.GetAllSubstrateConfigs())
                {
                    this.CmbSubstrateName.Properties.Items.Add(oppositeSex.Name);
                }

                // 自动选择选中的配置文件
                this.CmbSubstrateName.SelectedItem = this.SelectSubstrate;
                this.CmbModuleName.SelectedItem = this.SelectModule;
                this.CmbBondPositon.SelectedItem = this.SelectBondPosition;
            }
            else
            {
                for (int i = 0; i < ProductConfiguration.GetInstance().SubstrateConfig.Count; i++)
                {
                    this.CmbSubstrateName.Properties.Items.Add($"基板{i + 1}");
                    this.CmbSubstrateName.SelectedItem = $"基板{1}";
                }

                for (int i = 0; i < ProductConfiguration.GetInstance().ModuleConfig.Count; i++)
                {
                    this.CmbModuleName.Properties.Items.Add($"基岛{i + 1}");
                    this.CmbModuleName.SelectedItem = $"基岛{1}";
                }

                foreach (SingleBondPositionConfig singleBondPositionConfig in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
                {
                    this.CmbBondPositon.Properties.Items.Add(singleBondPositionConfig.Name);
                    this.CmbBondPositon.SelectedItem = ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList[0]?.Name;
                }
            }
        }

        /// <summary>
        /// 在预点胶板上点画胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtTestInPrePlant_Click(object sender, EventArgs e)
        {
            this.Confirm();

            DispenseRunTimeProvider.IsFirstDispense = true;

            bool isDispenseInsystem2 = MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2
                && (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense 
                    || (MachineHardwareConfiguration.GetInstance().IsSystem2ConfigPrintingTool && this.EpoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting));

            if (isDispenseInsystem2)
            {
                if (this.EpoxyApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting)
                {
                    Nozzle nozzle = System2Domain.GetInstance().BondProgram.NozzleShelfProgram.GetNozzleList().Find(it => it.Name == this.EpoxyApplication.PrintNozzleName);
                    if (nozzle == null)
                    {
                        AKRSXtraMessageBox.Show("蘸胶吸嘴为空，请重新选择");
                        return;
                    }
                    else if (!nozzle.IsAssistantSucceed)
                    {
                        AKRSXtraMessageBox.Show("蘸胶吸嘴未示教完成，请示教完后重试");
                        return;
                    }

                    // 换吸嘴动作
                    bool ret = System2Domain.GetInstance().System2Controller.JudgeAndChangeNozzle(this.EpoxyApplication.PrintNozzleName);
                    if (!ret)
                    {
                        return;
                    }
                }
                else
                {
                    if (BondProgram.GetInstance().S2DispenserProgram.Dispenser == null
                      || BondProgram.GetInstance().S2DispenserProgram.Dispenser.Name == string.Empty)
                    {
                        AKRSXtraMessageBox.Show("请先配置点胶头");
                        return;
                    }
                }

                // 如果预点胶板没有完成示教，退出
                if (!BondProgram.GetInstance().S2PreDispensePlateProgram.IsReady())
                {
                    AKRSXtraMessageBox.Show("系统2预点胶板未示教完成");
                    return;
                }

                // 获取位置
                AKRSPoint3D point3D = BondProgram.GetInstance().S2PreDispensePlateProgram
                    .GetEpoxy();

                if (point3D == null)
                {
                    return;
                }

                // 点/画胶
                System2Domain.GetInstance().S2DispenseController.InterpolationApplication(this.EpoxyApplication, point3D, true, true, false, 0);

                System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();

                point3D.Z += this.EpoxyApplication.PrePlantOffSetZ;

                System2Domain.GetInstance().S2DispenseController.VisionMoveToG0Pos(point3D);

                UcMainSystem.VmVisionShow();

                UcMainSystem.ChangeCameraVision(CameraEnum.BondCamera.GetDescription());
            }
            else
            {
                if (System1Program.GetInstance().DispenserProgram.Dispenser == null
                    || System1Program.GetInstance().DispenserProgram.Dispenser.Name == string.Empty)
                {
                    AKRSXtraMessageBox.Show("请先配置点胶头");
                    return;
                }

                if (!System1Domain.GetInstance().System1Program.PreDispensePlateProgram.IsReady())
                {
                    AKRSXtraMessageBox.Show("系统1预点胶板位置未示教完成，请先示教");
                    return;
                }

                AKRSPoint3D point3D = System1Domain.GetInstance().System1Program.PreDispensePlateProgram
                    .GetEpoxy();

                if (point3D == null)
                {
                    return;
                }

                System1Domain.GetInstance().DispenseController.InterpolationApplication(
                    this.EpoxyApplication,
                    point3D,
                    true,
                    true,
                    0);

                System1Domain.GetInstance().DispenseController.MoveZToSafePos();

                AKRSPoint3D visionPos = System1Domain.GetInstance().DispenseController.GetG0VisionPos(point3D);

                visionPos.Z += this.EpoxyApplication.PrePlantOffSetZ;

                System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(visionPos);

                UcMainSystem.VmVisionShow();
                UcMainSystem.ChangeCameraVision(CameraEnum.DispenseCamera.GetDescription());
            }
        }

        /// <summary>
        /// 当Sub的名称发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbSubstrateName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                OppositeSex oppositeSexSub = ProductConfiguration.GetInstance().OppositeSexConfiguration.GetAllSubstrateConfigs()
                    .Find(it => it.Name == this.CmbSubstrateName.SelectedItem.ToString());

                this.CmbModuleName.Properties.Items.Clear();

                foreach (OppositeSex oppositeSex in oppositeSexSub.DownConfigs)
                {
                    this.CmbModuleName.Properties.Items.Add(oppositeSex.Name);
                }
            }
        }

        /// <summary>
        /// 当Sub的名称发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbModuleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                OppositeSex oppositeSexSub = ProductConfiguration.GetInstance().OppositeSexConfiguration.GetAllModuleConfigs()
                    .Find(it => it.Name == this.CmbModuleName.SelectedItem.ToString());

                OppositeSex oppositeSexModule = oppositeSexSub.DownConfigs
                    .Find(it => it.Name == this.CmbModuleName.SelectedItem.ToString());
                foreach (OppositeSex oppositeSex in oppositeSexSub.DownConfigs)
                {
                    this.CmbBondPositon.Properties.Items.Add(oppositeSex.Name);
                }
            }
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbNozzleName_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueEpoxyType_EditValueChanged(object sender, EventArgs e)
        {
            this.CmbNozzleName.Enabled = (EpoxyApplicationTypeEnum)this.LueEpoxyType.EditValue == EpoxyApplicationTypeEnum.Printting;
            this.groupControl6.Enabled = (EpoxyApplicationTypeEnum)this.LueEpoxyType.EditValue == EpoxyApplicationTypeEnum.Printting;
        }

        private void EditValueChanged(object sender, EventArgs e)
        {
            this.Confirm();
            this.RefreshDispenseLineControl();
        }
    }
}
