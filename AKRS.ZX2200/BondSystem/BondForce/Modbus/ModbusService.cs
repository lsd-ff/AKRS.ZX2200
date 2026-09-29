using System;
using System.Windows.Forms;

using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;

using DevExpress.XtraEditors;

using EasyModbus;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.BondForce.Modbus
{
    using System.Threading;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    /// <summary>
    /// Modbus通讯服务类
    /// </summary>
    public class ModbusService : SingletonNoSave<ModbusService>
    {
        /// <summary>
        /// ModbusRtu对象
        /// </summary>
        [JsonIgnore]
        private ModbusClient ManometerClient { get; set; }

        /// <summary>
        /// ModbusRtu对象
        /// </summary>
        [JsonIgnore]
        private ModbusClient BondheadClient { get; set; }

        /// <summary>
        /// Modbus配置对象
        /// </summary>
        private RtuConnectConfig RtuConnectConfig => RtuConnectConfig.GetInstance();

        /// <summary>
        /// 耗时记录
        /// </summary>
        private MethodElapsedTimeRecorder methodElapsedTimeRecorder = new MethodElapsedTimeRecorder();

        /// <summary>
        ///  锁
        /// </summary>
        private static object lockObj = new object();


        /// <summary>
        /// 连接压力表,这个加到底层
        /// </summary>
        /// <returns>结果</returns>
        public bool ConnectManometer()
        {
            try
            {
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    return true;
                }

                RtuConnectConfig.GetInstance().Save();

                if (this.ManometerClient == null)
                {
                    this.ManometerClient = new ModbusClient(this.RtuConnectConfig.ManometerSerialPort);
                }

                // 防止端口占用，先断开连接
                //this.ManometerClient.Disconnect();

                this.methodElapsedTimeRecorder.RecordTime("Pick", $"开始连接焊头力");
                this.ManometerClient.SerialPort = this.RtuConnectConfig.ManometerSerialPort;
                this.ManometerClient.Baudrate = this.RtuConnectConfig.BaudRate;
                this.ManometerClient.Parity = this.RtuConnectConfig.Parity;
                this.ManometerClient.UnitIdentifier = (byte)this.RtuConnectConfig.ManometerUnitIdentifier;
                this.ManometerClient.StopBits = this.RtuConnectConfig.StopBits;
                this.ManometerClient.ConnectionTimeout = this.RtuConnectConfig.ConnectionTimeout;
                this.ManometerClient.Connect();

                this.methodElapsedTimeRecorder.RecordTime("Pick", $"连接焊头力成功");

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                // 权限拒绝 → 端口已被其他程序占用
                this.ManometerClient = this.BondheadClient;

                return true;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message, "Connect force sensor failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        /// <summary>
        /// 连接焊头
        /// </summary>
        /// <returns>结果</returns>
        public bool ConnectBondhead()
        {
            try
            {
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    return true;
                }

                RtuConnectConfig.GetInstance().Save();

                if (this.BondheadClient == null)
                {
                    this.BondheadClient = new ModbusClient(this.RtuConnectConfig.BondheadSerialPort);
                }

                // 防止端口占用，先断开连接
                //this.BondheadClient.Disconnect();


                this.BondheadClient.SerialPort = this.RtuConnectConfig.BondheadSerialPort;
                this.BondheadClient.Baudrate = this.RtuConnectConfig.BaudRate;
                this.BondheadClient.Parity = this.RtuConnectConfig.Parity;
                this.BondheadClient.UnitIdentifier = (byte)this.RtuConnectConfig.BondheadUnitIdentifier;
                this.BondheadClient.StopBits = this.RtuConnectConfig.StopBits;
                this.BondheadClient.ConnectionTimeout = this.RtuConnectConfig.ConnectionTimeout;
                this.BondheadClient.Connect();

                return true;
            }
            catch (UnauthorizedAccessException)
            {
                // 权限拒绝 → 端口已被其他程序占用
                this.BondheadClient = this.ManometerClient;

                return true;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    "焊头压力表连接失败:" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }
        }

        /// <summary>
        /// 断开传感器压力表连接
        /// </summary>
        public void DisConnectManometer()
        {
            this.ManometerClient.Disconnect();
        }

        /// <summary>
        /// 断开焊头应变片压力表连接
        /// </summary>
        public void DisConnectBondhead()
        {
            this.BondheadClient.Disconnect();
        }

        /// <summary>
        /// 读取摩尔力读数,读取前要先连接
        /// </summary>
        /// <returns>摩尔力读数数组</returns>
        public int[] ReadManometer()
        {
            lock (lockObj)
            {
                this.ManometerClient.UnitIdentifier = (byte)this.RtuConnectConfig.ManometerUnitIdentifier;
                return this.ManometerClient.ReadHoldingRegisters(this.RtuConnectConfig.StartingAddress, this.RtuConnectConfig.Quantity);
            }           
        }

        /// <summary>
        /// 读取焊头压力表
        /// </summary>
        /// <returns>摩尔力读数数组</returns>
        public int[] ReadBondForce()
        {
            lock (lockObj)
            {
                this.BondheadClient.UnitIdentifier = (byte)this.RtuConnectConfig.BondheadUnitIdentifier;
                return this.BondheadClient.ReadHoldingRegisters(this.RtuConnectConfig.StartingAddress, this.RtuConnectConfig.Quantity);
            }
        }

        /// <summary>
        /// 重置压力表
        /// </summary>
        public void ResetManometer()
        {
            //SerialPort sp = new SerialPort();
            //sp.PortName = ManometerClient.SerialPort;
            //sp.BaudRate = RtuConnectConfig.BaudRate;
            //sp.DataBits = 8;
            //sp.StopBits = RtuConnectConfig.StopBits;
            //sp.Parity = RtuConnectConfig.Parity;
            //sp.Open();

            //byte[] data = new byte[] { 0x01, 0x10, 0x00, 0x5E, 0x00, 0x01, 0x02, 0x00, 0x01, 0x6A, 0xEE };
            //sp.Write(data, 0, data.Length);

            //sp.Close();

            try
            {
                this.ManometerClient.UnitIdentifier = (byte)this.RtuConnectConfig.ManometerUnitIdentifier;

                // 置零
                this.ManometerClient.WriteMultipleRegisters(94, new int[] { 1 });
            }
            catch (Exception e)
            {
                Thread.Sleep(50);

                // 置零
                this.ManometerClient.WriteMultipleRegisters(94, new int[] { 1 });
            }
        }

        /// <summary>
        /// 重置焊头模拟量表
        /// </summary>
        public void ResetBondhead()
        {
            try
            {
                this.BondheadClient.UnitIdentifier = (byte)this.RtuConnectConfig.BondheadUnitIdentifier;

                // 置零
                this.BondheadClient.WriteMultipleRegisters(94, new int[] { 1 });
            }
            catch (Exception e)
            {
                Thread.Sleep(50);

                // 置零
                this.BondheadClient.WriteMultipleRegisters(94, new int[] { 1 });
            }
        }
    }
}
