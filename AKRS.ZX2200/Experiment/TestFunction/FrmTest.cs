using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Experiment.TestFunction
{
    using System.IO;
    using System.Threading;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
    using AKRS.ZX2200.SupportFeature.MotionPlan;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using ch.etel.edi.dsa.v40;
    using OfficeOpenXml;

    /// <summary>
    /// 测试按钮
    /// </summary>
    [MethodAop]
    public partial class FrmTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 测试界面
        /// </summary>
        public FrmTest()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 展示图案
        /// </summary>
        public static void ShowUi()
        {
            if (XtraInputBox.Show("请输入密码", "密码", 0) == 145632)
            {
                FrmTest frmTest = new FrmTest();
                frmTest.Show();
            }
        }

        private void SimpleButton1_Click(object sender, EventArgs e)
        {
            Axis BondAxisX = HardwareRepositoryService.GetHardware<Axis>("BondX");
            double temperature = BondAxisX.ReadTemperature();
        }

        [MethodAop]
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //AKRSPoint3D a = DefectCompensate.GetPostCompensation("测试焊后1");


            //BondHeadController bondHeadController = new BondHeadController();
            //(ExcuteResult Ret, double a) = bondHeadController.MeasureHeight(bondHeadController.GetAxisZRealPos(), HeightMeasurementFunctionEnum.WithTDSensor);


            //BondModule bondModule = new BondModule();


            //BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos = bondModule.Get3DRealPosition();
            //BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos.Z = a;
            //BondDevicePara.GetInstance().Save();

            //FrmAssistantDispense frmAssistantDispense = new FrmAssistantDispense();
            //frmAssistantDispense.Show();

            //FrmS2DispensePlateAssistant frmS2DispensePlateAssistant = new FrmS2DispensePlateAssistant();
            //frmS2DispensePlateAssistant.Show();

            //(bool success,AKRSPoint3D point3D) = TUAssistantHelper.AssistantPR("20250406两点定位ModuleMark1");

            //AKRSPoint3D point3D1 = point3D - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            //System2Domain.GetInstance().BondModuleController.MoveToG0Pos(point3D1);

            //Thread.Sleep(1000);

            //MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(null, "20250406两点定位ModuleMark1", "20250406两点定位ModuleMark1");

            //List<MatchResult> matchResults = new List<MatchResult>();

            //for (int i = 0; i < 10; i++)
            //{
            //    point3D1.X += 0.1;
            //    point3D1.Y -= 0.1;
            //    System2Domain.GetInstance().BondModuleController.MoveToG0Pos(point3D1);
            //    Thread.Sleep(100);
            //    // (bool success2, AKRSPoint3D point3D3) = TUAssistantHelper.AssistantPR("20250406两点定位ModuleMark1");
            //    MatchResult result3 = (MatchResult)System2Domain.GetInstance().System2CommonVision(null, "20250406两点定位ModuleMark1", "20250406两点定位ModuleMark1");
            //    AKRSPoint3D realPoint = System2Module.GetInstance().BondModule.GetG0RealPosition();

            //    realPoint = realPoint + new AKRSPoint3D((result3.CenterX - 1224) * 0.0017, -(result3.CenterY - 1024) * 0.0017,0);

            //    System2Domain.GetInstance().BondModuleController.MoveToG0Pos(realPoint);

            //    Thread.Sleep(100);
            //    MatchResult result2 = (MatchResult)System2Domain.GetInstance().System2CommonVision(null, "20250406两点定位ModuleMark1", "20250406两点定位ModuleMark1");
            //    matchResults.Add(result2);
                
            //}

            int a = 1;
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos = new AKRSPoint3D(-113.858, -147.002, -32.8016);

            CalibrateRunPara.GetInstance().Save();

          //  System2Domain.GetInstance().BondHeadController.RotateAxisT(0);

            //  MatchResult matchResult1 = (MatchResult)System2Domain.GetInstance().System2Controller.UpLookCameraVision(
            //      new AKRSPoint3D(-102.155, -311.527, 102.7086),
            //      "上视标定模板");

            //  // T轴转到定位角度,应该还需要加一个硬补偿
            //  System2Domain.GetInstance().BondHeadController.RotateAxisT(90);

            //  MatchResult matchResult2 = (MatchResult)System2Domain.GetInstance().System2Controller.UpLookCameraVision(
            //       new AKRSPoint3D(-102.155, -311.527, 102.7086),
            //     "上视标定模板");

            //  // T轴转到定位角度,应该还需要加一个硬补偿
            //  System2Domain.GetInstance().BondHeadController.RotateAxisT(180);

            //  MatchResult matchResult3 = (MatchResult)System2Domain.GetInstance().System2Controller.UpLookCameraVision(
            //     new AKRSPoint3D(-102.155, -311.527, 102.7086),
            //      "上视标定模板");


            //  // T轴转到定位角度,应该还需要加一个硬补偿
            //  System2Domain.GetInstance().BondHeadController.RotateAxisT(270);

            //  MatchResult matchResult4 = (MatchResult)System2Domain.GetInstance().System2Controller.UpLookCameraVision(
            //       new AKRSPoint3D(-102.155, -311.527, 102.7086),
            //     "上视标定模板");

            //  double x1 = 1224 - matchResult1.CenterX;
            //  double x2 = 1224 - matchResult2.CenterX;
            //  double x3 = 1224 - matchResult3.CenterX;
            //  double x4 = 1224 - matchResult4.CenterX;

            //  double Y1 = 1024 - matchResult1.CenterY;
            //  double Y2 = 1024 - matchResult2.CenterY;
            //  double Y3 = 1024 - matchResult3.CenterY;
            //  double Y4 = 1024 - matchResult4.CenterY;

            //  MatchResult matchResult = new MatchResult(
            //     (matchResult1.CenterX + matchResult2.CenterX + matchResult3.CenterX + matchResult4.CenterX) / 4,
            //     (matchResult1.CenterY + matchResult2.CenterY + matchResult3.CenterY + matchResult4.CenterY) / 4,
            //     0);

            //AKRSPoint3D point3D = System2Domain.GetInstance().UpLookController.ConvertPixelToG0Pos(System2Domain.GetInstance().BondModuleController.Get3DRealPosition(), matchResult);

            //System2Domain.GetInstance().BondModuleController.MoveToG0Pos(point3D);

            //  AKRSPoint3D point3D1 = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;

            //  AKRSPoint3D point3D2 = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();


            //  AKRSPoint3D offset = point3D1 - point3D2;


        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            FrmTemperatureDistribution frmTemperatureDistribution = new FrmTemperatureDistribution();
            frmTemperatureDistribution.Show();
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            string name = "上视一点定位测试";

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

            prEntity.SetHardware(System2Domain.GetInstance().System2Controller.GetHardware(CameraTypeEnum.UpLookCamera));

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        public double Distance(AKRSPoint3D point3D,AKRSPoint3D point3D1)
        {
            double x = point3D.X - point3D1.X;
            double Y = point3D.Y - point3D1.Y;

            return Math.Sqrt(x * x - Y * Y);

        }

        public double Distance(double X,double Y)
        {
            //double x = point3D.X - point3D1.X;
            //double Y = point3D.Y - point3D1.Y;

            return Math.Sqrt(Math.Abs(X * X - Y * Y));

        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Users\liujiangxian\Desktop\27日");

            //List<string> listDate = new List<string>();

            //List<string> listForce = new List<string>();

            //foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            //{
            //    ExcelPackage excelPackage = new ExcelPackage(fileInfo);
            //    ExcelWorksheet excelWorksheet1 = excelPackage.Workbook.Worksheets[0];

            //    for (int i = 1; i < excelWorksheet1.Rows.Count(); i++)
            //    {
            //        listDate.Add(excelWorksheet1.Cells[i + 1, 1].Value.ToString());
            //        listForce.Add(excelWorksheet1.Cells[i + 1, 2].Value.ToString());
            //    }

            //    listDate.Add("");
            //    listForce.Add("");
            //}

            for (int i = 0; i < 4; i++)
            {
                System2Domain.GetInstance().BondHeadController.RotateAxisT(15 * i);

                AKRSPoint3D cameraCenter = new AKRSPoint3D(-102.31035, -311.0708, 103.326);

                MatchResult matchResult1 = (MatchResult)System2Domain.GetInstance().System2Controller.UpLookCameraVision(
                   cameraCenter,
                   "上视一点定位测试");

                AKRSPoint3D point3D5 = System2Domain.GetInstance().UpLookController.ConvertPixelToG0Pos(new AKRSPoint3D(), matchResult1) -
                    System2Domain.GetInstance().UpLookController.ConvertPixelToG0Pos(new AKRSPoint3D(), new MatchResult());

                double dis = Distance(point3D5.X, point3D5.Y);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton7_Click(object sender, EventArgs e)
        {

            #region 读取数据

            DirectoryInfo directoryInfo = new DirectoryInfo(@"E:\资料\温漂资料\热机Y轴来回跑");

            List<double> doubles = new List<double>();

            List<List<(double, double)>> totalList = new List<List<(double, double)>>();

            // 遍历文件夹中的所有文件
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                List<(double, double)> list = new List<(double, double)>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // 读取数据
                    for (int row = 1; row < worksheet.Dimension.End.Row; row++)
                    {
                        object cellValue1 = worksheet.Cells[row, 1].Value;
                        object cellValue2 = worksheet.Cells[row, 2].Value;

                        if (cellValue1 == null || cellValue2 == null)
                        {
                            continue;
                        }

                        list.Add(((double)cellValue1, (double)cellValue2));
                    }
                }

                totalList.Add(list);
            }

            #endregion

            // 创建一个新的 Excel 包
            ExcelPackage excelPackage = new ExcelPackage();
            var workbook = excelPackage.Workbook;
            var worksheet1 = workbook.Worksheets.Add("Sheet1");

            for (int i = 0; i < totalList.Count; i++)
            {
                for (int j = 0; j < totalList[i].Count; j++)
                {
                    worksheet1.Cells[j + 1, i + 1].Value = totalList[i][j].Item1 - totalList[0][j].Item1;
                }
            }

            
            var worksheet2 = workbook.Worksheets.Add("Sheet2");

            for (int i = 0; i < totalList.Count; i++)
            {
                for (int j = 0; j < totalList[i].Count; j++)
                {
                    worksheet2.Cells[j + 1, i + 1].Value = totalList[i][j].Item2 - totalList[0][j].Item2;
                }
            }
            
            // 保存文件
            excelPackage.SaveAs(new System.IO.FileInfo(@"E:\资料\温漂资料\热机Y轴来回跑\汇总1.xlsx"));
            Console.WriteLine("Excel 文件已创建！");
            
        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            #region 读取数据

            DirectoryInfo directoryInfo = new DirectoryInfo(@"E:\资料\温漂资料\新建文件夹(1)");

            List<double> doubles = new List<double>();

            List<List<(double, double)>> totalList = new List<List<(double, double)>>();

            // 遍历文件夹中的所有文件
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                List<(double, double)> list = new List<(double, double)>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // 读取数据
                    for (int row = 1; row < worksheet.Dimension.End.Row; row++)
                    {
                        object cellValue1 = worksheet.Cells[row, 1].Value;
                        object cellValue2 = worksheet.Cells[row, 2].Value;

                        if (cellValue1 == null || cellValue2 == null)
                        {
                            continue;
                        }

                        list.Add(((double)cellValue1, (double)cellValue2));
                    }
                }

                totalList.Add(list);
            }

            #endregion

            // 创建一个新的 Excel 包
            ExcelPackage excelPackage = new ExcelPackage();
            var workbook = excelPackage.Workbook;
            var worksheet1 = workbook.Worksheets.Add("Sheet1");

            for (int i = 0; i < totalList.Count; i++)
            {
                for (int j = 0; j < totalList[i].Count; j++)
                {
                    worksheet1.Cells[j + 1, i + 1].Value = totalList[i][j].Item1 - totalList[0][j].Item1;
                }
            }


            var worksheet2 = workbook.Worksheets.Add("Sheet2");

            for (int i = 0; i < totalList.Count; i++)
            {
                for (int j = 0; j < totalList[i].Count; j++)
                {
                    worksheet2.Cells[j + 1, i + 1].Value = totalList[i][j].Item2 - totalList[0][j].Item2;
                }
            }

            // 保存文件
            excelPackage.SaveAs(new System.IO.FileInfo(@"E:\资料\温漂资料\新建文件夹(1)\汇总1.xlsx"));
            Console.WriteLine("Excel 文件已创建！");
        }


        /// <summary>
        /// 测试坐标系
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton9_Click(object sender, EventArgs e)
        {
            GeneralCoordinateSystem g0 = new GeneralCoordinateSystem() { Name = "G0" };

            GeneralCoordinateSystem generalCoordinateSystem =
                new GeneralCoordinateSystem("测试坐标系", CoordinateSystemTypeEnum.General, g0);

            generalCoordinateSystem.InitOffSet(new AKRSPoint3D(1, 1, 0), Math.PI / 180.0 * 45);

            generalCoordinateSystem.UpperCoordinateSystem = g0;

            AKRSPoint3D akrsPoint3D = generalCoordinateSystem.SelfPosToG0(new AKRSPoint3D(1,1,0));

            AKRSPoint3D akrsPoint3D3 = generalCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, 0));

            generalCoordinateSystem.UpdateCoordinateSystem(new AKRSPoint3D(1, 1, 0), new AKRSPoint3D(0, 0, 0), 0);

            AKRSPoint3D akrsPoint3D2 = generalCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, 0));

            AKRSPoint3D akrsPoint3D4 = generalCoordinateSystem.SelfPosToG0(akrsPoint3D);
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            SubstrateConfig substrateConfig = ProductConfiguration.GetInstance().SubstrateConfig;

            int count = 5;

            substrateConfig.IsMultiple = true;

            substrateConfig.RowCount = count;

            substrateConfig.ColumnCount = count;

            substrateConfig.Count = count * count;
            substrateConfig.ElementCoordinates.Clear();
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    substrateConfig.ElementCoordinates.Add(new ElementCoordinate(0, new AKRSPoint3D(j, i, 0)));
                }
            }

            int count2 = 5;

            ProductConfiguration.GetInstance().ModuleConfig.IsMultiple = true;

            ModuleConfig moduleConfig = ProductConfiguration.GetInstance().ModuleConfig;

            moduleConfig.IsMultiple = true;

            moduleConfig.Count = count2 * count2;

            moduleConfig.RowCount = count2;

            moduleConfig.ColumnCount = count2;

            moduleConfig.ElementCoordinates.Clear();
            for (int i = 0; i < count2; i++)
            {
                for (int j = 0; j < count2; j++)
                {
                    moduleConfig.ElementCoordinates.Add(new ElementCoordinate(0, new AKRSPoint3D(j, i, 0)));
                }
            }

            BondPositionConfig bondPositionConfig = ProductConfiguration.GetInstance().BondPositionConfig;

            bondPositionConfig.SingleBpPositionConfigList.Clear();

            for (int i = 0; i < 1; i++)
            {
                bondPositionConfig.SingleBpPositionConfigList.Add(
                    new SingleBondPositionConfig()
                    {
                        ElementCoordinate = new ElementCoordinate(0, new AKRSPoint3D(i, i, 0)),
                        Name = $"焊点{i}"
                    });
            }


            TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit = new TransportUnit("测试");

            UcMainSystem.ReFreshSystem2TuAction();
        }

        /// <summary>
        /// 插补
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton11_Click(object sender, EventArgs e)
        {
            //////Task.Run(() => {

            //////});

            //EpoxyApplication EpoxyApplication = (EpoxyApplication)EpoxyApplicationRepository.GetInstance().Find("Test123");


            //for (int i = 0; i < EpoxyApplication.DispensePatternParas.Count; i++)
            //{
            //    GuGaoDrive.InterpolationThree(2, EpoxyApplication.DispensePatternParas[i], 15);
            //}

            //EpoxyApplicationRepository.GetInstance().Save();
            ////BondModule bondModule = new BondModule();
            //////bondModule.BondHead.AxisZ.AbsoluteMove(-2.5584);

            ////BondDevicePara.GetInstance().BondHeadParam.AxisSafePos = bondModule.Get3DRealPosition();
            ////BondDevicePara.GetInstance().Save();
            ///

            //RtuConnectConfig.GetInstance().ManometerSerialPort = "COM10";
            //RtuConnectConfig.GetInstance().Save();
            //BondDevicePara.GetInstance().Save();
            //ModbusService.GetInstance().ConnectManometer();
            //double a = ModbusService.GetInstance().ReadManometer()[0] / 10.0;
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            //TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit = new TransportUnit();

            //System1Domain.GetInstance().DispenseWorkTask.Start();

            AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();
            System2Domain.GetInstance().S2DispenseController.MeasureHeightMoveToG0Pos(point3D);
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            //BondModule bondModule = new BondModule();
            //bondModule.BondHead.AxisZ.AbsoluteMove(-10);

            //Thread.Sleep(1000);

            //List<double> doubles = new List<double>();
            //for (int i = 0; i < 10; i++)
            //{
            //    bondModule.BondHead.AxisZ.RelativeMove(-0.2);
            //    Thread.Sleep(0);
            //    double a = System2Domain.GetInstance().S2DispenseController.LaserMeasureHeight();
            //    doubles.Add(a);
            //}

            //FrmAssistantDistanceMhAndBh assistantDistanceMhAndBh = new FrmAssistantDistanceMhAndBh();
            //assistantDistanceMhAndBh.Show();


            ////FrmAssistantDispense frmAssistantDispense = new FrmAssistantDispense();
            ////frmAssistantDispense.ShowDialog();

            ////System2Domain.GetInstance().S2DispenseController.ForceMeasureHeight();



            //AKRSPoint3D aKRSPoint3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

            //System2Domain.GetInstance().S2DispenseController.MeasureHeightMoveToG0Pos(aKRSPoint3D);

            List<double> doubles = new List<double>();

            for (int i = 0; i < 10; i++)
            {
                BondHeadController bondHeadController = new BondHeadController();
                (ExcuteResult Ret, double a) = bondHeadController.MeasureHeight(bondHeadController.GetAxisZRealPos(), HeightMeasurementFunctionEnum.ForceSensor);
                doubles.Add(a);
            }
           
        }


        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton14_Click(object sender, EventArgs e)
        {
          TransportUnit transportUnit = TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit = new TransportUnit("测试");

            Task.Run(
                () =>
                    {
                        foreach (Substrate substrate in transportUnit.Substrates)
                        {
                            

                        }

                    });
        }

        Task t;

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            MotionPlanDomain.GetInstance().PickSafeAreaPoint1 = new AKRSPoint3D(-19.27005, -284.13075, 117.707);
            MotionPlanDomain.GetInstance().PickSafeAreaPoint2 = new AKRSPoint3D(90.32115, -284.13075, 117.707);
            MotionPlanDomain.GetInstance().PickSafeAreaPoint3 = new AKRSPoint3D(90.32115, -327.89315, 0);
            MotionPlanDomain.GetInstance().PickSafeAreaPoint4 = new AKRSPoint3D(-19.27005, -327.89315, 0);

            MotionPlanDomain.GetInstance().Save();
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
           // AKRSPoint3D startpos = new AKRSPoint3D(1.63215, -216.87165, 123.1381);

           //// 移动到准备取片的位置
           // System2Domain.GetInstance().BondModuleController.MoveToG0Pos(startpos);

           // List<AKRSPoint3D> point = MotionPlanDomain.GetInstance().GetPickPath(
           //    startpos,
           //     new AKRSPoint3D(27.33995, -313.23075, 55.8814));

           // //List<AKRSPoint3D> point = new List<AKRSPoint3D>();
           // //point.Add(new AKRSPoint3D(27.33995, -313.23075, 123.1381));
           // //point.Add(new AKRSPoint3D(27.33995, -313.23075, 55.8814));

           // InterpolationParam interpolationParam = new InterpolationParam();

           // // 全局速度百分比
           // double vel = 10;

           // interpolationParam.ListNo = 2;
           // interpolationParam.GrpCrd = 2;
           // interpolationParam.Vel = vel;
           // interpolationParam.Acc = 0.1;
           // interpolationParam.AccAcc = 0.1;

           // interpolationParam.AxisDrives = System2Domain.GetInstance().BondModuleController.GetBondIpolAxis();

           // int count = point.Count;

           // interpolationParam.SegmentConfigs = new SegmentConfig[count];
           // for (int i = 0; i < point.Count; i++)
           // {
           //     AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.ConvertG0ToMachinePos(point[i]);

           //     interpolationParam.SegmentConfigs[i] =
           //         new SegmentConfig() { Point = new AKRSPoint4D(point3D.X, point3D.Y, point3D.Z, 0) };
           // }

           // foreach (var segmentConfig in interpolationParam.SegmentConfigs)
           // {
           //     // 加减速度默认*10
           //     segmentConfig.Velocity = vel;
           //     segmentConfig.Acc = 0.01;
           //     segmentConfig.Dec = 0.01;
           //     segmentConfig.AccAcc = 0.03;
           // }

           // //// 获取卡
           // //AxisCard card = HardwareRepositoryService.GetHardwaresByType<AxisCard>().Find(
           // //    card =>
           // //        card.AxisList.Select(axis => axis.AxisDrive).Exists(
           // //            drive => drive == interpolationParam.AxisDrives[0]));

           // //card.MotionController.ContinueInterpolationMove(interpolationParam);

           //  this.ThreeAxisMove(point);
        }

        private void ThreeAxisMove(List<AKRSPoint3D> point3Ds)
        {
            IAxisDrive[] axisDrives = System2Domain.GetInstance().BondModuleController.GetBondIpolAxis();

            // 获取ETEL轴
            AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

            // 获取控制器
            DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

            // 设置群组
            DsaIpolGroup iGroup = new DsaIpolGroup(
                ((ETELAxis)axisDrives[0]).GetDrive(),
                ((ETELAxis)axisDrives[1]).GetDrive(),
                ((ETELAxis)axisDrives[2]).GetDrive());

            // 设置控制者
            iGroup.setMaster(dsaMaster);

            // 开始插补
            iGroup.ipolBegin();

            // 设置为绝对坐标系 ，不设置绝对坐标系
            iGroup.ipolSetAbsMode(true, -1);

            foreach (AKRSPoint3D point3D in point3Ds)
            {
                AKRSPoint3D point3D1 = System2Domain.GetInstance().BondModuleController.ConvertG0ToMachinePos(point3D);

                // 设置速度,Etel的单位为M/s，外接传入为mm/s,需要转换
                iGroup.ipolTanVelocity(500 / 1000.0);

                // 设置加速度
                iGroup.ipolTanAcceleration(
                    5000 / 1000.0);
                iGroup.ipolTanDeceleration(5000 / 1000.0);

                iGroup.ipolTanJerkTime(0.1);

                iGroup.ipolLine(new DsaVector(point3D1.X / 1000.0, point3D1.Y / 1000.0, -point3D1.Z / 1000.0, 0));

                iGroup.ipolContinue();
            }

            // 等待插补结束
            iGroup.ipolWaitMovement(100000);

            // 退出插补模式
            iGroup.ipolEnd();
        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {
            //UcRealTimeHeight realTimeHeight = new UcRealTimeHeight() { Dock = DockStyle.Fill };
            //Form form = new Form();
            //form.Controls.Add(realTimeHeight);
            //form.Show();
            //Random random = new Random();
            //Task.Run(
            //    () =>
            //        {
            //            while (true)
            //            {
            //                realTimeHeight.AddHeightData(random.Next(10) / 1000.0);
            //                Thread.Sleep(100);
            //            }
            //        });


            AKRSPoint3D aKRSPoint3D = new AKRSPoint3D(-60.046, -235.61925, 113.0299);

            for (int i = 0; i < 8; i++)
            {
                System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(aKRSPoint3D.X + i*31, aKRSPoint3D.Y, aKRSPoint3D.Z));
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton18_Click(object sender, EventArgs e)
        {
            FrmSingleBpData frmSingleBpData = new FrmSingleBpData();

            frmSingleBpData.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton19_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                List<MatchResult> matchResults = new List<MatchResult>();

                List<AKRSPoint3D> list = new List<AKRSPoint3D>();

                list.Add(new AKRSPoint3D(-58.0899749999999, -233.134675, 98.5422));
                list.Add(new AKRSPoint3D(-13.3548749999999, -233.134675, 98.5422));
                list.Add(new AKRSPoint3D(45.8978250000001, -233.134675, 98.5422));
                list.Add(new AKRSPoint3D(106.945425, -233.134675, 98.5422));
                list.Add(new AKRSPoint3D(161.785525, -233.134675, 98.5422));
                list.Add(new AKRSPoint3D(-58.0899749999999, -182.729975, 98.5422));
                list.Add(new AKRSPoint3D(-13.3548749999999, -182.729975, 98.5422));
                list.Add(new AKRSPoint3D(45.8978250000001, -182.729975, 98.5422));
                list.Add(new AKRSPoint3D(106.945425, -182.729975, 98.5422));
                list.Add(new AKRSPoint3D(161.785525, -186.833875, 98.5422));

                while (true)
                {
                    for (int i = 0; i < 100; i++)
                    {
                        for (int j = 0; j < list.Count; j++)
                        {
                            MatchResult matchResult = this.BondTest(list[j]);

                            if (matchResult != null)
                            {
                                matchResults.Add(matchResult);
                            }
                        }

                    }

                    double distanceX = matchResults.Max(it => it.CenterX) - matchResults.Min(it => it.CenterX);
                    double distanceY = matchResults.Max(it => it.CenterY) - matchResults.Min(it => it.CenterY);

                    // 创建一个新的 Excel 包
                    ExcelPackage excelPackage = new ExcelPackage();
                    var workbook = excelPackage.Workbook;
                    var worksheet1 = workbook.Worksheets.Add("Sheet1");

                    for (int i = 0; i < matchResults.Count; i++)
                    {
                        worksheet1.Cells[i + 1, 1].Value = matchResults[i].CenterX;

                        worksheet1.Cells[i + 1, 2].Value = matchResults[i].CenterY;
                    }

                    string test = DateTime.Now.ToString("MM-dd-HH-mm-ss-fff");

                    // 保存文件
                    excelPackage.SaveAs(new System.IO.FileInfo(@$"D:\取放测试\{test}.xlsx"));
                    Console.WriteLine("Excel 文件已创建！");
                }
            });

        }

        /// <summary>
        /// Bond测试
        /// </summary>
        /// <returns>结果</returns>
        private MatchResult BondTest(AKRSPoint3D point3D)
        {
            AKRSPoint3D aKRSPoint3D = new AKRSPoint3D(-27.7225, -1.6122, 0);

            Electric electric = HardwareRepositoryService.GetHardware<Electric>("固晶区吸料真空");

            #region 贴片

            // 移动到位
            System2Domain.GetInstance().BondModuleController.MoveToG0Pos(point3D);

            double curLevel = System2Domain.GetInstance().BondHeadController.GetAxisZRealPos();

            // 力控
            System2Domain.GetInstance().BondHeadController.ForceControlSet(50, curLevel, 100);

            // 开启载台真空
            electric.SetOutputValue(true);

            // 停留
            Thread.Sleep(100);

            // 关闭吸嘴真空
            System2Domain.GetInstance().BondHeadController.CloseToolVaccum();

            // 打开弱吹
            System2Domain.GetInstance().BondHeadController.OpenToolBlowEle(500);

            // 停留
            Thread.Sleep(100);

            // 打开弱吹
            System2Domain.GetInstance().BondHeadController.CloseToolBlowEle();

            // t退出力控并上抬
            System2Domain.GetInstance().BondHeadController.ForceControlReset(curLevel, 10);

            #endregion

            #region 定位

            // 移动到位
            System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(point3D.X + aKRSPoint3D.X, point3D.Y + aKRSPoint3D.Y, 103.6349));

            Thread.Sleep(100);

            // 视觉定位
            // 执行定位
            BaseAlgResult baseAlg = VisionService.Vision(
                "测试",
                System2Domain.GetInstance().BondModuleController.GetHardware(),
                "Bond",
                "Test",
                false);

            #endregion

            #region 取片

            // 取片
            System2Domain.GetInstance().BondModuleController.MoveToG0Pos(point3D);

            // 力控
            System2Domain.GetInstance().BondHeadController.ForceControlSet(50, curLevel, 100);

            // 打开吸嘴真空
            System2Domain.GetInstance().BondHeadController.OpenToolVaccum();

            // 停留
            Thread.Sleep(100);

            // 关闭载台真空
            electric.SetOutputValue(false);

            // 停留
            Thread.Sleep(100);

            // t退出力控并上抬
            System2Domain.GetInstance().BondHeadController.ForceControlReset(curLevel, 10);

            #endregion

            return (MatchResult)baseAlg;
        }

        /// <summary>
        /// 画胶力控联合测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton20_Click(object sender, EventArgs e)
        {
            //System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(10.49645, -205.4627, 135.2444));

            //EpoxyApplication epoxyApplication = (EpoxyApplication)EpoxyApplicationRepository.GetInstance().Find("12222323");

            //EpoxyApplication epoxy = DispenserMoveHelper.EpoxyPretreatment(epoxyApplication, System2Domain.GetInstance().BondModuleController.Get3DRealPosition(), 0);

            //GuGaoDrive.Interpolation(2, epoxy.DispensePatternParas[0], 0);

            //System2Domain.GetInstance().BondModuleController.MoveToG0Pos(new AKRSPoint3D(10.49645, -205.4627, 135.2444));


            // 取片

            // 去上视

            // 定位

            // 去贴片
        }

        /// <summary>
        /// 力控
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton21_Click(object sender, EventArgs e)
        {
           
        }

        private void 存图测试_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                    {
                        while (true)
                        {
                           Bitmap bitmap = new Bitmap(10000, 10000);
                        }
                    });
        }

        private void simpleButton22_Click(object sender, EventArgs e)
        {
            VisionService.SaveBitmap();
        }


        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //System2Domain.GetInstance().ActionNodesService.Init();

            Electric WeakBlowProportionalElectric = HardwareRepositoryService.GetHardware<Electric>("邦头散热比例阀");

            WeakBlowProportionalElectric.WriteRxPDO((ushort)WeakBlowProportionalElectric.ElectricIO, 1, 10000);
        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                    {
                        AKRSXtraMessageBox.Show("测试");
                    });

            Task.Run(
                () =>
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"取片失败2! \r\n" + e.ToString(),
                            "取片报警",
                            new string[] { "确认" },
                            new DialogResult[] { DialogResult.OK },
                            AlarmLevel.SecondLevel);
                    });

            Task.Run(
                () =>
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                            $"取片失败3! \r\n" + e.ToString(),
                            "取片报警",
                            new string[] { "确认" },
                            new DialogResult[] { DialogResult.OK },
                            AlarmLevel.SecondLevel);
                    });

            Task.Run(
                () =>
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                            $"取片失败4! \r\n" + e.ToString(),
                            "取片报警",
                            new string[] { "确认" },
                            new DialogResult[] { DialogResult.OK },
                            AlarmLevel.SecondLevel);
                    });
        }

        private void FrmTest_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            UcMainSystem.InputDebugAction("测试");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton25_Click(object sender, EventArgs e)
        {

        }
    }
}