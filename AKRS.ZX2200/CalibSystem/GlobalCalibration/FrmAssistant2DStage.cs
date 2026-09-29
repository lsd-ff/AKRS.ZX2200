using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using ch.etel.edi.dsa.v40;
    using DevExpress.Utils;

    /// <summary>
    /// 2DMapping示教流程
    /// </summary>
    public partial class FrmAssistant2DStage : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// 2DMapping示教流程
        /// </summary>
        public FrmAssistant2DStage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 全局标定对象
        /// </summary>
        private GlobalCalibrationDomain GlobalCalibration => GlobalCalibrationDomain.GetInstance();

        /// <summary>
        /// 固晶模组
        /// </summary>
        private readonly BondModule bondModule = new BondModule();

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

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
        /// 行列间距
        /// </summary>
        private int rowSpacing, columnSpacing;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 找到开始点
                    new AssistantConfig(
                        index: 0,
                        descritpion: "移动到标定开始点（标定片左下角落），并制作视觉模板",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                  MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                                    null,
                                    this.GlobalCalibration.PrName,
                                    this.GlobalCalibration.PrName);

                                if (result == null)
                                {
                                    AKRSXtraMessageBox.Show("Visual positioning failure , please edit Program retry");
                                    this.stepIndex--;
                                }

                                // 移动到中心点

                                this.startPos = this.bondModule.Get3DRealPosition();
                            },
                        doneAction: () =>
                        {
                        }),
                    
                    // 移到第一行和最后一列
                    new AssistantConfig(
                        index: 2,
                        descritpion: "移动到第一行最后一列",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // 移动到中心

                            this.columnEndPos = this.bondModule.Get3DRealPosition();

                            RetryCommand:
                            // 输入列数
                            if (!int.TryParse(XtraInputBox.Show("请输入列间距", "列间距", "2"), out this.columnSpacing))
                            {
                                AKRSXtraMessageBox.Show("参数错误，请重新输入");
                                goto RetryCommand;
                            }

                            if (columnSpacing == 0)
                            {
                                AKRSXtraMessageBox.Show("参数错误，请重新输入");
                                goto RetryCommand;
                            }

                            // 非负判断
                            this.columnSpacing = Math.Abs(this.columnSpacing);

                            this.columns = (int)((this.columnEndPos.X - this.startPos.X) / this.columnSpacing);

                            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                            {
                                if (this.columns < 3)
                                {
                                    AKRSXtraMessageBox.Show("计算出的列数小于3，请重新输入");
                                    this.stepIndex--;
                                    return;
                                }
                            }

                            this.stepIndex++;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 移到最后一行和最后一列
                    new AssistantConfig(
                        index: 4,
                        descritpion: "移动到最后一行最后一列",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiColumnEndSub;
                            this.stepIndex--;
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiRowCount;
                            this.rowEndPos = this.bondModule.Get3DRealPosition();

                            RetryCommand:
                            // 输入列数
                            if (!int.TryParse(XtraInputBox.Show("请输入行间距", "行间距", "2"), out this.rowSpacing))
                            {
                                AKRSXtraMessageBox.Show("参数错误，请重新输入");
                                goto RetryCommand;
                            }

                            if (this.rowSpacing == 0)
                            {
                                AKRSXtraMessageBox.Show("参数错误，请重新输入");
                                goto RetryCommand;
                            }

                            this.rows = (int)((this.rowEndPos.Y - this.startPos.X) / this.rowSpacing);

                            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                            {
                                if (this.rows < 3)
                                {
                                    AKRSXtraMessageBox.Show("计算出的行数小于3，请重新输入");
                                    this.stepIndex--;
                                    return;
                                }
                            }

                            this.Save();

                            this.DialogResult = DialogResult.OK;
                        }),
                };

            ucGuideMove = new UcGuideMove("FrmAssistant2DStage");
            CommonHelper.ChangeUcMove(ucGuideMove, CurrentMachineSystemEnum.System2);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl();

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void Frm_Load(object sender, EventArgs e)
        {
            // 所有轴回原点
            Machine.GetInstance().AllAxisGoHome();

            // 关闭补偿
            Machine.GetInstance().CloseCompensate();

            // 初始化硬件
            this.InitControl();
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        private void SetUiControl()
        {
            this.TileBarTeach.SelectedItem = this.tileBarGroup2.Items[this.stepIndex];
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];

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
            this.GlobalCalibration.ColumnSpacing = this.columnSpacing;
            this.GlobalCalibration.RowSpacing = this.rowSpacing;
            this.GlobalCalibration.StartPoint3D = this.startPos;
            this.GlobalCalibration.ColumnCount = this.rows;
            this.GlobalCalibration.RowCount = this.rows;
            this.GlobalCalibration.Save();
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
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 制作模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmEditPr_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.GlobalCalibration.PrName);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(this.GlobalCalibration.PrName);
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Calibration;
                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            prEntity.SetHardware(System2Domain.GetInstance().System2Controller.GetHardware(CameraTypeEnum.BondCamera));

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmAssistant2DStage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtAutoFocus_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.AutoFocus(sender, this);
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
        }
    }
}