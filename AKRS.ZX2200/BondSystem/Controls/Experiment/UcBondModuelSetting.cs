using AKRS.ZX2200.Infrastructure.Service;
using DataAnalysis.Acquisition;
using System.Linq;

namespace AKRS.ZX2200.BondSystem.Controls
{
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LaserInterferometerEncoderControllers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.BondForce.Controllers;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls.Experiment;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.Control;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Parameter;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using ch.etel.edi.dsa.v40;
    using DevExpress.DataAccess.Native;
    using DevExpress.XtraEditors;
    using EasyModbus;
    using log4net.Core;
    using Newtonsoft.Json;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using LicenseContext = OfficeOpenXml.LicenseContext;

    /// <summary>
    /// 调试用
    /// </summary>
    public partial class UcBondModuelSetting : DevExpress.XtraEditors.XtraUserControl
    {
        public UcBondModuelSetting()
        {
            this.InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= timer1_Tick;
                    this.timer1.Dispose();
                };
            // MachineStateModel.GetInstance().MachineState = MachineStateEnum.Stop;
        }

        private Locker locker = new Locker();

        /// <summary>
        /// 重复定位线程
        /// </summary>
        private Task visionRepeatTask;

        private bool isStopTest;

        /// <summary>
        /// 当前传输单元/载具 
        /// </summary>
        private TransportUnit TransportUnit =>
            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

        /// <summary>
        /// Bond头
        /// </summary>
        [JsonIgnore]
        private BondHead bondHead => System2Module.GetInstance().BondModule.BondHead;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;


        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private NozzleShelfController NozzleShelfController => System2Domain.GetInstance().NozzleShelfController;


        /// <summary>
        /// 焊后检测补偿
        /// </summary>
        private AKRSPoint3D postBondCompensation = BondDevicePara.GetInstance().BMCDevicePara.PostBondCompensate;

        /// <summary>
        /// ULM运动点位
        /// </summary>
        private ULMPara ULmPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// BondModule模组
        /// </summary>
        [JsonIgnore]
        public BondModule bondModule => System2Module.GetInstance().BondModule;

        private AKRSPoint3D startPos = new AKRSPoint3D();

        private AKRSPoint3D endPos = new AKRSPoint3D();


        private AKRSPoint3D transitionPos = new AKRSPoint3D();

        private AKRSPoint3D transitionPos2 = new AKRSPoint3D();

        private AKRSPoint3D transitionPos3 = new AKRSPoint3D();

        private Stopwatch sp = new Stopwatch();

        private Sensor electric = HardwareRepositoryService.GetHardware<Sensor>("LVDT");


        /// <summary>
        /// 换吸嘴重复性测试线程
        /// </summary>
        private Task downLookTestTestTask;

