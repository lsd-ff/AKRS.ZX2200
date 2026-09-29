using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using System.Drawing;

    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using System.Xml.Linq;

    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;

    using DevExpress.Utils.Extensions;
    using DevExpress.XtraBars.Navigation;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;

    /// <summary>
    /// 导航示教-记录首位置、测高数据
    /// </summary>
    public partial class FrmComponentCarrierGeometryWithWaffleTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// Bond预取料位置
        /// </summary>
        private AKRSPoint3D readyBondPickPosition = new AKRSPoint3D();

        /// <summary>
        /// 第一点
        /// </summary>
        private AKRSPoint3D firstPoint = new AKRSPoint3D();

        /// <summary>
        /// 第二点
        /// </summary>
        private AKRSPoint3D secondPoint = new AKRSPoint3D();

        /// <summary>
        /// 第三点
        /// </summary>
        private AKRSPoint3D thirdPoint = new AKRSPoint3D();

        /// <summary>
        /// 载具名称
        /// </summary>
        private CarrierWithWaffleConfig CarrierWithWaffleConfig;

        /// <summary>
        /// 需要示教的CarrierNameMagazineSlotIndex
        /// </summary>
        private int magazineSlotIndexOfNeedTeach;

        /// <summary>
        /// 当前料片
        /// </summary>
        private AdapterTablet CurrentTablet => this.GetCurrentTablet();

        /// <summary>
        /// 获取当前料片
        /// </summary>
        /// <returns>return</returns>
        private AdapterTablet GetCurrentTablet()
        {
            if (this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                return WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet;
            }
            else
            {
                return (AdapterTablet)WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[this.magazineSlotIndexOfNeedTeach];
            }
        }

        /// <summary>
        /// 当前适配器
        /// </summary>
        private AdapterConfig CurrentAdapter => this.CurrentTablet.AdapterSetting;

        /// <summary>
        /// waffleCount
        /// </summary>
        private int WaffleCount => this.GetWaffleIndex().Count;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int StepCount => this.WaffleCount * 3 + 1;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// AssistantConfigList
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        #region 模组与模组参数

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private WaferTableModule WaferTableModule => WaferSubModule.GetInstance().WaferTable;

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private MagazineBoxModule MagazineBoxModule => WaferSubModule.GetInstance().MagazineBox;

        /// <summary>
        /// EjectModule
        /// </summary>
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// FlipChipModule
        /// </summary>
        private FlipModule FlipChipModule => WaferSubModule.GetInstance().FlipModule;

        /// <summary>
        /// WaferTableDevicePara
        /// </summary>
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// EjectDevicePara
        /// </summary>
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// FlipChipDevicePara
        /// </summary>
        private FlipTableDevicePara FlipChipDevicePara => WaferSubDevicePara.GetInstance().FlipChipDevicePara;

        #endregion

        /// <summary>
        /// 固晶模组
        /// </summary>
        private BondModule BondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 当前焊头上的吸嘴
        /// </summary>
        private Nozzle CurrentNozzle => BondModule.BondHead.CurrentNozzle;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="carrierWithWaffleConfig">waffleCarrier</param>
        public FrmComponentCarrierGeometryWithWaffleTeach(CarrierWithWaffleConfig carrierWithWaffleConfig)
        {
            this.InitializeComponent();
            this.CarrierWithWaffleConfig = carrierWithWaffleConfig;
            Block.GetInstance().SetCurrentCarrier(carrierWithWaffleConfig);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            List<int> list = this.GetWaffleIndex();
            List<int> listTemp = JsonFormatHelper<List<int>>.DeepGenericCopy<List<int>>(list);
            this.assistantConfigList = new List<AssistantConfig>();
            AssistantConfig temp;
            int index;
            string descritpion = string.Empty;
            bool isShowTitle = false;
            bool isShowBack = false;
            bool isShowNext = false;
            bool isShowDone = false;
            Action backAction;
            Action nextAction;
            Action doneAction;
            for (int i = 0; i < this.WaffleCount * 3; i++)
            {
                index = i;
                if (i % 3 == 0)
                {
                    //descritpion = $"Step {this.stepIndex++ + 1}/{this.StepCount}; Waffle pack {list[0] + 1}: Move to the upper left corner of the first cavity.";
                    descritpion = $"步 {this.stepIndex++ + 1}/{this.StepCount}; 华夫盒 {list[0] + 1}: 移动到第一个槽位的左上角.";
                }
                else if (i % 3 == 1)
                {
                    //descritpion = $"Step {this.stepIndex++ + 1}/{this.StepCount}; Waffle pack {list[0] + 1}: Move to the upper right corner of the first cavity.";
                    descritpion = $"步 {this.stepIndex++ + 1}/{this.StepCount}; 华夫盒 {list[0] + 1}: 移动到第一个槽位的右上角.";
                }
                else if (i % 3 == 2)
                {
                    //descritpion = $"Step {this.stepIndex++ + 1}/{this.StepCount}; Waffle pack {list[0] + 1}: Move to the bottom right corner of the first cavity.";
                    descritpion = $"步 {this.stepIndex++ + 1}/{this.StepCount}; 华夫盒 {list[0] + 1}: 移动到第一个槽位的右下角.";
                }

                if (i == 0)
                {
                    isShowTitle = true;
                    isShowBack = false;
                    isShowNext = true;
                    isShowDone = false;
                }
                else
                {
                    isShowTitle = true;
                    isShowBack = true;
                    isShowNext = true;

                    isShowDone = false;
                }

                if (this.stepIndex % 3 == 0)
                {
                    backAction = () =>
                        {
                            this.stepIndex = this.stepIndex - 2;
                        };
                    nextAction = () =>
                        {
                            for (int j = 0; j < listTemp.Count; j++)
                            {
                                if ((this.stepIndex + 1) / 3 - 1 == j)
                                {
                                    if (MachineStateModel.GetInstance().IsOffLineWork)
                                    {
                                        return;
                                    }

                                    // Calculation and Save FirstPosition
                                    this.thirdPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                                    double x = (this.firstPoint.X + this.secondPoint.X) / 2;
                                    double y = (this.secondPoint.Y + this.thirdPoint.Y) / 2;
                                    this.CurrentAdapter.WaffleArray[listTemp[j]].FirstPosition = new AKRSPoint3D(x, y, 0);
                                    AdapterConfigRepository.GetInstance().Save();

                                    // 控制Bond头去槽位中心
                                    if ((this.stepIndex + 1) / 3 == listTemp.Count)
                                    {
                                        // 将吸嘴旋转到示教的角度
                                        this.bondHeadController.RotateAxisT(this.bondHeadController.GetCurrentNozzle().AlignAngle);

                                        if (this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                                        {
                                            AKRSPoint3D measureHeightPos =
                                            this.CurrentAdapter.WaffleArray[listTemp[j]].FirstPosition
                                            + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                            // 去测高位值
                                            this.bondModuleController.MoveToG0Pos(measureHeightPos.X, measureHeightPos.Y);
                                        }
                                        else
                                        {
                                            this.bondModuleController.MoveToPickupPos();
                                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(this.CurrentAdapter.WaffleArray[listTemp[j]].FirstPosition);
                                        }
                                    }

                                    return;
                                }
                            }
                        };
                    doneAction = () =>
                        {
                        };

                    temp = new AssistantConfig(index, descritpion, isShowTitle, isShowBack, isShowNext, isShowDone, backAction, nextAction, doneAction);
                    this.assistantConfigList.Add(temp);
                    list.RemoveAt(0);
                }
                else if (this.stepIndex % 3 == 1)
                {
                    backAction = () =>
                        {
                        };
                    nextAction = () =>
                        {
                            if (MachineStateModel.GetInstance().IsOffLineWork)
                            {
                                return;
                            }

                            this.firstPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        };
                    doneAction = () =>
                        {
                        };

                    temp = new AssistantConfig(index, descritpion, isShowTitle, isShowBack, isShowNext, isShowDone, backAction, nextAction, doneAction);
                    this.assistantConfigList.Add(temp);
                }
                else if (this.stepIndex % 3 == 2)
                {
                    backAction = () =>
                        {
                            this.stepIndex = this.stepIndex - 1;
                        };
                    nextAction = () =>
                        {
                            if (MachineStateModel.GetInstance().IsOffLineWork)
                            {
                                return;
                            }

                            this.secondPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        };
                    doneAction = () =>
                        {
                        };

                    temp = new AssistantConfig(index, descritpion, isShowTitle, isShowBack, isShowNext, isShowDone, backAction, nextAction, doneAction);
                    this.assistantConfigList.Add(temp);
                }
            }

            index = this.StepCount - 1;
            //descritpion = $"Step {this.stepIndex++ + 1}/{this.StepCount}; Position the tool approx. 2 mm above the component.\r\n" + "DONE. carry out touchdown.";
            descritpion = this.CarrierWithWaffleConfig.IsUseFlipTable ? $"步 {this.stepIndex++ + 1}/{this.StepCount}; 示教结束（华夫盒芯片不需要顶针测高），点“确定”完成示教.\r\n": $"步 {this.stepIndex++ + 1}/{this.StepCount}; 请将吸嘴移动到靠近芯片5mm以内的地方.\r\n" + "点击完成将执行测高.";
            isShowTitle = true;
            isShowBack = true;
            isShowNext = false;
            isShowDone = true;
            backAction = () =>
            {
                this.stepIndex = this.StepCount - 2;
            };
            nextAction = () =>
            {
            };
            doneAction = () =>
            {
                if (!MachineStateModel.GetInstance().IsOffLineWork)
                {
                    if (this.CarrierWithWaffleConfig.IsUseFlipTable == false)
                    {
                        (ExcuteResult Ret, double HeightValue) result = (ExcuteResult.Success, BondModule.BondHead.AxisZ.GetRealPosition());

                        // 测高抬起高度设置为Z轴0点
                        double liftLevel = 0;

                    ReMeasureHeight:

                        result = this.bondHeadController.MeasureHeight(
                            liftLevel,
                            HeightMeasurementFunctionEnum.WithTDSensor);

                        if (result.Ret != ExcuteResult.Success)
                        {
                            this.bondHeadController.CloseBondHeadVaccum();
                            //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "Measure  height  failed!\r\n ReMeasureHeight with  OK\r\n End assistant  with  Cancel", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "测高失败!\r\n 重新测高请点击  OK\r\n 结束示教请点击  Cancel", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            switch (this.res)
                            {
                                case DialogResult.OK:
                                    goto ReMeasureHeight;
                                case DialogResult.Cancel:
                                    return;
                            }
                        }

                        this.readyBondPickPosition = MachineCoordinateSystem.GetInstance().BondCoordinateSystem.ConvertMachineZToG0Pos(result.HeightValue);

                        this.CarrierWithWaffleConfig.ReadyBondPickPosition = this.readyBondPickPosition;
                    }
                    else
                    {
                        this.bondModuleController.MoveToSafePos();
                    }
                }

                if (this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                {
                    //WaferSubDevicePara.GetInstance().StaticWaffleDevicePara.SafeHeightInG0 =
                    //    this.readyBondPickPosition.Z - CurrentNozzle.MeasureHeightOffset + 2;

                    double safeLevel = this.bondModuleController.ConvertZAxisG0ToMachinePos(this.readyBondPickPosition.Z - CurrentNozzle.MeasureHeightOffset + 6);

                    // 检查安全位置是否超过Z轴软极限
                    bool ret = this.bondHeadController.CheckZAxisSoftLimitAssitance(
                        safeLevel);

                    if (ret == false)
                    {
                        AKRSXtraMessageBox.Show(
                            $"静态华夫盒安全高度:{safeLevel} 超过Z轴软限位,请检查限位后重新示教！",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                      
                        this.DialogResult = DialogResult.Cancel;
                        return;
                    }

                    // 记录静态华夫盒安全高度:在测高结果基础上加6mm
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos.Z = this.readyBondPickPosition.Z - CurrentNozzle.MeasureHeightOffset + 6;

                    WaferSubDevicePara.GetInstance().WaferTableDevicePara
                        .SetNeedReCreateStateWithStaticAdapterTabletSignal();
                }
                else
                {
                    WaferSubDevicePara.GetInstance().MagazineDevicePara.SetNeedReCreateStateSignal();
                }

                this.CarrierWithWaffleConfig.ComponentTransportUnitGeometry.State = AssistantStateEnum.Able;
                WaferSubDevicePara.GetInstance().Save();
               CarrierConfigRepository.GetInstance().Save();

                this.DialogResult = DialogResult.OK;
            };

            temp = new AssistantConfig(index, descritpion, isShowTitle, isShowBack, isShowNext, isShowDone, backAction, nextAction, doneAction);
            this.assistantConfigList.Add(temp);


            if (this.CarrierWithWaffleConfig.CarrierType == Models.Enums.CarrierTypeEnum.StaticWaffle)
            {
                ucGuideMove = new UcGuideMove("固晶模组", "FrmComponentCarrierGeometryWithWaffleTeach", true, CameraEnum.BondCamera);
                this.PnlControl.Controls.Add(ucGuideMove);
                ucGuideMove.Dock = DockStyle.Fill;
            }
            else
            {
                ucGuideMove = new UcGuideMove("晶圆台模组", "FrmComponentCarrierGeometryWithWaffleTeach", true, CameraEnum.WaferCamera);
                this.PnlControl.Controls.Add(ucGuideMove);
                ucGuideMove.Dock = DockStyle.Fill;
            }

            this.TbiOrigin2.Text = this.CarrierWithWaffleConfig.IsUseFlipTable ? "结束" : "测高";

            // 首个步骤的UI设置
            this.stepIndex = 0;
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        private void CancelOperation()
        {
            //this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "End assistant?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "是否结束示教?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (this.res)
            {
                case DialogResult.Yes:
                    this.Close();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        /// <summary>
        /// Back操作
        /// </summary>
        private void BackOperation()
        {
            this.stepIndex--;
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
        }

        /// <summary>
        /// Next操作
        /// </summary>
        private void NextOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
        }

        /// <summary>
        /// Done操作
        /// </summary>
        private void DoneOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
        }

        /// <summary>
        /// Back按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.BackOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Next按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.NextOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Done按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.DoneOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.CancelOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnMoveToCameraCenter按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToCameraCenter_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.bondModuleController.MoveToPickupPos();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// 获取WaffleIndex
        /// </summary>
        /// <returns>WaffleIndex</returns>
        private List<int> GetWaffleIndex()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < this.CurrentAdapter?.MaxUseWaffleCount; i++)
            {
                if (this.CurrentAdapter.WaffleArray[i].Name == this.CarrierWithWaffleConfig.Name)
                {
                    list.Add(i);
                }
            }

            return list;
        }

        /// <summary>
        /// FrmComponentCarrierGeometryWithWaffleTeach_Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentCarrierGeometryWithWaffleTeach_Load(object sender, EventArgs e)
        {
            this.BtnMoveToCameraCenter.Enabled = this.CarrierWithWaffleConfig.CarrierType != CarrierTypeEnum.StaticWaffle;

            if ((this.CurrentNozzle == null || this.CurrentNozzle.Name == string.Empty) && !MachineStateModel.GetInstance().IsOffLineWork)
            {
                //AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "No nozzle exists on the bondHead.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "Bond头上不存在吸嘴.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Abort;
                return;
            }

            if (this.CarrierWithWaffleConfig.CarrierType != CarrierTypeEnum.StaticWaffle)
            {
                // 判断料片记忆
                if (!WaferSystemDomain.GetInstance().CheckTablet())
                {
                    this.DialogResult = DialogResult.Abort;
                    return;
                }

                // 选择某一层料片
                FrmChooseWafer temp = new FrmChooseWafer(this.CarrierWithWaffleConfig.Name);
                temp.ShowDialog();
                temp.Dispose();
                if (temp.DialogResult == DialogResult.Cancel)
                {
                    this.DialogResult = DialogResult.Abort;
                    return;
                }

                this.magazineSlotIndexOfNeedTeach = temp.MagazineSlotIndexOfNeedTeach;

                // 接下来开始换料
                if (WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift)
                {
                    // 自动更换某一层料片
                    WaferSubController.GetInstance().WaferTableController.RemoveWaferFromSlot(magazineSlotIndexOfNeedTeach);
                }
                else
                {
                    WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(this.CarrierWithWaffleConfig.Name);
                    string adtName = WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig
                        .TabletArray[magazineSlotIndexOfNeedTeach].Name;

                    if (WaferTableDevicePara.CurrentTablet?.Name != adtName)
                    {
                        // 提示人工更换
                        FrmChangeTabletTeach frmChangeTabletTeach = new FrmChangeTabletTeach(adtName);
                        frmChangeTabletTeach.Dispose();
                        if (frmChangeTabletTeach.DialogResult != DialogResult.OK)
                        {
                            this.DialogResult = DialogResult.Abort;
                            return;
                        }
                    }
                }
            }

            this.InitControl();
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.tileBarGroup2.Items.ForEach(
                a =>
                    {
                        TileBarItem b = (TileBarItem)a;
                        b.ImageAlignment = TileItemContentAlignment.MiddleLeft;
                        b.ImageScaleMode = TileItemImageScaleMode.ZoomInside;
                        b.ImageToTextAlignment = TileControlImageToTextAlignment.Left;
                    });

            if (this.assistantConfigList == null || this.stepIndex + 1 > this.assistantConfigList.Count)
            {
                return;
            }

            if (this.stepIndex + 1 == this.assistantConfigList.Count)
            {
                this.TileBarTeach.SelectedItem = this.tileBarGroup2.Items[3];
            }
            else
            {
                if (this.stepIndex % 3 == 0)
                {
                    this.TileBarTeach.SelectedItem = this.tileBarGroup2.Items[0];
                }
                else if (this.stepIndex % 3 == 1)
                {
                    this.TileBarTeach.SelectedItem = this.tileBarGroup2.Items[1];
                }
                else if (this.stepIndex % 3 == 2)
                {
                    this.TileBarTeach.SelectedItem = this.tileBarGroup2.Items[2];
                }
            }

            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
        }

        private void FrmComponentCarrierGeometryWithWaffleTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove?.Dispose();

            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }
        }

        /// <summary>
        /// 跳过当前首位置
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnSkipCurrentPosition_Click(object sender, EventArgs e)
        {
            int temp = 3 - this.stepIndex % 3 + this.stepIndex;
            if (temp < StepCount)
            {
                this.stepIndex = temp;
            }
        }
    }
}