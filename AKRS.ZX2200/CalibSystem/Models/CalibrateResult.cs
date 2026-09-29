using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.CalibSystem.Models
{
    /// <summary>
    /// 像素比类
    /// </summary>
    public class CalibCamScaleResult
    {
        /// <summary>
        /// 相机类型  Bond camere  uplook camere, wafer camere, dispense camera 用于显示  不做判断
        /// </summary>
        public string CameraType { get; set; }

        /// <summary>
        /// X方向像素比 mm/pixel
        /// </summary>
        public double CameraScaleX { get; set; }

        /// <summary>
        /// Y方向像素比  mm/pixel
        /// </summary>
        public double CameraScaleY { get; set; }

        /// <summary>
        /// 倾斜角度 像素坐标系的角度 ？？？
        /// </summary>
        public double CameraSlantTheata { get; set; }
    }

    /// <summary>
    /// 转换结果类
    /// </summary>
    public class CalibTransResult
    {
        /// <summary>
        /// 参数名称   显示用
        /// </summary>
        public string ParaName { get; set; }

        /// <summary>
        /// X相关
        /// </summary>
        public double RelativeX { get; set; }

        /// <summary>
        /// Y相关
        /// </summary>
        public double RelativeY { get; set; }

        /// <summary>
        /// 角度相关
        /// </summary>
        public double RelativeTheata { get; set; }
    }
}
