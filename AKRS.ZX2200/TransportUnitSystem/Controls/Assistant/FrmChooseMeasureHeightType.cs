using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.MeasureHeight;
using AKRS.ZX2200.BondSystem.Models;
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
using static AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Machine.Enums;

    using DevExpress.Utils.Internal.DTE;

    /// <summary>
    /// 选择测高方式
    /// </summary>
    public partial class FrmChooseMeasureHeightType : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 测高方式
        /// </summary>
        public MultipleHeightMeasurementType Type { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmChooseMeasureHeightType()
        {
            this.InitializeComponent();
            this.LueHeightType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<MultipleHeightMeasurementType>();
            this.LueHeightType.EditValue = MultipleHeightMeasurementType.TouchDown;
            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
            {
                System2Domain.GetInstance().BondHeadController.SetTouchDownOnBondhead();
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtOK_Click(object sender, EventArgs e)
        {
            this.Type = (MultipleHeightMeasurementType)this.LueHeightType.EditValue;
            this.DialogResult = DialogResult.OK;
        }


        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}