using System;
using System.Drawing;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;


namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    /// <summary>
    /// 吸嘴测高示教窗体
    /// </summary>
    public partial class FrmToolHeightTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nozzle">吸嘴对象</param>
        public FrmToolHeightTeach(Nozzle nozzle)
        {
            this.nozzle = nozzle;
            this.InitializeComponent();
            this.InitControl();
            this.InitMovement();
        }

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 判断当前是否在运动，在运动的时候不能取消示教
        /// </summary>
        private bool isMove;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 点击Start传进来的吸嘴对象
        /// </summary>
        private Nozzle nozzle;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmToolHeightTeach");

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.PnlControl.Controls.Add(ucGuideMove1);

            this.LbDescription.Text = @"Step1/1:确定吸嘴Z轴高度：将吸嘴移动到BMC平台上正上方大约5mm的位置。";

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiHeightMeasurement;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);

            this.BtnVacuum.Appearance.BackColor = Color.Transparent;

            this.Size = new Size(749, 850);
        }

        /// <summary>
        /// 初始运动
        /// </summary>
        private void InitMovement()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (!this.system2Controller.ChangeNozzleAssistance(this.nozzle.Name)) 
            {
                return;
            }               
            

            //if (this.BondModule.BondHead.CheckToolSensor == null)
            //{
            //    DialogResult dialog = AKRSXtraMessageBox.Show(
            //        $"CAUTION:no sensor on the bonding head.\r\n" + "Ignore with  OK\r\n" + "End with  Cancel\r\n",
            //        "Warn",
            //        MessageBoxButtons.OKCancel,
            //        MessageBoxIcon.Warning);

            //    if (dialog == DialogResult.Cancel)
            //    {
            //        return;
            //    }
            //}
            //else
            //{
            //    //// 检查焊头上是否存在吸嘴
            //    //if (!this.BondModule.BondHead.CheckToolSensor.GetInputValue())
            //    //{
            //    //    DialogResult dialog = AKRSXtraMessageBox.Show(
            //    //        $"Place tool: {this.nozzle.Name} into  slot{this.nozzle.SlotIdentification} \r\n" + "CAUTION:no tool may be on the bonding head.\r\n" + "Confirm with  OK\r\n",
            //    //        "Warn",
            //    //        MessageBoxButtons.OK,
            //    //        MessageBoxIcon.Warning);

            //    //    return;
            //    //}
            //}

            this.isMove = true;

            // 获取BMC测高位置
            AKRSPoint3D point = CalibrateRunPara.GetInstance().BMCMeasureHeightSearchMachinePos;

            // 目标位置往上抬5mm
            AKRSPoint3D targetPos = point + new AKRSPoint3D(0, 0, 8);

            // 移动到测高位置
            this.bondModuleController.MoveSafeBondXYZ(targetPos);

            this.isMove = false;
        }

        /// <summary>
        /// 点击Done执行事件
        /// </summary>
        private void DoneAction()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 手动测高
            if (this.nozzle.ZDeterminationMethod == ZDeterminationMethodEnum.Manual)
            {            
                if (System2Module.GetInstance().BondModule.BondHead.AxisZ == null)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"CAUTION:Bond  axis  Z  is  null.\r\n" + "Ignore with  OK\r\n" + "End with  Cancel\r\n",
                        "Warn",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (dialog == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    // 保存测高结果
                    this.nozzle.MeasureHeightOffset =
                        this.bondHeadController.GetAxisZRealPos() - BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult;
                }

                System2Domain.GetInstance().BondHeadController.MoveBondZToSafePos();

                return;
            }

            // 执行测高动作
            if (this.nozzle.NozzleHeightMeasurementFunction == HeightMeasurementFunctionEnum.WithTDSensor)
            {
                // 执行测高
                (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                    HeightMeasurementFunctionEnum.WithTDSensor);

                if (res.Ret == ExcuteResult.Success)
                {
                    // 保存测高结果
                    this.nozzle.MeasureHeightOffset =
                        res.HeightValue - BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult;
                }
            }
            else if (this.nozzle.NozzleHeightMeasurementFunction
                     == HeightMeasurementFunctionEnum.WithVacuumSensor)
            {
                // 执行测高
                (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                    HeightMeasurementFunctionEnum.WithVacuumSensor);

                double a = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult;

                if (res.Ret == ExcuteResult.Success)
                {
                    // 保存测高结果
                    this.nozzle.MeasureHeightOffset =
                        res.HeightValue - BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult;

                    this.DialogResult = DialogResult.OK;
                }
            }
            else
            {
                // 弹出压力实时曲线
                // 需要重写一个测高方法，达到指定力就停止测高
                // 暂时不需要
            }
        }

        /// <summary>
        /// 关闭窗体后事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleHeightTeach_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.isMove)
            {
                // 关闭安全们
                return;
            }
        }

        /// <summary>
        /// 关闭窗体时事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleHeightTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果轴正运动，不可取消
            if (this.isMove)
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 开关真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnVacuum_Click(object sender, EventArgs e)
        {
            if (!this.bondHeadController.GetNozzleVacuumState())
            {
                this.bondHeadController.OpenToolVaccum();
                this.BtnVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.bondHeadController.CloseToolVaccum();
                this.BtnVacuum.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDone_Click(object sender, EventArgs e)
        {
            this.stepIndex++;
            this.DoneAction();

            this.nozzle.ToolHeight.State = AssistantStateEnum.Able;
            NozzleRepository.GetInstance().Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否结束示教?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.Close();
                this.bondHeadController.MoveBondZToSafePos();
            }
        }
    }
}