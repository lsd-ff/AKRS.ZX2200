using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Programs;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
using DevExpress.DataAccess.Native.EntityFramework;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.Waferhandling
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;

    /// <summary>
    /// wafer handing 数据集设置
    /// </summary>
    public partial class UcWaferHanding : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// TreeList
        /// </summary>
        private TreeList treeListProgramming;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="treeListProgramming">tree list</param>
        public UcWaferHanding(TreeList treeListProgramming)
        {
            this.InitializeComponent();
            this.treeListProgramming = treeListProgramming;
            this.RefreshCmbItems();

            this.CmbWafer.EditValue = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.Name;
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbWafer_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<MagazineBoxConfig> frmRepository = new FrmRepository<MagazineBoxConfig>(MagazineBoxConfigRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    MagazineBoxConfig magazineBoxConfig = (MagazineBoxConfig)frmRepository.DsSetting;

                    if (magazineBoxConfig != null)
                    {
                        ((ComboBoxEdit)sender).Text = magazineBoxConfig.Name;
                    }
                }

                if (!MagazineBoxConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == ((ComboBoxEdit)sender).Text))
                {
                    WaferSystemProgram.GetInstance().MagazineProgram.Name = string.Empty;
                    WaferSystemProgram.GetInstance().Save();
                    Thread.Sleep(100);

                    ((ComboBoxEdit)sender).Text = string.Empty;
                }

                this.RefreshCmbItems();

                this.Confirm();
            }
        }

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            CmbWafer.Properties.Items.Clear();
            List<string> nameList = MagazineBoxConfigRepository.GetInstance().Filter(string.Empty, false).Select(a => a.Name).ToList();
            CmbWafer.Properties.Items.AddRange(nameList);
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            if (CmbWafer.EditValue == null)
            {
               // XtraMessageBox.Show();
            }
            
            WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.Name = this.CmbWafer.EditValue?.ToString();
            WaferSystemProgram.GetInstance().Save();
        }

        /// <summary>
        /// 选择改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbWafer_SelectedIndexChanged(object sender, EventArgs e)
        {
            TreeListNode waferHandlingNode = this.treeListProgramming.FindNode(a => a.Tag.ToString() == "WaferHandling");
 
            waferHandlingNode.Nodes.Clear();

            if (!string.IsNullOrEmpty(this.CmbWafer.Text))
            {
                WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.Name = this.CmbWafer.EditValue?.ToString();
                WaferSystemProgram.GetInstance().Save();

                MagazineBoxConfig magazineBoxConfig = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig;

                // 添加Node 
                string waferHandlingDatasetName = this.CmbWafer.Text;

                TreeListNode node = treeListProgramming.AppendNode(new object[] { waferHandlingDatasetName }, waferHandlingNode);
                node.Tag = "WaferHandlingSubNode";

                this.SetMagazineBoxStateImage(magazineBoxConfig, node);

                //// 设置图标
                //BaseDsSetting dsSetting = MagazineBoxConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == waferHandlingDatasetName);
                //node.StateImageIndex = (int)dsSetting.EditState;
            }

            this.Confirm();
        }

        /// <summary>
        /// SetMagazineBoxStateImage
        /// </summary>
        /// <param name="magazineBoxConfig">magazineBoxConfig</param>
        /// <param name="node">node</param>
        private void SetMagazineBoxStateImage(MagazineBoxConfig magazineBoxConfig, TreeListNode node)
        {
            if (magazineBoxConfig.IsAssistantSucceed)
            {
                node.StateImageIndex = 3;
            }
            else
            {
                node.StateImageIndex = 2;
            }
        }
    }
}
