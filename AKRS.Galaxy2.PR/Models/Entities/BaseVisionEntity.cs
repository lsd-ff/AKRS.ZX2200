


#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/8/30 9:31:43
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
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.ModulePublic.ModuleInterface;
using AKRS.Galaxy2.PR.Models.MatchResults;
using Newtonsoft.Json;
using static AKRS.Galaxy2.PR.Models.Entities.PREntity;

namespace AKRS.Galaxy2.PR.Models.Entities
{
    /// <summary>
    /// 描述： 视觉实体基类 集成于模块实体基类
    /// </summary>
    [Serializable]
    public abstract class BaseVisionEntity : BaseModuleEntity
    {
        /// <summary>
        /// 相机ID
        /// </summary>
        public string CameraName { get; set; }

        /// <summary>
        /// PR绑定的相机
        /// </summary>
        [JsonIgnore]
        public AKRSCamera Camera => HardwareRepositoryService.GetHardware<AKRSCamera>(this.CameraName);

        /// <summary>
        /// X轴名称
        /// </summary>
        public string AxisXName { get; set; }

        /// <summary>
        /// X轴
        /// </summary>
        [JsonIgnore]
        public Axis AxisX => HardwareRepositoryService.GetHardware<Axis>(this.AxisXName);

        /// <summary>
        /// Y轴名称
        /// </summary>    
        public string AxisYName { get; set; }

        /// <summary>
        /// Y轴
        /// </summary>
        [JsonIgnore]
        public Axis AxisY => HardwareRepositoryService.GetHardware<Axis>(this.AxisYName);

        /// <summary>
        /// Z轴名称
        /// </summary>
        public string AxisZName { get; set; }

        /// <summary>
        /// Y轴
        /// </summary>
        [JsonIgnore]
        public Axis AxisZ => HardwareRepositoryService.GetHardware<Axis>(this.AxisZName);

        /// <summary>
        /// 相机曝光
        /// </summary>
        public double Exposure { get; set; } = 500;

        /// <summary>
        /// 相机增益
        /// </summary>
        public double Gain { get; set; } = 1;

        /// <summary>
        /// 相机gamma
        /// </summary>
        public double Gamma { get; set; } = 40;


        /// <summary>
        /// 光源是否频闪
        /// </summary>
        public bool IsFlash { get; set; }


        /// <summary>
        /// 是否保存原图
        /// </summary>
        public bool IsSaveImage { get; set; } = false;


        /// <summary>
        /// 模组关联的光源集合
        /// 默认给6个光源
        /// </summary>
        public List<PRLight> PRLightList { get; set; } = new List<PRLight>();

        /// <summary>
        /// 算法结果
        /// </summary>
        [JsonIgnore]
        public List<BaseAlgResult> AlgResults;

        ///// <summary>
        ///// 2026 07 31 日改
        ///// 防止内存泄露
        ///// </summary>
        //[JsonIgnore]
        //public List<BaseAlgResult> AlgResults
        //{
        //    get => _algResults;
        //    set
        //    {
        //        // 如果引用不同，释放旧的算法结果中的图像资源，防止内存泄漏
        //        if (!ReferenceEquals(_algResults, value) && _algResults != null)
        //        {
        //            foreach (var r in _algResults)
        //            {
        //                if (r == null) continue;

        //                try
        //                {
        //                    if (r.OutPutImg1 != null)
        //                    {
        //                        r.OutPutImg1.Dispose();
        //                        r.OutPutImg1 = null;
        //                    }
        //                }
        //                catch { }

        //                try
        //                {
        //                    if (r.OutPutImg != null)
        //                    {
        //                        if (r.OutPutImg is IDisposable d)
        //                        {
        //                            d.Dispose();
        //                        }
        //                        r.OutPutImg = null;
        //                    }
        //                }
        //                catch { }
        //            }
        //            _algResults.Clear();
        //        }

        //        _algResults = value;
        //    }
        //}

        /// <summary>
        /// 获取第一个PR结果
        /// </summary>
        [JsonIgnore]
        public BaseAlgResult AlgResult => AlgResults == null || AlgResults.Count == 0 ? null : AlgResults[0];

        /// <summary>
        /// 执行
        /// </summary>
        /// <returns>执行结果</returns>
        public abstract ExcuteResult DoWork(PRHardware pRHardware = null,bool isSetLight = true, bool postImage = true);
    }
}
