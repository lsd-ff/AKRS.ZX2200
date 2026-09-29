using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    /// <summary>
    /// Bond标定示教窗体
    /// </summary>
    public partial class FrmBondCalibTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 晶圆台模组控制器
        /// </summary>
        private WaferTableController waferTableController = new WaferTableController();

        /// <summary>
        /// 运行参数
        /// </summary>
        private CalibrateRunPara CalibrateRunPara => CalibrateRunPara.GetInstance();

        /// <summary>
        /// 运行方法
        /// </summary>
        private CalibrateTask CalibrateTask => CalibrateTask.GetInstance();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 上次bmc点高度
        /// </summary>
        private double bmcVisionPosZ = 0;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 11;

        /// <summary>
        /// 是否使用上次点位
        /// </summary>
        private bool IsUsePos = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmBondCalibTeach"/> class.
        /// </summary>
        public FrmBondCalibTeach(bool isUsePos = false)
        {
            this.InitializeComponent();
            this.TileBarTeach.SelectedItem = this.TbiBmcCenter;
            this.InitControl();
            this.SetUiControl(this.stepIndex);
            this.IsUsePos = isUsePos;

            if (CalibrateRunPara.BMCVisionMachinePos != null && this.IsUsePos == true)
            {
                this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.BMCVisionMachinePos);
            }

            //  this.ucGuideMove2.ChangeModuleName("Bond Module");
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
            this.Close();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitControl()
        {
            this.stepCount = 11;
            this.BtnDone.Visible = false;

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 相机BMC中心
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/{stepCount} :将Bond相机中心对准BMC中心点位置.\r\n",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                if (CalibrateRunPara.BMCVisionMachinePos.Z != null)
                                {
                                     bmcVisionPosZ = CalibrateRunPara.BMCVisionMachinePos.Z;
                                }

                            CalibrateRunPara.BMCVisionMachinePos = this.bondModuleController.Get3DRealPosition();
                            CalibrateRunPara.Save();

                            CalibrateRunPara.waferCameraOffsetZ =
                                CalibrateRunPara.BMCVisionMachinePos.Z - bmcVisionPosZ;

                            if (CalibrateRunPara.BMCMeasureHeightSearchMachinePos != null && this.IsUsePos == true)
                            {
                                this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .BMCMeasureHeightSearchMachinePos.X,CalibrateRunPara
                                    .BMCMeasureHeightSearchMachinePos.Y);
                            }

                            this.TileBarTeach.SelectedItem = this.TbiToolCenter;
                        },
                       doneAction: () =>
                       {
                       }),
                 
                    // 吸嘴BMC中心
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"2/{stepCount}：将Touchdown中心置于BMC中心点上方3mm(以下)位置.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            CalibrateRunPara.BMCMeasureHeightSearchMachinePos = this.bondModuleController.Get3DRealPosition();
                            CalibrateRunPara.BondRotateCenterToCamOffset = new AKRSPoint3D();
                            CalibrateRunPara.BondRotateCenterToCamOffset.X = CalibrateRunPara.BMCMeasureHeightSearchMachinePos.X - CalibrateRunPara.BMCVisionMachinePos.X;
                            CalibrateRunPara.BondRotateCenterToCamOffset.Y = CalibrateRunPara.BMCMeasureHeightSearchMachinePos.Y - CalibrateRunPara.BMCVisionMachinePos.Y;
                            CalibrateRunPara.Save();

                            CalibrateTask.StartBondCalibTask();
                            CalibrateRunPara.Save();

                            if (CalibrateRunPara.GlassVisionMachinePos != null && this.IsUsePos == true)
                            {
                                this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .GlassVisionMachinePos.X,CalibrateRunPara
                                    .GlassVisionMachinePos.Y);

                            }

                            this.TileBarTeach.SelectedItem = this.TbiGlassCenter;

                        },
                        doneAction: () =>
                        {
                        }), 

                    // 玻璃片中心
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"3/{stepCount}：将Bond相机中心对准玻璃标定片中心点位置.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            CalibrateRunPara.GlassVisionMachinePos = this.bondModuleController.Get3DRealPosition();
                            CalibrateRunPara.Save();

                            this.bondHeadController.CloseBondHeadVaccum();

                            DialogResult dialog = AKRSXtraMessageBox.Show(
                                "System2: \n"
                                + "请移除Touchdown吸嘴并更换BMC吸嘴.\r\n",
                                "Prompt",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Information);

                            if (dialog == DialogResult.OK)
                            {
                                CalibrateTask.StartBondToCamCalibTask1();

                                if (CalibrateRunPara.GlassUpLookVisionMachinePos != null && this.IsUsePos == true)
                                {
                                    this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .GlassUpLookVisionMachinePos.X,CalibrateRunPara
                                    .GlassUpLookVisionMachinePos.Y);
                                    // this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GlassUpLookVisionMachinePos);
                                }

                                this.TileBarTeach.SelectedItem = this.TbiUpLookCenter;
                            }
                            else
                            {
                                this.stepIndex--;
                            }

                        },
                        doneAction: () =>
                        {
                        }),

                    // 标定点上视中心
                    new AssistantConfig(
                        index: 3,
                        descritpion: $"4/{stepCount} ： 将标定片中心置于上视相机中心位置.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                CalibrateRunPara.GlassUpLookVisionMachinePos = this.bondModuleController.Get3DRealPosition();
                                CalibrateRunPara.Save();

                                CalibrateTask.StartUpLookCalibTask();

                                DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                    "系统2: \n" + "移除BMC吸嘴并将Touchdown安装至Bond上.\r\n",
                                    "Prompt",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Information);

                                if (dialog1 == DialogResult.OK)
                                {
                                    if (CalibrateRunPara.BondCamUpLookCamMachinePos != null && this.IsUsePos == true)
                                    {
                                        this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .BondCamUpLookCamMachinePos.X,CalibrateRunPara
                                    .BondCamUpLookCamMachinePos.Y);
                                        // this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.BondCamUpLookCamMachinePos);
                                    }

                                    this.TileBarTeach.SelectedItem = this.TbiUpLookCenter;
                                }
                                else
                                {
                                    this.stepIndex--;
                                }
                            },
                        doneAction: () =>
                            {
                            }),

                    // 三点一线
                    new AssistantConfig(
                        index: 4,
                        descritpion: $"5/{stepCount} :将标定片放置于Bond相机和上视相机之间，两相机中心对准标定片中心。（此步可弹框选择Cancel跳过）\r\n",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                AKRSPoint3D curBondPos = this.bondModuleController.Get3DRealPosition();
                                CalibrateRunPara.GetInstance().BondCamUpLookCamMachinePos = curBondPos;

                                DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                    "系统2: \n" + "是否放置标定片执行对准 .\r\n",
                                    "Prompt",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Information);
                                if (dialog1 == DialogResult.OK)
                                {
                                    CalibrateTask.GetInstance().StartBondToCamBySameMarkTask();
                                }

                                CalibrateRunPara.GetInstance().Save();

                                if (CalibrateRunPara.UpLookMarkMachinePos != null && this.IsUsePos == true)
                                {
                                    this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .UpLookMarkMachinePos.X,CalibrateRunPara
                                    .UpLookMarkMachinePos.Y);
                                    // this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.UpLookMarkMachinePos);
                                }

                                this.TileBarTeach.SelectedItem = this.TbiUplookMarkCenter;
                            },
                        doneAction: () =>
                            {
                            }),

                    // 上视位置
                    new AssistantConfig(
                        index: 5,
                        descritpion: $"6/{stepCount} ：将Bond相机中心对准上视相机旁温漂标定点中心位置.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                CalibrateRunPara.UpLookMarkMachinePos = this.bondModuleController.Get3DRealPosition();

                                this.CalibrateTask.StartUpLookMachinePosTask();

                                //// 标定力
                                // CalibrateRunPara.WaferTableReadyMachinePos = new AKRSPoint3D(54.7070, 0.0540, 0);

                                if (CalibrateRunPara.WaferTableReadyMachinePos != null && this.IsUsePos == true)
                                {
                                    this.calibController.MoveWaferTableToMachinePos(CalibrateRunPara.WaferTableReadyMachinePos);
                                }
                                                        
                                // this.waferTableModule.MoveXY(CalibrateRunPara.WaferTableReadyPosition);
                                if (CalibrateRunPara.WaferTableLeftMarkBCVisionMachinePos != null && this.IsUsePos == true)
                                {
                                    this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .WaferTableLeftMarkBCVisionMachinePos.X,CalibrateRunPara
                                    .WaferTableLeftMarkBCVisionMachinePos.Y);
                                    // this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.WaferTableLeftMarkBCVisionMachinePos);
                                }

                                CalibrateRunPara.Save();
                                this.TileBarTeach.SelectedItem = this.TbiLeftMarkCenter;
                            },
                        doneAction: () =>
                            {
                            }),

                    // bond相机晶圆台左点
                    new AssistantConfig(
                        index: 6,
                        descritpion: $"7/{stepCount} ：将Bond相机中心对准晶圆台左标定点中心位置.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                CalibrateRunPara.WaferTableLeftMarkBCVisionMachinePos = this.bondModuleController.Get3DRealPosition();
                                CalibrateRunPara.WaferTableReadyMachinePos = this.calibController.GetWaferTableRealPos();
                                CalibrateRunPara.Save();

                                if (CalibrateRunPara.WaferTableRightMarkVisionMachinePos != null && this.IsUsePos == true)
                                {
                                    this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .WaferTableRightMarkVisionMachinePos.X,CalibrateRunPara
                                    .WaferTableRightMarkVisionMachinePos.Y);
                                    // this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.WaferTableRightMarkVisionMachinePos);
                                }

                                this.TileBarTeach.SelectedItem = this.TbiRightMarkCenter;
                            },
                        doneAction: () =>
                            {
                            }),

                    // bond相机晶圆台右点
                    new AssistantConfig(
                        index: 7,
                        descritpion: $"8/{stepCount} ：将Bond相机中心对准晶圆台右标定点中心位置（晶圆台不进行移动）.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                CalibrateRunPara.WaferTableRightMarkVisionMachinePos = this.bondModuleController.Get3DRealPosition();
                                CalibrateRunPara.Save();

                                CalibrateTask.StartWaferTableCalibTask();

                                this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.BMCVisionMachinePos);

                                // this.waferTableModule.MoveXY(CalibrateRunPara.WaferTableLeftMarkWCVisionMachinePos);
                                if (CalibrateRunPara.WaferTableLeftMarkWCVisionMachinePos != null && this.IsUsePos == true)
                                {
                                    // this.waferTableController.MoveWaferTableToMachinePos(CalibrateRunPara.WaferTableLeftMarkWCVisionMachinePos);
                                    this.calibController.MoveWaferTableToMachinePos(CalibrateRunPara.WaferTableLeftMarkWCVisionMachinePos);
                                    this.calibController.MoveWaferCameraZ(this.CalibrateRunPara.WaferTableLeftMarkWCVisionMachinePos.Z);
                                }
                               
                                //// this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.WaferTableLeftMarkWCVisionMachinePos);
                                
                                this.TileBarTeach.SelectedItem = this.TbiWaferCenter;
                            },
                        doneAction: () =>
                            {
                            }),

                    // 左点晶圆相机中心
                    new AssistantConfig(
                        index: 8,
                        descritpion: $"9/{stepCount} ： 将晶圆台左标定点中心置于晶圆相机中心位置.\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                CalibrateRunPara.WaferTableLeftMarkWCVisionMachinePos = this.calibController.GetWaferTableRealPos();
                                CalibrateRunPara.Save();

                                if (CalibrateRunPara.WaferTableMeasureHeightSearchMachinePos != null && this.IsUsePos == true)
                                {
                                    this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .WaferTableMeasureHeightSearchMachinePos.X,CalibrateRunPara
                                    .WaferTableMeasureHeightSearchMachinePos.Y);
                                    // this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.WaferTableMeasureHeightSearchMachinePos);
                                }

                                this.TileBarTeach.SelectedItem = this.TbiTouchdownLeftMark;
                            },
                        doneAction: () =>
                            {
                            }),

                    // 吸嘴左点上方
                    new AssistantConfig(
                        index: 9,
                        descritpion: $"10/{stepCount} ：将touchdown对准晶圆台左标定点中心上方5mm位置.（此步可跳过）\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {

                            },
                        nextAction: () =>
                            {
                                CalibrateRunPara.WaferTableMeasureHeightSearchMachinePos = this.bondModuleController.Get3DRealPosition();
                                CalibrateRunPara.Save();

                                if (CalibrateRunPara.WaferTableRightMarkBCVisionMachinePos != null && this.IsUsePos == true)
                                {
                                    this.bondHeadController.MoveBondZToSafePos();
                                this.bondModuleController.MoveBondXY(CalibrateRunPara
                                    .WaferTableRightMarkBCVisionMachinePos.X,CalibrateRunPara
                                    .WaferTableRightMarkBCVisionMachinePos.Y);
                                    // this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.WaferTableRightMarkBCVisionMachinePos);
                                }

                                this.TileBarTeach.SelectedItem = this.TbiBondRightMarkCenter;
                            },
                        doneAction: () =>
                        {
                        }),

                    // bond相机右点
                    new AssistantConfig(
                        index: 10,
                        descritpion: $"11/{stepCount} ：将Bond相机中心对准晶圆台右标定点中心位置（晶圆台不移动）.\r\n",
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
                                CalibrateRunPara.WaferTableRightMarkBCVisionMachinePos = this.bondModuleController.Get3DRealPosition();
                                CalibrateRunPara.Save();

                                CalibrateTask.StartWaferNPointCalibTask();

                                FrmCalibResult frmCalibResult = new FrmCalibResult();
                                frmCalibResult.CloseParentForm += this.MyUserControl_CloseParentForm;

                                frmCalibResult.Show();

                            }),
                };

            UcGuideMove ucGuideMove = new UcGuideMove("系统2标定");
            ucGuideMove.Dock = DockStyle.Fill;
            this.panelControl4.Controls.Add(ucGuideMove);

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
        /// 模版匹配界面
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnPattern_Click(object sender, EventArgs e)
        {
            string prName = string.Empty;
            switch (this.stepIndex)
            {
                case 0:
                    prName = this.CalibrateRunPara.BmcPRName;
                    break;
                case 2:
                    prName = this.CalibrateRunPara.GlassPRName;
                    break;
                case 3:
                    prName = this.CalibrateRunPara.UpLookPRName;
                    break;
                case 5:
                    prName = this.CalibrateRunPara.WaferLeftPRName;
                    break;
                case 6:
                    prName = this.CalibrateRunPara.WaferLeftPRName;
                    break;
                case 7:
                    prName = this.CalibrateRunPara.WaferRightPRName;
                    break;
                case 8:
                    prName = this.CalibrateRunPara.WaferPRName;
                    break;
                case 10:
                    prName = this.CalibrateRunPara.WaferRightPRName;
                    break;
            }

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
        /// 完成事件
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