using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using AKRS.Galaxy2.AutoFocusing;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Setting.Component;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using System.Drawing;

    /// <summary>
    /// 导航示教--记录扩晶位置、视觉检测模板、芯片间距、相机拍照位置
    /// </summary>
    public partial class FrmComponentGeometryWithWaferTeach : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 定位模板名称
        /// </summary>
        private string DieMatchName => this.CarrierWithWaferConfig.DieMatchName;

        /// <summary>
        /// 定位模板名称
        /// </summary>
        private string DieMatchNameP2 => this.CarrierWithWaferConfig.DieMatchNameP2;

        /// <summary>
        /// 芯片P1点相对中心距离
        /// </summary>
        private AKRSPoint3D distanceDieP1RelativeCenter = new AKRSPoint3D();

        /// <summary>
        /// 芯片P2点相对中心距离
        /// </summary>
        private AKRSPoint3D distanceDieP2RelativeCenter = new AKRSPoint3D();

        private AKRSPoint3D leftUpPoint = new AKRSPoint3D();

        private AKRSPoint3D rightDownPoint = new AKRSPoint3D();

        /// <summary>
        /// 行间距xy
        /// </summary>
        private AKRSPoint3D carrierRowSpacing;

        /// <summary>
        /// 列间距xy
        /// </summary>
        private AKRSPoint3D carrierColSpacing;

        /// <summary>
        /// 扩晶高位
        /// </summary>
        private AKRSPoint3D expandUpPosition = new AKRSPoint3D();

        /// <summary>
        /// 芯片Mark点与芯片中心的偏差
        /// </summary>
        private AKRSPoint3D relativeDistanceMarkWithCenter = new AKRSPoint3D();        

        /// <summary>
        /// 计算芯片间距时所隔芯片数
        /// </summary>
        private int numberOfSteps;

        /// <summary>
        /// 示教芯片中心点
        /// </summary>
        private AKRSPoint3D dieCenterPoint = new AKRSPoint3D();

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
        /// 起点
        /// </summary>
        private AKRSPoint3D iniPoint = new AKRSPoint3D();

        /// <summary>
        /// tempPoint
        /// </summary>
        private AKRSPoint3D tempPoint = new AKRSPoint3D();

        /// <summary>
        /// 终点
        /// </summary>
        private AKRSPoint3D tarPoint = new AKRSPoint3D();

        /// <summary>
        /// 芯片尺寸
        /// </summary>
        private AKRSPoint3D dieSize = new AKRSPoint3D();

        /// <summary>
        /// 载具名称
        /// </summary>
        private CarrierWithWaferConfig CarrierWithWaferConfig;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 11;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// AssistantConfigList
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 列间距
        /// </summary>
        private double pitchCol;

        /// <summary>
        /// 行间距
        /// </summary>
        private double pitchRow;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waferCarrierConfig">wafer TransportUnit</param>
        public FrmComponentGeometryWithWaferTeach(CarrierWithWaferConfig carrierWithWaferConfig)
        {
            this.InitializeComponent();
            this.CarrierWithWaferConfig = carrierWithWaferConfig;
            TsIsMatchWithVacuum.IsOn = this.CarrierWithWaferConfig.IsMatchWithVacuum;
            Block.GetInstance().SetCurrentCarrier(carrierWithWaferConfig);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 步骤1
                    new AssistantConfig(
                        index: 0,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Raise the ejection system to the appropriate height.",
                        descritpion: WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseCarrierHeightMeasure ? $"步 {this.stepIndex++ + 1}/{stepCount}; 移动顶针系统（切换到顶针架模组，并使用按钮操作将顶针升起）和扩晶环（切换到晶圆模组）到合适的高度,即将进行芯片及蓝膜厚度测高" : $"Step {this.stepIndex++ + 1}/{stepCount}; 移动顶针系统（切换到顶针架模组）和扩晶环（切换到晶圆模组）到合适的高度",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUseCarrierHeightMeasure)
                                {
                                    if (this.system2Controller.ChangeTouchDownAssistance()/*this.system2Controller.ChangeNozzleAssistance(this.CarrierWithWaferConfig.NozzleName)*/)
                                    {
                                        DialogResult result = ControlService.ShowDialogForm<FrmCarrierHeightMeasureTeach>(this.CarrierWithWaferConfig);
                                        System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                                        if (result != DialogResult.OK)
                                        {
                                            this.stepIndex--;
                                        }
                                    }
                                    else
                                    {
                                        this.stepIndex--;
                                    }
                                }
                            },
                        doneAction: () =>
                        {
                        }),
                 
                    // 步骤2
                    new AssistantConfig(
                        index: 1,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to the upper left component corner.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到芯片的左上角.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.firstPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤3
                    new AssistantConfig(
                        index: 2,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to the upper right component corner.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到芯片的右上角.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.secondPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤4
                    new AssistantConfig(
                        index: 3,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to the lower right component corner.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到芯片的右下角.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.thirdPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                            this.dieCenterPoint = (this.firstPoint + this.thirdPoint) / 2;
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(this.dieCenterPoint, true);

                            double x = Math.Abs(this.firstPoint.X - this.secondPoint.X);
                            double y = Math.Abs(this.thirdPoint.Y - this.secondPoint.Y);
                            this.dieSize = new AKRSPoint3D(x, y, 0);

                            // 增加芯片尺寸提示
                            AKRSXtraMessageBox.Show($"芯片尺寸- 长度:{x}mm,宽度:{y}mm!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤5
                    new AssistantConfig(
                        index: 4,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to search position and program search.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 晶圆台移动到芯片识别区域并制作pr1.（请不要移动BondXY轴位置）",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                // 防呆
                                this.BondModulePositionCheck();

                                // 选取视觉检测模板
                            (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(this.DieMatchName, false, false);
                            if (result.isSucceed)
                            {                                
                                if (!this.CarrierWithWaferConfig.IsTwoPointSearch)
                                {
                                    // 一点定位
                                    Block.GetInstance().MoveToCameraCenterWithThread(result.matchResults);
                                    this.relativeDistanceMarkWithCenter = this.dieCenterPoint - WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                                    this.stepIndex = 6;
                                    this.NextOperation();
                                }
                                else
                                {
                                    // 两点定位
                                    this.leftUpPoint = Block.GetInstance().GetDieMarkCenterG0Pos(result.matchResults[0]);
                                    this.distanceDieP1RelativeCenter = this.dieCenterPoint - this.leftUpPoint;
                                }
                            }
                            else
                            {
                                //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                AKRSXtraMessageBox.Show("pr1未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.stepIndex = 3;
                            }

                            //// 选取视觉检测模板
                            //(bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(this.DieMatchName, false, false);
                            //if (result.isSucceed)
                            //{
                            //    Block.GetInstance().MoveToCameraCenterWithThread(result.matchResults);
                            //    this.relativeDistanceMarkWithCenter = this.dieCenterPoint - WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                            //    if (!this.CarrierWithWaferConfig.IsRotationSearch)
                            //    {
                            //        this.stepIndex = 6;
                            //        this.NextOperation();
                            //    }
                            //}
                            //else
                            //{
                            //    //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //    AKRSXtraMessageBox.Show("pr未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //    this.stepIndex = 3;
                            //}
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤6
                    new AssistantConfig(
                        index: 5,
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到芯片识别区域并制作pr2.（请不要移动BondXY轴位置）",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            if (this.CarrierWithWaferConfig.IsTwoPointSearch)
                            {
                                this.stepIndex--;
                                this.BackOperation();
                            }
                            else
                            {
                                this.stepIndex -= 2;
                                this.BackOperation();
                            }
                        },
                        nextAction: () =>
                        {
                            // 防呆
                            this.BondModulePositionCheck();

                            if (this.CarrierWithWaferConfig.IsTwoPointSearch)
                            {
                                (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(this.DieMatchNameP2, false, false);
                                if (result.isSucceed)
                                {
                                    this.rightDownPoint = Block.GetInstance().GetDieMarkCenterG0Pos(result.matchResults[0]);
                                    this.distanceDieP2RelativeCenter = this.dieCenterPoint - this.rightDownPoint;
                                    this.relativeDistanceMarkWithCenter = this.dieCenterPoint - (this.leftUpPoint + this.rightDownPoint) / 2;

                                    // 此处自动移动到第一个识别pr位置好点，后面要自动进行动作
                                    WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(this.leftUpPoint, true);

                                    this.stepIndex = 6;
                                    this.NextOperation();
                                }
                                else
                                {
                                    //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AKRSXtraMessageBox.Show("pr2未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.stepIndex = 4;
                                }
                            }
                            else
                            {
                                this.stepIndex = 6;
                                this.NextOperation();
                            }                                                     
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤7
                    new AssistantConfig(
                        index: 6,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Enter the number of steps for automatic index and linefeed determination.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 请输入芯片自动跳转的个数以计算芯片间距.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            FrmInputNumberOfSteps temp = new FrmInputNumberOfSteps(this.CarrierWithWaferConfig);
                            temp.ShowDialog();
                            temp.Dispose();
                            if (temp.DialogResult == DialogResult.Cancel)
                            {
                                this.BackOperation();
                                return;
                            }
                            else if (temp.DialogResult == DialogResult.Abort)
                            {
                                this.Close();
                                return;
                            }

                            this.numberOfSteps = temp.NumberOfSteps;

                            this.stepIndex = this.stepCount - 4;
                            this.NextOperation();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤8
                    new AssistantConfig(
                        index: 7,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Motor to target position.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到目标位置.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {          
                            // 即便是两点定位，进行行列间距计算时也可以使用一点定位的逻辑去计算

                            // 移动到位后，模板定位到中心去，记录晶圆台当前位置
                            bool result = Block.GetInstance().MoveToCameraCenter(this.DieMatchName, false, false);
                            if (!result)
                            {
                                //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                AKRSXtraMessageBox.Show("pr未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.stepIndex = this.stepCount - 9;
                                return;
                            }

                            this.iniPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                            this.tempPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                            // 此处改为实际测量的列间距
                            // this.tempPoint.X = this.iniPoint.X - this.dieSize.X * this.numberOfSteps;
                            this.tempPoint.X = this.iniPoint.X - pitchCol * this.numberOfSteps;
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(this.tempPoint, true);
                            
                            this.stepIndex = this.stepCount - 3;
                            this.NextOperation();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤9
                    new AssistantConfig(
                        index: 8,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Calculate column spacing",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 计算列间距",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // 移动到位后，模板定位到中心去，记录晶圆台当前位置
                            bool result = Block.GetInstance().MoveToCameraCenter(this.DieMatchName, false, false);
                            if (!result)
                            {
                                //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                AKRSXtraMessageBox.Show("pr未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.stepIndex = this.stepCount - 10;
                                return;
                            }

                            this.tempPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                            this.carrierColSpacing = (this.iniPoint - this.tempPoint) / this.numberOfSteps;

                            this.stepIndex = this.stepCount - 2;
                            this.NextOperation();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤10
                    new AssistantConfig(
                        index: 9,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Motor to target position.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到目标位置.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {                                
                                this.tarPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                                // 此处改为实际测量的行间距
                                // this.tarPoint.Y = this.tempPoint.Y + this.dieSize.Y * this.numberOfSteps;
                                this.tarPoint.Y = this.tempPoint.Y + pitchRow * this.numberOfSteps;
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(this.tarPoint, true);

                                this.stepIndex = this.stepCount - 1;
                                this.DoneOperation();
                            },
                        doneAction: () =>
                            {
                            }),

                    // 步骤11
                    new AssistantConfig(
                        index: 10,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Calculate row spacing.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 计算行间距.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                        {
                            if (!MachineStateModel.GetInstance().IsOffLineWork)
                            {                                
                                // 移动到位后，模板定位到中心去，记录晶圆台当前位置
                                bool result = Block.GetInstance().MoveToCameraCenter(this.DieMatchName, false, false);
                                if (!result)
                                {
                                    //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AKRSXtraMessageBox.Show("pr未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.stepIndex = this.stepCount - 11;
                                    return;
                                }

                                this.tarPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                                this.carrierRowSpacing = (this.tempPoint - this.tarPoint) / this.numberOfSteps;

                                this.expandUpPosition = WaferSubController.GetInstance().WaferTableController.ExpandAxisZG0Pos;

                                this.CarrierWithWaferConfig.ExpandUpPosition = this.expandUpPosition;

                                // 两点定位数据记录
                                if (this.CarrierWithWaferConfig.IsTwoPointSearch)
                                {
                                    this.CarrierWithWaferConfig.DistanceDieP1RelativeCenter = this.distanceDieP1RelativeCenter;
                                    this.CarrierWithWaferConfig.DistanceDieP2RelativeCenter = this.distanceDieP2RelativeCenter;
                                }

                                this.CarrierWithWaferConfig.RelativeDistanceMarkWithCenter = this.relativeDistanceMarkWithCenter;

                                this.CarrierWithWaferConfig.CarrierRowSpacing = this.carrierRowSpacing;
                                this.CarrierWithWaferConfig.CarrierColSpacing = this.carrierColSpacing;

                                if (this.CarrierWithWaferConfig.SearchCamera == SearchCameraEnum.WaferCamera)
                                {
                                    this.CarrierWithWaferConfig.WaferAxisZPosition = WaferSubController.GetInstance().WaferTableController.WaferCameraAxisZG0Pos;
                                }
                                else
                                {
                                    this.CarrierWithWaferConfig.BondCameraSearchPosition =
                                        this.bondModuleController.GetG0RealPosition();
                                }

                                this.CarrierWithWaferConfig.EjectionTableWorkPosition = WaferSubController.GetInstance().EjectController.EjectionTableAxisZG0Pos;
                                EjectionConfigRepository.GetInstance().Save();
                            }

                            this.CarrierWithWaferConfig.ComponentGeometry.State = AssistantStateEnum.Able;
                            CarrierConfigRepository.GetInstance().Save();

                            if (this.CarrierWithWaferConfig.SearchCamera == SearchCameraEnum.BondCamera)
                            {
                                this.bondModuleController.MoveToSafePos();
                            }

                            this.stepIndex = this.stepCount - 6;

                            this.DialogResult = DialogResult.OK;
                        }),
                };

            ucGuideMove = new UcGuideMove("晶圆台模组", "FrmComponentGeometryWithWaferTeach", true, CameraEnum.WaferCamera);

            if (this.CarrierWithWaferConfig.SearchCamera == SearchCameraEnum.BondCamera)
            {
                ucGuideMove.ChangeCamera(CameraEnum.BondCamera);
                ucGuideMove.IsSetCameraParematerByModule = false;
            }

            this.PnlControl.Controls.Add(ucGuideMove);
            ucGuideMove.Dock = DockStyle.Fill;

            // 首个步骤的UI设置
            this.stepIndex = 0;
        }

        /// <summary>
        ///  Bond模组位置检查
        /// </summary>
        private void BondModulePositionCheck()
        {
            if (this.CarrierWithWaferConfig.SearchCamera == SearchCameraEnum.WaferCamera)
            {
                return;
            }

            AKRSPoint2D waferCenterPos = new AKRSPoint2D()
                                             {
                                                 X = CalibrateRunPara.GetInstance()
                                                         .BondHeadRotateCenterInWC.X
                                                     - CalibrateRunPara.GetInstance()
                                                         .BondRotateCenterToCamOffset.X,
                                                 Y = CalibrateRunPara.GetInstance()
                                                         .BondHeadRotateCenterInWC.Y
                                                     - CalibrateRunPara.GetInstance()
                                                         .BondRotateCenterToCamOffset.Y,
                                             };

            if (this.bondModuleController.IsBondModuleAtAxisPos(waferCenterPos) == false)
            {
                //AKRSXtraMessageBox.Show("Bond module is not at the position for teaching, please move it to the position and try again!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult dia = AKRSXtraMessageBox.Show(
                    "检测到固晶模组XY轴被移动，是否重新移动到示教位置!.",
                    "Prompt",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dia == DialogResult.Yes)
                {
                    this.bondModuleController.MoveBondXY(waferCenterPos.X, waferCenterPos.Y);
                }
            }
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
        /// ESVacuumOnOrOff按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESVacuumOnOrOff_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();
                    btn.Appearance.BackColor = Color.Yellow;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// ESUpOrDown按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESUpOrDown_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().EjectController.ReturnEjection();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    if (WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Exists(item => item.Name == this.CarrierWithWaferConfig.EjectionName) && this.CarrierWithWaferConfig.EjectionName != string.Empty)
                    {
                        EjectionBankSlotConfig tarBankSlotConfig = (EjectionBankSlotConfig)WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Find(item => item.Name == this.CarrierWithWaferConfig.EjectionName);
                        Block.GetInstance().SetCurrentCarrier(this.CarrierWithWaferConfig);
                        WaferSubController.GetInstance().EjectController.ChangeEjection(tarBankSlotConfig.Index, true);

                        btn.Appearance.BackColor = Color.Yellow;
                    }
                    else
                    {
                        //throw new Exception($"This Ejection does not exist in the current EjectionBank!");
                        throw new Exception($"该芯片使用的顶针在当前顶针架中不存在!");
                    }
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnBlockCyc_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBlockCyc_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();
                    btn.Appearance.BackColor = Color.Yellow;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnAutofocusWaferCamera_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutofocusWaferCamera_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().WaferTableController.Autofocus();
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
        /// BtnEditProgram_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram_Click(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().WaferTableController.EditPr(this.DieMatchName);
        }

        /// <summary>
        /// 模板编辑2-两点定位的第二个模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditProgram2_Click(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().WaferTableController.EditPr(this.DieMatchNameP2);
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentGeometryWithWaferTeach_Load(object sender, EventArgs e)
        {
            this.InitControl();

            if (this.CarrierWithWaferConfig.IsTwoPointSearch)
            {
                this.BtnEditProgram2.Enabled = true;
            }

            // 判断料片记忆
            if (!WaferSystemDomain.GetInstance().CheckTablet())
            {
                this.DialogResult = DialogResult.Abort;
                return;
            }
           
            // 选择某一层料片
            FrmChooseWafer temp = new FrmChooseWafer(this.CarrierWithWaferConfig.Name);
            temp.ShowDialog();
            temp.Dispose();
            if (temp.DialogResult == DialogResult.Cancel)
            {
                this.DialogResult = DialogResult.Abort;
                return;
            }

            int magazineSlotIndexOfNeedTeach = temp.MagazineSlotIndexOfNeedTeach;

            // 接下来开始换料
            if (WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift)
            {
                // 自动更换某一层料片
                WaferSubController.GetInstance().WaferTableController.RemoveWaferFromSlot(magazineSlotIndexOfNeedTeach);
            }
            else
            {
                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(this.CarrierWithWaferConfig.Name);
                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is WaferTablet wt)
                {
                    if (wt.CarrierConfigWithWafer.Name != this.CarrierWithWaferConfig.Name
                        | wt.SlotState != SlotStatuEnum.Good)
                    {
                        // 提示人工更换
                        FrmChangeTabletTeach frmChangeTabletTeach = new FrmChangeTabletTeach();
                        frmChangeTabletTeach.Dispose();
                        if (frmChangeTabletTeach.DialogResult != DialogResult.OK)
                        {
                            this.DialogResult = DialogResult.Abort;
                            return;
                        }
                    }
                }
                else
                {
                    // 提示人工更换
                    FrmChangeTabletTeach frmChangeTabletTeach = new FrmChangeTabletTeach();
                    frmChangeTabletTeach.Dispose();
                    if (frmChangeTabletTeach.DialogResult != DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.Abort;
                        return;
                    }
                }
            }

            // 智能化
            if (this.CarrierWithWaferConfig.SearchCamera == SearchCameraEnum.BondCamera)
            {
                AKRSPoint3D waferCenterPos = new AKRSPoint3D()
                {
                    X = CalibrateRunPara.GetInstance()
                                                             .BondHeadRotateCenterInWC.X
                                                         - CalibrateRunPara.GetInstance()
                                                             .BondRotateCenterToCamOffset.X,
                    Y = CalibrateRunPara.GetInstance()
                                                             .BondHeadRotateCenterInWC.Y
                                                         - CalibrateRunPara.GetInstance()
                                                             .BondRotateCenterToCamOffset.Y,
                    Z=0
                };

                this.bondModuleController.MoveSafeBondXYZ(waferCenterPos);
            }

            // 示教芯片间距
            FrmComponentGeometryWithWaferPitchTeach frmComponentGeometryWithWaferPitchTeach = new FrmComponentGeometryWithWaferPitchTeach(this.CarrierWithWaferConfig);
            frmComponentGeometryWithWaferPitchTeach.ShowDialog();
            frmComponentGeometryWithWaferPitchTeach.Dispose();
            if (frmComponentGeometryWithWaferPitchTeach.DialogResult != DialogResult.OK)
            {
                this.DialogResult = DialogResult.Abort;
                return;
            }
            else
            {
                pitchCol = frmComponentGeometryWithWaferPitchTeach.pitchCol;
                pitchRow = frmComponentGeometryWithWaferPitchTeach.pitchRow;

                AKRSXtraMessageBox.Show($"芯片跳转间距- 列间距:{pitchCol}mm,行间距:{pitchRow}mm!.", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 获取芯片所用的顶针的配置
        /// </summary>
        /// <returns>return</returns>
        private EjectionConfig GetCurrentComponentEjectionConfig()
        {
            return (EjectionConfig)EjectionConfigRepository.GetInstance().Find(this.CarrierWithWaferConfig.EjectionName);
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().SetUIControl(this.TileBarTeach, this.tileBarGroup2, this.assistantConfigList, this.stepIndex, this.BtBack, this.BtNext, this.BtDone, this.LbDescription);
        }

        private void FrmComponentGeometryWithWaferTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            ucGuideMove.Dispose();

            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }
        }

        /// <summary>
        /// system2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        private void BtnMoveToPickupPos_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;
                if (this.system2Controller.ChangeNozzleAssistance(this.CarrierWithWaferConfig.NozzleName))
                {
                    System2Domain.GetInstance().BondModuleController.MoveToPickupPos();
                }
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

        private void BtnMeasurementComponentThickness_Click(object sender, EventArgs e)
        {
            return;
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                (ExcuteResult Ret, double HeightValue) result = this.bondHeadController.MeasureHeight(
                    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                    HeightMeasurementFunctionEnum.WithTDSensor);

                if (result.Ret == ExcuteResult.Success)
                {
                    double offset = this.GetCurrentComponentEjectionConfig().EjectionTableMeasureHeightPosition.Z - WaferSubController.GetInstance().EjectController.EjectionTableAxisZG0Pos.Z;
                    double height = Math.Abs(MachineCoordinateSystem.GetInstance().BondCoordinateSystem
                                                 .ConvertMachineZToG0Pos(result.HeightValue).Z
                                             - this.GetCurrentComponentEjectionConfig().ReadyBondPickPosition.Z + offset);

                    this.CarrierWithWaferConfig.CarrierThickness = 0;
                    this.CarrierWithWaferConfig.ComponentAndCarrierThickness = this.CarrierWithWaferConfig.ComponentThickness = height;
                    CarrierConfigRepository.GetInstance().Save();
                    AKRSXtraMessageBox.Show($"测量芯片 + 蓝膜厚度结果为{height}mm.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    UcComponentEdit.SetLcThicknessAction(height);

                    System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                }
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

        private void TsIsMatchWithVacuum_Toggled(object sender, EventArgs e)
        {
            if (this.CarrierWithWaferConfig.IsMatchWithVacuum != TsIsMatchWithVacuum.IsOn)
            {
                this.CarrierWithWaferConfig.IsMatchWithVacuum = TsIsMatchWithVacuum.IsOn;
                CarrierConfigRepository.GetInstance().Save();
            }            
        }        
    }
}