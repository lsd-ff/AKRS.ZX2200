using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    /// <summary>
    /// 示教吸嘴清洁位置
    /// </summary>
    public partial class FrmNozzleCleanPosTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmNozzleCleanPosTeach()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 0;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 判断当前是否在运动，在运动的时候不能取消示教
        /// </summary>
        private bool isMove;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 轴是否准备好
        /// </summary>
        private bool isReady;


        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmNozzleCleanPosTeach");

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.PnlControl.Controls.Add(ucGuideMove1);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    new AssistantConfig(
                        index: 0,
                        descritpion: "Step1/3:将touchdown移动到左上角位置。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiTeachRightLowPos;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                BondDevicePara.GetInstance().BondHeadParam.NozzleCleanTableLeftTopPos = this.bondModuleController.GetG0RealPosition()/*+BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset*/;
                            },
                       doneAction: () =>
                       {
                       }),

                    new AssistantConfig(
                        index: 1,
                        descritpion: "Step2/3:将touchdown移动到右下角位置。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiTeachLeftTopPos;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiMeasureHeight;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                BondDevicePara.GetInstance().BondHeadParam.NozzleCleanTableRightBottomPos = this.bondModuleController.GetG0RealPosition()/*+BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset*/;
                            },
                        doneAction: () =>
                            {
                            }),

                    new AssistantConfig(
                        index: 2,
                        descritpion: "Step3/3:将touchdown移动到吸嘴清洁台中心正上方5mm,点击“确定”进行测高。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiTeachRightLowPos;
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                        {
                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            //  // 计算中心
                            //  AKRSPoint3D centerPos =
                            //      (BondDevicePara.GetInstance().BondHeadParam.NozzleCleanTableLeftTopPos + BondDevicePara
                            //           .GetInstance().BondHeadParam.NozzleCleanTableRightBottomPos) / 2 + BondDevicePara
                            //          .GetInstance().BondHeadParam.HeadToCameraOffset;

                            ////  运动到测高位
                            //// todo:这里吸嘴可能会碰到
                            //this.bondModuleController.MoveToG0Pos(centerPos);

          
                            // 执行测高
                            (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                                BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                                HeightMeasurementFunctionEnum.WithTDSensor);

                            if (res.Ret == ExcuteResult.Success)
                            {   
                                // 在G0中的高度
                                BondDevicePara.GetInstance().BondHeadParam.NozzleCleanTableRightBottomPos.Z =
                                    BondDevicePara.GetInstance().BondHeadParam.NozzleCleanTableLeftTopPos.Z =
                                        this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);
                            ReInput:
                                BondProgram.GetInstance().CleanNozzleProgram.Column  = XtraInputBox.Show<int>("请输入列数！","列", 5);
                                BondProgram.GetInstance().CleanNozzleProgram.Row  = XtraInputBox.Show<int>("请输入行数！","行", 5);

                                if(BondProgram.GetInstance().CleanNozzleProgram.Column < 1 || BondProgram.GetInstance().CleanNozzleProgram.Row < 1)
                                {
                                   AKRSXtraMessageBox.Show("行列数必须大于1");
                                    goto ReInput;
                                }

                                // 保存
                                BondDevicePara.GetInstance().Save();
                                BondProgram.GetInstance().Save();


                                this.DialogResult = DialogResult.OK;
                            }
                        }),
                };


            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiTeachLeftTopPos;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);
        }


        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUIControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
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
            TUAssistantHelper.SetColor(this.TileBarTeach);
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
            BondDevicePara.GetInstance().Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"End assistant?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleCleanPosTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.system2Controller.PutbackNozzleAssitance();
        }
    }
}
