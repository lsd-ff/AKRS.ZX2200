using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Setting.PostBond
{
    /// <summary>
    /// 增删焊后检测界面
    /// </summary>
    public partial class UcAddPostBond : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refreshAction">刷新父节点委托</param>
        public UcAddPostBond(Action refreshAction)
        {
            InitializeComponent();
            this.refreshparentTreeAction = refreshAction;
            this.RefreshControl();
        }

        /// <summary>
        /// 当前载具
        /// </summary>
        private TransportUnit TransportUnit => TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

        /// <summary>
        /// 当前程式焊后检测集
        /// </summary>
        private List<PostBondInspection> postBondInspectionList => BondProgram.GetInstance().PostBondProgram.PostBondInspections;

        /// <summary>
        /// 刷新父节点委托
        /// </summary>
        private Action refreshparentTreeAction;

        /// <summary>
        /// 刷新控件
        /// </summary>
        private void RefreshControl()
        {
            this.GcPostBond.DataSource = this.postBondInspectionList;
            this.GcPostBond.Refresh();
            this.GvPostBond.RefreshData();

            this.refreshparentTreeAction();
        }

        /// <summary>
        /// 删除PR
        /// </summary>
        /// <param name="prName">名称</param>
        private void DeletePostBondPr(string prName)
        {
            List<string> prNames = new List<string>();

            prNames.Add(prName + "1");

            prNames.Add(prName + "2");

            prNames.Add(prName + "Refer" + 1);

            prNames.Add(prName + "Refer" + 2);

            // 删除PRC文件
            foreach (string prName2 in prNames)
            {
                PREntity.DeletePREntity(prName2);

                // 删除PR
                VisionEntityRepository.GetInstance().PRVisionList.RemoveAll(item => item.GetName() == prName2);
            }
            
            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 新增焊后检测
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            FrmNewCreatePostBond frmNewCreatePostBond = new FrmNewCreatePostBond();

            if (frmNewCreatePostBond.ShowDialog() == DialogResult.OK)
            {
                // 控件刷新
                this.RefreshControl();
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // 获取当前选中的焊点配置
            PostBondInspection postBondInspection = (PostBondInspection)this.GvPostBond.GetFocusedRow();

            // 检查是否为空
            if (postBondInspection == null)
            {
                AKRSXtraMessageBox.Show($"请先选择焊后！", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult res = AKRSXtraMessageBox.Show($"是否确定移除此焊后检测：{postBondInspection.Name}?", "Question", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (res == DialogResult.OK)
            {
                BondProgram.GetInstance().PostBondProgram.PostBondInspectionLists.Remove(postBondInspection.Name);

                // 保存数据
                PostBondInspectionRepository.GetInstance().Save();
                BondProgram.GetInstance().Save();
            }

            this.RefreshControl();
        }

        /// <summary>
        ///  管理
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnManage_Click(object sender, EventArgs e)
        {
            FrmRepository<PostBondInspection> frmRepository = new FrmRepository<PostBondInspection>(PostBondInspectionRepository.GetInstance(),this.RefreshControl, this.DeletePostBondPr);

            // 获取显示器屏幕宽度,高度
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;
            frmRepository.Location = new Point(xWidth - 700, 250);
            frmRepository.StartPosition = FormStartPosition.Manual;
            frmRepository.ShowDialog();

            List<PostBondInspection> list = PostBondInspectionRepository.GetInstance().GetDataSourceByCurrentRecipe();

            foreach (PostBondInspection postBondInspection in list)
            {
                if (!BondProgram.GetInstance().PostBondProgram.PostBondInspectionLists.Contains(postBondInspection.Name))
                {
                    BondProgram.GetInstance().PostBondProgram.PostBondInspectionLists.Add(postBondInspection.Name);
                }
            }

            BondProgram.GetInstance().Save();

            this.RefreshControl();
        }

        /// <summary>
        ///  刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshControl();
        }
    }
}
