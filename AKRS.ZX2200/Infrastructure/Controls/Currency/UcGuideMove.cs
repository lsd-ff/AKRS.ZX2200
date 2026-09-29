namespace AKRS.ZX2200.Infrastructure.Controls.Currency;

using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Main.Controls;
using BondSystem.Modules;
using DevExpress.CodeParser;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DispenseSystem.Modules;
using Galaxy2.Component.Simple.MoveControl;
using Galaxy2.CoordinateSystems.CoordinateSystems;
using Galaxy2.LogicHardware.Hardwares.Cameras;
using Galaxy2.LogicHardware.Hardwares.MotionControllers;
using Galaxy2.LogicHardware.Repository;
using Galaxy2.Machine.Enums;
using Main.Machine.MachineSupport;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TransportSystem.Modules;
using WaferSubSystem.Controllers;
using WaferSubSystem.Modules;
using UcMainSystem = AKRS.ZX2200.Main.Controls.Ucmain.MainControls.UcMainSystem;

/// <summary>
/// 光源和轴移动
/// </summary>
public partial class UcGuideMove : DevExpress.XtraEditors.XtraUserControl
{
    /// <summary>
    /// 在G0中的坐标
    /// </summary>
    private BaseCoordinateSystem coordinateSystem;

    /// <summary>
    /// 相机名称
    /// </summary>
    private CameraEnum cameraEnum;

    /// <summary>
    /// 模组的名称
    /// </summary>
    private string moduleName;

    /// <summary>
    /// 是否允许改变模组
    /// </summary>
    private bool allowChangeModule = true;

    /// <summary>
    /// 轴X
    /// </summary>
    private Axis axisX;

    /// <summary>
    /// 轴Y
    /// </summary>
    private Axis axisY;

    /// <summary>
    /// 轴Z
    /// </summary>
    private Axis axisZ;

    /// <summary>
    /// 轴U
    /// </summary>
    private Axis axisU;

    /// <summary>
    /// 相机
    /// </summary>
    private AKRSCamera camera;

    /// <summary>
    /// 方向盘
    /// </summary>
    private UcMoveControl ucMoveControl { get; set; } = new() { Dock = DockStyle.Fill };

    private string sourceName;

    /// <summary>
    /// 根据模组名字初始化
    /// </summary>
    public UcGuideMove(string sourceName, bool isMainUi = false)
    {
        if (!isMainUi)
        {
            MainForm.MF.UcMainSystem.GuideMoveClose();
        }

        this.InitializeComponent();
        this.panelControl2.Controls.Add(this.ucMoveControl);
        this.sourceName = sourceName;

        this.Disposed += (s, e) =>
            {
                this.Timer.Stop();
                this.Timer.Tick -= Timer_Tick;
                this.Timer.Dispose();
                this.ucMoveControl?.Dispose();
                IsJoystickEnable = false;
            };
    }

    /// <summary>
    /// 根据模组名字初始化
    /// </summary>
    public UcGuideMove(string sourceName)
    {
        this.InitializeComponent();
        this.panelControl2.Controls.Add(this.ucMoveControl);
        this.sourceName = sourceName;

        this.Disposed += (s, e) =>
        {
            this.Timer.Stop();
            this.Timer.Tick -= Timer_Tick;
            this.Timer.Dispose();
            this.ucMoveControl?.Dispose();
            IsJoystickEnable = false;
        };
    }

    /// <summary>
    /// 根据模组名字初始化
    /// </summary>
    /// <param name="moduleName">模组名称</param>
    /// <param name="allowChangeModule">是否允许修改模组</param>
    /// <param name="cameraEnum">相机名枚举</param>
    public UcGuideMove(string moduleName, string sourceName, bool allowChangeModule = true,
        CameraEnum cameraEnum = CameraEnum.None) : this(sourceName)
    {
        this.moduleName = moduleName;
        this.allowChangeModule = allowChangeModule;
        this.cameraEnum = cameraEnum;

        this.Disposed += (s, e) =>
            {
                this.Timer.Stop();
                this.Timer.Tick -= Timer_Tick;
                this.Timer.Dispose();
                this.ucMoveControl?.Dispose();
                IsJoystickEnable = false;
            };
    }

