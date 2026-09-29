using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.DispenseSystem.Models.Enums;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.FlipTool
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;

    /// <summary>
    ///  新建翻转工具
    /// </summary>
    public partial class FrmNewCreateFlipTool : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNewCreateFlipTool()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            List<FlipTool> flipToolList =
                FlipToolRepository.GetInstance().BaseDsSettingList;

            this.CmbFlipTool.Properties.Items.Clear();
            List<string> nameList = FlipToolRepository.GetInstance().Filter(string.Empty, false).Select(a => a.Name).ToList();

            // 控件绑定数据源
            this.CmbFlipTool.Properties.Items.AddRange(nameList);

            this.CmbFlipTool.Text = WaferSystemProgram.GetInstance().FlipModuleProgram.FlipToolName;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.CmbFlipTool.Text))
            {
                return;
            }

            WaferSystemProgram.GetInstance().FlipModuleProgram.FlipToolName = this.CmbFlipTool.Text;

            FlipToolRepository.GetInstance().Save();
            WaferSystemProgram.GetInstance().Save();

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
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbFlipTool_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<FlipTool> frmRepository = new FrmRepository<FlipTool>(FlipToolRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    FlipTool temp = (FlipTool)frmRepository.DsSetting;

                    if (temp != null)
                    {
                        ((ComboBoxEdit)sender).Text = temp.Name;
                    }
                }

                this.InitControl();
            }
        }
    }
}