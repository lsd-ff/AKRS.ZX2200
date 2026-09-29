using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using DevExpress.DashboardCommon;
    using DevExpress.Utils.Extensions;

    /// <summary>
    /// 芯片上视示教窗体
    /// </summary>
    public partial class FrmComponentAccuracyMode : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="baseCarrierConfig">传入的芯片对象</param>
        /// <param name="isCaliPickOffset">是否用于取片偏移校正</param>
        public FrmComponentAccuracyMode(BaseCarrierConfig baseCarrierConfig, bool isCaliPickOffset = false)
        {
            this.component = baseCarrierConfig;
            this.isCaliPickOffset = isCaliPickOffset;
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 是否用于取片偏移校正
        /// </summary>
        private bool isCaliPickOffset = false;

        /// <summary>
        /// 示教两点定位时P1点的位置
        /// </summary>
        private AKRSPoint3D P1VisionPosInG0 { get; set; }

        /// <summary>
        /// 示教两点定位时P1点的角度
        /// </summary>
        private double P1VisionAngel { get; set; }

        /// <summary>
        /// 点击Start传进来的芯片对象
        /// </summary>
        private readonly BaseCarrierConfig component;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 0;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController BondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController BondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller System2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// UpLook控制器
        /// </summary>
        private UpLookController UpLookController => System2Domain.GetInstance().UpLookController;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 拉角度走的两个点位
        /// </summary>
        private AKRSPoint2D[] angleCorrectPoint2Ds = new AKRSPoint2D[2];

        /// <summary>
        /// 芯片四个角定位
        /// </summary>
        private AKRSPoint2D[] componentCornerPoint2Ds = new AKRSPoint2D[4];

        /// <summary>
        /// PR1名称
        /// </summary>
        private string PR1Name => this.cameraType == CameraTypeEnum.UpLookCamera ? this.component.UpLookAdjustConfig.P1PRName : this.component.DownLookAdjustConfig.P1PRName;

        /// <summary>
        /// PR2名称
        /// </summary>
        private string PR2Name => this.cameraType == CameraTypeEnum.UpLookCamera ? this.component.UpLookAdjustConfig.P2PRName : this.component.DownLookAdjustConfig.P2PRName;

        /// <summary>
        /// 晶圆检查是否通过
        /// </summary>
        private bool isWaferCheckSucceed = false;

        /// <summary>
        /// 取片线程
        /// </summary>
        private Task pickupTask;

        /// <summary>
        /// 芯片中心
        /// </summary>
        private AKRSPoint3D ComponentCenter { get; set; }

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmComponentAccuracyMode");

        /// <summary>
        ///  初始相机类型
        /// </summary>
        private CameraTypeEnum cameraType;

        /// <summary>
        /// 是否弹出
        /// </summary>
        /// <returns>结果</returns>
        public bool IsShowDialog()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return true;
            }

            // 吸嘴检查
            if (!this.System2Controller.ChangeNozzleAssistance(this.component.NozzleName))
            {
                return false;
            }

            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
            {
                if (this.component.DipMode == DipModeEnum.BeforeAccuracyMode)
                {
                    //bool hasComponent = this.bondHeadController.CheckComponentByIO();
                    //if (hasComponent)
                    {
                        DialogResult dialog = AKRSXtraMessageBox.Show(
                            $"吸嘴上的芯片是否需要先去蘸胶？",
                            "提示",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (dialog == DialogResult.Yes)
                        {
                            // 蘸胶
                            this.System2Controller.DipFluxOnFluxer(this.component);

                            Nozzle nozzle = this.BondHeadController.GetCurrentNozzle();

                            if (nozzle.ToolAlignment.State != AssistantStateEnum.Able)
                            {
                                // 去上视
                                this.BondModuleController.MoveToUpLookPos();
                            }
                            else
                            {
                                AKRSPoint3D pos = nozzle.RotateCenterPos;

                                // 移到吸嘴旋转中心
                                this.BondModuleController.MoveToG0Pos(pos);
                            }
                        }
                    }
                }
            }

            if (MachineHardwareConfiguration.GetInstance().IsIPTConfigrated == false&& MachineHardwareConfiguration.GetInstance().IsRightIPTConfigrated == false
                && this.cameraType == CameraTypeEnum.BondCamera)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"中转台未配置，纠偏相机不能选择Bond相机！",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            return true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            // 芯片取片自动校准必须用上视相机
            this.cameraType = this.isCaliPickOffset ? CameraTypeEnum.UpLookCamera : this.component.AdjustCamera;

            double prVisionAngle=0;

            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.PnlControl.Controls.Add(ucGuideMove1);

            if (this.cameraType == CameraTypeEnum.BondCamera)
            {
                this.ucGuideMove1.ChangeCamera(CameraEnum.BondCamera);
            }
            else
            {
                this.ucGuideMove1.ChangeCamera(CameraEnum.UplookCamera);
            }

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 拉角度第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"移动轴使得相机能看到芯片左上角。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiRotationMeasurement2;

                                // 记录第一点位置
                                angleCorrectPoint2Ds[0] = this.BondModuleController.Get2DRealPosition();
                            },
                       doneAction: () =>
                       {
                       }),
                 
                    // 拉角度第二点
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"移动轴使得相机能看到芯片右上角。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiRotationMeasurement1;
                        },
                        nextAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComponentCorner1;

                              // 记录第二点位置
                            angleCorrectPoint2Ds[1] = this.BondModuleController.Get2DRealPosition();

                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            // 两点距离过短报警（0.1mm）
                            if (Math.Abs(angleCorrectPoint2Ds[0].X - angleCorrectPoint2Ds[1].X) < 0.05)
                            {
                                DialogResult dialog = AKRSXtraMessageBox.Show(
                                    $"两点距离过短，至少 0.10 mm.\r\nOK：从头开始示教\r\nCancel:结束示教.",
                                    "Warn",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Warning);

                                if (dialog == DialogResult.OK)
                                {
                                    // 返回到第一步
                                    // 因为点next会自动加1，所以这里是-1
                                    this.stepIndex -= 1;
                                    this.TileBarTeach.SelectedItem = this.TbiRotationMeasurement2;
                                    return;
                                }
                                else
                                {
                                    // 清除数据
                                    this.angleCorrectPoint2Ds = new AKRSPoint2D[2];
                                    DialogResult = DialogResult.Cancel;
                                }
                            }

                            if (Math.Abs(angleCorrectPoint2Ds[1].X - angleCorrectPoint2Ds[0].X)
                                > Math.Abs(angleCorrectPoint2Ds[1].Y - angleCorrectPoint2Ds[0].Y))
                            {
                                // 计算芯片角度
                                double angle = Math.Atan(
                                                   (angleCorrectPoint2Ds[1].Y - angleCorrectPoint2Ds[0].Y)
                                                   / (angleCorrectPoint2Ds[1].X - angleCorrectPoint2Ds[0].X)) * (180 / Math.PI);

                                if (double.IsNaN(angle))
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"两点距离过短，至少 0.10 mm.\r\nOK：从头开始示教\r\nCancel:结束示教.",
                                        "Warn",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Warning);

                                    if (dialog == DialogResult.OK)
                                    {
                                        // 返回到第一步
                                        // 因为点next会自动加1，所以这里是-1
                                        this.stepIndex = -1;
                                        this.TileBarTeach.SelectedItem = this.TbiRotationMeasurement2;
                                        return;
                                    }
                                    else
                                    {
                                        // 清除数据
                                        this.angleCorrectPoint2Ds = new AKRSPoint2D[2];
                                        DialogResult = DialogResult.Cancel;
                                    }
                                }

                                if (this.cameraType  == CameraTypeEnum.UpLookCamera)
                                {
                                    // 芯片转正
                                    this.BondHeadController.RelativeRotateAxisT(-angle);
                                }
                                else
                                {
                                    // 保存角度
                                    this.component.IPTDownLookAngle = angle;

                                    // 旋转到这个角度去取片，然后放下去,确保做模板的时候是正的
                                    this.PickAndRotationAndDown(angle);
                                }
                            }
                            else
                            {
                                // 计算芯片角度
                                double angle = Math.Atan(
                                                   (angleCorrectPoint2Ds[1].X - angleCorrectPoint2Ds[0].X)
                                               / (angleCorrectPoint2Ds[1].Y - angleCorrectPoint2Ds[0].Y))
                                               * (180 / Math.PI);

                                if (double.IsNaN(angle))
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"两点距离过短，至少 0.10 mm.\r\nOK：从头开始示教\r\nCancel:结束示教.",
                                        "Warn",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Warning);

                                    if (dialog == DialogResult.OK)
                                    {
                                        // 返回到第一步
                                        // 因为点next会自动加1，所以这里是-1
                                        this.stepIndex = -1;
                                        this.TileBarTeach.SelectedItem = this.TbiRotationMeasurement2;
                                        return;
                                    }
                                    else
                                    {
                                        // 清除数据
                                        this.angleCorrectPoint2Ds = new AKRSPoint2D[2];
                                        DialogResult = DialogResult.Cancel;
                                    }
                                }

                                if (this.cameraType  == CameraTypeEnum.UpLookCamera)
                                {
                                    // 芯片转正
                                    this.BondHeadController.RelativeRotateAxisT(angle);
                                }
                                else
                                {
                                    // 保存角度
                                    this.component.IPTDownLookAngle = angle;

                                    // 旋转到这个角度去取片，然后放下去,确保做模板的时候是正的
                                    this.PickAndRotationAndDown(angle);
                                }
                            }

                             prVisionAngle =
                                        this.BondHeadController.GetAxisTRealPos()
                                        - this.BondHeadController.GetCurrentNozzle().AlignAngle;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 芯片左上角
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"再次移动轴使相机能看到芯片左上角。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiRotationMeasurement2;
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiComponentCorner2;

                                // 记录芯片左上角位置
                                componentCornerPoint2Ds[0] = this.BondModuleController.Get2DRealPosition();
                            },
                        doneAction: () =>
                            {
                            }),

                    // 芯片右上角
                    new AssistantConfig(
                        index: 3,
                        descritpion: $"移动轴使相机能看到芯片右上角。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiComponentCorner1;
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiComponentCorner3;

                                // 记录芯片右上角位置
                                componentCornerPoint2Ds[1] = this.BondModuleController.Get2DRealPosition();
                            },
                        doneAction: () =>
                            {
                            }),

                    // 芯片右下角
                    new AssistantConfig(
                        index: 4,
                        descritpion: $"移动轴使相机能看到芯片右下角。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiComponentCorner2;
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiComponentCorner4;

                                // 记录芯片右下角位置
                                componentCornerPoint2Ds[2] = this.BondModuleController.Get2DRealPosition();
                            },
                        doneAction: () =>
                            {
                            }),

                    // 芯片左下角
                    new AssistantConfig(
                        index: 5,
                        descritpion: $"移动轴使相机能看到芯片左下角。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiComponentCorner3;
                            },
                        nextAction: () =>
                            {
                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录芯片左下角位置
                                componentCornerPoint2Ds[3] = this.BondModuleController.Get2DRealPosition();

                                // 计算芯片中心
                                AKRSPoint2D intersectionPoint = GeometryService.GetIntersectionPoint(
                                    this.componentCornerPoint2Ds[3],
                                    this.componentCornerPoint2Ds[1],
                                    this.componentCornerPoint2Ds[0],
                                    this.componentCornerPoint2Ds[2]);

                                // todo:和工艺确认一下最大尺寸，目前是4*2mm
                                if (Math.Abs(intersectionPoint.X - this.BondModuleController.GetAxisXRealPos()) > 80.0
                                    || Math.Abs(intersectionPoint.Y - this.BondModuleController.GetAxisYRealPos())
                                    > 60.0)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"X轴将移动{Math.Abs(intersectionPoint.X - this.BondModuleController.GetAxisXRealPos())}mm,Y轴将移动{Math.Abs(intersectionPoint.Y - this.BondModuleController.GetAxisYRealPos())}mm!\r\n超过限制:80mm,请重新示教!",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);

                                    this.stepIndex--;
                                    return;
                                }

                                // 移动到芯片中心
                                System2Module.GetInstance().BondModule.MoveBondXY(
                                    intersectionPoint.X,
                                    intersectionPoint.Y);

                                ComponentCenter = System2Module.GetInstance().BondModule.GetG0RealPosition();

                                   // 保存示教的芯片中心
                                this.component.IPTComponentCenter =
                                    System2Domain.GetInstance().BondModuleController.GetG0RealPosition()
                                    + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                  this.component.ComponentSize =
                                      GeometryService.GetSizeFormFourPoint(componentCornerPoint2Ds);

                                this.TileBarTeach.SelectedItem = this.TbiAdjustPoint1;
                            },
                        doneAction: () =>
                            {
                            }),

                    // 芯片一点定位
                    new AssistantConfig(
                        index: 7,
                        descritpion: $"请搜索点移到相机中心并编辑芯片的P1视觉模板。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: this.component.IsTwoPointAdjust,
                        isShowDone: !this.component.IsTwoPointAdjust,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiComponentCorner4;
                            },
                        nextAction: () =>
                            {
                                #region 定位结果检查

                                // 检查PR
                                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(PR1Name);

                                if (pREntity == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先编辑视觉模板!",
                                        "Warn",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Warning);

                                    if (dialog1 == DialogResult.Cancel)
                                    {
                                        this.DialogResult = DialogResult.Cancel;
                                    }

                                    return;
                                }

                                this.BtnEditProgram.Enabled = true;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }   

                                // 执行定位
                                MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                                    null,
                                    "Component teach",
                                    PR1Name,
                                    false,
                                    this.cameraType);

                                if (result == null || !result.IsSuccess)
                                {
                                    AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                                    return;
                                }

                                #endregion
                                
                                // 上视相机
                                if (this.cameraType == CameraTypeEnum.UpLookCamera)
                                {
                                    #region 上视点位1信息保存

                                    // 获取G0坐标
                                    AKRSPoint3D posInG0 = this.UpLookController.ConvertPixelToG0Pos(
                                        this.BondModuleController.Get3DRealPosition(),
                                        result);

                                    // 计算真实拍照位并保存,这是相对值
                                    AKRSPoint3D point = new AKRSPoint3D()
                                                            {
                                                                X = posInG0.X,
                                                                Y = posInG0.Y,
                                                                Z = posInG0.Z - this.BondHeadController.GetCurrentNozzle()
                                                                        .MeasureHeightOffset
                                                            };

                                    this.component.UpLookAdjustConfig.P1VisionPos = point;

                                    this.component.UpLookAdjustConfig.PRVisionAngle =
                                        this.BondHeadController.GetAxisTRealPos()
                                        - this.BondHeadController.GetCurrentNozzle().AlignAngle;

                                    this.P1VisionPosInG0 = posInG0;
                                    this.P1VisionAngel = -result.Angle;

                                    #endregion
                                }
                                else
                                {
                                    #region 中转台信息保存

                                    // 保存拍照位置
                                    this.component.DownLookAdjustConfig.P1VisionPos =
                                        this.BondModuleController.ConvertPixelToG0Pos(
                                            this.BondModuleController.Get3DRealPosition(),
                                            result) + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                    this.P1VisionAngel = -result.Angle;

                                    #endregion
                                }
                            },
                        doneAction: () =>
                            {
                                #region 定位结果检查

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    this.DialogResult = DialogResult.OK;

                                    return;
                                }

                                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(PR1Name);

                                if (pREntity == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先编辑PR!",
                                        "Warn",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Warning);

                                    if (dialog1 == DialogResult.Cancel)
                                    {
                                        this.DialogResult = DialogResult.Cancel;
                                    }

                                    return;
                                }

                                // 执行定位
                                MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                                    null,
                                    "Component teach",
                                    PR1Name,
                                    false,
                                    this.cameraType );

                                if (result == null || !result.IsSuccess)
                                {
                                    AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                                    return;
                                }

                                #endregion
                                
                                // 上视相机
                                if (this.cameraType  == CameraTypeEnum.UpLookCamera)
                                {
                                    #region 上视结果保存

                                    // 获取G0坐标
                                    AKRSPoint3D posInG0 = this.UpLookController.ConvertPixelToG0Pos(
                                        this.BondModuleController.Get3DRealPosition(),
                                        result);

                                    // 计算真实拍照位并保存,这是相对值
                                    AKRSPoint3D point = new AKRSPoint3D()
                                                            {
                                                                X = posInG0.X,
                                                                Y = posInG0.Y,
                                                                Z = posInG0.Z - this.BondHeadController
                                                                        .GetCurrentNozzle().MeasureHeightOffset
                                                            };

                                    this.component.UpLookAdjustConfig.P1VisionPos = point;

                                    this.component.UpLookAdjustConfig.PRVisionAngle =
                                        this.BondHeadController.GetAxisTRealPos()
                                        - this.BondHeadController.GetCurrentNozzle().AlignAngle;

                                    // 计算视觉中心和芯片中心的差值
                                    this.component.UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter =
                                        posInG0 - ComponentCenter;

                                    // 角度的误差
                                    this.component.UpLookAdjustConfig.AngleForVisionCenterAndComponentCenter =
                                        -result.Angle;

                                    #endregion
                                }
                                else
                                {
                                    #region 中转台结果保存

                                    AKRSPoint3D point = this.BondModuleController.ConvertPixelToG0Pos(
                                        this.BondModuleController.Get3DRealPosition(),
                                        result);

                                    // Bond相机
                                    this.component.DownLookAdjustConfig.P1VisionPos =
                                        point + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                    this.component.DownLookAdjustConfig.PRVisionAngle = prVisionAngle;
                                        //this.BondHeadController.GetAxisTRealPos()
                                        //- this.BondHeadController.GetCurrentNozzle().AlignAngle;

                                    this.component.IPTDownLookVisionAngle = -result.Angle;

                                    #endregion
                                }

                                this.DoneAction();
                            }),

                    // 芯片两点定位
                    new AssistantConfig(
                        index: 8,
                        descritpion: $"请搜索点移到相机中心并编辑芯片的P2视觉模板.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiAdjustPoint1;
                            },
                        nextAction: () =>
                            {
                                  #region 定位检查

                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                this.Close();

                                return;
                            }

                            // 检查PR
                            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(PR2Name);

                            if (pREntity == null)
                            {
                                DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                    $"请先编辑视觉模板!",
                                    "Warn",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Warning);

                                if (dialog1 == DialogResult.Cancel)
                                {
                                    this.DialogResult = DialogResult.Cancel;
                                }

                                return;
                            }                         

                            // 执行定位
                            MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                                null,
                                "Component teach",
                                PR2Name,
                                false,
                                this.cameraType );

                            if (result == null)
                            {
                                AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                                return;
                            }

                            if (!result.IsSuccess)
                            {
                                AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                                return;
                            }

                            #endregion

                            // 上视相机
                            if (this.cameraType  == CameraTypeEnum.UpLookCamera)
                            {
                                #region 上视位置记录

                                // 获取G0坐标
                                AKRSPoint3D posInG0 = this.UpLookController.ConvertPixelToG0Pos(
                                    this.BondModuleController.Get3DRealPosition(),
                                    result);

                                // 计算真实拍照位并保存,这是相对值
                                AKRSPoint3D point = new AKRSPoint3D()
                                                        {
                                                            X = posInG0.X,
                                                            Y = posInG0.Y,
                                                            Z = posInG0.Z - this.BondHeadController.GetCurrentNozzle()
                                                                    .MeasureHeightOffset
                                                        };

                                this.component.UpLookAdjustConfig.P2VisionPos = point;

                                this.component.UpLookAdjustConfig.PRVisionAngle = this.BondHeadController.GetAxisTRealPos() - this.BondHeadController.GetCurrentNozzle().AlignAngle;

                                // 计算视觉中心和芯片中心的差值
                                this.component.UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter =
                                    (posInG0 + this.P1VisionPosInG0) / 2.0 - ComponentCenter;

                                // 计算角度差值
                                this.component.UpLookAdjustConfig.AngleForVisionCenterAndComponentCenter =
                                    (this.P1VisionAngel - result.Angle) / 2.0;

                                #endregion
                            }
                            else
                            {
                                #region 中转台位置记录

                                AKRSPoint3D point = this.BondModuleController.ConvertPixelToG0Pos(
                                    this.BondModuleController.Get3DRealPosition(),
                                    result);

                                // 计算真实拍照位并保存,这是相对值
                                this.component.DownLookAdjustConfig.P2VisionPos =
                                    point + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                        this.component.DownLookAdjustConfig.PRVisionAngle = prVisionAngle;
                                        //this.BondHeadController.GetAxisTRealPos()
                                        //- this.BondHeadController.GetCurrentNozzle().AlignAngle;

                                this.component.IPTDownLookVisionAngle = (this.P1VisionAngel - result.Angle) / 2.0;

                                #endregion
                            }
                            },
                        doneAction: () =>
                        {
                            #region 定位检查

                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                this.Close();

                                return;
                            }

                            // 检查PR
                            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(PR2Name);

                            if (pREntity == null)
                            {
                                DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                    $"请先编辑视觉模板!",
                                    "Warn",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Warning);

                                if (dialog1 == DialogResult.Cancel)
                                {
                                    this.DialogResult = DialogResult.Cancel;
                                }

                                return;
                            }                         

                            // 执行定位
                            MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                                null,
                                "Component teach",
                                PR2Name,
                                false,
                                this.cameraType );

                            if (result == null)
                            {
                                AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                                return;
                            }

                            if (!result.IsSuccess)
                            {
                                AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                                return;
                            }

                            #endregion

                            // 上视相机
                            if (this.cameraType  == CameraTypeEnum.UpLookCamera)
                            {
                                #region 上视位置记录

                                // 获取G0坐标
                                AKRSPoint3D posInG0 = this.UpLookController.ConvertPixelToG0Pos(
                                    this.BondModuleController.Get3DRealPosition(),
                                    result);

                                // 计算真实拍照位并保存,这是相对值
                                AKRSPoint3D point = new AKRSPoint3D()
                                                        {
                                                            X = posInG0.X,
                                                            Y = posInG0.Y,
                                                            Z = posInG0.Z - this.BondHeadController.GetCurrentNozzle()
                                                                    .MeasureHeightOffset
                                                        };

                                this.component.UpLookAdjustConfig.P2VisionPos = point;

                                this.component.UpLookAdjustConfig.PRVisionAngle = this.BondHeadController.GetAxisTRealPos() - this.BondHeadController.GetCurrentNozzle().AlignAngle;

                                // 计算视觉中心和芯片中心的差值
                                this.component.UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter =
                                    (posInG0 + this.P1VisionPosInG0) / 2.0 - ComponentCenter;

                                // 计算角度差值
                                this.component.UpLookAdjustConfig.AngleForVisionCenterAndComponentCenter =
                                    (this.P1VisionAngel - result.Angle) / 2.0;

                                #endregion
                            }
                            else
                            {
                                #region 中转台位置记录

                                AKRSPoint3D point = this.BondModuleController.ConvertPixelToG0Pos(
                                    this.BondModuleController.Get3DRealPosition(),
                                    result);

                                // 计算真实拍照位并保存,这是相对值
                                this.component.DownLookAdjustConfig.P2VisionPos =
                                    point + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                        this.component.DownLookAdjustConfig.PRVisionAngle = prVisionAngle;
                                        //this.BondHeadController.GetAxisTRealPos()
                                        //- this.BondHeadController.GetCurrentNozzle().AlignAngle;

                                this.component.IPTDownLookVisionAngle = (this.P1VisionAngel - result.Angle) / 2.0;

                                #endregion
                            }

                            this.DoneAction();
                        }),
                };

            // 加入参考点
            if (this.component.IsConfirmReferencePoint)
            {
                // 参考点中心
                this.assistantConfigList.Insert(6,new AssistantConfig(
                    index: 6,
                    descritpion: $"移动轴使相机中心对准芯片参考点中心。\r\n",
                    isShowTitle: true,
                    isShowBack: true,
                    isShowNext: true,
                    isShowDone: false,
                    backAction: () =>
                    {
                        this.TileBarTeach.SelectedItem = this.TbiComponentCorner4;
                    },
                    nextAction: () =>
                    {
                        this.component.IPTComponentReference =
                               this.component.IPTComponentCenter - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset - System2Domain.GetInstance().BondModuleController.GetG0RealPosition();
                    },
                    doneAction: () =>
                    {
                    }));
            }
            else
            {
                this.tileBarGroup4.Items.Remove(this.TbiComponentReference);
            }

            // 如果不是两点矫正
            if (!this.component.IsTwoPointAdjust)
            {
                this.assistantConfigList.RemoveAt(this.assistantConfigList.Count - 1);
                this.tileBarGroup4.Items.Remove(this.TbiAdjustPoint2);
            }

            // 如果不需要二维码识别，则移除二维码步骤
            if (this.component.IsRecognizeQRCode)
            {
                // 芯片识别二维码
                this.assistantConfigList.Add(new AssistantConfig(
                    index: 9,
                    descritpion: $"请搜索点移到相机中心并编辑芯片的二维码视觉模板.\r\n",
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
                        #region 定位检查

                        if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                        {
                            this.Close();

                            return;
                        }

                        // 检查PR
                        PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.component.UpLookAdjustConfig.CodeVisionPRName);

                        if (pREntity == null)
                        {
                            DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                $"请先编辑视觉模板!",
                                "Warn",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Warning);

                            if (dialog1 == DialogResult.Cancel)
                            {
                                this.DialogResult = DialogResult.Cancel;
                            }

                            return;
                        }

                        //// 执行定位
                        //BaseAlgResult result = System2Domain.GetInstance().System2CommonVision(
                        //    null,
                        //    "Component teach",
                        //    this.component.DownLookAdjustConfig.CodeVisionPRName,
                        //    false,
                        //    this.cameraType);

                        pREntity.DoWork();
                        BaseAlgResult result = pREntity.AlgResult;

                        if (result == null)
                        {
                            AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                            return;
                        }

                        if (!result.IsSuccess)
                        {
                            AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                            return;
                        }

                        #endregion

                        // 上视相机
                        if (this.cameraType == CameraTypeEnum.UpLookCamera)
                        {
                            #region 上视位置记录

                            // 获取G0坐标
                            AKRSPoint3D posInG0 = this.BondModuleController.GetG0RealPosition();

                            // 计算真实拍照位并保存,这是相对值
                            AKRSPoint3D point = new AKRSPoint3D()
                            {
                                X = posInG0.X,
                                Y = posInG0.Y,
                                Z = posInG0.Z - this.BondHeadController.GetCurrentNozzle()
                                                                .MeasureHeightOffset
                            };

                            this.component.UpLookAdjustConfig.CodeVisionPRPos = point;

                            #endregion
                        }
                        else
                        {
                            #region 中转台位置记录

                            // 获取G0坐标
                            AKRSPoint3D posInG0 = this.BondModuleController.GetG0RealPosition();

                            // 计算真实拍照位并保存,这是相对值
                            AKRSPoint3D point = new AKRSPoint3D()
                            {
                                X = posInG0.X,
                                Y = posInG0.Y,
                                Z = posInG0.Z - this.BondHeadController.GetCurrentNozzle()
                                                                .MeasureHeightOffset
                            };

                            this.component.DownLookAdjustConfig.CodeVisionPRPos = point;

                            #endregion
                        }

                        this.DoneAction();
                    }));
            }
            else
            {
                this.tileBarGroup4.Items.Remove(this.TbiCode);
            }

            for (int i = 0; i < this.assistantConfigList.Count; i++)
            {
                this.assistantConfigList[i].Descritpion = $"{i + 1}/{this.assistantConfigList.Count}" + this.assistantConfigList[i].Descritpion;
                this.assistantConfigList[i].Index = i;
                this.assistantConfigList[i].IsShowTitle = true;
                this.assistantConfigList[i].IsShowBack = i > 0;
                this.assistantConfigList[i].IsShowNext = i < this.assistantConfigList.Count - 1;
                this.assistantConfigList[i].IsShowDone = i == this.assistantConfigList.Count - 1;
            }

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiRotationMeasurement1;

            this.BtnPickFromIPT.Enabled = this.cameraType == CameraTypeEnum.BondCamera;
            this.BtnPlaceComponentOnIPT.Enabled = this.cameraType == CameraTypeEnum.BondCamera;
            this.BtnPickFromWaferTable.Text = this.component.IsUseFlipTable ? "翻转后取片" : "从晶圆台取片";

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("上视模组", true);
        }

        /// <summary>
        /// 完成动作
        /// </summary>
        private void DoneAction()
        {
            if (this.stepIndex != this.assistantConfigList.Count - 1)
            {
                return;
            }

            DialogResult dialog = AKRSXtraMessageBox.Show(
                               $"示教完成，是否抛料?",
                               "Question",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                if (this.cameraType == CameraTypeEnum.BondCamera)
                {
                    this.PickFromIPT();
                }

                // 抛料
                this.BondModuleController.ThrowAction();
                this.BondModuleController.MoveToSafePos();
            }

            // 示教情况下才修改状态
            if (this.isCaliPickOffset == false)
            {
                this.component.ComponentAccuracyMode.State = AssistantStateEnum.Able;
            }

            CarrierConfigRepository.GetInstance().Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUIControl(int stepIndex)
        {

            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[this.stepIndex];
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;

            this.BtnEditProgram.Visible = this.TileBarTeach.SelectedItem == TbiAdjustPoint1
                || this.TileBarTeach.SelectedItem == TbiAdjustPoint2
                || this.TileBarTeach.SelectedItem == TbiCode;
        }

        /// <summary>
        /// 返回上一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUIControl(this.stepIndex);
        }

        /// <summary>
        /// Next
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUIControl(this.stepIndex);
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否结束示教?",
                "问题",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                // 清除数据
                this.angleCorrectPoint2Ds = new AKRSPoint2D[2];
                this.componentCornerPoint2Ds = new AKRSPoint2D[4];

                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            CarrierConfigRepository.GetInstance().Save();
        }

        /// <summary>
        /// 自动对焦按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            this.BondHeadController.AutoFocusAssistance(CameraTypeEnum.UpLookCamera, sender, this);
        }

        /// <summary>
        /// 测试按钮，后续删除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void button1_Click(object sender, EventArgs e)
        {
            this.componentCornerPoint2Ds[0] = new AKRSPoint2D(2, 2);
            this.componentCornerPoint2Ds[1] = new AKRSPoint2D(4, 2);
            this.componentCornerPoint2Ds[2] = new AKRSPoint2D(2, 0);
            this.componentCornerPoint2Ds[3] = new AKRSPoint2D(0, 0);

            // 计算芯片中心
            AKRSPoint2D intersectionPoint = GeometryService.GetIntersectionPoint(this.componentCornerPoint2Ds[3], this.componentCornerPoint2Ds[1], this.componentCornerPoint2Ds[0], this.componentCornerPoint2Ds[2]);
        }

        /// <summary>
        /// 制作PR
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (this.cameraType == CameraTypeEnum.UpLookCamera)
            {
                this.component.UpLookAdjustConfig.P1PRName = this.component.Name + "UpLookMatch1";
                this.component.UpLookAdjustConfig.P2PRName = this.component.Name + "UpLookMatch2";
                this.component.UpLookAdjustConfig.CodeVisionPRName = this.component.Name + "二维码";
            }
            else
            {
                this.component.DownLookAdjustConfig.P1PRName = this.component.Name + "DownLookMatch1";
                this.component.DownLookAdjustConfig.P2PRName = this.component.Name + "DownLookMatch2";
                this.component.UpLookAdjustConfig.CodeVisionPRName = this.component.Name + "二维码";
            }

            if (this.TileBarTeach.SelectedItem == TbiAdjustPoint1)
            {
                TUAssistantHelper.EditPrInSystem2(PR1Name, this.cameraType);
            }
            else if (this.TileBarTeach.SelectedItem == TbiAdjustPoint2)
            {
                TUAssistantHelper.EditPrInSystem2(PR2Name, this.cameraType);
            }
            else if (this.TileBarTeach.SelectedItem == TbiCode)
            {
                TUAssistantHelper.EditPrInSystem2(this.component.UpLookAdjustConfig.CodeVisionPRName, this.cameraType);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentAccuracyMode_FormClosed(object sender, FormClosedEventArgs e)
        {
            Machine.GetInstance().Stop();
        }

        /// <summary>
        /// 抛料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnThow_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.BondModuleController.ThrowAction();
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
        /// 移动到视觉位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToVision_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                if (this.cameraType == CameraTypeEnum.UpLookCamera)
                {
                    // 去上视
                    this.BondModuleController.MoveToUpLookPos();
                }
                else
                {
                    // 移动到视觉位
                    AKRSPoint3D targetPos = component.IPTType == IPTTypeEnum.LeftIPT ? BondDevicePara.GetInstance().IPTDevicePara.IPTVisionPos : BondDevicePara.GetInstance().IPTDevicePara.RightIPTVisionPos;
                    this.BondModuleController.MoveToG0Pos(targetPos);
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

        /// <summary>
        /// 从中转台取片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnPickFromIPT_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;

            btn.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            await Task.Run(this.PickFromIPT);

            btn.Appearance.BackColor = default;
            btn.Enabled = true;
        }

        /// <summary>
        /// 从晶圆取片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPickFromWaferTable_Click(object sender, EventArgs e)
        {
            // 防止线程多次启动
            if (this.pickupTask == null || this.pickupTask.IsCompleted == true)
            {
                if (!isWaferCheckSucceed)
                {
                    // 检查
                    if (!WaferSystemDomain.GetInstance().CheckIsReady(true))
                    {
                        return;
                    }

                    isWaferCheckSucceed = true;
                }

                // 复位信号
                Machine.GetInstance().ResetMachineSignal();

                // 芯片名称传给晶圆台
                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                // 给晶圆台发要料信号
                SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                SimpleButton btn = sender as SimpleButton;
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                // 初始化搜精
                Block.GetInstance().StartInit();

                this.pickupTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("取片测试线程");
                            this.Pickup();
                        });
            }
        }

        /// <summary>
        /// 从晶圆台取片
        /// </summary>
        private void Pickup()
        {
            try
            {
                bool ret;

                if (this.component.IsUseFlipTable == false)
                {
                    ret = this.System2Controller.PickupFromWaferTable(component);
                }
                else
                {
                    ret = this.System2Controller.PickupFromFlipTable(component);
                }

                if (!ret)
                {
                    return;
                }

                if (this.cameraType == CameraTypeEnum.UpLookCamera)
                {
                    Nozzle nozzle = this.BondHeadController.GetCurrentNozzle();

                    if (nozzle.ToolAlignment.State != AssistantStateEnum.Able)
                    {
                        // 去上视
                        this.BondModuleController.MoveToUpLookPos();
                    }
                    else
                    {
                        // 移到吸嘴旋转中心
                        this.BondModuleController.MoveToG0Pos(nozzle.RotateCenterPos);
                    }
                }
                else
                {
                    // 判断是哪个中转台
                    AKRSPoint3D iPTPosInG0 = this.component.IPTType == IPTTypeEnum.LeftIPT
                                                 ? BondDevicePara.GetInstance().IPTDevicePara.IPTPos
                                                 : BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos;

                    // IPT位置转到Bond
                    AKRSPoint3D iPTPosInBond = this.BondModuleController.ConvertG0ToMachinePos(iPTPosInG0);

                    double curAngle = this.BondHeadController.GetAxisTRealPos();

                    Nozzle nozzle = this.BondHeadController.GetCurrentNozzle();

                    // 计算焊头旋转后的吸嘴偏移
                    AKRSPoint2D offsetRotated = this.BondHeadController.GetNozzleOffset(
                        nozzle.Name,
                        curAngle - nozzle.AlignAngle);

                    AKRSPoint2D placePos = new AKRSPoint2D() { X = iPTPosInBond.X + component.IPTCenterOffset.X - offsetRotated.X, Y = iPTPosInBond.Y + component.IPTCenterOffset.Y - offsetRotated.Y };

                    // 运动到放料位
                    this.BondModuleController.MoveSafeBondXY(placePos.X, placePos.Y);

                    BondTypeEnum bondType = component.IPTType == IPTTypeEnum.LeftIPT ? BondTypeEnum.BondOnLeftIPT : BondTypeEnum.BondOnRightIPT;

                    // 焊头下降放片
                    double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;
                    this.BondHeadController.BondAction(
                        iPTPosInBond.Z + nozzle.MeasureHeightOffset + component.ComponentThickness,
                        liftLevel,
                        component,
                       bondType);

                    // 移动到视觉位
                    AKRSPoint3D targetPos = component.IPTType == IPTTypeEnum.LeftIPT?BondDevicePara.GetInstance().IPTDevicePara.IPTVisionPos: BondDevicePara.GetInstance().IPTDevicePara.RightIPTVisionPos;
                    this.BondModuleController.MoveToG0Pos(targetPos);

                    this.BeginInvoke(
                        new Action(
                            () =>
                                {
                                    this.BtnPickFromWaferTable.Appearance.BackColor = default;
                                    this.BtnPickFromWaferTable.Enabled = true;
                                }));
                }
            }
            catch (Exception ex)
            {
                AKRSMessageBoxExt.Show(
                    $"取片失败:" + ex.Message,
                    "报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK });
            }
            finally
            {
                this.BeginInvoke(
                    new Action(
                        () =>
                            {
                                this.BtnPickFromWaferTable.Appearance.BackColor = default;
                                this.BtnPickFromWaferTable.Enabled = true;
                            }));
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentAccuracyMode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.pickupTask != null && this.pickupTask.IsCompleted == false)
            {
                e.Cancel = true;
                return;
            }

            this.ucGuideMove1?.Dispose();
        }

        /// <summary>
        /// 芯片放到中转台
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPlaceComponentOnIPT_Click(object sender, EventArgs e)
        {
            BtnPlaceComponentOnIPT.Enabled = false;
            BtnPlaceComponentOnIPT.Appearance.BackColor = Color.Yellow;

            try
            {
                // 获取当前吸嘴
                Nozzle nozzle = this.BondHeadController.GetCurrentNozzle();

                // 判断是哪个中转台
                AKRSPoint3D iPTPosInG0 = this.component.IPTType == IPTTypeEnum.LeftIPT
                                             ? BondDevicePara.GetInstance().IPTDevicePara.IPTPos
                                             : BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos;

                // IPT位置转到Bond
                AKRSPoint3D iPTPosInBond =
                    this.BondModuleController.ConvertG0ToMachinePos(iPTPosInG0);

                double curAngle = this.BondHeadController.GetAxisTRealPos();

                // 计算焊头旋转后的吸嘴偏移
                AKRSPoint2D offsetRotated = this.BondHeadController.GetNozzleOffset(
                    nozzle.Name,
                    curAngle - nozzle.AlignAngle);

                AKRSPoint2D placePos = new AKRSPoint2D()
                {
                    X = iPTPosInBond.X + component.IPTCenterOffset.X - offsetRotated.X,
                    Y = iPTPosInBond.Y + component.IPTCenterOffset.Y - offsetRotated.Y
                };

                // Z轴移到安全高度
                this.BondHeadController.MoveBondZToSafePos();

                // 运动到放料位
                this.BondModuleController.MoveSafeBondXY(placePos.X, placePos.Y);

                // 清零
                this.BondHeadController.ZeroBondhead(false);

                BondTypeEnum bondType = component.IPTType == IPTTypeEnum.LeftIPT ? BondTypeEnum.BondOnLeftIPT : BondTypeEnum.BondOnRightIPT;

                // 焊头下降放片
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;
                this.BondHeadController.BondAction(
                    iPTPosInBond.Z + nozzle.MeasureHeightOffset + component.ComponentThickness,
                    liftLevel,
                    component,
                    bondType);

                // 移动到视觉位
                AKRSPoint3D targetPos = component.IPTType == IPTTypeEnum.LeftIPT ? BondDevicePara.GetInstance().IPTDevicePara.IPTVisionPos : BondDevicePara.GetInstance().IPTDevicePara.RightIPTVisionPos;
                this.BondModuleController.MoveToG0Pos(targetPos);
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"将芯片放到中转台失败:" + exception.Message,
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                BtnPlaceComponentOnIPT.Enabled = true;
                BtnPlaceComponentOnIPT.Appearance.BackColor = default;
            }
        }

        /// <summary>
        /// 从中转台取片
        /// </summary>
        private void PickFromIPT()
        {
            try
            {
                this.BondHeadController.MoveBondZToSafePos();

                // 获取当前吸嘴
                Nozzle nozzle = this.BondHeadController.GetCurrentNozzle();
                double curAngle = this.BondHeadController.GetAxisTRealPos();

                // 判断是哪个中转台
                AKRSPoint3D iPTPosInG0 = this.component.IPTType == IPTTypeEnum.LeftIPT
                                             ? BondDevicePara.GetInstance().IPTDevicePara.IPTPos
                                             : BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos;

                // IPT位置转到Bond
                AKRSPoint3D iPTPosInBond = this.BondModuleController.ConvertG0ToMachinePos(iPTPosInG0);

                // 计算焊头旋转后的吸嘴偏移
                AKRSPoint2D nozzleOffsetRotated = this.BondHeadController.GetNozzleOffset(
                    nozzle.Name,
                    curAngle - nozzle.AlignAngle);

                AKRSPoint2D pickPos = new AKRSPoint2D() { X = iPTPosInBond.X + component.IPTCenterOffset.X - nozzleOffsetRotated.X, Y = iPTPosInBond.Y + component.IPTCenterOffset.Y - nozzleOffsetRotated.Y };

                // 运动到取料位
                this.BondModuleController.MoveSafeBondXY(pickPos.X, pickPos.Y);

                PickTypeEnum pickType = component.IPTType == IPTTypeEnum.LeftIPT ? PickTypeEnum.LeftIPT : PickTypeEnum.RightIPT;

                // 焊头下降取片
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;
                this.BondHeadController.PickAction(
                    iPTPosInBond.Z + nozzle.MeasureHeightOffset + component.ComponentThickness,
                    component,
                    liftLevel,
                   pickType);
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show(
                    $"从中转台取片失败:" + e.Message,
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 取片然后旋转放下
        /// </summary>
        /// <param name="angle">角度</param>
        private void PickAndRotationAndDown(double angle)
        {
            try
            {
                // 获取当前吸嘴
                Nozzle nozzle = this.BondHeadController.GetCurrentNozzle();
                double curAngle = this.BondHeadController.GetAxisTRealPos();

                // 判断是哪个中转台
                AKRSPoint3D iPTPosInG0 = this.component.IPTType == IPTTypeEnum.LeftIPT
                                             ? BondDevicePara.GetInstance().IPTDevicePara.IPTPos
                                             : BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos;

                // IPT位置转到Bond
                AKRSPoint3D iPTPosInBond = this.BondModuleController.ConvertG0ToMachinePos(iPTPosInG0);

                // 计算焊头旋转后的吸嘴偏移
                AKRSPoint2D nozzleOffsetRotated = this.BondHeadController.GetNozzleOffset(
                    nozzle.Name,
                    curAngle - nozzle.AlignAngle);

                AKRSPoint2D pickPos = new AKRSPoint2D()
                {
                    X = iPTPosInBond.X + this.component.IPTCenterOffset.X - nozzleOffsetRotated.X,
                    Y = iPTPosInBond.Y + this.component.IPTCenterOffset.Y - nozzleOffsetRotated.Y
                };

                // Z轴移到安全高度
                this.BondHeadController.MoveBondZToSafePos();

                // 运动到取料位
                this.BondModuleController.MoveSafeBondXY(pickPos.X, pickPos.Y);

                PickTypeEnum pickType = component.IPTType == IPTTypeEnum.LeftIPT ? PickTypeEnum.LeftIPT : PickTypeEnum.RightIPT;

                // 焊头下降取片
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;
                this.BondHeadController.PickAction(
                    iPTPosInBond.Z + nozzle.MeasureHeightOffset + component.ComponentThickness,
                    this.component,
                    liftLevel,
                    pickType);

                // 计算焊头旋转后的吸嘴偏移
                AKRSPoint2D offsetRotated = this.BondHeadController.GetNozzleOffset(
                    nozzle.Name,
                    curAngle - angle - nozzle.AlignAngle);

                AKRSPoint2D placePos = new AKRSPoint2D()
                {
                    X = iPTPosInBond.X + this.component.IPTCenterOffset.X - offsetRotated.X,
                    Y = iPTPosInBond.Y + this.component.IPTCenterOffset.Y - offsetRotated.Y
                };

                // Z轴移到安全高度
                this.BondHeadController.MoveBondZToSafePos();

                this.BondHeadController.RelativeRotateAxisT(-angle);

                // 运动到放料位
                this.BondModuleController.MoveSafeBondXY(placePos.X, placePos.Y);

                BondTypeEnum bondType = component.IPTType == IPTTypeEnum.LeftIPT ? BondTypeEnum.BondOnLeftIPT : BondTypeEnum.BondOnRightIPT;

                this.BondHeadController.BondAction(
                    iPTPosInBond.Z + nozzle.MeasureHeightOffset + this.component.ComponentThickness,
                    liftLevel,
                    this.component,
                    bondType);

                // 移动到视觉位
                AKRSPoint3D targetPos = component.IPTType == IPTTypeEnum.LeftIPT ? BondDevicePara.GetInstance().IPTDevicePara.IPTVisionPos : BondDevicePara.GetInstance().IPTDevicePara.RightIPTVisionPos;
                this.BondModuleController.MoveToG0Pos(targetPos);
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show(
                    $"从中转台取放片失败:" + e.Message,
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}