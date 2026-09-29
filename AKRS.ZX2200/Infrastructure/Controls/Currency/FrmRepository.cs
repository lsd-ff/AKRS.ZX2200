namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CustomControls.Controls;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Models;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 通用数据集 UI
    /// </summary>
    /// <typeparam name="T">数据集里的对象泛型</typeparam>
    public partial class FrmRepository<T> : XtraForm where T : BaseDsSetting, new()
    {
        /// <summary>
        /// 数据设置基类
        /// </summary>
        public BaseDsSetting DsSetting { get; set; }

        /// <summary>
        /// 数据集仓库
        /// </summary>
        private readonly BaseSettingRepository<T> baseSettingRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseSettingRepository">数据集仓库</param>
        /// <param name="action">委托</param>
        public FrmRepository(BaseSettingRepository<T> baseSettingRepository, Action action)
        {
            this.InitializeComponent();
            this.baseSettingRepository = baseSettingRepository;
            this.action = action;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseSettingRepository">数据集仓库</param>
        /// <param name="action">委托</param>
        /// <param name="deleteAction">删除委托</param>
        public FrmRepository(BaseSettingRepository<T> baseSettingRepository, Action action, Action<string> deleteAction)
        {
            this.InitializeComponent();
            this.baseSettingRepository = baseSettingRepository;
            this.action = action;
            this.actionDelete = deleteAction;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseSettingRepository">数据集仓库</param>
        public FrmRepository(BaseSettingRepository<T> baseSettingRepository)
        {
            this.InitializeComponent();
            this.baseSettingRepository = baseSettingRepository;
        }

        /// <summary>
        /// 委托
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// 委托
        /// </summary>
        private readonly Action<string> actionDelete;

        /// <summary>
        /// Load事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmRepository_Load(object sender, EventArgs e)
        {
            this.RefreshData();
            this.GvRepository.ClearSelection();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            string name = this.TxtName.Text;
            bool isIncludeAll = this.ChkIncludeAll.Checked;
            List<T> tList = this.baseSettingRepository.Filter(name, isIncludeAll);

            tList.Insert(0, new T() { Name = "No Reference" });

            this.BsRepository.DataSource = tList;

            // 设置时间显示格式
            this.GvRepository.Columns[1].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";

            // 按时间降序排列
            this.GvRepository.Columns[1].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtCreateNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmCreateName frmCreateName = new FrmCreateName(string.Empty, "Create", string.Empty);

            bool ValidatePredicate(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return false;
                }

                if (this.baseSettingRepository.BaseDsSettingList.Exists(a => a.Name == name))
                {
                    return false;
                }

                foreach (char rInvalidChar in Path.GetInvalidFileNameChars())
                {
                    if (name.Contains(rInvalidChar.ToString()))
                    {
                        return false;
                    }
                }

                return true;
            }

            frmCreateName.ValidatePredicate = ValidatePredicate;

            DialogResult dr = frmCreateName.ShowDialog();

            if (dr == DialogResult.OK)
            {
                T t = this.GvRepository.GetFocusedRow() as T;
                T newT;

                if (t == null || t.Name == "No Reference") 
                {
                    newT = new T();
                }
                else
                {
                    // 深拷贝
                    newT = JsonFormatHelper<T>.DeepGenericCopy<T>(t);
                    newT.CopyName = t.Name;
                }

                newT.Name = frmCreateName.CreateName;
                newT.CreateTime = DateTime.Now;

                // 添加到当前Recipe
                newT.AddBelongRecipeIds();

                this.baseSettingRepository.Add(newT);

                this.RefreshData();
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否永久删除?",
                "提示",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.No)
            {
                return;
            }

            T t = this.GvRepository.GetFocusedRow() as T;

            if (t.IsSystemConfiguration)
            {
                AKRSXtraMessageBox.Show(
                    $"系统默认工具不能删除",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            this.baseSettingRepository.Remove(t);

            if(this.actionDelete != null)
            {
                this.actionDelete(t.Name);
            }

            this.RefreshData();

            AKRSXtraMessageBox.Show($"删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.baseSettingRepository.Save();

            AKRSXtraMessageBox.Show($"Saved  Successfully!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 包含所有
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkIncludeAll_CheckedChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        /// <summary>
        /// 过滤名称
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TxtName_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            this.RefreshData();
        }

        /// <summary>
        /// 行双击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void GvRepository_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                if (this.GvRepository.GetFocusedRow() is T t)
                {
                    this.DsSetting = t;
                    t.AddBelongRecipeIds();
                    this.action?.Invoke();
                }

                // this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Extract dataset to recipe
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtExtract_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (this.GvRepository.GetFocusedRow() is T t)
            {
                t.AddBelongRecipeIds();
                this.DsSetting = t;
                this.action?.Invoke();
            }

            AKRSXtraMessageBox.Show(
                $"Extracted  Successfully!",
                "Prompt",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// 手动关闭时也应该保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmRepository_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.GvRepository.CloseEditor();
            this.GvRepository.UpdateCurrentRow();

            this.baseSettingRepository.Save();
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            string focusedDisplayText = this.GvRepository.GetFocusedDisplayText();
            this.BarBtDelete.Enabled = !(focusedDisplayText == null || focusedDisplayText == "No Reference");
            this.BarBtExtract.Enabled = !(focusedDisplayText == null || focusedDisplayText == "No Reference");
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmRepository_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Timer1.Tick -= Timer1_Tick;
            this.Timer1.Dispose();
        }
    }
}