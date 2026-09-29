namespace AKRS.ZX2200.Experiment.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    using DevExpress.XtraEditors;

    using GlobalCalibration.GlobalCalibration;

    using log4net.Core;

    using OfficeOpenXml;

    using LicenseContext = OfficeOpenXml.LicenseContext;

    /// <summary>
    /// 测试移动精度
    /// </summary>
    public partial class FrmTestMoveAccuracy : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// Bond当前轴位置
        /// </summary>
        private AKRSPoint3D bondCurPos;

        /// <summary>
        /// 移动距离
        /// </summary>
        private double step;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmTestMoveAccuracy()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 向上移动
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnUp_Click(object sender, EventArgs e)
        {
            this.bondCurPos = this.bondModuleController.Get3DRealPosition();
            this.step = Convert.ToDouble(this.txtStep.Text);

            AKRSPoint3D targetPos = new AKRSPoint3D()
                                        {
                                            X = this.bondCurPos.X,
                                            Y = this.bondCurPos.Y + this.step,
                                            Z = this.bondCurPos.Z
                                        };
            this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
        }

        /// <summary>
        /// 向右移动
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnRight_Click(object sender, EventArgs e)
        {
            this.bondCurPos = this.bondModuleController.Get3DRealPosition();
            this.step = Convert.ToDouble(this.txtStep.Text);

            AKRSPoint3D targetPos = new AKRSPoint3D()
                                        {
                                            X = this.bondCurPos.X + this.step,
                                            Y = this.bondCurPos.Y,
                                            Z = this.bondCurPos.Z
                                        };
            this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
        }

        /// <summary>
        /// 向下移动
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnDown_Click(object sender, EventArgs e)
        {
            this.bondCurPos = this.bondModuleController.Get3DRealPosition();
            this.step = Convert.ToDouble(this.txtStep.Text);

            AKRSPoint3D targetPos = new AKRSPoint3D()
                                        {
                                            X = this.bondCurPos.X,
                                            Y = this.bondCurPos.Y - this.step,
                                            Z = this.bondCurPos.Z
                                        };
            this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
        }

        /// <summary>
        /// 向左移动
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnLeft_Click(object sender, EventArgs e)
        {
            this.bondCurPos = this.bondModuleController.Get3DRealPosition();
            this.step = Convert.ToDouble(this.txtStep.Text);

            AKRSPoint3D targetPos = new AKRSPoint3D()
                                        {
                                            X = this.bondCurPos.X - this.step,
                                            Y = this.bondCurPos.Y,
                                            Z = this.bondCurPos.Z
                                        };
            this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnEditPr_Click(object sender, EventArgs e)
        {
            this.EditPr("MoveTestPR");
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="algBeLong">模板类型</param>
        public void EditPr(string name, AlgBeLongEnum algBeLong = AlgBeLongEnum.Calibration)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);
                prEntity.Alg.AlgBeLong = algBeLong;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.ShowDialog();
            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 移动到相机中心
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnMoveToCenter_Click(object sender, EventArgs e)
        {
            try
            { 
                MatchResult result = this.LocatePosition("MoveTestPR");
                // MatchResult result = this.LocatePosition(CalibrateRunPara.GetInstance().BmcPRName);

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(
                    ex.Message,
                    "异常",
                    new string[] { "异常" },
                    new DialogResult[] { DialogResult.Yes });
            }
        }

        /// <summary>
        /// 模板定位
        /// </summary>
        /// <param name="patternName">模板名称</param>
        /// <returns>定位结果</returns>
        public MatchResult LocatePosition(string patternName)
        {
            // 获取Pr实体
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(patternName);

            Thread.Sleep(800);
            ExcuteResult excuteResult = pREntity.DoWork();

            // 拍照失败，直接返回错误
            if (excuteResult != ExcuteResult.Success)
            {
                throw new ArgumentNullException(patternName, "The" + patternName + " excuteResult is fail.");
            }

            // 获取定位结果
            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            return matchResult;
        }

        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnLocate_Click(object sender, EventArgs e)
        {
            try
            { 
                MatchResult result = this.LocatePosition("MoveTestPR");
                // MatchResult result = this.LocatePosition(CalibrateRunPara.GetInstance().BmcPRName);

                this.txtResultX.Text = (result.CenterX - 1224).ToString();
                this.txtResultY.Text = (result.CenterY - 1024).ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(
                    ex.Message,
                    "异常",
                    new string[] { "异常" },
                    new DialogResult[] { DialogResult.Yes });
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                    {
                        this.bondCurPos = this.bondModuleController.Get3DRealPosition();
                        this.step = Convert.ToDouble(this.txtStep.Text);

                        List<double> resultX = new List<double>();
                        List<double> resultY = new List<double>();

                        MatchResult result = this.LocatePosition("MoveTestPR");
                        this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);

                        AKRSPoint3D targetPos1 = new AKRSPoint3D()
                                                     {
                                                         X = this.bondCurPos.X,
                                                         Y = this.bondCurPos.Y,
                                                         Z = this.bondCurPos.Z
                                                     };

                        for (int i = 0; i < 51; i++)
                        {
                            this.bondCurPos = this.bondModuleController.Get3DRealPosition();

                            AKRSPoint3D targetPos = new AKRSPoint3D()
                                                        {
                                                            X = this.bondCurPos.X + 54,
                                                            Y = this.bondCurPos.Y,
                                                            Z = this.bondCurPos.Z
                                                        };

                            double X = AxisCompensateProgram.GetInstance().GetOffset("BondX", targetPos.X);
                            double Y = AxisCompensateProgram.GetInstance().GetOffset("BondY", targetPos.Y);

                            if (Math.Abs(X - targetPos.X) > 1 || Math.Abs(Y - targetPos.Y) > 1)
                            {
                                AKRSXtraMessageBox.Show("错误");
                            }



                            //targetPos.X = X;
                            //targetPos.Y = Y;

                            this.bondModuleController.MoveBondXY(X, Y);

                            result = this.LocatePosition("MoveTestPR");
                            resultX.Add(result.CenterX - 1224);
                            resultY.Add(result.CenterY - 1024);

                            // 反向移动
                            //  this.bondCurPos = this.bondModuleController.Get3DRealPosition();
                            //targetPos = new AKRSPoint3D()
                            //{
                            //    X = this.bondCurPos.X - 51 - i * 3,
                            //    Y = this.bondCurPos.Y,
                            //    Z = this.bondCurPos.Z
                            //};

                            targetPos.X = targetPos.X - 51;

                            double X1 = AxisCompensateProgram.GetInstance().GetOffset("BondX", targetPos.X);
                            double Y1 = AxisCompensateProgram.GetInstance().GetOffset("BondY", targetPos.Y);

                            if (Math.Abs(X1 - targetPos.X) > 1 || Math.Abs(Y1 - targetPos.Y) > 1)
                            {
                                AKRSXtraMessageBox.Show("错误");
                            }

                            targetPos.X = X1;
                            targetPos.Y = Y1;

                            this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);

                            //// 移动到相机中心
                            //result = this.LocatePosition("MoveTestPR");
                            //this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);

                        }

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelPackage package = new ExcelPackage(new FileInfo("D:\\MoveTestPR.xlsx"));

                        // 添加一个工作表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                        worksheet.Cells[1, 1].Value = "次数";
                        worksheet.Cells[1, 2].Value = "a1x";
                        worksheet.Cells[1, 3].Value = "a1y";

                        // 写入数据
                        for (int row = 0; row < resultY.Count; row++)
                        {
                            worksheet.Cells[row + 2, 1].Value = (row + 1);
                            worksheet.Cells[row + 2, 2].Value = resultX[row];
                            worksheet.Cells[row + 2, 3].Value = resultY[row];
                        }

                        // 保存Excel文件
                        package.Save();
                    });
        }

        private void BtnTestMoveCenter_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                    {
                        this.bondCurPos = this.bondModuleController.Get3DRealPosition();
                        this.step = Convert.ToDouble(this.txtStep.Text);

                        List<double> resultX = new List<double>();
                        List<double> resultY = new List<double>();

                        for (int i = 0; i < 80; i++)
                        {
                            MatchResult result = this.LocatePosition("MoveTestPR");
                            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);

                            this.bondCurPos = this.bondModuleController.Get3DRealPosition();

                            AKRSPoint3D targetPos1 = new AKRSPoint3D()
                                                         {
                                                             X = this.bondCurPos.X + 1,
                                                             Y = this.bondCurPos.Y - 1,
                                                             Z = this.bondCurPos.Z
                                                         };

                            this.bondModuleController.MoveBondXY(targetPos1.X, targetPos1.Y);
                            
                            result = this.LocatePosition("MoveTestPR");
                            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);

                            result = this.LocatePosition("MoveTestPR");
                            resultX.Add(result.CenterX - 1224);
                            resultY.Add(result.CenterY - 1024);

                            this.bondCurPos = this.bondModuleController.Get3DRealPosition();
                            AKRSPoint3D targetPos2 = new AKRSPoint3D()
                                                         {
                                                             X = this.bondCurPos.X + 3,
                                                             Y = this.bondCurPos.Y,
                                                             Z = this.bondCurPos.Z
                                                         };

                            this.bondModuleController.MoveBondXY(targetPos2.X, targetPos2.Y);
                        }

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelPackage package = new ExcelPackage(new FileInfo("D:\\MoveCenterTestPR.xlsx"));

                        // 添加一个工作表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                        worksheet.Cells[1, 1].Value = "次数";
                        worksheet.Cells[1, 2].Value = "a1x";
                        worksheet.Cells[1, 3].Value = "a1y";

                        // 写入数据
                        for (int row = 0; row < resultY.Count; row++)
                        {
                            worksheet.Cells[row + 2, 1].Value = (row + 1);
                            worksheet.Cells[row + 2, 2].Value = resultX[row];
                            worksheet.Cells[row + 2, 3].Value = resultY[row];
                        }

                        // 保存Excel文件
                        package.Save();
                    });
        }
    }
}