        /// <summary>
        /// 弱吹比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //  // 当前生产时长清零
            //  StatisticsDepository.Instance.ClearTimeSpan(StatisticsDepository.Instance.StatisticsTotallySystem1Instance);
            //  StatisticsDepository.Instance.ClearTimeSpan(StatisticsDepository.Instance.StatisticsTotallySystem2Instance);
            //  StatisticsDepository.Instance.Save();
            //  StatisticsDepository.Instance.ClearTimeSpan(StatisticsDepository.Instance.StatisticsForProductSystem1Instance);
            //  StatisticsDepository.Instance.ClearTimeSpan(StatisticsDepository.Instance.StatisticsForProductSystem2Instance);
            //  StatisticsDepository.Instance.Save();
        }

        /// <summary>
        /// 吸嘴架清理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNozzleShelfClean_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < NozzleRepository.GetInstance().BaseDsSettingList.Count; i++)
            {
                Nozzle nozzle = (Nozzle)NozzleRepository.GetInstance().BaseDsSettingList[i];
                if (nozzle.NozzleType == NozzleTypeEnum.Calibrate && nozzle.Name != "TouchDown" && nozzle.Name != "BMC")
                {
                    NozzleRepository.GetInstance().BaseDsSettingList.Remove(nozzle);
                }

                if (nozzle.Name == "No Reference")
                {
                    NozzleRepository.GetInstance().BaseDsSettingList.Remove(nozzle);
                }
            }

            var slots = BondProgram.GetInstance().NozzleShelfProgram.NozzleShelf.NozzleShelfSlots;

            for (int i = 0; i < slots.Length; i++)
            {
                if (NozzleRepository.GetInstance().BaseDsSettingList
                        .Find(it => it.Name == slots[i].NozzleName) == null)
                {
                    slots[i].NozzleName = string.Empty;
                }
            }

            NozzleRepository.GetInstance().Save();
            BondProgram.GetInstance().Save();
        }


        /// <summary>
        /// 中转台放片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button12_Click(object sender, EventArgs e)
        {
            //this.bondHeadController.MoveBondZToSafePos();
            //// 获取当前吸嘴
            //Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();
            //double curAngle = this.bondHeadController.GetAxisTRealPos();

            //BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find("coc1");

            //if (nozzle == null || nozzle.Name != component.NozzleName)
            //{
            //    DialogResult dialog = AKRSXtraMessageBox.Show(
            //    $"Please  attach  tool:{component.NozzleName}  on  bondhead!",
            //    "Warn",
            //    MessageBoxButtons.OKCancel,
            //    MessageBoxIcon.Warning);

            //    if (dialog == DialogResult.OK)
            //    {
            //        this.bondHeadController.SetCurrentNozzleName(component.NozzleName);
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            //// IPT位置转到Bond
            //AKRSPoint3D iPTPosInBond = this.bondModuleController.ConvertG0ToMachinePos(BondDevicePara.GetInstance().IPTDevicePara.IPTPos);

            //// 计算焊头旋转后的吸嘴偏移
            //AKRSPoint2D nozzleOffsetRotated = this.bondHeadController.GetNozzleOffset(
            //    nozzle.Name,
            //    curAngle - nozzle.AlignAngle);


            //AKRSPoint2D placePos = new AKRSPoint2D() { X = iPTPosInBond.X + component.IPTCenterOffset.X - nozzleOffsetRotated.X, Y = iPTPosInBond.Y + component.IPTCenterOffset.Y - nozzleOffsetRotated.Y };

            //// 运动到放料位
            //this.bondModuleController.MoveSafeBondXY(
            //   placePos.X,
            //placePos.Y);

            //// MessageBox.Show($"移动到放片位\r\nX{placePos.X}Y:{placePos.Y}\r\nangle:{curAngle}");

            //// 焊头下降放片
            //double liftLevel = this.bondModuleController.ConvertG0ToMachinePos(component.DownLookAdjustConfig.P1VisionPos).Z;
            //this.bondHeadController.BondAction(
            //    iPTPosInBond.Z + nozzle.MeasureHeightOffset + component.ComponentThickness,
            //    liftLevel,
            //    component,
            //    BondTypeEnum.BondOnIPT);

            //this.bondModuleController.MoveToG0Pos(component.DownLookAdjustConfig.P1VisionPos);

            //Thread.Sleep(1000);

            //// 寻找Pr模板
            //PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(component.DownLookAdjustConfig.P1PRName);

            //// 设置硬件
            //this.system2Controller.SetHardware(component.DownLookAdjustConfig.P1PRName, CameraTypeEnum.BondCamera);

            //// 开始定位
            //ExcuteResult result = pREntity.DoWork();

            //MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            //AKRSXtraMessageBox.Show($"定位结果isSuccess{matchResult.IsSuccess}\r\nX{matchResult.CenterX}Y:{matchResult.CenterY}\r\nangle:{matchResult.Angle}");
        }

        /// <summary>
        /// 中转台取片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>33
        private void button4_Click_1(object sender, EventArgs e)
        {
            //this.bondHeadController.MoveBondZToSafePos();

            //// 获取当前吸嘴
            //Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();
            //double curAngle = this.bondHeadController.GetAxisTRealPos();

            //BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find("coc1");

            //// IPT位置转到Bond
            //AKRSPoint3D iPTPosInBond = this.bondModuleController.ConvertG0ToMachinePos(BondDevicePara.GetInstance().IPTDevicePara.IPTPos);

            //// 计算焊头旋转后的吸嘴偏移
            //AKRSPoint2D nozzleOffsetRotated = this.bondHeadController.GetNozzleOffset(
            //    nozzle.Name,
            //    curAngle - nozzle.AlignAngle);


            //AKRSPoint2D pickPos = new AKRSPoint2D() { X = iPTPosInBond.X + component.IPTCenterOffset.X - nozzleOffsetRotated.X, Y = iPTPosInBond.Y + component.IPTCenterOffset.Y - nozzleOffsetRotated.Y };

            //// 运动到取料位
            //this.bondModuleController.MoveSafeBondXY(
            //    pickPos.X,
            //   pickPos.Y);

            //// MessageBox.Show($"移动到取片位\r\nX{pickPos.X}Y:{pickPos.Y}\r\nangle:{curAngle}");

            //// 焊头下降取片
            //double liftLevel = this.bondModuleController.ConvertG0ToMachinePos(component.DownLookAdjustConfig.P1VisionPos).Z;
            //this.bondHeadController.PickAction(
            //    iPTPosInBond.Z + nozzle.MeasureHeightOffset + component.ComponentThickness,
            //              component,
            //    liftLevel,
            //    PickTypeEnum.IPT);
        }

        /// <summary>
        /// PR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("上视温漂测试Mark");
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity("上视温漂测试Mark");
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Calibration;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.ShowDialog();
            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button15_Click(object sender, EventArgs e)
        {
            XtraOpenFileDialog ofd = new XtraOpenFileDialog();
            ofd.Title = "导入扫描列表文件";
            ofd.Filter = "Json文件|*.json";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            DialogResult dialogResult = ofd.ShowDialog();
            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            string filePath = "D:\\BMCTestDetail.xlsx";

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // 创建Excel对象
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                // 获取第一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // 获取单元格值
                int rowCount = worksheet.Dimension.Rows;

                int colCount = worksheet.Dimension.Columns;

                for (int i = 2; i <= rowCount; i++)
                {
                    for (int j = 1; j <= colCount - 1; j++)
                    {
                        double value = (double)worksheet.Cells[i, j].Value;
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            bool isAutoWork = true;

            Sensor sensor = HardwareRepositoryService.GetHardware<Sensor>("焊头LVDT");

            if (sensor != null)
            {
                TeLvdt.EditValue = sensor.ReadTxPDO((ushort)sensor.SensorIO, 1);
            }

            uint pTime,
              pTimeMax, pValue;

GTN.mc.GTN_GetTime(2, GTN.mc.ETimeElapse.TIME_ELAPSE_PROFILE, out pTime, out pTimeMax, out pValue);

            this.SpForce.Value = pTime;

            this.SpCheckForceLevel.Value = pTimeMax;

            this.SpForceControlSpeed.Value = pValue;
        }

        /// <summary>
        /// 自动对焦
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            //this.bondHeadController.AutoFocus(CameraTypeEnum.UpLookCamera);
        }

        /// <summary>
        /// 力控测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button17_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        ///  读压力表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button16_Click(object sender, EventArgs e)
        {
            ModbusService.GetInstance().ConnectBondhead();
            int[] ints = ModbusService.GetInstance().ReadBondForce();

            ModbusService.GetInstance().ConnectManometer();
            int[] ints2 = ModbusService.GetInstance().ReadManometer();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            this.bondHeadController.SetBlowProportion(2000);

            int i = this.bondHeadController.ReadWeakBlowProportion();

            double j = System2Module.GetInstance().BondModule.BondHead.CheckVaccumSensor.ReadTxPDO(
                (ushort)this.bondHead.CheckVaccumSensor.SensorIO ,
                1);
        }

        /// <summary>
        /// 测试同步顶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtSynchronousEjection_Click(object sender, EventArgs e)
        {
            this.bondHeadController.SynchronousEjection(50);
        }

        /// <summary>
        /// ULM两段运动测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button23_Click(object sender, EventArgs e)
        {
            try
            {
                this.bondModuleController.MoveSafeBondXYZ(this.startPos);

                double startLevel = this.bondHeadController.GetAxisZRealPos();

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.ULmPara.JumpToBondPosSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.ULmPara.JumpToBondPosAccelerationTime;
                interpolationParam.AccAcc = this.ULmPara.JumpToBondPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = startPos.X, Y = startPos.Y, Z = startPos.Z, T = 0 };
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = endPos.X, Y = endPos.Y, Z = endPos.Z, T = 90 };
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.ULmPara.JumpToBondPosTime,
                    RadiusRatio = this.ULmPara.JumpToBondPosRadiusRatio
                };

                foreach (var segmentConfig in interpolationParam.SegmentConfigs)
                {
                    // 加减速度默认*10
                    segmentConfig.Velocity = vel;
                    segmentConfig.Acc = vel * 10.0;
                    segmentConfig.Dec = vel * 10.0;
                }

                // 获取卡
                AxisCard card = HardwareRepositoryService.GetHardwaresByType<AxisCard>().Find(
                    card =>
                    card.AxisList.Select(axis => axis.AxisDrive).Exists(
                        drive => drive == interpolationParam.AxisDrives[0]));

                card.MotionController.ContinueInterpolationMove(interpolationParam);
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"运动到贴片位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"运动到贴片位失败！", ex, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 力控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button24_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 单轴同步控制运动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button25_Click(object sender, EventArgs e)
        {
//            this.sp.Restart();

//            // 三轴同时运动
//            MotionService.MoveAxesToTargetPosition(
//                (bondModule.BondAxisX, true, this.transitionPos.X, AccuracyMode.HighAccuracy),
//                  (bondModule.BondAxisY, true, this.transitionPos.Y, AccuracyMode.HighAccuracy)
//                );

//            MotionService.MoveAxesToTargetPosition(
//                (bondModule.BondAxisX, true, this.transitionPos2.X, AccuracyMode.HighAccuracy),
//                      (bondModule.BondAxisY, true, this.transitionPos2.Y, AccuracyMode.HighAccuracy)
//              );

//            MotionService.MoveAxesToTargetPosition(
//                (bondModule.BondAxisX, true, this.transitionPos3.X, AccuracyMode.HighAccuracy),
//                (bondModule.BondAxisY, true, this.transitionPos3.Y, AccuracyMode.HighAccuracy)
//               );

//            MotionService.MoveAxesToTargetPosition(
//                (bondModule.BondAxisX, true, this.endPos.X, AccuracyMode.HighAccuracy),
//                        (bondModule.BondAxisY, true, this.endPos.Y, AccuracyMode.HighAccuracy)
//);


//            this.SpAbsoluteMoveTime.Value = this.sp.ElapsedMilliseconds;
//            this.sp.Stop();
        }

        /// <summary>
        /// 设置起点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.startPos = this.bondModuleController.Get3DRealPosition();
            AKRSXtraMessageBox.Show("起点设置成功!");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.endPos = this.bondModuleController.Get3DRealPosition();
            AKRSXtraMessageBox.Show("终点设置成功!");
        }

        private void button27_Click(object sender, EventArgs e)
        {
            this.transitionPos = this.bondModuleController.Get3DRealPosition();
        }

        /// <summary>
        /// UML运动一段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button28_Click(object sender, EventArgs e)
        {
            this.transitionPos2 = this.bondModuleController.Get3DRealPosition();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            this.transitionPos3 = this.bondModuleController.Get3DRealPosition();
        }

        /// <summary>
        /// 运动到起点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button26_Click(object sender, EventArgs e)
        {
            // 先运动到起点
            //this.bondModuleController.MoveSafeBondXYZ(this.startPos);
        }

        /// <summary>
        /// 吸嘴示教重复性验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button32_Click(object sender, EventArgs e)
        {
            //Nozzle nozzle = NozzleRepository.GetInstance().GetNozzle("test");

            //for (int i = 0; i < 4; i++)
            //{
            //    this.bondHeadController.RotateAxisT(i * 90);

            //    AKRSPoint2D offset = this.bondHeadController.GetNozzleOffset(nozzle.Name, i * 90 - nozzle.AlignAngle);

            //    AKRSPoint3D pos = new AKRSPoint3D()
            //    {
            //        X = BondDevicePara.GetInstance().CameraDevicePara.UpLookPos.X
            //                                   - offset.X,
            //        Y = BondDevicePara.GetInstance().CameraDevicePara.UpLookPos.Y
            //                                   - offset.Y,
            //        Z = -25.0886
            //    };


            //    this.bondModuleController.MoveSafeBondXYZ(pos);

            //Retry:

            //    PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("圆搜索");

            //    // 执行定位
            //    MatchResult matchResult = (MatchResult)this.system2Controller.UpLookCameraVision(null, "圆搜索", true);

            //    if (matchResult == null)
            //    {
            //        DialogResult dialog = AKRSXtraMessageBox.Show(
            //            $"上视相机定位失败!",
            //            "Error",
            //            MessageBoxButtons.RetryCancel,
            //            MessageBoxIcon.Error);

            //        switch (dialog)
            //        {
            //            case DialogResult.Retry:
            //                goto Retry;

            //            case DialogResult.Cancel:
            //                return;

            //            default: return;
            //        }
            //    }


            //    // 打印测高结果
            //    string path = @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--"
            //                  + "吸嘴示教测试结果" + ".xlsx";

            //    // 添加一个工作表
            //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //    ExcelPackage excelPackage = new ExcelPackage(
            //        new FileInfo(
            //            @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--" + "吸嘴示教测试结果"
            //            + ".xlsx"));

            //    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
            //                                   ? excelPackage.Workbook.Worksheets[0]
            //                                   : excelPackage.Workbook.Worksheets.Add("DataSheet");

            //    // 设置列宽
            //    for (int j = 1; j <= 10; j++)
            //    {
            //        worksheet.Column(j).Width = 15;
            //    }

            //    // 添加标题行
            //    if (worksheet.Dimension == null)
            //    {
            //        worksheet.Cells[1, 1].Value = "T轴坐标";

            //        worksheet.Cells[1, 2].Value = "matchResult-X";


            //        worksheet.Cells[1, 3].Value = "matchResult-Y";
            //    }

            //    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            //    worksheet.Cells[lastUsedRow + 1, 1].Value = this.bondHeadController.GetAxisTRealPos();

            //    worksheet.Cells[lastUsedRow + 1, 2].Value = matchResult.CenterX;


            //    worksheet.Cells[lastUsedRow + 1, 3].Value = matchResult.CenterY;

            //    excelPackage.Save();
            //}
        }

        private void BtStart_Click(object sender, EventArgs e)
        {
            if (this.visionRepeatTask == null || this.visionRepeatTask.Status != TaskStatus.Running)
            {
                this.visionRepeatTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("定位重复性测试线程");

                            this.BtStart.BackColor = Color.Yellow;
                            this.sp.Restart();
                            bool isFirstStart = true;
                            isStopTest = false;
                            while (true)
                            {
                                if (this.sp.Elapsed.Minutes < (int)this.SpVisionInterval.Value
                                    && isFirstStart == false)
                                {
                                    continue;
                                }

                                if (isFirstStart)
                                {
                                    isFirstStart = false;
                                }

                                if (this.isStopTest)
                                {
                                    return;
                                }

                                // 吸嘴PR
                                PREntity nozzlePrEntity = (PREntity)VisionEntityRepository.GetInstance().Find("圆搜索");
                                if (nozzlePrEntity == null)
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"Please  edit  program  first!",
                                        "Prompt",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    return;
                                }

                            Retry1:

                                // 执行定位
                                MatchResult matchResult1 = (MatchResult)this.system2Controller.UpLookCameraVision(null, "圆搜索", true);

                                if (matchResult1 == null)
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"Uplook  camera  vision  failed!",
                                        "Error",
                                        MessageBoxButtons.RetryCancel,
                                        MessageBoxIcon.Error);

                                    switch (dialog)
                                    {
                                        case DialogResult.Retry:
                                            goto Retry1;

                                        case DialogResult.Cancel:
                                            return;

                                        default: return;
                                    }
                                }

                                // 标定片PR
                                PREntity glassPrEntity = (PREntity)VisionEntityRepository.GetInstance().Find("小标定片模板");
                                if (nozzlePrEntity == null)
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"Please  edit  program  first!",
                                        "Prompt",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    return;
                                }

                            Retry2:

                                // 执行定位
                                MatchResult matchResult2 = (MatchResult)this.system2Controller.BondCameraVision(null, "小标定片模板", true);

                                if (matchResult2 == null)
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"Uplook  camera  vision  failed!",
                                        "Error",
                                        MessageBoxButtons.RetryCancel,
                                        MessageBoxIcon.Error);

                                    switch (dialog)
                                    {
                                        case DialogResult.Retry:
                                            goto Retry2;

                                        case DialogResult.Cancel:
                                            return;

                                        default: return;
                                    }
                                }

                                (bool isSucceed, MatchResult[] matchResults) resultPr = Block.GetInstance().MatchResult("20240920EjectMatch", true);


                                // 打印
                                this.SaveVisionResult(matchResult1, matchResult2, resultPr.matchResults[0]);

                                Thread.Sleep(10);

                                this.sp.Restart();
                            }
                        });
            }
        }

        /// <summary>
        ///  保存数据
        /// </summary>
        /// <param name="matchResult1">定位结果</param>
        /// <param name="fileName">文件名</param>
        public void SaveVisionResult(MatchResult matchResult1, MatchResult matchResult2, MatchResult matchResult3)
        {
            try
            {
                lock (this.locker)
                {
                    string path = @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--" + "VisionRepeatTest"
                                  + ".xlsx";

                    // 添加一个工作表
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    ExcelPackage excelPackage = new ExcelPackage(
                        new FileInfo(
                            @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--" + "VisionRepeatTest" + ".xlsx"));

                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                                   ? excelPackage.Workbook.Worksheets[0]
                                                   : excelPackage.Workbook.Worksheets.Add("DataSheet");

                    // 设置列宽
                    for (int i = 1; i <= 10; i++)
                    {
                        worksheet.Column(i).Width = 15;
                    }

                    // 添加标题行
                    if (worksheet.Dimension == null)
                    {
                        worksheet.Cells[1, 1].Value = "Time";

                        worksheet.Cells[1, 2].Value = "Uplook-CenterX";

                        worksheet.Cells[1, 3].Value = "Uplook-CenterY";

                        worksheet.Cells[1, 4].Value = "Uplook-Angle";

                        worksheet.Cells[1, 5].Value = "Downlook-CenterX";

                        worksheet.Cells[1, 6].Value = "Downlook-CenterY";

                        worksheet.Cells[1, 7].Value = "Downlook-Angle";

                        worksheet.Cells[1, 8].Value = "Wafer-CenterX";

                        worksheet.Cells[1, 9].Value = "Wafer-CenterY";

                        worksheet.Cells[1, 10].Value = "Wafer-Angle";
                    }

                    int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                    worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    worksheet.Cells[lastUsedRow + 1, 2].Value = matchResult1?.CenterX;

                    worksheet.Cells[lastUsedRow + 1, 3].Value = matchResult1?.CenterY;

                    worksheet.Cells[lastUsedRow + 1, 4].Value = matchResult1?.Angle;

                    worksheet.Cells[lastUsedRow + 1, 5].Value = matchResult2?.CenterX;

                    worksheet.Cells[lastUsedRow + 1, 6].Value = matchResult2?.CenterY;

                    worksheet.Cells[lastUsedRow + 1, 7].Value = matchResult2?.Angle;

                    worksheet.Cells[lastUsedRow + 1, 8].Value = matchResult3?.CenterX;

                    worksheet.Cells[lastUsedRow + 1, 9].Value = matchResult3?.CenterY;

                    worksheet.Cells[lastUsedRow + 1, 10].Value = matchResult3?.Angle;

                    excelPackage.Save();
                }
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"File  is  opened ,saving  product  data  failed!\r\nOK:Close  file  and  retry\r\nCancel:Do  not  save  product  data",
                    "Warn",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                if (dialog == DialogResult.OK)
                {
                    SaveVisionResult(matchResult1, matchResult2, matchResult3);
                }

                //Process[] process = Process.GetProcessesByName("wps");
                //foreach (var item in process)
                //{
                //    item.Kill();
                //}

                //Thread.Sleep(100);

                //SaveVisionResult(matchResult, fileName);
            }
        }

        private void BtStop_Click(object sender, EventArgs e)
        {
            this.isStopTest = true;
            this.BtStart.BackColor = default;
            Machine.GetInstance().Stop();
        }

        /// <summary>
        /// UML向右等距运动10段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button31_Click(object sender, EventArgs e)
        {
            // 距离间隔
            //double pitch = 3;

            //AKRSPoint3D curPos = this.bondModuleController.Get3DRealPosition();

            //double accTime = 0.1;

            //double jerkTime = 0.04;

            //DsaIpolGroup iGroup = null;

            //// 获取X，Y轴驱动
            //DsaDrive moveDriveX = ((ETELAxis)bondModule.BondAxisX.AxisDrive).GetDrive();
            //DsaDrive moveDriveY = ((ETELAxis)bondModule.BondAxisY.AxisDrive).GetDrive();
            //DsaDrive moveDriveZ = ((ETELAxis)bondHead.AxisZ.AxisDrive).GetDrive();

            //// 开启10段插补
            //for (int i = 1; i < 30; i++)
            //{
            //    // 设置群组
            //    iGroup = new DsaIpolGroup(moveDriveX, moveDriveY/*, moveDriveZ*/);
            //    this.sp.Restart();
            //    // 开始插补
            //    iGroup.ipolBegin();
            //    this.sp.Stop();

            //    LogHelper.Post(Level.Info, $"开始第{i}段插补用时： {sp.ElapsedMilliseconds}ms", LogCategory.Bond);

            //    this.sp.Restart();

            //    // 设置为绝对坐标系 ，不设置绝对坐标系
            //    iGroup.ipolSetAbsMode(true, -1);

            //    double speed = (double)this.SpSpeed.Value / 1000.0;

            //    // 设置矢量速度  
            //    iGroup.ipolUSpeed(speed);

            //    // 设置加速时间和加加速时间  加速时间 应该大于等于 2 * 加加速时间 
            //    iGroup.ipolUTime(accTime, jerkTime);

            //    AKRSPoint4D point1 = new AKRSPoint4D()
            //    {
            //        X = (curPos.X + pitch * i) / 1000.0,
            //        Y = curPos.Y / 1000.0,
            //        Z = -curPos.Z / 1000.0,
            //    };
            //    double[] array1 = new[] { point1.X, point1.Y, point1.Z, point1.T };
            //    DsaVector vector1 = new DsaVector(array1);
            //    iGroup.ipolULine(vector1);

            //    // 等待插补结束
            //    iGroup.ipolWaitMovement(100000);

            //    // 退出插补模式
            //    iGroup.ipolEnd();

            //    this.sp.Stop();

            //    LogHelper.Post(Level.Info, $"第{i}段插补用时: {sp.ElapsedMilliseconds}ms", LogCategory.Bond);
            //}
        }

        private void button33_Click(object sender, EventArgs e)
        {
            //sp.Restart();
            //// 寻找Pr模板
            ////PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("20240920ModuleMark1");
            ////DispenseRunTimeProvider.DispensePREntity = pREntity.CloneToMemory();

            //this.SpForceTime.Value = sp.ElapsedMilliseconds;
            //A.LocalMethod();
            //A.LocalMethod();
            //A.LocalMethod();

            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 50; i++)
            //    {
            //        Thread.Sleep(1000);
            //        this.NozzleShelfController.MoveShelfToHome();
            //        //System2Module.GetInstance().NozzleShelfModule.AxisY.WaitForArrival();
            //        Thread.Sleep(1000);
            //        this.NozzleShelfController.MoveShelfToChangeNozzlePos();
            //        //System2Module.GetInstance().NozzleShelfModule.AxisY.WaitForArrival();
            //    }
            //});

            int forceMaxVal = 0;

            Task.Run(() =>
            {
                while (true)
                {
                    int[] read = ModbusService.GetInstance().ReadManometer();

                    if (read[0] / 10 > forceMaxVal)
                    {
                        forceMaxVal = read[0] / 10;

                        this.Invoke(
                  new Action(
                      () =>
                      {
                          this.SpForceTime.Value = forceMaxVal;
                      }));
                    }
                }
            });


        }

        /// <summary>
        /// 点胶视觉模块控制器
        /// </summary>
        public DispenseVisionController DispenseVisionController { get; set; } = new DispenseVisionController();

        private void button34_Click(object sender, EventArgs e)
        {
            int limit = 500;
            Task.Run(() =>
            {
                for (int i = 0; i < limit; i++)
                {
                    // 点胶相机定点拍照
                    MatchResult matchResult1 = (MatchResult)this.DispenseVisionController.DispenseVision(
                    null,
                    "20240920ModuleMark1");

                    System2CommonService.SaveVisionResult(matchResult1, "DispenseCamera");
                }
            });

            Task.Run(() =>
            {
                for (int i = 0; i < limit; i++)
                {
                    // Bond相机定点拍照
                    MatchResult matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
               null,
               "20240920ModuleMark1");
                    System2CommonService.SaveVisionResult(matchResult1, "BondCamera");
                }
            });
        }


        /// <summary>
        ///  温漂定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button35_Click(object sender, EventArgs e)
        {
            //this.bondModuleController.MoveSafeBondXYZ
            //                   (
            //                       BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos);

            //MatchResult driftMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(null,
            //                "温漂定位",
            //                "上视温漂测试Mark");

            //AKRSPoint3D pos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), driftMatchResult);

        }


        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 焊头旋转中心定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button37_Click(object sender, EventArgs e)
        {
            //#region 旋转中心计算

            //// 寻找Pr模板
            //PREntity upLookPREntity = (PREntity)VisionEntityRepository.GetInstance()
            //    .Find(CalibrateRunPara.GetInstance().UpLookPRName);

            //MatchResult matchRrotateCenterResult = new MatchResult();

            //AKRSPoint3D rotatePos = new AKRSPoint3D();


            ////this.bondModuleController.MoveSafeBondXYZ(
            ////    CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos);

            //int[] angleArray = new int[] { -180, -90, 0, 90, 180 };
            //List<AKRSPoint2D> circlePointList = new List<AKRSPoint2D>();
            //List<AKRSPoint2D> matchResultList2D = new List<AKRSPoint2D>();

            //AKRSPoint2D curMachinePos2D = this.bondModuleController.Get2DRealPosition();

            //foreach (int angle in angleArray)
            //{
            //    // T轴旋转一定角度
            //    this.bondHeadController.RotateAxisT(angle);
            //    Thread.Sleep(100);

            //RetryCommand:
            //    ExcuteResult res = upLookPREntity.DoWork();

            //    // 处理拍照完成后的结果
            //    if (res != ExcuteResult.Success)
            //    {
            //        DialogResult dialog = AKRSMessageBoxExt.Show(
            //              $"Uplook  {CalibrateRunPara.GetInstance().UpLookPRName} P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
            //              "Alarm",
            //              new string[] { "Retry", "Abort", "Ignore" },
            //              new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
            //              AlarmLevel.SecondLevel);

            //        switch (dialog)
            //        {
            //            case DialogResult.Retry:

            //                goto RetryCommand;

            //            case DialogResult.Abort:

            //                throw new Exception($"{CalibrateRunPara.GetInstance().UpLookPRName}  adjust  failed!");

            //            case DialogResult.Ignore:
            //                matchRrotateCenterResult = new MatchResult();
            //                break;
            //        }
            //    }
            //    else
            //    {
            //        matchRrotateCenterResult = (MatchResult)upLookPREntity.AlgResult;
            //    }

            //    matchResultList2D.Add(new AKRSPoint2D(matchRrotateCenterResult.CenterX, matchRrotateCenterResult.CenterY));

            //    AKRSPoint2D circlePoint = CalibService.GetMachinePosByPixelPos(
            //        curMachinePos2D,
            //        matchRrotateCenterResult,
            //        "UpLookCameraCoordinateSystem");

            //    circlePointList.Add(circlePoint);
            //}

            //CalibService.FitCircle(matchResultList2D, out double circleCenterPixelX, out double circleCenterPixelY, out double circleRadiusPixel);

            //matchRrotateCenterResult.CenterX = circleCenterPixelX;
            //matchRrotateCenterResult.CenterY = circleCenterPixelY;
            //this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, new MatchResult(circleCenterPixelX, circleCenterPixelY, 0));
            //rotatePos = this.bondModuleController.Get3DRealPosition();



            //#endregion
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button38_Click(object sender, EventArgs e)
        {
            sp.Restart();
            Task.Run(() =>
            {
                this.bondHead.VaccumElectric.SetOutputValue(true);
            });

            Task.Run(() =>
            {
                this.bondHead.WeakBlowElectric.SetOutputValue(false);
            });

            while (true)
            {
                if (electric.ReadTxPDO(0, 1) > SpSetVal.Value)
                {
                    SpEclispTime.Value = sp.ElapsedMilliseconds;
                    break;
                }

                Thread.Sleep(1);

            }

        }


        /// <summary>
        /// 开始计时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button39_Click(object sender, EventArgs e)
        {
            sp.Restart();
            Task.Run(() =>
            {
                this.bondHead.VaccumElectric.SetOutputValue(false);
            });

            Task.Run(() =>
            {
                this.bondHead.WeakBlowElectric.SetOutputValue(true);
            });

            while (true)
            {
                if (electric.ReadTxPDO(0, 1) > SpSetVal.Value)
                {
                    SpEclispTime.Value = sp.ElapsedMilliseconds;

                    break;
                }

                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 顶针Z
        /// </summary>        
        private Axis EjectionAxisZ = HardwareRepositoryService.GetHardware<Axis>("顶针Z");

        /// <summary>
        /// 顶针台Z
        /// </summary>        
        private Axis EjectionTableZ = HardwareRepositoryService.GetHardware<Axis>("顶针台Z");

        /// <summary>
        /// 顶针回零重复性测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button36_Click(object sender, EventArgs e)
        {
            //Task.Run(() =>
            //{

            //    try
            //    {
            //        // 顶针循环运动
            //        // todo:修改循环次数
            //        for (int i = 0; i < 50; i++)
            //        {
            //            // 回零
            //            //EjectionAxisZ.GoHome();
            //            EjectionAxisZ.AbsoluteMove(0);

            //            // 顶针顶起
            //            // todo:修改顶针顶起位置
            //            EjectionAxisZ.AbsoluteMove(1.321);

            //            // 回零
            //            //EjectionTableZ.GoHome();

            //            // 顶针顶起
            //            // todo:修改顶针顶起位置
            //            //EjectionTableZ.AbsoluteMove(180);

            //            if (false)
            //            {
            //                // 移动到测高位置
            //                // todo:修改测高位置
            //                this.bondModuleController.MoveSafeBondXYZ(new AKRSPoint3D(20.2008, -158.4076, -76.2078));

            //            ReMeasureHeight:
            //                // 顶针测高
            //                (ExcuteResult Ret, double HeightValue) result = this.bondHeadController.MeasureHeight(
            //                    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
            //                    HeightMeasurementFunctionEnum.WithTDSensor);

            //                if (result.Ret != ExcuteResult.Success)
            //                {
            //                    DialogResult res = AKRSXtraMessageBox.Show(
            //                        "System 2: Warning 2.2264:\r\n"
            //                        + "Measure  height  failed!\r\n ReMeasureHeight with  OK\r\n End assistant  with  Cancel",
            //                        "Prompt",
            //                        MessageBoxButtons.OKCancel,
            //                        MessageBoxIcon.Information);
            //                    switch (res)
            //                    {
            //                        case DialogResult.OK:
            //                            goto ReMeasureHeight;
            //                        case DialogResult.Cancel:
            //                            return;
            //                    }
            //                }

            //                // 打印测高结果
            //                string path = @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--"
            //                              + "顶针测高结果" + ".xlsx";

            //                // 添加一个工作表
            //                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //                ExcelPackage excelPackage = new ExcelPackage(
            //                    new FileInfo(
            //                        @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--" + "顶针测高结果"
            //                        + ".xlsx"));

            //                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
            //                                               ? excelPackage.Workbook.Worksheets[0]
            //                                               : excelPackage.Workbook.Worksheets.Add("DataSheet");

            //                // 设置列宽
            //                for (int j = 1; j <= 10; j++)
            //                {
            //                    worksheet.Column(j).Width = 15;
            //                }

            //                // 添加标题行
            //                if (worksheet.Dimension == null)
            //                {
            //                    worksheet.Cells[1, 1].Value = "Time";

            //                    worksheet.Cells[1, 2].Value = "顶针测高结果";
            //                }

            //                int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            //                worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            //                worksheet.Cells[lastUsedRow + 1, 2].Value = result.HeightValue;

            //                excelPackage.Save();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        AKRSXtraMessageBox.Show($"测试失败：{e.ToString()}");
            //    }

            //});

        }

        private void button40_Click(object sender, EventArgs e)
        {
            //Task.Run(() =>
            //{

            //    try
            //    {
            //        // todo:修改循环次数
            //        for (int i = 0; i < 50; i++)
            //        {
            //            // 移动到测高位置
            //            // todo:修改测高位置
            //            this.bondModuleController.MoveSafeBondXYZ(new AKRSPoint3D(-14.52, -46.86, -34.2126));

            //        ReMeasureHeight:
            //            // 顶针测高
            //            (ExcuteResult Ret, double HeightValue) result = this.bondHeadController.MeasureHeight(
            //                BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
            //                HeightMeasurementFunctionEnum.WithTDSensor);

            //            if (result.Ret != ExcuteResult.Success)
            //            {
            //                DialogResult res = AKRSXtraMessageBox.Show(
            //                    "System 2: Warning 2.2264:\r\n"
            //                    + "Measure  height  failed!\r\n ReMeasureHeight with  OK\r\n End assistant  with  Cancel",
            //                    "Prompt",
            //                    MessageBoxButtons.OKCancel,
            //                    MessageBoxIcon.Information);
            //                switch (res)
            //                {
            //                    case DialogResult.OK:
            //                        goto ReMeasureHeight;
            //                    case DialogResult.Cancel:
            //                        return;
            //                }
            //            }

            //            // 打印测高结果
            //            string path = @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--"
            //                          + "基板测高结果" + ".xlsx";

            //            // 添加一个工作表
            //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //            ExcelPackage excelPackage = new ExcelPackage(
            //                new FileInfo(
            //                    @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--" + "基板测高结果"
            //                    + ".xlsx"));

            //            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
            //                                           ? excelPackage.Workbook.Worksheets[0]
            //                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            //            // 设置列宽
            //            for (int j = 1; j <= 10; j++)
            //            {
            //                worksheet.Column(j).Width = 15;
            //            }

            //            // 添加标题行
            //            if (worksheet.Dimension == null)
            //            {
            //                worksheet.Cells[1, 1].Value = "Time";

            //                worksheet.Cells[1, 2].Value = "基板测高结果";
            //            }

            //            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            //            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            //            worksheet.Cells[lastUsedRow + 1, 2].Value = result.HeightValue;

            //            excelPackage.Save();
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        AKRSXtraMessageBox.Show($"测试失败：{e.ToString()}");
            //    }

            //});

        }

        private void button41_Click(object sender, EventArgs e)
        {
            //BondModuleController bondModuleController = new BondModuleController();
            //BondModule bondModule = new BondModule();
            //Stopwatch sp = new Stopwatch();
            //bool flag = true;
            //Task.Run(() =>
            //{
            //    while (flag)
            //    {
            //        //sp.Restart();
            //        //AKRSPoint3D p1 = new AKRSPoint3D(22.85, -180, 120);
            //        //bondModuleController.MoveToG0Pos(p1);
            //        //sp.Stop();
            //        //this.Invoke(new Action(()=>SpBondMoveTime1.Text = sp.ElapsedMilliseconds.ToString()));

            //        //sp.Restart();
            //        //AKRSPoint3D p2 = new AKRSPoint3D(24.5, -177, 120);
            //        //bondModuleController.MoveToG0Pos(p2);
            //        //sp.Stop();
            //        //this.Invoke(new Action(() => SpBondMoveTime2.Text = sp.ElapsedMilliseconds.ToString()));


            //        sp.Restart();
            //        double x = bondModule.BondAxisX.GetCmdPosition();
            //        sp.Stop();
            //        this.Invoke(new Action(() => SpBondMoveTime1.Text = sp.ElapsedMilliseconds.ToString()));

            //        sp.Restart();
            //        bondModule.BondAxisX.AbsoluteMove(x);
            //        sp.Stop();
            //        this.Invoke(new Action(() => SpBondMoveTime2.Text = sp.ElapsedMilliseconds.ToString()));
            //    }
            //});
        }

        /// <summary>
        ///  当前位置取消力控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button42_Click(object sender, EventArgs e)
        {
            //double speed = Convert.ToDouble(this.SpForceControlSpeed.EditValue);


            //// 力控上抬到安全高度
            //this.bondHeadController.ForceControlReset(this.bondHeadController.GetAxisZRealPos(), speed);
        }

        /// <summary>
        ///  上抬2mm取消力控
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button43_Click(object sender, EventArgs e)
        {
            //double speed = Convert.ToDouble(this.SpForceControlSpeed.EditValue);

            //// 力控上抬到安全高度
            //this.bondHeadController.ForceControlReset(this.bondHeadController.GetAxisZRealPos() + 2, speed);
        }

        /// <summary>
        /// 吸嘴架运动重复性测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNozzleShelfTest_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < 50; i++)
            //{
            //    this.NozzleShelfController.MoveShelfToHome();

            //    this.NozzleShelfController.MoveShelfToChangeNozzlePos();
            //}

            //AKRSPoint3D pos = BondDevicePara.GetInstance().CameraDevicePara.UpLookPos;

            this.system2Controller.GetCleanPos();
        }

        /// <summary>
        ///  移动温漂拍照位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button46_Click(object sender, EventArgs e)
        {
            //this.bondModuleController.MoveSafeBondXYZ
            //(
            //    BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);
        }

        /// <summary>
        ///日志打印测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            BaseCarrierConfig component = new BaseCarrierConfig();
            this.sp.Restart();
            //for (int i = 0; i < 9999; i++)
            //{
            Thread.Sleep(5);
            LogHelper.Post(Level.Info,
$" 固精流程焊头准备下压固精，固精参数：" +

$" 固精前低速段速度: {component.SlowTravelSpeedBeforeBonding}" +
$" 固精后低速段速度: {component.SlowTravelSpeedAfterBonding}," +
$" 固精前低速段距离: {component.SlowTravelDistanceBeforeBonding} " +
$" 固精后低速段距离: {component.SlowTravelDistanceAfterBonding} " +
$" 抬起高度: {BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z} " +
$" 固精延时: {component.PlacementDelay} "
, LogCategory.Bond);
            //}
            this.sp.Stop();
            this.SpBondMoveTime1.Value = this.sp.ElapsedMilliseconds;

            LogHelper.Post(Level.Info, $"晶圆相机定位-等Bond模组到避让位超时ms!", LogCategory.Component);
        }

        /// <summary>
        /// 打印的数据
        /// </summary>
        private List<(DateTime time, double forceVal, double forceVal2, double forceVal3, double forceVal4, double forceVal5, double forceVal6, double forceVal7, double forceVal8, double forceVal9)> printSource = new();

        /// <summary>
        /// 数据记录测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDataLogTest_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                printSource.Add(new() { time = DateTime.Now, forceVal = 99 , forceVal2 = 99 , forceVal3 = 99 , forceVal4 = 99 , forceVal5 = 99 , forceVal6 = 99 , forceVal7 = 99 , forceVal8 = 99 , forceVal9 = 99 });
            }


            // 数据大于5000就打印一次
            //if (this.printSource.Count > 5000)
            {
                this.printSource.Select(
                    (tuple, index) =>
                        {
                            return new
                                       {
                                           时间 = tuple.time,
                                           力值 = tuple.forceVal,
                                           力值2 = tuple.forceVal2,
                                           力值3 = tuple.forceVal3,
                                           力值4 = tuple.forceVal4,
                                           力值5 = tuple.forceVal5,
                                           力值6 = tuple.forceVal6,
                                           力值7 = tuple.forceVal7,
                                           力值8 = tuple.forceVal8,
                                           力值9 = tuple.forceVal9,
                                       };
                        }).ExportToXlsx(@"D:\力控记录\ForceTest.xlsx");

                //this.printSource.Clear();
            }
        }

        private static readonly Random GlobalRandom = new Random();

        private void LogDataTestMethod()
        {
            IDataLog log = DataLogManager.Instance.GetDataLog("BondingData");
            lock (GlobalRandom)
            {
                AKRSPoint2D pos2D1 = new AKRSPoint2D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);
                AKRSPoint2D pos2D2 = new AKRSPoint2D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);
                AKRSPoint2D pos2D3 = new AKRSPoint2D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);

                AKRSPoint2D pos2D4 = new AKRSPoint2D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);
                AKRSPoint3D pos3D1 = new AKRSPoint3D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);
                AKRSPoint3D pos3D2 = new AKRSPoint3D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);
                AKRSPoint4D pos4D1 = new AKRSPoint4D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);
                AKRSPoint4D pos4D2 = new AKRSPoint4D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);
                List<AKRSPoint3D> posList = new List<AKRSPoint3D> { pos3D1, pos3D2 };


                //List<AKRSPoint2D> prResult = [];
                for (int i = 0; i < 10; i++)
                {
                    AKRSPoint2D temPoint2D = new AKRSPoint2D(GlobalRandom.NextDouble() * 20 - 10, GlobalRandom.NextDouble() * 20 - 10);

                    log.AddData($"第{i + 1}次定位", pos2D1.Export());

                    //prResult.Add(temPoint2D);
                }

                log.AddData("2D点位1", pos2D1.Export());
                log.AddData("2D点位2", pos2D2.Export());
                log.AddData("2D点位3", pos2D3.Export());
                log.AddData("2D点位4", pos2D4.Export());
                log.AddData("3D点位3", pos3D1.Export());
                log.AddData("3D点位4", pos3D2.Export());
                log.AddData("4D点位5", pos4D1.Export());
                log.AddData("4D点位6", pos4D2.Export());
                log.AddData("点位集合3D", posList.Select(p => p.Export()));
            }

            log.CommitData();
            log.FlushAsync();
        }

        /// <summary>
        /// 翻转台T
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //flipTableController.MoveFlipTAxis(0);
        }

        /// <summary>
        /// 删除旧程式PR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //if (RecipeRepository.GetInstance().IsExistRecipeName(this.TxtOldRecipeName.Text) == false)
            //{
            //    return;
            //}

            //TuService.DeleteOldRecipePR(this.TxtOldRecipeName.Text);
        }


        private AKRSPoint4D pickPos = new AKRSPoint4D();

        /// <summary>
        ///  设置起点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetStartPos_Click(object sender, EventArgs e)
        {
            this.startPos = this.bondModuleController.Get3DRealPosition();
        }

        /// <summary>
        /// 设置取片位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSetPickPos_Click(object sender, EventArgs e)
        {
            this.pickPos = this.bondModuleController.Get4DRealPosition();
        }

        /// <summary>
        /// 运动到起点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveToStartPos_Click(object sender, EventArgs e)
        {
            //this.bondModuleController.MoveSafeBondXYZ(this.startPos);
        }

        private PickActionNode pickAction = new PickActionNode();

        /// <summary>
        /// Bond域
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController actionNodesService => System2Domain.GetInstance().ActionNodesService;

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController = new NozzleShelfController();

        /// <summary>
        /// 顶针控制器
        /// </summary>
        private EjectController ejectController = new EjectController();

        /// <summary>
        /// Pick动作节点帮助类
        /// </summary>
        private PickActionService pickActionProvider = new PickActionService();

        /// <summary>
        ///  绝对运动到取片位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAbusoluteMoveToPickPos_Click(object sender, EventArgs e)
        {
            //this.bondModuleController.MoveSafeBondXYZ(this.startPos);

            //this.JumpToPickupPosWithAbsoluteMove(
            //    this.pickPos,
            //    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z);
        }


        /// <summary>
        ///ulm运动到取片位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnULMMoveToPickPos_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 摇杆参数
        /// </summary>
        private JoystickPara joystickPara => MachineDevicePara.GetInstance().JoystickPara;

        /// <summary>
        /// 摇杆参数
        /// </summary>
        private MachineDevicePara MachineDevicePara => MachineDevicePara.GetInstance();

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtReadLvdt_Click(object sender, EventArgs e)
        {
            //LaserEncoder laserEncoder = HardwareRepositoryService.GetHardware<LaserEncoder>("激光干涉尺");

            //// 生成两个Task来读Election
            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 100; i++)
            //    {
            //        bool status = laserEncoder.GetWarmupStatus();
            //    }
            //});

            //Task.Run(() =>
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        bool status = laserEncoder.GetWarmupStatus();
            //    }
            //});

            //FrmChipFrictionSetting FrmChipFrictionSetting = new FrmChipFrictionSetting();
            //FrmChipFrictionSetting.Show();

            DispenseRunTimeProvider.RecordTime("预点胶", $"手动模式-去视觉位完成");
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            AKRSCamera bondCamera = HardwareRepositoryService.GetHardware<AKRSCamera>("BOND相机");

            Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    bondCamera.SnapImage(false);
                    this.Invoke(
                        new Action(() => { TeSnapCount.EditValue = i; }));
                }
            });
        }


        /// <summary>
        /// 系统2画胶
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            LogHelper.Post(Level.Error, $"流程{this.Name}运行故障", new Exception(), LogCategory.Bond);

            LogHelper.Post(Level.Info,
                $" 固精流程焊头准备下压固精，固精参数：" 
                , LogCategory.Bond);

            System2RunTimeProvider.RecordTime("取片信号交互", $"第一次启动，给晶圆发要料信号，");
        }

        /// <summary>
        /// 力控WatchOn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            short rtn;
            short core = 1;

            rtn = GTN.mc.GTN_LoadWatchConfig(1, "watchTimerCore1.ini");
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core1GTN_LoadWatchConfig" + rtn.ToString());
            }

            rtn = GTN.mc.GTN_LoadWatchConfig(2, "watchTimerCore2.ini");
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core2GTN_LoadWatchConfig" + rtn.ToString());
            }
        }

       /// <summary>
       /// 力控WatchOff
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            short rtn;
            short core = 1;

            rtn = GTN.mc.GTN_WatchOff(1);
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core1 GTN_WatchOff" + rtn.ToString());
            }

            rtn = GTN.mc.GTN_PrintWatch(1, "watchdataCore1.txt", 0, 0);
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core1 GTN_PrintWatch" + rtn.ToString());
            }

            rtn = GTN.mc.GTN_WatchOff(2);
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core2 GTN_WatchOff" + rtn.ToString());
            }

            rtn = GTN.mc.GTN_PrintWatch(2, "watchdataC  `ore2.txt", 0, 0);
            if (0 != rtn)
            {
                AKRSXtraMessageBox.Show("Core2 GTN_PrintWatch" + rtn.ToString());
            }
        }

       /// <summary>
        ///  jog测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click_1(object sender, EventArgs e)
        {
            //// 移动
            //this.bondModule.BondAxisX.JogByAbsoluteMove(MoveDirection.Positive);

            //// 更新速度
            //this.bondModule.BondAxisX.UpdateVelOnLine(50);

            //Thread.Sleep(1000);

            //this.bondModule.BondAxisX.StopMove();

        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            WaferSubModule.GetInstance().WaferTable.WaferTableAxisX.StopMove();
            this.bondModule.BondAxisX.StopMove();
        }

        private void button30_Click_1(object sender, EventArgs e)
        {
            XtraSaveFileDialog fileDialog = new XtraSaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();

                    // 打印
                    ForceCalibrationService.ExportCaliData(fileDialog.FileName);

                    AKRSXtraMessageBox.Show(
                        $"打印成功!",
                        "提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("正由另一进程使用"))
                    {
                        AKRSXtraMessageBox.Show("数据导出失败！文件正由另一个程序占用！", "提示");
                    }
                    else
                    {
                        AKRSXtraMessageBox.Show("数据导出失败！数据量过大，请分别统计再导出！", "提示");
                    }
                }
            }
        }

        private void BtnChangeForceData_Click(object sender, EventArgs e)
        {
            //double increase = 3;

            //ForceCalibrationData.GetInstance().LargeForceRelationList.ForEach(
            //    it => it.TheoreticalForceAndActualForceList.ForEach(it => it.CaliTableForce -= 3));

            ForceCalibrationData.GetInstance();
            ClosedLoopCalibrationController closedLoopCalibrationController = new ClosedLoopCalibrationController();
            closedLoopCalibrationController.Save();
        }

        /// <summary>
        ///  单轴变速运动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //this.bondHeadController.MoveAxisZ(14);

            //Thread.Sleep(50);

            //double[] posArr = new[]
            //                      {                
            //                         14.1,
            //                  24
            //                      };

            //double[] velArr = new[] { 50, 800.0 };

            //this.bondModuleController.MoveContinuousAbsolute(posArr, velArr);
        }
    }
}
