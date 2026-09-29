using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.MagazineTeach
{
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;

    /// <summary>
    /// 新建工具窗体
    /// </summary>
    public partial class FrmNewCreateAdapter : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 吸嘴
        /// </summary>
        public AdapterConfig NewAdapter { get; set; }

        /// <summary>
        /// 是否是静态华夫盘
        /// </summary>
        private bool isStaticAdapter;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isStaticAdapter">是否是静态华夫盘</param>
        public FrmNewCreateAdapter(bool isStaticAdapter = false)
        {
            InitializeComponent();
            this.isStaticAdapter = isStaticAdapter;
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            // 控件绑定数据源
            LueCarrierType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdapterTypeEnum>();
            if (this.isStaticAdapter)
            {
                this.LueCarrierType.EditValue = AdapterTypeEnum.Static;
            }
            else
            {
                this.LueCarrierType.EditValue = AdapterTypeEnum.WaferTable;
            }
            
            List<AdapterConfig> carrierList = AdapterConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().ToList();

            carrierList.Insert(0, new AdapterConfig() { Name = "No Pattern" });

            this.GcAdapter.DataSource = carrierList;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            AdapterConfig adapter = this.GvAdapter.GetFocusedRow() as AdapterConfig;

            if (adapter == null || adapter.Name == "No Pattern")
            {
                if ((AdapterTypeEnum)this.LueCarrierType.EditValue == AdapterTypeEnum.WaferTable)
                {
                    adapter = new AdapterConfig() { AdapterType = AdapterTypeEnum.WaferTable };
                }
                else if ((AdapterTypeEnum)this.LueCarrierType.EditValue == AdapterTypeEnum.Static)
                {
                    adapter = new AdapterConfig() { AdapterType = AdapterTypeEnum.Static };
                }
            }

            NewAdapter =
                JsonFormatHelper<AdapterConfig>.DeepGenericCopy<AdapterConfig>(adapter);

            NewAdapter.Name = this.TxtName.Text;
            NewAdapter.AddBelongRecipeIds();

            AdapterConfigRepository.GetInstance().Add(NewAdapter);
            AdapterConfigRepository.GetInstance().Save();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.BtnGenerate.Enabled = !string.IsNullOrWhiteSpace(this.TxtName.Text) && !AdapterConfigRepository
                                           .GetInstance().BaseDsSettingList.Exists(a => a.Name == this.TxtName.Text);
        }

        private void FrmNewCreateAdapter_FormClosing(object sender, FormClosingEventArgs e)
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