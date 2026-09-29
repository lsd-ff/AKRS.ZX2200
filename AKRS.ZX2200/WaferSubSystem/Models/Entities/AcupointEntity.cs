using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using DevExpress.XtraPrinting.Native;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.Entities
{
    using AKRS.Galaxy2.Infrastructure.Helper;

    /// <summary>
    /// 华夫盒槽位
    /// </summary>
    [Serializable]
    public class AcupointEntity
    {
        /// <summary>
        /// 槽索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 槽号
        /// </summary>
        [JsonIgnore]
        public int SlotNum => this.Index + 1;

        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// 列索引
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// 槽位位置
        /// </summary>
        public AKRSPoint3D SlotPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 槽位状态
        /// </summary>
        public SlotStatuEnum SlotState { get; set; } = SlotStatuEnum.None;

        /// <summary>
        /// Bottom right corner of rect 
        /// </summary>
        [JsonIgnore]
        internal PointF SpecPos { get; set; } = new PointF();

        /// <summary>
        /// 矩形区域 
        /// </summary>
        [JsonIgnore]
        public RectangleF Rect { get; set; }

        /// <summary>
        ///  Get brush with state
        /// </summary>
        /// <returns>Brush</returns>
        internal (string Description, Brush brush) GetColorBrushDescription()
        {
            AcupointAttribute cb = (AcupointAttribute)EnumHelper.GetCustomAttributes<AcupointAttribute>(this.SlotState);
            Brush brush = new SolidBrush(Color.FromName(cb.StateColor));

            return (cb.StateDescription, brush);
        }
    }
}
