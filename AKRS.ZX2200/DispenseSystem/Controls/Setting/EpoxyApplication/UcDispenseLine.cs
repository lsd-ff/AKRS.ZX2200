using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraCharts;

namespace AKRS.ZX2200.DispenseSystem.Controls.Setting.EpoxyApplication
{
    using Models.DispensePara;
    using Models.Repositories.Pattern;

    /// <summary>
    /// 显示画胶的线段
    /// </summary>
    public partial class UcDispenseLine : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public UcDispenseLine()
        {
            this.InitializeComponent(); 
            
            this.Init();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="epoxyApplication">画胶参数</param>
        public void Init(EpoxyApplication epoxyApplication = null)
        {
            this.ChangeDispenseLine(epoxyApplication);
        }

        /// <summary>
        /// 将线段实时显示出来
        /// </summary>
        /// <param name="epoxyApplication">画胶图形</param>
        public void ChangeDispenseLine(EpoxyApplication epoxyApplication)
        {
            if (epoxyApplication == null)
            {
                return;
            }

            // 获取画胶图形中的点位集合
            List<EpoxyApplicationLocation[]> dispensePatternParasList = epoxyApplication.DispensePatternParas;
            this.chartControl1.Series[0].Points.Clear();

            foreach (EpoxyApplicationLocation[] epoxyApplicationLocationArr in dispensePatternParasList)
            {
                for (int j = 0; j < epoxyApplicationLocationArr.Length; j++)
                {
                    this.chartControl1.Series[0].Points.Add(
                        new SeriesPoint(
                            epoxyApplicationLocationArr[j].X * 1000,
                            epoxyApplicationLocationArr[j].Y * 1000));

                    if (j == epoxyApplicationLocationArr.Length - 1)
                    {
                        this.chartControl1.Series[0].Points.Add(
                            new SeriesPoint(
                                epoxyApplicationLocationArr[j].X * 1000,
                                double.NaN));
                    }
                }
            }
        }
    }
}
