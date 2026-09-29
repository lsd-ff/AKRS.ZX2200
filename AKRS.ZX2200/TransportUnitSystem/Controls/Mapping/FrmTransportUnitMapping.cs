using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.Programs;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.TransportUnitSystem.Service;

    /// <summary>
    /// 载具实时显示界面
    /// </summary>
    public partial class FrmTransportUnitMapping : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 载具实时显示界面
        /// </summary>
        /// <param name="baseSubSectionProgram">载台</param>
        public FrmTransportUnitMapping(BaseSubSectionProgram baseSubSectionProgram)
        {
            this.InitializeComponent();
            this.baseSubSectionProgram = baseSubSectionProgram;
        }

        /// <summary>
        /// 载具程式
        /// </summary>
        private readonly BaseSubSectionProgram baseSubSectionProgram;

        /// <summary>
        /// 载具
        /// </summary>
        private TransportUnit Transport => this.baseSubSectionProgram.TransportUnit;

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
            if (this.Transport != null)
            {
                this.Transport.TransportUnitInfo.IsProduct = true;
            }

            this.ucTransportShow = new UcTransportShow(this.Transport, CurrentMachineSystemEnum.System2);

            this.ucTransportShow.Dock = DockStyle.Fill;

            this.panelControl2.Controls.Add(this.ucTransportShow);

            this.labelControl2.Text = this.Transport?.TransportUnitInfo.SubstrateNumber;
        }


        /// <summary>
        /// 刷新UI
        /// </summary>
        public void ReFreshUi()
        {
            this.ucTransportShow.ReFreshUi(this.Transport);
        }

        /// <summary>
        /// 导入Mapping
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtImportMapping_Click(object sender, EventArgs e)
        {
            if (this.Transport == null)
            {
                AKRSXtraMessageBox.Show("请先放上载具并搜索");
                return;
            }

            //string id = XtraInputBox.Show("请用扫码枪扫描条形码", "ID", string.Empty);

            //if (id == string.Empty)
            //{
            //    AKRSXtraMessageBox.Show("输入有误，请重写输入");
            //}
            //else 
            //{
            //    // 寻找载具
            //}

            TransportUnit transportNew = TuService.ReadTransportMapping();

            if (transportNew == null)
            {
                return;
            }

            TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit = transportNew;

            this.panelControl2.Controls.Clear();

            this.ucTransportShow = new UcTransportShow(this.Transport, CurrentMachineSystemEnum.System2);

            this.ucTransportShow.Dock = DockStyle.Fill;

            this.panelControl2.Controls.Add(this.ucTransportShow);

            this.labelControl2.Text = this.Transport?.TransportUnitInfo.SubstrateNumber;
        }


        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtInit_Click(object sender, EventArgs e)
        {
            foreach (Substrate substrate in this.Transport.Substrates)
            {
                foreach (Module module in substrate.Modules)
                {
                    foreach (BondPosition bondPosition in module.BondPositions)
                    {
                        bondPosition.SetMatterEnable();
                        bondPosition.SetProcessUnable();
                    }
                }
            }

            this.ReFreshUi();
        }

        /// <summary>
        /// 输入产品信息
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtInPutTuInfo_Click(object sender, EventArgs e)
        {
            if (this.Transport == null)
            {
                AKRSXtraMessageBox.Show("请先放上载具并搜索");
                return;
            }

            FrmAddTuInfo frmAddTuInfo = new FrmAddTuInfo(this.Transport);
            frmAddTuInfo.ShowDialog();
            frmAddTuInfo.Dispose();

            this.labelControl2.Text = this.Transport?.TransportUnitInfo.SubstrateNumber;
        }

        /// <summary>
        /// 保存mapping
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSaveMapping_Click(object sender, EventArgs e)
        {
            TuService.SaveMappingToLocal(this.Transport);
            this.labelControl2.Text = this.Transport?.TransportUnitInfo.SubstrateNumber;
        }

        /// <summary>
        /// 扫码识别Mapping
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtInputID_Click(object sender, EventArgs e)
        {
            if (this.Transport == null)
            {
                AKRSXtraMessageBox.Show("请先放上载具并搜索");
                return;
            }

            string id = XtraInputBox.Show("请用扫码枪扫描条形码", "ID", string.Empty);

            if (id == string.Empty)
            {
                AKRSXtraMessageBox.Show("输入有误，请重写输入");
            }
            else
            {
                TransportUnit transportUnit = TuService.ReadTransportMapping(
                    "D:\\ZX2200存储数据\\" + MachineConfigContext.GetInstance().RecipeName + "//" + id + ".txt");

                if (transportUnit == null)
                {
                    return;
                }

                this.baseSubSectionProgram.TransportUnit = transportUnit;

                this.labelControl2.Text = this.Transport?.TransportUnitInfo.SubstrateNumber;
            }
        }
    }
}