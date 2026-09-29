using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;

    /// <summary>
    /// Tranport System 设置
    /// </summary>
    public partial class UcTransportSystem : XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 基板流道程式
        /// </summary>
        private TransportProgram transportProgram => TransportProgram.GetInstance() ?? new TransportProgram();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcTransportSystem()
        {
            InitializeComponent();
            this.RefreshCmbItems();
            this.InitControl();
        }

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            ImgCmbInputSystem.Properties.Items.Clear();
            ImgCmbOutputSystem.Properties.Items.Clear();
            ImageComboBoxItem[] list = InOutPutBeltSettingRepository.GetInstance()
                .Filter(string.Empty, false)
                .Select(a => new ImageComboBoxItem(a.Name, a.Name, (int)a.EditState)).ToArray();
            ImgCmbInputSystem.Properties.Items.AddRange(list);
            ImgCmbOutputSystem.Properties.Items.AddRange(list);

            ImgCmbTransportbelt1.Properties.Items.Clear();
            ImgCmbTransportbelt2.Properties.Items.Clear();
            ImgCmbTransportbelt3.Properties.Items.Clear();
            list = TransportBeltSettingRepository.GetInstance()
                .Filter(string.Empty, false)
                .Select(a => new ImageComboBoxItem(a.Name, a.Name, (int)a.EditState)).ToArray();
            ImgCmbTransportbelt1.Properties.Items.AddRange(list);
            ImgCmbTransportbelt2.Properties.Items.AddRange(list);
            ImgCmbTransportbelt3.Properties.Items.AddRange(list);

            ImgCmbBondinsert1.Properties.Items.Clear();
            ImgCmbBondinsert2.Properties.Items.Clear();
            list = BondinsertSettingRepository.GetInstance()
                .Filter(string.Empty, false)
                .Select(a => new ImageComboBoxItem(a.Name, a.Name, (int)a.EditState)).ToArray();
            ImgCmbBondinsert1.Properties.Items.AddRange(list);
            ImgCmbBondinsert2.Properties.Items.AddRange(list);

            ImgCmbOutputSystem.Refresh();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            ImgCmbInputSystem.EditValue = this.transportProgram.LoadingSubSectionProgram.TransportBeltSettingName;
            ImgCmbOutputSystem.EditValue = this.transportProgram.UnloadingSubSectionProgram.TransportBeltSettingName;

            ImgCmbTransportbelt1.EditValue = this.transportProgram.DispenseSubSectionProgram.TransportBeltSettingName;
            ImgCmbTransportbelt2.EditValue = this.transportProgram.BondSubSectionProgram.TransportBeltSettingName;
            ImgCmbTransportbelt3.EditValue = this.transportProgram.WaitingUnloadSubSectionProgram.TransportBeltSettingName;

            ImgCmbBondinsert1.EditValue = this.transportProgram.DispenseSubSectionProgram.BondinsertSettingName;
            ImgCmbBondinsert2.EditValue = this.transportProgram.BondSubSectionProgram.BondinsertSettingName;
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbInputSystem_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<InOutPutBeltSetting> frmRepository = new FrmRepository<InOutPutBeltSetting>(InOutPutBeltSettingRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    InOutPutBeltSetting inOutPutBeltSetting = (InOutPutBeltSetting)frmRepository.DsSetting;

                    if (inOutPutBeltSetting != null)
                    {
                        ((ComboBoxEdit)sender).Text = inOutPutBeltSetting.Name;
                    }
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbTransportbelt_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<TransportBeltSetting> frmRepository = new FrmRepository<TransportBeltSetting>(TransportBeltSettingRepository.GetInstance());
                
                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    TransportBeltSetting transportBeltSetting = (TransportBeltSetting)frmRepository.DsSetting;

                    if (transportBeltSetting != null)
                    {
                        ((ComboBoxEdit)sender).Text = transportBeltSetting.Name;
                    }
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbBondinsert_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<BondinsertSetting> frmRepository = new FrmRepository<BondinsertSetting>(BondinsertSettingRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    BondinsertSetting bondinsertSetting = (BondinsertSetting)frmRepository.DsSetting;

                    if (bondinsertSetting != null)
                    {
                        ((ComboBoxEdit)sender).Text = bondinsertSetting.Name;
                    }
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            this.transportProgram.LoadingSubSectionProgram.TransportBeltSettingName = ImgCmbInputSystem.EditValue.ToString();
            this.transportProgram.UnloadingSubSectionProgram.TransportBeltSettingName = ImgCmbOutputSystem.EditValue.ToString();

            this.transportProgram.DispenseSubSectionProgram.TransportBeltSettingName = ImgCmbTransportbelt1.EditValue.ToString();
            this.transportProgram.BondSubSectionProgram.TransportBeltSettingName = ImgCmbTransportbelt2.EditValue.ToString();
            this.transportProgram.WaitingUnloadSubSectionProgram.TransportBeltSettingName = ImgCmbTransportbelt3.EditValue.ToString();

            this.transportProgram.DispenseSubSectionProgram.BondinsertSettingName = ImgCmbBondinsert1.EditValue.ToString();
            this.transportProgram.BondSubSectionProgram.BondinsertSettingName = ImgCmbBondinsert2.EditValue.ToString();

            this.transportProgram.Save();
        }

        /// <summary>
        /// 使用相同数据集
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkUseInOutputSameDatasets_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkUseInOutputSameDatasets.Checked)
            {
                this.ImgCmbOutputSystem.ReadOnly = true;
                this.ImgCmbOutputSystem.EditValue = this.ImgCmbInputSystem.EditValue;
            }
            else
            {
                this.ImgCmbOutputSystem.ReadOnly = false;
            }
        }

        /// <summary>
        /// 使用相同数据集
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkUseTransportBeltSameDatasets_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkUseTransportBeltSameDatasets.Checked)
            {
                this.ImgCmbTransportbelt2.ReadOnly = true;
                this.ImgCmbTransportbelt3.ReadOnly = true;
                this.ImgCmbTransportbelt2.EditValue = this.ImgCmbTransportbelt1.EditValue;
                this.ImgCmbTransportbelt3.EditValue = this.ImgCmbTransportbelt1.EditValue;
            }
            else
            {
                this.ImgCmbTransportbelt2.ReadOnly = false;
                this.ImgCmbTransportbelt3.ReadOnly = false;
            }
        }
    }
}
