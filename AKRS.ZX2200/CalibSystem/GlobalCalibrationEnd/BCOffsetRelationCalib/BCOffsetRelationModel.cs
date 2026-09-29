#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/7/30 10:23:39
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

namespace AKRS.ZX2200.CalibSystem.GlobalCalibrationEnd.BCOffsetRelationCalib
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 标定对象
    /// </summary>
    public class BCOffsetRelationCalibration : Singleton<BCOffsetRelationCalibration>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static BCOffsetRelationCalibration()
        {
            FilePath = ZX2200PathConfig.BCOffsetRelationCalibration;
        }

        /// <summary>
        /// 标定关系集合 (y，(x, o))
        /// </summary> 
        public SortedList<double, SortedList<double, BCOffsetRelationModel>> BCOffsetRelations { get; set; } =
            new SortedList<double, SortedList<double, BCOffsetRelationModel>>();

        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this.BCOffsetRelations.Clear();
        }

        /// <summary>
        /// ΔX,  ΔY
        /// </summary>
        /// <param name="xPos">轴X坐标</param>
        /// <param name="yPos">轴Y坐标</param>
        /// <returns>结果</returns>
        public (double ΔX, double ΔY) GetΔXY(double xPos, double yPos)
        {
            SortedList<double, BCOffsetRelationModel> preValue = null;
            SortedList<double, BCOffsetRelationModel> nextValue = null;

            if (this.BCOffsetRelations.Capacity <= 1)
            {
                return (0, 0);
            }

            // 选获取相邻的两行标定数据
            double yOffset1 = 0;
            double yOffset2 = 0;
            foreach (var kv in this.BCOffsetRelations)
            {
                if (yPos > kv.Key)
                {
                    preValue = kv.Value;
                    yOffset1 = Math.Abs(yPos - kv.Key);
                }
                else if (yPos <= kv.Key)
                {
                    nextValue = kv.Value;
                    yOffset2 = Math.Abs(yPos - kv.Key);
                    break;
                }
            }

            (double ΔX, double ΔY) ret = (0,0);
            if (preValue != null && nextValue != null)
            {
                // 看哪个近点
                if (yOffset1 < yOffset2)
                {
                    ret = this.GetΔXYByRowData(xPos, preValue);
                }
                else
                {
                    ret = this.GetΔXYByRowData(xPos, nextValue);
                }
            }

            return ret;
        }

        /// <summary>
        /// ΔX,  ΔY 根据行数据计算
        /// </summary>
        /// <param name="xPos">轴X坐标</param>
        /// <param name="bCOffsetRelationsRow">一行标定数据</param>
        /// <returns>结果</returns>
        private (double ΔX, double ΔY) GetΔXYByRowData(double xPos, SortedList<double, BCOffsetRelationModel> bCOffsetRelationsRow)
        {
            double ΔX = 0;
            double ΔY = 0;

            BCOffsetRelationModel preValue = null;
            BCOffsetRelationModel nextValue = null;

            if (bCOffsetRelationsRow.Capacity <= 1)
            {
                return (ΔX, ΔY);
            }

            foreach (var kv in bCOffsetRelationsRow)
            {
                if (xPos > kv.Key)
                {
                    preValue = kv.Value;
                }
                else if (xPos <= kv.Key)
                {
                    nextValue = kv.Value;
                    break;
                }
            }

            // 前面的没补偿到  采取第一个补偿
            if (preValue == null)
            {
                preValue = bCOffsetRelationsRow.Values[0];
                nextValue = bCOffsetRelationsRow.Values[1];
            }

            if (preValue != null && nextValue != null)
            {
                ΔX = this.GenLineEqu(preValue.Pos.X, preValue.ΔX, nextValue.Pos.X, nextValue.ΔX, xPos);
                ΔY = this.GenLineEqu(preValue.Pos.X, preValue.ΔY, nextValue.Pos.X, nextValue.ΔY, xPos);

                if (ΔX > 1 || ΔY > 1)
                {
                    AKRSXtraMessageBox.Show($"拟合获取 ΔX，ΔY 异常  \r\nΔX:{ΔX} \r\nΔY:{ΔY}！", "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return (0, 0);
                }
                else
                {
                    return (ΔX, ΔY);
                }
            }
            else
            {
                return (0, 0);
            }
        }

        /// <summary>
        /// 两点获取直线参数
        /// k为斜率，kf为法线斜率
        /// </summary>
        /// <param name="x1">点1 x</param>
        /// <param name="y1">点1 y</param>
        /// <param name="x2">点2 x</param>
        /// <param name="y2">点2 y</param>
        /// <param name="xPos">x 位置</param>
        /// <returns> 结果 </returns>
        public double GenLineEqu(double x1, double y1, double x2, double y2, double xPos)
        {
            double dx = x1 - x2;
            double dy = y1 - y2;
            double k = dy / dx;
            double b = y1 - k * x1;

            return k * xPos + b;
        }
    }

    /// <summary>
        /// 描述：标定关系模型
        /// </summary>
    public class BCOffsetRelationModel
    {
        /// <summary>
        /// Bond XY位置
        /// </summary>
        public AKRSPoint2D Pos { get; set; }

        /// <summary>
        /// 偏移量X
        /// </summary>
        public double ΔX { get; set; } 

        /// <summary>
        /// 偏移量X
        /// </summary>
        public double ΔY { get; set; }
    }
}
