namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram
{
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Models;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using DevExpress.XtraTreeList.Nodes;
    using System;

    /// <summary>
    /// UcEditProductProgramming
    /// </summary>
    public partial class UcEditProductProgramming
    {
        /// <summary>
        /// RefreshILstAssistantStepWithComponentsAction
        /// </summary>
        public static Action<BaseCarrierConfig> RefreshILstAssistantStepWithComponentsAction { get; set; }

        /// <summary>
        /// RefreshILstAssistantStepWithTuAction
        /// </summary>
        public static Action<TransportUnitConfig> RefreshILstAssistantStepWithTuAction { get; set; }

        /// <summary>
        /// RefreshILstAssistantStepWithBpAction
        /// </summary>
        public static Action<SingleBondPositionConfig> RefreshILstAssistantStepWithBpAction { get; set; }

        /// <summary>
        /// RefreshILstAssistantStepWithUnLoader
        /// </summary>
        public static Action<LoaderBin> RefreshILstAssistantStepWithLoader { get; set; }

        /// <summary>
        /// RefreshILstAssistantStepWithBpAction
        /// </summary>
        public static Action<UnloaderBin> RefreshILstAssistantStepWithUnLoader { get; set; }

        /// <summary>
        /// RefreshILstAssistantStepWithPBAction
        /// </summary>
        public static Action<PostBondInspection> RefreshILstAssistantStepWithPBAction { get; set; }


        /// <summary>
        /// RefreshILstAssistantStepWithTransportUnit
        /// </summary>
        /// <param name="transportProgram">传送程式</param>
        private void RefreshILstAssistantStepWithTransportLoader(LoaderBin loader)
        {
            this.ILstAssistantStep.Items.Clear();

            this.ILstAssistantStep.Items.AddRange(new object[] { "上料位置示教" });

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = loader.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!loader.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 1;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = loader.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithTransportUnit
        /// </summary>
        /// <param name="transportProgram">传送程式</param>
        private void RefreshILstAssistantStepWithTransportUnLoader(UnloaderBin unLoader)
        {
            this.ILstAssistantStep.Items.Clear();

            this.ILstAssistantStep.Items.AddRange(new object[] { "下料位置示教" });

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = unLoader.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!unLoader.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = unLoader.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithComponents
        /// </summary>
        /// <param name="carrierConfig">carrierConfig</param>
        private void RefreshILstAssistantStepWithComponents(BaseCarrierConfig carrierConfig)
        {
            this.ILstAssistantStep.Items.Clear();

            this.ILstAssistantStep.Items.AddRange(new object[]
                                                      {
                                                          "芯片形状示教",
                                                          "翻转工具取晶位示教",
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

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();

                if (carrierConfig.FlipToWafer == null)
                {
                    carrierConfig.FlipToWafer = new AssistantState() { Name = "Flip to wafer", State = AssistantStateEnum.ForBidden };
                }

                AssistantState temp = carrierConfig.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!carrierConfig.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = carrierConfig.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithEjects
        /// </summary>
        /// <param name="ejectionBankSlotConfig">ejectionBankSlotConfig</param>
        private void RefreshILstAssistantStepWithEjects(EjectionBankSlotConfig ejectionBankSlotConfig)
        {
            this.ILstAssistantStep.Items.Clear();

            this.ILstAssistantStep.Items.AddRange(new object[]
                                                      {
                                                          "顶针高度示教",
                                                          "顶针零位示教",
                                                          "顶针XY位置示教"
                                                      });

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = ejectionBankSlotConfig.EjectionConfig.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!ejectionBankSlotConfig.EjectionConfig.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = ejectionBankSlotConfig.EjectionConfig.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithMagazineBox
        /// </summary>
        /// <param name="magazineBoxConfig">magazineBoxConfig</param>
        private void RefreshILstAssistantStepWithMagazineBox(MagazineBoxConfig magazineBoxConfig)
        {
            this.ILstAssistantStep.Items.Clear();
            this.ILstAssistantStep.Items.AddRange(new object[]
                                                      {
                                                          "晶圆料盒示教",
                                                          "换晶圆示教",
                                                          "晶圆台环限位示教"
                                                      });

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = magazineBoxConfig.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!magazineBoxConfig.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = magazineBoxConfig.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithTransportUnit
        /// </summary>
        /// <param name="transportUnitConfig">transportUnitConfig</param>
        private void RefreshILstAssistantStepWithTransportUnit(TransportUnitConfig transportUnitConfig)
        {
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

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = transportUnitConfig.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }


            // 如果都示教完成了，退出
            if (!transportUnitConfig.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                TreeListNode TransportUnitGeneralNode = this.TreeListProgramming.FindNode(a => a.Tag.ToString() == "TransportUnitGeneral");
                TransportUnitGeneralNode.StateImageIndex = 3;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = transportUnitConfig.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithNozzle
        /// </summary>
        /// <param name="nozzleShelfSlot">nozzleShelfSlot</param>
        private void RefreshILstAssistantStepWithNozzle(NozzleShelfSlot nozzleShelfSlot)
        {
            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = nozzleShelfSlot.Nozzle.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!nozzleShelfSlot.Nozzle.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = nozzleShelfSlot.Nozzle.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithPostBond
        /// </summary>
        /// <param name="postBondInspection">postBondInspection</param>
        private void RefreshILstAssistantStepWithPostBond(PostBondInspection postBondInspection)
        {
            this.ILstAssistantStep.Items.Clear();

            this.ILstAssistantStep.Items.AddRange(new object[]
                                                        {
                                                                "检测示教",
                                                                "背崩检测"
                                                        });

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = postBondInspection.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!postBondInspection.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = postBondInspection.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithBondPosition
        /// </summary>
        /// <param name="singleBondPositionConfig">singleBondPositionConfig</param>
        private void RefreshILstAssistantStepWithBondPosition(SingleBondPositionConfig singleBondPositionConfig)
        {
            this.ILstAssistantStep.Items.Clear();

            this.ILstAssistantStep.Items.AddRange(new object[]
                                                      {
                                                          "焊点位置示教",
                                                          "移动到焊点位置",
                                                          "焊点视觉矫正示教",
                                                          "焊点测高示教",
                                                          "焊点ID识别示教"
                                                      });

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = singleBondPositionConfig.GetAssistantStates().Find(a => a.ChName == stepName);

                if (temp != null)
                {
                    if (temp.State == AssistantStateEnum.Able)
                    {
                        this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                    }
                    else if (temp.State == AssistantStateEnum.UnAble)
                    {
                        this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                    }
                    else
                    {
                        this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                    }

                    this.ILstAssistantStep.SelectedIndex++;
                }
                
            }

            // 如果都示教完成了，退出
            if (!singleBondPositionConfig.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = singleBondPositionConfig.GetAssistantStates().Find(a => a.ChName == stepName);

                if (temp != null)
                {
                    if (temp.State == AssistantStateEnum.UnAble)
                    {
                        break;
                    }
                    else
                    {
                        this.ILstAssistantStep.SelectedIndex++;
                    }
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithDispenser
        /// </summary>
        /// <param name="dispenser">dispenser</param>
        private void RefreshILstAssistantStepWithDispenser(Dispenser dispenser)
        {
            this.ILstAssistantStep.Items.Clear();

            this.ILstAssistantStep.Items.AddRange(new object[]
                                                      {
                                                          "点胶针示教"
                                                      });

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            if (dispenser == null)
            {
                return;
            }

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                
                AssistantState temp = dispenser.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!dispenser.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = dispenser.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }

        /// <summary>
        /// RefreshILstAssistantStepWithflipTool
        /// </summary>
        /// <param name="flipTool">flipTool</param>
        private void RefreshILstAssistantStepWithFlipTool(FlipTool flipTool)
        {
            this.ILstAssistantStep.Items.Clear();

            this.ILstAssistantStep.Items.AddRange(new object[]
                                                      {
                                                          "翻转工具示教"
                                                      });

            // todo start改变
            this.ILstAssistantStep.SelectedIndex = 0;

            for (int i = 0; i < this.ILstAssistantStep.Items.Count; i++)
            {
                string stepName = this.ILstAssistantStep.Items[i].ToString();
                AssistantState temp = flipTool.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.Able)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.status_16x16;
                }
                else if (temp.State == AssistantStateEnum.UnAble)
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.warning_16x16;
                }
                else
                {
                    this.ILstAssistantStep.Items[i].Image = Properties.Resources.handtool_16x16;
                }

                this.ILstAssistantStep.SelectedIndex++;
            }

            // 如果都示教完成了，退出
            if (!flipTool.GetAssistantStates().Exists(it => it.State == AssistantStateEnum.UnAble))
            {
                this.ILstAssistantStep.SelectedIndex = -1;
                return;
            }

            this.ILstAssistantStep.SelectedIndex = 0;

            // 增加示教完成程度判断
            foreach (var item in this.ILstAssistantStep.Items)
            {
                string stepName = item.ToString();
                AssistantState temp = flipTool.GetAssistantStates().Find(a => a.ChName == stepName);
                if (temp.State == AssistantStateEnum.UnAble)
                {
                    break;
                }
                else
                {
                    this.ILstAssistantStep.SelectedIndex++;
                }
            }
        }
    }
}
