using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Experiment.TestFunction
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
    using System.Threading;

    public partial class FrmTest2 : DevExpress.XtraEditors.XtraForm
    {
        public FrmTest2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 相机实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("下视测试");

            List<List<double>> list = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(1000);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;

                list.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            FileHelper.SaveDoubleExcel(list, $"D:\\精度实验\\相机实验\\{DateTime.Now.ToFileTime().ToString()}.xlsx");
        }

        /// <summary>
        /// 运动精度实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("下视测试");

            List<List<double>> list = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                Random random = new Random();

                double offsetX = random.Next(0, 60);
                double offsetY = random.Next(-30, 30);
                System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X + offsetX, point3D.Y + offsetY);

                System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X, point3D.Y);

                Thread.Sleep(1000);

                prEntity.DoWork();
                
                MatchResult matchResult = (MatchResult)prEntity.AlgResult;

                list.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            FileHelper.SaveDoubleExcel(list, $"D:\\精度实验\\运动实验\\{DateTime.Now.ToFileTime().ToString()} .xlsx");
        }

        /// <summary>
        /// 取放片实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {
            // 获取当前位置
            AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

            // 减去相机和焊头之间的差值
            point3D += System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset;

            List<List<double>> list = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                // 取片
                this.Pick(point3D);

                // 放片
                this.Bond(point3D);

                // 移动到位
                System2Domain.GetInstance().BondModuleController.MoveBondXYZWithoutSafe(
                    point3D - System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset);

                System2Domain.GetInstance().BondHeadController.MoveAxisZ((point3D - System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset).Z);

                // 取片
                System2Domain.GetInstance().BondModuleController.MoveBondXY((point3D - System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset).X, 
                    (point3D - System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset).Y);

                Thread.Sleep(100);

                // 执行定位
                BaseAlgResult baseAlg = VisionService.Vision(
                    "下视测试",
                    System2Domain.GetInstance().BondModuleController.GetHardware(),
                    "Bond",
                    "下视测试",
                    false);

                MatchResult matchResult = (MatchResult)baseAlg;

                list.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            FileHelper.SaveDoubleExcel(list, $"D:\\精度实验\\重复取放实验\\{DateTime.Now.ToFileTime().ToString()} .xlsx");
        }

        #region 取放定位

        /// <summary>
        /// 取片
        /// </summary>
        /// <param name="point3D">点位</param>
        private void Pick(AKRSPoint3D point3D)
        {
            System2Domain.GetInstance().BondHeadController.MoveAxisZ(point3D.Z + 5);

            // 取片
            System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X, point3D.Y);

            // 力控
            System2Domain.GetInstance().BondHeadController.ForceControlSet(50, point3D.Z+ 3, 100);

            Thread.Sleep(200);

            // 打开吸嘴真空
            System2Domain.GetInstance().BondHeadController.OpenToolVaccum();

            // 停留
            Thread.Sleep(100);

            Electric VaccumElectric = HardwareRepositoryService.GetHardware<Electric>("中转台真空电磁阀");

            VaccumElectric.SetOutputValue(false);

            // 停留
            Thread.Sleep(100);

            // t退出力控并上抬
            System2Domain.GetInstance().BondHeadController.ForceControlReset(point3D.Z+2, 10);
        }

        /// <summary>
        /// 贴片
        /// </summary>
        /// <param name="point3D">位置</param>
        private void Bond(AKRSPoint3D point3D)
        {
            // 移动到位
            System2Domain.GetInstance().BondHeadController.MoveAxisZ(point3D.Z + 5);

            // 取片
            System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X, point3D.Y);

            // 移动到位
            System2Domain.GetInstance().BondHeadController.MoveAxisZ(point3D.Z + 5);

            // 力控
            System2Domain.GetInstance().BondHeadController.ForceControlSet(50, point3D.Z + 2, 100);


            Thread.Sleep(500);

            Electric VaccumElectric = HardwareRepositoryService.GetHardware<Electric>("中转台真空电磁阀");

            VaccumElectric.SetOutputValue(true);

            Thread.Sleep(500);

            // 关闭吸嘴真空
            System2Domain.GetInstance().BondHeadController.CloseToolVaccum();

            Task task = Task.Run(() => 
            {
                // 打开弱吹
                System2Domain.GetInstance().BondHeadController.OpenToolBlowEle(100);

            });

            // t退出力控并上抬
            System2Domain.GetInstance().BondHeadController.ForceControlReset(point3D.Z + 2, 10);

            task.Wait();

            // 打开弱吹
            System2Domain.GetInstance().BondHeadController.CloseToolBlowEle();

        }

        /// <summary>
        /// 定位
        /// </summary>
        private AKRSPoint3D VisionResult(AKRSPoint3D point3D)
        {
            // 移动到位
            System2Domain.GetInstance().BondModuleController.MoveBondXYZWithoutSafe(
                point3D + System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset);

            // 执行定位
            BaseAlgResult baseAlg = VisionService.Vision(
                "测试",
                System2Domain.GetInstance().BondModuleController.GetHardware(),
                "Bond",
                "Test",
                false);

            MatchResult matchResult = (MatchResult)baseAlg;

            return System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                 new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
        }

        #endregion

        /// <summary>
        /// 下视相机模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("下视测试");
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity("下视测试");
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Substrate;


                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            prEntity.SetHardware(System2Domain.GetInstance().System2Controller.GetHardware(CameraTypeEnum.BondCamera));

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 上视相机模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton5_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("上视测试");
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity("上视测试");
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Substrate;


                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            prEntity.SetHardware(System2Domain.GetInstance().System2Controller.GetHardware(CameraTypeEnum.UpLookCamera));

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 随机取，上视矫正
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton6_Click(object sender, EventArgs e)
        {
            Task.Run(() => 
            {
                // 获取当前位置
                AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

                // 减去相机和焊头之间的差值
                point3D += System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset;

                List<List<double>> list = new List<List<double>>();

                AKRSPoint3D point3D3 = new AKRSPoint3D(35.5361, -211.5189, -27.318);

                PREntity prEntity2 = (PREntity)VisionEntityRepository.GetInstance().Find("上视温漂测试Mark");

                for (int i = 0; i < 5; i++)
                {
                    // 取片
                    this.Pick(point3D);

                    // 温漂
                    System2Domain.GetInstance().BondHeadController.MoveAxisZ(point3D3.Z);
                    System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D3.X, point3D3.Y);

                    Thread.Sleep(200);

                    prEntity2.DoWork();

                    MatchResult matchResult2 = (MatchResult)prEntity2.AlgResult;


                    // 上视
                    System2Domain.GetInstance().BondHeadController.MoveAxisZ(-25.2371);
                    System2Domain.GetInstance().BondModuleController.MoveBondXY(57.453, -234.033);

                    Thread.Sleep(200);

                    //// 上视定位
                    //System2Domain.GetInstance().BondModuleController.MoveBondXYZ(new AKRSPoint3D(57.453,-234.033,-25.2371));

                    // 执行定位
                    MatchResult matchResultUpLook = (MatchResult)VisionService.Vision(
                        "上视测试",
                        System2Domain.GetInstance().UpLookController.GetHardware(),
                        "Bond",
                        "上视测试",
                        false);

                    AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.UpLookCameraCoordinateSystem.ForwardConvertCoordinate(
                        new AKRSPoint3D(matchResultUpLook.CenterX, matchResultUpLook.CenterY, 0));

                    // 放片
                    this.Bond(point3D + point3D2);

                    // 移动到位
                    System2Domain.GetInstance().BondModuleController.MoveBondXYZWithoutSafe(
                        point3D - System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset);

                    Thread.Sleep(200);

                    // 执行定位
                    BaseAlgResult baseAlg = VisionService.Vision(
                        "下视测试",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "下视测试",
                        false);

                    MatchResult matchResult = (MatchResult)baseAlg;

                    list.Add(
                        new List<double>()
                            {
                                matchResult.CenterX,
                                matchResult.CenterY,
                                matchResultUpLook.CenterX,
                                matchResultUpLook.CenterY,
                                matchResult2.CenterX,
                                matchResult2.CenterY
                            });
                }

                FileHelper.SaveDoubleExcel(list, $"D:\\精度实验\\重复取放实验\\{DateTime.Now.ToFileTime().ToString()} .xlsx");
            });
        }


        /// <summary>
        /// 回相机中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton7_Click(object sender, EventArgs e)
        {

            Task.Run(() =>
            {
                AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

                PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("下视测试");

                while (true)
                {
                    Thread.Sleep(1000);

                    prEntity.DoWork();

                    MatchResult matchResult2 = (MatchResult)prEntity.AlgResult;

                    //point3D = System2Module.GetInstance().UpLookModule.UpLookCameraCoordinateSystem.ForwardConvertCoordinate(
                    //    new AKRSPoint3D(matchResult2.CenterX, matchResult2.CenterY, 0)) + point3D;

                    point3D = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                      new AKRSPoint3D(matchResult2.CenterX, matchResult2.CenterY, 0)) + point3D;

                    if (Math.Abs(matchResult2.CenterX - 1224) < 0.5 && Math.Abs(matchResult2.CenterY - 1024) < 0.5)
                    {
                        break;
                    }

                    System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X, point3D.Y);
                }

                List<List<double>> list = new List<List<double>>();

                for (int i = 0; i < 100; i++)
                {
                    Random random = new Random();

                    double offsetX = random.Next(-10, 10) / 10.0;

                    //if (offsetX > 0)
                    //{
                    //    offsetX += 1;
                    //}
                    //else
                    //{
                    //    offsetX -= 1;
                    //}

                    double offsetY = random.Next(-10, 10) / 10.0;

                    //if (offsetY > 0)
                    //{
                    //    offsetY += 1;
                    //}
                    //else
                    //{
                    //    offsetY -= 1;
                    //}

                    System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X + offsetX, point3D.Y + offsetY);

                    //System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X, point3D.Y);

                    Thread.Sleep(200);

                    prEntity.DoWork();

                    MatchResult matchResult = (MatchResult)prEntity.AlgResult;


                    //AKRSPoint3D point3D1 = System2Module.GetInstance().UpLookModule.UpLookCameraCoordinateSystem.ForwardConvertCoordinate(
                    // new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0)) + System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

                    AKRSPoint3D point3D1 = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0)) + System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

                    System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D1.X, point3D1.Y);

                    Thread.Sleep(200);

                    prEntity.DoWork();

                    MatchResult matchResult2 = (MatchResult)prEntity.AlgResult;

                    list.Add(new List<double>() { matchResult2.CenterX, matchResult2.CenterY });

                }

                FileHelper.SaveDoubleExcel(list, $"D:\\精度实验\\相机回中心实验\\{DateTime.Now.ToFileTime().ToString()} .xlsx");

            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton18_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                while (false)
                {
                    AKRSPoint3D point3D = new AKRSPoint3D(67.3903, -170.3301, -25.509);
                    AKRSPoint3D point3D2 = new AKRSPoint3D(35.5361, -211.5189, -27.318);

                    PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("下视测试");
                    PREntity prEntity2 = (PREntity)VisionEntityRepository.GetInstance().Find("上视温漂测试Mark");

                    List<List<double>> list = new List<List<double>>();

                    for (int i = 0; i < 100; i++)
                    {

                        System2Domain.GetInstance().BondHeadController.MoveAxisZ(point3D2.Z);
                        System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D2.X, point3D2.Y);

                        Thread.Sleep(200);

                        prEntity2.DoWork();

                        MatchResult matchResult2 = (MatchResult)prEntity2.AlgResult;

                        list.Add(new List<double>() { matchResult2.CenterX, matchResult2.CenterY });

                        for (int j = 0; j < 5; j++)
                        {
                            for (int k = 0; k < 20; k++)
                            {
                                System2Domain.GetInstance().BondHeadController.MoveAxisZ(point3D.Z);
                                System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X + k * 10.6, point3D.Y + j * 10.6);

                                Thread.Sleep(200);

                                prEntity.DoWork();

                                MatchResult matchResult = (MatchResult)prEntity.AlgResult;

                                list.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY,
                            System2Domain.GetInstance().BondModuleController.Get2DRealPosition().X, System2Domain.GetInstance().BondModuleController.Get2DRealPosition().Y });
                            }
                        }
                    }

                    FileHelper.SaveDoubleExcel(list, $"D:\\精度实验\\运动实验\\{DateTime.Now.ToFileTime().ToString()} .xlsx");
                    list.Clear();
                }


            });

           
        }



        /// <summary>
        /// Bond相机标定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton21_Click(object sender, EventArgs e)
        {
            Task.Run(() => 
            {
                // 移动到中心
                AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

                PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("下视测试");

                while (true)
                {
                    Thread.Sleep(1000);

                    prEntity.DoWork();

                    MatchResult matchResult2 = (MatchResult)prEntity.AlgResult;

                    point3D = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                                  new AKRSPoint3D(matchResult2.CenterX, matchResult2.CenterY, 0)) + point3D;

                    if (Math.Abs(matchResult2.CenterX - 1224) < 0.2 && Math.Abs(matchResult2.CenterY - 1024) < 0.2)
                    {
                        break;
                    }

                    System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X, point3D.Y);
                }


                List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();
                List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();


                // 跑点位
                for (int i = 0; i < 5 ; i++)
                {
                    for (int j = 0; j <= 5; j++)
                    {
                        double x = (3 - j) * 0.6 - 0.3;
                        double y = (2 - i) * 0.5;
                        System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X + x, point3D.Y + y);

                        Thread.Sleep(100);

                        prEntity.DoWork();

                        MatchResult matchResult2 = (MatchResult)prEntity.AlgResult;

                        realPoints.Add(new AKRSPoint3D((float)x, (float)y, 0));
                        imgPoints.Add(new AKRSPoint3D((float)matchResult2.CenterX, (float)matchResult2.CenterY, 0));
                    }
                }


                // 坐标系转换
                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("BondCameraCoordinateSystem", "BondCoordinateSystem", false, CoordinateSystemTypeEnum.Vm);

                DependentCoordinateSystem dependentCoordinateSystem = (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "BondCameraCoordinateSystem");
                dependentCoordinateSystem.Init(realPoints, imgPoints);

                MachineCoordinateSystem.GetInstance().Save();
                MachineCoordinateSystem.Refresh();
            });

        }


        /// <summary>
        /// 上视相机
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton22_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                // 移动到中心
                AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

                PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("上视测试");

                while (true)
                {
                    Thread.Sleep(1000);

                    prEntity.DoWork();

                    MatchResult matchResult2 = (MatchResult)prEntity.AlgResult;

                    point3D = System2Module.GetInstance().UpLookModule.UpLookCameraCoordinateSystem.ForwardConvertCoordinate(
                                  new AKRSPoint3D(matchResult2.CenterX, matchResult2.CenterY, 0)) + point3D;

                    if (Math.Abs(matchResult2.CenterX - 1224) < 0.2 && Math.Abs(matchResult2.CenterY - 1024) < 0.2)
                    {
                        break;
                    }

                    System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X, point3D.Y);
                }


                List<AKRSPoint3D> realPoints = new List<AKRSPoint3D>();
                List<AKRSPoint3D> imgPoints = new List<AKRSPoint3D>();


                // 跑点位
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j <= 5; j++)
                    {
                        double x = (3 - j) * 0.6 - 0.3;
                        double y = (2 - i) * 0.5;
                        System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X + x, point3D.Y + y);

                        Thread.Sleep(100);

                        prEntity.DoWork();

                        MatchResult matchResult2 = (MatchResult)prEntity.AlgResult;

                        realPoints.Add(new AKRSPoint3D((float)x, (float)y, 0));
                        imgPoints.Add(new AKRSPoint3D((float)matchResult2.CenterX, (float)matchResult2.CenterY, 0));
                    }
                }


                // 坐标系转换
                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("UpLookCameraCoordinateSystem", "BondCoordinateSystem", false, CoordinateSystemTypeEnum.Vm);

                DependentCoordinateSystem dependentCoordinateSystem = (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "UpLookCameraCoordinateSystem");
                dependentCoordinateSystem.Init(realPoints, imgPoints, TransformTool.CalibModuleEnum.CameraStaticDown);
                MachineCoordinateSystem.GetInstance().Save();
                MachineCoordinateSystem.Refresh();
            });
        }

        /// <summary>
        /// 不同位置的取放误差
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void simpleButton10_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton17_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton23_Click(object sender, EventArgs e)
        {
            FrmTemperatureDistribution frm = new FrmTemperatureDistribution();
            frm.ShowDialog();
        }

        private void simpleButton24_Click(object sender, EventArgs e)
        {
            AKRS.ZX2200.Experiment.TestFunction.FrmTest.ShowUi();
        }
    }
}