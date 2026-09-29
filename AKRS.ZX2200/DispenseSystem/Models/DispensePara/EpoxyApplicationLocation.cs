using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using static AKRS.DispensingPattern.PointEx;

namespace AKRS.ZX2200.DispenseSystem.Models.DispensePara
{
    /// <summary>
    /// 画胶位置数据
    /// </summary>
     [Serializable]
    public class EpoxyApplicationLocation
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 点胶点的名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 点胶X位置
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 点胶Y位置
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 点胶高度
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// 提前关胶
        /// </summary>
        public int AdvanceClose { get; set; }

        /// <summary>
        /// 滞后开胶
        /// </summary>
        public int LagOpen { get; set; }

        /// <summary>
        /// 到下一段是否开胶
        /// </summary>
        public bool OpenGlueFlag { get; set; }
        
        /// <summary>
        /// 到下一段的加速度
        /// </summary>
        public double Acc { get; set; }

        /// <summary>
        /// 到下一段的减速度
        /// </summary>
        public double Dec { get; set; }

        /// <summary>
        /// 画胶类型
        /// </summary>
        public EpoxyApplicationLocationType Type { get; set; }

        /// <summary>
        /// 到下一段的速度
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// 结束
        /// </summary>
        public double EndSpeed { get; set; }

        /// <summary>
        /// 滞留时间
        /// </summary>
        public int DripTime { get; set; }

        /// <summary>
        /// 高度补偿
        /// </summary>
        public double AltitudeCompensation { get; set; }
    }

    /// <summary>
    /// 线段类型
    /// </summary>
    public enum EpoxyApplicationLocationType
    {
        /// <summary>
        /// 点胶
        /// </summary>
        Dispense,

        /// <summary>
        /// 画胶
        /// </summary>
        Paint
    }
}
