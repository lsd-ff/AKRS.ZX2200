using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.AOP.Module;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    using System;
    using System.ComponentModel;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using PropertyChanged;

    /// <summary>
    /// ULM运动过程需要经过的点位(都是机械坐标！)
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class ULMPara : PropertyChangeAop
    {
        #region JumpToUplook

        /// <summary>
        /// 晶圆环光上方位置
        /// </summary>
        [TreeProgramListArgs("晶圆环光上方位置", "取片去上视", false, UnitHelper.mm)]
        public AKRSPoint3D AboveWaferRingLightPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 中转台右上方位置
        /// </summary>
        [TreeProgramListArgs("左中转台右上方/翻转台正上方位置", "取片去上视", false, UnitHelper.mm)]
        public AKRSPoint3D AboveIPTRightPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 取片去上视速度
        /// </summary>
        [TreeProgramListArgs("去上视速度", "取片去上视", 0.1, 1500, UnitHelper.speed, RoleEnum.Admin)]
        public double JumpToUplookSpeed { get; set; } = 50;

        /// <summary>
        /// 取片去上视加速时间
        /// </summary>
        [TreeProgramListArgs("去上视加速时间", "取片去上视", 0.04,0.5, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToUplookPosAccelerationTime { get; set; } = 0.11;

        /// <summary>
        /// 取片去上视加加速时间
        /// </summary>
        [TreeProgramListArgs("去上视加加速时间", "取片去上视", 0.01,0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToUplookPosJerkTime { get; set; } = 0.03;

        ///// <summary>
        ///// 固晶完去取片加速时间
        ///// </summary>
        //[TreeProgramListArgs("取片去上视加加速度", "取片去上视", 10, 5000, UnitHelper.acc, RoleEnum.Admin)]
        //public double JumpToUplookPosAcc { get; set; } = 100;

        ///// <summary>
        ///// 固晶完去取片加速时间
        ///// </summary>
        //[TreeProgramListArgs("取片去上视加减速度", "取片去上视", 10, 5000, UnitHelper.dec, RoleEnum.Admin)]
        //public double JumpToUplookPosDec { get; set; } = 100;

        /// <summary>
        /// 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("去上视转角系数", "取片去上视", 0.1, 20000, null, RoleEnum.Admin)]
        public double JumpToUplookPosTime { get; set; } = 0.1;

        /// <summary>
        /// 曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("去上视曲率半径", "取片去上视", 0.1, 20000, null, RoleEnum.Admin)]

        public double JumpToUplookPosRadiusRatio { get; set; } = 0.5;


        #endregion

        #region JumpToPickupPos

        /// <summary>
        /// 晶圆环光左极限
        /// </summary> 
        [TreeProgramListArgs("晶圆环光左极限", "固晶完去取片", false, UnitHelper.mm)]
        public AKRSPoint3D WaferRingLightLeftLimitPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 晶圆环光右极限
        /// </summary> 
        [TreeProgramListArgs("晶圆环光右极限", "固晶完去取片", false, UnitHelper.mm)]
        public AKRSPoint3D WaferRingLightRightLimitPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 流道外延位置
        /// </summary> 
        [TreeProgramListArgs("流道外延位置", "固晶完去取片", false, UnitHelper.mm)]
        public AKRSPoint3D TransportUnitEdgePos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 固晶完去取片速度
        /// </summary>
        [TreeProgramListArgs("固晶完去取片速度", "固晶完去取片", 0.1, 1500, UnitHelper.speed)]
        public double JumpToPickupPosSpeed { get; set; } = 50;

        /// <summary>
        /// 固晶完去取片加速时间
        /// </summary>
        [TreeProgramListArgs("固晶完去取片加速时间", "固晶完去取片", 0.04, 0.5, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToPickupPosAccelerationTime { get; set; } = 0.11;

        /// <summary>
        /// 固晶完去取片加加速时间
        /// </summary>
        [TreeProgramListArgs("固晶完去取片加加速时间", "固晶完去取片", 0.01,0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToPickupPosJerkTime { get; set; } = 0.03;

        ///// <summary>
        ///// 固晶完去取片加速时间
        ///// </summary>
        //[TreeProgramListArgs("固晶完去取片加速度", "固晶完去取片", 10, 5000, UnitHelper.acc, RoleEnum.Admin)]
        //public double JumpToPickupPosAcc { get; set; } = 100;

        ///// <summary>
        ///// 固晶完去取片加速时间
        ///// </summary>
        //[TreeProgramListArgs("固晶完去取片减速度", "固晶完去取片", 10, 5000, UnitHelper.dec, RoleEnum.Admin)]
        //public double JumpToPickupPosDec { get; set; } = 100;

        /// <summary>
        /// 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("固晶完去取片转角系数", "固晶完去取片", 0.1, 50, null, RoleEnum.Admin)]
        public double JumpToPickupPosTime { get; set; } = 0.1;

        /// <summary>
        /// 曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("固晶完去取片曲率半径", "固晶完去取片", 0.1, 50, null, RoleEnum.Admin)]

        public double JumpToPickupPosRadiusRatio { get; set; } = 0.5;

        #endregion

        #region JumpToStaticWaffleVisionPos

        /// <summary>
        /// 去静态华夫盒拍照位速度
        /// </summary>
        [TreeProgramListArgs("去静态华夫盒拍照位速度", "去静态华夫盒拍照位", 0.1, 1500, UnitHelper.speed, RoleEnum.Admin)]
        public double JumpToStaticWaffleVisionPosSpeed { get; set; } = 50;

        /// <summary>
        /// 去静态华夫盒拍照位加速时间
        /// </summary>
        [TreeProgramListArgs("去静态华夫盒拍照位加速时间", "去静态华夫盒拍照位", 0.04, 0.5, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToStaticWaffleVisionPosAccelerationTime { get; set; } = 0.11;

        /// <summary>
        /// 去静态华夫盒拍照位加加速时间
        /// </summary>
        [TreeProgramListArgs("去静态华夫盒拍照位加加速时间", "去静态华夫盒拍照位", 0.01, 0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToStaticWaffleVisionPosJerkTime { get; set; } = 0.03;


        ///// <summary>
        ///// 去静态华夫盒拍照位加速度
        ///// </summary>
        //[TreeProgramListArgs("去静态华夫盒拍照位加速度", "去静态华夫盒拍照位", 10, 5000, UnitHelper.acc, RoleEnum.Admin)]
        //public double JumpToStaticWaffleVisionPosAcc { get; set; } = 100;

        ///// <summary>
        ///// 去静态华夫盒拍照位减速度
        ///// </summary>
        //[TreeProgramListArgs("去静态华夫盒拍照位减速度", "去静态华夫盒拍照位", 10, 5000, UnitHelper.dec, RoleEnum.Admin)]
        //public double JumpToStaticWaffleVisionPosDec { get; set; } = 100;

        /// <summary>
        /// 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("去静态华夫盒拍照位转角系数", "去静态华夫盒拍照位", 0.1, 50, null, RoleEnum.Admin)]
        public double JumpToStaticWaffleVisionPosTime { get; set; } = 0.1;

        /// <summary>
        /// 曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("去静态华夫盒拍照位曲率半径", "去静态华夫盒拍照位", 0.1, 50, null, RoleEnum.Admin)]

        public double JumpToStaticWaffleVisionPosRadiusRatio { get; set; } = 0.5;

        #endregion

        #region JumpToBondPos

        /// <summary>
        /// 去固晶速度
        /// </summary>
        [TreeProgramListArgs("去固晶速度", "去固晶位置", 0.1, 1500, UnitHelper.speed)]
        public double JumpToBondPosSpeed { get; set; } = 50;

        /// <summary>
        /// 去固晶加速时间
        /// </summary>
        [TreeProgramListArgs("去固晶加速时间", "去固晶位置", 0.04,0.5, UnitHelper.s,RoleEnum.Admin)]
        public double JumpToBondPosAccelerationTime { get; set; } = 0.11;

        /// <summary>
        /// 去固晶速度加加速时间
        /// </summary>
        [TreeProgramListArgs("去固晶加加速时间", "去固晶位置", 0.01, 0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToBondPosJerkTime { get; set; } = 0.03;

        /// <summary>
        /// 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("去固晶位转角系数", "去固晶位置", 0.1, 50, null, RoleEnum.Admin)]
        public double JumpToBondPosTime { get; set; } = 0.1;

        /// <summary>
        /// 曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("去固晶位曲率半径", "去固晶位置", 0.1, 50, null, RoleEnum.Admin)]

        public double JumpToBondPosRadiusRatio { get; set; } = 0.5;

        #endregion

        #region 中转台

        /// <summary>
        /// 右中转台左上方
        /// </summary>
        [TreeProgramListArgs("右中转台左上方", "取片去中转台", true, UnitHelper.mm)]
        public AKRSPoint3D AboveRightIPTLeftPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 取片去中转台速度
        /// </summary>
        [TreeProgramListArgs("取片去中转台速度", "取片去中转台", 0.1, 1500, UnitHelper.speed, RoleEnum.Admin)]
        public double JumpToIPTSpeed { get; set; } = 50;

        /// <summary>
        /// 取片去中转台加速时间
        /// </summary>
        [TreeProgramListArgs("取片去中转台加速时间", "取片去中转台", 0.05, 0.5, UnitHelper.s,RoleEnum.Admin)]
        public double JumpToIPTAccelerationTime { get; set; } = 0.11;

        /// <summary>
        /// 取片去中转台加加速时间
        /// </summary>
        [TreeProgramListArgs("取片去中转台加加速时间", "取片去中转台", 0.01, 0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToIPTJerkTime { get; set; } = 0.03;

        ///// <summary>
        ///// 取片去中转台加速度
        ///// </summary>
        //[TreeProgramListArgs("取片去中转台加速度", "取片去中转台", 10, 5000, UnitHelper.acc, RoleEnum.Admin)]
        //public double JumpToIPTAcc { get; set; } = 100;

        ///// <summary>
        ///// 取片去中转台减速度
        ///// </summary>
        //[TreeProgramListArgs("取片去中转台减速度", "取片去中转台", 10, 5000, UnitHelper.dec, RoleEnum.Admin)]
        //public double JumpToIPTDec { get; set; } = 100;

        /// <summary>
        /// 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("取片去中转台转角系数", "取片去中转台", 0.1, 50, null, RoleEnum.Admin)]
        public double JumpToIPTTime { get; set; } = 0.1;

        /// <summary>
        /// 曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("取片去中转台曲率半径", "取片去中转台", 0.1, 50, null, RoleEnum.Admin)]

        public double JumpToIPTRadiusRatio { get; set; } = 0.5;

        /// <summary>
        /// 去中转台视觉位速度
        /// </summary>
        [TreeProgramListArgs("去中转台视觉位速度", "去中转台视觉位", 0.1, 1500, UnitHelper.speed, RoleEnum.Admin)]
        public double JumpToIPTVisionPosSpeed { get; set; } = 50;

        /// <summary>
        /// 去中转台视觉位加速时间
        /// </summary>
        [TreeProgramListArgs("去中转台视觉位加速时间", "去中转台视觉位", 0.05, 0.5, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToIPTVisionPosAccelerationTime { get; set; } = 0.11;

        /// <summary>
        /// 去中转台视觉位加加速时间
        /// </summary>
        [TreeProgramListArgs("去中转台视觉位加加速时间", "去中转台视觉位", 0.01, 0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToIPTVisionPosJerkTime { get; set; } = 0.03;

        ///// <summary>
        ///// 取片去中转台加速度
        ///// </summary>
        //[TreeProgramListArgs("去中转台视觉位加速度", "去中转台视觉位", 10, 5000, UnitHelper.acc, RoleEnum.Admin)]
        //public double JumpToIPTVisionPosAcc { get; set; } = 100;

        ///// <summary>
        ///// 取片去中转台减速度
        ///// </summary>
        //[TreeProgramListArgs("去中转台视觉位减速度", "去中转台视觉位", 10, 5000, UnitHelper.dec, RoleEnum.Admin)]
        //public double JumpToIPTVisionPosDec { get; set; } = 100;

        /// <summary>
        /// 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("去中转台视觉位转角系数", "去中转台视觉位", 0.1, 50, null, RoleEnum.Admin)]
        public double JumpToIPTVisionPosTime { get; set; } = 0.1;

        /// <summary>
        /// 曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("去中转台视觉位曲率半径", "去中转台视觉位", 0.1, 50, null, RoleEnum.Admin)]

        public double JumpToIPTVisionPosRadiusRatio { get; set; } = 0.5;


        /// <summary>
        ///  去中转台取片位速度
        /// </summary>
        [TreeProgramListArgs("去中转台取片位速度", "去中转台取片位", 0.1, 1500, UnitHelper.speed, RoleEnum.Admin)]
        public double JumpToIPTPickPosSpeed { get; set; } = 50;

        /// <summary>
        ///  去中转台取片位加速时间
        /// </summary>
        [TreeProgramListArgs("去中转台取片位加速时间", "去中转台取片位", 0.05, 0.5, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToIPTPickPosAccelerationTime { get; set; } = 0.11;

        /// <summary>
        ///  去中转台取片位加加速时间
        /// </summary>
        [TreeProgramListArgs("去中转台取片位加加速时间", "去中转台取片位", 0.01, 0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToIPTPickPosJerkTime { get; set; } = 0.03;

        ///// <summary>
        ///// 取片去中转台加速度
        ///// </summary>
        //[TreeProgramListArgs("去中转台取片位加速度", "去中转台取片位", 10, 5000, UnitHelper.acc, RoleEnum.Admin)]
        //public double JumpToIPTPickPosAcc { get; set; } = 100;

        ///// <summary>
        ///// 取片去中转台减速度
        ///// </summary>
        //[TreeProgramListArgs("去中转台取片位减速度", "去中转台取片位", 10, 5000, UnitHelper.dec, RoleEnum.Admin)]
        //public double JumpToIPTPickPosDec { get; set; } = 100;


        /// <summary>
        /// 转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("去中转台取片位转角系数", "去中转台取片位", 0.1, 50, null, RoleEnum.Admin)]
        public double JumpToIPTPickPosTime { get; set; } = 0.1;

        /// <summary>
        /// 曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("去中转台取片位曲率半径", "去中转台取片位", 0.1, 50, null, RoleEnum.Admin)]

        public double JumpToIPTPickPosRadiusRatio { get; set; } = 0.5;

        #endregion

        #region 运动到G0位置

        ///// <summary>
        /////  运动到G0位置速度
        ///// </summary>
        //[TreeProgramListArgs("运动到G0位置速度", "运动到G0位置", 0.01, 2, UnitHelper.speed, RoleEnum.Admin)]
        //public double JumpToG0PosSpeed { get; set; } = 0.1;

        ///// <summary>
        /////  运动到G0位置加速时间
        ///// </summary>
        //[TreeProgramListArgs("运动到G0位置加速时间", "运动到G0位置", 0.05, 0.5, UnitHelper.s, RoleEnum.Admin)]
        //public double JumpToG0PosAccelerationTime { get; set; } = 0.11;

        ///// <summary>
        /////  运动到G0位置加加速时间
        ///// </summary>
        //[TreeProgramListArgs("运动到G0位置加加速时间", "运动到G0位置", 0.01, 0.1, UnitHelper.s, RoleEnum.Admin)]
        //public double JumpToG0PosJerkTime { get; set; } = 0.03;

        #endregion

        #region 运动到刮胶盘

        /// <summary>
        /// 去刮胶盘速度
        /// </summary>
        [TreeProgramListArgs("去刮胶盘速度", "去刮胶盘", 0.1, 1500, UnitHelper.speed)]
        public double JumpToSlideFluxerSpeed { get; set; } = 50;

        /// <summary>
        /// 去刮胶盘加速时间
        /// </summary>
        [TreeProgramListArgs("去刮胶盘加速时间", "去刮胶盘", 0.04, 0.5, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToSlideFluxerAccelerationTime { get; set; } = 0.11;

        /// <summary>
        /// 去刮胶盘加加速时间
        /// </summary>
        [TreeProgramListArgs("去刮胶盘加加速时间", "去刮胶盘", 0.01, 0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToSlideFluxerJerkTime { get; set; } = 0.03;


        /// <summary>
        /// 去刮胶盘转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("去刮胶盘转角系数", "去刮胶盘", 0.1, 50, null, RoleEnum.Admin)]
        public double JumpToSlideFluxerTime { get; set; } = 0.1;

        /// <summary>
        /// 去刮胶盘曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("去刮胶盘曲率半径", "去刮胶盘", 0.1, 50, null, RoleEnum.Admin)]

        public double JumpToSlideFluxerRadiusRatio { get; set; } = 0.5;

        #endregion

        #region 去晶圆台搜晶位置

        /// <summary>
        /// 去晶圆台搜晶位置速度
        /// </summary>
        [TreeProgramListArgs("去晶圆台搜晶位置速度", "去晶圆台搜晶位置", 0.1, 1500, UnitHelper.speed)]
        public double JumpToWaferSearchPosSpeed { get; set; } = 50;

        /// <summary>
        /// 去晶圆台搜晶位置加速时间
        /// </summary>
        [TreeProgramListArgs("去晶圆台搜晶位置加速时间", "去晶圆台搜晶位置", 0.04, 0.5, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToWaferSearchPosAccelerationTime { get; set; } = 0.11;

        /// <summary>
        /// 去晶圆台搜晶位置加加速时间
        /// </summary>
        [TreeProgramListArgs("去晶圆台搜晶位置加加速时间", "去晶圆台搜晶位置", 0.01, 0.1, UnitHelper.s, RoleEnum.Admin)]
        public double JumpToWaferSearchPosJerkTime { get; set; } = 0.03;


        /// <summary>
        /// 去晶圆台搜晶位置转角系数，转角系数越低，转角速度越低，转角处的轮廓精度越高；反之，转角系数越高，转角速度越高，转角处的轮廓精度越低
        /// </summary>
        [TreeProgramListArgs("去晶圆台搜晶位置转角系数", "去晶圆台搜晶位置", 0.1, 50, null, RoleEnum.Admin)]
        public double JumpToWaferSearchPosTime { get; set; } = 0.1;

        /// <summary>
        /// 去晶圆台搜晶位置曲率半径 圆弧系数，系数越大，目标速度越高；反之，系数越小，目标速度越小
        /// </summary>
        [TreeProgramListArgs("去晶圆台搜晶位置曲率半径", "去晶圆台搜晶位置", 0.1, 50, null, RoleEnum.Admin)]

        public double JumpToWaferSearchPosRadiusRatio { get; set; } = 0.5;

        #endregion


        /// <summary>
        /// 参数是否为空
        /// </summary>
        /// <returns>结果</returns>
        public bool IsEmpty()
        {
            bool ret1 = this.AboveWaferRingLightPos.IsEmpty;
            bool ret2 = this.AboveIPTRightPos.IsEmpty;
            bool ret3 = this.TransportUnitEdgePos.IsEmpty;

            return ret1 || ret2 || ret3;
        }


        /// <summary>
        /// 属性更改事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 属性更改记录
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <param name="before">开始</param>
        /// <param name="after">结束</param>
        protected virtual void OnPropertyChanged(string propertyName, object before, object after)
        {
            PropertyChangeAop.OnPropertyChangedEvent(this, propertyName, before, after);
        }
    }
}
