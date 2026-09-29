using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using System.Threading;

    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;

    /// <summary>
    /// 顶针架配置界面
    /// </summary>
    public partial class FrmEjectionSetting : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmEjectionSetting()
        {
            this.InitializeComponent();
            this.Init();
        }

        /// <summary>
        /// 顶针池(当前配方的顶针集合）
        /// </summary>
        private List<EjectionConfig> ejectionSettingPool = new List<EjectionConfig>();

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void Init()
        {
            if (!EjectionBankConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == WaferSystemProgram.GetInstance().EjectionBankProgram.Name))
            {
                WaferSystemProgram.GetInstance().EjectionBankProgram.Name = string.Empty;
                WaferSystemProgram.GetInstance().Save();
                Thread.Sleep(100);
            }

            this.CmbEjectionBankRepository.EditValue = WaferSystemProgram.GetInstance().EjectionBankProgram.Name;
            this.RefreshCmbItems();
            this.RefreshGcEjectionBank();
            this.RefreshGcEjectionPool();
        }

        /// <summary>
        /// 刷新当前顶针架
        /// </summary>
        private void RefreshGcEjectionBank()
        {
            if (string.IsNullOrEmpty(this.CmbEjectionBankRepository.Text))
            {
                return;
            }

            BaseDsSetting dsSetting = EjectionBankConfigRepository.GetInstance().Find(this.CmbEjectionBankRepository.Text); 

            if (dsSetting != null)
            {
                EjectionBankConfig ejectionBankConfig = dsSetting as EjectionBankConfig;
                this.GcEjectionBank.DataSource = ejectionBankConfig.EjectionBankSlots;
            }

            this.GcEjectionBank.RefreshDataSource();

            this.RefreshGcEjectionPool();
        }

        /// <summary>
        /// 刷新当前顶针池
        /// </summary>
        private void RefreshGcEjectionPool()
        {
            if (string.IsNullOrWhiteSpace(this.CmbEjectionBankRepository.Text))
            {
                return;
            }

            // 获取当前配方的顶针集合
            this.ejectionSettingPool = EjectionConfigRepository.GetInstance().GetDataSourceByCurrentRecipe();

            EjectionBankConfig ejectionBank = EjectionBankConfigRepository.GetInstance()
                .Find(this.CmbEjectionBankRepository.Text) as EjectionBankConfig;

            if (ejectionBank == null)
            {
                return;
            }

            EjectionBankSlotConfig[] ejectionBankSlots = ejectionBank.EjectionBankSlots;

            if (ejectionBankSlots != null)
            {
                this.ejectionSettingPool.RemoveAll(a => ejectionBankSlots.Exists(b => b.Name == a.Name));
            }


            this.GcEjectionPool.DataSource = this.ejectionSettingPool;

            this.GcEjectionPool.RefreshDataSource();
        }

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            this.CmbEjectionBankRepository.Properties.Items.Clear();
            List<string> nameList = EjectionBankConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().Select( a => a.Name).ToList();
            this.CmbEjectionBankRepository.Properties.Items.AddRange(nameList);
        }

        /// <summary>
        /// 顶针架下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbEjectionBankRepository_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<EjectionBankConfig> frmRepository = new FrmRepository<EjectionBankConfig>(EjectionBankConfigRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    EjectionBankConfig temp = (EjectionBankConfig)frmRepository.DsSetting;

                    if (temp != null)
                    {
                        this.CmbEjectionBankRepository.Text = temp.Name;
                    }
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 顶针管理按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEjectionManage_Click(object sender, EventArgs e)
        {
            FrmRepository<EjectionConfig> frmRepository = new FrmRepository<EjectionConfig>(EjectionConfigRepository.GetInstance());

            frmRepository.ShowDialog();
            this.RefreshGcEjectionPool();
            this.RefreshCurrentEjectBank();
            this.RefreshGcEjectionBank();
        }

        /// <summary>
        /// 改变所选顶针架
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbEjectionBankRepository_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshCurrentEjectBank();
            this.RefreshGcEjectionBank();
        }

        /// <summary>
        /// 修改CapDiameter
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpCapDiameter_EditValueChanged(object sender, EventArgs e)
        {
            EjectionConfig ejectionConfig = (EjectionConfig)this.GvEjectionPool.GetFocusedRow();
            if (ejectionConfig == null)
            {
                return;
            }

            ejectionConfig.CapDiameter = (double)this.SpCapDiameter.Value;
            EjectionConfigRepository.GetInstance().Save();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void GvEjectionPool_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            EjectionConfig ej = (EjectionConfig)this.GvEjectionPool.GetFocusedRow();
            if (ej != null)
            {
                this.SpCapDiameter.EditValue = ej.CapDiameter;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            WaferSystemDomain.GetInstance().WaferSystemProgram.EjectionBankProgram.Name = this.CmbEjectionBankRepository.Text;
            EjectionBankConfigRepository.GetInstance().Save();
            EjectionConfigRepository.GetInstance().Save();
            WaferSystemProgram.GetInstance().EjectionBankProgram.CreateEjectionBankEntity();
            WaferSystemProgram.GetInstance().Save();
        }

        /// <summary>
        /// 顶针向左填充到顶针槽
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGoLeft_Click(object sender, EventArgs e)
        {
            // 顶针架当前选中的槽
            EjectionBankSlotConfig ejectionBankSlotConfig = (EjectionBankSlotConfig)this.GvEjectionBank.GetFocusedRow();

            EjectionConfig ejectionConfig = (EjectionConfig)this.GvEjectionPool.GetFocusedRow();

            if (ejectionBankSlotConfig == null || ejectionConfig == null || ejectionBankSlotConfig.Name != string.Empty) 
            {
                return;
            }

            ejectionBankSlotConfig.Name = ejectionConfig.Name;
            //ejectionBankSlotConfig.SlotState = EjectSlotStatuEnum.UnUsing;

            this.ejectionSettingPool.Remove(ejectionConfig);

            this.RefreshGcEjectionPool();
            this.RefreshGcEjectionBank();
        }

        /// <summary>
        /// 顶针向右移出按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGoRight_Click(object sender, EventArgs e)
        {
            // 顶针架当前选中的槽
            EjectionBankSlotConfig ejectionBankSlotConfig = (EjectionBankSlotConfig)this.GvEjectionBank.GetFocusedRow();

            if (ejectionBankSlotConfig == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(ejectionBankSlotConfig.Name)) 
            {
                return;
            }

            if (ejectionBankSlotConfig.EjectionConfig != null)
            {
                this.ejectionSettingPool.Add(ejectionBankSlotConfig.EjectionConfig);
                ejectionBankSlotConfig.Name = string.Empty;
                //ejectionBankSlotConfig.SlotState = EjectSlotStatuEnum.Empty;
            }

            this.RefreshGcEjectionPool();
            this.RefreshGcEjectionBank();
        }

        /// <summary>
        /// 新建顶针按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            FrmNewCreateEjection frmNewCreateEjection = new FrmNewCreateEjection();

            if (frmNewCreateEjection.ShowDialog() == DialogResult.OK)
            {
                // 顶针池刷新
                this.RefreshGcEjectionPool();
            }

            frmNewCreateEjection.Dispose();
        }

        /// <summary>
        /// 删除顶针按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            EjectionConfig ejection = (EjectionConfig)this.GvEjectionPool.GetFocusedRow();

            if (ejection == null)
            {
                return;
            }

            // 移除顶针
            ejection.RemoveFromBelongRecipeIds();
            EjectionBankConfigRepository.GetInstance().Save();

            // 顶针池刷新
            this.RefreshGcEjectionPool();
        }

        /// <summary>
        /// OK
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtOk_Click(object sender, EventArgs e)
        {
            this.Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            EjectionConfig ej = (EjectionConfig)this.GvEjectionPool.GetFocusedRow();
            if (ej != null)
            {
                this.GcCapDiameter.Enabled = true;
            }
            else
            {
                this.GcCapDiameter.Enabled = false;
            }
        }

        /// <summary>
        /// 刷新顶针架，防呆
        /// </summary>
        private void RefreshCurrentEjectBank()
        {
            EjectionBankConfig bank = EjectionBankConfigRepository.GetInstance()
                .GetDataSourceByCurrentRecipe().Find(item => item.Name == this.CmbEjectionBankRepository.Text);

            if (bank == null) 
            {
                return;
            }

            foreach (var slot in bank.EjectionBankSlots)
            {
                if (!string.IsNullOrEmpty(slot.Name))
                {
                    if (!EjectionConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().Exists(item => item.Name == slot.Name))
                    {
                        slot.Name = string.Empty;
                        //slot.SlotState = EjectSlotStatuEnum.Empty;
                        EjectionBankConfigRepository.GetInstance().Save();
                    }
                }
            }
        }

        private void FrmEjectionSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }
        }
    }
}
