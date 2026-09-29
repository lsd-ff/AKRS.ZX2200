using AKRS.ZX2200.SupportFeature.Parameters.Model;
using AKRS.ZX2200.SupportFeature.Parameters.ObjParameter;
using AKRS.ZX2200.SupportFeature.Parameters;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
using AKRS.ZX2200.WaferSubSystem.Models;

namespace AKRS.ZX2200.SupportFeature.Parameters.ObjParameter
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Programs;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;

    public class ParameterManage
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public static void InitControl(UcProgramArgs ucProgramArgs)
        {
            ucProgramArgs.TreeProgramsArgsList.Clear();

            //-------------------TEST
            //  TreeProgramsArgsList.Add(new ObjAndNodesInfo(new NozzleshelfPara(),
            //   TreeGroupChildNodesEnum.General,
            //   "asd"));

            //  TreeProgramsArgsList.Add(new ObjAndNodesInfo(new NozzleshelfPara(),
            //TreeGroupChildNodesEnum.General,
            //"asd1"));


            //-------------------------------Product------------------------------------

            //-------------------------------System1------------------------------------
            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                DispenseDevicePara.GetInstance().PreDispensePlatePara,
                TreeGroupChildNodesEnum.MachineData,
                "预点胶板参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                DispenseDevicePara.GetInstance().DispenseModulePara,
                TreeGroupChildNodesEnum.MachineData,
                "系统1模组参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                DispenseDevicePara.GetInstance().DispenserPara,
                TreeGroupChildNodesEnum.MachineData,
                "点胶头位置参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                BondDevicePara.GetInstance().S2DispenseDevicePara.S2PreDispensePlatePara,
                TreeGroupChildNodesEnum.MachineData,
                "系统2预点胶板参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                BondDevicePara.GetInstance().S2DispenseDevicePara,
                TreeGroupChildNodesEnum.MachineData,
                "系统2点胶头位置参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                System1Domain.GetInstance().System1Program.DispenserProgram.Dispenser,
                TreeGroupChildNodesEnum.Dispenser,
                "程式点胶头参数"));

            //-------------------------------TransportUnit------------------------------------
            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                ProductConfiguration.GetInstance().TransportUnitConfig,
                TreeGroupChildNodesEnum.TransportUnitConfig,
                "框架参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                ProductConfiguration.GetInstance().SubstrateConfig,
                TreeGroupChildNodesEnum.SubstrateConfig,
                "基板参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                ProductConfiguration.GetInstance().ModuleConfig,
                TreeGroupChildNodesEnum.ModuleConfig,
                "基岛参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                ProductConfiguration.GetInstance().BondPositionConfig,
                TreeGroupChildNodesEnum.BondPositionConfig,
                "焊点参数"));

            // ------------------------------- Bond System ------------------------------------

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    BondDevicePara.GetInstance().BondHeadParam,
                    TreeGroupChildNodesEnum.MachineData,
                    "系统2模组参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    RtuConnectConfig.GetInstance(),
                    TreeGroupChildNodesEnum.ModbusConnect,
                    "系统2"));

            ucProgramArgs.TreeProgramsArgsList.Add(
              new ObjAndNodesInfo(
                  BondDevicePara.GetInstance().IPTDevicePara,
                  TreeGroupChildNodesEnum.IPT,
                  "系统2"));

            ucProgramArgs.TreeProgramsArgsList.Add(
              new ObjAndNodesInfo(
                  BondDevicePara.GetInstance().BMCDevicePara,
                  TreeGroupChildNodesEnum.BMC,
                  "系统2"));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(new NozzleshelfPara(), TreeGroupChildNodesEnum.PPtoolbankallocation, "系统2"));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    BondDevicePara.GetInstance().NozzleShelfParam,
                    TreeGroupChildNodesEnum.Toolbank,
                    "系统2"));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    System2Domain.GetInstance().System2Configuration,
                    TreeGroupChildNodesEnum.Configuration,
                    "系统2"));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    BondDevicePara.GetInstance().CameraDevicePara,
                    TreeGroupChildNodesEnum.Camera,
                    "系统2"));

            foreach (var nozzle in BondProgram.GetInstance().NozzleList)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(
                    new ObjAndNodesInfo(nozzle, TreeGroupChildNodesEnum.PPandepoxytools, nozzle.Name));
            }

            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                FlipTool flipTool = (FlipTool)FlipToolRepository.GetInstance().Find(WaferSystemProgram.GetInstance().FlipModuleProgram.FlipToolName);

                if (flipTool != null)
                {
                    ucProgramArgs.TreeProgramsArgsList.Add(
                        new ObjAndNodesInfo(flipTool, TreeGroupChildNodesEnum.Fliptool, flipTool.Name));
                }
            }


            ucProgramArgs.TreeProgramsArgsList.Add(
              new ObjAndNodesInfo(
                  ForceConfig.GetInstance(),
                  TreeGroupChildNodesEnum.ForceCalibrate,
                  "系统2"));

            foreach (var item in ForceConfig.GetInstance().LargeForceConfigItemList)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(
                    new ObjAndNodesInfo(
                        item,
                        TreeGroupChildNodesEnum.ForceControl,
                        item.ForceLowerLimit + "~" + item.ForceUpperLimit + "(g)"));
            }

            foreach (var item in ForceConfig.GetInstance().SmallForceConfigItemList)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(
                    new ObjAndNodesInfo(
                        item,
                        TreeGroupChildNodesEnum.SmallForceControl,
                        item.ForceLowerLimit + "~" + item.ForceUpperLimit + "(g)"));
            }

            foreach (var postBond in System2Domain.GetInstance().BondProgram.PostBondProgram.PostBondInspections)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(
                    new ObjAndNodesInfo(postBond, TreeGroupChildNodesEnum.Postbondinspection, postBond.Name));
            }

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                BondDevicePara.GetInstance().ULMPara,
                TreeGroupChildNodesEnum.MachineData,
                "ULM运动配置"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                BondProgram.GetInstance().SlideFluxerProgram,
                TreeGroupChildNodesEnum.MachineData,
                "刮胶盘设置"));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    MachineHardwareConfiguration.GetInstance(),
                    TreeGroupChildNodesEnum.MachineHardwarecConfiguration,
                    ""));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    MachineSoftwareConfiguration.GetInstance(),
                    TreeGroupChildNodesEnum.MachineSoftwarecConfiguration,
                    ""));

            ucProgramArgs.TreeProgramsArgsList.Add(
    new ObjAndNodesInfo(
        BondProgram.GetInstance().CleanNozzleProgram ,
        TreeGroupChildNodesEnum.NozzleClean,
        ""));

            // -----------------------------------------------------------------------------------------


            // ------------------------------- Transport System ------------------------------------

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    TransportProgram.GetInstance().LoadingSubSectionProgram.InOutPutBeltSetting,
                    TreeGroupChildNodesEnum.TUData,
                    "进料载台:" + TransportProgram.GetInstance().LoadingSubSectionProgram.TransportBeltSettingName));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    TransportProgram.GetInstance().DispenseSubSectionProgram.TransportBeltSetting,
                    TreeGroupChildNodesEnum.TUData,
                    "点胶载台:" + TransportProgram.GetInstance().DispenseSubSectionProgram.TransportBeltSettingName));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    TransportProgram.GetInstance().BondSubSectionProgram.TransportBeltSetting,
                    TreeGroupChildNodesEnum.TUData,
                    "Bond载台:" + TransportProgram.GetInstance().BondSubSectionProgram.TransportBeltSettingName));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    TransportProgram.GetInstance().WaitingUnloadSubSectionProgram.TransportBeltSetting,
                    TreeGroupChildNodesEnum.TUData,
                    "等待出料载台:" + TransportProgram.GetInstance().WaitingUnloadSubSectionProgram.TransportBeltSettingName));

            ucProgramArgs.TreeProgramsArgsList.Add(
                new ObjAndNodesInfo(
                    TransportProgram.GetInstance().UnloadingSubSectionProgram.InOutPutBeltSetting,
                    TreeGroupChildNodesEnum.TUData,
                    "出料载台:" + TransportProgram.GetInstance().UnloadingSubSectionProgram.TransportBeltSettingName));

            // Bondinsert for
            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                TransportProgram.GetInstance().DispenseSubSectionProgram.BondinsertSetting,
                TreeGroupChildNodesEnum.Bondinsert,
                TransportProgram.GetInstance().DispenseSubSectionProgram.BondinsertSettingName));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                TransportProgram.GetInstance().BondSubSectionProgram.BondinsertSetting,
                TreeGroupChildNodesEnum.Bondinsert,
                TransportProgram.GetInstance().BondSubSectionProgram.BondinsertSettingName));

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration== LoadConfigurationEnum.LoaderBin
                && TransportProgram.GetInstance().LoaderBinProgram.LoaderBin != null)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                    TransportProgram.GetInstance().LoaderBinProgram.LoaderBin,
                    TreeGroupChildNodesEnum.Loader,
                    TransportProgram.GetInstance().LoaderBinProgram.LoaderBin.Name));
            }

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin
                && TransportProgram.GetInstance().UnLoaderBinProgram.UnloaderBin != null)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                    TransportProgram.GetInstance().UnLoaderBinProgram.UnloaderBin,
                    TreeGroupChildNodesEnum.Unloader,
                    TransportProgram.GetInstance().UnLoaderBinProgram.UnloaderBin.Name));
            }

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                   TransportDevicePara.GetInstance(),
                   TreeGroupChildNodesEnum.MachineData,
                   "流道设备参数"));


            // -----------------------------------------------------------------------------------------


            // ------------------------------- Bond Force ------------------------------------


            // -----------------------------------------------------------------------------------------


            //-------------------------------WaferSub System------------------------------------

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                WaferSubDevicePara.GetInstance().WaferTableDevicePara,
                TreeGroupChildNodesEnum.MachineData,
                "晶圆设备参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                WaferSubDevicePara.GetInstance().MagazineDevicePara,
                TreeGroupChildNodesEnum.MachineData,
                "Magazine设备参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                WaferSubDevicePara.GetInstance().EjectDevicePara,
                TreeGroupChildNodesEnum.MachineData,
                "顶针系统设备参数"));

            ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
    WaferSubDevicePara.GetInstance().FlipChipDevicePara,
    TreeGroupChildNodesEnum.MachineData,
    "翻转模组设备参数"));

            if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig != null)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                    WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig,
                    TreeGroupChildNodesEnum.Wafermagazineallocations,
                    WaferSystemProgram.GetInstance().MagazineAllocationsProgram.Name));
            }

            foreach (BaseCarrierConfig baseCarrier in WaferSystemProgram.GetInstance().GetCarriers())
            {
                ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                    baseCarrier,
                    TreeGroupChildNodesEnum.Component,
                    baseCarrier.Name));
            }

            foreach (AdapterConfig adapterConfig in WaferSystemProgram.GetInstance().GetAdapterConfig())
            {
                ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                    new AdapterPara(adapterConfig),
                    TreeGroupChildNodesEnum.Wafflegelpackadapter,
                    adapterConfig.Name));
            }

            if (WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig != null)
            {
                foreach (EjectionConfig ejectionSetting in WaferSystemProgram.GetInstance().GetDistinctEjectionSettings())
                {
                    ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                        ejectionSetting,
                        TreeGroupChildNodesEnum.Ejectionsystem,
                        ejectionSetting.Name));
                }
            }

            if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig != null)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                    WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig,
                    TreeGroupChildNodesEnum.Waferchange,
                    WaferSystemProgram.GetInstance().MagazineProgram.Name));

                if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo != null)
                {
                    ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                        WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo,
                        TreeGroupChildNodesEnum.Wafermagazinegeometry,
                        WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeoName));
                }
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                foreach (string name in System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames)
                {
                    ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                        EpoxyApplicationRepository.GetInstance().Find(name),
                        TreeGroupChildNodesEnum.Epoxyapplication,
                        name));
                }
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                foreach (string name in BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames)
                {
                    ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
                        EpoxyApplicationRepository.GetInstance().Find(name),
                        TreeGroupChildNodesEnum.Epoxyapplication,
                        name));
                }
            }
            
            foreach (SingleBondPositionConfig bond in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
            {
                ucProgramArgs.TreeProgramsArgsList.Add(
                    new ObjAndNodesInfo(bond, TreeGroupChildNodesEnum.BondPositionConfig, bond.Name));
            }

            // ------------------------------- Level Measure System ------------------------------------
            //ucProgramArgs.TreeProgramsArgsList.Add(new ObjAndNodesInfo(
            //    BondLevelMeasurePara.GetInstance(),
            //    TreeGroupChildNodesEnum.Machinedata,
            //    "BondLevelMeasurePara"));
        }
    }
}
