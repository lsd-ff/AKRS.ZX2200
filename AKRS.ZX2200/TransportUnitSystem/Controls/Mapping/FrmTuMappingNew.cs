using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 载具实时显示界面
    /// </summary>
    public partial class FrmTuMappingNew : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 载具实时显示界面
        /// </summary>
        public FrmTuMappingNew()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 载具
        /// </summary>
        private TransportUnit TransportUnit = new TransportUnit("mapping");

        /// <summary>
        /// 配置对象
        /// </summary>
        private ProductConfiguration productConfig => ProductConfiguration.GetInstance();

        /// <summary>
        /// 载具显示
        /// </summary>
        private UcTransportShow ucTransportShow;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmTransportUnitMapping_Load(object sender, EventArgs e)
        {
            this.ucTransportShow = new UcTransportShow(this.TransportUnit);

            this.ucTransportShow.Dock = DockStyle.Fill;

            this.panelControl2.Controls.Add(this.ucTransportShow);

            this.LueWorkSortType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<WorkOrderEnum>();
            this.LueWorkSortType.EditValue = this.productConfig.SubstrateConfig.WorkOrderEnum;
            this.ChkAutoReturn.Checked = this.productConfig.SubstrateConfig.Arrangement == ArrangementEnum.Snake;
        }

        /// <summary>
        /// 选择基板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkSubstrate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkSubstrate.Checked)
            {
                this.LueWorkSortType.EditValue = this.productConfig.SubstrateConfig.WorkOrderEnum;
                this.ChkAutoReturn.Checked = this.productConfig.SubstrateConfig.Arrangement == ArrangementEnum.Snake;
                this.productConfig.Save();
            }
        }

        /// <summary>
        /// 选择基岛
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkModule_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkModule.Checked)
            {
                this.LueWorkSortType.EditValue = this.productConfig.ModuleConfig.WorkOrderEnum;
                this.ChkAutoReturn.Checked = this.productConfig.ModuleConfig.Arrangement == ArrangementEnum.Snake;
                this.productConfig.Save();
            }
        }

        /// <summary>
        /// 是否自动转向
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkAutoReturn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkSubstrate.Checked)
            {
                this.productConfig.SubstrateConfig.Arrangement =
                    this.ChkAutoReturn.Checked ? ArrangementEnum.Snake : ArrangementEnum.Normal;
            }
            else
            {
                this.productConfig.ModuleConfig.Arrangement =
                    this.ChkAutoReturn.Checked ? ArrangementEnum.Snake : ArrangementEnum.Normal;
            }

            this.productConfig.Save();
        }

        /// <summary>
        /// 清空所有记忆
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClearAll_Click(object sender, EventArgs e)
        {
            this.productConfig.OtherConfig.ClearInformation();
            this.TransportUnit = new TransportUnit("mapping");
            this.ucTransportShow.ReFreshUi(TransportUnit);
            this.productConfig.Save();
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueWorkSortType_EditValueChanged(object sender, EventArgs e)
        {
            if (this.ChkSubstrate.Checked)
            {
                this.productConfig.SubstrateConfig.WorkOrderEnum = (WorkOrderEnum)this.LueWorkSortType.EditValue;
            }
            else
            {
                this.productConfig.ModuleConfig.WorkOrderEnum = (WorkOrderEnum)this.LueWorkSortType.EditValue;
            }

            this.ChangeImageBySelect((WorkOrderEnum)this.LueWorkSortType.EditValue);
            this.productConfig.Save();
        }

        /// <summary>
        /// 根据选项更改图片
        /// </summary>
        /// <param name="workOrder">工作顺序</param>
        private void ChangeImageBySelect(WorkOrderEnum workOrder)
        {
            switch ((int)workOrder)
            {
                case 0:
                    this.pictureEdit1.EditValue = Properties.Resources.ToRightUp;
                    break;
                case 1:
                    this.pictureEdit1.EditValue = Properties.Resources.ToUpRight;
                    break;
                case 2:
                    this.pictureEdit1.EditValue = Properties.Resources.ToLeftUp;
                    break;
                case 3:
                    this.pictureEdit1.EditValue = Properties.Resources.ToUpLeft;
                    break;
                case 4:
                    this.pictureEdit1.EditValue = Properties.Resources.ToRightDown;
                    break;
                case 5:
                    this.pictureEdit1.EditValue = Properties.Resources.ToDownRight;
                    break;
                case 6:
                    this.pictureEdit1.EditValue = Properties.Resources.ToLeftDown;
                    break;
                case 7:
                    this.pictureEdit1.EditValue = Properties.Resources.ToDownLeft;
                    break;
            }
        }

        /// <summary>
        /// 保存更改的信息
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmTuMappingNew_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.ucTransportShow.IsChange)
            {
                // 保存更改的信息
                this.productConfig.OtherConfig.SaveInformation(this.TransportUnit);
            }
        }
    }
}