    /// <summary>
    /// Load 事件
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">参数封装</param>
    private void UcGuideMove_Load(object sender, EventArgs e)
    {
        this.InitControl();
        this.RefreshControl();

        this.TbExposure.Properties.Maximum = this.camera == null ? 0 : 60000;
        this.TbExposure.Properties.Minimum = this.camera == null ? 0 : 1;
        this.TbGain.Properties.Maximum = this.camera == null ? 0 : 20;
        this.TbGain.Properties.Minimum = this.camera == null ? 0 : 0;
        this.TbGama.Properties.Maximum = this.camera == null ? 0 : 40;
        this.TbGama.Properties.Minimum = this.camera == null ? 0 : 0;

        this.TbExposure.Value = this.camera == null ? 0 : (int)this.camera.GetExposureTime();
        this.TbGain.Value = this.camera == null ? 0 : (int)Math.Round(this.camera.GetGain());
        this.TbGama.Value = this.camera == null ? 0 : 40;
    }

    /// <summary>
    /// 绑定硬件
    /// </summary>
    private void BindHardware()
    {
        if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
        {
            // 脱机模式不绑定
            return;
        }

        AxisConfig axisConfig;

        // 根据名称绑定轴
        switch (this.moduleName)
        {
            case "点胶模组":
                DispenseModule dispenseModule = new DispenseModule();

                this.axisX = dispenseModule.GetDispenseXAxis();
                this.axisY = dispenseModule.GetDispenseYAxis();
                this.axisZ = dispenseModule.GetDispenseZAxis();
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                System1Domain.GetInstance().DispenseVisionController.InitLight(this.ucLight1, this.ucLight2);

                this.CbChangeModule.SelectedItem = "点胶模组";

                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
                    .Find(it => it.Name == "DispenseCoordinateSystem");
             

                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.DispenseCamera);

                    VisionModule visionModule = new();
                    this.camera = visionModule.DispenseCamera;
                }
           
                break;

