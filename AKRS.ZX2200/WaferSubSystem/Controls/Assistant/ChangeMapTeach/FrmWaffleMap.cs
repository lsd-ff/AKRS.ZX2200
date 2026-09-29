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

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach
{
    using System.Threading;

    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.CodeParser;

    using Block = AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip.Block;

    /// <summary>
    /// 华夫盒图
    /// </summary>
    public partial class FrmWaffleMap : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmWaffleMap()
        {
            this.InitializeComponent();
            this.LueWafflesName.Visible = false;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmWaffleMap(bool isStaticAdapterTablet)
        {
            this.InitializeComponent();
            this.Load += new System.EventHandler(this.FrmWaffleMap_Load);
            this.LueWafflesName.Visible = true;
            this.IsStaticAdapterTablet = isStaticAdapterTablet;
        }

        private bool IsStaticAdapterTablet;

        /// <summary>
        /// FrmWaffleMap_FormClosing
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmWaffleMap_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.BeginInvoke(new Action(()=>this.Hide()));
        }

        private UcCarrierMapping ucCarrierMapping;

        /// <summary>
        /// LoadCarrierMapping
        /// </summary>
        /// <param name="adapterSlotEntity">adapterSlotEntity</param>
        public void LoadCarrierMapping(AdapterSlotEntity adapterSlotEntity)
        {
            if (adapterSlotEntity == null)
            {
                this.Invoke(new Action(() => this.GcWaffleMap.Controls.Clear()));
            }
            else
            {
                this.Invoke(new Action(() => this.GcWaffleMap.Controls.Clear()));

                if (ucCarrierMapping == null)
                {
                    this.Invoke(new Action(() => ucCarrierMapping = new UcCarrierMapping(adapterSlotEntity)));
                }
                else
                {
                    this.Invoke(new Action(() => ucCarrierMapping.SetCarrierMapping(adapterSlotEntity)));
                }

                this.Invoke(new Action(() => this.GcWaffleMap.Controls.Add(ucCarrierMapping)));

                //this.Invoke(new Action(() => this.GcWaffleMap.Controls.Add(new UcCarrierMapping(adapterSlotEntity))));
            }
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public void RefreshMap()
        {
            this.Invoke(new Action(() => this.GcWaffleMap.Refresh()));
        }

        private AdapterTablet ad;

        private void FrmWaffleMap_Load(object sender, EventArgs e)
        {
            this.ad = null;
            if (this.IsStaticAdapterTablet)
            {
                ad = (AdapterTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet;
            }
            else if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet adapterTablet)
            {
                ad = adapterTablet;
            }

            this.LueWafflesName.Properties.DataSource =
                ad?.AdapterSetting.WaffleArray.Where(a => a.CarrierWithWaffleConfig != null && a.Index + 1 <= ad.AdapterSetting.MaxUseWaffleCount).Select(a => (a.Index + 1) + "-" + a.Name);
        }

        private void LueWafflesName_EditValueChanged(object sender, EventArgs e)
        {
            string str = this.LueWafflesName.Text;
            if (string.IsNullOrEmpty(str))
            {
                return;
            }

            int i = int.Parse(str.Substring(0, 1)) - 1;
            LoadCarrierMapping(ad.AdapterState.WafflePlateSlotsState[i]);
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            WaferSubDevicePara.GetInstance().Save();
        }
    }
}