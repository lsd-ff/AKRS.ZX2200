using System.Collections.Generic;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Modules
{
    /// <summary>
    /// 上视模组
    /// </summary>
    public class UpLookModule
    {
        /// <summary>
        /// 上视相机
        /// </summary>
        [JsonIgnore]
        public AKRSCamera UpLookCamera => HardwareRepositoryService.GetHardware<AKRSCamera>("上视相机");

        /// <summary>
        /// 上视相机环光
        /// </summary>
        [JsonIgnore]
        public Light AmbientLight => HardwareRepositoryService.GetHardware<Light>("上视环光");

        /// <summary>
        /// 上视相机点光
        /// </summary>
        [JsonIgnore]
        public Light SpotLight => HardwareRepositoryService.GetHardware<Light>("上视点光");

        /// <summary>
        /// 相机硬件
        /// </summary>
        public PRHardware Hardware;

        /// <summary>
        /// 固晶域Uplook相机的坐标系
        /// </summary>
        [JsonIgnore]
        public DependentCoordinateSystem UpLookCameraCoordinateSystem =>
            (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "UpLookCameraCoordinateSystem");

        /// <summary>
        /// 将Uplook相机的结果转换成G0中的位置
        /// </summary>
        /// <param name="visionPos">轴的真实坐标</param>
        /// <param name="result">定位的结果</param>
        /// <returns>结果</returns>
        public AKRSPoint3D ConvertPixelToG0Pos(AKRSPoint3D visionPos, MatchResult result)
        {
            return this.UpLookCameraCoordinateSystem.SelfPosToG0(
                new AKRSPoint3D(result.CenterX, result.CenterY, 0),
                visionPos);
        }

        /// <summary>
        /// 上视相机的定位结果
        /// </summary>
        /// <param name="result">定位的结果</param>
        /// <returns>结果</returns>
        public AKRSPoint3D ConvertPixelToDistance(MatchResult result)
        {
            return this.UpLookCameraCoordinateSystem.ForwardConvertCoordinate(
                new AKRSPoint3D(result.CenterX, result.CenterY, 0)
             );
        }

        /// <summary>
        /// 获取相机硬件
        /// </summary>
        /// <returns>硬件集合</returns>
        public PRHardware GetHardware()
        {
            List<string> list;

            list = new List<string>
                       {
                           this.SpotLight.HardwareName,
                           this.AmbientLight.HardwareName,
                       };

            this.Hardware = new PRHardware(
                this.UpLookCamera.HardwareName,
                System2Module.GetInstance().BondModule.BondAxisX.HardwareName,
                System2Module.GetInstance().BondModule.BondAxisY.HardwareName,
                System2Module.GetInstance().BondModule.BondHead.AxisZ.HardwareName,
                list);

            return this.Hardware;
        }

        /// <summary>
        /// 相机设置硬件
        /// </summary>
        /// <param name="pRName">模版名称</param>
        public void SetHardware(string pRName)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(pRName);

            List<string> pRLights = new List<string>();

            pRLights.Add(this.SpotLight.HardwareName);
            pRLights.Add(this.AmbientLight.HardwareName);

            pREntity.SetHardware(this.GetHardware());
        }

        /// <summary>
        /// 获取灯光集合
        /// </summary>
        /// <returns>结果</returns>
        public List<Light> GetLights()
        {
            return new List<Light>() { this.SpotLight, this.AmbientLight };
        }
    }
}
