using System.IO.Ports;

using AKRS.Galaxy2.Infrastructure.CommonModel;

namespace AKRS.ZX2200.BondSystem.BondForce.Modbus
{
    using System;
    using System.ComponentModel;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using PropertyChanged;

    /// <summary>
    /// modbus配置
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class RtuConnectConfig : Singleton<RtuConnectConfig>, INotifyPropertyChanged
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RtuConnectConfig()
        {
            RtuConnectConfig.FilePath = ZX2200PathConfig.RtuConnectConfig;
            RtuConnectConfig.Load();
        }

        /// <summary>
        /// 压力表端口号
        /// </summary>
        [TreeProgramListArgs("校正台压力表端口号", (string)null)]
        public string ManometerSerialPort { get; set; } = "COM13";

        /// <summary>
        /// 校正台压力表站号
        /// </summary>
        [TreeProgramListArgs("校正台压力表站号", (string)null)]
        public int ManometerUnitIdentifier { get; set; } = 1;

        /// <summary>
        /// 焊头模拟量端口号
        /// </summary>
        [TreeProgramListArgs("焊头压力表端口号", (string)null)]
        public string BondheadSerialPort { get; set; } = "COM14";

        /// <summary>
        /// 焊头压力表站号
        /// </summary>
        [TreeProgramListArgs("焊头压力表站号", (string)null)]
        public int BondheadUnitIdentifier { get; set; } = 1;

        /// <summary>
        /// 波特率
        /// </summary>
        [TreeProgramListArgs("波特率", (string)null)]
        public int BaudRate { get; set; } = 9600;

        /// <summary>
        /// 从站地址
        /// </summary>
        public int UnitIdentifier { get; set; } = 1;

        /// <summary>
        /// Parity
        /// </summary>
        public Parity Parity { get; set; } = Parity.None;

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;

        /// <summary>
        /// 超时时间
        /// </summary>
        [TreeProgramListArgs("超时时间", (string)null)]
        public int ConnectionTimeout { get; set; } = 3000;

        /// <summary>
        /// 起始地址
        /// </summary>
        [TreeProgramListArgs("起始地址", (string)null)]
        public int StartingAddress { get; set; } = 33;

        /// <summary>
        /// 长度参数
        /// </summary>
        public int Quantity { get; set; } = 1;


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