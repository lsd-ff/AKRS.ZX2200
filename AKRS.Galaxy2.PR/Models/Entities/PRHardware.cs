#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/11 18:17:43
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using Newtonsoft.Json;

namespace AKRS.Galaxy2.PR.Models.Entities
{
    /// <summary>
    /// 描述：
    /// </summary>
    /// <summary>
    /// PR硬件
    /// </summary>
    public class PRHardware
    {
        /// <summary>
        /// PR硬件初始化
        /// </summary>
        /// <param name="cameraName">相机名称</param>
        /// <param name="axisXName">X轴名称</param>
        /// <param name="axisYName">Y轴名称</param>
        /// <param name="axisZName">Z轴名称</param>
        /// <param name="lightName">光源名称列表</param>
        public PRHardware(string cameraName, string axisXName, string axisYName, string axisZName, List<string> lightName)
        {
            CameraName = cameraName;
            AxisXName = axisXName;
            AxisYName = axisYName;
            AxisZName = axisZName;
            LightName = lightName;
        }

        /// <summary>
        /// X轴名称
        /// </summary>
        public string AxisXName { get; set; }

        /// <summary>
        /// Y轴名称
        /// </summary>    
        public string AxisYName { get; set; }

        /// <summary>
        /// Z轴名称
        /// </summary>
        public string AxisZName { get; set; }

        /// <summary>
        /// 相机ID
        /// </summary>
        public string CameraName { get; set; }

        /// <summary>
        /// 光源名称集合
        /// </summary>
        public List<string> LightName { get; set; } = new List<string>();
    }
}
