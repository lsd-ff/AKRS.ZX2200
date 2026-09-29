namespace AKRS.ZX2200.DispenseSystem.Models.DeviceParams
{
    using System.Collections.Generic;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using PropertyChanged;
    using System.ComponentModel;
    using AKRS.ZX2200.BondSystem.Models;

    /// <summary>
    /// 设备参数，单独保存到一个文件夹
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class DispenseDevicePara : Singleton<DispenseDevicePara>, INotifyPropertyChanged
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static DispenseDevicePara()
        {
            DispenseDevicePara.FilePath = ZX2200PathConfig.DispenseDevicePath;
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

        /// <summary>
        /// 点胶头测高参数
        /// </summary>
        public DispenserPara DispenserPara { get; set; } = new DispenserPara();

        /// <summary>
        /// 预点胶板参数
        /// </summary>
        public PreDispensePlatePara PreDispensePlatePara { get; set; } = new PreDispensePlatePara();

        /// <summary>
        /// 点胶模组参数
        /// </summary>
        public DispenseModulePara DispenseModulePara { get; set; } = new DispenseModulePara();

        /// <summary>
        /// 偏移点位
        /// </summary>
        /// <param name="offset">事件源</param>
        public void OffSetPoint(AKRSPoint3D offset)
        {
            this.DispenserPara.ThrustPosition -= offset;
            this.DispenserPara.ErasePosition -= offset;
            this.DispenserPara.VisionPosForCalibration -= offset;
            this.DispenserPara.DispenserMeasureHeightPos -= offset;
            this.DispenserPara.PrintingToolPos -= offset;
            this.PreDispensePlatePara.PreDispensePlateStartPos -= offset;
            this.PreDispensePlatePara.PreDispensePlateEndPos -= offset;
            System1Domain.GetInstance().System1Program.PreDispensePlateProgram.InitPre();
            this.Save();
        }

        /// <summary>
        /// 清除记忆
        /// </summary>
        public void ClearPosition()
        {
            this.DispenserPara.ReplaceGluePosition = new AKRSPoint3D();
            this.DispenserPara.ThrustPosition = new AKRSPoint3D(); ;
            this.DispenserPara.ErasePosition = new AKRSPoint3D(); ;
            this.DispenserPara.VisionPosForCalibration = new AKRSPoint3D(); ;
            this.DispenserPara.DispenserMeasureHeightPos = new AKRSPoint3D(); ;
            this.DispenserPara.PrintingToolPos = new AKRSPoint3D(); ;
        }
    }
}
