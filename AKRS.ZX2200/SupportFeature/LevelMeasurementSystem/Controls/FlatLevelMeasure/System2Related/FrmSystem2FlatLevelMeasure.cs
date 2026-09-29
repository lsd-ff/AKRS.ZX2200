namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure.System2Related
{
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.LevelMeasurementSystem.Models.FlatLevelMeasure;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure.System2Related;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// 系统2水平检测
    /// </summary>
    public partial class FrmSystem2FlatLevelMeasure : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Bond模组控制器
        /// </summary>
        private BondModuleController BondModuleController => System2Domain.GetInstance().BondModuleController;
        
        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController BondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 初始化
        /// </summary>
        public FrmSystem2FlatLevelMeasure()
        {
            this.InitializeComponent();
            this.RefreshCmbItems();
            this.InitControl();
            this.ChkLaserMeasure.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh;
            this.ChkBackCy.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh;
        }

        /// <summary>
        /// Bond头
        /// </summary> 
        private BondHead BondHead => System2Module.GetInstance().BondModule.BondHead;

        /// <summary>
        /// 结果存储
        /// </summary>
        private List<FlatLevelMeasureResult> flatLevelMeasureResultList = new List<FlatLevelMeasureResult>();

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            this.ImgCmbSystem2FlatLevelMeasure.Properties.Items.Clear();
            ImageComboBoxItem[] list = System2FlatLevelMeasureRepository.GetInstance()
                .Filter(string.Empty, false)
                .Select(a => new ImageComboBoxItem(a.Name, a.Name, (int)a.EditState)).ToArray();
            this.ImgCmbSystem2FlatLevelMeasure.Properties.Items.AddRange(list);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ImgCmbSystem2FlatLevelMeasure_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<System2FlatLevelMeasureSetting> frmRepository = new FrmRepository<System2FlatLevelMeasureSetting>(System2FlatLevelMeasureRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    System2FlatLevelMeasureSetting system2FlatLevelMeasureSetting = (System2FlatLevelMeasureSetting)frmRepository.DsSetting;

                    if (system2FlatLevelMeasureSetting != null)
                    {
                        ((ComboBoxEdit)sender).Text = system2FlatLevelMeasureSetting.Name;
                    }
                }

                this.RefreshCmbItems();
                frmRepository.Dispose();
            }
        }

        /// <summary>
        /// 示教
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnTeach_Click(object sender, EventArgs e)
        {
            string name = this.ImgCmbSystem2FlatLevelMeasure.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                AKRSXtraMessageBox.Show("请选择一个测高点", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (this.ChkLaserMeasure.Checked)
            {
                System2Domain.GetInstance().S2DispenseController.OpenDispenseHeightMeasurementCylinder();
            }

            FrmSystem2FlatLevelMeasureTeach frmFlatLevelMeasureTeach = new FrmSystem2FlatLevelMeasureTeach(name);
            frmFlatLevelMeasureTeach.ShowDialog();
            frmFlatLevelMeasureTeach.Dispose();

            if (this.ChkLaserMeasure.Checked)
            {
                System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
            }
        }

        /// <summary>
        /// 开始测量
        /// </summary>
        /// <returns>测量结果</returns>
        public List<double> Measure()
        {
            // 创建一个列表临时存储结果
            List<double> result = new List<double>();

            try
            {
                string levelMeasureName = this.ImgCmbSystem2FlatLevelMeasure.Text;

                System2FlatLevelMeasureSetting system2FlatLevelMeasureSetting =
                    (System2FlatLevelMeasureSetting)System2FlatLevelMeasureRepository.GetInstance().Find(levelMeasureName);

                double z = system2FlatLevelMeasureSetting.ZG0Pos;

                foreach (AKRSPoint2D point2D in system2FlatLevelMeasureSetting.FlatLevelMeasurePointList)
                {
                    AKRSPoint3D point3d = new AKRSPoint3D(point2D.X, point2D.Y, z);

                    this.BondHeadController.MoveBondZToSafePos();

                    // 移动到示教保存的点位
                    this.BondModuleController.MoveToG0Pos(point3d);

                    // 转到G0点
                    AKRSPoint3D point = this.BondModuleController.ConvertG0ToMachinePos(new AKRSPoint3D(0, 0, z));

                    if (!this.ChkLaserMeasure.Checked)
                    {
                        // 测高
                        (ExcuteResult Ret, double HeightValue) res = this.BondHeadController.MeasureHeight(
                            point.Z,
                            HeightMeasurementFunctionEnum.WithTDSensor, 10, 2);

                        if (res.Ret != ExcuteResult.Success)
                        {
                            AKRSXtraMessageBox.Show("测高失败");
                            return result;
                        }

                        // 转到G0上的高度
                        double height = System2Module.GetInstance().BondModule.ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, res.HeightValue)).Z;
                        result.Add(height);
                    }
                    else
                    {
                        System2Domain.GetInstance().S2DispenseController.OpenDispenseHeightMeasurementCylinder();

                        // 到位等待
                        Thread.Sleep(BondDevicePara.GetInstance().S2DispenseDevicePara.LaserMhDelayTime);

                        // 测高
                        double heightInMachine = System2Domain.GetInstance().S2DispenseController.LaserMeasureHeight();

                        if (heightInMachine == double.NaN)
                        {
                            AKRSXtraMessageBox.Show("测高失败");
                            return result;
                        }

                        // 转到G0上的高度
                        double height = System2Module.GetInstance().BondModule.ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, heightInMachine)).Z;
                        result.Add(height);

                        if (this.ChkBackCy.Checked)
                        {
                            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();

                        }
                    }

                }
            }
            catch (Exception e)
            {
                this.BondModuleController.MoveToSafePos();
                AKRSXtraMessageBox.Show("错误：\n" + e.Message);
            }

            return result;
        }

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            DialogResult dr1 = AKRSMessageBoxExt.Show(
                @"请确保焊头上安装了相应的治具
                              点击
                              确定 : 开始
                              取消 : 取消",
                $"提示",
                new[] { "确定", "取消"},
                new[] { DialogResult.OK, DialogResult.Cancel });

            switch (dr1)
            {
                case DialogResult.Cancel:
                    return;

                case DialogResult.OK:

                    // 获取名称
                     string levelMeasureName = this.ImgCmbSystem2FlatLevelMeasure.EditValue.ToString();

                    if (AKRSXtraMessageBox.Show("需要重新示教预测高度吗? ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                    {
                        if (this.ChkLaserMeasure.Checked)
                        {
                            System2Domain.GetInstance().S2DispenseController.OpenDispenseHeightMeasurementCylinder();
                        }

                        FrmSystem2HeightTeach frmSystem2HeightTeach = new FrmSystem2HeightTeach(levelMeasureName);
                        frmSystem2HeightTeach.ShowDialog();
                        frmSystem2HeightTeach.Dispose();

                        if (this.ChkLaserMeasure.Checked)
                        {
                            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
                        }
                    }

                    // 如果没有找到该数据集需要先示教
                    if (System2FlatLevelMeasureRepository.GetInstance().Find(levelMeasureName) == null)
                    {
                        AKRSXtraMessageBox.Show("请先示教", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        // 获取数据集
                        System2FlatLevelMeasureSetting system2FlatLevelMeasureSetting =
                            (System2FlatLevelMeasureSetting)System2FlatLevelMeasureRepository.GetInstance()
                                .Find(levelMeasureName);

                        // 如果高度之前没有示教，则要先示教一次
                        if (system2FlatLevelMeasureSetting.ZG0Pos == BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z)
                        {
                            AKRSXtraMessageBox.Show(
                                "请先示教",
                                "提示",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        else
                        {
                            Remeasure:

                            if (MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh)
                            {
                                System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
                            }

                            // 移动到安全位置
                            this.BondHeadController.MoveBondZToSafePos();

                            // 清空结果
                            this.flatLevelMeasureResultList.Clear();

                            // 循环执行次数
                            int times = Convert.ToInt32(this.SpTimes.EditValue);

                            for (int i = 0; i < times; i++)
                            {
                                // 测高
                                List<double> result = this.Measure();
                                if (result.Count == 0)
                                {  
                                    // 测量结束
                                    this.BondModuleController.MoveToSafePos();
                                    return;                              
                                }

                                double range = result.Max() - result.Min();
                                FlatLevelMeasureResult flatLevelMeasureResult = new FlatLevelMeasureResult();

                                // 把结果传入保存结果的对象，这里用反射方式。示教几个点就有几个结果，剩余的结果默认为零
                                PropertyInfo[] properties = typeof(FlatLevelMeasureResult).GetProperties();

                                for (int j = 0; j < result.Count; j++)
                                {
                                    properties[j].SetValue(flatLevelMeasureResult, result[j]);
                                }

                                flatLevelMeasureResult.Range = range;

                                this.flatLevelMeasureResultList.Add(flatLevelMeasureResult);
                            }

                            if (MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh)
                            {
                                System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
                            }

                            // 测量结束
                            this.BondModuleController.MoveToSafePos();

                            FrmLevelMeasureResult frmLevelMeasureResult =
                                new FrmLevelMeasureResult(levelMeasureName, this.flatLevelMeasureResultList);

                            DialogResult drs = frmLevelMeasureResult.ShowDialog();

                            if (drs == DialogResult.Retry)
                            {
                                goto Remeasure;
                            }

                            frmLevelMeasureResult.Dispose();    
                        }
                    }

                    break;
            }
        }

        /// <summary>
        /// 激光测高
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkLaserMeasure_CheckedChanged(object sender, EventArgs e)
        {
            string name = this.ImgCmbSystem2FlatLevelMeasure.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            System2FlatLevelMeasureSetting system2FlatLevelMeasureSetting =
               (System2FlatLevelMeasureSetting)System2FlatLevelMeasureRepository.GetInstance().Find(name);

            system2FlatLevelMeasureSetting.IsLaser = this.ChkLaserMeasure.Checked;

            System2FlatLevelMeasureRepository.GetInstance().Save();
        }

        /// <summary>
        /// 是否收回气缸
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkBackCy_CheckedChanged(object sender, EventArgs e)
        {
            string name = this.ImgCmbSystem2FlatLevelMeasure.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            System2FlatLevelMeasureSetting system2FlatLevelMeasureSetting =
               (System2FlatLevelMeasureSetting)System2FlatLevelMeasureRepository.GetInstance().Find(name);

            system2FlatLevelMeasureSetting.IsBackCy = this.ChkBackCy.Checked;

            System2FlatLevelMeasureRepository.GetInstance().Save();
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ImgCmbSystem2FlatLevelMeasure_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = this.ImgCmbSystem2FlatLevelMeasure.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            System2FlatLevelMeasureSetting system2FlatLevelMeasureSetting =
               (System2FlatLevelMeasureSetting)System2FlatLevelMeasureRepository.GetInstance().Find(name);

            this.ChkBackCy.Checked = system2FlatLevelMeasureSetting.IsBackCy;
            this.ChkLaserMeasure.Checked = system2FlatLevelMeasureSetting.IsLaser;
        }
    }
}