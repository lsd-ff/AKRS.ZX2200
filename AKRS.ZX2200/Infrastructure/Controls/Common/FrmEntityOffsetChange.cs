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
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 偏移设置
    /// </summary>
    public partial class FrmEntityOffsetChange : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 焊点偏移
        /// </summary>
        private readonly UcAddOffset frmAddOffsetBp = new UcAddOffset();

        /// <summary>
        /// 点胶偏移
        /// </summary>
        private readonly UcAddOffset frmAddOffsetDs = new UcAddOffset();

        /// <summary>
        /// 焊后偏移
        /// </summary>
        private readonly UcDefect frmAddOffsetPb = new UcDefect();

        /// <summary>
        /// 点胶偏移
        /// </summary>
        private readonly UcAddOffset frmAddOffsetBpDs = new UcAddOffset();

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmEntityOffsetChange()
        {
            this.InitializeComponent();

            this.frmAddOffsetBp.Dock = DockStyle.Fill;
            this.panelControl4.Controls.Add(this.frmAddOffsetBp);

            this.frmAddOffsetDs.Dock = DockStyle.Fill;
            this.panelControl3.Controls.Add(this.frmAddOffsetDs);

            this.frmAddOffsetPb.Dock = DockStyle.Fill;
            this.panelControl5.Controls.Add(this.frmAddOffsetPb);

            this.frmAddOffsetBpDs.Dock = DockStyle.Fill;
            this.panelControl7.Controls.Add(this.frmAddOffsetBpDs);

            this.xtraTabControl1.SelectedTabPageIndex = 1;
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmEntityOffsetChange_Load(object sender, EventArgs e)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                foreach (OppositeSexConfig oppositeSexConfig in ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.BaseConfigs)
                {
                    if (oppositeSexConfig.EntityType == EntityTypeEnum.BondPosition)
                    {
                        this.CmbBpName.Properties.Items.Add(oppositeSexConfig.Name);
                        this.CmbDispenseBondName.Properties.Items.Add(oppositeSexConfig.Name);
                    }
                }
            }
            else
            {
                foreach (SingleBondPositionConfig baseBondPositionConfig in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
                {
                    this.CmbBpName.Properties.Items.Add(baseBondPositionConfig.Name);
                    this.CmbDispenseBondName.Properties.Items.Add(baseBondPositionConfig.Name);
                }
            }
            
            foreach (EpoxyApplication epoxyApplication in System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplications)
            {
                if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
                {
                    this.CmbEpName.Properties.Items.Add(epoxyApplication.Name);
                }
            }

            foreach (EpoxyApplication epoxyApplication in System2Domain.GetInstance().BondProgram.EpoxyApplicationProgram.EpoxyApplications)
            {
                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
                {
                    this.CmbEpName.Properties.Items.Add(epoxyApplication.Name);
                }
            }

            foreach (PostBondInspection postBondInspection in BondProgram.GetInstance().PostBondProgram.PostBondInspections)
            {
                this.CmbDefectName.Properties.Items.Add(postBondInspection.Name);
            }

            if (this.CmbBpName.Properties.Items.Count != 0)
            {
                this.CmbBpName.SelectedIndex = 0;
            }

            if (this.CmbEpName.Properties.Items.Count != 0)
            {
                this.CmbEpName.SelectedIndex = 0;
            }

            if (this.CmbDefectName.Properties.Items.Count != 0)
            {
                this.CmbDefectName.SelectedIndex = 0;
            }

            if (this.CmbDispenseBondName.Properties.Items.Count != 0)
            {
                this.CmbDispenseBondName.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 焊点名称改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbBpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                OppositeSexConfig oppositeSexConfig = ProductConfiguration.GetInstance().OppositeSexConfiguration
                    .BaseConfigs.Find(it => it.Name == this.CmbBpName.SelectedItem.ToString());
                if (oppositeSexConfig != null)
                {
                    this.frmAddOffsetBp.SetOffset(oppositeSexConfig);
                }
            }
            else
            {
                SingleBondPositionConfig singleBondPositionConfig = ProductConfiguration.GetInstance().BondPositionConfig
                    .SingleBpPositionConfigList.Find(it => it.Name == this.CmbBpName.SelectedItem.ToString());

                if (singleBondPositionConfig != null)
                {
                    this.frmAddOffsetBp.SetOffset(singleBondPositionConfig);
                }
            }
        }

        /// <summary>
        /// 胶型名称改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbEpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                EpoxyApplication epoxyApplication = BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplications
                   .Find(it => it.Name == this.CmbEpName.SelectedItem.ToString());

                if (epoxyApplication != null)
                {
                    this.frmAddOffsetDs.SetOffset(epoxyApplication);
                }
            }
            else
            {
                EpoxyApplication epoxyApplication = System1Domain.GetInstance().System1Program.EpoxyApplicationProgram
                   .EpoxyApplications.Find(it => it.Name == this.CmbEpName.SelectedItem.ToString());

                if (epoxyApplication != null)
                {
                    this.frmAddOffsetDs.SetOffset(epoxyApplication);
                }
            }
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbDefectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            PostBondInspection postBondInspection = BondProgram.GetInstance().PostBondProgram
                .PostBondInspections.Find(it => it.Name == this.CmbDefectName.SelectedItem.ToString());

            if (postBondInspection != null)
            {
                this.frmAddOffsetPb.PostBondEdit(postBondInspection);
            }
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {

        }

        /// <summary>
        /// 焊点名称改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbDispenseBondName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                OppositeSexConfig oppositeSexConfig = ProductConfiguration.GetInstance().OppositeSexConfiguration
                    .BaseConfigs.Find(it => it.Name == this.CmbDispenseBondName.SelectedItem.ToString());
                if (oppositeSexConfig != null)
                {
                    this.frmAddOffsetBpDs.SetOffset(oppositeSexConfig, false);
                }
            }
            else
            {
                SingleBondPositionConfig singleBondPositionConfig = ProductConfiguration.GetInstance().BondPositionConfig
                    .SingleBpPositionConfigList.Find(it => it.Name == this.CmbDispenseBondName.SelectedItem.ToString());

                if (singleBondPositionConfig != null)
                {
                    this.frmAddOffsetBpDs.SetOffset(singleBondPositionConfig, false);
                }
            }
        }
    }
}