using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using DevExpress.XtraEditors;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// 添加对象
    /// </summary>
    public partial class FrmAddObject : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 添加对象
        /// </summary>
        public FrmAddObject()
        {
            this.InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 选中的选择对象
        /// </summary>
        public OppositeSex SelectOppositeSexConfig { get; set; }

        /// <summary>
        /// 选择类型
        /// </summary>
        public string Type { get; set; } = "基板";

        /// <summary>
        /// 配置对象
        /// </summary>
        public OppositeSex OppositeSexConfig { get; set; }

        /// <summary>
        /// 选中的sub配置对象
        /// </summary>
        public OppositeSex SubstrateOppositeSexConfig =>
            ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs
                .DownConfigs.Find(it => it.Name == this.CmbSubstrate.SelectedItem?.ToString());

        /// <summary>
        /// 选中的module配置对象
        /// </summary>
        public OppositeSex ModuleOppositeSexConfig =>
            this.SubstrateOppositeSexConfig?.DownConfigs
                .Find(it => it.Name == this.CmbModule.SelectedItem?.ToString());

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSure_Click(object sender, EventArgs e)
        {
            if (OppositeSex.IsExistName(this.TxName.Text))
            {
                AKRSXtraMessageBox.Show("名字以及存在，请更改后重试");
                return;
            }

            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                if (this.TxName.Text.Contains(rInvalidChar.ToString()))
                {
                    AKRSXtraMessageBox.Show($"焊点名存在非法字符: {rInvalidChar.ToString()}！", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            if (this.ChkAbsoluteOffset.Checked)
            {
                this.OppositeSexConfig = new OppositeSex()
                                             {
                                                 Name = this.TxName.Text,
                                                 ConfigName = this.TxName.Text,
                                                 ElementCoordinate = new ElementCoordinate(
                                                     0,
                                                     new AKRSPoint3D(
                                                         (double)this.SpOffsetX.Value - this.SelectOppositeSexConfig.AbsoluteCoordinate.Point.X,
                                                         (double)this.SpOffsetY.Value - this.SelectOppositeSexConfig.AbsoluteCoordinate.Point.Y,
                                                         (double)this.SpOffsetZ.Value - this.SelectOppositeSexConfig.AbsoluteCoordinate.Point.Z))
                                             };
            }
            else
            {
                this.OppositeSexConfig = new OppositeSex()
                                             {
                                                 Name = this.TxName.Text,
                                                 ConfigName = this.TxName.Text,
                                                 ElementCoordinate = new ElementCoordinate(
                                                     0,
                                                     new AKRSPoint3D(
                                                         (double)this.SpOffsetX.Value,
                                                         (double)this.SpOffsetY.Value,
                                                         (double)this.SpOffsetZ.Value))
                                             };
            }

            this.OppositeSexConfig.SizeX = this.SelectOppositeSexConfig.SizeX / 2.0;
            this.OppositeSexConfig.SizeY = this.SelectOppositeSexConfig.SizeY / 2.0;
            OppositeSexConfig oppositeSexConfig = this.OppositeSexConfig.Config;
            if (this.Type == "基板")
            {
                this.OppositeSexConfig.MatterTypeEnum = EntityTypeEnum.Substrate;
                this.OppositeSexConfig.Config.EntityType = EntityTypeEnum.Substrate;
                ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.DownConfigs.Add(this.OppositeSexConfig);
            }
            else if (this.Type == "基岛")
            {
                this.OppositeSexConfig.MatterTypeEnum = EntityTypeEnum.Module;
                this.OppositeSexConfig.Config.EntityType = EntityTypeEnum.Module;
                this.SubstrateOppositeSexConfig.DownConfigs.Add(this.OppositeSexConfig);
            }
            else if (this.Type == "焊点")
            {
                this.OppositeSexConfig.MatterTypeEnum = EntityTypeEnum.BondPosition;
                this.OppositeSexConfig.Config.EntityType = EntityTypeEnum.BondPosition;
                this.ModuleOppositeSexConfig.DownConfigs.Add(this.OppositeSexConfig);
            }

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAddObject_Load(object sender, EventArgs e)
        {
            this.CmbType.Properties.Items.Add("基板");
            this.CmbType.Properties.Items.Add("基岛");
            this.CmbType.Properties.Items.Add("焊点");
            this.CmbType.SelectedItem = this.Type;
            this.CmbSubstrate.Properties.Items.AddRange(
                ProductConfiguration.GetInstance().OppositeSexConfiguration.GetAllSubstrateConfigs()
                    .Select(it => it.Name).ToList());

            this.CmbModule.Properties.Items.AddRange(
                ProductConfiguration.GetInstance().OppositeSexConfiguration.GetAllModuleConfigs()
                    .Select(it => it.Name).ToList());

            if (this.SelectOppositeSexConfig != null)
            {
                if (this.SelectOppositeSexConfig.MatterTypeEnum == EntityTypeEnum.TransportUnit)
                {
                    this.CmbType.SelectedItem = "基板";
                    this.TxName.Text = OppositeSex.GetDefaultName("基板");
                }
                else if (this.SelectOppositeSexConfig.MatterTypeEnum == EntityTypeEnum.Substrate)
                {
                    this.CmbSubstrate.SelectedItem = this.SelectOppositeSexConfig.Name;
                    this.CmbType.SelectedItem = "基岛";
                    this.TxName.Text = OppositeSex.GetDefaultName("基岛");
                }
                else if (this.SelectOppositeSexConfig.MatterTypeEnum == EntityTypeEnum.Module)
                {
                    OppositeSex oppositeSexConfigSub = ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.DownConfigs
                        .Find(it => it.Id == this.SelectOppositeSexConfig.ParentId);
                    this.CmbSubstrate.SelectedItem = oppositeSexConfigSub.Name;
                    this.CmbModule.SelectedItem = this.SelectOppositeSexConfig.Name;
                    this.CmbType.SelectedItem = "焊点";
                    this.TxName.Text = OppositeSex.GetDefaultName("焊点");
                }
            }
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Type = this.CmbType.SelectedItem.ToString();

            if (this.Type == "基板")
            {
                this.CmbSubstrate.Enabled = false;
                this.CmbModule.Enabled = false;
            }
            else if (this.Type == "基岛")
            {
                this.CmbSubstrate.Enabled = true;
                this.CmbModule.Enabled = false;
            }
            else
            {
                this.CmbSubstrate.Enabled = true;
                this.CmbModule.Enabled = true;
            }
        }

        /// <summary>
        /// 绝对偏移值
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkAbsoluteOffset_CheckedChanged(object sender, EventArgs e)
        {
            this.groupControl1.Enabled = this.ChkAbsoluteOffset.Checked;
        }
    }
}