using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.SensorCheckScan
{
    using AKRS.Galaxy2.Component.Simple.MessageBox;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

    /// <summary>
    /// 检测传感器报警
    /// </summary>
    public partial class FrmSensorAlarm : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 设备检测信号报警
        /// </summary>
        public FrmSensorAlarm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 传感器报警集合
        /// </summary>
        private readonly List<SensorAlarm> sensorAlarms = new List<SensorAlarm>();

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="message">信息</param>
        public void AddAlarmMessage(string message)
        {
            if (this.sensorAlarms.Find(it => it.Message == message) == null)
            {
                this.sensorAlarms.Add(new SensorAlarm(DateTime.Now, message));

                //AlarmLogEntity alarm = new AlarmLogEntity();
                //alarm.StartTime = DateTime.Now;
                //alarm.Message = message;
                //alarm.HandleTime = DateTime.Now;
                //DBService.Insert(alarm);
                this.Invoke(this.RefreshData);
                Machine.GetInstance().Stop();
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            this.gridControl1.DataSource = this.sensorAlarms;
        }

        /// <summary>
        /// 清除报警
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClearAlarm_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
            {
                // 恢复三色灯、蜂鸣器状态
                List<Alarmer> hardWaresByType = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
                hardWaresByType[0]?.SetRunState();
            }

            this.Close();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmSensorAlarm_Load(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
            {
                // 恢复三色灯、蜂鸣器状态
                List<Alarmer> hardWaresByType = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
                hardWaresByType[0]?.SetFirstLevelAlarm();
            }
        }
    }


    /// <summary>
    /// 传感器报警
    /// </summary>
    internal class SensorAlarm
    {
        /// <summary>
        /// 报警信息构造函数
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="message">信息</param>
        public SensorAlarm(DateTime dateTime, string message)
        {
            this.DateTime = dateTime;
            this.Message = message;
        }

        /// <summary>
        /// 报警时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 报警信息
        /// </summary>
        public string Message { get; set; }
    }
}