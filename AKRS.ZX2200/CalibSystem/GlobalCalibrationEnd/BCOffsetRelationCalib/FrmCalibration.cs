namespace AKRS.ZX2200.CalibSystem.GlobalCalibrationEnd.BCOffsetRelationCalib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
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

    using log4net.Core;

    using OfficeOpenXml;

    /// <summary>
    /// 测试移动精度
    /// </summary>
    public partial class FrmCalibration : XtraForm
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
        /// 移动距离
        /// </summary>
        private double step;

        /// <summary>
        /// Bond 坐标系
        /// </summary>
        private DependentCoordinateSystem bondCoordinateSystem => (DependentCoordinateSystem)MachineCoordinateSystem
            .GetInstance().CoordinateSystems.Find(it => it.Name == "BondCameraCoordinateSystem");

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCalibration()
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
            AKRSPoint3D bondCurPos = this.bondModuleController.Get3DRealPosition();
            this.step = Convert.ToDouble(this.SpStep.Text);

            AKRSPoint3D targetPos = new AKRSPoint3D()
                                        {
                                            X = bondCurPos.X,
                                            Y = bondCurPos.Y + this.step,
                                            Z = bondCurPos.Z
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
            AKRSPoint3D bondCurPos = this.bondModuleController.Get3DRealPosition();
            this.step = Convert.ToDouble(this.SpStep.Text);

            AKRSPoint3D targetPos = new AKRSPoint3D()
                                        {
                                            X = bondCurPos.X + this.step,
                                            Y = bondCurPos.Y,
                                            Z = bondCurPos.Z
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
            AKRSPoint3D bondCurPos = this.bondModuleController.Get3DRealPosition();
            this.step = Convert.ToDouble(this.SpStep.Text);

            AKRSPoint3D targetPos = new AKRSPoint3D()
                                        {
                                            X = bondCurPos.X,
                                            Y = bondCurPos.Y - this.step,
                                            Z = bondCurPos.Z
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
            AKRSPoint3D bondCurPos  = this.bondModuleController.Get3DRealPosition();
            this.step = Convert.ToDouble(this.SpStep.Text);

            AKRSPoint3D targetPos = new AKRSPoint3D()
                                        {
                                            X = bondCurPos.X - this.step,
                                            Y = bondCurPos.Y,
                                            Z = bondCurPos.Z
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
            this.EditPr("GlobalCalibrationRing");
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
                MatchResult result = this.LocatePosition("GlobalCalibrationRing");
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
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
                MatchResult result = this.LocatePosition("GlobalCalibrationRing");
                this.txtResultX.Text = (result.CenterX - 1224).ToString();
                this.txtResultY.Text = (result.CenterY - 1024).ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
            }
        }

        private double markDistance = 1;

        /// <summary>
        /// 开始标定按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStartCalibration_Click(object sender, EventArgs e)
        {
            int xNum = Convert.ToInt32(this.SpXNum.EditValue);
            int yNum = Convert.ToInt32(this.SpYNum.EditValue);
            this.markDistance = Convert.ToInt32(this.SpStep.EditValue);

            Task.Run(
                () =>
                    {
                        SortedList<double, SortedList<double, BCOffsetRelationModel>> bcOffsetRelations = new SortedList<double, SortedList<double, BCOffsetRelationModel>>();

                        try
                        {
                            // 首先需要找到一个Mark 并移动到相机中心
                            MatchResult result = this.LocatePosition("GlobalCalibrationRing");
                            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);

                            // 记录开始点
                            AKRSPoint3D startPos = this.bondModuleController.Get3DRealPosition();

                            for (int i = 0; i < yNum; i++)
                            {
                                AKRSPoint3D targetPos = new AKRSPoint3D()
                                {
                                    X = startPos.X,
                                    Y = this.bondModuleController.GetAxisYRealPos() - this.markDistance,   // 向后移动一行
                                    Z = startPos.Z
                                };

                                // 向后移动一行
                                this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);

                                // 定位并移动到中心点
                                result = this.LocatePosition("GlobalCalibrationRing");
                                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);

                                // 获取当前轴坐标
                                AKRSPoint3D bondCurPos = this.bondModuleController.Get3DRealPosition();

                                // 几路y 坐标
                                double yPos = bondCurPos.Y; 
                                
                                SortedList<double, BCOffsetRelationModel> bcOffsetRalationRow = new SortedList<double, BCOffsetRelationModel>();

                                for (int j = 0; j < xNum; j++)
                                {
                                    try
                                    {
                                        bondCurPos = this.bondModuleController.Get3DRealPosition();

                                         targetPos = new AKRSPoint3D()
                                        {
                                            X = bondCurPos.X + 56, Y = bondCurPos.Y, Z = bondCurPos.Z
                                        };

                                        // 移动到焊头位置的Mark
                                        this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
                                        Thread.Sleep(100);
                                        result = this.LocatePosition("GlobalCalibrationRing");

                                        bondCurPos = this.bondModuleController.Get3DRealPosition();

                                        BCOffsetRelationModel bcOffsetRelationModel = new BCOffsetRelationModel();
                                        bcOffsetRelationModel.Pos = new AKRSPoint2D(bondCurPos.X, bondCurPos.Y);

                                        // 像素转换成机械坐标，我觉得这里不用这个，用乘像素比的方式也OK。 因为实际补偿的时候就是乘以像素比的
                                        AKRSPoint3D locationCenter =
                                            this.bondCoordinateSystem.ForwardConvertCoordinate(
                                                new AKRSPoint3D(result.CenterX, result.CenterY, 0));

                                        bcOffsetRelationModel.ΔX = locationCenter.X;
                                        bcOffsetRelationModel.ΔY = locationCenter.Y;

                                        bcOffsetRalationRow.Add(bondCurPos.X, bcOffsetRelationModel);

                                        // 反向移动
                                        bondCurPos = this.bondModuleController.Get3DRealPosition();
                                        targetPos = new AKRSPoint3D()
                                        {
                                            X = bondCurPos.X - 54, Y = bondCurPos.Y, Z = bondCurPos.Z
                                        };

                                        this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);

                                        // 移动到相机中心
                                        result = this.LocatePosition("GlobalCalibrationRing");
                                        this.calibController.MoveToCamCenter(
                                            CalibController.CamCoordinateType.Bond,
                                            result);
                                    }
                                    catch (Exception ex)
                                    {
                                        AKRSXtraMessageBox.Show($"Calibration Exception:{ex.Message}！", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                    }
                                }

                                bcOffsetRelations.Add(yPos, bcOffsetRalationRow);
                            }

                            BCOffsetRelationCalibration.GetInstance().BCOffsetRelations = bcOffsetRelations;

                            BCOffsetRelationCalibration.GetInstance().Save();

                            AKRSXtraMessageBox.Show($"Calibration Finished！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            AKRSXtraMessageBox.Show($"Calibration Exception:{ex.Message}！", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
        }

        /// <summary>
        /// 获取XPos 位置的 偏移量
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtGetOffset_Click(object sender, EventArgs e)
        {
            double xPos = Convert.ToDouble(this.SpXPos.EditValue);
            double yPos = Convert.ToDouble(this.SpYPos.EditValue);
            (double ΔX, double ΔY) ret = BCOffsetRelationCalibration.GetInstance().GetΔXY(xPos, yPos);
            this.TxOffsetX.Text = ret.ΔX.ToString();
            this.TxOffsetY.Text = ret.ΔY.ToString();
        }

        /// <summary>
        /// 启动视觉任务
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtStartVisionTask_Click(object sender, EventArgs e)
        {
            AKRSPoint3D bondCurPos;

            int xNum = Convert.ToInt32(this.SpXNum.EditValue);
            int yNum = Convert.ToInt32(this.SpYNum.EditValue);
            this.markDistance = Convert.ToInt32(this.SpStep.EditValue);
            Task.Run(
                () =>
                    {
                        bondCurPos = this.bondModuleController.Get3DRealPosition();
                        List<double> resultXPos = new List<double>();
                        List<double> resultYPos = new List<double>();
                        List<double> resultX = new List<double>();
                        List<double> resultY = new List<double>();

                        MatchResult result = this.LocatePosition("GlobalCalibrationRing");
                        this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);

                        // 记录开始点
                        AKRSPoint3D startPos = this.bondModuleController.Get3DRealPosition();

                        for (int i = 0; i < yNum; i++)
                        {
                            AKRSPoint3D targetPos = new AKRSPoint3D()
                            {
                                X = startPos.X,
                                Y = this.bondModuleController.GetAxisYRealPos() - this.markDistance,   // 向后移动一行
                                Z = startPos.Z
                            };

                            // 向后移动一行
                            this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);

                            // 定位并移动到中心点
                            result = this.LocatePosition("GlobalCalibrationRing");
                            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);

                            // 获取当前轴坐标
                             bondCurPos = this.bondModuleController.Get3DRealPosition();

                            // 几路y 坐标
                            double yPos = bondCurPos.Y;
                            Thread.Sleep(100);
                            for (int j = 0; j < xNum; i++)
                            {
                                try
                                {
                                    bondCurPos = this.bondModuleController.Get3DRealPosition();

                                    (double X, double Y) ret = BCOffsetRelationCalibration.GetInstance().GetΔXY(bondCurPos.X + 56, bondCurPos.Y);

                                    targetPos = new AKRSPoint3D()
                                    {
                                        X = bondCurPos.X + 56,
                                        Y = bondCurPos.Y,
                                        Z = bondCurPos.Z
                                    };

                                    this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
                                    Thread.Sleep(500);  

                                    targetPos = new AKRSPoint3D()
                                    {
                                        X = bondCurPos.X + 56 + ret.X,
                                        Y = bondCurPos.Y + ret.Y,
                                        Z = bondCurPos.Z
                                    };

                                    this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);
                                    Thread.Sleep(500);

                                    result = this.LocatePosition("GlobalCalibrationRing");

                                    resultXPos.Add(targetPos.X);
                                    resultYPos.Add(targetPos.Y);
                                    resultX.Add((result.CenterX - 1224) * 0.001765);
                                    resultY.Add((result.CenterY - 1024) * 0.001765);

                                    // 反向移动
                                    bondCurPos = this.bondModuleController.Get3DRealPosition();
                                    targetPos = new AKRSPoint3D()
                                    {
                                        X = bondCurPos.X - 54,
                                        Y = bondCurPos.Y,
                                        Z = bondCurPos.Z
                                    };

                                    this.bondModuleController.MoveBondXY(targetPos.X, targetPos.Y);

                                    // 移动到相机中心
                                    result = this.LocatePosition("GlobalCalibrationRing");
                                    this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, result);
                                    Thread.Sleep(100);
                                }
                                catch (Exception ex)
                                {
                                    AKRSXtraMessageBox.Show("定位失败");
                                    continue;
                                }
                            }
                        }

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        ExcelPackage package = new ExcelPackage(new FileInfo("D:\\MoveTestPR.xlsx"));

                        // 添加一个工作表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                        worksheet.Cells[1, 1].Value = "Xpos";
                        worksheet.Cells[1, 2].Value = "Ypos";
                        worksheet.Cells[1, 3].Value = "a1x";
                        worksheet.Cells[1, 4].Value = "a1y";

                        // 写入数据
                        for (int row = 0; row < resultY.Count; row++)
                        {
                            worksheet.Cells[row + 2, 1].Value = resultXPos[row];
                            worksheet.Cells[row + 2, 1].Value = resultYPos[row];
                            worksheet.Cells[row + 2, 3].Value = resultX[row];
                            worksheet.Cells[row + 2, 4].Value = resultY[row];
                        }

                        // 保存Excel文件
                        package.Save();
                    });
        }
    }
}