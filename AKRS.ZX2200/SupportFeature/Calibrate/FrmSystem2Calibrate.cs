using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Controls.Assistant;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Calibrate
{
    /// <summary>
    /// 系统2校准
    /// </summary>
    public partial class FrmSystem2Calibrate : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// EjectDevicePara
        /// </summary>
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// 吸嘴架程式
        /// </summary>
        private NozzleShelfProgram nozzleShelfProgram => BondProgram.GetInstance().NozzleShelfProgram;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        ///  系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 取片线程
        /// </summary>
        private Task pickupTask;

        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmSystem2Calibrate()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            // 绑定顶针
            this.CmbEjectionName.Properties.Items.Clear();

            List<EjectionBankSlotConfig> ejectionBankConfigList =
                WaferSystemProgram.GetInstance().GetDistinctEjectionBankSlotConfig();

            if (ejectionBankConfigList.Count != 0)
            {
                foreach (var item in ejectionBankConfigList)
                {
                    this.CmbEjectionName.Properties.Items.Add(item.EjectionConfig.Name);
                }

                if (EjectDevicePara.CurrentSlotConfig != null)
                {
                    this.CmbEjectionName.EditValue = EjectDevicePara.CurrentSlotConfig.EjectionConfig.Name;
                }
            }

            // 绑定吸嘴
            this.CmbNozzleName.Properties.Items.Clear();

            if (nozzleShelfProgram.GetNozzleNameList().Count != 0)
            {
                foreach (var item in nozzleShelfProgram.GetNozzleNameList())
                {
                    this.CmbNozzleName.Properties.Items.Add(item);
                }

                this.CmbNozzleName.EditValue = bondHeadController.GetCurrentNozzleName();
            }

            // 绑定芯片
            this.CmbComponent.Properties.Items.Clear();
            this.CmbComponent1.Properties.Items.Clear();

            if (WaferSystemProgram.GetInstance().GetCarriers().Count != 0)
            {
                foreach (var item in WaferSystemProgram.GetInstance().GetCarriers())
                {
                    this.CmbComponent.Properties.Items.Add(item.Name);
                    this.CmbComponent1.Properties.Items.Add(item.Name);
                }
            }

            this.BtnCaliAll.Visible = false;
            this.BtnCaliAll.Enabled = false;
        }

        /// <summary>
        /// 校准吸嘴
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCaliNozzle_Click(object sender, EventArgs e)
        {
            bool checkRes = this.system2Controller.PrepareBeforceNozzleAutoCali(this.CmbNozzleName.Text);

            if (checkRes == false)
            {
                return;
            }

            //if (AKRSXtraMessageBox.Show("是否需要重新做吸嘴模板？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            //    == DialogResult.Yes)
            //{
            //    FrmNozzleCaliTeach frmNozzleCaliTeach = new FrmNozzleCaliTeach(nozzle);
            //    if (frmNozzleCaliTeach.IsShowDialog())
            //    {
            //        DialogResult dialog = frmNozzleCaliTeach.ShowDialog();

            //        if (dialog == DialogResult.Cancel)
            //        {
            //            return;
            //        }
            //        else
            //        {
            //            if (AKRSXtraMessageBox.Show("是否开始校正吸嘴？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            //                != DialogResult.Yes)
            //            {
            //                System2Domain.GetInstance().BondModuleController.MoveToSafePos();
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            Nozzle nozzle = NozzleRepository.GetInstance().GetNozzle(this.CmbNozzleName.Text);

            // 校准前数据
            AKRSPoint2D initialVal = new AKRSPoint2D(nozzle.NozzleOffset.X, nozzle.NozzleOffset.Y);

            // 自动校准
            ExcuteResult res = this.system2Controller.NozzleCaliAssistance(nozzle);

            if (res != ExcuteResult.Success)
            {
                AKRSXtraMessageBox.Show($"吸嘴：{nozzle.Name}自动校准失败！", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                AKRSXtraMessageBox.Show($"吸嘴：{nozzle.Name}自动校准完成！\r\n校准前 X:{initialVal.X},Y:{initialVal.Y} ,校准后X:{nozzle.NozzleOffset.X},Y:{nozzle.NozzleOffset.Y}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ///// <summary>
        ///// 校准顶针前准备,旧
        ///// </summary>
        ///// <exception cref="Exception">异常</exception>
        //private void PrepareBeforeCaliEjection()
        //{
        //    WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

        //    // 顶针台下降到不剥离位置
        //    WaferSubController.GetInstance().EjectController.MoveEjectionToInseparablePos();

        //    // 晶圆台去准备位
        //    // 这里顶针台已经下降到安全位置，所以不检查
        //    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(WaferSubDevicePara.GetInstance().WaferTableDevicePara.ReadyPosition, true);

        //    // 检查料片
        //    bool hasTablet = WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable();

        //    if (hasTablet)
        //    {

        //        AKRSMessageBoxExt.Show("晶圆台感应到料片, 请先手动拿出料片！ \r\n" , "Prompt", new string[] { "确认" }, new DialogResult[] { DialogResult.OK });

        //        throw new Exception("晶圆台没有感应到料片, 请先手动更换料片！");
        //    }
        //    else
        //    {
        //        WaferSubController.GetInstance().WaferTableController.MoveExpandToDownPosition();

        //        // 放下晶圆夹持气缸
        //        WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
        //    }

        //    Retry:
        //    // 检查是否手动拿出料片
        //    hasTablet = WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable();

        //    if (hasTablet)
        //    {
        //        DialogResult dia = AKRSMessageBoxExt.Show("晶圆台感应到料片, 校准顶针前请先将料片拿出！ \r\n", "Prompt", new string[] { "重试", "取消" }, new DialogResult[] { DialogResult.Retry, DialogResult.Cancel });

        //        switch (dia)
        //        {
        //            case DialogResult.Retry:
        //                goto Retry;
        //            case DialogResult.Cancel:
        //                throw new Exception("晶圆台感应到料片, 请人工确保晶圆台内无料！");
        //        }
        //    }
        //}

        ///// <summary>
        ///// 校准顶针后动作
        ///// </summary>
        ///// <param name="component">芯片</param>
        ///// <exception cref="Exception">异常</exception>
        //private void ActionAfterCaliEjection(CarrierWithWaferConfig component)
        //{
        //    // 顶针座下降到不剥离位置
        //    WaferSubController.GetInstance().EjectController.MoveEjectionToInseparablePos();
        //    WaferSubController.GetInstance().WaferTableController.MoveExpandToDownPosition();

        //    Retry:
        //    // 检查料片
        //    bool hasTablet = WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable();

        //    if (hasTablet == false)
        //    {
        //        DialogResult dia = AKRSMessageBoxExt.Show(
        //            "顶针示教结束, 请放回料片！ \r\n" + "重试：重新检测\r\n",
        //            "Prompt",
        //            new string[] { "重试" },
        //            new DialogResult[] { DialogResult.Retry });

        //        switch (dia)
        //        {
        //            case DialogResult.Retry:
        //                goto Retry;

        //         default:
        //                throw new Exception("顶针校准结束后未放回料片！");
        //        }
        //    }
        //    else
        //    {
        //        // 晶圆夹持气缸抬起
        //        WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();

        //        // 扩晶
        //        WaferSubController.GetInstance().WaferTableController.MoveExpandToUpPosition(component);

        //        // 顶针座升起
        //        WaferSubController.GetInstance().EjectController.MoveEjectionTableToWorkPosition();
        //    }
        //}

        /// <summary>
        /// 校准顶针
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCaliEjection_Click(object sender, EventArgs e)
        {
            if (this.system2Controller.PrepareBeforeEjectionAutoCali(this.CmbEjectionName.Text) == false)
            {
                return;
            }

            // 获取晶圆对象
            CarrierWithWaferConfig component =
                (CarrierWithWaferConfig)CarrierConfigRepository.GetInstance().Find(this.CmbComponent1.Text);

            EjectionBankSlotConfig ejectionBankSlotConfig = WaferSystemProgram.GetInstance()
                .GetDistinctEjectionBankSlotConfig().Find(it => it.EjectionConfig.Name == this.CmbEjectionName.Text);


            // 校准前数据
            AKRSPoint3D initialVal = new AKRSPoint3D(
                    ejectionBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter.X,
                    ejectionBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter.Y,
                    ejectionBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter.Z);

            if (AKRSXtraMessageBox.Show("是否需要做顶针中心模板？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                == DialogResult.Yes)
            {
                FrmEjectionCaliTeach frmEjectionCaliTeach = new FrmEjectionCaliTeach(ejectionBankSlotConfig);
                if (frmEjectionCaliTeach.IsShowDialog())
                {
                    DialogResult dialog = frmEjectionCaliTeach.ShowDialog();

                    if (dialog == DialogResult.OK)
                    {
                        AKRSXtraMessageBox.Show(
                          $"顶针：{ejectionBankSlotConfig.EjectionConfig.Name}自动校准完成！\r\n校准前 顶针中心和晶圆相机之间的偏移X：{initialVal.X},Y:{initialVal.Y} \r\n校准后 顶针中心和晶圆相机之间的偏移X：{ejectionBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter.X},Y:{ejectionBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter.Y} ",
                          "提示",
                          MessageBoxButtons.OK,
                          MessageBoxIcon.Information);
                    }

                    return;
                }
                else
                {
                    return;
                }
            }

            // 自动校准
            ExcuteResult res = this.system2Controller.EjectionConfigCaliAssistance(ejectionBankSlotConfig);

            if (res != ExcuteResult.Success)
            {
                AKRSXtraMessageBox.Show(
                    $"顶针：{ejectionBankSlotConfig.EjectionConfig.Name}自动校准失败！",
                    "异常",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }
            else
            {
                //this.ActionAfterCaliEjection(component);

                AKRSXtraMessageBox.Show(
                    $"顶针：{ejectionBankSlotConfig.EjectionConfig.Name}自动校准完成！\r\n校准前 顶针中心和晶圆相机之间的偏移X：{initialVal.X},Y:{initialVal.Y} \r\n校准后 顶针中心和晶圆相机之间的偏移X：{ejectionBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter.X},Y:{ejectionBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter.Y} ",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        ///  取片偏移校准
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnCaliPickOffset_Click(object sender, EventArgs e)
        {
            BaseCarrierConfig component =
                (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(this.CmbComponent.Text);

            if (component == null)
            {
                AKRSXtraMessageBox.Show("请先选择芯片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Nozzle nozzle = NozzleRepository.GetInstance().GetNozzle(component.NozzleName);

            if (nozzle == null)
            {
                AKRSXtraMessageBox.Show($"芯片{component}绑定的吸嘴为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (AKRSXtraMessageBox.Show("是否需要示教芯片模板？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                == DialogResult.Yes)
            {
                FrmComponentAccuracyMode frmComponentAccuracyMode = new FrmComponentAccuracyMode(component, true);
                if (frmComponentAccuracyMode.IsShowDialog())
                {
                    DialogResult dialog = frmComponentAccuracyMode.ShowDialog();

                    if (dialog == DialogResult.Cancel)
                    {
                        return;
                    }

                    if (AKRSXtraMessageBox.Show(
                            "是否开始校正芯片取片偏移？",
                            "提示",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information) == DialogResult.No)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            // 校准前数据
            AKRSPoint2D initialVal = new AKRSPoint2D(
               component.PickupOffset.X, component.PickupOffset.Y);
            ExcuteResult ret = ExcuteResult.Fail;
            this.system2Controller.PrepareBeforePickOffsetAutoCali(component);

            // 防呆
            SimpleButton btn = sender as SimpleButton;
            btn.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            try
            {
                await Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("取片偏移自动校准线程");

                            ret = this.system2Controller.PickOffsetCaliAssistance(component);
                        });
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(
                    $"芯片：{component.Name}取片偏移自动校准失败！\r\n{exception.ToString()}",
                    "异常",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }
            finally
            {
                btn.Enabled = true;
                btn.Appearance.BackColor = Color.Transparent;
            }

            if (ret == ExcuteResult.Success)
            {
                AKRSXtraMessageBox.Show(
                    $"芯片：{component.Name}取片偏移自动校准完成！\r\n校准前 取片偏移X：{initialVal.X},Y:{initialVal.Y}  校准后 取片偏移X：{component.PickupOffset.X},Y:{component.PickupOffset.Y} ",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 全部校准
        /// 顶针校准需要手动拿出料片，等这个问题解决后再开发一键校准
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnCaliAll_Click(object sender, EventArgs e)
        {
            //// 获取晶圆对象
            //CarrierWithWaferConfig component =
            //    (CarrierWithWaferConfig)CarrierConfigRepository.GetInstance().Find(this.CmbComponent.Text);

            //Nozzle nozzle = NozzleRepository.GetInstance().GetNozzle(component.NozzleName);

            //// 获取顶针对象
            //EjectionConfig ejectionConfig =
            //    (EjectionConfig)EjectionConfigRepository.GetInstance().Find(component.EjectionName);

            //// 顶针槽
            //EjectionBankSlotConfig ejectionBankSlotConfig = WaferSystemProgram.GetInstance()
            //    .GetDistinctEjectionBankSlotConfig().Find(it => it.EjectionConfig.Name == component.EjectionName);

            //if (component == null)
            //{
            //    AKRSXtraMessageBox.Show("请先选择芯片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //if (nozzle == null)
            //{
            //    AKRSXtraMessageBox.Show($"芯片{component}绑定的吸嘴为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //if (ejectionConfig == null)
            //{
            //    AKRSXtraMessageBox.Show($"芯片{component}绑定的顶针为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            //ExcuteResult res = ExcuteResult.Fail;

            //// 防呆
            //SimpleButton btn = sender as SimpleButton;
            //btn.Enabled = false;
            //btn.Appearance.BackColor = Color.Yellow;

            //try
            //{
            //    await Task.Run(
            //        () =>
            //            {
            //                CommonUtil.SetCurrentThreadName("三点一线自动校准线程");

            //                // 检查料片
            //                WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);

            //                // 校准顶针
            //                res = this.system2Controller.EjectionConfigCaliAssistance(ejectionBankSlotConfig);

            //                if (res != ExcuteResult.Success)
            //                {
            //                    AKRSXtraMessageBox.Show(
            //                        $"顶针：{ejectionBankSlotConfig.EjectionConfig.Name}自动校准失败！",
            //                        "异常",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Error);

            //                    return;
            //                }

            //                // 校准吸嘴
            //                res = this.system2Controller.NozzleCaliAssistance(nozzle);

            //                if (res != ExcuteResult.Success)
            //                {
            //                    AKRSXtraMessageBox.Show($"吸嘴：{nozzle.Name}自动校准失败！", "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);

            //                    return;
            //                }

            //                // 准备
            //                this.PrepareBeforePick(component);

            //                // 校准取片偏移
            //                res = this.system2Controller.PickOffsetCaliAssistance(component);

            //                if (res != ExcuteResult.Success)
            //                {
            //                    AKRSXtraMessageBox.Show(
            //                        $"芯片：{component.Name}取片偏移自动校准失败！",
            //                        "异常",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Error);

            //                    return;
            //                }
            //            });
            //}
            //catch (Exception exception)
            //{
            //    AKRSXtraMessageBox.Show(
            //        $"芯片：{component.Name}取片偏移自动校准异常！\r\nexception:{exception.ToString()}",
            //        "异常",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Error);

            //    return;
            //}
            //finally
            //{
            //    btn.Enabled = true;
            //    btn.Appearance.BackColor = Color.Transparent;
            //}

            //if (res == ExcuteResult.Success)
            //{
            //    AKRSXtraMessageBox.Show(
            //        $"芯片：{component.Name}一键校准完成！",
            //        "提示",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Information);
            //}
        }

        /// <summary>
        ///  窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmSystem2Calibrate_FormClosing(object sender, FormClosingEventArgs e)
        {
            Machine.GetInstance().Stop();
        }

        /// <summary>
        ///  下拉框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbComponent1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 获取晶圆对象
            CarrierWithWaferConfig component =
                (CarrierWithWaferConfig)CarrierConfigRepository.GetInstance().Find(this.CmbComponent1.Text);

            if (component != null)
            {
                this.CmbEjectionName.EditValue = component.EjectionName;
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmSystem2Calibrate_Load(object sender, EventArgs e)
        {
            System2RunTimeProvider.IsWaferCheckSucceed = false;
        }
    }
}
