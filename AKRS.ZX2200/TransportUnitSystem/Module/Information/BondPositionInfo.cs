using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.TransportUnitSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Information
{
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using DevExpress.Office.Utils;
    using LanguageExt.TypeClasses;

    /// <summary>
    ///  焊点信息
    /// </summary>
    public class BondPositionInfo : BaseInformation
    {
        /// <summary>
        ///  上视定位结果第一次
        /// </summary>
        public MatchResult UpLookMatchResult1 { get; set; } = new MatchResult();

        /// <summary>
        ///  上视定位结果第二次
        /// </summary>
        public MatchResult UpLookMatchResult2 { get; set; } = new MatchResult();

        /// <summary>
        /// 焊点定位之后的真实位置
        /// </summary>
        public AKRSPoint3D RealBondPoint3D { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 实际贴片位
        /// </summary>
        public AKRSPoint3D RealBondPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 温漂Mark补偿
        /// </summary>
        public AKRSPoint3D TpMarkCompensate { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 焊后补偿
        /// </summary>
        public AKRSPoint3D DefectCompensate { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 中转台拍照位置
        /// </summary>
        public AKRSPoint3D IPTVisionPos { get; set; }

        /// <summary>
        /// 中转台P2拍照位置
        /// </summary>
        public AKRSPoint3D IPTP2VisionPos { get; set; }

        /// <summary>
        /// 中转台取片位置
        /// </summary>
        public AKRSPoint3D IPTPickPos { get; set; }

        /// <summary>
        /// 芯片在晶圆/华府盒上的角度
        /// </summary>
        public double ComponentAngleOnWafer { get; set; } = 0;

        /// <summary>
        /// 最终取片力值
        /// </summary>
        public double ActualPickForce { get; set; } = 0;

        /// <summary>
        /// 最终贴片力值
        /// </summary>
        public double ActualBondForce { get; set; } = 0;

        /// <summary>
        /// 芯片二维码
        /// </summary>
        public string ComponentCode { get; set; } = string.Empty;

        #region 视觉矫正的过程量

        /// <summary>
        /// 在贴片时，焊头还需要旋转的角度
        /// </summary>
        public double ComponentAngleOffSet { get; set; } = 0;

        /// <summary>
        /// 吸嘴上的芯片此时距离旋转中心的偏移值
        /// </summary>
        public AKRSPoint3D ComponentOffSet { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 中转台的补偿
        /// </summary>
        public AKRSPoint3D SubstrateCameraCompensate { get; set; } = new AKRSPoint3D();

        #endregion

        /// <summary>
        /// 系统1剩余工作步骤
        /// </summary>
        public Dictionary<string, bool> S1RemainingSteps { get; set; } = new Dictionary<string, bool>();

        /// <summary>
        /// 系统2剩余工作步骤
        /// </summary>
        public Dictionary<string, bool> S2RemainingSteps { get; set; } = new Dictionary<string, bool>();

        /// <summary>
        /// 系统1是否完成
        /// </summary>
        /// <returns>结果</returns>
        public bool IsFinishedInSystem1()
        {
            if (this.S1RemainingSteps.Count == 0)
            {
                return true;
            }

            if (this.S2RemainingSteps.Count == 0)
            {
                return false;
            }

            foreach (var item in this.S1RemainingSteps)
            {
                if (!item.Value)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 系统2是否完成
        /// </summary>
        /// <returns>结果</returns>
        public bool IsFinishedInSystem2()
        {
            if (this.S2RemainingSteps.Count == 0)
            {
                return true;
            }

            foreach (var item in this.S2RemainingSteps)
            {
                if (!item.Value)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 设置系统1完成
        /// </summary>
        public void SetFinishedInSystem1()
        {
            List<string> steps = new List<string>();
            foreach (var item in this.S1RemainingSteps)
            {
                steps.Add(item.Key);
            }

            foreach (var item in steps)
            {
                this.S1RemainingSteps[item] = true;
            }
        }

        /// <summary>
        /// 设置系统1未完成
        /// </summary>
        public void SetUnFinishedInSystem1()
        {
            List<string> steps = new List<string>();
            foreach (var item in this.S1RemainingSteps)
            {
                steps.Add(item.Key);
            }

            foreach (var item in steps)
            {
                this.S1RemainingSteps[item] = false;
            }
        }

        /// <summary>
        /// 设置系统2完成
        /// </summary>
        public void SetFinishedInSystem2()
        {
            List<string> steps = new List<string>();
            foreach (var item in this.S2RemainingSteps)
            {
                steps.Add(item.Key);
            }

            foreach (var item in steps)
            {
                this.S2RemainingSteps[item] = true;
            }
        }

        /// <summary>
        /// 设置系统2未完成
        /// </summary>
        public void SetUnFinishedInSystem2()
        {
            List<string> steps = new List<string>();
            foreach (var item in this.S2RemainingSteps)
            {
                steps.Add(item.Key);
            }

            foreach (var item in steps)
            {
                this.S2RemainingSteps[item] = false;
            }
        }
    }
}
