using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.Models.Enums;

using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Setting.PostBond
{
    using System.IO;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 新建焊后检测窗体
    /// </summary>
    public partial class FrmNewCreatePostBond : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNewCreatePostBond()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 焊后检测
        /// </summary>
        public PostBondInspection NewPostBondInspection { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            // 控件绑定数据源
            List<PostBondInspection> dataList = PostBondInspectionRepository.GetInstance().GetDataSourceByCurrentRecipe().ToList();
            List<PostBondInspection> repositoryList = PostBondInspectionRepository.GetInstance().BaseDsSettingList;

            dataList.Insert(0, new PostBondInspection("No Reference"));

            this.GcDataset.DataSource = dataList;
            this.GcRepository.DataSource = repositoryList;
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

            // 获取焊点名集合
            List<string> nameList = new List<string>();

            foreach (var item in PostBondInspectionRepository.GetInstance().BaseDsSettingList)
            {
                nameList.Add(item.Name);
            }

            // 检查名称是否冲突
            if (nameList.Contains(this.TxtName.Text))
            {
                DialogResult ret = AKRSXtraMessageBox.Show(
                    "Name conflict! Please  rename  post-bond  inspection!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
            {
                if (this.TxtName.Text.Contains(rInvalidChar.ToString()))
                {
                    AKRSXtraMessageBox.Show($"焊点名存在非法字符: {rInvalidChar.ToString()}！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }
            }

            PostBondInspection postBondInspection = new PostBondInspection(this.TxtName.Text);

            PostBondInspectionRepository.GetInstance().Add(postBondInspection);

            BondProgram.GetInstance().PostBondProgram.PostBondInspectionLists.Add(postBondInspection.Name);

            this.NewPostBondInspection = postBondInspection;

            PostBondInspectionRepository.GetInstance().Save();
            BondProgram.GetInstance().Save();

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
        /// 输入文本改变事件(防呆)
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TxtName_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.TxtName.Text))
            {
                BtnGenerate.Enabled = false;
                return;
            }

            if (this.TxtName.Text.Length > 15)
            {
                BtnGenerate.Enabled = false;
                return;
            }

            List<PostBondInspection> pbList = PostBondInspectionRepository.GetInstance().BaseDsSettingList;

            foreach (PostBondInspection item in pbList)
            {
                if (this.TxtName.Text == item.Name)
                {
                    BtnGenerate.Enabled = false;
                    return;
                }
            }

            BtnGenerate.Enabled = true;
        }
    }
}