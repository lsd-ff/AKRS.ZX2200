using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
using AKRS.ZX2200.DispenseSystem.Models.Programs;
using AKRS.ZX2200.DispenseSystem.Models;
using DevExpress.XtraEditors;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.Models.Programs
{
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using System.Windows.Forms;

    /// <summary>
    /// 系统2预点胶板程式
    /// </summary>
    public class S2PreDispensePlateProgram
    {
        /// <summary>
        /// 可以点胶的区域
        /// </summary>
        public AKRSPoint3D[,] EpoxyArea { get; set; }

        /// <summary>
        /// 预点胶板的设备参数
        /// </summary>
        [JsonIgnore]
        public S2PreDispensePlatePara S2PreDispensePlatePara => BondDevicePara.GetInstance().S2DispenseDevicePara.S2PreDispensePlatePara;

        /// <summary>
        /// 点胶头参数
        /// </summary>
        [JsonIgnore]
        public S2DispenserProgram S2DispenserProgram => System2Domain.GetInstance().BondProgram.S2DispenserProgram;

        /// <summary>
        /// 索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 初始化
        /// 工艺需要更改行列数之后就可以使用
        /// 到时候调用这个方法
        /// </summary>
        public void InitPre()
        {
            this.EpoxyArea = new AKRSPoint3D[this.S2PreDispensePlatePara.Rows, this.S2PreDispensePlatePara.Columns];

            double columnSpacing = 0;

            double rowSpacing = 0;

            if (this.S2PreDispensePlatePara.Columns > 1)
            {
                // 计算行列间距
                columnSpacing = (this.S2PreDispensePlatePara.PreDispensePlateEndPos.X
                                 - this.S2PreDispensePlatePara.PreDispensePlateStartPos.X)
                                / (this.S2PreDispensePlatePara.Columns - 1);
            }

            if (this.S2PreDispensePlatePara.Rows > 1)
            {
                // 计算行列间距
                rowSpacing = (this.S2PreDispensePlatePara.PreDispensePlateEndPos.Y
                              - this.S2PreDispensePlatePara.PreDispensePlateStartPos.Y)
                             / (this.S2PreDispensePlatePara.Rows - 1);
            }

            for (int i = 0; i < this.S2PreDispensePlatePara.Rows; i++)
            {
                for (int j = 0; j < this.S2PreDispensePlatePara.Columns; j++)
                {
                    this.EpoxyArea[i, j] = new AKRSPoint3D(
                        this.S2PreDispensePlatePara.PreDispensePlateStartPos.X + j * columnSpacing,
                        this.S2PreDispensePlatePara.PreDispensePlateStartPos.Y + i * rowSpacing,
                        this.S2PreDispensePlatePara.PreDispensePlateStartPos.Z);
                }
            }

            this.Index = 0;
            System1Domain.GetInstance().System1Program.Save();
        }

        /// <summary>
        /// 获取画胶的点位，此点位为G0中的坐标
        /// </summary>
        /// <returns>需要画胶的点位</returns>
        public AKRSPoint3D GetEpoxy()
        {
            if (this.EpoxyArea == null || this.EpoxyArea.Length == 0)
            {
                this.InitPre();
            }

            if (this.Index == this.EpoxyArea.Length)
            {
               DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"预点胶板位置已经使用完，请清洁预点胶板 \r\n",
                    "预点胶板未清洁报警",
                    new string[] { "确认", "停止" },
                    new DialogResult[] { DialogResult.OK, DialogResult.Abort },
                    AlarmLevel.SecondLevel);

               if (dialogResult == DialogResult.OK)
               {
                   this.InitPre();
               }
               else
               {
                   return null;
               }
            }

            // 这个是判断外界行列数有没有改变
            if (this.S2PreDispensePlatePara.Rows != this.EpoxyArea.GetLength(0)
                || this.S2PreDispensePlatePara.Columns != this.EpoxyArea.GetLength(1))
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $" 预点胶板行列数发生改变，请清洁预点胶板 \r\n",
                    " 预点胶板行列数发生改变报警",
                    new string[] { "确认", "停止" },
                    new DialogResult[] { DialogResult.OK, DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                if (dialogResult == DialogResult.OK)
                {
                    this.InitPre();
                }
                else
                {
                    return null;
                }
            }

            int column = 0;
            int row = 0;

            if (this.S2PreDispensePlatePara.Rows != 0 && this.S2PreDispensePlatePara.Columns != 0)
            {
                column = this.Index / this.S2PreDispensePlatePara.Rows;
                row = this.Index - column * this.S2PreDispensePlatePara.Rows;
            }

            this.Index++;

            AKRSPoint3D point3D = this.EpoxyArea[row, column];

            System1Domain.GetInstance().System1Program.Save();

            return point3D;
        }


        /// <summary>
        /// 获取画胶的点位，此点位为G0中的坐标
        /// </summary>
        /// <param name="count">个数</param>
        /// <returns>画胶点位的集合</returns>
        public List<AKRSPoint3D> GetEpoxy(int count)
        {
            List<AKRSPoint3D> points = new List<AKRSPoint3D>();

            for (int i = 0; i < count; i++)
            {
                AKRSPoint3D point3D = this.GetEpoxy();

                if (point3D == null)
                {
                    return null;
                }

                points.Add(point3D);
            }

            return points;
        }


        /// <summary>
        /// 预点胶板是否准备好
        /// </summary>
        /// <returns>结果</returns>
        public bool IsReady()
        {
            return this.S2PreDispensePlatePara.AssistanceFinish;
        }
    }
}
