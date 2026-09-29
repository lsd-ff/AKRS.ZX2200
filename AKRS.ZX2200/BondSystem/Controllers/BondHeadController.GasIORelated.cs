using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    /// <summary>
    /// 气路和IO相关
    /// </summary>
    public partial class BondHeadController
    {
        /// <summary>
        /// 清理吸嘴
        /// </summary>
        public void ClearHead()
        {
            this.SetBlowProportion(this.bondHeadParam.ThrowBlowProportion);
            DelayHelper.Delay(20);
            this.bondHead.WeakBlowElectric.SetOutputValue(true);
            DelayHelper.Delay(500);
            this.bondHead.WeakBlowElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 设置吹气比例
        /// </summary>
        /// <param name="value">值</param>
        public void SetBlowProportion(int value)
        {
            this.bondHead.WeakBlowProportionalElectric.WriteRxPDO((ushort)this.bondHead.WeakBlowProportionalElectric.ElectricIO, 1, value);
        }

        /// <summary>
        /// 读弱吹比例
        /// </summary>
        /// <returns>弱吹比例</returns>
        public int ReadWeakBlowProportion()
        {
            return this.bondHead.WeakBlowProportionalElectric.ReadRxPDO((ushort)this.bondHead.WeakBlowProportionalElectric.ElectricIO, 1);
        }

        /// <summary>
        /// 读漏晶值
        /// </summary>
        /// <returns>弱吹比例</returns>
        public int ReadVacuumValue()
        {
            return this.bondHead.CheckVaccumSensor.ReadTxPDO((ushort)this.bondHead.CheckVaccumSensor.SensorIO , 1);
        }

        /// <summary>
        /// 读焊头吸附真空模拟量
        /// </summary>
        /// <returns>值</returns>
        public int ReadBondheadVaccumValue()
        {
            return this.bondHead.ReadBondheadVaccumValue();
        }

        /// <summary>
        /// 开吹气
        /// </summary>
        public void OpenToolBlowEle()
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            if (this.GetBondheadVacuumState())
            {
                // 先关真空电磁阀
                this.bondHead.VaccumElectric.SetOutputValue(false);
                Thread.Sleep(50);
            }

            // 开吹气
            this.bondHead.WeakBlowElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 开吹气
        /// </summary>
        /// <param name="proportion">比例</param>
        public void OpenToolBlowEle(int proportion)
        {
            //// 单步工作
            //if (!System2Domain.GetInstance().WaitSingleStep())
            //{
            //    return;
            //}

            if (this.GetNozzleVacuumState())
            {
                // 先关真空电磁阀
                this.bondHead.VaccumElectric.SetOutputValue(false);
            }

            // 跟当前不一样就才设置比例阀
            int curProportion = this.ReadWeakBlowProportion();
            if (curProportion != proportion)
            {
                this.SetBlowProportion(proportion);

                // 这个延时是因为比例阀写入模拟量实际生效需要时间
                Thread.Sleep(50);
            }

            // 开吹气
            this.bondHead.WeakBlowElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 检查焊头上是否有吸嘴
        /// </summary>
        /// <returns>结果</returns>
        public bool IsToolOnBondheadPhy()
        {
            // 吸真空
            this.bondHead.BondHeadVaccumElectric.SetOutputValue(true);

            Thread.Sleep(200);

            bool ret;

            if (System2Configuration.GetInstance().IsBondheadVacuumUseAnalogue == false)
            {
                // IO
                ret = this.bondHead.CheckToolSensor.GetInputValue();
            }
            else
            {
                // 模拟量
                ret = this.ReadBondheadVaccumValue() < System2Configuration.GetInstance().BondheadVacuumAnalogueLimit;
            }

            return ret;
        }

        /// <summary>
        /// 检查焊头上是否有吸嘴（通过流量表）
        /// </summary>
        /// <param name="nozzle">吸嘴</param>
        /// <returns>结果</returns>
        public bool IsToolOnBondheadPhy(Nozzle nozzle)
        {
            // 吸真空
            this.bondHead.BondHeadVaccumElectric.SetOutputValue(true);

            Thread.Sleep(200);

            // 读模拟量
            int val = this.bondHead.CheckToolSensor.ReadTxPDO((ushort)this.bondHead.CheckVaccumSensor.SensorIO , 1);

            return true;
        }

        /// <summary>
        /// 根据程式记忆检查吸嘴
        /// </summary>
        /// <returns>结果</returns>
        public bool IsToolOnBondheadLogic()
        {
            bool ret = string.IsNullOrEmpty(this.bondHead.CurrentNozzleName);
            return !ret;
        }

        /// <summary>
        /// 关吸嘴吹气
        /// </summary>
        public void CloseToolBlowEle()
        {
            if (this.bondHead.WeakBlowElectric.GetOutputValue())
            {

                this.bondHead.WeakBlowElectric.SetOutputValue(false);
            }
        }

        /// <summary>
        /// 打开吸嘴真空
        /// </summary>
        public void OpenToolVaccum()
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            // 检查吹气状态
            if (this.GetNozzleBlowState())
            {
                // 先关吹气电磁阀
                this.bondHead.WeakBlowElectric.SetOutputValue(false);

                // todo：要不要加延时
            }

            // 开真空
            this.bondHead.VaccumElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 开焊头吸附
        /// </summary>
        public void OpenBondHeadVaccum()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            // 已经开了就不开
            if (this.GetBondheadVacuumState())
            {
                return;
            }

            this.bondHead.BondHeadVaccumElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 关焊头吸附
        /// </summary>
        public void CloseBondHeadVaccum()
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.bondHead.BondHeadVaccumElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 关闭吸嘴真空
        /// </summary>
        public void CloseToolVaccum()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.bondHead.VaccumElectric.SetOutputValue(false);
        }
    }
}
