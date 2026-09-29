using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.SupportFeature.Parameters;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using PropertyChanged;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    /// <summary>
    /// 吸嘴架设备参数，都是关于G0
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class NozzleShelfParam : PropertyChangeAop
    {
        /// <summary>
        /// 吸嘴槽数量
        /// </summary>
        [TreeProgramListArgs("吸嘴槽数量", "", true, "个", RoleEnum.Admin)]
        public int SlotNum { get; set; } = 7;

        /// <summary>
        /// 吸嘴架换吸嘴位
        /// </summary>
        [TreeProgramListArgs("吸嘴架换吸嘴位", "吸嘴架", UnitHelper.mm, RoleEnum.Admin)]
        public AKRSPoint3D ToolChangePosition { get; set; }

        /// <summary>
        /// 换吸嘴Y向二段速距离
        /// </summary>
        [TreeProgramListArgs("换吸嘴Y向二段速距离", "吸嘴架", 1, 50, UnitHelper.mm, RoleEnum.Admin)]
        public double ChangeToolDistanceY { get; set; } = 30;

        /// <summary>
        /// 换吸嘴Z向二段速距离
        /// </summary>
        [TreeProgramListArgs("换吸嘴Z向二段速距离", "吸嘴架", 1, 50, UnitHelper.mm, RoleEnum.Admin)]
        public double ChangeToolDistanceZ { get; set; }

        /// <summary>
        /// 吸嘴进入槽位慢速速度
        /// </summary>
        [TreeProgramListArgs("吸嘴进入槽位慢速速度(Y向)", "吸嘴架", 0, 200, UnitHelper.speed, RoleEnum.Admin)]
        public double SlowTravelToSlotSpeed { get; set; } = 5;

        /// <summary>
        /// 吸嘴离开槽位慢速速度
        /// </summary>
        [TreeProgramListArgs("吸嘴离开槽位慢速速度(Y向)", "吸嘴架", 0, 200, UnitHelper.speed, RoleEnum.Admin)]
        public double SlowTravelAwayFromSlotSpeed { get; set; } = 5;

        /// <summary>
        /// 吸嘴慢速下压速度
        /// </summary>
        [TreeProgramListArgs("吸嘴慢速下压速度(Z向)", "吸嘴架", 0, 250, UnitHelper.speed, RoleEnum.Admin)]
        public double SlowTravelDownSpeed { get; set; } = 5;

        /// <summary>
        /// 吸嘴慢速上抬速度
        /// </summary>
        [TreeProgramListArgs("吸嘴慢速上抬速度(Z向)", "吸嘴架", 0, 250, UnitHelper.speed, RoleEnum.Admin)]
        public double SlowTravelUpSpeed { get; set; } = 5;

        /// <summary>
        /// 吸嘴架换吸嘴位角度,暂时不用统一的角度，改成一个槽位对应一个角度
        /// </summary>
        public double ToolChangePositionTheta { get; set; }

        /// <summary>
        /// 吸嘴槽间距
        /// </summary>
        [TreeProgramListArgs("吸嘴槽间距", "吸嘴架", false, UnitHelper.mm, RoleEnum.Admin)]
        public double SlotPitch { get; set; }

        /// <summary>
        /// 吸嘴归还位置
        /// </summary>
        [TreeProgramListArgs("吸嘴归还位置", (TreeGroupChildNodesEnum[])null)]
        public AKRSPoint3D[] PlaceNozzlePos { get; set; } = new AKRSPoint3D[7]
                                                                 {
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                 };

        /// <summary>
        /// 还吸嘴角度
        /// </summary>
        [TreeProgramListArgs("还吸嘴角度", (TreeGroupChildNodesEnum[])null, UnitHelper.degree, RoleEnum.Admin)]
        public double[] PlaceNozzleAngle { get; set; } = new double[7];

        /// <summary>
        /// 取吸嘴位置
        /// </summary>
        [TreeProgramListArgs("取吸嘴位置", (TreeGroupChildNodesEnum[])null, UnitHelper.mm, RoleEnum.Admin)]
        public AKRSPoint3D[] PickNozzlePos { get; set; } = new AKRSPoint3D[7]
                                                                 {
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                     new AKRSPoint3D(),
                                                                 };

        /// <summary>
        /// 取吸嘴角度
        /// </summary>
        [TreeProgramListArgs("取吸嘴角度", (TreeGroupChildNodesEnum[])null, UnitHelper.degree, RoleEnum.Admin)]
        public double[] PickNozzleAngle { get; set; } = new double[7];

        /// <summary>
        /// 吸嘴架标定治具几何数据
        /// todo:由机械给出，写死
        /// </summary>
        public AKRSPoint2D TCCaliToolGeometry { get; set; } = new AKRSPoint2D();

        /// <summary>
        /// 吸嘴架治具高度和吸嘴槽高度差值
        /// todo:试出来，写死
        /// </summary>
        [TreeProgramListArgs("吸嘴架治具高度和吸嘴槽高度差值", (TreeGroupChildNodesEnum[])null, UnitHelper.mm, RoleEnum.Admin)]
        public double TCCaliToolHeightOffset { get; set; } = 6.6126;

        /// <summary>
        /// 取放吸嘴位置高度偏差
        /// </summary>
        [TreeProgramListArgs("取放吸嘴位置高度偏差", (TreeGroupChildNodesEnum[])null, UnitHelper.mm, RoleEnum.Admin)]
        public double PlacePickPosHeightOffset { get; set; } = 0;

        /// <summary>
        /// 吸嘴架标定治具和焊头中心的偏移
        /// </summary>
        [TreeProgramListArgs("吸嘴架标定治具和焊头中心的偏移","",false, UnitHelper.mm, RoleEnum.Admin)]
        public AKRSPoint3D CaliToolToBondheadCenterOffset { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 槽1视觉位
        /// </summary>
        [TreeProgramListArgs("槽1视觉位", "", false, UnitHelper.mm, RoleEnum.Admin)]
        public AKRSPoint3D Slot1VisionPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 槽1视觉位
        /// </summary>
        [TreeProgramListArgs("槽7视觉位", "", false, UnitHelper.mm, RoleEnum.Admin)]
        public AKRSPoint3D Slot7VisionPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 槽1视觉位
        /// </summary>
        [TreeProgramListArgs("吸嘴架测高结果", "", false, UnitHelper.mm, RoleEnum.Admin)]
        public AKRSPoint3D NozzleShelfHeight { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 参数是否为空
        /// </summary>
        /// <returns>结果</returns>
        public bool IsEmpty()
        {
            foreach (var pos in this.PickNozzlePos)
            {
                if (pos.IsEmpty)
                {
                    return true;
                }
            }

            foreach (var pos in this.PlaceNozzlePos)
            {
                if (pos.IsEmpty)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
