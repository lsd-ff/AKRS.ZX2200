namespace AKRS.ZX2200.WaferSubSystem.Models.DeviceParams
{
    using System;
    using System.Linq;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBoxGeo;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using Newtonsoft.Json;
    using PropertyChanged;

    /// <summary>
    /// MagazineBox模组设备参数
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class MagazineBoxDevicePara : PropertyChangeAop
    {
        #region Delay

        /// <summary>
        /// 推料气缸动作后延时
        /// </summary>
        [TreeProgramListArgs("推料气缸动作后延时", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayPushCycActionAfter { get; set; } = 10;

        /// <summary>
        /// 推料保持时间
        /// </summary>
        [TreeProgramListArgs("推料保持时间", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayPushHold { get; set; } = 10;

        #endregion

        #region MagazineBox模组设备参数

        /// <summary>
        /// MagazineBox当前属于哪一层（当前层号）
        /// </summary>
        public int CurrentLayerNo { get; set; } = 0;

        /// <summary>
        /// 是否需要重新创建magazine状态
        /// </summary>
        [TreeProgramListArgs("是否需要重新创建magazine状态", "上料模组设备参数", false)]
        public bool IsNeedReCreateState { get; set; } = false;

        /// <summary>
        /// 是否手动更换料片（相对于晶圆台内的料片）
        /// </summary>
        [TreeProgramListArgs("是否使用magazine", "上料模组设备参数")]
        public bool IsUseMagazineLift { get; set; } = false;

        /// <summary>
        /// 晶圆夹取料与放料时magazineLift的位置差
        /// </summary>
        [TreeProgramListArgs("晶圆夹取料与放料时magazineLift的位置差", "上料模组设备参数", UnitHelper.mm)]
        public double MagazineLiftDistanceWithClamp { get; set; }

        /// <summary>
        /// 安全位
        /// </summary> 
        [TreeProgramListArgs("安全位", "上料模组设备参数", UnitHelper.mm)]
        public AKRSPoint3D SafePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 基准位
        /// </summary> 
        [TreeProgramListArgs("基准位", "上料模组设备参数", UnitHelper.mm)]
        public AKRSPoint3D MarkPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// MagazineBox层Position
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D[] Position => this.GetPosition();

        /// <summary>
        /// 获取MagazineBox层Position
        /// </summary>
        /// <returns>result</returns>
        private AKRSPoint3D[] GetPosition()
        {
            MagazineBoxConfig box = WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig;

            if (box == null)
            {
                return null;
            }

            AKRSPoint3D[] position = new AKRSPoint3D[box.MagazineBoxGeo.LayerCount];
            for (int i = 0; i < box.MagazineBoxGeo.LayerCount; i++)
            {
                position[i] = new AKRSPoint3D();
                position[i].Z = Math.Round(this.MarkPosition.Z - box.MagazineBoxGeo.PositionOfLowestSlot - box.MagazineBoxGeo.SlotPitch * i, 10);
            }

            if (box.Direction == MagazineDirectionEnum.TopToDrow)
            {
                AKRSPoint3D[] temp = new AKRSPoint3D[box.MagazineBoxGeo.LayerCount];
                for (int i = 0; i < box.MagazineBoxGeo.LayerCount; i++)
                {
                    temp[i] = position[box.MagazineBoxGeo.LayerCount - i - 1];
                }

                position = temp;
            }

            return position;
        }

        /// <summary>
        /// 置位需要重新创建状态信号
        /// </summary>
        public void SetNeedReCreateStateSignal()
        {
            this.IsNeedReCreateState = true;
            WaferSubDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 复位需要重新创建状态信号
        /// </summary>
        public void ResetNeedReCreateStateSignal()
        {
            this.IsNeedReCreateState = false;
            WaferSubDevicePara.GetInstance().Save();
        }

        #endregion
    }
}
