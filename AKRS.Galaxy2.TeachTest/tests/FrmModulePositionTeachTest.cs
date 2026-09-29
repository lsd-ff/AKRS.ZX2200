using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Models.CommonModels;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.TransportUnit.Controls.Assistant.ModuleTeach
{
    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmModulePositionTeachTest : XtraForm
    {
        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 9;

        /// <summary>
        /// 载具
        /// </summary>
       // private Galaxy2.Carriers.Models.TransportUnit TransportUnit => MachineConfigContext.GetInstance().CurrentTransportUnit;

        /// <summary>
        /// 原点位置1
        /// </summary>
        private AKRSPoint3D orginPos1;

        /// <summary>
        /// 原点位置2
        /// </summary>
        private AKRSPoint3D orginPos2;

        /// <summary>
        /// 原点位置
        /// </summary>
        private AKRSPoint3D orginPos;

        /// <summary>
        /// 拉角度点1
        /// </summary>
        private AKRSPoint3D anglePos1;

        /// <summary>
        /// 拉角度点2
        /// </summary>
        private AKRSPoint3D anglePos2;

        /// <summary>
        /// 角度
        /// </summary>
        private double degree;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 开始点
        /// </summary>
        private AKRSPoint3D startPos;

        /// <summary>
        /// 行末点
        /// </summary>
        private AKRSPoint3D rowEndPos;

        /// <summary>
        /// 列末点
        /// </summary>
        private AKRSPoint3D columnEndPos;

        /// <summary>
        /// 行列数
        /// </summary>
        private int rows, columns;

        /// <summary>
        /// 行间距
        /// </summary>
        private double rowSpacing;

        /// <summary>
        /// 列间距
        /// </summary>
        private double columnSpacing;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmModulePositionTeachTest()
        {
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
                        descritpion: $"Step 1/{stepCount}; Determination of rotary position Transport: TransportUnit \n"
                                     + @"Move to a prominent point at the left Transport unit end (to define the current Transport unit rotary displacement)",
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

                            //this.anglePos1 = this.ChooseSystem();
                           // this.anglePos1 = this.TransportUnit.SubstrateArray[0,0].SubstrateCoordinateSystem.G0PosToSelf(this.anglePos1);
                        },
                       doneAction: () =>
                       {
                       }),
                 
                    // 拉角度点2
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"Step 2/{stepCount}; Determination of rotary position Transport: TransportUnit \n"
                                     + @"Move to a prominent point at the right Transport unit end in a horizontal direction from the first point",
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
                            this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin1;

                            //this.anglePos2 = this.ChooseSystem();
                           // this.anglePos2 = this.TransportUnit.SubstrateArray[0,0].SubstrateCoordinateSystem.G0PosToSelf(this.anglePos2);

                            // todo 角度计算
                            //double tanValue = (this.anglePos1.Y - this.anglePos2.Y) / (this.anglePos1.X - this.anglePos2.X);
                            //degree = Math.Atan(tanValue);
                        },
                        doneAction: () =>
                        {
                        }),

                          // 原点1
                          new AssistantConfig(
                              index: 0,
                              descritpion:
                              $"Step 3/{stepCount}; Specify origin \n"
                              + " Move to Module zero position (M0) with the crosshairs.\n"
                              + " Confirm with DONE to save current position as Transport unit zero position (M0).\n"
                              + " Confirm with NEXT to enter more then one position and calculate the center at the end.",
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiRotaryPoint2;
                                  },
                              nextAction: () =>
                                  {
                                       this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin2;

                                      // this.orginPos1 = this.ChooseSystem();
                                     // this.orginPos1 = this.TransportUnit.SubstrateArray[0,0].SubstrateCoordinateSystem.G0PosToSelf(this.orginPos1);
                                  },
                              doneAction: () =>
                                  {
                                       // 只找一个中心原点时,直接跳到第三步
                                       this.TileBarTeach.SelectedItem = this.TbiStartPoint;
                                       this.stepIndex++;

                                      // this.orginPos = this.ChooseSystem();
                                      // this.orginPos = this.TransportUnit.SubstrateArray[0,0].SubstrateCoordinateSystem.G0PosToSelf(this.orginPos);
                                  }),
                 
                          // 原点2
                          new AssistantConfig(
                              index: 1,
                              descritpion: $"Step 4/{stepCount}; Specify origin \n"
                                           + " Move to Module zero position (M0) with the crosshairs.\n"
                                           + " Confirm with DONE to calculate the Module zero position (M0) with two opposing positions.\n"
                                           + " Confirm with NEXT to enter more then two positions and calculate the center at the end",
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
                                      this.TileBarTeach.SelectedItem = this.TbiStartPoint;

                                      // 找原点2
                                     // this.orginPos2 = this.ChooseSystem();
                                      // this.orginPos2 = this.TransportUnit.SubstrateArray[0,0].SubstrateCoordinateSystem.G0PosToSelf(this.orginPos2);
                                     // this.orginPos = (this.orginPos1 + this.orginPos2) / 2;
                                  }),

                          // 首料点
                          new AssistantConfig(
                              index: 2,
                              descritpion: $"Step 5/{stepCount}; Index/line feed determination Module \n"
                                           + @" Move to corner or pattern on first Module to then determine index/line feed",
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiSpecifyOrigin2;
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiColumnEndPoint;

                                      //this.startPos = this.ChooseSystem();
                                       //this.startPos = this.TransportUnit.SubstrateArray[0,0].SubstrateCoordinateSystem.G0PosToSelf(this.startPos);
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 列尾料点
                          new AssistantConfig(
                              index: 3,
                              descritpion: $"Step 6/{stepCount}; Index/line feed determination Module \n"
                                           + @" Move to the same point on last Module in the same row",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiStartPoint;
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiColumnCount;

                                     // this.columnEndPos = this.ChooseSystem();
                                     // this.columnEndPos = this.TransportUnit.SubstrateArray[0,0].SubstrateCoordinateSystem.G0PosToSelf(this.columnEndPos);
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 列数
                          new AssistantConfig(
                              index: 4,
                              descritpion: $"Step 7/{stepCount}; Index/line feed determination Module \n"
                                           + @" Move to the same point on last Module in the same row",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiColumnEndPoint;
                                  },
                              nextAction: () =>
                                  {
                                      // 输入列数
                                      this.columns = XtraInputBox.Show("please enter the number of columns", "columns", 2);

                                      this.TileBarTeach.SelectedItem = this.TbiRowEndPoint;

                                      // 计算列间距
                                      //this.columnSpacing = Math.Abs((this.columnEndPos.X - this.startPos.X) / (this.columns - 1));
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 行尾料点
                          new AssistantConfig(
                              index: 5,
                              descritpion: $"Step 8/{stepCount}; Index/line feed determination Module \n"
                                           + @" Move to the same point on last Module in the same column",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiColumnCount;
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiRowCount;

                                     // this.rowEndPos = this.ChooseSystem();
                                      // this.rowEndPos = this.TransportUnit.SubstrateArray[0,0].SubstrateCoordinateSystem.G0PosToSelf(this.rowEndPos);
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 行数
                          new AssistantConfig(
                              index: 6,
                              descritpion: $"Step 9/{stepCount}; Index/line feed determination Module \n"
                                           + @" Input number of columns",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: false,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiRowEndPoint;
                                  },
                              nextAction: () =>
                                  {
                                  },
                              doneAction: () =>
                                  {
                                      // 输入行数
                                      this.rows = XtraInputBox.Show("please enter the number of rows", "rows", 2);

                                      // 计算行间距
                                    //  this.rowSpacing = Math.Abs((this.columnEndPos.Y - this.rowEndPos.Y) / (this.rows - 1));

                                      // 创建module坐标系
                                      //this.TransportUnit.CreateIslandCoordinateSystem(
                                      //    this.orginPos,
                                      //    0,
                                      //    this.rows,
                                      //    this.columns,
                                      //    this.rowSpacing,
                                      //    this.columnSpacing);

                                      // 全部完成后把数据传到配置中，最后关闭窗口
                                      //this.TransportUnit.ModuleConfig.Origin = this.orginPos;
                                      //this.TransportUnit.ModuleConfig.Angle = this.degree;
                                      //this.TransportUnit.ModuleConfig.ColumnSpacing = this.columnSpacing;
                                      //this.TransportUnit.ModuleConfig.RowSpacing = this.rowSpacing;

                                      this.DialogResult = DialogResult.OK;
                                      this.Close();
                                  }),
                      };
            //UcGuideMove ucGuideMove = new UcGuideMove();
            //ucGuideMove.ChangeModuleName("固晶模组");
            //this.PnlControl.Controls.Add(ucGuideMove);

            // 测试
            // this.TransportUnit.ModuleConfig.IsMultiple = true;
            // 如果是多个基板  则
            //if (this.transportUnit.IslandAcupointConfig.IsMultiple)
            //{
            //    this.stepCount = 7;
            //    this.assistantConfigList.ForEach(a => a.IsShowTitle = true);
            //}
            //else
            //{
            //    // 单基板
            //    this.stepCount = 2;
            //    this.assistantConfigList.RemoveAt(2);
            //    this.assistantConfigList.RemoveAt(3);
            //    this.assistantConfigList.RemoveAt(4);
            //    this.assistantConfigList.RemoveAt(5);
            //    this.assistantConfigList.RemoveAt(6);

            //    this.TbiStartPoint.Visible = false;
            //    this.TbiColumnEndPoint.Visible = false;
            //    this.TbiColumnCount.Visible = false;
            //    this.TbiRowEndPoint.Visible = false;
            //    this.TbiRowCount.Visible = false;
            //}

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
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.stepIndex++;

            // 避免索引超出数组界限
            if (this.stepIndex > this.stepCount - 1)
            {
                this.stepIndex--;
            }

            this.SetUiControl(this.stepIndex);
        }

        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {

        }

        private void TileBarTeach_Click(object sender, EventArgs e)
        {

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