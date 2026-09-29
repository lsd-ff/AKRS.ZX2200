using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using AKRS.ZX2200.Infrastructure.Action;

namespace AKRS.ZX2200.DispenseSystem.Controls.Setting.EpoxyApplication
{
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    using EpoxyApplication = Models.Repositories.Pattern.EpoxyApplication;

    /// <summary>
    /// 新建工具窗体
    /// </summary>
    public partial class FrmNewCreateEpoxyApplication : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Epoxy application
        /// </summary>
        public EpoxyApplication NewEpoxyApplication { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNewCreateEpoxyApplication()
        {
            this.InitializeComponent();

            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            // 控件绑定数据源
            this.LueDispenerType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<DispenserTypeEnum>();
            this.LueDispenerType.EditValue = DispenserTypeEnum.Dispenser;

            //List<EpoxyApplication> epoxyApplicationList = 
            //    EpoxyApplicationRepository.GetInstance().GetDataSourceByCurrentRecipe().ToList();

            List<EpoxyApplication> epoxyApplicationList =
                System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplications;

            epoxyApplicationList.Insert(0, new EpoxyApplication() { Name = "No Pattern" });

            this.GcTool.DataSource = epoxyApplicationList;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TxtName.Text))
            {
                return;
            }

            if (EpoxyApplicationRepository.GetInstance().BaseDsSettingList.Exists(it => it.Name == this.TxtName.Text))
            {
                AKRSXtraMessageBox.Show($"已经存在胶型名为：{this.TxtName.Text} 的图形,创建失败");
                return;
            }

            EpoxyApplication epoxyApplication = this.GvEpoxyApplication.GetFocusedRow() as EpoxyApplication;
            
            if (epoxyApplication == null)
            {
                epoxyApplication = new EpoxyApplication();
            }

            this.NewEpoxyApplication = JsonFormatHelper<EpoxyApplication>.DeepGenericCopy(epoxyApplication);
            this.NewEpoxyApplication.Name = this.TxtName.Text;
            this.NewEpoxyApplication.DispensingType = (DispensingTypeEnum)this.LueDispenerType.EditValue;

            this.NewEpoxyApplication.AddBelongRecipeIds();

            EpoxyApplicationRepository.GetInstance().Add(this.NewEpoxyApplication);

            EpoxyApplicationRepository.GetInstance().Save();

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
    }
}