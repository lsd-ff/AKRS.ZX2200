using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.Models.Path;
using System.Collections.Generic;

namespace AKRS.ZX2200.Main.Machine.Process
{
    using DevExpress.XtraEditors;
    using System.ComponentModel;
    using System.Linq;

    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 制程
    /// </summary>
    public class ProcessDomain : Singleton<ProcessDomain>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ProcessDomain()
        {
            Singleton<ProcessDomain>.FilePath = ZX2200PathConfig.ProcessProgramFilePath;
        }

        /// <summary>
        /// 初始化点胶程式
        /// </summary>
        public void InitProcessProgram()
        {
            ProcessDomain.FilePath = ZX2200PathConfig.ProcessProgramFilePath;
            instance = null;
        }

        /// <summary>
        /// 系统1制程
        /// </summary>
        public SystemProcess S1SystemProcess { get; set; } = new SystemProcess();

        /// <summary>
        /// 系统2制程
        /// </summary>
        public SystemProcess S2SystemProcess { get; set; } = new SystemProcess();
        
        /// <summary>
        /// 是否准备
        /// </summary>
        /// <param name="useName">报错名称</param>
        /// <returns>结果</returns>
        public bool IsEnableToWork(string useName)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                foreach (SingleProcessStep processStep in this.S1SystemProcess.ProcessSteps)
                {
                    if (processStep.EpoxyNameApplicationName != "Null")
                    {
                        EpoxyApplication epoxyApplication = (EpoxyApplication)EpoxyApplicationRepository.GetInstance().Find(processStep.EpoxyNameApplicationName);

                        double total = epoxyApplication.DispensePressure + DispenseDevicePara.GetInstance().DispenseModulePara.GluePressCompensation;

                        if (total < 30)
                        {
                            XtraMessageBox.Show($"{epoxyApplication.Name}胶压过小，最小为30");
                            return false;
                        }

                        if (total > 500)
                        {
                            XtraMessageBox.Show($"{epoxyApplication.Name}胶压过大，最大为500");
                            return false;
                        }
                    }

                }

