using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using System.Drawing;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

    /// <summary>
    /// 导航示教-记录视觉检测模板、行列数、行列间距、相机拍照位置
    /// </summary>
    public partial class FrmComponentGeometryWithWaffleTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 定位模板名称
        /// </summary>
        private string DieMatchName => this.CarrierWithWaffleConfig.DieMatchName;

        /// <summary>
        /// 定位模板名称
        /// </summary>
        private string DieMatchNameP2 => this.CarrierWithWaffleConfig.DieMatchNameP2;

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
        /// 行数
        /// </summary>
        private int rowCount;

        /// <summary>
        /// 列数
        /// </summary>
        private int columnCount;

        /// <summary>
        /// 芯片Mark点与芯片中心的偏差
        /// </summary>
        private AKRSPoint3D relativeDistanceMarkWithCenter = new AKRSPoint3D();

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
        private CarrierWithWaffleConfig CarrierWithWaffleConfig;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 10;

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


        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="carrierWithWaffleConfig">waffleCarrier</param>
        public FrmComponentGeometryWithWaffleTeach(CarrierWithWaffleConfig carrierWithWaffleConfig)
        {
            this.InitializeComponent();
            this.CarrierWithWaffleConfig = carrierWithWaffleConfig;
            Block.GetInstance().SetCurrentCarrier(carrierWithWaffleConfig);
            this.InitControl();
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
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to the upper left component corner.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到芯片的左上角.",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.firstPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),
                 
                    // 步骤2
                    new AssistantConfig(
                        index: 1,
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
                            this.secondPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤3
                    new AssistantConfig(
                        index: 2,
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
                            this.thirdPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                            this.dieCenterPoint = (this.firstPoint + this.thirdPoint) / 2;
                            if (this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                            {
                                System2Domain.GetInstance().BondModuleController.MoveToG0Pos(this.dieCenterPoint);
                            }
                            else
                            {
                                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(this.dieCenterPoint);
                            }

                            double x = Math.Abs(this.firstPoint.X - this.secondPoint.X);
                            double y = Math.Abs(this.thirdPoint.Y - this.secondPoint.Y);
                            this.dieSize = new AKRSPoint3D(x, y, 0);

                            // 增加芯片尺寸提示
                            AKRSXtraMessageBox.Show($"芯片尺寸- 长度:{x}mm,宽度:{y}mm!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤4
                    new AssistantConfig(
                        index: 3,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to search position and program search.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到芯片识别区域并制作pr1.",
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

                                // todo 需要判断几点定位进行分流--暂时只有一点
                                if (this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                                {
                                    BaseAlgResult result = System2Domain.GetInstance().System2Controller.BondCameraVision(null, this.DieMatchName);
                                    if (result != null)
                                    {
                                        System2Domain.GetInstance().BondModuleController.MoveToG0Pos(System2Domain.GetInstance().System2Controller
                                            .GetBondVisionResultPos((MatchResult)result));
                                        this.relativeDistanceMarkWithCenter = System2Domain.GetInstance().BondModuleController.GetG0RealPosition() - this.dieCenterPoint;

                                        if (!this.CarrierWithWaffleConfig.IsTwoPointSearch)
                                        {
                                            this.stepIndex = 4;
                                        }
                                    }
                                    else
                                    {
                                        //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        AKRSXtraMessageBox.Show("pr未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.stepIndex = 2;
                                    }
                                }
                                else
                                {
                                    (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(this.DieMatchName, false, false);
                                    if (result.isSucceed)
                                    {
                                        //Block.GetInstance().MoveToCameraCenterWithThread(result.matchResults);
                                        //this.relativeDistanceMarkWithCenter = this.dieCenterPoint - Block.GetInstance().GetDieMarkCenterG0Pos(result.matchResults[0]);

                                        if (this.CarrierWithWaffleConfig.IsTwoPointSearch)
                                        {
                                            this.leftUpPoint = Block.GetInstance().GetDieMarkCenterG0Pos(result.matchResults[0]);
                                            this.distanceDieP1RelativeCenter = this.dieCenterPoint - this.leftUpPoint;
                                        }
                                        else
                                        {
                                            this.relativeDistanceMarkWithCenter = this.dieCenterPoint - Block.GetInstance().GetDieMarkCenterG0Pos(result.matchResults[0]);
                                            this.stepIndex = 4;
                                        }
                                    }
                                    else
                                    {
                                        //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        AKRSXtraMessageBox.Show("pr1未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.stepIndex = 2;
                                    }
                                }
                            },
                        doneAction: () =>
                        {
                        }),

                    // 步骤5
                    new AssistantConfig(
                        index: 4,
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到芯片识别区域并制作pr2.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                if (!this.CarrierWithWaffleConfig.IsTwoPointSearch)
                                {
                                    this.BackOperation();
                                }
                            },
                        nextAction: () =>
                            {
                                // 防呆
                                this.BondModulePositionCheck();

                                (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(this.DieMatchNameP2, false, false);
                                if (result.isSucceed)
                                {
                                    this.rightDownPoint = Block.GetInstance().GetDieMarkCenterG0Pos(result.matchResults[0]);
                                    this.distanceDieP2RelativeCenter = this.dieCenterPoint - this.rightDownPoint;
                                    this.relativeDistanceMarkWithCenter = this.dieCenterPoint - (this.leftUpPoint + this.rightDownPoint) / 2;
                                }
                                else
                                {
                                    //AKRSXtraMessageBox.Show("The die is not exist, please check match template!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AKRSXtraMessageBox.Show("pr2未识别到芯片, 请检查pr模板!.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.stepIndex = 3;
                                }
                            },
                        doneAction: () =>
                            {
                            }),

                    // 步骤6
                    new AssistantConfig(
                        index: 5,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to the upper right corner of the cavity in the first row and first column on the waffle pack.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到华夫盒第一行第一列槽位的右上角.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {                            
                            this.iniPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;                            
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤7
                    new AssistantConfig(
                        index: 6,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to the upper right corner of the cavity in the first row and last column on the waffle pack.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到华夫盒第一行最后一列槽位的右上角.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {                            
                            this.tempPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                            FrmInputColumns temp = new FrmInputColumns(this.CarrierWithWaffleConfig);
                            temp.ShowDialog();
                            temp.Dispose();
                            if (temp.DialogResult == DialogResult.Cancel)
                            {
                                this.stepIndex = this.stepCount - 6;
                                return;
                            }
                            else if (temp.DialogResult == DialogResult.Abort)
                            {
                                this.DialogResult = DialogResult.Abort;
                                return;
                            }

                            this.columnCount = temp.ColumnCount;

                            if (this.columnCount == 1)
                            {
                                this.carrierColSpacing = new AKRSPoint3D();
                            }
                            else
                            {
                                // 计算间距-是否符合要求
                                this.carrierColSpacing = (this.iniPoint - this.tempPoint) / (this.columnCount - 1);
                                bool isLimit = Math.Abs(this.carrierColSpacing.X) >= Math.Abs(this.dieSize.X);
                                if (!isLimit && !MachineStateModel.GetInstance().IsOffLineWork)
                                {
                                    //this.res = AKRSXtraMessageBox.Show("System 2: Information 2.3280:\r\n" + "Programming component transportUnit geometry failed\r\n" + "Repeat the programming and follow the directions on the screen.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.res = AKRSXtraMessageBox.Show("System 2: Information 2.3280:\r\n" + "计算列间距失败\r\n" + "请重试之前的操作.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.stepIndex = this.stepCount - 6;
                                    return;
                                }
                            }

                            this.stepIndex = this.stepCount - 3;
                            this.NextOperation();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤8
                    new AssistantConfig(
                        index: 7,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Please input Columns",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 请输入列数",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.stepIndex = this.stepCount - 5;
                        },
                        nextAction: () =>
                        {
                            this.stepIndex = this.stepCount - 4;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤9
                    new AssistantConfig(
                        index: 8,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to the upper right corner of the cavity in the last row and last column.",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 移动到华夫盒最后一行最后一列槽位的右上角.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {                            
                            this.tarPoint = this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle ? System2Domain.GetInstance().BondModuleController.GetG0RealPosition() : WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                            FrmInputRows temp = new FrmInputRows(this.CarrierWithWaffleConfig);
                            temp.ShowDialog();
                            temp.Dispose();
                            if (temp.DialogResult == DialogResult.Cancel)
                            {
                                this.stepIndex = this.stepCount - 6;
                                return;
                            }
                            else if (temp.DialogResult == DialogResult.Abort)
                            {
                                this.DialogResult = DialogResult.Abort;
                                return;
                            }

                            this.rowCount = temp.RowCount;

                            if (this.rowCount == 1)
                            {
                                this.carrierRowSpacing = new AKRSPoint3D();
                            }
                            else
                            {
                                // 计算间距-是否符合要求
                                this.carrierRowSpacing = (this.tempPoint - this.tarPoint) / (this.rowCount - 1);
                                bool isLimit = Math.Abs(this.carrierRowSpacing.Y) >= Math.Abs(this.dieSize.Y);
                                if (!isLimit && !MachineStateModel.GetInstance().IsOffLineWork)
                                {
                                    //this.res = AKRSXtraMessageBox.Show("System 2: Information 2.3280:\r\n" + "Programming component transportUnit geometry failed\r\n" + "Repeat the programming and follow the directions on the screen.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.res = AKRSXtraMessageBox.Show("System 2: Information 2.3280:\r\n" + "计算行间距失败\r\n" + "请重试之前的操作.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.stepIndex = this.stepCount - 6;
                                    return;
                                }
                            }

                            this.stepIndex = this.stepCount - 1;
                            DoneOperation();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤10
                    new AssistantConfig(
                        index: 9,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Please input Rows",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 请输入行数",
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
                            this.CarrierWithWaffleConfig.CarrierRowSpacing = this.carrierRowSpacing;
                            this.CarrierWithWaffleConfig.CarrierColSpacing = this.carrierColSpacing;
                            this.CarrierWithWaffleConfig.RowCount = this.rowCount;
                            this.CarrierWithWaffleConfig.ColumnCount = this.columnCount;
                            if (this.CarrierWithWaffleConfig.IsTwoPointSearch)
                            {
                                this.CarrierWithWaffleConfig.DistanceDieP1RelativeCenter = this.distanceDieP1RelativeCenter;
                                this.CarrierWithWaffleConfig.DistanceDieP2RelativeCenter = this.distanceDieP2RelativeCenter;
                            }

                            this.CarrierWithWaffleConfig.RelativeDistanceMarkWithCenter = this.relativeDistanceMarkWithCenter;

                            if (this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
                            {
                                // 此处相机高度需要记录BondZ轴位置
                                this.CarrierWithWaffleConfig.WaferAxisZPosition = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

                                WaferSubDevicePara.GetInstance().WaferTableDevicePara
                                    .SetNeedReCreateStateWithStaticAdapterTabletSignal();
                            }
                            else
                            {
                                // 此处相机高度需要记录WaferCameraZ轴位置
                                this.CarrierWithWaffleConfig.WaferAxisZPosition = WaferSubController.GetInstance().WaferTableController.WaferCameraAxisZG0Pos;

                                WaferSubDevicePara.GetInstance().MagazineDevicePara.SetNeedReCreateStateSignal();
                            }

                            this.CarrierWithWaffleConfig.ComponentGeometry.State = AssistantStateEnum.Able;
                            CarrierConfigRepository.GetInstance().Save();

                            this.stepIndex = this.stepCount - 2;
                            this.DialogResult = DialogResult.OK;
                        }),
                };

            if (this.CarrierWithWaffleConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                ucGuideMove = new UcGuideMove("固晶模组", "FrmComponentGeometryWithWaffleTeach", true, CameraEnum.BondCamera);
                this.PnlControl.Controls.Add(ucGuideMove);
                ucGuideMove.Dock = DockStyle.Fill;
            }
            else
            {
                if (this.CarrierWithWaffleConfig.SearchCamera == SearchCameraEnum.BondCamera)
                {
                    ucGuideMove = new UcGuideMove("固晶模组", "FrmComponentGeometryWithWaferPitchTeach", true, CameraEnum.WaferCamera);
                    ucGuideMove.ChangeCamera(CameraEnum.BondCamera);
                    ucGuideMove.IsSetCameraParematerByModule = false;
                }
                else
                {
                    ucGuideMove = new UcGuideMove("晶圆台模组", "FrmComponentGeometryWithWaffleTeach", true, CameraEnum.WaferCamera);
                    this.PnlControl.Controls.Add(ucGuideMove);
                    ucGuideMove.Dock = DockStyle.Fill;
                }
            }

            // 首个步骤的UI设置
            this.stepIndex = 0;
        }

        /// <summary>
        ///  Bond模组位置检查
        /// </summary>
        private void BondModulePositionCheck()
        {
            if (this.CarrierWithWaffleConfig.SearchCamera == SearchCameraEnum.BondCamera&&this.CarrierWithWaffleConfig.CarrierType!=CarrierTypeEnum.StaticWaffle)
            {
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

                    if (dia == DialogResult.OK)
                    {
                        this.bondModuleController.MoveBondXY(waferCenterPos.X, waferCenterPos.Y);
                    }
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
            this.Close();
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
            WaferSubController.GetInstance().WaferTableController.EditPr(this.DieMatchName, this.CarrierWithWaffleConfig.CarrierType);         
        }

        /// <summary>
        /// BtnEditProgram_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram2_Click(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().WaferTableController.EditPr(this.DieMatchNameP2, this.CarrierWithWaffleConfig.CarrierType);
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
        /// FrmComponentGeometryWithWaffleTeach_Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentGeometryWithWaffleTeach_Load(object sender, EventArgs e)
        {
            if (this.CarrierWithWaffleConfig.IsTwoPointSearch)
            {
                this.BtnEditProgram2.Enabled = true;
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

                int magazineSlotIndexOfNeedTeach = temp.MagazineSlotIndexOfNeedTeach;

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

        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {

        }

        private void FrmComponentGeometryWithWaffleTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove.Dispose();

            if (this.Timer1 != null)
            {
                this.Timer1.Stop();
                this.Timer1.Tick -= this.Timer1_Tick;
                this.Timer1.Dispose();
                this.Timer1 = null;
            }
        }

        private void BtnWaffleVacuum_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.OpenWaffleVacuum();
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

        private void BtnStaticWaffleVacuum_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
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
    }
}