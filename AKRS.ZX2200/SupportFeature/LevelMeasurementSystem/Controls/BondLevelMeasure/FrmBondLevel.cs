namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.BondLevelMeasure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.LevelMeasurementSystem.Models;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.BondLevelMeasure;

    using DevExpress.XtraEditors;

    /// <summary>
    /// bond水平测试
    /// </summary>
    public partial class FrmBondLevel : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Bond头
        /// </summary> 
        private BondHead BondHead => System2Module.GetInstance().BondModule.BondHead;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController BondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondModule BondModule = new BondModule();

        /// <summary>
        /// Bond模组控制器
        /// </summary>
        private BondModuleController BondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 结果存储
        /// </summary>
        private List<BondLevelMeasureResult> bondLevelMeasureResultList = new List<BondLevelMeasureResult>();

        /// <summary>
        /// 初始化
        /// </summary>
        public FrmBondLevel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 示教
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnTeach_Click(object sender, EventArgs e)
        {
            FrmBondLevelMeasureTeach frmBondLevelMeasureTeach = new FrmBondLevelMeasureTeach();
            frmBondLevelMeasureTeach.ShowDialog();
            frmBondLevelMeasureTeach.Dispose(); 
        }

        /// <summary>
        /// 测量
        /// </summary>
        /// <returns>测量结果</returns>
        public BondLevelMeasureResult Measure()
        {
            // 创建测量结果对象
            BondLevelMeasureResult bondLevelMeasureResult = new BondLevelMeasureResult();

            try
            {
                // 创建一个列表依次存储中心点,旋转0,90,180,270时点的高度，用于求差值
                List<double> heightList = new List<double>();

                AKRSPoint3D point3D;

                // 记录当前高度位置
                double liftPosition = this.BondHead.AxisZ.GetRealPosition();
          

                for (int i = 1; i < 5; i++)
                {
                    this.BondHeadController.MoveBondZToSafePos();
                    AKRSPoint3D safePoint = this.BondModuleController.GetG0RealPosition();

                    if (i == 1)
                    {
                        this.BondHeadController.RotateAxisT(-180);

                        point3D = new AKRSPoint3D(
                            BondLevelMeasureSetting.GetInstance().BmcPinCenter2DG0Pos.X,
                            BondLevelMeasureSetting.GetInstance().BmcPinCenter2DG0Pos.Y,
                            safePoint.Z);

                        // 移动到测高位置
                        this.BondModuleController.MoveToG0Pos(point3D);
                    }
                    else if (i == 2)
                    {
                        this.BondHeadController.RotateAxisT(-90);

                        AKRSPoint3D currentPoint = this.BondModuleController.GetG0RealPosition();

                        point3D = new AKRSPoint3D(
                            currentPoint.X - 17,
                            currentPoint.Y - 17,
                            safePoint.Z);

                        this.BondModuleController.MoveToG0Pos(point3D);
                    }
                    else if (i == 3)
                    {
                        this.BondHeadController.RotateAxisT(0);

                        AKRSPoint3D currentPoint = this.BondModuleController.GetG0RealPosition();

                        point3D = new AKRSPoint3D(
                            currentPoint.X + 17,
                            currentPoint.Y - 17,
                            safePoint.Z);


                        this.BondModuleController.MoveToG0Pos(point3D);
                    }
                    else if (i == 4)
                    {
                        this.BondHeadController.RotateAxisT(90);

                        AKRSPoint3D currentPoint = this.BondModuleController.GetG0RealPosition();

                        point3D = new AKRSPoint3D(
                            currentPoint.X + 17,
                            currentPoint.Y + 17,
                            safePoint.Z);

                        this.BondModuleController.MoveToG0Pos(point3D);
                    }

                    // 测高
                    (ExcuteResult Ret, double HeightValue) resPoint = this.BondHeadController.MeasureHeight(
                        liftPosition,
                        HeightMeasurementFunctionEnum.WithTDSensor,
                        this.BondModuleController.ConvertG0ToMachinePos(new AKRSPoint3D(0,0,BondLevelMeasureSetting.GetInstance().ZG0Pos)).Z,
                         10, 
                        0.5);


                    if (resPoint.Ret != ExcuteResult.Success)
                    {
                        AKRSXtraMessageBox.Show("测高失败");
                        return bondLevelMeasureResult;
                    }

                    // 转到G0上的高度
                    double resPointHeight = System2Module.GetInstance().BondModule
                        .ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, resPoint.HeightValue)).Z;
                    heightList.Add(resPointHeight);
                }


                // 把数据传给结果对象
                bondLevelMeasureResult.Height0 = heightList[0];
                bondLevelMeasureResult.Height90 = heightList[1];
                bondLevelMeasureResult.Height180 = heightList[2];
                bondLevelMeasureResult.Height270 = heightList[3];

                bondLevelMeasureResult.MaxDifference = Math.Round(heightList.Max() - heightList.Min(), 4);
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show("错误：\n" + e.Message);
            }
            finally
            {
                this.BondModuleController.MoveToSafePos();
            }

            // 测量完成
            return bondLevelMeasureResult;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
           // int d = HardwareRepositoryService.GetHardware<Sensor>("LVDT").ReadTxPDO();

            // 是否需要重新选择高度
            if (AKRSXtraMessageBox.Show("需要重新示教预测高度吗? ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                this.BondHeadController.MoveBondZToSafePos();
                FrmBondLevelHeightTeach frmBondLevelHeightTeach = new FrmBondLevelHeightTeach();

                if (frmBondLevelHeightTeach.ShowDialog() == DialogResult.Cancel)
                {
                    frmBondLevelHeightTeach.Dispose();
                    return;
                }
            }

            DialogResult dr = AKRSMessageBoxExt.Show(
                @"请确保焊头上安装了相应的治具
                              点击
                              确定 : 开始
                              取消 : 取消",
                $"提示",
                new[] { "确定", "取消" },
                new[] { DialogResult.OK, DialogResult.Cancel });

            switch (dr)
            {
                case DialogResult.Cancel:
                    return;

                case DialogResult.OK:
                    if (BondLevelMeasureSetting.GetInstance().BmcPinCenter2DG0Pos == null)
                    {
                        AKRSXtraMessageBox.Show("请先示教", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        Remeasure:

                        // 移动到安全位置
                        this.BondHeadController.MoveBondZToSafePos();

                        this.bondLevelMeasureResultList.Clear();

                        // 循环执行次数
                        int times = Convert.ToInt32(this.SpTimes.EditValue);

                        for (int i = 0; i < times; i++)
                        {
                            BondLevelMeasureResult bondLevelMeasureResult  = this.Measure();

                            this.bondLevelMeasureResultList.Add(bondLevelMeasureResult);
                        }

                        // 测量结束
                        this.BondModuleController.MoveToSafePos();

                        FrmBondLevelMeasureResult frmBondLevelMeasureResult = new FrmBondLevelMeasureResult(this.bondLevelMeasureResultList);
                        DialogResult drs = frmBondLevelMeasureResult.ShowDialog();
                        if (drs == DialogResult.Retry)
                        {
                            goto Remeasure;
                        }

                        frmBondLevelMeasureResult.Dispose();
                    }

                    break;
            }
        }
    }
}