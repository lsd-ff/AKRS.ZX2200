namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.Hardwares.DispenseControllers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls.Assistant;
    using AKRS.ZX2200.BondSystem.Controls.Setting.BondPosition;
    using AKRS.ZX2200.BondSystem.Controls.Setting.Nozzle;
    using AKRS.ZX2200.BondSystem.Controls.Setting.NozzleShelf;
    using AKRS.ZX2200.BondSystem.Controls.Setting.PostBond;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Controls.ToolControls.Programming;
    using AKRS.ZX2200.DispenseSystem.Controls.Assistant;
    using AKRS.ZX2200.DispenseSystem.Controls.Setting.Dispenser;
    using AKRS.ZX2200.DispenseSystem.Controls.Setting.EpoxyApplication;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.TransportSystem.Controls.Assistance;
    using AKRS.ZX2200.TransportSystem.Controls.Setting;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant.CarrierTeach;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant.ModuleTeach;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant.SubstrateTeach;
    using AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.FlipTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.MagazineTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.WaferHandlingTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Setting.Component;
    using AKRS.ZX2200.WaferSubSystem.Controls.Setting.FlipTool;
    using AKRS.ZX2200.WaferSubSystem.Controls.Setting.Waferhandling;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using DevExpress.XtraEditors;
    using DevExpress.XtraTreeList.Nodes;
    using LanguageExt;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.ZX2200.WaferSubSystem.Models.Enums;

    using Dispenser = DispenseSystem.Models.Repositories.Dispenser.Dispenser;

    /// <summary>
    /// 编辑产品程式
    /// </summary>
    [MethodAopAttribute]
    public partial class UcEditProductProgramming : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 选中的组件
        /// </summary>
        private XtraUserControl selectedControl;

        /// <summary>
        /// 模组控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcEditProductProgramming()
        {
            this.InitializeComponent();

            if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                this.selectedControl = new UcTransportSystem { Dock = DockStyle.Fill };
            }
            else
            {
                this.selectedControl = new UcBondMaxSubSection() { Dock = DockStyle.Fill };
            }
            
            this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

            this.RefreshNode();
            this.SortNodesUI();
        }

        /// <summary>
        /// 刷新Node
        /// </summary>
        public void RefreshNode()
        {
            // Wafer handing 
            TreeListNode waferHandlingNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "WaferHandling");
            waferHandlingNode.Nodes.Clear();
            string waferHandlingDatasetName = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.Name;
            if (!string.IsNullOrWhiteSpace(waferHandlingDatasetName))
            {
                MagazineBoxConfig magazineBoxConfig = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig;

                TreeListNode node =
                    this.TreeListProgramming.AppendNode(new object[] { waferHandlingDatasetName }, waferHandlingNode);
                node.Tag = new NodeTag() { TagName = "WaferHandlingSubNode", TagObject = magazineBoxConfig };

                // 设置图标
                if (!MagazineBoxConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == WaferSystemProgram.GetInstance().MagazineProgram.Name))
                {
                    WaferSystemProgram.GetInstance().MagazineProgram.Name = string.Empty;
                    WaferSystemProgram.GetInstance().Save();
                    Thread.Sleep(100);
                }
                else
                {
                    this.SetMagazineBoxStateImage(magazineBoxConfig, node);
                }
            }

            // PP Tools
            TreeListNode ppToolNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "PPTools");
            ppToolNode.Nodes.Clear();
            NozzleShelf nozzleShelf = System2Domain.GetInstance().BondProgram.NozzleShelfProgram.NozzleShelf;
            if (nozzleShelf != null)
            {
                foreach (var nozzleShelfNozzleShelfSlot in nozzleShelf.NozzleShelfSlots)
                {
                    if (!string.IsNullOrWhiteSpace(nozzleShelfNozzleShelfSlot?.NozzleName)
                        && nozzleShelfNozzleShelfSlot.NozzleName != "TouchDown" && nozzleShelfNozzleShelfSlot.NozzleName != "BMC")
                    {
                        TreeListNode node =
                            this.TreeListProgramming.AppendNode(new object[] { nozzleShelfNozzleShelfSlot.NozzleName }, ppToolNode);
                        node.Tag = new NodeTag() { TagName = "PPToolsSubNode", TagObject = nozzleShelfNozzleShelfSlot };

                        this.SetNozzleStateImage(nozzleShelfNozzleShelfSlot, node);
                    }
                }
            }

            // ES Tools
            TreeListNode esToolNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "ESTools");
            esToolNode.Nodes.Clear();

            List<EjectionBankSlotConfig> ejectionBankConfigList = WaferSystemProgram.GetInstance().GetDistinctEjectionBankSlotConfig();

            if (ejectionBankConfigList != null)
            {
                foreach (EjectionBankSlotConfig ejectionBankSlot in ejectionBankConfigList)
                {
                    if (!string.IsNullOrWhiteSpace(ejectionBankSlot.EjectionConfig?.Name))
                    {
                        TreeListNode node =
                            this.TreeListProgramming.AppendNode(new object[] { ejectionBankSlot.EjectionConfig.Name }, esToolNode);

                        node.Tag = new NodeTag() { TagName = "ESToolsSubNode", TagObject = ejectionBankSlot };

                        this.SetEjectStateImage(ejectionBankSlot, node);
                    }
                }
            }

            // 翻转工具
            TreeListNode flipToolNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "FlipTool");

            if (flipToolNode != null)
            {
                flipToolNode.Nodes.Clear();

                FlipTool flipTool = WaferSystemProgram.GetInstance().FlipModuleProgram.CurrentFlipTool;

                if (flipTool != null)
                {
                    TreeListNode nd = this.TreeListProgramming.AppendNode(new object[] { flipTool.Name }, flipToolNode);
                    nd.Tag = new NodeTag() { TagName = "FlipToolSubNode", TagObject = flipTool };

                    this.SetFlipToolStateImage(flipTool, nd);
                }
            }

            // Dispenser
            TreeListNode dispenserNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "Dispenser");
            dispenserNode?.Nodes.Clear();

            if (dispenserNode != null)
            {
                dispenserNode.Nodes.Clear();
                
                if (this.CmbSystem.SelectedIndex == 1 && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
                {
                    string dispenserName = BondProgram.GetInstance().S2DispenserProgram.DispenserName;
                    if (dispenserName != null && dispenserName != string.Empty)
                    {
                        Dispenser dispenser = BondProgram.GetInstance().S2DispenserProgram.Dispenser;

                        TreeListNode nd = this.TreeListProgramming.AppendNode(new object[] { dispenserName }, dispenserNode);
                        nd.Tag = "DispenserSubNode";

                        this.SetDispenserStateImage(dispenser, nd);
                    }
                }
                else
                {
                    string dispenserName = System1Program.GetInstance().DispenserProgram.DispenserName;
                    if (dispenserName != null)
                    {
                        Dispenser dispenser = System1Program.GetInstance().DispenserProgram.Dispenser;

                        TreeListNode nd = this.TreeListProgramming.AppendNode(new object[] { dispenserName }, dispenserNode);
                        nd.Tag = "DispenserSubNode";

                        this.SetDispenserStateImage(dispenser, nd);
                    }
                }
            }

            // Transport unit general
            TreeListNode TransportUnitGeneralNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "TransportUnitGeneral");
            TransportUnitGeneralNode.Nodes.Clear();
            TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;
            if (transportUnitConfig != null)
            {
                this.SetTransportUnitStateImage(transportUnitConfig, TransportUnitGeneralNode);
            }

            // Epoxy Application 
            TreeListNode applicationEpoxyNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "EpoxyApplication");
            applicationEpoxyNode?.Nodes.Clear();
            if (applicationEpoxyNode != null)
            {
                applicationEpoxyNode.Nodes.Clear();
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense || MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2 
                    && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
                {
                    List<EpoxyApplication> epoxyApplications = BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplications;

                    if (epoxyApplications != null)
                    {
                        foreach (EpoxyApplication ep in epoxyApplications)
                        {
                            if (!string.IsNullOrWhiteSpace(ep?.Name))
                            {
                                TreeListNode node =
                                    this.TreeListProgramming.AppendNode(new object[] { ep.Name }, applicationEpoxyNode);

                                node.Tag = new NodeTag() { TagName = "EpoxyApplicationSubNode", TagObject = ep };
                            }
                        }
                    }
                }
                else
                {
                    List<EpoxyApplication> epoxyApplications = System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplications;

                    if (epoxyApplications != null)
                    {
                        foreach (EpoxyApplication ep in epoxyApplications)
                        {
                            if (!string.IsNullOrWhiteSpace(ep?.Name))
                            {
                                TreeListNode node =
                                    this.TreeListProgramming.AppendNode(new object[] { ep.Name }, applicationEpoxyNode);

                                node.Tag = new NodeTag() { TagName = "EpoxyApplicationSubNode", TagObject = ep };
                            }
                        }
                    }
                }
            }


            // Bond Position
            TreeListNode bpNode = this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "BondingPosition");
            bpNode.Nodes.Clear();

            if (ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList != null)
            {
                if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
                {
                    foreach (SingleBondPositionConfig bpConfig in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
                    {
                        if (!string.IsNullOrWhiteSpace(bpConfig?.Name))
                        {
                            TreeListNode node =
                                this.TreeListProgramming.AppendNode(new object[] { bpConfig.Name }, bpNode);
                            node.Tag = new NodeTag() { TagName = "BondingPositionSubNode", TagObject = bpConfig };

                            this.SetBondPositionStateImage(bpConfig, node);
                        }
                    }
                }
            }

            // Component
            MagazineAllocationsConfig magazineAllocationsConfig =
                WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig;

            List<BaseCarrierConfig> carrierList = WaferSystemProgram.GetInstance().GetCarriers();

            TreeListNode componentNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "Components");
            componentNode.Nodes.Clear();

            if (carrierList != null)
            {
                foreach (BaseCarrierConfig ca in carrierList)
                {
                    TreeListNode node =
                        this.TreeListProgramming.AppendNode(new object[] { ca.Name }, componentNode);

                    node.Tag = new NodeTag() { TagName = "ComponentsSubNode", TagObject = ca };

                    this.SetComponentStateImage(ca, node);
                }
            }

            // Post-bond
            TreeListNode postBondNode = this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "PostBond");
            postBondNode.Nodes.Clear();
        
            if (PostBondInspectionRepository.GetInstance().BaseDsSettingList != null)
            {
                foreach (PostBondInspection pbConfig in BondProgram.GetInstance().PostBondProgram.PostBondInspections)
                {
                    if (!string.IsNullOrWhiteSpace(pbConfig?.Name))
                    {
                        TreeListNode node =
                            this.TreeListProgramming.AppendNode(new object[] { pbConfig.Name }, postBondNode);

                        node.Tag = new NodeTag() { TagName = "PostBondSubNode", TagObject = pbConfig };

                        this.SetPostBondStateImage(pbConfig, node);
                    }
                }
            }

            TreeListNode transportLoaderNode = this.GetInputNode();
            transportLoaderNode.Nodes.Clear();
            LoaderBin loader = TransportProgram.GetInstance().LoaderBinProgram.LoaderBin;
            if (loader != null)
            {
                transportLoaderNode.Tag = new NodeTag() { TagName = "Input", TagObject = loader };

                this.SetLoaderStateImage(loader, transportLoaderNode);
            }

            TreeListNode transportUnLoaderNode = this.GetOutputNode();
            transportUnLoaderNode.Nodes.Clear();
            UnloaderBin unLoader = TransportProgram.GetInstance().UnLoaderBinProgram.UnloaderBin;
            if (unLoader != null)
            {
                transportUnLoaderNode.Tag = new NodeTag() { TagName = "Output", TagObject = unLoader };

                this.SetUnLoaderStateImage(unLoader, transportUnLoaderNode);
            }
        }

        /// <summary>
        /// 获取输入节点
        /// </summary>
        /// <returns>result</returns>
        private TreeListNode GetInputNode()
        {
            TreeListNode node = this.TreeListProgramming.FindNode(a => (a.Tag as NodeTag)?.TagName.ToString() == "Input");

            if (node == null)
            {
                return this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "Input");
            }
            else
            {
                return node;
            }
        }

        /// <summary>
        /// 获取输出节点
        /// </summary>
        /// <returns>result</returns>
        private TreeListNode GetOutputNode()
        {
            TreeListNode node = this.TreeListProgramming.FindNode(a => (a.Tag as NodeTag)?.TagName.ToString() == "Output");

            if (node == null)
            {
                return this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "Output");
            }
            else
            {
                return node;
            }
        }

        /// <summary>
        /// 整理UI
        /// </summary>
        private void SortNodesUI() 
        {
            bool issystem2 = this.CmbSystem.Text == "System2" ? true : false;

            this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "ConfigurationFirNode").Visible = !issystem2;
            this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "ConfigurationSubNode").Visible = issystem2;

            this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "WaferHandling").Visible = issystem2;
            this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "PPTools").Visible = issystem2;
            this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "ESTools").Visible = issystem2;

            //this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "Input").Visible =
            //    MachineHardwareConfiguration.GetInstance().IsTransportConfigured;
            this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "BeltSystem1").Visible =
                MachineHardwareConfiguration.GetInstance().IsTransportConfigured;
            this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "BeltSystem2").Visible =
                MachineHardwareConfiguration.GetInstance().IsTransportConfigured;
            //this.TreeListProgramming.FindNode(a => a.Tag?.ToString() == "Output").Visible =
            //    MachineHardwareConfiguration.GetInstance().IsTransportConfigured;

            this.RefreshNode();
        }

        /// <summary>
        /// 节点切换事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        public void TreeListProgramming_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            this.SpiltContainerProgramming.Panel1.Controls.Clear();

            if (e.Node?.Tag == null)
            {
                return;
            }

            string tagStr = e.Node.Tag?.ToString();

            if (e.Node.Tag is NodeTag tag)
            {
                tagStr = tag.TagName;
            }

            string systemName = this.CmbSystem.Text;

            this.ILstAssistantStep.Items.Clear();

            switch (tagStr)
            {
                // Transport System
                case "TransportSystem":
                    if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
                    {
                        this.selectedControl = new UcTransportSystem { Dock = DockStyle.Fill };
                        this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    }
                    else
                    {
                        this.selectedControl = new UcBondMaxSubSection() { Dock = DockStyle.Fill };
                        this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    }
                    
                    break;

                case "Input":
                    LoaderBin loader = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as LoaderBin;

                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
                    {
                        this.selectedControl = new UcLoderSetting { Dock = DockStyle.Fill };
                    }
                    else
                    {
                        this.selectedControl = new UcInput { Dock = DockStyle.Fill };
                    }

                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
                    {
                        this.ILstAssistantStep.Items.Clear();
                        this.ILstAssistantStep.Items.AddRange(new object[] { "上料位置示教" });
                    }

                    if (loader != null)
                    {
                        this.RefreshILstAssistantStepWithTransportLoader(loader);
                    }

                    break;

                case "BeltSystem1":
                    this.selectedControl = new UcBeltSystem1 { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;

                case "BeltSystem2":
                    this.selectedControl = new UcBeltSystem2 { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;

                case "Output":
                    UnloaderBin unLoader = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as UnloaderBin;

                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
                    {
                        this.selectedControl = new UcUnloaderSetting() { Dock = DockStyle.Fill};
                    }
                    else
                    {
                        this.selectedControl = new UcOutput { Dock = DockStyle.Fill };
                    }
                 
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
                    {
                        this.ILstAssistantStep.Items.Clear();
                        this.ILstAssistantStep.Items.AddRange(new object[] { "下料位置示教" });
                    }

                    if (unLoader != null)
                    {
                        this.RefreshILstAssistantStepWithTransportUnLoader(unLoader);
                    }
                    
                    break;

                // Wafer handing 
                case "WaferHandling":
                    this.selectedControl = new UcWaferHanding(this.TreeListProgramming) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;

                case "WaferHandlingSubNode":
                    MagazineBoxConfig magazineBoxConfig = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig;
                    this.selectedControl = new UcMagazineGeometry() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.ILstAssistantStep.Items.Clear();
                    this.ILstAssistantStep.Items.AddRange(new object[]
                    {
                         "晶圆料盒示教",
                         "换晶圆示教",
                         "晶圆台环限位示教"
                    });

                    this.RefreshILstAssistantStepWithMagazineBox(magazineBoxConfig);

                    break;

                // configuration
                case "Configuration":
                    this.selectedControl = new UcConfigurationDescription() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;

                case "ConfigurationFirNode":

                    this.selectedControl = new UcConfiguration1() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.ILstAssistantStep.Items.Clear();

                    this.ILstAssistantStep.Items.AddRange(new object[]
                    {
                            "点胶头"
                    });

                    break;

                case "ConfigurationSubNode":

                    this.selectedControl = new UcConfiguration2() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.ILstAssistantStep.Items.Clear();

                    if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
                    {
                        this.ILstAssistantStep.Items.AddRange(
                            new object[] { "翻转工具", "点胶头", "吸嘴", "顶针", "晶圆配置", "飞达", "静态华夫盒" });
                    }
                    else
                    {
                        this.ILstAssistantStep.Items.AddRange(
                            new object[] { "点胶头", "吸嘴", "顶针", "晶圆配置", "飞达", "静态华夫盒" });
                    }

                    break;

                // PP Tools
                case "PPTools":
                    this.selectedControl = new UcPPToolsDescription() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    break;

                case "PPToolsSubNode":
                    NozzleShelfSlot nozzleShelfSlot = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as NozzleShelfSlot;

                    Nozzle nozzle = (Nozzle)NozzleRepository.GetInstance().Find(nozzleShelfSlot.NozzleName);

                    this.selectedControl = new UcNozzleEdit(nozzle) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.ILstAssistantStep.Items.Clear();
                    this.ILstAssistantStep.Items.AddRange(new object[]
                    {
                        "吸嘴高度示教",
                        "吸嘴旋转中心示教",
                        "吸嘴几何中心示教"
                    });

                    this.RefreshILstAssistantStepWithNozzle(nozzleShelfSlot);

                    break;

                // ES Tools
                case "ESTools":
                    this.selectedControl = new UcESToolsDescription() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    break;

                case "ESToolsSubNode":
                    EjectionBankSlotConfig ejectionBankSlotConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as EjectionBankSlotConfig;
                    this.selectedControl = new UcESToolsDescription() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.ILstAssistantStep.Items.Clear();

                    this.ILstAssistantStep.Items.AddRange(new object[]
                    {
                        "顶针高度示教",
                        "顶针零位示教",
                        "顶针XY位置示教"
                    });

                    this.RefreshILstAssistantStepWithEjects(ejectionBankSlotConfig);

                    break;

                // FlipTool
                case "FlipTool":
                    this.selectedControl = new UcFlipToolsDescreption() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    break;


                case "FlipToolSubNode":
                    FlipTool flipTool = (FlipTool)FlipToolRepository.GetInstance()
                        .Find(WaferSystemProgram.GetInstance().FlipModuleProgram.FlipToolName);

                    this.selectedControl = new UcFlipToolEdit(flipTool) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.RefreshILstAssistantStepWithFlipTool(flipTool);

                    break;


                case "Dispenser":
                    this.selectedControl = new UcDispenserDescription() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;

                case "DispenserSubNode":
                    Dispenser dispenser = System1Program.GetInstance().DispenserProgram.Dispenser;

                    this.selectedControl = new UcDispenser() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.ILstAssistantStep.Items.Clear();

                    this.ILstAssistantStep.Items.AddRange(new object[]
                                                              {
                                                                  "点胶针示教"
                                                              });

                    this.RefreshILstAssistantStepWithDispenser(dispenser);

                    break;

                // Transport unit
                case "TransportUnit":
                    this.selectedControl = new UcTransportUnitDescription() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    break;

                case "TransportUnitGeneral":

                    if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
                    {
                        this.selectedControl = (XtraUserControl)new FrmTransportUnitEdit() { Dock = DockStyle.Fill };
                        this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                        this.ILstAssistantStep.Items.Clear();
                    }
                    else
                    {
                        TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;

                        this.selectedControl = new UcTransportUnitFrame { Dock = DockStyle.Fill };
                        this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                        this.ILstAssistantStep.Items.Clear();

                        this.ILstAssistantStep.Items.AddRange(new object[]
                                                                  {
                                                                      "框架位置示教",
                                                                      "框架视觉矫正示教",
                                                                      "框架ID识别示教",
                                                                      "框架测高示教",

                                                                      "基板位置示教",
                                                                      "基板视觉矫正示教",
                                                                      "基板墨点示教",
                                                                      "基板ID识别示教",
                                                                      "基板测高示教",

                                                                      "基岛位置示教",
                                                                      "基岛视觉矫正示教",
                                                                      "基岛墨点示教",
                                                                      "基岛测高示教",
                                                                      "不良基岛示教",
                                                                      "Mapping",
                                                                      "基岛ID识别示教",
                                                                  });

                        this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                    }
                    
                    break;

                // Bonding position
                case "BondingPosition":

                    this.selectedControl = new UcAddBondPosition(this.RefreshNode) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    this.RefreshNode();
                    break;

                case "BondingPositionSubNode":
                    SingleBondPositionConfig singleBondPositionConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;
                    this.selectedControl = new UcBondPositionEdit(singleBondPositionConfig) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.ILstAssistantStep.Items.Clear();

                    this.ILstAssistantStep.Items.AddRange(new object[]
                    {
                        "焊点位置示教",
                        "移动到焊点位置",
                        "焊点视觉矫正示教",
                        "焊点测高示教",
                        "焊点ID识别示教"
                    });

                    this.RefreshILstAssistantStepWithBondPosition(singleBondPositionConfig);

                    break;

                // Components
                case "Components":
                    this.selectedControl = new UcComponentsDescription { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;

                case "ComponentsSubNode":
                    BaseCarrierConfig carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;
                    this.selectedControl = new UcComponentEdit(carrierConfig) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.ILstAssistantStep.Items.Clear();

                    this.ILstAssistantStep.Items.AddRange(new object[]
                    {
                        "芯片形状示教",
                        "芯片图示教",
                        "芯片墨点示教",
                        "芯片环形限位",
                        "参考点",
                        "芯片载具形状",
                        "不良芯片图",
                        "芯片矫正",
                        "取芯片",
                        "芯片精度模式",
                        "芯片贴片"
                    });

                    this.RefreshILstAssistantStepWithComponents(carrierConfig);

                    break;

                // Epoxy application
                case "EpoxyApplication":
                    this.selectedControl = new UcAddEpoxyApplication(this.RefreshNode) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;

                case "EpoxyApplicationSubNode":
                    EpoxyApplication epoxyApplication = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as EpoxyApplication;
                    this.selectedControl = new UcEpoxyApplication(epoxyApplication) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;

                // post - bond
                case "PostBond":
                    this.selectedControl = new UcAddPostBond(this.RefreshNode) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    this.RefreshNode();
                    break;

                case "PostBondSubNode":
                    PostBondInspection postBondInspection = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as PostBondInspection;
                    this.selectedControl = new UcPostBondEdit(postBondInspection) { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);

                    this.RefreshILstAssistantStepWithPostBond(postBondInspection);

                    break;

                // processing list
                case "ProcessingList":
                    //this.selectedControl = new UcProcessStepEdit { Dock = DockStyle.Fill };
                    //this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    this.selectedControl = new UcProcessStep() { Dock = DockStyle.Fill };
                    this.SpiltContainerProgramming.Panel1.Controls.Add(this.selectedControl);
                    break;            
            }

            // 貌似只有 ILstAssistantStep 里有条目 才能启用 BtReset 按钮，先这么写。
            this.BtReset.Enabled = this.ILstAssistantStep.Items.Count > 0;
        }

        /// <summary>
        /// Start事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtStart_Click(object sender, System.EventArgs e)
        {
            // 确定
            (this.selectedControl as IProgrammingControl)?.Confirm();
            this.Start(true);
        }

        /// <summary>
        /// System1 和System2 的切换
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbSystem_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            // System1
            if (this.CmbSystem.SelectedIndex == 0)
            {
                MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;
                this.SortNodesUI();
            }
            else
            {
                MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;
                this.SortNodesUI();
            }
        }

        /// <summary>
        /// 示教步骤 Index改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LstAssisantStep_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        /// <summary>
        /// 绘制 List Box Item 的Apperance
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LstAssisantStep_DrawItem(object sender, DevExpress.XtraEditors.ListBoxDrawItemEventArgs e)
        {
            if (e.State == DrawItemState.Selected)
            {
                e.Appearance.BackColor = Color.Aquamarine;
            }
        }

        /// <summary>
        /// Close
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtClose_Click(object sender, System.EventArgs e)
        {
            this.ParentForm?.Close();

            // 示教完成转为准备状态
            Machine.GetInstance().SetState(MachineStateEnum.Stop);
        }

        /// <summary>
        /// 重置按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtReset_Click(object sender, System.EventArgs e)
        {
            // 芯片
            BaseCarrierConfig carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;
            if (carrierConfig != null)
            {
                carrierConfig.ResetAssistantStates();
                CarrierConfigRepository.GetInstance().Save();

                this.RefreshILstAssistantStepWithComponents(carrierConfig);
            }

            // 顶针
            EjectionBankSlotConfig ejectionBankSlotConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as EjectionBankSlotConfig;
            if (ejectionBankSlotConfig != null)
            {
                ejectionBankSlotConfig.EjectionConfig.ResetAssistantStates();
                EjectionConfigRepository.GetInstance().Save();

                this.RefreshILstAssistantStepWithEjects(ejectionBankSlotConfig);
            }

            // WaferHandling
            if (this.TreeListProgramming.FocusedNode.ParentNode.Tag.ToString() == "WaferHandling")
            {
                MagazineBoxConfig magazineBoxConfig = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig;
                if (magazineBoxConfig != null)
                {
                    magazineBoxConfig.ResetAssistantStates();
                    MagazineBoxConfigRepository.GetInstance().Save();

                    this.RefreshILstAssistantStepWithMagazineBox(magazineBoxConfig);
                }
            }

            // TransportUnit
            if (this.TreeListProgramming.FocusedNode.ParentNode.Tag.ToString() == "TransportUnit")
            {
                TransportUnitConfig transportUnitConfig = ProductConfiguration.GetInstance().TransportUnitConfig;
                if (transportUnitConfig != null)
                {
                    transportUnitConfig.ResetAssistantStates();
                    ProductConfiguration.GetInstance().Save();

                    this.RefreshILstAssistantStepWithTransportUnit(transportUnitConfig);
                }
            }

            // Dispenser
            if (this.TreeListProgramming.FocusedNode.ParentNode.Tag.ToString() == "Dispenser")
            {
                Dispenser dispenser = System1Program.GetInstance().DispenserProgram.Dispenser;
                if (dispenser != null)
                {
                    dispenser.ResetAssistantStates();
                    DispenserRepository.GetInstance().Save();
                }
            }

            // 吸嘴
            NozzleShelfSlot nozzleShelfSlot = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as NozzleShelfSlot;
            if (nozzleShelfSlot != null)
            {
                nozzleShelfSlot.Nozzle.ResetAssistantStates();
                NozzleRepository.GetInstance().Save();

                this.RefreshILstAssistantStepWithNozzle(nozzleShelfSlot);
            }

            // PostBond
            PostBondInspection postBondInspection = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as PostBondInspection;
            if (postBondInspection != null)
            {
                postBondInspection.ResetAssistantStates();
                PostBondInspectionRepository.GetInstance().Save();

                this.RefreshILstAssistantStepWithPostBond(postBondInspection);
            }

            // BondPosition
            SingleBondPositionConfig singleBondPositionConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;
            if (singleBondPositionConfig != null)
            {
                singleBondPositionConfig.ResetAssistantStates();
                ProductConfiguration.GetInstance().Save();

                this.RefreshILstAssistantStepWithBondPosition(singleBondPositionConfig);
            }

            LoaderBin loader = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as LoaderBin;
            if (loader != null)
            {
                loader.ResetAssistantStates();
                LoaderBinRepository.GetInstance().Save();

                this.RefreshILstAssistantStepWithTransportLoader(loader);
            }

            UnloaderBin unLoader = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as UnloaderBin;
            if (unLoader != null)
            {
                unLoader.ResetAssistantStates();
                UnLoaderBinRepository.GetInstance().Save();

                this.RefreshILstAssistantStepWithTransportUnLoader(unLoader);
            }

            this.RefreshNode();
            this.UnFocusedWithILstAssistantStep();
        }

        /// <summary>
        /// UnFocusedWithILstAssistantStep
        /// </summary>
        private void UnFocusedWithILstAssistantStep()
        {
            this.ILstAssistantStep.SelectedIndex = -1;

            if (this.TreeListProgramming?.FocusedNode?.ParentNode?.Tag?.ToString() != "TransportUnit")
            {
                this.ILstAssistantStep.Items.Clear();
            }
        }

        /// <summary>
        /// 保存
        /// 主要是为了保存之后不进入示教界面
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                (this.selectedControl as IProgrammingControl)?.Confirm();

                AKRSXtraMessageBox.Show("保存成功");
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcEditProductProgramming_Load(object sender, System.EventArgs e)
        {
            Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming.RefreshILstAssistantStepWithComponentsAction +=
                this.RefreshILstAssistantStepWithComponents;
            Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming.RefreshILstAssistantStepWithTuAction +=
                this.RefreshILstAssistantStepWithTransportUnit;
            Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming.RefreshILstAssistantStepWithBpAction +=
                this.RefreshILstAssistantStepWithBondPosition;
            Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming.RefreshILstAssistantStepWithLoader +=
                this.RefreshILstAssistantStepWithTransportLoader;
            Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming.RefreshILstAssistantStepWithUnLoader +=
                this.RefreshILstAssistantStepWithTransportUnLoader;
            Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram.UcEditProductProgramming.RefreshILstAssistantStepWithPBAction +=
                this.RefreshILstAssistantStepWithPostBond;

            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated 
                && !MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                var dispenserTreeListNode = this.TreeListProgramming.Nodes.Find(it => it.Tag.ToString() == "Dispenser");
                this.TreeListProgramming.Nodes.Remove((TreeListNode)dispenserTreeListNode);
                
                var epoxyTreeListNode = this.TreeListProgramming.Nodes.Find(it => it.Tag.ToString() == "EpoxyApplication");
                this.TreeListProgramming.Nodes.Remove((TreeListNode)epoxyTreeListNode);
                
            }

            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated == false)
            {
                // 翻转工具
                TreeListNode flipToolNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "FlipTool");

                // 移除节点
                this.TreeListProgramming.Nodes.Remove(flipToolNode);
                //flipToolNode.Nodes.Clear();
            }

            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.CmbSystem.SelectedIndex = 1;
                this.CmbSystem.Visible = false;
            }

            if (!MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                this.CmbSystem.SelectedIndex = 1;
                this.CmbSystem.Visible = false;
            }
        }

        /// <summary>
        /// Start
        /// </summary>
        /// <param name="isContinue">是否连续</param>
        private void Start(bool isContinue = false)
        {
            try
            {
                // ---------------------------如果遇到这些节点，点击Start 直接跳到下一个节点-----------------------
                TreeListNode focusNode = this.TreeListProgramming.FocusedNode;

                if (focusNode?.Tag != null)
                {
                    string tagStr = focusNode.Tag?.ToString();
                    if (focusNode.Tag is NodeTag tag)
                    {
                        tagStr = tag.TagName;
                    }

                    switch (tagStr)
                    {
                        case "TransportSystem":
                        case "BeltSystem1":
                        case "BeltSystem2":
                        case "WaferHandling":
                        case "Configuration":
                       // case "ConfigurationFirNode":
                        case "PPTools":
                        case "ESTools":
                        case "Dispenser":
                        case "TransportUnit":
                        case "BondingPosition":
                        case "Components":
                        case "EpoxyApplication":
                        case "PostBond":
                        case "ProcessingList":
                            this.TreeListProgramming.MoveNext();
                            break;

                            #region 注释
                            //case "WaferHandlingSubNode":
                            //    selectedControl = new UcMagazineGeometry() { Dock = DockStyle.Fill };
                            //    break;

                            //case "ConfigurationSubNode":
                            //    selectedControl = new UcConfiguration2() { Dock = DockStyle.Fill };
                            //    break;

                            //case "PPToolsSubNode":
                            //    NozzleShelfSlot nozzleShelfSlot = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as NozzleShelfSlot;

                            //    Nozzle nozzle = (Nozzle)NozzleRepository.GetInstance().Find(nozzleShelfSlot.NozzleName);

                            //    selectedControl = new UcNozzleEdit(nozzle) { Dock = DockStyle.Fill };

                            //    break;

                            //case "ESToolsSubNode":
                            //    selectedControl = new UcESToolsDescription() { Dock = DockStyle.Fill };
                            //    break;

                            //case "DispenserSubNode":
                            //    this.selectedControl = new UcDispenser() { Dock = DockStyle.Fill };
                            //    break;

                            //case "TransportUnitGeneral":
                            //    selectedControl = new UcTransportUnitFrame { Dock = DockStyle.Fill };
                            //    break;

                            //case "BondingPositionSubNode":
                            //    SingleBondPositionConfig singleBondPositionConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as SingleBondPositionConfig;
                            //    selectedControl = new UcBondPositionEdit(singleBondPositionConfig) { Dock = DockStyle.Fill };
                            //    break;

                            //case "ComponentsSubNode":
                            //    BaseCarrier carrier = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrier;
                            //    selectedControl = new UcComponentEdit(carrier) { Dock = DockStyle.Fill };
                            //    break;

                            //case "EpoxyApplicationSubNode":
                            //    EpoxyApplication epoxyApplication = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as EpoxyApplication;
                            //    selectedControl = new UcEpoxyApplication(epoxyApplication) { Dock = DockStyle.Fill };
                            //    break;

                            //case "PostBondSubNode":
                            //    PostBondInspection postBondInspection = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as PostBondInspection;
                            //    selectedControl = new UcPostBondEdit(postBondInspection) { Dock = DockStyle.Fill };
                            //    SpiltContainerProgramming.Panel1.Controls.Add(selectedControl);
                            //    break;
                            #endregion
                    }

                    // -------------------------Lst 条目---------------------------------------
                    string lstItemStr = this.ILstAssistantStep.SelectedValue?.ToString();

                    switch (lstItemStr)
                    {
                        case "上料位置示教":

                            // todo:?
                            LoaderBin loader = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as LoaderBin;

                            FrmLoadingAssistance frmLoadingAssistance = new FrmLoadingAssistance();
                            frmLoadingAssistance.ShowDialog();
                            frmLoadingAssistance.Dispose();
                            this.RefreshILstAssistantStepWithTransportLoader(loader);
                            if (frmLoadingAssistance.DialogResult == DialogResult.OK)
                            {
                                loader.Loader.State = AssistantStateEnum.Able;
                                LoaderBinRepository.GetInstance().Save();

                                this.RefreshNode();
                                this.UnFocusedWithILstAssistantStep();
                            }

                            break;

                        case "下料位置示教":
                            UnloaderBin unLoader = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as UnloaderBin;

                            FrmUnLoadAssistance frmUnLoadAssistance = new FrmUnLoadAssistance();
                            frmUnLoadAssistance.ShowDialog();
                            frmUnLoadAssistance.Dispose();
                            this.RefreshILstAssistantStepWithTransportUnLoader(unLoader);
                            if (frmUnLoadAssistance.DialogResult == DialogResult.OK)
                            {
                                unLoader.Unloader.State = AssistantStateEnum.Able;
                                UnLoaderBinRepository.GetInstance().Save();

                                this.RefreshNode();
                                this.UnFocusedWithILstAssistantStep();
                            }

                            break;

                        // Wafer handling
                        case "晶圆料盒示教":
                            MagazineBoxConfig magazineBoxConfig = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig;
                            FrmWaferMagazineGeoTeach frmWaferMagazineGeoTeach = new FrmWaferMagazineGeoTeach();
                            frmWaferMagazineGeoTeach.ShowDialog();

                            this.RefreshILstAssistantStepWithMagazineBox(magazineBoxConfig);
                            if (frmWaferMagazineGeoTeach.DialogResult == DialogResult.OK)
                            {
                                this.Start();
                            }

                            frmWaferMagazineGeoTeach.Dispose();

                            break;

                        case "换晶圆示教":
                            magazineBoxConfig = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig;
                            FrmWaferChangeTeach frmWaferChangeTeach = new FrmWaferChangeTeach();
                            frmWaferChangeTeach.ShowDialog();

                            this.RefreshILstAssistantStepWithMagazineBox(magazineBoxConfig);
                            if (frmWaferChangeTeach.DialogResult == DialogResult.OK)
                            {
                                this.Start();
                            }

                            frmWaferChangeTeach.Dispose();

                            // 确认是否控制magazine去安全位
                            DialogResult res = AKRSXtraMessageBox.Show("确认magazine位置安全，是否控制magazine去安全位?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            switch (res)
                            {
                                case DialogResult.Yes:
                                    WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition();
                                    break;
                                case DialogResult.No:
                                    break;
                            }

                            break;

                        case "晶圆台环限位示教":
                            magazineBoxConfig = WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig;
                            FrmWaferTableCollisionCircleTeach frmWaferTableCollisionCircleTeach =
                                new FrmWaferTableCollisionCircleTeach();
                            frmWaferTableCollisionCircleTeach.ShowDialog();

                            this.RefreshILstAssistantStepWithMagazineBox(magazineBoxConfig);
                            if (frmWaferTableCollisionCircleTeach.DialogResult == DialogResult.OK)
                            {
                                this.RefreshNode();
                                this.UnFocusedWithILstAssistantStep();
                            }

                            frmWaferTableCollisionCircleTeach.Dispose();

                            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule == false)
                            {
                                // 王杰提出-复位顶针台（排除意外因素）
                                WaferSubModule.GetInstance().Eject.EjectionTableAxisZ.GoHome();
                            }    
               

                            break;

                        // Cofiguration
                        case "点胶头":
                            FrmDispenseOverAllSettings frmDispenseOverAllSettings = new FrmDispenseOverAllSettings();
                            frmDispenseOverAllSettings.ShowDialog();

                            this.RefreshNode();

                            break;

                        case "翻转工具":
                            FrmNewCreateFlipTool frmNewCreateFlipTool = new FrmNewCreateFlipTool();
                            frmNewCreateFlipTool.StartPosition = FormStartPosition.CenterParent;
                            frmNewCreateFlipTool.ShowDialog();

                            frmNewCreateFlipTool.Dispose();

                            // 刷新Config 界面
                            (this.selectedControl as UcConfiguration2)?.RefreshControl();

                            // 刷新PP Tool子节点
                            this.RefreshNode();
                            break;

                        case "吸嘴":
                            FrmNozzleShelfSetting frmNozzleShelfSetting = new FrmNozzleShelfSetting();
                            frmNozzleShelfSetting.ShowDialog();

                            frmNozzleShelfSetting.Dispose();

                            // 刷新Config 界面
                            (this.selectedControl as UcConfiguration2)?.RefreshControl();

                            // 刷新PP Tool子节点
                            this.RefreshNode();
                            break;

                        case "顶针":

                            WaferSubController.GetInstance().EjectController.InitCurrentSlotConfig();

                            FrmEjectionSetting frmEjectionSetting = new FrmEjectionSetting();
                            frmEjectionSetting.ShowDialog();
                            frmEjectionSetting.Dispose();

                            // 刷新Config 界面
                            (this.selectedControl as UcConfiguration2)?.RefreshControl();
                            this.RefreshNode();

                            break;

                        case "晶圆配置":
                            WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory(true, false);
                            FrmMagazineFilling frmMagazineFilling = new FrmMagazineFilling();
                            frmMagazineFilling.ShowDialog();
                            frmMagazineFilling.Dispose();

                            (this.selectedControl as UcConfiguration2)?.RefreshControl();
                            this.RefreshNode();

                            break;

                        case "飞达":
                            AKRSXtraMessageBox.Show("no open", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "静态华夫盒":

                            if (!MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
                            {
                                DialogResult dialog = AKRSXtraMessageBox.Show(
                                    $"Static waffle is not configured!",
                                    "Warn",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                return;
                            }

                            WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory(false, true);
                            FrmStaticWaffleConfiguration frmStaticWaffleConfiguration = new FrmStaticWaffleConfiguration();
                            frmStaticWaffleConfiguration.ShowDialog();
                            frmStaticWaffleConfiguration.Dispose();

                            (this.selectedControl as UcConfiguration2)?.RefreshControl();
                            this.RefreshNode();
                            break;

                        case "翻转工具示教":

                            this.AssistanceFlipTool();

                            break;

                        // PP tools 
                        case "吸嘴高度示教":
                            this.AssistanceToolHeight();

                            break;

                        case "吸嘴旋转中心示教":
                            this.AssistanceToolAlignment();

                            break;

                        case "吸嘴几何中心示教":
                            this.AssistanceToolGeometry();

                            break;

                        // ES Tool
                        case "顶针高度示教":
                            EjectionBankSlotConfig ejectionBankSlotConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as EjectionBankSlotConfig;

                            if (MachineStateModel.GetInstance().IsOffLineWork)
                            {
                                FrmHeightMeasurementTeach frmHeightMeasurementTeach = new FrmHeightMeasurementTeach(ejectionBankSlotConfig);
                                frmHeightMeasurementTeach.ShowDialog();

                                this.RefreshILstAssistantStepWithEjects(ejectionBankSlotConfig);
                                if (frmHeightMeasurementTeach.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }
                                else
                                {
                                    WaferSubController.GetInstance().EjectController.ReturnEjection();
                                }

                                frmHeightMeasurementTeach.Dispose();

                                return;
                            }

                            // 自动换Touchdown
                            if (this.system2Controller.ChangeTouchDownAssistance())
                            {
                                System2Domain.GetInstance().BondModuleController.MoveToPickupPos();
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();
                                WaferSubController.GetInstance().EjectController.ChangeEjection(ejectionBankSlotConfig.Index, true);

                                FrmHeightMeasurementTeach frmHeightMeasurementTeach = new FrmHeightMeasurementTeach(ejectionBankSlotConfig);
                                frmHeightMeasurementTeach.ShowDialog();

                                this.system2Controller.PutbackNozzleAssitance();

                                this.RefreshILstAssistantStepWithEjects(ejectionBankSlotConfig);
                                if (frmHeightMeasurementTeach.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }
                                else
                                {                                                               
                                    WaferSubController.GetInstance().EjectController.ReturnEjection();
                                }

                                frmHeightMeasurementTeach.Dispose();
                            }
                            
                            break;

                        case "顶针零位示教":
                            ejectionBankSlotConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as EjectionBankSlotConfig;

                            if (!MachineStateModel.GetInstance().IsOffLineWork)
                            {
                                System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                            }

                            if (isContinue)
                            {
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();
                                WaferSubController.GetInstance().EjectController.ChangeEjection(ejectionBankSlotConfig.Index, true);
                            }
                            
                            WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();

                            FrmNeedleZeroPositionTeach frmNeedleZeroPositionTeach =
                                new FrmNeedleZeroPositionTeach(ejectionBankSlotConfig);
                            frmNeedleZeroPositionTeach.ShowDialog();

                            this.RefreshILstAssistantStepWithEjects(ejectionBankSlotConfig);
                            if (frmNeedleZeroPositionTeach.DialogResult == DialogResult.OK)
                            {
                                this.Start();
                            }
                            else
                            {
                                WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                                WaferSubController.GetInstance().EjectController.ReturnEjection();
                            }

                            frmNeedleZeroPositionTeach.Dispose();

                            break;

                        case "顶针XY位置示教":
                            ejectionBankSlotConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as EjectionBankSlotConfig;

                            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

                            if (!MachineStateModel.GetInstance().IsOffLineWork)
                            {
                                if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
                                {
                                    System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                                }
                                else
                                {
                                    this.bondModuleController.MoveSafeBondXYZ(new AKRSPoint3D(CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.X, CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.Y, 0) -
                                                                              new AKRSPoint3D(CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.X, CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.Y, 0));
                                }                                
                            }

                            if (isContinue)
                            {
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();
                                WaferSubController.GetInstance().EjectController.ChangeEjection(ejectionBankSlotConfig.Index, true);
                            }                                                          

                            FrmXYPositionTeach frmXyPositionTeach = new FrmXYPositionTeach(ejectionBankSlotConfig);
                            frmXyPositionTeach.ShowDialog();
                            System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                            this.RefreshILstAssistantStepWithEjects(ejectionBankSlotConfig);
                            if (frmXyPositionTeach.DialogResult == DialogResult.OK)
                            {
                                this.RefreshNode();
                                this.UnFocusedWithILstAssistantStep();
                            }

                            frmXyPositionTeach.Dispose();

                            WaferSubController.GetInstance().WaferTableController.CloseLight();
                            WaferSubController.GetInstance().EjectController.ReturnEjection();

                            break;

                        // Transport Unit general
                        case "框架位置示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("框架位置示教"))
                            {
                                return;
                            }

                            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                            {
                                if (!this.system2Controller.ChangeTouchDownAssistance())
                                {
                                    return;
                                }
                            }

                            FrmTUPositionTeach frmTuPositionTeach = new FrmTUPositionTeach();
                            frmTuPositionTeach.ShowDialog();

                            this.RefreshILstAssistantStepWithTransportUnit(this.TransportUnitConfig);
                            if (frmTuPositionTeach.DialogResult == DialogResult.OK)
                            {
                                this.Start();
                            }

                            frmTuPositionTeach.Dispose();

                            break;

                        case "框架视觉矫正示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("框架视觉矫正示教"))
                            {
                                return;
                            }

                            this.AssistanceTuAdjust();
                            break;

                        case "框架ID识别示教":
                            this.AssistanceTuIdentity();
                            break;

                        case "框架测高示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("框架测高示教"))
                            {
                                return;
                            }

                            this.AssistanceMeasureHeightTu();
                            break;

                        case "基板位置示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("基板位置示教"))
                            {
                                return;
                            }

                            FrmSubstratePositionTeach frmSubstratePositionTeach = new FrmSubstratePositionTeach();
                            frmSubstratePositionTeach.ShowDialog();

                            this.RefreshILstAssistantStepWithTransportUnit(this.TransportUnitConfig);
                            if (frmSubstratePositionTeach.DialogResult == DialogResult.OK)
                            {
                                this.Start();
                            }

                            frmSubstratePositionTeach.Dispose();

                            break;

                        case "基板视觉矫正示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("基板视觉矫正示教"))
                            {
                                return;
                            }

                            this.AssistanceSubAdjust();
                            break;

                        case "基板墨点示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("基板墨点示教"))
                            {
                                return;
                            }

                            AKRSXtraMessageBox.Show("没有打开", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "基板ID识别示教":
                            this.AssistanceSubIdentity();
                            break;

                        case "基板测高示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("基板测高示教"))
                            {
                                return;
                            }

                            this.AssistanceMeasureHeightSu();
                            break;

                        case "基岛位置示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("基岛位置示教"))
                            {
                                return;
                            }

                            FrmModulePositionTeach frmModulePositionTeach = new FrmModulePositionTeach();
                            frmModulePositionTeach.ShowDialog();

                            if (frmModulePositionTeach.DialogResult == DialogResult.OK)
                            {
                                this.RefreshILstAssistantStepWithTransportUnit(this.TransportUnitConfig);
                                this.Start();
                            }

                            frmModulePositionTeach.Dispose();

                            break;

                        case "基岛视觉矫正示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("基岛视觉矫正示教"))
                            {
                                return;
                            }

                            this.AssistanceModuleAdjust();
                            break;

                        case "基岛墨点示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("基岛墨点示教"))
                            {
                                return;
                            }

                            AKRSXtraMessageBox.Show("没有打开", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "基岛测高示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("基岛测高示教"))
                            {
                                return;
                            }

                            this.AssistanceMeasureHeightModule();
                            break;

                        case "不良基岛示教":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("不良基岛示教"))
                            {
                                return;
                            }

                            AKRSXtraMessageBox.Show("没有打开", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "Mapping":

                            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsNormalOrderAssistant("Mapping"))
                            {
                                return;
                            }

                            this.RefreshNode();
                            this.UnFocusedWithILstAssistantStep();
                            break;

                        case "基岛ID识别示教":
                            this.AssistanceModuleIdentity();
                            break;

                        // Bond position
                        case "焊点位置示教":
                            this.AssistanceTeachBondingPosition();

                            break;

                        case "移动到焊点位置":
                            this.AssistanceMoveToBondingPosition();

                            break;

                        case "焊点视觉矫正示教":
                            this.AssistanceBondPositionAdjust();
                            break;

                        case "焊点测高示教":
                            this.AssistanceBondPositionMeasureHeight();
                            break;

                        case "焊点ID识别示教":
                            this.AssistanceBondPositionIdentity();
                            break;

                        // Components
                        case "芯片形状示教":

                            BaseCarrierConfig carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;
                            //WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory();

                            //if (!MachineStateModel.GetInstance().IsOffLineWork)
                            //{
                            //    System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                            //}

                            // 智能化
                            WaferSubController.GetInstance().EjectController.ReturnEjection();
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();
                        
                            //FrmChangeTabletTeach frmChangeTabletTeach = new FrmChangeTabletTeach();
                            //frmChangeTabletTeach.Dispose();
                            //if (frmChangeTabletTeach.DialogResult != DialogResult.OK)
                            //{
                            //    break;
                            //}

                            if (carrierConfig is CarrierWithWaferConfig waferCarrier)
                            {
                                //if (WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Exists(item => item.Name == waferCarrier.EjectionName) && waferCarrier.EjectionName != string.Empty)
                                //{
                                //    EjectionBankSlotConfig tarBankSlotConfig = (EjectionBankSlotConfig)WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Find(item => item.Name == waferCarrier.EjectionName);                                    
                                //    WaferSubController.GetInstance().EjectController.ChangeEjection(tarBankSlotConfig.Index);         
                                //}
                                //else
                                //{
                                //    throw new Exception($"This Ejection does not exist in the current EjectionBank!");
                                //}

                                FrmComponentGeometryWithWaferTeach frm = new FrmComponentGeometryWithWaferTeach(waferCarrier);
                                frm.ShowDialog();

                                this.RefreshILstAssistantStepWithComponents(carrierConfig);
                                if (frm.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }

                                frm.Dispose();
                            }
                            else if (carrierConfig is CarrierWithWaffleConfig waffleCarrier)
                            {
                                FrmComponentGeometryWithWaffleTeach frm = new FrmComponentGeometryWithWaffleTeach(waffleCarrier);
                                frm.ShowDialog();

                                this.RefreshILstAssistantStepWithComponents(carrierConfig);
                                if (frm.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }

                                frm.Dispose();
                            }

                            break;

                        case "翻转工具取晶位示教":
                             carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;

                             FrmFlipToWaferTeach frmFlipToWaferTeach = new FrmFlipToWaferTeach(carrierConfig);
                             frmFlipToWaferTeach.ShowDialog();

                             this.RefreshILstAssistantStepWithComponents(carrierConfig);
                             if (frmFlipToWaferTeach.DialogResult == DialogResult.OK)
                             {
                                 this.Start();
                             }

                             frmFlipToWaferTeach.Dispose();

                            break;

                        case "芯片图示教":

                            carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;
                            //WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory();
                            if (carrierConfig is CarrierWithWaferConfig waferCarrier4)
                            {
                                AKRSXtraMessageBox.Show("no open", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //FrmComponentMap frmComponentMap = new FrmComponentMap(waferCarrier4);
                                //frmComponentMap.ShowDialog();
                                //if (frmComponentMap.DialogResult == DialogResult.OK)
                                //{
                                //    this.Start();
                                //}
                            }
                            else if (carrierConfig is CarrierWithWaffleConfig waffleCarrier)
                            {
                                AKRSXtraMessageBox.Show("no open", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            break;

                        case "芯片墨点示教":

                            carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;
                            //WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory();
                            if (carrierConfig is CarrierWithWaferConfig waferCarrier1)
                            {
                                FrmInkDotWithWaferTeach frm = new FrmInkDotWithWaferTeach(waferCarrier1);
                                frm.ShowDialog();

                                this.RefreshILstAssistantStepWithComponents(carrierConfig);
                                if (frm.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }

                                frm.Dispose();
                            }
                            else if (carrierConfig is CarrierWithWaffleConfig waffleCarrier)
                            {
                                FrmInkDotWithWaffleTeach frm = new FrmInkDotWithWaffleTeach(waffleCarrier);
                                frm.ShowDialog();

                                this.RefreshILstAssistantStepWithComponents(carrierConfig);
                                if (frm.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }

                                frm.Dispose();
                            }

                            break;

                        case "芯片环形限位":
                            carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;
                            //WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory();
                            if (carrierConfig is CarrierWithWaferConfig waferCarrier2)
                            {
                                FrmWaferEdgeWithWaferTeach frm =
                                    new FrmWaferEdgeWithWaferTeach(waferCarrier2);
                                frm.ShowDialog();

                                this.RefreshILstAssistantStepWithComponents(carrierConfig);
                                if (frm.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }

                                frm.Dispose();
                            }

                            break;

                        case "参考点":
                            AKRSXtraMessageBox.Show("no open", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "芯片载具形状":

                            carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;
                            //WaferSubController.GetInstance().WaferTableController.ClearWaferSubMemory();
                            if (carrierConfig is CarrierWithWaferConfig waferCarrier3)
                            {
                                AKRSXtraMessageBox.Show("no open", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //FrmComponentCarrierGeometryWithWaferTeach frmComponentCarrierGeometryWithWaferTeach =
                                //    new FrmComponentCarrierGeometryWithWaferTeach(waferCarrier3);
                                //frmComponentCarrierGeometryWithWaferTeach.ShowDialog();
                            }
                            else if (carrierConfig is CarrierWithWaffleConfig waffleCarrier3)
                            {
                                if (!MachineStateModel.GetInstance().IsOffLineWork)
                                {
                                    if (string.IsNullOrEmpty(waffleCarrier3.NozzleName))
                                    {
                                        DialogResult dialog = AKRSXtraMessageBox.Show(
                                            $"请先选择吸嘴再进行示教！" ,
                                            "提示",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Warning);

                                        return;
                                    }


                                    if (!System2Domain.GetInstance().System2Controller.ChangeNozzleAssistance(waffleCarrier3.NozzleName))
                                    {
                                        break;
                                    }
                                }

                                FrmComponentCarrierGeometryWithWaffleTeach frm =
                                    new FrmComponentCarrierGeometryWithWaffleTeach(waffleCarrier3);
                                frm.ShowDialog();

                                this.RefreshILstAssistantStepWithComponents(carrierConfig);
                                if (frm.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }

                                frm.Dispose();
                            }

                            break;

                        case "不良芯片图":
                            AKRSXtraMessageBox.Show("no open", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "芯片矫正":

                            //carrier = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrier;
                            //if (carrier is CarrierWithWafer waferCarrier4)
                            //{
                            //    FrmComponentCarrierAdjustWithWaferTeach frmComponentCarrierAdjustWithWaferTeach =
                            //    new FrmComponentCarrierAdjustWithWaferTeach(waferCarrier4);
                            //    frmComponentCarrierAdjustWithWaferTeach.ShowDialog();
                            //}

                            AKRSXtraMessageBox.Show("no open", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            break;

                        case "取芯片":
                            carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;

                            FrmComponentPickup frmComponentPickup = new FrmComponentPickup(carrierConfig);
                            frmComponentPickup.ShowDialog();

                            this.RefreshILstAssistantStepWithComponents(carrierConfig);
                            if (frmComponentPickup.DialogResult == DialogResult.OK)
                            {
                                this.Start();
                            }

                            frmComponentPickup.Dispose();

                            break;

                        case "芯片精度模式":
                            carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;

                            FrmComponentAccuracyMode frmComponentAccuracyMode = new FrmComponentAccuracyMode(carrierConfig);
                            if (frmComponentAccuracyMode.IsShowDialog())
                            {
                                frmComponentAccuracyMode.ShowDialog();

                                this.RefreshILstAssistantStepWithComponents(carrierConfig);
                                if (frmComponentAccuracyMode.DialogResult == DialogResult.OK)
                                {
                                    this.Start();
                                }

                                frmComponentAccuracyMode.Dispose();
                            }

                            break;

                        case "芯片贴片":
                            carrierConfig = (this.TreeListProgramming.FocusedNode.Tag as NodeTag)?.TagObject as BaseCarrierConfig;

                            FrmComponentPlacement frmComponentPlacement = new FrmComponentPlacement(carrierConfig);
                            frmComponentPlacement.ShowDialog();

                            this.RefreshILstAssistantStepWithComponents(carrierConfig);
                            if (frmComponentPlacement.DialogResult == DialogResult.OK)
                            {
                                this.RefreshNode();
                                this.UnFocusedWithILstAssistantStep();
                            }

                            frmComponentPlacement.Dispose();

                            break;

                        // Post-bond
                        case "检测示教":
                            this.AssistancePostBond();
                            break;


                        // 背崩检测
                        case "背崩检测":
                            this.AssistanceBacksideCrackDetection();
                            break;

                        case "点胶针示教":
                            if (this.CmbSystem.SelectedIndex == 1 && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
                            {
                                Dispenser dispenser = BondProgram.GetInstance().S2DispenserProgram.Dispenser;
                                FrmAssistantDispense frmAssistantDispense = new FrmAssistantDispense();
                                frmAssistantDispense.ShowDialog();

                                this.RefreshILstAssistantStepWithDispenser(dispenser);
                                if (frmAssistantDispense.DialogResult == DialogResult.OK)
                                {
                                    this.RefreshNode();
                                    this.UnFocusedWithILstAssistantStep();
                                }

                                frmAssistantDispense.Dispose();
                            }
                            else
                            {
                                Dispenser dispenser = System1Program.GetInstance().DispenserProgram.Dispenser;
                                FrmChangeEpoxy frmChangeEpoxy = new FrmChangeEpoxy();
                                frmChangeEpoxy.ShowDialog();

                                this.RefreshILstAssistantStepWithDispenser(dispenser);
                                if (frmChangeEpoxy.DialogResult == DialogResult.OK)
                                {
                                    this.RefreshNode();
                                    this.UnFocusedWithILstAssistantStep();
                                }

                                frmChangeEpoxy.Dispose();
                            }

                            break;
                    }
                }
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtSkip_Click(object sender, EventArgs e)
        {

        }
    }

    /// <summary>
    /// 节点Tag
    /// </summary>
    public class NodeTag
    {
        /// <summary>
        /// Tag 名称
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Tag 对象
        /// </summary>
        public object TagObject { get; set; }
    }
}