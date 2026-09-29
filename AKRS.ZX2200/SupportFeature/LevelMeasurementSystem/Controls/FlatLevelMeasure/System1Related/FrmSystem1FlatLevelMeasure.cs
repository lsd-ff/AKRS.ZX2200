namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure.System1Related
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.LevelMeasurementSystem.Models.FlatLevelMeasure;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure.System1Related;

    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;

    /// <summary>
    /// 系统1水平测量
    /// </summary>
    public partial class FrmSystem1FlatLevelMeasure : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController DispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// 点胶测高传感器
        /// </summary>
        private DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// 结果存储
        /// </summary>
        private List<FlatLevelMeasureResult> flatLevelMeasureResultList = new List<FlatLevelMeasureResult>();

        /// <summary>
        /// 初始化
        /// </summary>
        public FrmSystem1FlatLevelMeasure()
        {
            this.InitializeComponent();

            this.RefreshCmbItems();
        }

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            this.ImgCmbSystem1FlatLevelMeasure.Properties.Items.Clear();
            ImageComboBoxItem[] list = System1FlatLevelMeasureRepository.GetInstance()
                .Filter(string.Empty, false)
                .Select(a => new ImageComboBoxItem(a.Name, a.Name, (int)a.EditState)).ToArray();
            this.ImgCmbSystem1FlatLevelMeasure.Properties.Items.AddRange(list);
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
                string levelMeasureName = this.ImgCmbSystem1FlatLevelMeasure.Text;

                System1FlatLevelMeasureSetting system1FlatLevelMeasureSetting =
                (System1FlatLevelMeasureSetting)System1FlatLevelMeasureRepository.GetInstance().Find(levelMeasureName);

                double z = system1FlatLevelMeasureSetting.ZG0Pos;


                foreach (AKRSPoint2D point2D in system1FlatLevelMeasureSetting.FlatLevelMeasurePointList)
                {
                    AKRSPoint3D point3d = new AKRSPoint3D(point2D.X, point2D.Y, z);

                    this.DispenseController.MoveToG0Pos3D(point3d);

                    // 转到G0点
                    AKRSPoint3D point = this.DispenseController.ConvertG0ToMachinePos(new AKRSPoint3D(0, 0, z));

                    // 测高
                    (ExcuteResult Ret, double HeightValue) res =
                        this.dispenseMeasureHeightController.DispenserHeightMeasurementG0(
                            System1MeasHeightToolEnum.HeightSensor,
                            null,
                            point.Z,
                            double.NaN,
                            false);

                    if (res.Ret != ExcuteResult.Success)
                    {
                        AKRSXtraMessageBox.Show("测高失败");
                        return result;
                    }

                    result.Add(res.HeightValue);
                }
            }
            catch (Exception e)
            {
                this.DispenseController.MoveToSafePos();
                AKRSXtraMessageBox.Show($"测高失败 {e.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // 用线程， 且界面提供停止按钮 
            // short ret = GTN.mc.GTN_SetStopDec(1, 3, 5, 1000);
            // ret = GTN.mc.GTN_GetStopDec(1, 3, out double decSmooth, out double dec);

            // 获取当前要测试的名称
            string levelMeasureName = this.ImgCmbSystem1FlatLevelMeasure.Text;

            if (string.IsNullOrWhiteSpace(levelMeasureName))
            {
                AKRSXtraMessageBox.Show("请选择一个测高位置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 是否需要重新选择高度
            if (AKRSXtraMessageBox.Show("要重新示教预测高度吗? ", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                this.DispenseController.MoveToSafePos();
                this.dispenseMeasureHeightController.OpenDispenseHeightMeasurementCylinder();
                FrmSystem1HeightTeach frmSystem1HeightTeach = new FrmSystem1HeightTeach(levelMeasureName);

                if (frmSystem1HeightTeach.ShowDialog() == DialogResult.Cancel)
                {
                    frmSystem1HeightTeach.Dispose();
                    return;
                }
            }

            if (System1FlatLevelMeasureRepository.GetInstance().Find(levelMeasureName) == null)
            {
                AKRSXtraMessageBox.Show("请先示教", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // 获取数据集
                System1FlatLevelMeasureSetting system1FlatLevelMeasureSetting =
                    (System1FlatLevelMeasureSetting)System1FlatLevelMeasureRepository.GetInstance()
                        .Find(levelMeasureName);

                // 如果高度之前没有示教，则要先示教一次
                if (system1FlatLevelMeasureSetting.ZG0Pos == 0)
                {
                    AKRSXtraMessageBox.Show(
                        "请先示教预测高度",
                        "提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                else
                {
                    Remeasure:

                    // 清空结果
                    this.flatLevelMeasureResultList.Clear();

                    this.DispenseController.MoveToSafePos();

                    // 循环执行次数
                    int times = Convert.ToInt32(this.SpTimes.EditValue);

                    for (int i = 0; i < times; i++)
                    {
                        /** 测试用 ****/
                        this.dispenseMeasureHeightController.OpenDispenseHeightMeasurementCylinder();
                        /***********/

                        // 测高
                        List<double> result = this.Measure();

                        if (result.Count == 0)
                        {      
                            // 测量结束
                            this.DispenseController.MoveToSafePos();
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

                        /** 测试用 ****/
                        // this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();
                        /******/
                    }

                    // 测量结束
                    this.DispenseController.MoveToSafePos();

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
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ImgCmbSystem1FlatLevelMeasure_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<System1FlatLevelMeasureSetting> frmRepository = new FrmRepository<System1FlatLevelMeasureSetting>(System1FlatLevelMeasureRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    System1FlatLevelMeasureSetting system1FlatLevelMeasureSetting = (System1FlatLevelMeasureSetting)frmRepository.DsSetting;

                    if (system1FlatLevelMeasureSetting != null)
                    {
                        ((ComboBoxEdit)sender).Text = system1FlatLevelMeasureSetting.Name;
                    }

                    frmRepository.Dispose();
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 示教
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnTeach_Click(object sender, EventArgs e)
        {
            string name = this.ImgCmbSystem1FlatLevelMeasure.Text;

            if (string.IsNullOrWhiteSpace(name))
            {
                AKRSXtraMessageBox.Show("请选择一个测高位置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FrmSystem1FlatLevelMeasureTeach frmSystem1FlatLevelMeasureTeach = new FrmSystem1FlatLevelMeasureTeach(name);
            frmSystem1FlatLevelMeasureTeach.ShowDialog();

            frmSystem1FlatLevelMeasureTeach.Dispose();  
        }
    }
}