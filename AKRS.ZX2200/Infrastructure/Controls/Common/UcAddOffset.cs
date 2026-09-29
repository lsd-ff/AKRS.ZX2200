using DevExpress.CodeParser;
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

namespace AKRS.ZX2200.Infrastructure.Controls.Common
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;

    /// <summary>
    /// 添加偏移
    /// </summary>
    public partial class UcAddOffset : XtraUserControl
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public UcAddOffset()
        {
            this.InitializeComponent();
            this.BtHeightAdd.Text = "高\n度\n增\n加";

            this.BtHeightDe.Text = "高\n度\n降\n低";

            this.BackColor = Color.AliceBlue;
        }

        /// <summary>
        /// 配置对象
        /// </summary>
        private object config = new object();

        /// <summary>
        /// 是否为焊点
        /// </summary>
        private bool isBond = true;

        /// <summary>
        /// 设置偏移量
        /// </summary>
        /// <param name="config">配置文件</param>
        /// <param name="isBond">是否为焊点偏移</param>
        public void SetOffset(object config, bool isBond = true)
        {
            this.config = config;
            this.Ck1um.Checked = true;
            this.isBond = isBond;

            if (config is SingleBondPositionConfig)
            {
                SingleBondPositionConfig config2 = (SingleBondPositionConfig)config;

                if (isBond)
                {
                    if (BondProgram.GetInstance().PostBondProgram.IsCompensateOpen())
                    {
                        this.Enabled = false;
                        this.labelControl10.Visible = true;
                    }

                    this.SpValueX.EditValue = config2.BondPosOffset.X * 1000.0;

                    this.SpValueY.EditValue = config2.BondPosOffset.Y * 1000.0;

                    this.SpHeight.EditValue = config2.BondPosOffset.Z * 1000.0;

                    this.SpValueAngle.EditValue = config2.RotaryPosition;
                }
                else
                {
                    this.SpValueX.EditValue = config2.DispensePosOffset.X * 1000.0;

                    this.SpValueY.EditValue = config2.DispensePosOffset.Y * 1000.0;

                    this.SpHeight.EditValue = config2.DispensePosOffset.Z * 1000.0;

                    this.SpValueAngle.EditValue = config2.DispenseRotaryPosition;
                }
            }
            else if (config is EpoxyApplication)
            {
                this.Enabled = true;
                this.labelControl10.Visible = false;

                EpoxyApplication config2 = (EpoxyApplication)config;

                this.SpValueX.EditValue = config2.OffsetX * 1000.0;

                this.SpValueY.EditValue = config2.OffsetY * 1000.0;

                this.SpHeight.EditValue = config2.OffsetZ * 1000.0;

                this.BtAngleAdd.Visible = false;

                this.BtAngleDe.Visible = false;

                this.SpValueAngle.Visible = false;

                this.labelControl4.Visible = false;

                this.labelControl7.Visible = false;
            }
            else if (config is OppositeSexConfig)
            {
                if (isBond)
                {
                    OppositeSexConfig oppositeSex = (OppositeSexConfig)config;
                    this.SpValueX.EditValue = oppositeSex.PositionCompensate.X * 1000.0;

                    this.SpValueY.EditValue = oppositeSex.PositionCompensate.Y * 1000.0;

                    this.SpHeight.EditValue = oppositeSex.PositionCompensate.Z * 1000.0;

                    this.SpValueAngle.EditValue = oppositeSex.AngleCompensate;
                }
                else
                {
                    OppositeSexConfig oppositeSex = (OppositeSexConfig)config;
                    this.SpValueX.EditValue = oppositeSex.DispensePositionCompensate.X * 1000.0;

                    this.SpValueY.EditValue = oppositeSex.DispensePositionCompensate.Y * 1000.0;

                    this.SpHeight.EditValue = oppositeSex.DispensePositionCompensate.Z * 1000.0;

                    this.SpValueAngle.EditValue = oppositeSex.DispenseAngleCompensate;
                }
            }
        }
        
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAddOffset_Load(object sender, EventArgs e)
        {
            this.Ck1um.Checked = true;
        }

        /// <summary>
        /// 获取补偿
        /// </summary>
        /// <returns>结果</returns>
        private double GetOffset()
        {
            if (this.Ck1um.Checked)
            {
                return 1;
            }
            else if (this.Ck5um.Checked)
            {
                return 5;
            }
            else if (this.Ck10um.Checked)
            {
                return 10;
            }

            return 0;
        }
        
        /// <summary>
        /// 1um开启
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void Ck1um_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Ck1um.Checked)
            {
                this.Ck5um.Checked = false;
                this.Ck10um.Checked = false;
            }
        }
        
        /// <summary>
        /// 5um开启
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void Ck5um_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Ck5um.Checked)
            {
                this.Ck1um.Checked = false;
                this.Ck10um.Checked = false;
            }
        }

        /// <summary>
        /// 10um开启
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void Ck10um_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Ck10um.Checked)
            {
                this.Ck5um.Checked = false;
                this.Ck1um.Checked = false;
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAdd_Click(object sender, EventArgs e)
        {
            this.SpValueY.Value += (decimal)this.GetOffset();
        }

        /// <summary>
        /// 减少
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDe_Click(object sender, EventArgs e)
        {
            this.SpValueY.Value -= (decimal)this.GetOffset();
        }

        /// <summary>
        /// 向左移动
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLeft_Click(object sender, EventArgs e)
        {
            this.SpValueX.Value -= (decimal)this.GetOffset();
        }

        /// <summary>
        /// 向右移动
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtRight_Click(object sender, EventArgs e)
        {
            this.SpValueX.Value += (decimal)this.GetOffset();
        }

        /// <summary>
        /// 正转
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAngleAdd_Click(object sender, EventArgs e)
        {
            this.SpValueAngle.Value += (decimal)(this.GetOffset() / 10.0);
        }

        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAngleDe_Click(object sender, EventArgs e)
        {
            this.SpValueAngle.Value -= (decimal)(this.GetOffset() / 10.0);
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSure_Click(object sender, EventArgs e)
        {
            if (this.config is SingleBondPositionConfig)
            {
                SingleBondPositionConfig config2 = (SingleBondPositionConfig)this.config;
                
                if (this.isBond)
                {
                    AKRSPoint3D oldOffset = config2.BondPosOffset;
                    config2.RotaryPosition = (double)this.SpValueAngle.Value;
                    config2.BondPosOffset = new AKRSPoint3D(
                        (double)this.SpValueX.Value / 1000.0,
                        (double)this.SpValueY.Value / 1000.0,
                        (double)this.SpHeight.Value / 1000.0);

                    ProductConfiguration.GetInstance().Save();

                    AKRSXtraMessageBox.Show($"保存成功,偏移值由：{oldOffset * 1000.0} 变成 {config2.BondPosOffset * 1000.0}");
                }
                else
                {
                    AKRSPoint3D oldOffset = config2.DispensePosOffset;
                    config2.DispenseRotaryPosition = (double)this.SpValueAngle.Value;
                    config2.DispensePosOffset = new AKRSPoint3D(
                        (double)this.SpValueX.Value / 1000.0,
                        (double)this.SpValueY.Value / 1000.0,
                        (double)this.SpHeight.Value / 1000.0);

                    ProductConfiguration.GetInstance().Save();

                    AKRSXtraMessageBox.Show($"保存成功,偏移值由：{oldOffset * 1000.0} 变成 {config2.DispensePosOffset * 1000.0}");
                }
            }
            else if (this.config is EpoxyApplication)
            {
                EpoxyApplication config2 = (EpoxyApplication)this.config;

                AKRSPoint3D oldOffset = new AKRSPoint3D(config2.OffsetX, config2.OffsetY, config2.OffsetZ);

                config2.OffsetX = (double)this.SpValueX.Value / 1000.0;

                config2.OffsetY = (double)this.SpValueY.Value / 1000.0;

                config2.OffsetZ = (double)this.SpHeight.Value / 1000.0;

                EpoxyApplicationRepository.GetInstance().Save();

                AKRSPoint3D newOffset = new AKRSPoint3D(config2.OffsetX, config2.OffsetY, config2.OffsetZ);

                AKRSXtraMessageBox.Show($"保存成功,偏移值由：{oldOffset * 1000.0} 变成 {newOffset * 1000.0}");
            }
            else if (this.config is OppositeSexConfig)
            {
                OppositeSexConfig oppositeSexConfig = (OppositeSexConfig)this.config;
               
                if (this.isBond)
                {
                    AKRSPoint3D oldOffset = oppositeSexConfig.PositionCompensate;
                    oppositeSexConfig.AngleCompensate = (double)this.SpValueAngle.Value;
                    oppositeSexConfig.PositionCompensate = new AKRSPoint3D(
                        (double)this.SpValueX.Value / 1000.0,
                        (double)this.SpValueY.Value / 1000.0,
                        (double)this.SpHeight.Value / 1000.0);

                    ProductConfiguration.GetInstance().Save();

                    AKRSXtraMessageBox.Show($"保存成功,偏移值由：{oldOffset * 1000.0} 变成 {oppositeSexConfig.PositionCompensate * 1000.0}");
                }
                else
                {
                    AKRSPoint3D oldOffset = oppositeSexConfig.DispensePositionCompensate;
                    oppositeSexConfig.DispenseAngleCompensate = (double)this.SpValueAngle.Value;
                    oppositeSexConfig.DispensePositionCompensate = new AKRSPoint3D(
                        (double)this.SpValueX.Value / 1000.0,
                        (double)this.SpValueY.Value / 1000.0,
                        (double)this.SpHeight.Value / 1000.0);

                    ProductConfiguration.GetInstance().Save();
                    AKRSXtraMessageBox.Show($"保存成功,偏移值由：{oldOffset * 1000.0} 变成 {oppositeSexConfig.DispensePositionCompensate * 1000.0}");
                }
            }
        }

        /// <summary>
        /// 高度增加
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtHeightAdd_Click(object sender, EventArgs e)
        {
            this.SpHeight.Value += (decimal)this.GetOffset();
        }

        /// <summary>
        /// 高度降低
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtHeightDe_Click(object sender, EventArgs e)
        {
            this.SpHeight.Value -= (decimal)this.GetOffset();
        }
    }
}