using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.ZX2200.Models.CommonModels;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.TransportUnit.Controls.Assistant.CarrierTeach
{
    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmTUPoistionTeachTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 5;

        /// <summary>
        /// 载具
        /// </summary>
       // private Galaxy2.Carriers.Models.TransportUnit TransportUnit => MachineConfigContext.GetInstance().CurrentTransportUnit;
        
        /// <summary>
        /// 创建BondModule实例
        /// </summary>
       // private BondModule bondModule = new BondModule();

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 原点位置1
        /// </summary>
        private AKRSPoint3D orginPos1;

        /// <summary>
        /// 原点位置2
        /// </summary>
        private AKRSPoint3D orginPos2;

        /// <summary>
        /// 拉角度点1
        /// </summary>
        private AKRSPoint3D anglePos1;

        /// <summary>
        /// 拉角度2
        /// </summary>
        private AKRSPoint3D anglePos2;

        /// <summary>
        /// 角度
        /// </summary>
        private double angle;

        /// <summary>
        /// 开始测高的位置
        /// </summary>
        private AKRSPoint3D searchPos;

        /// <summary>
        /// 高度
        /// </summary>
        private double height;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmTUPoistionTeachTest()
        {
           // DispenseDomain.GetInstance().DispenseModule.MoveToSafePos();
           // DispenseDomain.GetInstance().DispenseModule.DispenseAltimemetryCyclinder.SetOutputValue(true);
            this.InitializeComponent();
            this.InitControl();
            this.BtBack.Visible = false;
            this.BtDone.Visible = false;
            this.TileBarTeach.SelectedItem = this.TbiRotaryPoint1;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                      {
             
                          // 拉角度点1
                          new AssistantConfig(
                              index: 0,
                              descritpion:
                              $"Step 1/{stepCount}; Determination of rotary position Transport: TransportUnit \n"
                              + "Move to a prominent point at the left Transport unit end \n"
                              + "(to define the current Transport unit rotary displacement)",
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiRotaryPoint2;

                                     // this.anglePos1 = this.ChooseSystem();
                                  },
                              doneAction: () =>
                                  {
                                  }),
                 
                          // 拉角度点2
                          new AssistantConfig(
                              index: 1,
                              descritpion: $"Step 2/{stepCount}; Determination of rotary position Transport: TransportUnit \n"
                                           + "Move to a prominent point at the right Transport unit end \n"
                                           + "in a horizontal direction from the first point",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiRotaryPoint1;
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiHeightMeasurement;

                                     // this.anglePos2 = this.ChooseSystem();

                                      // todo 角度计算
                                     // double tanValue = (this.anglePos1.Y - this.anglePos2.Y) / (this.anglePos1.X - this.anglePos2.X);
                                     // angle = Math.Atan(tanValue);
                                      //angle = radian * (180 / Math.PI);
                                  },
                              doneAction: () =>
                                  {
                                  }),
                          // 测高
                          new AssistantConfig(
                              index: 0,
                              descritpion:
                              $"Step 3/{stepCount}; Teach-in height measurement Transport unit \n"
                              + "Move to the point with the substrate camera where the height measurement will be carried out. \n"
                              + "the position must be reachable in the ID system with the PP axis",
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      TileBarTeach.SelectedItem = TbiRotaryPoint2;
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin1;

                                      // this.searchPos = this.ChooseSystem();

                                      // 执行测高
                                      //if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
                                      //{
                                      //    (ExcuteResult Ret, double HeightValue) res = this.bondModule.MeasureHeight(
                                      //        BondDevicePara.GetInstance().BondHeadParam.SafeLevel,
                                      //        "接触传感器");
                                      //    if (res.Ret == ExcuteResult.Success)
                                      //    {
                                      //        height = res.HeightValue;
                                      //    }
                                      //    else
                                      //    {
                                      //        XtraMessageBox.Show("测高错误");
                                      //    }
                                      //}
                                      //else
                                      //{
                                      //    // 打开点胶测高气缸
                                      //    DispenseDomain.GetInstance().DispenseModule.DispenseAltimemetryCyclinder.SetOutputValue(true);
                                      //    // 在当前位置开始测高

                                      //    (ExcuteResult Ret, double HeightValue) res = DispenseDomain.GetInstance().DispenserHeightMeasurement(DispenseDomain.GetInstance().DispenseModule.DispenseAltimemetryElectric);

                                      //    // HeightValue的使用


                                      //    if (res.Ret == ExcuteResult.Success)
                                      //    {
                                      //       height = res.HeightValue;
                                      //    }
                                      //    else
                                      //    {
                                      //        XtraMessageBox.Show("测高错误");
                                      //    }
                                      //}
                                  },
                              doneAction: () =>
                                  {
                                  }),
                 
                          // 找原点1
                          new AssistantConfig(
                              index: 1,
                              descritpion: 
                              $"Step 4/{stepCount}; Specify origin \n"
                              + " Move to Transport unit zero position (T0) with the crosshairs. \n"
                              + "Confirm with DONE to save current position as Transport unit zero position (T0).\n"
                              + "Confirm with NEXT to enter more then one position and calculate the center at the end.",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiHeightMeasurement;
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin2;

                                      // 找原点1
                                      // this.orginPos1 = this.ChooseSystem();
                                  },
                              doneAction: () =>
                                  {
                                      // 找中心点
                                      //this.TransportUnit.TransportUnitConfig.Origin = this.ChooseSystem();

                                      //GeneralCoordinateSystem generalCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name=="TransportUniCoordinateSystem");
                                      //this.TransportUnit.TransportUnitConfig.Origin = generalCoordinateSystem.G0PosToSelf(this.TransportUnit.TransportUnitConfig.Origin);

                                      //double distanceZ = DispenseDevicePara.GetInstance().DispenseModulePara.DispenseVisionPos.Z - DispenseDevicePara.GetInstance().DispenseModulePara.DispenseSearchPos.Z;

                                      //this.TransportUnit.TransportUnitConfig.Origin.Z = this.TransportUnit.TransportUnitConfig.Origin.Z - height + distanceZ;

                                      //// 创建carrier坐标系
                                      //this.TransportUnit.CreateCarrierCoordinateSystem(this.TransportUnit.TransportUnitConfig.Origin, 0);

                                      //// 测高数据传到配置中，最后关闭窗口
                                      // this.TransportUnit.TransportUnitConfig.Height = this.height;
                                      //this.DialogResult = DialogResult.OK;
                                      this.Close();
                                  }),

                          // 找原点2
                          new AssistantConfig(
                              index: 2,
                              descritpion: 
                              $"Step 5/{stepCount}; Specify origin \n"
                              + " Move to Transport unit zero position (T0) with the crosshairs.\n"
                              + " Confirm with DONE to calculate the Transport unit zero position (T0) with two opposing positions.\n"
                              + " Confirm with NEXT to enter more then two positions and calculate the center at the end ",
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
                                      // 找原点2
                                      //this.orginPos2 = this.ChooseSystem();
                                      //this.TransportUnit.TransportUnitConfig.Origin = (this.orginPos1 + this.orginPos2) / 2;

                                      // GeneralCoordinateSystem generalCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name=="TransportUniCoordinateSystem");
                                      //this.TransportUnit.TransportUnitConfig.Origin = generalCoordinateSystem.G0PosToSelf(this.TransportUnit.TransportUnitConfig.Origin);

                                      //double distanceZ = DispenseDevicePara.GetInstance().DispenseModulePara.DispenseVisionPos.Z - DispenseDevicePara.GetInstance().DispenseModulePara.DispenseSearchPos.Z;

                                      //this.TransportUnit.TransportUnitConfig.Origin.Z = height + distanceZ;

                                      //// 创建carrier坐标系
                                      //this.TransportUnit.CreateCarrierCoordinateSystem(this.TransportUnit.TransportUnitConfig.Origin, 0);

                                  
                                      //// 测高数据传到配置中，最后关闭窗口
                                      //this.TransportUnit.Height = this.height;
                                      this.DialogResult = DialogResult.OK;
                                      this.Close();
                                  }),
                      };
            //UcGuideMove ucGuideMove = new UcGuideMove();
            //ucGuideMove.ChangeModuleName("固晶模组");
            //this.PnlControl.Controls.Add(ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
        }

        /// <summary>
        /// 如果是坐标点，选择系统2为固晶域，系统1为点胶域
        /// </summary>
        /// <returns>坐标点</returns>
        //private AKRSPoint3D ChooseSystem()
        //{
        //    if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
        //    {
        //        AKRSPoint3D akrsPoint3D = BondDomain.GetInstance().this.bondModuleController.Get3DRealPosition();
        //        return akrsPoint3D;
        //    }
        //    else
        //    {
        //        AKRSPoint3D akrsPoint3D = DispenseDomain.GetInstance().DispenseModule.GetPosByG0();
        //        return akrsPoint3D;
        //    }
        //}

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
            this.SetUiControl(this.stepIndex);
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
            this.SetUiControl(this.stepIndex);
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
            this.Close();
        }
    }
}