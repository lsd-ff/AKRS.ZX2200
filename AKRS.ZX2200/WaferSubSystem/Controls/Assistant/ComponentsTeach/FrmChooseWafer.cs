using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using AKRS.ZX2200.WaferSubSystem.Models;
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
using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using AKRS.Galaxy2.BackgroundWorkThread;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.XtraEditors.Controls;

    /// <summary>
    /// 选择Slot
    /// </summary>
    public partial class FrmChooseWafer : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 需要示教的CarrierNameMagazineSlotIndex
        /// </summary>
        public int MagazineSlotIndexOfNeedTeach { get; set; }

        /// <summary>
        /// 载具名称
        /// </summary>
        private string carrierName = string.Empty;

        /// <summary>
        /// 载具列表
        /// </summary>
        private List<string> list = new List<string>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="carrierName">carrierName</param>
        public FrmChooseWafer(string carrierName)
        {
            this.InitializeComponent();
            this.carrierName = carrierName;
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.GetCarrierList();
            foreach (var item in this.list)
            {
                RadioGroupItem temp = new RadioGroupItem() { Description = item };
                this.RgSlotSelect.Properties.Items.Add(temp);
            }
        }

        /// <summary>
        /// OK按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.MagazineSlotIndexOfNeedTeach = int.Parse(this.list[this.RgSlotSelect.SelectedIndex].Split(':')[0].Substring(5)) - 1;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 获取载具列表
        /// </summary>
        private void GetCarrierList()
        {
            for (int i = 0; i < WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.MaxUseLayerCount; i++)
            {
                if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i] is AdapterTablet)
                {
                    AdapterTablet wft = (AdapterTablet)WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i];
                    for (int j = 0; j < wft.AdapterSetting.MaxUseWaffleCount; j++)
                    {
                        if (wft.AdapterSetting.WaffleArray[j].Name == this.carrierName && wft.AdapterSetting.WaffleArray[j].Name != string.Empty)
                        {
                            string name = $"Slot {i + 1}: " + wft.AdapterSetting.Name;
                            if (!this.list.Exists(item => item == name))
                            {
                                this.list.Add(name);
                            }
                        }
                    }
                }
                else if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i] is WaferTablet)
                {
                    WaferTablet wt = (WaferTablet)WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i];
                    if (wt?.CarrierConfigWithWafer?.Name == this.carrierName)
                    {
                        string name = $"Slot {i + 1}: " + wt.CarrierConfigWithWafer.Name;
                        if (!this.list.Exists(item => item == name))
                        {
                            this.list.Add(name);
                        }
                    }
                }
            }
        }
    }
}