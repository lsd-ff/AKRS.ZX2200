using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Models;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant.ModuleTeach
{
    using System.Linq;

    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.ZX2200.BondSystem.Controls.Assistant;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;


    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmModulePositionTeach : XtraForm
    {
        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        #region 参数

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 9;

        /// <summary>
        /// 真实高度
        /// </summary>
        private double realHeight;

        /// <summary>
        /// 原点位置
        /// </summary>
        private readonly List<AKRSPoint3D> realOriginPos = new List<AKRSPoint3D>();

        /// <summary>
        /// 行列数
        /// </summary>
        private int rows, columns;

        /// <summary>
        /// 行列间距
        /// </summary>
        private double rowSpacing, rowSpacingX, columnSpacing, columnSpacingY;

        /// <summary>
        /// module的数量
        /// </summary>
        private int moduleCount;

        /// <summary>
        /// module正在做的索引
        /// </summary>
        private int moduleIndex = 1;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 传输对象
        /// </summary>
        private TransportUnit transportUnit;

        /// <summary>
        /// 传输单元对象
        /// </summary>
        private GeneralCoordinateSystem coordinateSystem;

        /// <summary>
        /// 配置对象
        /// </summary>
        private ModuleConfig ModuleConfig =>
            ProductConfiguration.GetInstance().ModuleConfig;

        /// <summary>
        /// 距离开始点的距离
        /// </summary>
        private double distanceStartX = 0;


        /// <summary>
        /// 距离开始点的距离
        /// </summary>
        private double distanceStartY = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmModulePositionTeach()
        {
            this.InitializeComponent();
        }

        #endregion


        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            string message1, message2, message3, message4, message5, message6, message8;

            // 原点位置
            AKRSPoint3D originPos1 = new();
            AKRSPoint3D originPos2 = new();
            AKRSPoint3D originPos3 = new();
            AKRSPoint3D originPos4 = new();

            // 开始点
            AKRSPoint3D startPos = new();

            // 行末点
            AKRSPoint3D rowEndPos = new();

            // 列末点
            AKRSPoint3D columnEndPos = new();

            if (this.ModuleConfig.Multiplication == MultiplicationEnum.Matrix)
            {
                message1 = $"步骤 1/{stepCount}:找原点1\r\n"
                                + $"移动固晶相机去找到此基板第一个基岛的中心（角落），设备将设置该点为基岛坐标系原点\r\n"
                                + $"如果点击完成，设备将相机现在所看的点设置为基岛坐标系原点（基岛中心点）\r\n"
                                + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

                message2 = $"步骤 2/{stepCount}:找原点2\r\n"
                                + $"移动固晶相机去找到此基板第一个基岛的中心（角落），设备将设置该点为基岛坐标系原点\r\n"
                                + $"如果点击完成，设备计算基岛坐标系原点（基岛中心点）\r\n"
                                + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

                message3 = $"步骤 3/{stepCount}:找原点3\r\n"
                                + $"移动固晶相机去找到此基板第一个基岛的中心（角落），设备将设置该点为基岛坐标系原点\r\n"
                                + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

                message4 = $"步骤 4/{stepCount}:找原点4\r\n"
                                + $"移动固晶相机去找到此基板第一个基岛的中心（角落），设备将设置该点为基岛坐标系原点\r\n"
                                + $"点击完成，设备将计算基岛坐标系原点（基岛中心点）\r\n";

                message5 = $"Step 5/{this.stepCount}; 计算基岛的行列间距.\r\n"
                                + "找到此基板(左下角)第一个基岛的某一个特征点";

                message6 = $"Step 6/{this.stepCount}; 计算基岛的行列间距 \n"
                                + " 找到此基板(右下角)同一行最后一个基岛的同一个特征点";

                message8 = $"Step 8/{this.stepCount}; 计算基岛的行列间距 \r\n"
                                + " 找到此基板(右上角)最后一个基岛的同一个特征点";
            }
            else
            {
                message1 = $"步骤 1/{stepCount}:找原点1\r\n"
                                + $"移动固晶相机去找到此基板第一个基岛的中心（角落），设备将设置该点为基岛坐标系原点\r\n"
                                + $"如果点击完成，设备将相机现在所看的点设置为基岛坐标系原点（基岛中心点）\r\n"
                                + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

                message2 = $"步骤 2/{stepCount}:找原点2\r\n"
                                + $"移动固晶相机去找到此基板第一个基岛的中心（角落），设备将设置该点为基岛坐标系原点\r\n"
                                + $"如果点击完成，设备计算基岛坐标系原点（基岛中心点）\r\n"
                                + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

                message3 = $"步骤 3/{stepCount}:找原点3\r\n"
                                + $"移动固晶相机去找到此基板第一个基岛的中心（角落），设备将设置该点为基岛坐标系原点\r\n"
                                + $"如果点击下一步，设备将保存该位置，与后续步骤一同计算";

                message4 = $"步骤 4/{stepCount}:找原点4\r\n"
                                + $"移动固晶相机去找到此基板第一个基岛的中心（角落），设备将设置该点为基岛坐标系原点\r\n"
                                + $"点击完成，设备将计算基岛坐标系原点（基岛中心点）\r\n";

                message5 = string.Empty;
                message6 = string.Empty;
                message8 = string.Empty;
            }

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 移动到安全位置

                    // 拉原点1
                    new AssistantConfig(
                        index: 0,
                        descritpion: message1,
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiRotaryPoint2;

                            originPos1 = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                        },
                       doneAction: () =>
                       {
                           this.realOriginPos[this.moduleIndex - 1] = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                           this.IsAssistanceContinue();
                       }),
                 
                    // 拉原点2
                    new AssistantConfig(
                        index: 1,
                        descritpion: message2,
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiRotaryPoint1;
                        },
                        nextAction: () =>
                        {
                            originPos2 = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                        },
                        doneAction: () =>
                        {
                            originPos2 = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                            this.realOriginPos[this.moduleIndex - 1] = (originPos1 + originPos2) / 2;
                            this.IsAssistanceContinue();
                        }),

                    // 拉原点3
                    new AssistantConfig(
                        index: 2,
                        descritpion: message3,
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiRotaryPoint2;
                        },
                        nextAction: () =>
                        {
                            originPos3 = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                        },
                        doneAction: () =>
                        {
                            originPos3 = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                            this.realOriginPos[this.moduleIndex - 1] = (originPos1 + originPos2 + originPos3) / 3;
                            this.IsAssistanceContinue();
                        }),

                    // 拉原点4
                    new AssistantConfig(
                        index: 3,
                        descritpion: message4,
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
                            originPos4 = TUAssistantHelper.GetPosBySystem(coordinateSystem);
                            this.realOriginPos[this.moduleIndex - 1] = (originPos1 + originPos2 + originPos3 + originPos4) / 4;
                            this.IsAssistanceContinue();
                        }),

                    // 找到第一个基板
                    new AssistantConfig(
                        index: 4,
                        descritpion: message5,
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: !this.ModuleConfig.IsAssistanceDistanceByInput,
                        isShowDone: this.ModuleConfig.IsAssistanceDistanceByInput,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                startPos = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                            },
                        doneAction: () =>
                            {
                                this.Hide();
                                FrmInputMatrixInfo frmInputMatrixInfo = new FrmInputMatrixInfo(this.ModuleConfig);
                                frmInputMatrixInfo.ShowDialog();
                                this.distanceStartX = frmInputMatrixInfo.DistanceToStartX;
                                this.distanceStartY = frmInputMatrixInfo.DistanceToStartY;
                                this.Save();
                            }),

                    // 移到第一行和最后一列
                    new AssistantConfig(
                        index: 5,
                        descritpion: message6,
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            columnEndPos = TUAssistantHelper.GetPosBySystem(coordinateSystem);

                            RetryCommand:
                            // 输入列数
                            if (!int.TryParse(XtraInputBox.Show("请输入一个基板中基岛一共有多少列", "列数", "2"), out this.columns))
                            {
                                AKRSXtraMessageBox.Show("输入参数错误，请重新输入");
                                goto RetryCommand;
                            }

                            // 非负判断
                            this.columns = Math.Abs(this.columns);

                            if (this.columns == 0 || this.columns == 1)
                            {
                                this.columnSpacing = 0;
                            }
                            else
                            {
                                // 计算列间距
                                columnSpacing = (columnEndPos.X - startPos.X) / (this.columns - 1);

                                // 计算列间距
                                columnSpacingY = (columnEndPos.Y - startPos.Y) / (this.columns - 1);
                            }

                            this.stepIndex++;
                        },
                        doneAction: () =>
                        {
                        }),


                    // 移到第一行和最后一列
                    new AssistantConfig(
                        index: 6,
                        descritpion: message5,
                        isShowTitle: true,
                        isShowBack: false,
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

                    // 移到最后一行和最后一列
                    new AssistantConfig(
                        index: 7,
                        descritpion: message8,
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiColumnEndPoint;
                            this.stepIndex--;
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiRowCount;
                            rowEndPos = TUAssistantHelper.GetPosBySystem(coordinateSystem);

                            RetryCommand:
                            // 输入列数
                            if (!int.TryParse(XtraInputBox.Show("请输入一个基板中基岛一共有多少行", "行数", "2"), out this.rows))
                            {
                                AKRSXtraMessageBox.Show("输入参数错误，请重新输入");
                                goto RetryCommand;
                            }

                            // 非负判断
                            this.rows = Math.Abs(this.rows);

                            if (this.rows == 0 || this.rows == 1)
                            {
                                this.rowSpacing = 0;
                            }
                            else
                            {
                                // 计算行间距
                                this.rowSpacing = (rowEndPos.Y - columnEndPos.Y) / (this.rows - 1);

                                // 计算行间距
                                this.rowSpacingX = (rowEndPos.X - columnEndPos.X) / (this.rows - 1);
                            }

                            this.Save();
                        }),
                };

            ucGuideMove = new UcGuideMove("FrmModulePositionTeach");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl(0);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmModulePositionTeach_Load(object sender, EventArgs e)
        {
            this.transportUnit = TUAssistantHelper.JudgeTuExist();
            if (this.transportUnit == null)
            {
                this.Close();
                return;
            }

            // 是否开启基板示教
            if (!ProductConfiguration.GetInstance().ModuleConfig.IsMultiple)
            {
                AKRSXtraMessageBox.Show("请选择多个基岛");
                this.Close();
                return;
            }

            if (ProductConfiguration.GetInstance().ModuleConfig.Multiplication == MultiplicationEnum.Configurable)
            { 
                RetryCommand:
                // 输入列数
                if (!int.TryParse(XtraInputBox.Show("请输入基岛一共有多少个", "基岛个数", "2"), out this.moduleCount))
                {
                    AKRSXtraMessageBox.Show("输入参数错误，请重新输入");
                    goto RetryCommand;
                }

                this.stepCount = 4;

                while (this.tileBarGroup4.Items.Count > 4)
                {
                    this.tileBarGroup4.Items.RemoveAt(4);
                }
               
                for (int i = 0; i < this.moduleCount; i++)
                {
                    this.realOriginPos.Add(new AKRSPoint3D());
                }
            }
            else
            {
                this.realOriginPos.Add(new AKRSPoint3D());
            }

            FrmModuleSelect fmrModuleSelect = new FrmModuleSelect(EntityTypeEnum.Substrate);

            if (fmrModuleSelect.ShowDialog() == DialogResult.OK)
            {
                this.coordinateSystem = fmrModuleSelect.CurrentSubstrate.CoordinateSystem;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }


            // 测高
            FrmAssistanceHeight frmAssistanceHeight = new FrmAssistanceHeight(this.coordinateSystem, "Module");

            if (frmAssistanceHeight.ShowDialog() == DialogResult.OK)
            {
                this.realHeight = frmAssistanceHeight.MatterHeight;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            this.InitControl();
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[stepIndex];
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            if (!this.ModuleConfig.IsAssistanceDistanceByInput)
            {
                ProductConfiguration.GetInstance().ModuleConfig.ColumnCount =
                    this.columns;
                ProductConfiguration.GetInstance().ModuleConfig.RowCount = this.rows;

                ProductConfiguration.GetInstance().ModuleConfig.ColumnSpacing =
                    this.columnSpacing;

                ProductConfiguration.GetInstance().ModuleConfig.RowSpacing =
                    this.rowSpacing;
            }
            
            this.ModuleConfig.ElementCoordinates = new List<ElementCoordinate>();

            this.ModuleConfig.ElementCoordinates.Clear();

            if (this.ModuleConfig.Multiplication == MultiplicationEnum.Matrix)
            {
                this.ModuleConfig.Count = this.ModuleConfig.ColumnCount * this.ModuleConfig.RowCount;

                for (int i = 0; i < this.ModuleConfig.RowCount; i++)
                {
                    for (int j = 0; j < this.ModuleConfig.ColumnCount; j++)
                    {
                        ElementCoordinate elementCoordinate = new ElementCoordinate();
                        elementCoordinate.Point = new AKRSPoint3D(
                            this.realOriginPos[0].X + this.distanceStartX + this.ModuleConfig.ColumnSpacing * j + this.rowSpacingX * i,
                            this.realOriginPos[0].Y + this.distanceStartY + this.ModuleConfig.RowSpacing * i + this.columnSpacingY * j,
                            this.realHeight);
                        elementCoordinate.Degree = 0;
                        this.ModuleConfig.ElementCoordinates.Add(elementCoordinate);
                    }
                }
            }
            else
            {
                this.ModuleConfig.Count = this.realOriginPos.Count;

                for (int i = 0; i < this.realOriginPos.Count; i++)
                {
                    ElementCoordinate elementCoordinate = new ElementCoordinate() { Point = this.realOriginPos[i] };

                    this.ModuleConfig.ElementCoordinates.Add(elementCoordinate);
                }
            }


            ProductConfiguration.GetInstance().TransportUnitConfig.ModulePosition.State = AssistantStateEnum.Able;
            ProductConfiguration.GetInstance().Save();
            this.DialogResult = DialogResult.OK;

            this.Close();
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
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TbiStartPoint_ItemClick(object sender, TileItemEventArgs e)
        {

        }

        /// <summary>
        /// 自动测高
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtAutoFocus_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.AutoFocus(sender, this);
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
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmModulePositionTeach_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose(); 
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
                TUAssistantHelper.MoveToPos(
                    result.point - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset);
            }
            else
            {
                AKRSXtraMessageBox.Show("定位失败，请重置制作模板");
            }
        }
        private void TbiRowCount_ItemClick(object sender, TileItemEventArgs e)
        {

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

            if (this.stepIndex != 8)
            {
                this.SetUiControl(this.stepIndex);
            }
        }

        /// <summary>
        /// 是否继续示教，这个一般针对于异性基板
        /// </summary>
        private void IsAssistanceContinue()
        {
            if (ProductConfiguration.GetInstance().ModuleConfig.Multiplication == MultiplicationEnum.Configurable)
            {
                if (this.moduleIndex < this.moduleCount)
                {
                    this.stepIndex = -1;
                    this.moduleIndex++;
                    this.Hide();
                    this.InitControl();
                    this.SetUiControl(0);
                    this.Show();
                }
                else
                {
                    this.Save();
                    this.Close();
                }
            }
            else
            {
                this.stepIndex = 3;
            }
        }
    }
}