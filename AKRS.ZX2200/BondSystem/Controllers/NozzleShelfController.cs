using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using DevExpress.CodeParser;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    /// <summary>
    /// 吸嘴架控制器
    /// </summary>
    public class NozzleShelfController
    {
        /// <summary>
        /// 吸嘴架模组
        /// </summary>
        private NozzleShelfModule nozzleShelfModule => System2Module.GetInstance().NozzleShelfModule;

        /// <summary>
        /// 吸嘴架程式
        /// </summary>
        private NozzleShelfProgram nozzleShelfProgram => BondProgram.GetInstance().NozzleShelfProgram;

        /// <summary>
        /// 吸嘴架程式
        /// </summary>
        private NozzleShelfParam nozzleShelfParam => BondDevicePara.GetInstance().NozzleShelfParam;

        /// <summary>
        /// 吸嘴架回0
        /// </summary>
        public ExcuteResult MoveShelfToHome()
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

           return this.nozzleShelfModule.MoveShelfToHome();
        }

        /// <summary>
        /// 吸嘴架回0不等待
        /// </summary>
        public void MoveShelfToHomeNoWait()
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return;
            }

            this.nozzleShelfModule.MoveShelfToHomeNoWait();
        }

        /// <summary>
        /// 吸嘴架配置检查
        /// </summary>
        /// <returns>结果</returns>
        public bool CheckToolBankConfiguration()
        {
            if (System2Configuration.GetInstance().IsToolBankEnable == false) 
            {
                return true;
            }

            string nozzleName = this.nozzleShelfProgram.NozzleShelf.NozzleShelfSlots[0].NozzleName;

            if (string.IsNullOrEmpty(nozzleName) && this.nozzleShelfModule.PickerHolderSensor1.GetInputValue())
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架槽1配置错误，请检查吸嘴架槽!",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.FirstLevel);

                return false;
            }

            nozzleName = this.nozzleShelfProgram.NozzleShelf.NozzleShelfSlots[1].NozzleName;

            if (string.IsNullOrEmpty(nozzleName) && this.nozzleShelfModule.PickerHolderSensor2.GetInputValue())
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架槽2配置错误，请检查吸嘴架槽!",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.FirstLevel);

                return false;
            }

            nozzleName = this.nozzleShelfProgram.NozzleShelf.NozzleShelfSlots[2].NozzleName;

            if (string.IsNullOrEmpty(nozzleName) && this.nozzleShelfModule.PickerHolderSensor3.GetInputValue())
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架槽3配置错误，请检查吸嘴架槽!",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.FirstLevel);

                return false;
            }

            nozzleName = this.nozzleShelfProgram.NozzleShelf.NozzleShelfSlots[3].NozzleName;

            if (string.IsNullOrEmpty(nozzleName) && this.nozzleShelfModule.PickerHolderSensor4.GetInputValue())
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架槽4配置错误，请检查吸嘴架槽!",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.FirstLevel);

                return false;
            }

            nozzleName = this.nozzleShelfProgram.NozzleShelf.NozzleShelfSlots[4].NozzleName;

            if (string.IsNullOrEmpty(nozzleName) && this.nozzleShelfModule.PickerHolderSensor5.GetInputValue())
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架槽5配置错误，请检查吸嘴架槽!",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.FirstLevel);

                return false;
            }

            nozzleName = this.nozzleShelfProgram.NozzleShelf.NozzleShelfSlots[5].NozzleName;

            if (string.IsNullOrEmpty(nozzleName) && this.nozzleShelfModule.PickerHolderSensor6.GetInputValue())
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架槽6配置错误，请检查吸嘴架槽!",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.FirstLevel);

                return false;
            }

            nozzleName = this.nozzleShelfProgram.NozzleShelf.NozzleShelfSlots[6].NozzleName;

            if (string.IsNullOrEmpty(nozzleName) && this.nozzleShelfModule.PickerHolderSensor7.GetInputValue())
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架槽7配置错误，请检查吸嘴架槽!",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.FirstLevel);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 指定吸嘴是否在吸嘴架上配置
        /// </summary>
        /// <param name="name">吸嘴名</param>
        /// <returns>结果</returns>
        public bool IsConfiguredOnToolBank(string name)
        {
            return this.nozzleShelfProgram.NozzleShelf.IsOnToolBank(name);
        }

        /// <summary>
        /// Touchdown是否在吸嘴架上
        /// </summary>
        /// <returns>结果</returns>
        public bool IsTouchDownOnToolBank()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return false;
            }

            return this.nozzleShelfProgram.NozzleShelf.IsOnToolBank("TouchDown");
        }

        /// <summary>
        /// Touchdown是否在吸嘴架上
        /// </summary>
        /// <returns>结果</returns>
        public bool IsBMCToolBank()
        {
            return this.nozzleShelfProgram.NozzleShelf.IsOnToolBank("BMC");
        }

        /// <summary>
        /// 吸嘴架移动到换吸嘴位
        /// </summary>
        public ExcuteResult MoveShelfToChangeNozzlePos()
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 先从G0转出来
            AKRSPoint3D pos =
                this.nozzleShelfModule.ToolBankCoordinateSystem.G0PosToSelf(
                    this.nozzleShelfParam.ToolChangePosition);

           return this.nozzleShelfModule.AxisY.AbsoluteMove(pos.Y);
        }

        /// <summary>
        /// 吸嘴架是否在换吸嘴位
        /// </summary>
        /// <returns>true:在 false:不在</returns>
        public bool IsShelfAtChangeNozzlePos()
        {
            double pos = this.GetYAxisPos();

                // 先从G0转出来
                double changePosition = this.nozzleShelfModule.ToolBankCoordinateSystem
                    .G0PosToSelf(this.nozzleShelfParam.ToolChangePosition).Z;

            return Math.Abs(pos - changePosition) < 0.1;
        }

        /// <summary>
        /// 吸嘴槽上是否有吸嘴
        /// </summary>
        /// <param name="slotNum">吸嘴槽位号</param>
        /// <returns>结果</returns>
        public bool IsSlotHaveNozzle(int slotNum)
        {
            return this.nozzleShelfModule.IsSlotHaveNozzle(slotNum);
        }

        /// <summary>
        ///  吸嘴架是否空
        /// </summary>
        /// <returns>true :空</returns>
        public bool IsNozzleShelfEmpty()
        {
            for (int i = 1; i < 8; i++)
            {
                bool isSlotHaveNozzle = this.IsSlotHaveNozzle(i);

                if (isSlotHaveNozzle)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取Y轴坐标
        /// </summary>
        /// <returns>坐标</returns>
        public double GetYAxisPos()
        {
            return this.nozzleShelfModule.AxisY.GetRealPosition();
        }

        /// <summary>
        /// 获取当前吸嘴的槽位
        /// </summary>
        /// <returns>吸嘴槽</returns>
        public NozzleShelfSlot GetCurrentToolNozzleShelfSlot()
        {
            return this.nozzleShelfProgram.GetCurrentToolNozzleShelfSlot();
        }

        /// <summary>
        /// 获取吸嘴架上所有的吸嘴对象
        /// </summary>
        /// <returns>吸嘴架上所有的吸嘴对象</returns>
        public List<Nozzle> GetNozzleList()
        {
            return this.nozzleShelfProgram.GetNozzleList();
        }

        /// <summary>
        ///  吸嘴架是否在零位
        /// </summary>
        /// <returns>结果</returns>
        public bool IsNozzleShelfAtHome()
        {
            double pos = this.GetYAxisPos();

            return pos < -3;
        }
    }
}
