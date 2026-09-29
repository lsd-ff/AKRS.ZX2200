namespace AKRS.ZX2200.SupportFeature.Consumables
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Windows.Forms;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Models;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 耗材程式
    /// </summary>
    public class ConsumablesProgram : SingletonNoSave<ConsumablesProgram>
    {
        /// <summary>
        /// 耗材的集合
        /// </summary>
        public List<BaseConsumable> Consumables { get; set; }

        /// <summary>
        /// 耗材扫描线程
        /// </summary>
        private Thread thread;

        /// <summary>
        /// 获取耗材对象
        /// </summary>
        private void GetConsumablesList()
        {
            this.Consumables = new List<BaseConsumable>();

            foreach (TimeConsumable timeConsumable in System1Program.GetInstance().GetEpoxyConsumable())
            {
                this.Consumables.Add(timeConsumable);
            }

            foreach (TimeConsumable timeConsumable in BondProgram.GetInstance().GetConsumable())
            {
                this.Consumables.Add(timeConsumable);
            }

            foreach (FrequencyConsumables frequencyConsumables in BondProgram.GetInstance().NozzleShelfProgram.GetToolConsumable())
            {
                this.Consumables.Add(frequencyConsumables);
            }

            foreach (FrequencyConsumables frequencyConsumables in WaferSystemProgram.GetInstance().EjectionBankProgram.GetEjectFrequencyList())
            {
                this.Consumables.Add(frequencyConsumables);
            }
        }

        /// <summary>
        /// 后台统计线程
        /// </summary>
        public void BackgroundConsumable()
        {
            this.ScanConsumables();
        }

        /// <summary>
        /// 扫描耗材对象
        /// </summary>
        public void ScanConsumables()
        {
            this.GetConsumablesList();
            foreach (BaseConsumable baseConsumable in this.Consumables)
            {
                if (baseConsumable != null)
                {
                    if (baseConsumable.IsExpired() && baseConsumable.IsEnableTimeToAlarm())
                    {
                        baseConsumable.IsReminded = true;

                        baseConsumable.LastAlarmTime = DateTime.Now;

                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"耗材名字为： {baseConsumable.Name} 寿命已经到期 , 请选择怎么去处理\r\n" + $"忽略 : 继续工作，10分钟后再次提醒\r\n"
                            + "停止 : 设备停止，然后立即更换\r\n",
                            "耗材到期",
                            new[] { "忽略", "停止" },
                            new[] { DialogResult.Ignore, DialogResult.Abort });

                        switch (dialogResult)
                        {
                            case DialogResult.Ignore:
                                break;
                            case DialogResult.Abort:
                                Machine.GetInstance().Stop();
                                baseConsumable.Clear();
                                break;
                        }
                    }
                    else if (baseConsumable.IsRemind() && !baseConsumable.IsReminded)
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"耗材名字为： {baseConsumable.Name} 寿命即将到期 , 请即使处理\r\n",
                            "耗材即将到期",
                            new[] { "确定" },
                            new[] { DialogResult.OK});

                        baseConsumable.IsReminded = true;
                    }
                }
            }
        }
    }
}
