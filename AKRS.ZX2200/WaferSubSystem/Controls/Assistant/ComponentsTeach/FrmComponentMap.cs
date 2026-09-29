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

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using System.Threading;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.Utils.Behaviors.Common;

    /// <summary>
    /// FrmComponentMap
    /// </summary>
    public partial class FrmComponentMap : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// baseCarrierConfig
        /// </summary>
        private BaseCarrierConfig baseCarrierConfig;

        /// <summary>
        /// 需要示教的CarrierNameMagazineSlotIndex
        /// </summary>
        private int magazineSlotIndexOfNeedTeach;

        /// <summary>
        /// FrmComponentMap
        /// </summary>
        /// <param name="baseCarrierConfig">baseCarrier</param>
        public FrmComponentMap(BaseCarrierConfig baseCarrierConfig)
        {
            this.InitializeComponent();
            Block.GetInstance().StartInit();
            this.baseCarrierConfig = baseCarrierConfig;
            Block.GetInstance().SetCurrentCarrier(this.baseCarrierConfig);
            Block.GetInstance().ClearReferencePointCoordinates();
            if (!(this.baseCarrierConfig is CarrierWithWaferConfig))
            {
                this.BtnSetReferencePoint1.Enabled = false;
            }

            FrmChooseWafer temp = new FrmChooseWafer(this.baseCarrierConfig.Name);
            temp.ShowDialog();
            if (temp.DialogResult == DialogResult.Cancel)
            {
                this.Close();
                return;
            }

            this.magazineSlotIndexOfNeedTeach = temp.MagazineSlotIndexOfNeedTeach;
        }

        /// <summary>
        /// BtnOK_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                if (this.baseCarrierConfig is CarrierWithWaferConfig carrierWithWaferConfig)
                {
                    carrierWithWaferConfig.WaferMapDataConfig = Block.GetInstance().GetWaferMapDataConfig();
                    CarrierConfigRepository.GetInstance().Save();
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnCancel_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// BtnSetReferencePoint1_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnSetReferencePoint1_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                Block.GetInstance().SetReferencePoint1();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnLoadWaferMap_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnLoadWaferMap_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                string fileName = this.baseCarrierConfig.Name + "-" + $"{this.magazineSlotIndexOfNeedTeach + 1}";
                Block.GetInstance().LoadMap(fileName);
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnResetWaferMap_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnResetWaferMap_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                string fileName = this.baseCarrierConfig.Name + "-" + $"{this.magazineSlotIndexOfNeedTeach + 1}";
                Block.GetInstance().LoadMap(fileName, true);
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        private void BtnShowMap_Click(object sender, EventArgs e)
        {
            Block.GetInstance().ShowMapping();
        }

        private void FrmComponentMap_Load(object sender, EventArgs e)
        {
            Block.GetInstance().ShowMapping();
        }
    }
}