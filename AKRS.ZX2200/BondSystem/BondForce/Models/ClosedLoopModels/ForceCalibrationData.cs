using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AKRS.Galaxy2.Infrastructure.Helper;

using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 力闭环标定参数
    /// </summary>
    public class ForceCalibrationData : Singleton<ForceCalibrationData>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        static ForceCalibrationData()
        {
            Singleton<ForceCalibrationData>.FilePath = ZX2200PathConfig.ForceCalibrationStorageFilePath;
        }

        /// <summary>
        /// 大力值关系对象存储
        /// </summary>
        public List<ForceRelateAngleItem> LargeForceRelationList { get; set; } =
            new List<ForceRelateAngleItem>();

        /// <summary>
        /// 小力力值关系对象存储
        /// </summary>
        public List<ForceRelateAngleItem> SmallForceRelationList { get; set; } =
            new List<ForceRelateAngleItem>();

        /// <summary>
        ///  焊头力初始值字典
        ///  角度-力值
        /// </summary>
        public Dictionary<double, double> ForceInitialValDic { get; set; } =
            new Dictionary<double, double>();

        /// <summary>
        /// 标定耗时(分钟)
        /// </summary>
        public double ForceCaliCostTime { get; set; }

        /// <summary>
        /// 力控标定完成时间 
        /// </summary>
        public DateTime ForceCaliFinishTime { get; set; } = DateTime.Now;
    }
}
