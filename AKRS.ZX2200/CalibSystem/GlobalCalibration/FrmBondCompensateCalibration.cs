using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Service;
    using System.Threading;

    using MathNet.Numerics;
    using DevExpress.CodeParser;
    using AKRS.ZX2200.Infrastructure.Utils;
    using System.IO;
    using DevExpress.CodeParser.VB;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 贴片补偿标定
    /// </summary>
    public partial class FrmBondCompensateCalibration : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 补偿
        /// </summary>
        private BondCompensateCalibration BondCompensateCalibration => BondCompensateCalibration.GetInstance();

        /// <summary>
        /// 工作线程
        /// </summary>
        private Task task;

        /// <summary>
        /// 是否停止
        /// </summary>
        private bool stop = true;

            /// <summary>
        /// 贴片补偿标定
        /// </summary>
        public FrmBondCompensateCalibration()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmBondCompensateCalibration_Load(object sender, EventArgs e)
        {
            this.SpStartX.EditValue = this.BondCompensateCalibration.StartPoint.X;
            this.SpStartY.EditValue = this.BondCompensateCalibration.StartPoint.Y;
            this.SpStartZ.EditValue = this.BondCompensateCalibration.StartPoint.Z;
            this.SpColumnCount.EditValue = this.BondCompensateCalibration.ColumnCount;
            this.SpRowCount.EditValue = this.BondCompensateCalibration.RowCount;
            this.SpColumnSpacing.EditValue = this.BondCompensateCalibration.DistanceX;
            this.SpRowSpacing.EditValue = this.BondCompensateCalibration.DistanceY;

            this.TxPrName.Text = this.BondCompensateCalibration.PrName;

            this.SpPrDelay.EditValue = this.BondCompensateCalibration.PrDelay;

            this.CmbChooseType.Properties.Items.Add("精度测试");
            this.CmbChooseType.Properties.Items.Add("贴片补偿标定");
        }

        /// <summary>
        /// 移动到中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtGetStartPos_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = AKRSXtraMessageBox.Show("请将相机移动到玻璃片中心", "提示", MessageBoxButtons.YesNo);

            if (dialogResult != DialogResult.Yes)
            {
                return;
            }

            // 获取当前位置
            AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();

            // 减去相机和焊头之间的差值
            point3D += System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset;

            bool isContinue = true;

            // 取片
            this.Pick(point3D);

            while (isContinue)
            {
                // 放片
                this.Bond(point3D);

                // 定位
                AKRSPoint3D pointOffset = this.VisionResult(point3D);

                if (Math.Abs(pointOffset.X) < 0.002 && Math.Abs(pointOffset.Y) < 0.002)
                {
                    isContinue = false;
                }

                // 取片
                this.Pick(point3D + pointOffset);
            }

            this.BondCompensateCalibration.StartPoint = point3D;

            this.SpStartX.EditValue = this.BondCompensateCalibration.StartPoint.X;
            this.SpStartY.EditValue = this.BondCompensateCalibration.StartPoint.Y;
            this.SpStartZ.EditValue = this.BondCompensateCalibration.StartPoint.Z;
        }

        /// <summary>
        /// 编辑模板
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

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            this.BondCompensateCalibration.StartPoint.X = (double)this.SpStartX.Value;
            this.BondCompensateCalibration.StartPoint.Y = (double)this.SpStartY.Value;
            this.BondCompensateCalibration.StartPoint.Z = (double)this.SpStartZ.Value;
            this.BondCompensateCalibration.ColumnCount = (int)this.SpColumnCount.Value;
            this.BondCompensateCalibration.RowCount = (int)this.SpRowCount.Value;
            this.BondCompensateCalibration.DistanceX = (int)this.SpColumnSpacing.Value;
            this.BondCompensateCalibration.DistanceY = (int)this.SpRowSpacing.Value;

            this.BondCompensateCalibration.PrName = this.TxPrName.Text;

            this.BondCompensateCalibration.PrDelay = (int)this.SpPrDelay.Value;

            this.BondCompensateCalibration.Save();
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStart_Click(object sender, EventArgs e)
        {
            if (this.task != null && this.task.Status == TaskStatus.Running)
            {
                AKRSXtraMessageBox.Show("线程仍在执行，请稍后");
                return;
            }

            if (this.CmbChooseType.SelectedIndex == 1)
            {
                this.task = new Task(this.StartBondCompensateCalibration);
                this.task.Start();
            }
            else
            {
                this.task = new Task(this.ValidateBondCompensateCalibration);
                this.task.Start();
            }
        }

        /// <summary>
        /// 开始二维补偿
        /// </summary>
        private void StartBondCompensateCalibration()
        {
            #region 初始化

            AKRSPoint3D startPoint3D1 = this.BondCompensateCalibration.StartPoint;

            bool isContinue = true;

            while (isContinue)
            {
                // 放片
                this.Bond(startPoint3D1);

                // 定位
                AKRSPoint3D pointOffset = this.VisionResult(startPoint3D1);

                if (Math.Abs(pointOffset.X) < 0.002 && Math.Abs(pointOffset.Y) < 0.002)
                {
                    isContinue = false;
                }

                // 取片
                this.Pick(startPoint3D1 += pointOffset);
            }

            this.BondCompensateCalibration.StartPoint = startPoint3D1;

            //this.SpStartX.EditValue = this.BondCompensateCalibration.StartPoint.X;
            //this.SpStartY.EditValue = this.BondCompensateCalibration.StartPoint.Y;
            //this.SpStartZ.EditValue = this.BondCompensateCalibration.StartPoint.Z;

            this.stop = false;

            // 首先保证取到中心

            this.BondCompensateCalibration.PlanePoint2Ds = new AKRSPoint2D[this.BondCompensateCalibration.RowCount,
                this.BondCompensateCalibration.ColumnCount];

            for (int i = 0; i < this.BondCompensateCalibration.RowCount; i++)
            {
                for (int j = 0; j < this.BondCompensateCalibration.ColumnCount; j++)
                {
                    this.BondCompensateCalibration.PlanePoint2Ds[i, j] = new AKRSPoint2D();
                }
            }

            List<List<double>> resultList = new List<List<double>>();

            #endregion

            AKRSPoint3D startPoint3D = this.BondCompensateCalibration.StartPoint;

            while (true)
            {
                resultList.Clear();

                for (int i = 0; i < this.BondCompensateCalibration.RowCount; i++)
                {
                    for (int j = 0; j < this.BondCompensateCalibration.ColumnCount; j++)
                    {
                        if (this.stop)
                        {
                            return;
                        }

                        AKRSPoint3D point3D = startPoint3D + new AKRSPoint3D(
                                                  j * this.BondCompensateCalibration.DistanceX,
                                                  i * this.BondCompensateCalibration.DistanceY,
                                                  0);

                        this.Bond(point3D + new AKRSPoint3D(this.BondCompensateCalibration.PlanePoint2Ds[i, j].X, this.BondCompensateCalibration.PlanePoint2Ds[i, j].Y, 0));
                        AKRSPoint3D pointOffset = this.VisionResult(point3D);
                        this.Pick(point3D + new AKRSPoint3D(this.BondCompensateCalibration.PlanePoint2Ds[i, j].X, this.BondCompensateCalibration.PlanePoint2Ds[i, j].Y, 0));
                        this.BondCompensateCalibration.PlanePoint2Ds[i, j].X -= pointOffset.X;
                        this.BondCompensateCalibration.PlanePoint2Ds[i, j].Y -= pointOffset.Y;
                        resultList.Add(
                            new List<double>()
                                {
                                    i,
                                    j,
                                    point3D.X,
                                    point3D.Y,
                                    this.BondCompensateCalibration.PlanePoint2Ds[i, j].X,
                                    this.BondCompensateCalibration.PlanePoint2Ds[i, j].Y
                                });
                    }
                }

                // 数据处理
                string path = FileHelper.CreateFileByDate("贴片标定");

                FileHelper.SaveDoubleExcel(resultList, Path.Combine(path, DateTime.Now.ToFileTime().ToString() + ".xlsx"));

                resultList.Clear();
            }
        }

        /// <summary>
        /// 开始二维补偿
        /// </summary>
        private void ValidateBondCompensateCalibration()
        {
            this.stop = false;

            List<List<double>> resultList = new List<List<double>>();

            AKRSPoint3D startPoint3D = this.BondCompensateCalibration.StartPoint;

            while (true)
            {
                resultList.Clear();

                for (int i = 0; i < this.BondCompensateCalibration.RowCount; i++)
                {
                    for (int j = 0; j < this.BondCompensateCalibration.ColumnCount; j++)
                    {
                        if (this.stop)
                        {
                            return;
                        }

                        AKRSPoint3D point3D = startPoint3D + new AKRSPoint3D(
                                                  j * this.BondCompensateCalibration.DistanceX,
                                                  i * this.BondCompensateCalibration.DistanceY,
                                                  0);

                        point3D.X += this.BondCompensateCalibration.PlanePoint2Ds[i, j].X;

                        point3D.Y += this.BondCompensateCalibration.PlanePoint2Ds[i, j].Y;
                        this.Bond(point3D);
                        AKRSPoint3D pointOffset = this.VisionResult(point3D);
                        this.Pick(point3D);

                        resultList.Add(
                            new List<double>()
                                {
                                    i,
                                    j,
                                    point3D.X,
                                    point3D.Y,
                                    this.BondCompensateCalibration.PlanePoint2Ds[i, j].X,
                                    this.BondCompensateCalibration.PlanePoint2Ds[i, j].Y
                                });
                    }
                }

                // 数据处理
                string path = FileHelper.CreateFileByDate("贴片标定");

                FileHelper.SaveDoubleExcel(resultList, Path.Combine(path, DateTime.Now.ToFileTime().ToString() + ".xlsx"));

                resultList.Clear();
            }
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="point3D"></param>
        private void MoveToPos(AKRSPoint3D point3D)
        {
            System2Domain.GetInstance().BondModuleController.MoveBondXY(point3D.X, point3D.Y);

            System2Domain.GetInstance().BondHeadController.MoveAxisZ(point3D.Z);
        }

        /// <summary>
        /// 取片
        /// </summary>
        /// <param name="point3D">点位</param>
        private void Pick(AKRSPoint3D point3D)
        {

            AKRSPoint3D point3D1 = new AKRSPoint3D(point3D.X, point3D.Y, point3D.Z + 3);

            this.MoveToPos(point3D1);

            // 力控
            System2Domain.GetInstance().BondHeadController.ForceControlSet(50, point3D1.Z, 100);

            // 打开吸嘴真空
            System2Domain.GetInstance().BondHeadController.OpenToolVaccum();

            // 停留
            Thread.Sleep(100);

            // t退出力控并上抬
            System2Domain.GetInstance().BondHeadController.ForceControlReset(point3D1.Z, 10);
        }

        /// <summary>
        /// 贴片
        /// </summary>
        /// <param name="point3D">位置</param>
        private void Bond(AKRSPoint3D point3D)
        {

            AKRSPoint3D point3D1 = new AKRSPoint3D(point3D.X, point3D.Y, point3D.Z + 3);

            // 移动到位
            this.MoveToPos(point3D1);

            // 力控
            System2Domain.GetInstance().BondHeadController.ForceControlSet(50, point3D1.Z, 100);

            // 关闭吸嘴真空
            System2Domain.GetInstance().BondHeadController.CloseToolVaccum();

            // 打开弱吹
            System2Domain.GetInstance().BondHeadController.OpenToolBlowEle(500);

            // 停留
            Thread.Sleep(100);

            // 打开弱吹
            System2Domain.GetInstance().BondHeadController.CloseToolBlowEle();

            // t退出力控并上抬
            System2Domain.GetInstance().BondHeadController.ForceControlReset(point3D1.Z, 10);
        }

        /// <summary>
        /// 定位
        /// </summary>
        private AKRSPoint3D VisionResult(AKRSPoint3D point3D)
        {
            // 移动到位
            this.MoveToPos(
                point3D - System2Domain.GetInstance().BondDevicePara.BondHeadParam.HeadToCameraOffset);

            Thread.Sleep(this.BondCompensateCalibration.PrDelay);

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

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStop_Click(object sender, EventArgs e)
        {
            this.stop = true;
        }
    }
}