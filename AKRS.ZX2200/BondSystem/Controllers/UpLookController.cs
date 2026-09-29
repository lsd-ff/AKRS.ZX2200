using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Modules;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    /// <summary>
    /// 上视控制器
    /// </summary>
    public class UpLookController
    {
        /// <summary>
        /// 上视模组
        /// </summary>
        private UpLookModule upLookModule => System2Module.GetInstance().UpLookModule;

        /// <summary>
        /// 获取相机硬件
        /// </summary>
        /// <returns>硬件集合</returns>
        public PRHardware GetHardware()
        {
            return this.upLookModule.GetHardware();
        }

        /// <summary>
        /// 相机设置硬件
        /// </summary>
        /// <param name="pRName">模版名称</param>
        public void SetHardware(string pRName)
        {
            this.upLookModule.SetHardware(pRName);
        }

        /// <summary>
        /// 将Uplook相机的结果转换成G0中的位置
        /// </summary>
        /// <param name="visionPos">轴的真实坐标</param>
        /// <param name="result">定位的结果</param>
        /// <returns>结果</returns>
        public AKRSPoint3D ConvertPixelToG0Pos(AKRSPoint3D visionPos, MatchResult result)
        {
            return this.upLookModule.ConvertPixelToG0Pos(visionPos, result);
        }

        /// <summary>
        /// 上视相机的定位结果
        /// </summary>
        /// <param name="result">定位的结果</param>
        /// <returns>结果</returns>
        public AKRSPoint3D ConvertPixelToDistance(MatchResult result)
        {
            return this.upLookModule.ConvertPixelToDistance(result);
        }

        /// <summary>
        /// 关闭点光环光
        /// </summary>
        public void CloseLight()
        {
            this.upLookModule.SpotLight.SetIntensity(0);
            this.upLookModule.AmbientLight.SetIntensity(0);
        }
    }
}
