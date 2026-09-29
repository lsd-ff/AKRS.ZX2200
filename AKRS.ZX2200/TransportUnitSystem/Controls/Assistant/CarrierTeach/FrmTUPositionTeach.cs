using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant.CarrierTeach
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    using DevExpress.XtraEditors;

    using static Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmTUPositionTeach : DevExpress.XtraEditors.XtraForm
    {
        #region MyRegion
        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 5;

        /// <summary>
        /// 流道坐标系
        /// </summary>
        private GeneralCoordinateSystem TransportSystemCoordinateSystem =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().Find("TransportCoordinateSystem");

        /// <summary>
        /// 传输单元对象
        /// </summary>
        private TransportUnit transportUnit;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 示教的参数数据
        /// </summary>

       private (AKRSPoint3D realOriginPos1, double realOriginPosHeight, double safeHeight, double additionalSafeHeight, AdditionalSafetyHeightType additionalSafetyHeightType, MultipleHeightMeasurementType type) args;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmTUPositionTeach()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmTUPositionTeach_Load(object sender, EventArgs e)
        {
            // 获取载台上的TU对象
            this.transportUnit = TUAssistantHelper.JudgeTuExist();
            if (this.transportUnit == null)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            // 判断测高方式
            FrmChooseMeasureHeightType frm = new FrmChooseMeasureHeightType();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                args.type = frm.Type;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }
           
            this.InitControl();
            this.SetUiControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {

            string message1, message2, message3, message4, message5, message6;
            AKRSPoint3D originPos1 = new AKRSPoint3D();
            AKRSPoint3D originPos2 = new AKRSPoint3D();
            AKRSPoint3D originPos3 = new AKRSPoint3D();
            AKRSPoint3D originPos4 = new AKRSPoint3D();

            TUAssistantHelper.SetColor(this.TileBarTeach);

            if (args.type == MultipleHeightMeasurementType.TouchDown)
            {
                message1 = $"步骤 1/{stepCount}: 载具测高\r\n"
                           + $"将测高吸嘴移动到载具上方5mm处进行测高\r\n"
                           + $"点击下一步，设备将执行测高";
            }
            else
            {
                message1 = $"步骤 1/{stepCount}: 载具测高\r\n"
                           + $"移动固晶相机使得相机能够看清晰载具\r\n"
                           + $"点击下一步，设备将执行测高";
            }

            // 安全高度
            message2 = $"步骤 2/{stepCount}:XY轴移动时Z轴的安全高度 \r\n"
                              + $"设备将记录此时的高度作为载具的Z方向零位";

            message3 = $"步骤 3/{stepCount}:找原点1\r\n"
                              + $"移动固晶相机去找到载具的中心（角落），设备将设置该点为载具坐标系原点\r\n"
                              + $"如果点击完成，设备将相机现在所看的点设置为载具坐标系原点（载具中心点）\r\n" 
                              + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

            message4 = $"步骤 4/{stepCount}:找原点2\r\n"
                            + $"移动固晶相机去找到载具的中心（角落），设备将设置该点为载具坐标系原点\r\n"
                              + $"如果点击完成，设备计算载具坐标系原点（载具中心点）\r\n"
                              + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

            message5 = $"步骤 5/{stepCount}:找原点3\r\n"
                            + $"移动固晶相机去找到载具的中心（角落），设备将设置该点为载具坐标系原点\r\n"
                            + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

            message6 = $"步骤 6/{stepCount}:找原点4\r\n" 
                            + $"移动固晶相机去找到载具的中心（角落），设备将设置该点为载具坐标系原点\r\n"
                            + $"点击完成，设备将计算载具坐标系原点（载具中心点）\r\n";

            assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 测高
                          new AssistantConfig(
                              index: 0,
                              descritpion: message1,
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      (ExcuteResult result, double height) =
                                          TUAssistantHelper.AssistantMeasureHeight(args.type);

                                      if (result == ExcuteResult.Success)
                                      {
                                          if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                                          {
                                              args.realOriginPosHeight = this.TransportSystemCoordinateSystem
                                                  .G0PosToSelf(new AKRSPoint3D(0, 0, height)).Z;
                                          }

                                          if(args.type == MultipleHeightMeasurementType.TouchDown)
                                          {
                                              AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();
                                              point3D.Z = height;
                                              System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(point3D);
                                          }
                                      }
                                      else
                                      {
                                          this.stepIndex--;
                                          return;
                                      }

                                      this.Hide();

                                      FrmEditSafeHeight frmEditSafeHeight = new FrmEditSafeHeight();
                                      frmEditSafeHeight.ShowDialog();

                                      if (frmEditSafeHeight.DialogResult == DialogResult.Cancel)
                                      {
                                          this.DialogResult = DialogResult.Cancel;
                                          this.Close(); 
                                          this.Dispose();
                                          return;
                                      }
                                      else if (frmEditSafeHeight.DialogResult == DialogResult.None)
                                      {
                                          this.stepIndex--;
                                      }

                                      args.safeHeight = frmEditSafeHeight.SafeHeight;
                                      args.additionalSafeHeight =frmEditSafeHeight.AdditionalSafeHeight;
                                      args.additionalSafetyHeightType = frmEditSafeHeight.Type;

                                      frmEditSafeHeight.Dispose(); 

                                      this.stepIndex++;
                                      this.Show();
                                  },
                              doneAction: () =>
                                  {
                                  }),
                 
                          // 输入安全高度
                          new AssistantConfig(
                              index: 1,
                              descritpion: message2,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 找原点1
                          new AssistantConfig(
                              index: 2,
                              descritpion: message3,
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      originPos1 = TUAssistantHelper.GetPosBySystem(this.TransportSystemCoordinateSystem);
                                  },
                              doneAction: () =>
                                  {
                                      // 直接退出
                                      args.realOriginPos1 = TUAssistantHelper.GetPosBySystem(this.TransportSystemCoordinateSystem);
                                      this.Save();
                                      this.DialogResult = DialogResult.OK;
                                  }),
                 
                          // 找原点2
                          new AssistantConfig(
                              index: 3,
                              descritpion: message4,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      // 找原点2
                                      originPos2 = TUAssistantHelper.GetPosBySystem(this.TransportSystemCoordinateSystem);
                                  },
                              doneAction: () =>
                                  {
                                      originPos2 = TUAssistantHelper.GetPosBySystem(this.TransportSystemCoordinateSystem);

                                      // 找原点2
                                      args.realOriginPos1 = (originPos1 + originPos2) / 2;
                                      this.Save();
                                      this.DialogResult = DialogResult.OK;
                                  }),

                          // 找原点3
                          new AssistantConfig(
                              index: 4,
                              descritpion: message5,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      // 找原点3
                                      originPos3 = TUAssistantHelper.GetPosBySystem(this.TransportSystemCoordinateSystem);
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 找原点4
                          new AssistantConfig(
                              index: 5,
                              descritpion: message6,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: false,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin1;
                                  },
                              nextAction: () =>
                                  {
                                  },
                              doneAction: () =>
                                  {
                                      originPos4 = TUAssistantHelper.GetPosBySystem(this.TransportSystemCoordinateSystem);

                                      args.realOriginPos1 =
                                          (originPos1 + originPos2 + originPos3 + originPos4) / 4;

                                      this.Save();
                                      this.DialogResult = DialogResult.OK;
                                  }),
                      };

            ucGuideMove = new UcGuideMove("固晶模组", "FrmTUPositionTeach", true, CameraEnum.BondCamera);
  
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;

            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            // 传递配置信息
            ProductConfiguration.GetInstance().TransportUnitConfig
                .ElementCoordinate.Point = args.realOriginPos1;

            ProductConfiguration.GetInstance().TransportUnitConfig.ElementCoordinate.Point.Z = args.realOriginPosHeight;

            // 重新示教默认角度为0
            ProductConfiguration.GetInstance().TransportUnitConfig.ElementCoordinate.Degree = 0;

            ProductConfiguration.GetInstance().TransportUnitConfig
                .SafeHeight = args.safeHeight;

            ProductConfiguration.GetInstance().TransportUnitConfig
                .AdditionalSafeHeight = args.additionalSafeHeight;

            ProductConfiguration.GetInstance().TransportUnitConfig
                .AdditionalSafetyHeightType = args.additionalSafetyHeightType;

            // 完成
            ProductConfiguration.GetInstance().TransportUnitConfig.TUPosition.State = AssistantStateEnum.Able;
            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        private void SetUiControl()
        {
            if (this.IsDisposed)
            {
                return;
            }

            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[this.stepIndex];
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];

            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
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
            this.SetUiControl();
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
            this.SetUiControl();
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
            this.stepIndex++;
        }

        /// <summary>
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
           this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtAutoFocus_Click(object sender, EventArgs e)
        {
            this.bondHeadController.AutoFocusAssistance(BondSystem.Models.Enums.CameraTypeEnum.BondCamera, sender,this);
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void EditPr_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPrInSystem2("框架示教找中心模板");
        }

        /// <summary>
        /// 移动到相机中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtMoveToCenter_Click(object sender, EventArgs e)
        {
            (bool success, AKRSPoint3D point) result = TUAssistantHelper.AssistantPR("框架示教找中心模板");

            if (result.success)
            {
                TUAssistantHelper.MoveToPos(result.point - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset);
            }
            else
            {
                AKRSXtraMessageBox.Show("定位失败，请重置制作模板");
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmTUPositionTeach_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();  
        }
    }
}