            case "固晶模组":
                this.axisX = System2Module.GetInstance().BondModule.BondAxisX;
                this.axisY = System2Module.GetInstance().BondModule.BondAxisY;
                this.axisZ = System2Module.GetInstance().BondModule.BondHead.AxisZ;
                this.axisU = System2Module.GetInstance().BondModule.BondHead.AxisT;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, this.axisU, null, null);

                // Bond光源配置
                this.ucLight1.Init("固晶点光", "邦头三色点光-红", "邦头三色点光-绿", "邦头三色点光-蓝");
                this.ucLight2.Init("固晶环光", "邦头三色环光-红", "邦头三色环光-绿", "邦头三色环光-蓝");

                this.CbChangeModule.SelectedItem = "固晶模组";

                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
                    .Find(it => it.Name == "BondCoordinateSystem");
              

                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.BondCamera);
                    this.camera = System2Module.GetInstance().BondModule.BondCamera;
                }            

                break;

            case "上视模组":
                this.axisX = System2Module.GetInstance().BondModule.BondAxisX;
                this.axisY = System2Module.GetInstance().BondModule.BondAxisY;
                this.axisZ = System2Module.GetInstance().BondModule.BondHead.AxisZ;
                this.axisU = System2Module.GetInstance().BondModule.BondHead.AxisT;

                // Bond光源配置
                this.ucLight1.Init("上视点光", "上视点光", "上视点光", "上视点光");
                this.ucLight2.Init("上视环光", "上视环光", "上视环光", "上视环光");
                axisConfig = new(this.axisX, this.axisY, this.axisZ, this.axisU, null, null);
                this.CbChangeModule.SelectedItem = "上视模组";

                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
               .Find(it => it.Name == "BondCoordinateSystem");

                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.UplookCamera);
                    this.camera = System2Module.GetInstance().UpLookModule.UpLookCamera;
                }

                break;

            case "吸嘴架模组":
                this.axisY = HardwareRepositoryService.GetHardware<Axis>("焊头放置架Y");
                axisConfig = new(null, this.axisY, null, null, null, null);

                this.CbChangeModule.SelectedItem = "吸嘴架模组";

                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.BondCamera);

                    this.camera = System2Module.GetInstance().BondModule.BondCamera;
                }
            

                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
                    .Find(it => it.Name == "ToolBankCoordinateSystem");
                break;

            case "晶圆台模组":
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferTableAxisY;
                this.axisZ = WaferSubModule.GetInstance().WaferTable.ExpandAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.CbChangeModule.SelectedItem = "晶圆台模组";
                this.coordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems
                    .Find(it => it.Name == "WaferTableCoordinateSystem");


                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.WaferCamera);

                    this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
                }
            
                break;

            case "晶圆相机模组":
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferTableAxisY;
                this.axisZ = WaferSubModule.GetInstance().WaferTable.WaferCameraAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.CbChangeModule.SelectedItem = "晶圆相机模组";

                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.WaferCamera);
                    this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
                }
               
                break;

            case "晶圆料盒模组":
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferClampAxisY;
                this.axisZ = WaferSubModule.GetInstance().MagazineBox.MagazineAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.CbChangeModule.SelectedItem = "晶圆料盒模组";


                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.WaferCamera);
                    this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
                }
                break;

            case "顶针架模组":
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferTableAxisY;
                this.axisZ = WaferSubModule.GetInstance().Eject.EjectionTableAxisZ;
                this.axisU = WaferSubModule.GetInstance().Eject.EjectionBankAxisT;

                axisConfig = new(this.axisX, this.axisY, this.axisZ, this.axisU, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.CbChangeModule.SelectedItem = "顶针架模组";
                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.WaferCamera);
                    this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
                }
                break;

            case "顶针模组":
                this.axisX = WaferSubModule.GetInstance().WaferTable.WaferTableAxisX;
                this.axisY = WaferSubModule.GetInstance().WaferTable.WaferTableAxisY;
                this.axisZ = WaferSubModule.GetInstance().Eject.EjectionAxisZ;
                this.axisU = WaferSubModule.GetInstance().Eject.EjectionBankAxisT;

                axisConfig = new(this.axisX, this.axisY, this.axisZ, this.axisU, null, null);

                // 晶圆光源配置
                this.ucLight1.Init("晶圆点光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRedSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetGreenSpotLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetBlueSpotLightHardwareName()}");
                this.ucLight2.Init("晶圆环光",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}",
                    $"{WaferSubController.GetInstance().WaferTableController.GetRingLightHardwareName()}");

                this.CbChangeModule.SelectedItem = "顶针模组";
                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.WaferCamera);
                    this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
                }
                break;

            case "自动上料模组":

                this.axisX = loaderBinModule.LoaderPushRod;
                this.axisY = loaderBinModule.LoaderTableAxisY;
                this.axisZ = loaderBinModule.LoaderTableAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);
                this.CbChangeModule.SelectedItem = "自动上料模组";
                this.camera = null;
                break;

            case "自动下料模组":
             
                this.axisX = unLoaderBinModule.UnLoaderPushRod;
                this.axisY = unLoaderBinModule.UnLoaderTableAxisY;
                this.axisZ = unLoaderBinModule.UnLoaderTableAxisZ;
                axisConfig = new(this.axisX, this.axisY, this.axisZ, null, null, null);
                this.CbChangeModule.SelectedItem = "自动下料模组";
                this.camera = null;
                break;

            case "翻转台模组":

                this.axisU = flipModule.FlipTableAxisT;
                axisConfig = new(null, null, null, this.axisU, null, null);
                this.CbChangeModule.SelectedItem = "翻转台模组";
                if (IsSetCameraParematerByModule)
                {
                    this.ChangeCamera(CameraEnum.WaferCamera);
                    this.camera = WaferSubModule.GetInstance().WaferTable.WaferCamera;
                }
                break;

            default:

                AKRSXtraMessageBox.Show($"没有找到模组名为{this.moduleName}");

                axisConfig = new(null, null, null, null, null, null);
                break;
        }

        // 如果有传进来的相机 则重置相机
        if (this.cameraEnum != CameraEnum.None)
        {
            this.ChangeCamera(this.cameraEnum);
        }

        this.SetExposureTime();

        this.ucMoveControl.Init(axisConfig, this.sourceName);
    }

    /// <summary>
    /// 是否自动切换相机
    /// </summary>
    public  bool IsSetCameraParematerByModule = true;

    /// <summary>
    /// 改变模组名称
    /// </summary>
    /// <param name="changeModuleName">改变模组的名称</param>
    /// <param name="allowChangeModule">是否允许改变</param>
    public void  ChangeModuleName(string changeModuleName, bool allowChangeModule = true)
    {
        if (IsJoystickEnable)
        {
            // 摇杆的下拉框选项和正常的不一样
            return;
        }

        if (changeModuleName != null)
        {
            this.moduleName = changeModuleName;
        }

        this.BindHardware();

        if (IsSetCameraParematerByModule)
        {
            this.SetCameraParematerByModule(this.camera);
        }
    }

    /// <summary>
    /// 改变相机名称
    /// </summary>
    /// <param name="cameraEnum">相机名称</param>
    public void ChangeCamera(CameraEnum cameraEnum)
    {
        this.cameraEnum = cameraEnum;
        if (UcMainSystem.ChangeCameraVision != null)
        {
            UcMainSystem.ChangeCameraVision(cameraEnum.GetDescription());
        }
    }

    /// <summary>
    /// 改变事件
    /// </summary>
    /// <param name="sender">事件</param>
    /// <param name="e">事件源</param>
    private void CbChangeModule_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (IsJoystickEnable)
        {
            // 先停轴
            this.StopCurAxis();
            CurModule = this.CbChangeModule.SelectedItem.ToString();
            if (this.CbChangeModule.SelectedItem != null)
            {
                this.moduleName=this.CbChangeModule.SelectedItem.ToString();
                this.JoystickBindHardware();
            }
        }
        else
        {
            if (this.CbChangeModule.SelectedItem != null)
            {
                this.ChangeModuleName(this.CbChangeModule.SelectedItem.ToString(), this.allowChangeModule);
            }
        }
    }

    private void SetCameraParematerByModule(AKRSCamera aKRSCamera)
    {
        if (aKRSCamera == null)
        {
            return;
        }

        this.TbExposure.Value = (int)aKRSCamera.GetExposureTime();
        this.TbGain.Value = (int)Math.Round(aKRSCamera.GetGain());
        this.TbGama.Value = 40;
    }

    /// <summary>
    /// 坐标实时刷新
    /// </summary>
    /// <param name="sender">事件</param>
    /// <param name="e">参数</param>
    private void Timer_Tick(object sender, EventArgs e)
    {
        if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
        {
            return;
        }

        //// 如果不是停止状态则不允许移动方向盘
        //if (Machine.GetInstance().IsStop())
        //{
        //    this.ucMoveControl.Visible = true;
        //}
        //else
        //{
        //    this.ucMoveControl.Visible = false;
        //}

        if (this.axisX == null || this.axisY == null || this.axisZ == null)
        {
            return;
        }

        if (this.coordinateSystem == null)
        {
            return;
        }

        // 获取真实位置
        AKRSPoint3D point3D = new(
            this.axisX.GetCmdPosition(),
            this.axisY.GetCmdPosition(),
            this.axisZ.GetCmdPosition());

        point3D = this.coordinateSystem.ForwardConvertCoordinate(point3D);

        if (this.axisX == WaferSubModule.GetInstance().WaferTable.WaferTableAxisX)
        {
            this.TxX1.Text = WaferSubController.GetInstance().WaferTableController.WaferTableAxisXG0Pos.X.ToString();
        }
        else
        {
            this.TxX1.Text = point3D.X.ToString();
        }

        if (this.axisY == WaferSubModule.GetInstance().WaferTable.WaferTableAxisY)
        {
            this.TxY.Text = WaferSubController.GetInstance().WaferTableController.WaferTableAxisYG0Pos.Y.ToString();
        }
        else if (this.axisY == WaferSubModule.GetInstance().WaferTable.WaferClampAxisY)
        {
            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured)
            {
                this.TxY.Text = WaferSubController.GetInstance().WaferTableController.WaferClampAxisYG0Pos.Y.ToString();
            }
        }
        else
        {
            this.TxY.Text = point3D.Y.ToString();
        }

        if (this.axisZ == WaferSubModule.GetInstance().WaferTable.WaferCameraAxisZ)
        {
            this.TxZ.Text = WaferSubController.GetInstance().WaferTableController.WaferCameraAxisZG0Pos.Z.ToString();
        }
        else if (this.axisZ == WaferSubModule.GetInstance().WaferTable.ExpandAxisZ)
        {
            this.TxZ.Text = WaferSubController.GetInstance().WaferTableController.ExpandAxisZG0Pos.Z.ToString();
        }
        else if (this.axisZ == WaferSubModule.GetInstance().MagazineBox.MagazineAxisZ)
        {
            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured)
            {
                this.TxZ.Text = WaferSubController.GetInstance().MagazineController.MagazineAxisZG0Pos.Z.ToString();
            }
        }
        else if (this.axisZ == WaferSubModule.GetInstance().Eject.EjectionTableAxisZ)
        {
            this.TxZ.Text = WaferSubController.GetInstance().EjectController.EjectionTableAxisZG0Pos.Z.ToString();
        }
        else if (this.axisZ == WaferSubModule.GetInstance().Eject.EjectionAxisZ)
        {
            this.TxZ.Text = WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z.ToString();
        }
        else
        {
            this.TxZ.Text = point3D.Z.ToString();
        }

        this.ucMoveControl.Enabled = !IsJoystickEnable;
    }

    /// <summary>
    /// 打开影像窗口
    /// </summary>
    public void ShowVision()
    {
        UcMainSystem.VmVisionShow();
        if (this.cameraEnum != CameraEnum.None)
        {
            UcMainSystem.ChangeCameraVision(this.cameraEnum.GetDescription());
        }
    }

    /// <summary>
    /// 弹出对话框
    /// </summary>
    /// <param name="sender">事件</param>
    /// <param name="e">参数</param>
    private void BtVision_Click(object sender, EventArgs e)
    {
        this.ShowVision();
    }

    /// <summary>
    /// 曝光改变事件
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">封装参数</param>
    private void TbExposure_EditValueChanged(object sender, EventArgs e)
    {
        if (this.camera != null)
        {
            this.SpExposure.Value = (decimal)this.TbExposure.Value;
            this.camera.SetExposureTime((int)this.SpExposure.Value);
        }
    }

    /// <summary>
    /// 初始化曝光时间
    /// </summary>
    private void SetExposureTime()
    {
        if (this.camera != null)
        {
            //this.TbExposure.Value = (int)this.camera.GetExposureTime();
        }
    }

    private void BtVision_Click_1(object sender, EventArgs e)
    {
        this.ShowVision();
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    private void InitControl()
    {
        this.BtnJoystick.Appearance.BackColor = IsJoystickEnable ? Color.Yellow : default;
        this.BtnJoystick.Enabled = this.BtnJoystick.Visible = MachineHardwareConfiguration.GetInstance().IsJoyStickConfigured;
        this.rGSpeedMode.Enabled = this.rGSpeedMode.Visible = MachineHardwareConfiguration.GetInstance().IsJoyStickConfigured;

        // 清空现有项目
        this.rGSpeedMode.Properties.Items.Clear();

        // 添加三档选项
        this.rGSpeedMode.Properties.Items.Add(
            new RadioGroupItem(
                1,
                $"低速")
        );

        this.rGSpeedMode.Properties.Items.Add(
            new RadioGroupItem(
                50,
                $"中速")
        );

        this.rGSpeedMode.Properties.Items.Add(
            new RadioGroupItem(
                80,
                $"高速")
        );

        // 设置样式
        this.rGSpeedMode.Properties.Appearance.BackColor = Color.White;
        this.rGSpeedMode.Properties.BorderStyle = BorderStyles.Simple;
        this.rGSpeedMode.Properties.Appearance.Options.UseBackColor = true;
        this.rGSpeedMode.SelectedIndex = 1;
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    private void RefreshControl()
    {
        this.CbChangeModule.Properties.Items.Clear();

        if (IsJoystickEnable == false)
        {
            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.CbChangeModule.Properties.Items.Add("点胶模组");
            }

            this.CbChangeModule.Properties.Items.Add("固晶模组");
            this.CbChangeModule.Properties.Items.Add("上视模组");
            this.CbChangeModule.Properties.Items.Add("吸嘴架模组");
            this.CbChangeModule.Properties.Items.Add("晶圆台模组");
            this.CbChangeModule.Properties.Items.Add("晶圆相机模组");

            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured)
            {
                this.CbChangeModule.Properties.Items.Add("晶圆料盒模组");
            }

            this.CbChangeModule.Properties.Items.Add("顶针架模组");
            this.CbChangeModule.Properties.Items.Add("顶针模组");

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
            {
                this.CbChangeModule.Properties.Items.Add("自动上料模组");
                this.CbChangeModule.Properties.Items.Add("自动下料模组");
            }

            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                this.CbChangeModule.Properties.Items.Add("翻转台模组");
            }

            if (this.moduleName == null)
            {
                if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
                {
                    this.CbChangeModule.SelectedItem = "点胶模组";
                }
                else
                {
                    this.CbChangeModule.SelectedItem = "固晶模组";
                }
            }
            else
            {
                this.CbChangeModule.SelectedItem = this.moduleName;
            }
        }
        else
        {
            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.CbChangeModule.Properties.Items.Add("点胶XY");
                this.CbChangeModule.Properties.Items.Add("点胶Z");
            }

            this.CbChangeModule.Properties.Items.Add("固晶XY");
            this.CbChangeModule.Properties.Items.Add("固晶Z");
            this.CbChangeModule.Properties.Items.Add("固晶T");
            this.CbChangeModule.Properties.Items.Add("吸嘴架Y");
            this.CbChangeModule.Properties.Items.Add("晶圆台XY");

            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured)
            {
                this.CbChangeModule.Properties.Items.Add("晶圆夹Y");
                this.CbChangeModule.Properties.Items.Add("晶圆料盒Z");
            }

            this.CbChangeModule.Properties.Items.Add("晶圆台扩晶Z");
            this.CbChangeModule.Properties.Items.Add("晶圆相机Z");

            this.CbChangeModule.Properties.Items.Add("顶针台Z");
            this.CbChangeModule.Properties.Items.Add("顶针架T");
            this.CbChangeModule.Properties.Items.Add("顶针Z");

            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin)
            {
                this.CbChangeModule.Properties.Items.Add("自动上料模组XY");
                this.CbChangeModule.Properties.Items.Add("自动上料模组Z");
                this.CbChangeModule.Properties.Items.Add("自动下料模组XY");
                this.CbChangeModule.Properties.Items.Add("自动下料模组Z");
            }

            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                this.CbChangeModule.Properties.Items.Add("翻转台T");
            }

            this.CbChangeModule.SelectedItem = CurModule;
        }
    }

    /// <summary>
    /// 改变增益
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">封装参数</param>
    private void TbGain_EditValueChanged(object sender, EventArgs e)
    {
        this.camera?.SetGain(TbGain.Value);
        this.SpGain.EditValue = TbGain.Value;
    }

    /// <summary>
    /// 改变伽马
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">封装参数</param>
    private void TbGama_EditValueChanged(object sender, EventArgs e)
    {
        double gamma = (double)TbGama.Value / 10;
        this.camera?.SetGamma(gamma);
        this.SpGamma.EditValue = TbGama.Value;
    }
}

/// <summary>
/// 相机枚举
/// </summary>
public enum CameraEnum
{
    /// <summary>
    /// 无
    /// </summary>
    [Description("None")]
    None,

    /// <summary>
    /// Bond相机
    /// </summary>
    [Description("Bond相机")]
    BondCamera,

    /// <summary>
    /// 点胶相机
    /// </summary>
    [Description("点胶相机")]
    DispenseCamera,

    /// <summary>
    /// 晶圆相机
    /// </summary>
    [Description("晶圆相机")]
    WaferCamera,

    /// <summary>
    /// 上视相机
    /// </summary>
    [Description("上视相机")]
    UplookCamera
}