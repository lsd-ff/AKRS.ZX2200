using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Main.Controls
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Parameter;
    using DevExpress.XtraEditors;

    /// <summary>
    ///  示教摇杆
    /// </summary>
    public partial class FrmTeachJoystick : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmTeachJoystick()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        ///  摇杆参数
        /// </summary>
        private JoystickPara joystickPara => MachineDevicePara.GetInstance().JoystickPara;

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitControl()
        {
            this.SpXInitialVal.Value = (decimal)this.joystickPara.XInitialVal;
            this.SpYInitialVal.Value = (decimal)this.joystickPara.YInitialVal;
            this.SpTInitialVal.Value = (decimal)this.joystickPara.TInitialVal;

            this.SpXMaxVal.Value = (decimal)this.joystickPara.XUpperLimit;
            this.SpYMaxVal.Value = (decimal)this.joystickPara.YUpperLimit;
            this.SpTMaxVal.Value= (decimal)this.joystickPara.TUpperLimit;

            this.SpBlindRange.Value = (decimal)this.joystickPara.BlindRange;
        }

        /// <summary>
        /// X初始值写入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSetXInitialVal_Click(object sender, EventArgs e)
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
                MachineModule.GetInstance().JoystickXRead.AxisCardNum,
                (short)MachineModule.GetInstance().JoystickXRead.ElectricIO,
                out double pValue,
                1,
                out UInt32 p1Clock);

            this.SpXInitialVal.Value   = Convert.ToDecimal(Math.Round(pValue, 6) * 1000.0);
        }

        /// <summary>
        /// Y初始值写入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSetYInitialVal_Click(object sender, EventArgs e)
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
                MachineModule.GetInstance().JoystickYRead.AxisCardNum,
                (short)MachineModule.GetInstance().JoystickYRead.ElectricIO,
                out double pValue,
                1,
                out UInt32 p1Clock);

            this.SpYInitialVal.Value   = Convert.ToDecimal(Math.Round(pValue, 6) * 1000.0);
        }

        /// <summary>
        /// T初始值写入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSetTInitialVal_Click(object sender, EventArgs e)
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
                MachineModule.GetInstance().JoystickTRead.AxisCardNum,
                (short)MachineModule.GetInstance().JoystickTRead.ElectricIO,
                out double pValue,
                1,
                out UInt32 p1Clock);

            this.SpTInitialVal.Value   = Convert.ToDecimal(Math.Round(pValue, 6) * 1000.0);
        }

        /// <summary>
        /// X最大值写入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSetXMaxVal_Click(object sender, EventArgs e)
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
                MachineModule.GetInstance().JoystickXRead.AxisCardNum,
                (short)MachineModule.GetInstance().JoystickXRead.ElectricIO,
                out double pValue,
                1,
                out UInt32 p1Clock);

            this.SpXMaxVal.Value = Convert.ToDecimal(Math.Round(pValue, 6) * 1000.0);
        }

        /// <summary>
        /// Y最大值写入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSetYMaxVal_Click(object sender, EventArgs e)
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
                MachineModule.GetInstance().JoystickYRead.AxisCardNum,
                (short)MachineModule.GetInstance().JoystickYRead.ElectricIO,
                out double pValue,
                1,
                out UInt32 p1Clock);

            this.SpYMaxVal.Value = Convert.ToDecimal(Math.Round(pValue, 6) * 1000.0);
        }

        /// <summary>
        /// T最大值写入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSetTMaxVal_Click(object sender, EventArgs e)
        {
            short rtn = GTN.mc.GTN_GetAuAdc(
                MachineModule.GetInstance().JoystickTRead.AxisCardNum,
                (short)MachineModule.GetInstance().JoystickTRead.ElectricIO,
                out double pValue,
                1,
                out UInt32 p1Clock);

            this.SpTMaxVal.Value = Convert.ToDecimal(Math.Round(pValue, 6) * 1000.0);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            MachineDevicePara.GetInstance().JoystickPara.XInitialVal = (double)this.SpXInitialVal.Value;
            MachineDevicePara.GetInstance().JoystickPara.YInitialVal = (double)this.SpYInitialVal.Value;
            MachineDevicePara.GetInstance().JoystickPara.TInitialVal = (double)this.SpTInitialVal.Value;

            MachineDevicePara.GetInstance().JoystickPara.XUpperLimit = (double)this.SpXMaxVal.Value;
            MachineDevicePara.GetInstance().JoystickPara.YUpperLimit = (double)this.SpYMaxVal.Value;
            MachineDevicePara.GetInstance().JoystickPara.TUpperLimit = (double)this.SpTMaxVal.Value;

            MachineDevicePara.GetInstance().JoystickPara.BlindRange = (double)this.SpBlindRange.Value;

            MachineDevicePara.GetInstance().Save();

            AKRSXtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsJoyStickConfigured)
            {
                short rtn = GTN.mc.GTN_GetAuAdc(
                    MachineModule.GetInstance().JoystickXRead.AxisCardNum,
                    (short)MachineModule.GetInstance().JoystickXRead.ElectricIO,
                    out double pValue,
                    1,
                    out UInt32 p1Clock);

                this.SpJoystickX.Value = Convert.ToDecimal(Math.Round(pValue, 6) * 1000.0);

                rtn = GTN.mc.GTN_GetAuAdc(
                    MachineModule.GetInstance().JoystickYRead.AxisCardNum,
                    (short)MachineModule.GetInstance().JoystickYRead.ElectricIO,
                    out double pValue2,
                    1,
                    out UInt32 p2Clock);

                this.SpJoystickY.EditValue = Convert.ToDecimal(Math.Round(pValue2, 6) * 1000.0);

                rtn = GTN.mc.GTN_GetAuAdc(
                    MachineModule.GetInstance().JoystickTRead.AxisCardNum,
                    (short)MachineModule.GetInstance().JoystickTRead.ElectricIO,
                    out double pValue3,
                    1,
                    out UInt32 p3Clock);

                this.SpJoystickT.EditValue = Convert.ToDecimal(Math.Round(pValue3, 6) * 1000.0);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmTeachJoystick_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1?.Dispose();
        }
    }
}
