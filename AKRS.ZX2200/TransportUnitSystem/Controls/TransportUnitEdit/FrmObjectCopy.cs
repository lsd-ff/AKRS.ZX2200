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

namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;

    using DevComponents.DotNetBar.Controls;

    /// <summary>
    /// 复制
    /// </summary>
    public partial class FrmObjectCopy : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="oppositeSexConfig">被复制对象</param>
        public FrmObjectCopy(OppositeSex oppositeSexConfig)
        {
            this.InitializeComponent();
            this.olOppositeSexConfig = oppositeSexConfig;
            this.TxName.Text = oppositeSexConfig.Name;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// <summary>
        /// 被复制对象
        /// </summary>
        private readonly OppositeSex olOppositeSexConfig;

        /// <summary>
        /// 复制的对象
        /// </summary>
        public List<OppositeSex> NewOppositeSexConfigs { get; set; } = new ();

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 配置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSure_Click(object sender, EventArgs e)
        {
            double columnCount = (double)this.SpinColumnCount.Value;
            double rowCount = (double)this.SpinRowCount.Value;
            double columnSpacing = (double)this.SpinColumnSpace.Value;
            double rowSpacing = (double)this.SpinRowSpace.Value;

            if (!this.ChkCenterDistance.Checked)
            {
                columnSpacing += this.olOppositeSexConfig.SizeX;
                rowSpacing += this.olOppositeSexConfig.SizeY;
            }

            for (int j = 0; j < columnCount; j++)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    OppositeSex oppositeSexConfig = ObjectHelper.Clone(this.olOppositeSexConfig);

                    oppositeSexConfig.ElementCoordinate.Point += new AKRSPoint3D(columnSpacing * j, rowSpacing * i, 0);

                    this.NewOppositeSexConfigs.Add(oppositeSexConfig);
                }
            }


            // 创建对象
            this.DialogResult = DialogResult.OK;
        }
    }
}