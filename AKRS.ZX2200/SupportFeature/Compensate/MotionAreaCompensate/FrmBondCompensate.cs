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

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using DevExpress.XtraGrid.Views.Grid;

    /// <summary>
    /// 贴片补偿
    /// </summary>
    public partial class FrmBondCompensate : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 贴片补偿
        /// </summary>
        public FrmBondCompensate()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 补偿值X
        /// </summary>
        private double[,] TransportUnitCompensateX => TransportUnitCompensate.GetInstance().TransportUnitCompensateX;

        /// <summary>
        /// 补偿值Y
        /// </summary>
        private double[,] TransportUnitCompensateY => TransportUnitCompensate.GetInstance().TransportUnitCompensateY;

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtInput_Click(object sender, EventArgs e)
        {
            FrmInputData frmInputData = new FrmInputData();
            if (frmInputData.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            List<DefectStatisticsEntity> list = frmInputData.list;

            if (list == null || list.Count == 0)
            {
                AKRSXtraMessageBox.Show("数据为空或数据量为0，请更改后重试");
                return;
            }

            TransportUnitCompensate.GetInstance().ColumnCount = ProductDomain.GetInstance().ProductConfig.SubstrateConfig.ColumnCount;
            TransportUnitCompensate.GetInstance().RowCount = ProductDomain.GetInstance().ProductConfig.SubstrateConfig.RowCount;

            int allCount = TransportUnitCompensate.GetInstance().ColumnCount * TransportUnitCompensate.GetInstance().RowCount;

            if (list.Count != allCount)
            {
                AKRSXtraMessageBox.Show($"数据个数不匹配，当前数据为{list.Count}，需要数量为{allCount}，请更改后重试");
                return;
            }

            TransportUnitCompensate.GetInstance().StartPoint = new AKRSPoint3D(
                list[0].BondPositionX,
                list[0].BondPositionY,
                0);

            TransportUnitCompensate.GetInstance().ColumnSpacing = ProductDomain.GetInstance().ProductConfig.SubstrateConfig.ColumnSpacing;

            TransportUnitCompensate.GetInstance().RowSpacing = ProductDomain.GetInstance().ProductConfig.SubstrateConfig.RowSpacing;

            double[,] compensateX = new double[TransportUnitCompensate.GetInstance().RowCount, TransportUnitCompensate.GetInstance().ColumnCount];
            double[,] compensateY = new double[TransportUnitCompensate.GetInstance().RowCount, TransportUnitCompensate.GetInstance().ColumnCount];

            double[,] compensateX1 = new double[TransportUnitCompensate.GetInstance().RowCount, TransportUnitCompensate.GetInstance().ColumnCount];
            double[,] compensateY1 = new double[TransportUnitCompensate.GetInstance().RowCount, TransportUnitCompensate.GetInstance().ColumnCount];



            for (int i = 0; i < TransportUnitCompensate.GetInstance().RowCount; i++)
            {
                for (int j = 0; j < TransportUnitCompensate.GetInstance().ColumnCount; j++)
                {
                    compensateX[i, j] = list[j + i * TransportUnitCompensate.GetInstance().ColumnCount].OffsetX / 1000.0;
                    compensateY[i, j] = list[j + i * TransportUnitCompensate.GetInstance().ColumnCount].OffsetY / 1000.0;
                }
            }

            for (int i = 0; i < TransportUnitCompensate.GetInstance().RowCount; i++)
            {
                for (int j = 0; j < TransportUnitCompensate.GetInstance().ColumnCount; j++)
                {
                    if (j == 0 || j == TransportUnitCompensate.GetInstance().ColumnCount - 1)
                    {
                        compensateX1[i, j] = compensateX[i, j];
                        compensateY1[i, j] = compensateY[i, j];

                        continue;
                    }
                    else if (j == 1 || j == TransportUnitCompensate.GetInstance().ColumnCount - 2)
                    {
                        compensateX1[i, j] = (compensateX[i, j - 1] + compensateX[i, j] + compensateX[i, j + 1]) / 3.0;
                        compensateY1[i, j] = (compensateY[i, j - 1] + compensateY[i, j] + compensateY[i, j + 1]) / 3.0;
                    }
                    else
                    {
                        compensateX1[i, j] = (compensateX[i, j - 2] + compensateX[i, j - 1] + compensateX[i, j] + compensateX[i, j + 1] + compensateX[i, j + 2]) / 5.0;
                        compensateY1[i, j] = (compensateY[i, j - 2] + compensateY[i, j - 1] + compensateY[i, j] + compensateY[i, j + 1] + compensateY[i, j + 2]) / 5.0;
                    }

                }
            }

            TransportUnitCompensate.GetInstance().TransportUnitCompensateX = compensateX1;
            TransportUnitCompensate.GetInstance().TransportUnitCompensateY = compensateY1;

            TransportUnitCompensate.GetInstance().Save();
            this.InitData();
        }

        private void ucDefectStatistics1_Load(object sender, EventArgs e)
        {

        }

        private void BtClear_Click(object sender, EventArgs e)
        {
            TransportUnitCompensate.GetInstance().TransportUnitCompensateX = null;
            TransportUnitCompensate.GetInstance().TransportUnitCompensateY = null;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmBondCompensate_Load(object sender, EventArgs e)
        {
            this.gridView1.CustomDrawRowIndicator += gdv_CustomDrawRowIndicator;
            this.gridColumn1.FieldName = "X";
            this.gridColumn2.FieldName = "Y";
            this.InitData();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        private void InitData()
        {

            this.GcCompensateData.DataSource = null;

            if (TransportUnitCompensate.GetInstance().TransportUnitCompensateX == null
                   || TransportUnitCompensate.GetInstance().TransportUnitCompensateY == null)
            {
                return;
            }

            List<AKRSPoint2D> list = new List<AKRSPoint2D>();
            for (int i = 0; i < TransportUnitCompensate.GetInstance().TransportUnitCompensateX.GetLength(0); i++)
            {
                for (int j = 0; j < TransportUnitCompensate.GetInstance().TransportUnitCompensateX.GetLength(1); j++)
                {
                    list.Add(new AKRSPoint2D(TransportUnitCompensate.GetInstance().TransportUnitCompensateX[i, j], TransportUnitCompensate.GetInstance().TransportUnitCompensateY[i, j]));
                }

            }

            this.GcCompensateData.DataSource = list;
            this.GcCompensateData.Refresh();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.TransportUnitCompensateX.GetLength(0); i++)
            {
                for (int j = 0; j < this.TransportUnitCompensateX.GetLength(1); j++)
                {
                    this.TransportUnitCompensateX[i, j] = (double)this.gridView1.GetRowCellValue(i * this.TransportUnitCompensateX.GetLength(1) + j, "X");
                    this.TransportUnitCompensateY[i, j] = (double)this.gridView1.GetRowCellValue(i * this.TransportUnitCompensateX.GetLength(1) + j, "Y");
                }

            }
            TransportUnitCompensate.GetInstance().Save();

            this.InitData();
        }

        private void gdv_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

    }
}