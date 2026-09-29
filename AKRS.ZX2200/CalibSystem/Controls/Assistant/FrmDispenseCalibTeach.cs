using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.DispenseSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Modules;
using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using System.Windows.Forms;

    /// <summary>
    /// 点胶示教窗体
    /// </summary>
    public partial class FrmDispenseCalibTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// PR界面
        /// </summary>
        private FrmPRList frmPRList;

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        private DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController dispenseController = new DispenseController();

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 运行参数实例
        /// </summary>
        private CalibrateRunPara CalibrateRunPara => CalibrateRunPara.GetInstance();

        /// <summary>
        /// 标定方法对象
        /// </summary>
        private CalibrateTask CalibrateTask => CalibrateTask.GetInstance();

        /// <summary>
        /// 点胶测高模组
        /// </summary>
        private readonly MeasureHeightModule measureHeightModule = new MeasureHeightModule();

        /// <summary>
        /// 针孔橡皮泥测高位置
        /// </summary>
        private AKRSPoint3D pinholePos;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 0;

        /// <summary>
        /// 是否使用上次点位
        /// </summary>
        private bool IsUsePos = false;

        //// private UcProgressStep UcProgressStep = new UcProgressStep(2) { Dock = DockStyle.Fill };

        /// <summary>
        /// 窗体构造函数
        /// </summary>
        public FrmDispenseCalibTeach(bool isUsePos = false)
        {
            this.InitializeComponent();
            this.panel1.AutoScroll = true;

            this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();
            this.TileBarTeach.SelectedItem = this.TbiDispenseCenter;
            this.InitControl();
            this.SetUiControl(this.stepIndex);
            CalibrateRunPara.Load();
            this.IsUsePos = isUsePos;

            if (CalibrateRunPara.DispenseMarkVisionMachinePos != null && this.IsUsePos == true)
            {
                this.calibController.MoveDispenseToMachinePos(CalibrateRunPara.DispenseMarkVisionMachinePos);
            }
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUiControl(this.stepIndex);
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUiControl(this.stepIndex);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this?.Close();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitControl()
        {
            this.stepCount = 4;
            this.BtnDone.Visible = false;

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/{stepCount} :将点胶相机中心置于点胶标记点中心位置.\r\n",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                CalibrateRunPara.DispenseMarkVisionMachinePos = this.dispenseController.GetAxisPos();
                                this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();


                                if (CalibrateRunPara.DispenseMeasureHeightSearchMachinePos != null && this.IsUsePos == true)
                                {
                                   this.calibController.MoveDispenseToMachinePos(CalibrateRunPara.DispenseMeasureHeightSearchMachinePos);
                                }
                                this.calibController.MoveDispenseZToSafePos();

                               this.dispenseMeasureHeightController.OpenDispenseHeightMeasurementCylinder();

                                // CalibrateRunPara.DispensePRName = this.frmPRList.VisionEntityName;

                                this.TileBarTeach.SelectedItem = this.TbiToolCenter;
                            },
                       doneAction: () =>
                       {
                       }),

                    new AssistantConfig(
                        index: 1,
                        descritpion: $"2/{stepCount}：将测高针中心置于点胶标定点中心上方5mm位置（此步做完可直接点击done完成标定）.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            CalibrateRunPara.DispenseMeasureHeightSearchMachinePos = this.dispenseController.GetAxisPos();

                            CalibrateRunPara.Save();
                        },
                        doneAction: () =>
                        {
                            CalibrateRunPara.DispenseMeasureHeightSearchMachinePos = this.dispenseController.GetAxisPos();

                            CalibrateRunPara.DispenseCameraToPinOffset = new AKRSPoint3D
                                                                       {
                                                                           X = CalibrateRunPara
                                                                                   .DispenseMeasureHeightSearchMachinePos.X
                                                                               - CalibrateRunPara
                                                                                   .DispenseMarkVisionMachinePos.X,
                                                                           Y = CalibrateRunPara
                                                                                   .DispenseMeasureHeightSearchMachinePos.Y
                                                                               - CalibrateRunPara
                                                                                   .DispenseMarkVisionMachinePos.Y,
                                                                           Z = 0
                                                                       };

                            CalibrateRunPara.Save();

                            CalibrateTask.StartDispenseCalibTask();
                                                          FrmCalibResult frmCalibResult = new FrmCalibResult();
                                frmCalibResult.CloseParentForm += this.MyUserControl_CloseParentForm;
                                frmCalibResult.ShowDialog();
                                frmCalibResult.Dispose();
                        }),

                    // 测高针橡皮泥点一点
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"3/{stepCount}：将测高针中心置于橡皮泥上方5mm位置.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.dispenseMeasureHeightController.OpenDispenseHeightMeasurementCylinder();

                            (ExcuteResult result, double height) = this.dispenseMeasureHeightController.HeightMeasurement(this.measureHeightModule.DispenseAltimemetrySensor);

                            if (result != ExcuteResult.Success)
                            {
                                AKRSXtraMessageBox.Show("测高失败！");
                            }

                            this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();

                            pinholePos = this.dispenseController.GetAxisPos();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 相机中心对点
                    new AssistantConfig(
                        index: 3,
                        descritpion: $"4/{stepCount}：将点胶相机中心置于橡皮泥孔位中心位置.\r\n",
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
                                AKRSPoint3D cameraCenter = this.dispenseController.GetAxisPos();
                                CalibrateRunPara.DispenseCameraToPinOffset = new AKRSPoint3D
                                                                             {
                                                                                 X = pinholePos.X
                                                                                     - cameraCenter.X,
                                                                                 Y = pinholePos.Y
                                                                                     - cameraCenter.Y,
                                                                                 Z = 0
                                                                             };
                                CalibrateTask.StartDispenseCalibTask();

                                FrmCalibResult frmCalibResult = new FrmCalibResult();
                                frmCalibResult.CloseParentForm += this.MyUserControl_CloseParentForm;
                                frmCalibResult.ShowDialog();
                                frmCalibResult.Dispose();

                            }),
                };
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
            UcGuideMove ucGuideMove = new UcGuideMove("点胶标定");
            ucGuideMove.ChangeModuleName("点胶模组");
            ucGuideMove.Dock = DockStyle.Fill;
            this.panelControl5.Controls.Add(ucGuideMove);
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtnBack.Visible = assistantConfig.IsShowBack;
            this.BtnNext.Visible = assistantConfig.IsShowNext;
            this.BtnDone.Visible = assistantConfig.IsShowDone;
        }

        /// <summary>
        /// 模版匹配
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnPattern_Click(object sender, EventArgs e)
        {
            string prName = CalibrateRunPara.GetInstance().DispensePRName;
            this.EditPr(prName);
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="algBeLong">模板类型</param>
        public void EditPr(string name, AlgBeLongEnum algBeLong = AlgBeLongEnum.Calibration)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);
                prEntity.Alg.AlgBeLong = algBeLong;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.ShowDialog();
            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void MyUserControl_CloseParentForm(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 完成按钮
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
        }
    }
}