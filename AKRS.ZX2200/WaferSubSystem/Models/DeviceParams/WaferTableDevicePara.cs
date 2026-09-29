using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Models.DeviceParams
{
    using System.IO;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using Newtonsoft.Json;
    using PropertyChanged;

    /// <summary>
    /// 晶圆台模组设备参数
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class WaferTableDevicePara : PropertyChangeAop
    {
        /// <summary>
        /// bond在上视模式下的避让晶圆相机的位置
        /// </summary>
        [TreeProgramListArgs("bond在上视模式下的避让晶圆相机的位置", "", UnitHelper.mm)]
        public AKRSPoint3D BondAvoidWaferCameraPositionWithUpLook { get; set; } = WaferSubController.GetInstance().WaferTableController.GetWaferCameraG0Position();

        /// <summary>
        /// bond在IPT模式下的避让晶圆相机的位置
        /// </summary>
        [TreeProgramListArgs("bond在IPT模式下的避让晶圆相机的位置", "", UnitHelper.mm)]
        public AKRSPoint3D BondAvoidWaferCameraPositionWithIPT { get; set; } = WaferSubController.GetInstance().WaferTableController.GetWaferCameraG0Position();

        /// <summary>
        /// bond在IPT模式下的避让晶圆相机的位置
        /// 这里Z轴默认是0位
        /// </summary>
        [TreeProgramListArgs("bond避让静态华夫盒的位置", "", UnitHelper.mm)]
        public AKRSPoint3D BondAvoidStaticWafflePos { get; set; } = new AKRSPoint3D(29, -99, 135);

        #region Delay

        /// <summary>
        /// 相机拍照延时
        /// </summary>
        [TreeProgramListArgs("相机拍照延时", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayPhoto { get; set; } = 100;

        /// <summary>
        /// 晶圆夹气缸动作后延时
        /// </summary>
        [TreeProgramListArgs("晶圆夹气缸动作后延时", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayClampCycActionAfter { get; set; } = 10;

        /// <summary>
        /// 晶圆夹持气缸动作后延时
        /// </summary>
        [TreeProgramListArgs("晶圆夹持气缸动作后延时", "延时", 0, 2000, UnitHelper.ms)]
        public int DelayBlockCycActionAfter { get; set; } = 10;

        /// <summary>
        /// 晶圆夹检测延时
        /// </summary>
        [TreeProgramListArgs("晶圆夹检测延时", "延时", 0, 20000, UnitHelper.ms)]
        public int DelayClampCheckSensor { get; set; } = 10000;

        /// <summary>
        /// 晶圆台检测延时
        /// </summary>
        [TreeProgramListArgs("晶圆台检测延时", "延时", 0, 20000, UnitHelper.ms)]
        public int DelayWaferTableSensor { get; set; } = 10000;

        #endregion

        #region Regrip

        /// <summary>
        /// 是否需要重复夹--取料时使用
        /// </summary>
        [TreeProgramListArgs("是否需要重复夹", "重复夹")]
        public bool IsNeedRegrip { get; set; }

        /// <summary>
        /// 第一次拉出来多少
        /// </summary>
        [TreeProgramListArgs("第一次拉出来多少", "重复夹", 0, 50, UnitHelper.mm)]
        public double FirstPullDistance { get; set; }

        /// <summary>
        /// 再进多少
        /// </summary>
        [TreeProgramListArgs("再进多少", "重复夹", 0, 50, UnitHelper.mm)]
        public double RegripDistance { get; set; }

        /// <summary>
        /// 重复夹速度百分比
        /// </summary>
        [TreeProgramListArgs("重复夹速度百分比", "重复夹", 1, 100, UnitHelper.percent)]
        public double RegripFeedrate { get; set; } = 100;

        #endregion

        #region SlowTravel

        /// <summary>
        /// 是否需要慢速夹--取料时使用
        /// </summary>
        [TreeProgramListArgs("是否需要慢速夹", "慢速夹")]
        public bool IsNeedSlowTravel { get; set; }

        /// <summary>
        /// 慢速距离
        /// </summary>
        [TreeProgramListArgs("慢速距离", "慢速夹", 0, 50, UnitHelper.mm)]
        public double SlowDistance { get; set; }

        /// <summary>
        /// 慢速速度百分比
        /// </summary>
        [TreeProgramListArgs("慢速速度百分比", "慢速夹", 1, 100, UnitHelper.percent)]
        public double SlowFeedrate { get; set; } = 20;

        #endregion

        #region 晶圆台模组设备参数

        /// <summary>
        /// 是否提前结束行列搜索
        /// </summary>
        [TreeProgramListArgs("是否提前结束行列搜索", "晶圆台模组设备参数")]
        public bool IsAheadReachEnd { get; set; } = false;

        /// <summary>
        /// 晶圆台速度百分比-非搜晶
        /// </summary>
        [TreeProgramListArgs("晶圆台速度百分比", "晶圆台模组设备参数", 1, 100, UnitHelper.percent)]
        public double WaferTableSpeedPercent { get; set; } = 20;

        /// <summary>
        /// 晶圆台手动换料位
        /// </summary>
        [TreeProgramListArgs("晶圆台手动换料位", "晶圆台模组设备参数", UnitHelper.mm)]
        public AKRSPoint3D ManualChangePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆台自动换料位
        /// </summary>
        [TreeProgramListArgs("晶圆台自动换料位", "晶圆台模组设备参数", UnitHelper.mm)]
        public AKRSPoint3D AutoChangePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 环限位中心坐标
        /// </summary>
        [TreeProgramListArgs("环限位中心坐标", "晶圆台模组设备参数", UnitHelper.mm)]
        public AKRSPoint3D WaferTableCenter { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 环限位半径
        /// </summary>
        [TreeProgramListArgs("环限位半径", "晶圆台模组设备参数", UnitHelper.mm)]
        public double WaferTableRadius { get; set; }

        /// <summary>
        /// 晶圆台准备位
        /// </summary>
        [TreeProgramListArgs("晶圆台准备位", "晶圆台模组设备参数", UnitHelper.mm)]
        public AKRSPoint3D ReadyPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆台扫码位
        /// </summary>
        [TreeProgramListArgs("晶圆台扫码位", "晶圆台模组设备参数", UnitHelper.mm)]
        public AKRSPoint3D ScanPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆夹槽深度
        /// </summary>
        [TreeProgramListArgs("晶圆夹槽深度", "晶圆更换", 0, 10, UnitHelper.mm)]
        public double WaferClampSlotDepth { get; set; } = 5;

        /// <summary>
        /// 晶圆夹在晶圆台内的补偿值
        /// </summary>
        [TreeProgramListArgs("晶圆夹在晶圆台内的补偿值", "晶圆更换", 0, 15, UnitHelper.mm)]
        public double WaferClampOffset { get; set; } = 10;

        /// <summary>
        /// 晶圆夹等待取料位置
        /// </summary>
        [TreeProgramListArgs("晶圆夹等待取料位置", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampWaitPickPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆夹在晶圆台取放料
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D WaferClampAtWaferTablePosition => this.GetWaferClampAtWaferTablePosition();

        /// <summary>
        /// 晶圆夹在magazine取放料
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D WaferClampAtMagazinePosition => this.GetWaferClampAtMagazinePosition();

        /// <summary>
        /// 4寸取放料位置（晶圆夹在晶圆台取放料）
        /// </summary>
        [TreeProgramListArgs("4寸取放料位置（晶圆夹在晶圆台取放料）", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampGetAtWaferTablePositionFourInches { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 6寸取放料位置（晶圆夹在晶圆台取放料）
        /// </summary>
        [TreeProgramListArgs("6寸取放料位置（晶圆夹在晶圆台取放料）", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampGetAtWaferTablePositionSixInches { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 8寸取放料位置（晶圆夹在晶圆台取放料）
        /// </summary>
        [TreeProgramListArgs("8寸取放料位置（晶圆夹在晶圆台取放料）", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampGetAtWaferTablePositionEightInches { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 12寸取放料位置（晶圆夹在晶圆台取放料）
        /// </summary>
        [TreeProgramListArgs("12寸取放料位置（晶圆夹在晶圆台取放料）", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampGetAtWaferTablePositionTwelveInches { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 4寸取放料位置（晶圆夹在magazine取放料）
        /// </summary>
        [TreeProgramListArgs("4寸取放料位置（晶圆夹在magazine取放料）", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampGetAtMagazinePositionFourInches { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 6寸取放料位置（晶圆夹在magazine取放料）
        /// </summary>
        [TreeProgramListArgs("6寸取放料位置（晶圆夹在magazine取放料）", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampGetAtMagazinePositionSixInches { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 8寸取放料位置（晶圆夹在magazine取放料）
        /// </summary>
        [TreeProgramListArgs("8寸取放料位置（晶圆夹在magazine取放料）", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampGetAtMagazinePositionEightInches { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 12寸取放料位置（晶圆夹在magazine取放料）
        /// </summary>
        [TreeProgramListArgs("12寸取放料位置（晶圆夹在magazine取放料）", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampGetAtMagazinePositionTwelveInches { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆夹安全位置
        /// </summary>
        [TreeProgramListArgs("晶圆夹安全位置", "晶圆更换", UnitHelper.mm)]
        public AKRSPoint3D WaferClampSafePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆台当前料片
        /// </summary>
        public BaseTablet CurrentTablet { get; set; } = new NullTablet();      

        /// <summary>
        /// 扩晶低位
        /// </summary>
        [TreeProgramListArgs("扩晶低位", "晶圆台模组设备参数", UnitHelper.mm)]
        public AKRSPoint3D ExpandDownPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 扩晶速度
        /// </summary>
        [TreeProgramListArgs("扩晶速度", "晶圆台模组设备参数", 0.01, 2.1, UnitHelper.speed)]
        public double ExpandSpeed { get; set; } = 0.5;

        /// <summary>
        /// 是否需要扩晶回零
        /// </summary>
        [TreeProgramListArgs("是否需要扩晶回零", "")]
        public bool IsNeedExpandZero { get; set; } = false;

        /// <summary>
        /// 是否开启晶圆图位置预获取
        /// </summary>
        [TreeProgramListArgs("是否开启晶圆图位置预获取", "")]
        public bool IsOpenPreGetWaferMapNextDiePos { get; set; } = true;

        /// <summary>
        /// 晶圆图文件路径
        /// </summary>
        public string WaferMapFilePath { get; set; } = string.Empty;

        /// <summary>
        /// 是否使用热吹风
        /// </summary>
        [TreeProgramListArgs("是否使用热吹风", "热吹风")]
        public bool IsUseHotBlower { get; set; } = false;

        /// <summary>
        /// 热吹风保持时间
        /// </summary>
        [TreeProgramListArgs("热吹风保持时间", "热吹风", 0, 20000, UnitHelper.ms)]
        public int DelayHotBlowerHold { get; set; } = 2000;

        /// <summary>
        /// 热吹风关闭后延时
        /// </summary>
        [TreeProgramListArgs("热吹风关闭后延时", "热吹风", 0, 20000, UnitHelper.ms)]
        public int DelayHotBlowerAfterClose { get; set; } = 1000;

        ///// <summary>
        ///// 是否启用静态适配器
        ///// </summary>
        //[TreeProgramListArgs("Is Static Adapter", "WaferTableDevicePara")]
        //public bool IsStaticAdapter { get; set; } = false;

        /// <summary>
        /// 当前静态华夫盘料片
        /// </summary>
        public AdapterTablet CurrentStaticAdapterTablet { get; set; } = new AdapterTablet();

        /// <summary>
        /// 是否需要重新创建静态华夫盘状态
        /// </summary>
        public bool IsNeedReCreateStateWithStaticAdapterTablet { get; set; } = false;

        /// <summary>
        /// 是否屏蔽晶圆料片感应
        /// </summary>
        [TreeProgramListArgs("是否屏蔽晶圆料片感应", "晶圆更换")]
        public bool IsUnCheckTablet { get; set; } = false;

        /// <summary>
        /// 是否使用载具高度测厚
        /// </summary>
        [TreeProgramListArgs("是否使用载具高度测厚", "晶圆台模组设备参数")]
        public bool IsUseCarrierHeightMeasure { get; set; } = false;

        /// <summary>
        /// 是否使用晶圆台华夫盘真空
        /// </summary>
        [TreeProgramListArgs("是否使用晶圆台华夫盘真空", "晶圆台模组设备参数")]
        public bool IsUseWaferTableWaffleVacuum { get; set; } = false;

        /// <summary>
        /// 晶圆模块持续搜晶延时
        /// </summary>
        [TreeProgramListArgs("晶圆模块持续搜晶延时", "晶圆台模组设备参数", 0, 5000, UnitHelper.ms)]
        public int DelaySearchDieContinue { get; set; } = 1000;

        /// <summary>
        /// 是否使用静态华夫盘真空
        /// </summary>
        [TreeProgramListArgs("是否使用静态华夫盘真空", "晶圆台模组设备参数")]
        public bool IsUseStaticWaffleVacuum { get; set; } = true;

        /// <summary>
        /// 单颗搜晶方式的当前搜索方向
        /// </summary>
        [TreeProgramListArgs("单颗搜晶方式的当前搜索方向")]
        public SingleSearchDirection SingleSearchDirectionCurrent
        {
            get
            {
                if (CurrentTablet is WaferTablet wt)
                {
                    return wt.SingleSearchDirectionCurrent;
                }
                else
                {
                    return SingleSearchDirection.ToRightDown;
                }
            }

            set
            {
                if (CurrentTablet is WaferTablet wt)
                {
                    wt.SingleSearchDirectionCurrent = value;
                }
            }
        }

        /// <summary>
        /// 创建静态华夫盘状态
        /// </summary>
        public void CreateStateWithStaticAdapterTablet()
        {
            this.CurrentStaticAdapterTablet =
                new AdapterTablet() { Name = WaferSystemProgram.GetInstance().StaticAdapterProgram.Name };

            if (this.CurrentStaticAdapterTablet.Name == string.Empty)
            {
                return;
            }

            this.CurrentStaticAdapterTablet.CreateAdapterEntity();
            WaferSubDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 置位是否需要重新创建静态华夫盘状态信号
        /// </summary>
        public void SetNeedReCreateStateWithStaticAdapterTabletSignal()
        {
            this.IsNeedReCreateStateWithStaticAdapterTablet = true;
            WaferSubDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 复位是否需要重新创建静态华夫盘状态信号
        /// </summary>
        public void ResetNeedReCreateStateWithStaticAdapterTabletSignal()
        {
            this.IsNeedReCreateStateWithStaticAdapterTablet = false;
            WaferSubDevicePara.GetInstance().Save();
        }

        ///// <summary>
        ///// 扩晶-预热时间
        ///// </summary>
        //public double ExpandHeatTime { get; set; }

        ///// <summary>
        ///// 扩晶-松膜吹风时间
        ///// </summary>
        //public double ExpandLooseBlowTime { get; set; }

        #endregion

        /// <summary>
        /// 获取晶圆夹在晶圆台的位置
        /// </summary>
        /// <returns>晶圆夹在晶圆台的位置</returns>
        private AKRSPoint3D GetWaferClampAtWaferTablePosition()
        {
            if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.FourInches)
            {
                return this.WaferClampGetAtWaferTablePositionFourInches;
            }
            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.SixInches)
            {
                return this.WaferClampGetAtWaferTablePositionSixInches;
            }
            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.EightInches)
            {
                return this.WaferClampGetAtWaferTablePositionEightInches;
            }
            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.TwelveInches)
            {
                return this.WaferClampGetAtWaferTablePositionTwelveInches;
            }
            else
            {
                throw new Exception("Not exist wafer sensor on wafer table");
            }
        }

        /// <summary>
        /// 获取晶圆夹在magazine的位置
        /// </summary>
        /// <returns>晶圆夹在magazine的位置</returns>
        private AKRSPoint3D GetWaferClampAtMagazinePosition()
        {
            if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.FourInches)
            {
                return this.WaferClampGetAtMagazinePositionFourInches;
            }
            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.SixInches)
            {
                return this.WaferClampGetAtMagazinePositionSixInches;
            }
            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.EightInches)
            {
                return this.WaferClampGetAtMagazinePositionEightInches;
            }
            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.TwelveInches)
            {
                return this.WaferClampGetAtMagazinePositionTwelveInches;
            }
            else
            {
                throw new Exception("Not exist wafer sensor on wafer table");
            }
        }
    }
}
