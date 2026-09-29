using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
using AKRS.ZX2200.BondSystem.Modules;

using DevExpress.XtraEditors;


namespace AKRS.ZX2200.BondSystem.Controls.Setting.NozzleShelf
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    using Nozzle = AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle.Nozzle;
    using NozzleShelf = AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf.NozzleShelf;

    /// <summary>
    /// 吸嘴架配置界面
    /// </summary>
    public partial class FrmNozzleShelfSetting : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNozzleShelfSetting()
        {
            this.InitializeComponent();
            this.Init();
        }

        /// <summary>
        /// 吸嘴池(当前配方的吸嘴集合）
        /// </summary>
        private List<Nozzle> nozzlePool = new List<Nozzle>();

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// Bond模组
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController = new NozzleShelfController();

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void Init()
        {
            this.CmbCurNozzleShelf.Text = BondProgram.GetInstance().NozzleShelfProgram.NozzleShelfName;
            this.RefreshCombobox();
            this.RefreshGcNozzleShelf();
            this.RefreshGcNozzlePool();
        }

        /// <summary>
        /// 刷新当前吸嘴架
        /// </summary>
        private void RefreshGcNozzleShelf()
        {
            if (string.IsNullOrEmpty(this.CmbCurNozzleShelf.Text))
            {
                return;
            }

            if (NozzleShelfRepository.GetInstance().Find(this.CmbCurNozzleShelf.Text) == null)
            {
                return;
            }

            NozzleShelfSlot[] nozzleShelfSlots = NozzleShelfRepository.GetInstance()
                .GetNozzleShelf(this.CmbCurNozzleShelf.Text).NozzleShelfSlots;

            this.GcNozzleShelf.DataSource = nozzleShelfSlots;
            this.GcNozzleShelf.RefreshDataSource();
            this.RefreshGcNozzlePool();
        }

        /// <summary>
        /// 刷新Combobox
        /// </summary>
        private void RefreshCombobox()
        {
            this.CmbCurNozzleShelf.Properties.Items.Clear();
            List<string> nameList = NozzleShelfRepository.GetInstance().Filter(string.Empty, false).Select(a => a.Name).ToList();

            // 控件绑定数据源
            this.CmbCurNozzleShelf.Properties.Items.AddRange(nameList);
            this.CmbCurNozzleShelf.EditValue = BondProgram.GetInstance().NozzleShelfProgram.NozzleShelfName;

            // 控件绑定数据源
            LueNozzleForceMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<NozzleForceModeEnum>();

            Nozzle nozzle = (Nozzle)this.GvNozzlePool.GetFocusedRow() ?? new Nozzle();
        }
        
        /// <summary>
        /// 刷新当前吸嘴池
        /// </summary>
        private void RefreshGcNozzlePool()
        {
            // 获取当前配方的吸嘴集合
            this.nozzlePool = NozzleRepository.GetInstance().GetDataSourceByCurrentRecipe();

            if (NozzleShelfRepository.GetInstance().Find(this.CmbCurNozzleShelf.Text) != null) 
            {
                NozzleShelf shelf = NozzleShelfRepository.GetInstance()
                    .GetNozzleShelf(this.CmbCurNozzleShelf.Text);

                if (shelf == null)
                {
                    return;
                }

                NozzleShelfSlot[] nozzleShelfSlots = shelf.NozzleShelfSlots;

                if (nozzleShelfSlots != null)
                {
                    this.nozzlePool.RemoveAll(a => nozzleShelfSlots.Exists(b => b.NozzleName == a.Name));
                }
            }

            this.GcNozzlePool.DataSource = this.nozzlePool;
            this.GcNozzlePool.Refresh();
        }

        /// <summary>
        /// 取吸嘴调试动作
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnToolBankService_Click(object sender, EventArgs e)
        {
            double pos = this.nozzleShelfController.GetYAxisPos();
            if (pos >= -1.5)
            {
                this.BtnToolBankService.Appearance.BackColor = Color.Yellow;

                // 防撞
                if (!this.system2Controller.IsNozzleShelfAxisYSafe())
                {
                    this.bondModuleController.MoveToSafePos();
                }

                // 吸嘴架Y到换吸嘴位
                this.nozzleShelfController.MoveShelfToChangeNozzlePos();
            }
            else
            {
                this.BtnToolBankService.Appearance.BackColor = Color.Transparent;

                // 吸嘴架Y回到原位
                this.nozzleShelfController.MoveShelfToHome();
            }
        }

        /// <summary>
        /// 吸嘴架下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbNozzleShelfRepository_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<NozzleShelf> frmRepository = new FrmRepository<NozzleShelf>(NozzleShelfRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                   NozzleShelf temp = (NozzleShelf)frmRepository.DsSetting;

                    if (temp != null)
                    {
                        ((ComboBoxEdit)sender).Text = temp.Name;
                    }
                }

                this.RefreshCombobox();
            }
        }

        /// <summary>
        /// 吸嘴管理按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnNozzleManage_Click(object sender, EventArgs e)
        {
            FrmRepository<Nozzle> frmRepository = new FrmRepository<Nozzle>(NozzleRepository.GetInstance(),this.RefreshGcNozzlePool);

            // 获取显示器屏幕宽度,高度
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;
            frmRepository.Location = new Point(xWidth - 700, 250);
            frmRepository.StartPosition = FormStartPosition.Manual;
            frmRepository.ShowDialog();

            // 获取当前配方的吸嘴集合
            this.nozzlePool = NozzleRepository.GetInstance().GetDataSourceByCurrentRecipe();

            this.RefreshGcNozzlePool();
            this.RefreshNozzleShelf();
            this.RefreshGcNozzleShelf();
        }

        /// <summary>
        /// 改变所选吸嘴架
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbNozzleShelfRepository_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshNozzleShelf();
            this.RefreshGcNozzleShelf();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            Nozzle nozzle = (Nozzle)this.GvNozzlePool.GetFocusedRow() ?? new Nozzle();

            BondProgram.GetInstance().NozzleShelfProgram.NozzleShelfName = this.CmbCurNozzleShelf.Text;

            // 保存
            NozzleRepository.GetInstance().Save();
            NozzleShelfRepository.GetInstance().Save();
            BondProgram.GetInstance().Save();
        }

        /// <summary>
        /// 吸嘴向左填充到吸嘴槽
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGoLeft_Click(object sender, EventArgs e)
        {
            // 吸嘴架当前选中的槽
            NozzleShelfSlot toolBankSlot = (NozzleShelfSlot)this.GvNozzleShelf.GetFocusedRow();

            Nozzle nozzle = (Nozzle)this.GvNozzlePool.GetFocusedRow();

            if (toolBankSlot == null || toolBankSlot.NozzleName != string.Empty || nozzle == null) 
            {
                return;
            }

            // 工艺要求，暂时注释
            //if (nozzle.NozzleSize == NozzleSizeEnum.Large
            //    && toolBankSlot.MaximumToolSize == MaximumToolSizeEnum.SmallTool)
            //{
            //    return;
            //}

            toolBankSlot.NozzleName = nozzle.Name;
            nozzle.SlotIdentification = toolBankSlot.SlotNum;
            toolBankSlot.NozzleState = NozzleStateEnum.OnSlot;
            this.nozzlePool.Remove(nozzle);

            this.RefreshGcNozzlePool();
            this.RefreshGcNozzleShelf();
        }

        /// <summary>
        /// 吸嘴向右移出按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGoRight_Click(object sender, EventArgs e)
        {
            // 吸嘴架当前选中的槽
            NozzleShelfSlot toolBankSlot = (NozzleShelfSlot)this.GvNozzleShelf.GetFocusedRow();

            if (toolBankSlot == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(toolBankSlot.NozzleName)) 
            {
                return;
            }

            // 槽位初始化
            NozzleRepository.GetInstance().GetNozzle(toolBankSlot.NozzleName).SlotIdentification = -1;

            this.nozzlePool.Add(NozzleRepository.GetInstance().GetNozzle(toolBankSlot.NozzleName));
            toolBankSlot.NozzleName = string.Empty;
            toolBankSlot.NozzleState = NozzleStateEnum.Empty;

            this.RefreshGcNozzlePool();
            this.RefreshGcNozzleShelf();
        }

        /// <summary>
        /// 新建吸嘴按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            FrmNewCreateNozzle frmNewCreateNozzle = new FrmNewCreateNozzle();

            // 获取显示器屏幕宽度,高度
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;
            frmNewCreateNozzle.Location = new Point(xWidth - 500, 250);
            frmNewCreateNozzle.StartPosition = FormStartPosition.Manual;
            if (frmNewCreateNozzle.ShowDialog() == DialogResult.OK)
            {
                // 吸嘴池刷新
                this.RefreshGcNozzlePool();
            }
        }

        /// <summary>
        /// 删除吸嘴按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            Nozzle nozzle = (Nozzle)this.GvNozzlePool.GetFocusedRow();

            if (nozzle == null)
            {
                return;
            }

            // 移除吸嘴
            nozzle.RemoveFromBelongRecipeIds();
            NozzleRepository.GetInstance().Save();

            // 吸嘴池刷新
            this.RefreshGcNozzlePool();
        }

        /// <summary>
        /// OK
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtOk_Click(object sender, EventArgs e)
        {
            Save();

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
        /// 刷新吸嘴架，防呆
        /// </summary>
        private void RefreshNozzleShelf()
        {
            NozzleShelf shelf = NozzleShelfRepository.GetInstance()
                .GetNozzleShelf(this.CmbCurNozzleShelf.Text);

            if (shelf == null)
            {
                return;
            }

            foreach (var slot in shelf.NozzleShelfSlots)
            {
                if (!string.IsNullOrEmpty(slot.NozzleName))
                {
                    if (NozzleRepository.GetInstance().Find(slot.NozzleName) == null)
                    {
                        slot.NozzleName = string.Empty;
                        slot.NozzleState = NozzleStateEnum.Empty;
                    }
                }
            }
        }
    }
}
