using System.Collections.Generic;
using AKRS.Galaxy2.Infrastructure.CommonModel;

using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Models.Programs
{
    using System.Windows.Forms;

    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.DispenseSystem.Models.Programs;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    /// <summary>
    /// 固晶程式
    /// </summary>
    public class BondProgram : Singleton<BondProgram>
    {
        /// <summary>
        /// 吸嘴架程式
        /// </summary>
        public NozzleShelfProgram NozzleShelfProgram { get; set; } = new NozzleShelfProgram();

        /// <summary>
        /// 焊后程式
        /// </summary>
        public PostBondProgram PostBondProgram { get; set; } = new PostBondProgram();

        /// <summary>
        /// 系统2点胶头程式
        /// </summary>
        public S2DispenserProgram S2DispenserProgram { get; set; } = new S2DispenserProgram();

        /// <summary>
        /// 系统2胶水程式
        /// </summary>
        public S2EpoxyMaterialProgram S2EpoxyMaterialProgram { get; set; } = new S2EpoxyMaterialProgram();

        /// <summary>
        /// 系统2预点胶板程式
        /// </summary>
        public S2PreDispensePlateProgram S2PreDispensePlateProgram { get; set; } = new S2PreDispensePlateProgram();

        /// <summary>
        /// 点胶图形
        /// </summary>
        public EpoxyApplicationProgram EpoxyApplicationProgram { get; set; } = new EpoxyApplicationProgram();

        /// <summary>
        ///  吸嘴清洁程式
        /// </summary>
        public CleanNozzleProgram CleanNozzleProgram { get; set; } = new CleanNozzleProgram();

        /// <summary>
        /// 刮胶程式
        /// </summary>
        public SlideFluxerProgram SlideFluxerProgram { get; set; } = new SlideFluxerProgram();

        /// <summary>
        /// 吸嘴架上的吸嘴
        /// </summary>
        [JsonIgnore]
        public List<Nozzle> NozzleList => this.NozzleShelfProgram.GetNozzleList();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static BondProgram()
        {
            FilePath = ZX2200PathConfig.BondProgramFilePath;
        }

        /// <summary>
        /// 程式自检
        /// </summary>
        /// <returns>结果</returns>
        public bool SelfCheck()
        {
            if (!this.NozzleShelfProgram.SelfCheck())
            {
                return false;
            }

            // 遍历所有芯片
            foreach (BaseCarrierConfig carrier in WaferSystemProgram.GetInstance().GetCarriers())
            {
                if (string.IsNullOrEmpty(carrier.NozzleName))
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"芯片:{carrier.Name}未配置吸嘴!\r\n  请先示教芯片!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                if (carrier.BondingForceMode == ForceModeEnum.Force)
                {
                    if (carrier.BondingForce <= BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal || carrier.BondingForce >= BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal)
                    {
                        DialogResult dialog = AKRSXtraMessageBox.Show(
                            $"芯片:{carrier.Name}  贴片力:{carrier.BondingForce}g 超限!力控范围：{BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal}g~{BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal}g\r\n请检查参数设置!",
                            "Warn",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return false;
                    }
                }

                if (carrier.PickupForceMode == ForceModeEnum.Force)
                {
                    if (carrier.PickupForce <= BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal || carrier.PickupForce >= BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal)
                    {
                        DialogResult dialog = AKRSXtraMessageBox.Show(
                            $"芯片:{carrier.Name}  取片力:{carrier.PickupForce}g 超限!力控范围：{BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal}g~{BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal}g\r\n请检查参数设置!",
                            "Warn",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return false;
                    }
                }

                if (carrier.IPTPickupForceMode == ForceModeEnum.Force)
                {
                    if (carrier.IPTPickupForce <= BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal || carrier.IPTPickupForce >= BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal)
                    {
                        DialogResult dialog = AKRSXtraMessageBox.Show(
                            $"芯片:{carrier.Name}  中转台取片力:{carrier.IPTPickupForce}g 超限!力控范围：{BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal}g~{BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal}g\r\n请检查参数设置!",
                            "Warn",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return false;
                    }
                }

                if (carrier.IPTPlacementForceMode == ForceModeEnum.Force)
                {
                    if (carrier.IPTPlaceForce <= BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal || carrier.IPTPlaceForce >= BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal)
                    {
                        DialogResult dialog = AKRSXtraMessageBox.Show(
                            $"芯片:{carrier.Name}  中转台放片力:{carrier.IPTPlaceForce}g 超限!力控范围：{BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal}g~{BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal}g\r\n请检查参数设置!",
                            "Warn",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        return false;
                    }
                }

                if (carrier.UpLookAdjustConfig.P1VisionPos.Equals(new AKRSPoint3D())
                    && carrier.AdjustCamera == CameraTypeEnum.UpLookCamera) 
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"芯片:{carrier.Name}  上视拍照位未示教!\r\n请先示教!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                if (carrier.DownLookAdjustConfig.P1VisionPos.Equals(new AKRSPoint3D())
                    && carrier.AdjustCamera == CameraTypeEnum.BondCamera)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"芯片:{carrier.Name}  下视拍照位未示教!\r\n请先示教!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                if (carrier.VacuumOffDelay > carrier.PlacementDelay)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"芯片:{carrier.Name}  关真空延时：{carrier.VacuumOffDelay}ms>固精延时:{carrier.PlacementDelay}ms!\r\n请检查参数设置!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }

                if (MachineHardwareConfiguration.GetInstance().IsIPTConfigrated
                    && carrier.NozzleVacuumOffDelayDuringPlaceOnIPT > carrier.IPTPlacementDelay) 
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"芯片:{carrier.Name}  中转台吸嘴关真空延时：{carrier.NozzleVacuumOffDelayDuringPlaceOnIPT}ms>放片延时:{carrier.IPTPlacementDelay}ms!\r\n请检查参数设置!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 初始化Bond程式
        /// </summary>
        public void InitBondProgram()
        {
            Singleton<BondProgram>.FilePath = ZX2200PathConfig.BondProgramFilePath;
            instance = null;
        }


        /// <summary>
        /// 获取耗材的集合
        /// </summary>
        /// <returns>结果</returns>
        public List<TimeConsumable> GetConsumable()
        {
            List<TimeConsumable> list = new List<TimeConsumable>();

            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                // 点胶耗材
                if (this.S2EpoxyMaterialProgram.EpoxyMaterial != null)
                {
                    if (this.S2EpoxyMaterialProgram.EpoxyMaterial.EpoxyMaterialConsumable == null)
                    {
                        this.S2EpoxyMaterialProgram.EpoxyMaterial.EpoxyMaterialConsumable = new TimeConsumable();
                    }

                    this.S2EpoxyMaterialProgram.EpoxyMaterial.EpoxyMaterialConsumable.Name = this.S2EpoxyMaterialProgram.EpoxyMaterial.Name;
                    list.Add(this.S2EpoxyMaterialProgram.EpoxyMaterial.EpoxyMaterialConsumable);
                }

            }


            // 助焊剂耗材
            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
            {
                if(this.SlideFluxerProgram.FluxConsumable==null)
                {
                    this.SlideFluxerProgram.FluxConsumable=new TimeConsumable() { Name = "助焊剂" };

                }
                list.Add(this.SlideFluxerProgram.FluxConsumable);
            }

            return list;
        }
    }
}