                return true;
            }

            useName += "失败";

            if (this.S2SystemProcess.ProcessSteps.Count == 0)
            {
                return false;
            }

            // 2025年12月30日 唐鹏新增
            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                int count = this.S2SystemProcess.ProcessSteps
                    .Where(it => (it.ComponentName != "Null") && (it.IsEnable == true)).Count();

                if (count > 1)
                {
                    AKRSXtraMessageBox.Show(useName + "原因FC机台只能贴一种芯片");
                    return false;
                }
            }

            foreach (SingleProcessStep processStep in this.S2SystemProcess.ProcessSteps)
            {
                if (processStep.ProcessStepName == string.Empty)
                {
                    AKRSXtraMessageBox.Show(useName + "原因有步骤的名称为空");
                    return false;
                }

                if (processStep.Index == 0)
                {
                    AKRSXtraMessageBox.Show(useName + $"步骤{processStep.ProcessStepName}的序号不能为0");
                    return false;
                }

                if (processStep.BondPositionName == "Null" || processStep.BondPositionName == null)
                {
                    AKRSXtraMessageBox.Show(useName + $"步骤{processStep.ProcessStepName}选择的焊点不能为空");
                    return false;
                }

                if (!ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList.Exists(it => it.Name == processStep.BondPositionName))
                {
                    // 临时注释
                    //AKRSXtraMessageBox.Show(useName + $"步骤{processStep.ProcessStepName}选择的焊点不存在，检查是否已经被删除");
                    //return false;
                }

                int task = 0;
                task = processStep.ComponentName != "Null" ? task + 1 : task;
                task = processStep.DefectName != "Null" ? task + 1 : task;
                task = processStep.EpoxyNameApplicationName != "Null" ? task + 1 : task;

                if (task == 0)
                {
                    AKRSXtraMessageBox.Show(useName + $"步骤{processStep.ProcessStepName}选择的功能全为空");
                    return false;
                }
                else if (task != 1)
                {
                    AKRSXtraMessageBox.Show(useName + $"步骤{processStep.ProcessStepName}选择的功能过多");
                    return false;
                }

                if (processStep.EpoxyNameApplicationName != "Null") 
                {
                    EpoxyApplication epoxyApplication = (EpoxyApplication)EpoxyApplicationRepository.GetInstance().Find(processStep.EpoxyNameApplicationName);

                    double total = epoxyApplication.DispensePressure + DispenseDevicePara.GetInstance().DispenseModulePara.GluePressCompensation;

                    if (total < 30)
                    {
                        XtraMessageBox.Show($"{epoxyApplication.Name}胶压过小，最小为30");
                        return false;
                    }

                    if (total > 500)
                    {
                        XtraMessageBox.Show($"{epoxyApplication.Name}胶压过大，最大为500");
                        return false;
                    }

                }
            }

            return true;
        }

        /// <summary>
        /// 是否准备好
        /// </summary>
        /// <returns>结果</returns>
        public bool CheckIsReady()
        {
            // 2025年12月30日 唐鹏新增 FC只能贴一颗芯片
            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                int count = this.S2SystemProcess.ProcessSteps
                    .Where(it => (it.ComponentName != "Null") && (it.IsEnable == true)).Count();

                if (count > 1)
                {
                    AKRSXtraMessageBox.Show("FC机台只能贴一种芯片");
                    return false;
                }
            }

            foreach (SingleProcessStep processStep in this.S2SystemProcess.ProcessSteps)
            {
                if (processStep.ProcessStepName == string.Empty)
                {
                    AKRSXtraMessageBox.Show("有步骤的名称为空,请检查后重试");
                    return false;
                }

                if (processStep.Index == 0)
                {
                    AKRSXtraMessageBox.Show($"步骤{processStep.ProcessStepName}的序号不能为0");
                    return false;
                }

                if (processStep.BondPositionName == "Null" || processStep.BondPositionName == null)
                {
                    AKRSXtraMessageBox.Show($"步骤{processStep.ProcessStepName}选择的焊点不能为空");
                    return false;
                }

                if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
                {
                    //// todo:判断焊点是否在焊点列表中
                    //if (!ProductConfiguration.GetInstance().OppositeSexConfiguration.GetAllBondPositionConfigs().Exists(it => it.Name == processStep.BondPositionName))
                    //{
                    //    // 临时注释
                    //    //AKRSXtraMessageBox.Show($"步骤{processStep.ProcessStepName}选择的焊点不存在，检查是否已经被删除");
                    //    //return false;
                    //}
                }
                else
                {
                    // 判断焊点是否在焊点列表中
                    if (!ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList.Exists(it => it.Name == processStep.BondPositionName))
                    {
                        AKRSXtraMessageBox.Show($"步骤{processStep.ProcessStepName}选择的焊点不存在，检查是否已经被删除");
                        return false;
                    }
                }

                int task = 0;
                task = processStep.ComponentName != "Null" ? task + 1 : task;
                task = processStep.DefectName != "Null" ? task + 1 : task;
                task = processStep.EpoxyNameApplicationName != "Null" ? task + 1 : task;

                if (task == 0)
                {
                    AKRSXtraMessageBox.Show($"步骤{processStep.ProcessStepName}选择的功能全为空，请修改后重试");
                    return false;
                }
                else if (task != 1)
                {
                    AKRSXtraMessageBox.Show($"步骤{processStep.ProcessStepName}选择的功能过多，请修改后重试");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 更改焊点的名称
        /// </summary>
        /// <param name="oldName">原来的名称</param>
        /// <param name="newName">新的名称</param>
        public void ChangeBpName(string oldName, string newName)
        {
            foreach (SingleProcessStep singleProcess in this.S1SystemProcess.ProcessSteps)
            {
                if (singleProcess.BondPositionName == oldName)
                {
                    singleProcess.BondPositionName = newName;
                }
            }

            foreach (SingleProcessStep singleProcess in this.S2SystemProcess.ProcessSteps)
            {
                if (singleProcess.BondPositionName == oldName)
                {
                    singleProcess.BondPositionName = newName;
                }
            }

            this.Save();
        }

        /// <summary>
        /// 自动生成步骤
        /// </summary>
        public void AutoGenerateProcessStep()
        {
            this.S1SystemProcess.ProcessSteps.Clear();
            this.S2SystemProcess.ProcessSteps.Clear();
            ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.RemoveNotExistConfig();

            int index1 = 1;
            int index2 = 1;

            // 寻找到所有焊点配置
            List<OppositeSexConfig> listOppositeSexConfigs = ProductDomain.GetInstance().ProductConfig
                .OppositeSexConfiguration.BaseConfigs.FindAll(it => it.EntityType == EntityTypeEnum.BondPosition);

            foreach (OppositeSexConfig oppositeSexConfig in listOppositeSexConfigs)
            {
                if (!string.IsNullOrEmpty(oppositeSexConfig.DispenseName))
                {
                    SingleProcessStep step = new SingleProcessStep()
                    {
                        BondPositionName = oppositeSexConfig.Name,
                        ProcessStepName = oppositeSexConfig.Name + "点胶",
                        EpoxyNameApplicationName = oppositeSexConfig.DispenseName,
                        IsEnable = oppositeSexConfig.Dispense,
                        Index = index1
                    };

                    this.S1SystemProcess.ProcessSteps.Add(step);
                    index1++;
                }

                if (!string.IsNullOrEmpty(oppositeSexConfig.EpoxyCheckName))
                {
                    SingleProcessStep step = new SingleProcessStep()
                    {
                        BondPositionName = oppositeSexConfig.Name,
                        ProcessStepName = oppositeSexConfig.Name + "胶量检测",
                        DefectName = oppositeSexConfig.EpoxyCheckName,
                        IsEnable = oppositeSexConfig.EpoxyCheck,
                        Index = index1
                    };
                    this.S1SystemProcess.ProcessSteps.Add(step);
                    index1++;
                }

                if (!string.IsNullOrEmpty(oppositeSexConfig.BondComponentName))
                {
                    SingleProcessStep step = new SingleProcessStep()
                    {
                        BondPositionName = oppositeSexConfig.Name,
                        ProcessStepName = oppositeSexConfig.Name + "贴片",
                        ComponentName = oppositeSexConfig.BondComponentName,
                        IsEnable = oppositeSexConfig.BondComponent,
                        Index = index2
                    };
                    this.S2SystemProcess.ProcessSteps.Add(step);
                    index2++;
                }

                if (!string.IsNullOrEmpty(oppositeSexConfig.AfterBondName))
                {
                    SingleProcessStep step = new SingleProcessStep()
                    {
                        BondPositionName = oppositeSexConfig.Name,
                        ProcessStepName = oppositeSexConfig.Name + "焊后检测",
                        DefectName = oppositeSexConfig.AfterBondName,
                        IsEnable = oppositeSexConfig.AfterBond,
                        Index = index2
                    };
                    this.S2SystemProcess.ProcessSteps.Add(step);
                    index2++;
                }
            }
        }
    }

    /// <summary>
    /// 制程系统
    /// </summary>
    public enum ProcessSystemEnum
    {
        /// <summary>
        /// 系统1
        /// </summary>
        [Description("系统1")]
        System1,

        /// <summary>
        /// 系统2
        /// </summary>
        [Description("系统2")]
        System2,
    }

    /// <summary>
    /// 工作流程
    /// </summary>
    public enum WorkProcessEnum
    {
        /// <summary>
        /// 框架优先
        /// </summary>
        [Description("框架优先")]
        TransportFirst,

        /// <summary>
        /// 基板优先
        /// </summary>
        [Description("基板优先")]
        SubstrateFirst,

        /// <summary>
        /// 基岛优先
        /// </summary>
        [Description("基岛优先")]
        ModuleFirst,

        /// <summary>
        /// 焊点优先
        /// </summary>
        [Description("焊点优先")]
        BondPositionFirst,
    }

    /// <summary>
    /// 前置流程
    /// </summary>
    public enum WorkProcessModuleEnum
    {
        /// <summary>
        /// 框架优先
        /// </summary>
        [Description("步骤优先")]
        StepFirst,

        /// <summary>
        /// 基板优先
        /// </summary>
        [Description("位置优先")]
        PositionFirst,
    }
}
