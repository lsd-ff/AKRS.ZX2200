using DevExpress.XtraEditors;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate
{
    /// <summary>
    /// 绘制温漂图
    /// </summary>
    public partial class FrmTemperatureDistribution : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 绘制温漂图
        /// </summary>
        public FrmTemperatureDistribution()
        {
            this.InitializeComponent();
            this.MouseWheel += FrmMain_MouseWheel;
        }

        private List<PointF[,]> data;

        private int max;

        private int index = 0;

        /// <summary>
        /// 画笔
        /// </summary>
        private Graphics graphics;

        /// <summary>
        /// 重新绘画
        /// </summary>
        private void RefreshPaint()
        {

        }

        private void FrmMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 && this.index < max)
            {
                this.index++;
            }
            else if (e.Delta < 0)
            {
                if (this.index > 0)
                {
                    this.index--;
                }
            }

            this.PlDraw.Refresh();
        }



        /// <summary>
        /// 绘制圆
        /// </summary>
        private void PaintCycle()
        {
            double height = this.PlDraw.Height - 50;

            double length = this.PlDraw.Width - 50;

            double x = (length - 50) / 45.0;

            double y = (height) / 32.0;

            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 45.0; j++)
                {
                    int x1 = (int)(j * x) + 50;
                    int y1 = (int)(height - i * y);

                    int x2 = (int)(this.data[this.index][i * 4, j * 4].X * 10.0);
                    int y2 = (int)(this.data[this.index][i * 4, j * 4].Y * 10.0);

                    Pen pen = new Pen(Color.Red);
                    SolidBrush brush = new SolidBrush(Color.YellowGreen);
                    Rectangle rect = new Rectangle(x1, y1, 10, 10);
                    this.graphics.FillEllipse(brush, rect); // 填充圆
                    this.graphics.DrawEllipse(pen, rect); // 绘制圆边框

                    // 创建画笔并设置箭头样式
                    Pen arrowPen = new Pen(Color.Blue, 2);
                    arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

                    // 绘制带箭头的直线
                    this.graphics.DrawLine(arrowPen, new Point(x1 + 5, y1 + 5), new Point(x1 + x2 + 5, y1 + 5 + y2));
                }
            }
        }

        private void PlDraw_Paint(object sender, PaintEventArgs e)
        {
            this.graphics = e.Graphics;
            this.PaintCycle();
        }

        private void FrmTemperatureDistribution_Load(object sender, EventArgs e)
        {
            data = new List<PointF[,]>();

            DirectoryInfo directoryInfo = new DirectoryInfo(@"E:\2025年\8月");

            List<List<(double, double)>> totalList = new List<List<(double, double)>>();

            // 遍历文件夹中的所有文件
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                List<(double, double)> list = new List<(double, double)>();
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                    // 读取数据WW
                    for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                    {
                        object cellValue1 = worksheet.Cells[row, 1].Value;
                        object cellValue2 = worksheet.Cells[row, 2].Value;

                        if (cellValue1 == null || cellValue2 == null)
                        {
                            continue;
                        }

                        list.Add(((double)cellValue1, (double)cellValue2));
                    }
                }

                totalList.Add(list);
            }

            for (int i = 0; i < totalList.Count; i++)
            {
                PointF[,] point = new PointF[130, 180];

                for (int j = 0; j < 130; j++)
                {
                    for (int k = 0; k < 180; k++)
                    {
                        point[j, k].X = (float)totalList[i][j * k + k].Item1 - (float)totalList[i][0].Item1;
                        point[j, k].Y = (float)totalList[i][j * k + k].Item2 - (float)totalList[i][0].Item2;
                    }
                }

                data.Add(point);
            }
            
            max = data.Count - 1;
        }
    }
}