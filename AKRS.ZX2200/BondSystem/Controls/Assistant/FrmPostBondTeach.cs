using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Controls.Setting.PostBond;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.DispenseSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using System.Threading.Tasks;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem;
    using LanguageExt;
    using LanguageExt.Pipes;

    /// <summary>
    /// 焊后检测示教窗体
    /// </summary>
    public partial class FrmPostBondTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 点胶视觉模块控制器
        /// </summary>
        private DispenseVisionController dispenseVisionController = new DispenseVisionController();

        /// <summary>
        ///  焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 0;

        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="postBondInspection">传入的对象</param>
        public FrmPostBondTeach(PostBondInspection postBondInspection, GeneralCoordinateSystem generalCoordinateSystem)
        {
            this.InitializeComponent();

            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.postBondInspection = postBondInspection;
            this.CoordinateSystem = generalCoordinateSystem;

            // 获取当前Module的G0坐标
            if (postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.AfterBondCheck)
            {
                switch (postBondInspection.MeasurePointNumber)
                {
                    case MeasurePointNumberEnum.OnePoint:
                        switch (postBondInspection.PostBondInspectionMode)
                        {
                            case PostBondInspectionModeEnum.WithoutReferenceSearch:

                                this.InitOnePointWithoutReferControl();
                                break;
                            case PostBondInspectionModeEnum.WithReferenceSearch:

                                this.InitOnePointWithReferControl();
                                break;
                        }
                        break;
                    case MeasurePointNumberEnum.TwoPoint:
                        switch (postBondInspection.PostBondInspectionMode)
                        {
                            case PostBondInspectionModeEnum.WithoutReferenceSearch:

                                this.InitTwoPointWithoutReferControl();
                                break;
                            case PostBondInspectionModeEnum.WithReferenceSearch:

                                this.InitTwoPointWithReferControl();
                                break;
                        }
                        break;
                }
            }
            else if (postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.EpoxyCheck)
            {
                switch (postBondInspection.MeasurePointNumber)
                {
                    case MeasurePointNumberEnum.OnePoint:
                        switch (postBondInspection.PostBondInspectionMode)
                        {
                            case PostBondInspectionModeEnum.WithoutReferenceSearch:

                                this.DispenseInitOnePointWithoutReferControl();

                                break;
                            case PostBondInspectionModeEnum.WithReferenceSearch:

                                this.DispenseInitOnePointWithReferControl();
                                break;
                        }
                        break;
                    case MeasurePointNumberEnum.TwoPoint:
                        break;
                }
            }
        }

        /// <summary>
        /// 点击Start传进来的对象
        /// </summary>
        private PostBondInspection postBondInspection;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController dispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// Module
        /// </summary>
        private GeneralCoordinateSystem CoordinateSystem;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 芯片三个角定位
        /// </summary>
        private AKRSPoint3D[] componentCornerPoint3Ds = new AKRSPoint3D[3];

        /// <summary>
        /// Substrate三个角定位
        /// </summary>
        private AKRSPoint3D[] substrateCornerPoint3Ds = new AKRSPoint3D[3];

        /// <summary>
        /// 当前Module坐标G0
        /// </summary>
        private AKRSPoint3D ModulePointInG0 = new AKRSPoint3D();

        private FrmPREditor frmPREditor;

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
        /// 设置PREfitor硬件
        /// </summary>
        /// <param name="pREntity"></param>
        private void SetPREditorUIControl(PREntity pREntity)
        {
            // 设置Bond硬件
            pREntity.SetHardware(this.system2Controller.GetHardware(CameraTypeEnum.BondCamera));
        }

        /// <summary>
        /// 做视觉模板界面
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnTeachModel_Click(object sender, EventArgs e)
        {
            string path = "";
            if (this.TileBarTeach.SelectedItem == this.TbiComSearchPoint)
            {
                path = this.postBondInspection.Name + "1";
            }
            if (this.TileBarTeach.SelectedItem == this.TbiSubSearchPoint)
            {
                path = this.postBondInspection.Name + "r1";
            }
            if (this.TileBarTeach.SelectedItem == this.TbiComSecondSearchPoint)
            {
                path = this.postBondInspection.Name + "2";
            }
            if (this.TileBarTeach.SelectedItem == this.TbiSubSecondSearchPoint)
            {
                path = this.postBondInspection.Name + "r2";
            }

            if (this.TileBarTeach.SelectedItem == this.TbiBacksideCrackDetect)
            {
                path = this.postBondInspection.Name + "BCD";
            }

            //先加载库里的prentity,如果存在则加载,如不存在,则创建新的
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(path);
            BaseVisionEntity visionEntity = null;
            if (pREntity == null)
            {
                pREntity = new PREntity(path);
                pREntity.Alg.AlgBeLong = AlgBeLongEnum.PostBond;
                visionEntity = pREntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }
            else
            {
                visionEntity = pREntity;
            }

            SetPREditorUIControl((PREntity)visionEntity);

            this.frmPREditor = new FrmPREditor((PREntity)visionEntity, false);
            this.frmPREditor.ShowDialog();
            //DialogResult dialog = this.frmPREditor.ShowDialog();
            //if (dialog == DialogResult.OK)
            //{
            //    this.frmPREditor.Dispose();
            //}
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
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();

            this.postBondInspection.PostBond.State = AssistantStateEnum.Able;
            PostBondInspectionRepository.GetInstance().Save();
            this.DialogResult = DialogResult.OK;
            this.ucGuideMove1.Dispose();
            this.Dispose();
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
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                // 清除数据
                this.componentCornerPoint3Ds = new AKRSPoint3D[3];

                this.Close();
            }
        }

        /// <summary>
        /// 自动对焦按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            this.bondHeadController.AutoFocusAssistance(CameraTypeEnum.BondCamera, sender, this);
        }

        #region BondInSystem2

        /// <summary>
        /// 1点无参考点焊后检测
        /// </summary>
        private void InitOnePointWithoutReferControl()
        {
            this.stepCount = this.postBondInspection.IsDetectBacksideCrack ? 5 : 4;

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 做模板  
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"step 1/{stepCount} :移动到芯片上的第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                // 判断一下是否做了模板
                                 if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P1VisionPos =
                                    TUAssistantHelper.GetPosBySystem(
                                        CoordinateSystem,
                                        MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance()
                                        .BondHeadParam.HeadToCameraOffset;

                                this.postBondInspection.VisionConfig.P1PRName =this.postBondInspection.Name+"1";

                                // 移动到拍照位，验证用（到拍照位，完成）
                                AKRSPoint3D aKRSPoint3D =
                                    CoordinateSystem.SelfPosToG0(this.postBondInspection.VisionConfig.P1VisionPos)
                                    - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                this.bondModuleController.MoveToG0Pos(aKRSPoint3D);

                                // 保存P1定位结果
                                (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"1");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P1VisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }

                                this.TileBarTeach.SelectedItem = this.TbiCompCorner1;
                                this.BtnTeachModel.Visible=false;
                            },
                       doneAction: () =>
                       {
                       }),

                    // 左上角
                    new AssistantConfig(
                        index: 1, 
                        descritpion: $"step 2/{stepCount} :移动到芯片左上角第一点，该点和第二点定义了芯片的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;
                            this.BtnTeachModel.Visible=true;
                        },
                        nextAction: () =>
                        {
                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            // 记录坐标
                            this.componentCornerPoint3Ds[0] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                            this.TileBarTeach.SelectedItem = this.TbiCompCorner2;

                        },
                        doneAction: () =>
                        {
                        }),

                    // 右上角
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"step 3/{stepCount} :移动到芯片右上角第二点，该点和第一点定义了芯片的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiCompCorner1;
                            },
                        nextAction: () =>
                            {
                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录坐标
                                this.componentCornerPoint3Ds[1] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片角度
                                double angle = Math.Atan(
                                                   (this.componentCornerPoint3Ds[1].Y - this.componentCornerPoint3Ds[0].Y)
                                                   / (this.componentCornerPoint3Ds[1].X - this.componentCornerPoint3Ds[0].X)) * (180 / Math.PI);

                                // if (Math.Abs(angle) > 0.1)
                                // {
                                //     AKRSXtraMessageBox.Show("The distance between two points is too large, please try again");
                                //    this.stepIndex--;
                                //    return;
                                // }
                                //else
                                //{
                                //     // 保存
                                
                                //}

                                this.TileBarTeach.SelectedItem = this.TbiCompCorner3;
                            },
                        doneAction: () =>
                            {
                            }),

                    // 第三点
                    new AssistantConfig(
                        index: 3,  
                        descritpion: $"step 4/{stepCount} :移动到芯片右下角第三点，该点和第一点定义了芯片的位置。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiCompCorner2;
                            },
                        nextAction: () =>
                            {
                                // 记录坐标
                                this.componentCornerPoint3Ds[2] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                 // 移动到模版中心
                                this.bondModuleController.MoveBondXYZWithoutSafe( this.postBondInspection.VisionConfig.P1VisionMatchResultInModule);


                                this.TileBarTeach.SelectedItem = this.TbiBacksideCrackDetect;
                            },
                        doneAction: () =>
                            {
                                //if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                //{
                                //    return;
                                //}

                                //// 记录坐标
                                //this.componentCornerPoint3Ds[2] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片中点  相对于Module中心的偏移量
                               // this.postBondInspection.PostBondTeachPos = (this.componentCornerPoint3Ds[2] + this.componentCornerPoint3Ds[0])/ 2;                                

                                // 移动到芯片中心，验证用(轴到中心，完成)
                               //this.BondModule.MoveToG0Pos3D(CoordinateSystem.SelfPosToG0(this.postBondInspection.PostBondTeachPos));

                            }),

                    // 背崩检测模版
                    new AssistantConfig(
                        index: 4,
                        descritpion: $"step 5/{stepCount} :点击“编辑PR”，做背崩模板。\r\n",
                        isShowTitle: this.postBondInspection.IsDetectBacksideCrack,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiCompCorner2;
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

                                this.postBondInspection.VisionConfig.BacsideCrackDetectPRName =
                                    this.postBondInspection.Name + "BCD";

                           //     // 定位
                           //     (BaseAlgResult baseAlgResult, bool isCrack) res =
                           //         this.system2Controller.BacksideCrackDetect(
                           //             null,
                           //             this.postBondInspection.VisionConfig.P1PRName,
                           //             this.postBondInspection.VisionConfig.BacsideCrackDetectPRName);

                           //     // todo:失败要怎么处理？
                           //if (res.baseAlgResult == null)
                           //{
                           //    DialogResult dialog = AKRSXtraMessageBox.Show(
                           //        $"背崩模板定位失败，请重新编辑！",
                           //        "错误",
                           //        MessageBoxButtons.OK,
                           //        MessageBoxIcon.Question);
                           //}
                           //else
                           //{
                           //    this.DialogResult = DialogResult.OK;
                           //}

                            }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);
        }

        /// <summary>
        /// 1Point带参考点焊后检测
        /// </summary>
        private void InitOnePointWithReferControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"step 1/8 :移动到芯片上的第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {

                            // 判断一下是否做了模板
                                 if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P1VisionPos =
                                    TUAssistantHelper.GetPosBySystem(
                                        CoordinateSystem,
                                        MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance()
                                        .BondHeadParam.HeadToCameraOffset;

                                this.postBondInspection.VisionConfig.P1PRName = this.postBondInspection.Name + "1";

                               // 保存P1定位结果
                              (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"1");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P1VisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }


                                this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                            },
                       doneAction: () =>
                       {
                       }),

                     // 做substrate模板
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"step 2/8 :移动到基板上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;
                        },
                        nextAction: () =>
                            {
                                // 判断一下是否做了模板
                                 if ( VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"r1") == null)
                                 {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作基板PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                                    this.stepIndex--;
                                    return;
                                 }


                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                this.postBondInspection.VisionConfig.P1ReferVisionPos =
                                    TUAssistantHelper.GetPosBySystem(
                                        CoordinateSystem,
                                        MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance()
                                        .BondHeadParam.HeadToCameraOffset;

                                this.postBondInspection.VisionConfig.P1ReferName =this.postBondInspection.Name+"r1";

                                (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"r1");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P1ReferVisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }

                                  this.TileBarTeach.SelectedItem = this.TbiCompCorner1;
                            },
                       doneAction: () =>
                       {
                       }),

                    // component 第一点左上角
                    new AssistantConfig(
                        index: 2,
                        descritpion: "step 3/8 :移动到芯片左上角第一点，该点和第二点定义了芯片的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                        },
                        nextAction: () =>
                        {
                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            this.componentCornerPoint3Ds[0] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                            this.BtnTeachModel.Visible=false;

                             this.TileBarTeach.SelectedItem = this.TbiSubCorner1;
                        },
                        doneAction: () =>
                        {
                        }),
                    
                    // substrate 第一点
                     new AssistantConfig(
                        index: 3,
                        descritpion: "step 4/8 :移动到基板上第一点，该点和第二点定义了基板的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiCompCorner1;
                            this.BtnTeachModel.Visible=true;
                        },
                        nextAction: () =>
                        {
                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            this.substrateCornerPoint3Ds[0] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                            this.TileBarTeach.SelectedItem = this.TbiCompCorner2;
                        },
                    doneAction: () =>
                    {
                    }),
                    // component 第二点右上角
                    new AssistantConfig(
                    index: 4,
                        descritpion: "step 5/8 :移动到芯片右上角第二点，该点和第一点定义了芯片的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiSubCorner1;
                            },
                        nextAction: () =>
                            {
                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                this.componentCornerPoint3Ds[1] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片角度
                                double angle = Math.Atan(
                                                   (this.componentCornerPoint3Ds[1].Y - this.componentCornerPoint3Ds[0].Y)
                                                   / (this.componentCornerPoint3Ds[1].X - this.componentCornerPoint3Ds[0].X)) * (180 / Math.PI);

                                 if (Math.Abs(angle) > 0.1)
                                 {
                                    AKRSXtraMessageBox.Show( "芯片两点间距太大，请重试");
                                    this.stepIndex--;
                                    return;
                                 }
                                else
                                {
                                     // 保存
                                   // this.postBondInspection.PostBondTeachAngle = angle;
                                }
                                this.TileBarTeach.SelectedItem = this.TbiSubCorner2;

                            },
                        doneAction: () =>
                            {
                            }),
                     // substrate 第二点
                      new AssistantConfig(
                    index: 5,
                        descritpion: "step 6/8 :移动到基板上第二点，该点和第一点定义了基板的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                               this.TileBarTeach.SelectedItem = this.TbiCompCorner2;
                            },
                        nextAction: () =>
                            {
                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                this.substrateCornerPoint3Ds[1] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片角度
                                double angle = Math.Atan(
                                                   (this.substrateCornerPoint3Ds[1].Y - this.substrateCornerPoint3Ds[0].Y)
                                                   / (this.substrateCornerPoint3Ds[1].X - this.substrateCornerPoint3Ds[0].X)) * (180 / Math.PI);

                                 if (Math.Abs(angle) > 0.1)
                                 {
                                    AKRSXtraMessageBox.Show( "基板两点间距太大，请重试");
                                    this.stepIndex--;
                                    return;
                                 }
                                else
                                {
                                   // 保存
                                  // this.postBondInspection.PostBondSubstrateTeachAngle = angle;
                                }

                                this.TileBarTeach.SelectedItem = this.TbiCompCorner3;
                            },

                        doneAction: () =>
                            {
                            }),

                    // component 第三点右下角
                    new AssistantConfig(
                        index: 6,
                        descritpion: "step 7/8 :移动到芯片右下角第三点，该点和第一点定义了芯片的位置。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                 this.TileBarTeach.SelectedItem = this.TbiSubCorner2;
                            },
                        nextAction: () =>
                            {


                                 if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录坐标
                                this.componentCornerPoint3Ds[2] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片中点
                               // this.postBondInspection.PostBondTeachPos = (this.componentCornerPoint3Ds[2] + this.componentCornerPoint3Ds[0])
                               //                   / 2;
                                this.TileBarTeach.SelectedItem = this.TbiSubCorner3;
                            },
                        doneAction: () =>
                            {

                            }),
                     // substrate 第三点
                     new AssistantConfig(
                        index: 7,
                        descritpion: "step 8/8 :移动到基板上第三点，该点和第一点定义了基板的位置。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiCompCorner3;
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
                                // 记录坐标
                                this.substrateCornerPoint3Ds[2] = TUAssistantHelper.GetPosBySystem(CoordinateSystem); 

                                // 计算芯片中点
                              //  this.postBondInspection.PostBondSubstrateTeachPos = (this.substrateCornerPoint3Ds[2] + this.substrateCornerPoint3Ds[0]) / 2;
                            }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);
        }

        /// <summary>
        /// 1Point距离测量焊后检测
        /// </summary>
        private void InitOnePointDistanceControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 做component模板
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"step 1/2 :移动到芯片上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {

                            // 判断一下是否做了模板
                                 if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P1VisionPos =
                                    TUAssistantHelper.GetPosBySystem(
                                        CoordinateSystem,
                                        MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance()
                                        .BondHeadParam.HeadToCameraOffset;

                                this.postBondInspection.VisionConfig.P1PRName =this.postBondInspection.Name+"1";

                               // 保存P1定位结果
                              (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"1");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P1VisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }


                                this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                            },
                       doneAction: () =>
                       {
                       }),

                     // 做substrate模板
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"step 2/2 :移动到基板上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;
                        },
                        nextAction: () =>
                            {

                            },
                       doneAction: () =>
                       {
                            // 判断一下是否做了模板
                                 if ( VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"r1") == null)
                                 {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作基板PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                                    this.stepIndex--;
                                    return;
                                 }


                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P1ReferVisionPos =TUAssistantHelper.GetPosBySystem(CoordinateSystem,
                                    MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance()
                                    .BondHeadParam.HeadToCameraOffset;

                                this.postBondInspection.VisionConfig.P1ReferName =this.postBondInspection.Name+"r1";

                                // 保存ReferenceP1定位结果

                                (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"r1");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P1ReferVisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }

                                  this.TileBarTeach.SelectedItem = this.TbiCompCorner1;
                       }),

                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);
        }

        /// <summary>
        /// 2Point不带参考点焊后检测
        /// </summary>
        private void InitTwoPointWithoutReferControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 做component第一点模板
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"step 1/5 :移动到芯片上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // 判断一下是否做了模板
                                if ( VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.stepIndex--;
                                    return;
                                }


                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P1VisionPos =TUAssistantHelper.GetPosBySystem(CoordinateSystem ,
                                    MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance()
                                    .BondHeadParam.HeadToCameraOffset;

                                this.postBondInspection.VisionConfig.P1PRName =this.postBondInspection.Name+"1";

                                // 保存P1定位结果
                                  (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"1");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P1VisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }

                                this.TileBarTeach.SelectedItem = this.TbiComSecondSearchPoint;
                        },
                       doneAction: () =>
                       {
                       }),

                     // 做component第二点模板
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"step 2/5 :移动到芯片上第二点并示教模板。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;
                        },
                        nextAction: () =>
                        {
                            // 判断一下是否做了模板
                                 if ( VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"2") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片第二点PR！!",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.TileBarTeach.SelectedItem = this.TbiComSecondSearchPoint;
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P2VisionPos =TUAssistantHelper.GetPosBySystem(CoordinateSystem ,
                                    MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance()
                                    .BondHeadParam.HeadToCameraOffset;

                                this.postBondInspection.VisionConfig.P2PRName =this.postBondInspection.Name+"2";

                                // 保存P2定位结果

                             (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"2");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P2VisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }

                            this.BtnTeachModel.Visible=false;

                            this.TileBarTeach.SelectedItem = this.TbiCompCorner1;

                        },
                       doneAction: () =>
                       {
                       }),

                    // component 第一点左上角
                    new AssistantConfig(
                        index: 2,
                        descritpion: "step 3/5 :移动到芯片左上角第一点，该点和第二点定义了芯片的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSecondSearchPoint;
                             this.BtnTeachModel.Visible=true;
                        },
                        nextAction: () =>
                        {

                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            // 记录坐标
                            this.componentCornerPoint3Ds[0] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                            this.TileBarTeach.SelectedItem = this.TbiCompCorner2;
                        },
                    doneAction: () =>
                    {
                    }),
                    
                 
                    // component 第二点右上角
                    new AssistantConfig(
                        index: 3,
                        descritpion: "step 4/5 :移动到芯片右上角第二点，该点和第一点定义了芯片的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiCompCorner1;
                            },
                        nextAction: () =>
                            {


                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录坐标
                                this.componentCornerPoint3Ds[1] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片角度
                                double angle = Math.Atan(
                                                   (this.componentCornerPoint3Ds[1].Y - this.componentCornerPoint3Ds[0].Y)
                                                   / (this.componentCornerPoint3Ds[1].X - this.componentCornerPoint3Ds[0].X)) * (180 / Math.PI);

                                 if (Math.Abs(angle) > 0.1)
                                 {
                                     AKRSXtraMessageBox.Show( "芯片两点间距太大，请重试");
                                    this.stepIndex--;
                                    return;
                                 }
                                else
                                {
                                     // 保存
                                //    this.postBondInspection.PostBondTeachAngle = angle;
                                }

                                 this.TileBarTeach.SelectedItem = this.TbiCompCorner3;
                            },
                        doneAction: () =>
                            {
                            }),
                
                    // component 第三点右下角
                    new AssistantConfig(
                        index: 4,
                        descritpion: "step 5/5 :移动到芯片右下角第三点，该点和第一点定义了芯片的位置。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                 this.TileBarTeach.SelectedItem = this.TbiCompCorner2;
                            },
                        nextAction: () =>
                            {
                                 if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录坐标
                                this.componentCornerPoint3Ds[2] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片中点
                            //    this.postBondInspection.PostBondTeachPos = (this.componentCornerPoint3Ds[2] + this.componentCornerPoint3Ds[0]) / 2;

                            },
                        doneAction: () =>
                            {

                            }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);
        }

        /// <summary>
        /// 2Point带参考点焊后检测
        /// </summary>
        private void InitTwoPointWithReferControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 做component第一点模板
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"step 1/10 :移动到芯片上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                    nextAction: () =>
                    {
                                if ( VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P1VisionPos =TUAssistantHelper.GetPosBySystem(CoordinateSystem,
                                    MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;;
                                this.postBondInspection.VisionConfig.P1PRName =this.postBondInspection.Name+"1";

                                // 保存P1定位结果

                          (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"1");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P1VisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }


                                this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                            },
                       doneAction: () =>
                       {
                       }),                   

                     // 做substrate第一点模板
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"step 2/10 :移动到基板上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;
                        },
                        nextAction: () =>
                            {
                               

                                // 判断一下是否做了模板
                                 if ( VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"r1") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作基板PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                     this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P1ReferVisionPos =TUAssistantHelper.GetPosBySystem(CoordinateSystem,
                                    MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;;
                                this.postBondInspection.VisionConfig.P1ReferName =this.postBondInspection.Name+"r1";

                                // 保存ReferenceP1定位结果
                               (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"r1");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P1ReferVisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }

                               this.TileBarTeach.SelectedItem = this.TbiComSecondSearchPoint;
                            },
                       doneAction: () =>
                       {
                       }),


                      // 做component第二点模板
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"step 3/10 :移动到芯片上第二点并示教模板。 ",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                             this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                        },
                    nextAction: () =>
                    {
                                 if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"2") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片第二点PR！!",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.TileBarTeach.SelectedItem = this.TbiComSecondSearchPoint;
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P2VisionPos =TUAssistantHelper.GetPosBySystem(CoordinateSystem ,
                                    MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;;
                                this.postBondInspection.VisionConfig.P2PRName =this.postBondInspection.Name+"2";

                                // 保存P1定位结果
                                (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"2");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P2VisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }

                                this.TileBarTeach.SelectedItem = this.TbiSubSecondSearchPoint;
                            },
                       doneAction: () =>
                       {
                       }),



                     // 做substrate第二点模板
                    new AssistantConfig(
                        index: 3,
                        descritpion: $"step 4/10 :移动到基板上第二点并示教模板。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSecondSearchPoint;
                        },
                        nextAction: () =>
                            {
                                if ( VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"r2") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作基板第二点PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.TileBarTeach.SelectedItem = this.TbiSubSecondSearchPoint;
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 保存拍照点位
                                this.postBondInspection.VisionConfig.P2ReferVisionPos =
                                    TUAssistantHelper.GetPosBySystem(
                                        CoordinateSystem,
                                        MultipleHeightMeasurementType.TouchDown) + BondDevicePara.GetInstance()
                                        .BondHeadParam.HeadToCameraOffset;

                                this.postBondInspection.VisionConfig.P2ReferName =this.postBondInspection.Name+"r2";

                                // 保存ReferenceP2定位结果
                                 (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.postBondInspection.Name+"r2");
                                      if (success)
                                      {
                                          this.postBondInspection.VisionConfig.P2ReferVisionMatchResultInModule = CoordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                          this.stepIndex--;
                                          return;
                                      }

                                this.BtnTeachModel.Visible=false;

                                this.TileBarTeach.SelectedItem = this.TbiCompCorner1;
                            },
                       doneAction: () =>
                       {
                       }),


                    // component 第一点左上角
                    new AssistantConfig(
                        index: 4,
                        descritpion: "step 5/10 :移动到芯片左上角第一点，该点和第二点定义了芯片的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSecondSearchPoint;
                            this.BtnTeachModel.Visible=true;
                        },
                        nextAction: () =>
                        {


                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            // 记录坐标
                            this.componentCornerPoint3Ds[0] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                            this.TileBarTeach.SelectedItem = this.TbiSubCorner1;
                        },
                    doneAction: () =>
                    {
                    }),
                    
                    // substrate 第一点
                     new AssistantConfig(
                        index: 5,
                        descritpion: "step 6/10 :移动到基板上第一点，该点和第二点定义了基板的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiCompCorner1;
                        },
                        nextAction: () =>
                        {


                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            // 记录坐标
                            this.substrateCornerPoint3Ds[0] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                             this.TileBarTeach.SelectedItem = this.TbiCompCorner2;
                        },
                    doneAction: () =>
                    {
                    }),

                    // component 第二点右上角
                    new AssistantConfig(
                    index: 6,
                        descritpion: "step 7/10 :移动到芯片右上角第二点，该点和第一点定义了芯片的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiSubCorner1;
                            },
                        nextAction: () =>
                            {


                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录坐标
                                this.componentCornerPoint3Ds[1] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片角度
                                double angle = Math.Atan(
                                                   (this.componentCornerPoint3Ds[1].Y - this.componentCornerPoint3Ds[0].Y)
                                                   / (this.componentCornerPoint3Ds[1].X - this.componentCornerPoint3Ds[0].X)) * (180 / Math.PI);

                                 if (Math.Abs(angle) > 0.1)
                                 {
                                    AKRSXtraMessageBox.Show( "芯片两点间距太大，请重试");
                                    this.stepIndex--;
                                    return;
                                 }
                                else
                                {
                                     // 保存
                                  //    this.postBondInspection.PostBondTeachAngle = angle;
                                }

                                this.TileBarTeach.SelectedItem = this.TbiSubCorner2;
                            },
                        doneAction: () =>
                            {
                            }),

                     // substrate 第二点
                      new AssistantConfig(
                    index: 7,
                        descritpion: "step 8/10 :移动到基板上第二点，该点和第一点定义了基板的旋转角度。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                               this.TileBarTeach.SelectedItem = this.TbiCompCorner2;
                            },
                        nextAction: () =>
                            {


                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录坐标
                                this.substrateCornerPoint3Ds[1] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                double angle = Math.Atan(
                                                   (this.substrateCornerPoint3Ds[1].Y - this.substrateCornerPoint3Ds[0].Y)
                                                   / (this.substrateCornerPoint3Ds[1].X - this.substrateCornerPoint3Ds[0].X)) * (180 / Math.PI);

                                 if (Math.Abs(angle) > 0.1)
                                 {
                                      AKRSXtraMessageBox.Show(
                                       "基板两点间距太大，请重试");

                                    this.stepIndex--;
                                    return;
                                 }
                                else
                                {
                                    //  this.postBondInspection.PostBondSubstrateTeachAngle = angle;
                                }

                                 this.TileBarTeach.SelectedItem = this.TbiCompCorner3;
                            },
                        doneAction: () =>
                            {
                            }),

                    // component 第三点右下角
                    new AssistantConfig(
                        index: 8,
                        descritpion: "step 9/10 :移动到芯片右下角第三点，该点和第一点定义了芯片的位置。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                 this.TileBarTeach.SelectedItem = this.TbiSubCorner2;
                            },
                        nextAction: () =>
                            {


                                 if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录坐标
                                this.componentCornerPoint3Ds[2] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片中点
                            //    this.postBondInspection.PostBondTeachPos = (this.componentCornerPoint3Ds[2] + this.componentCornerPoint3Ds[0]) / 2;

                                 this.TileBarTeach.SelectedItem = this.TbiSubCorner3;

                            },
                        doneAction: () =>
                            {

                            }),
                     // substrate 第三点
                     new AssistantConfig(
                        index: 9,
                        descritpion: "step 10/10 :移动到基板上第三点，该点和第一点定义了基板的位置。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiCompCorner3;
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
                                // 记录坐标
                                this.substrateCornerPoint3Ds[2] = TUAssistantHelper.GetPosBySystem(CoordinateSystem);

                                // 计算芯片中点
                           //     this.postBondInspection.PostBondSubstrateTeachPos = (this.substrateCornerPoint3Ds[2] + this.substrateCornerPoint3Ds[0]) / 2;
                            }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);
        }
        #endregion


        /// <summary>
        /// 点胶1点无参考点焊后检测
        /// </summary>
        private void DispenseInitOnePointWithoutReferControl()
        {

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"step 1/1 :移动到芯片上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: false,
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
                             if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 获取轴坐标

                                this.postBondInspection.VisionConfig.P1PRName =this.postBondInspection.Name+"1";

                           PREntity pREntity=(PREntity)VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1");

                           pREntity.DoWork();

                           if(pREntity.AlgResults.Count > 0)
                           {
                               this.postBondInspection.PostBondEpoxyArea=new List<double>();
                               this.postBondInspection.PostBondEpoxyCenterX=new List<double>();
                               this.postBondInspection.PostBondEpoxyCenterY=new List<double>();
                               this.postBondInspection.PostBondEpoxyNum=pREntity.AlgResults.Count;
                               for (int i = 0; i < pREntity.AlgResults.Count; i++)
                               {
                                   BlobResult blobResult=(BlobResult)pREntity.AlgResults[i];
                                   MatchResult matchResult=new MatchResult(blobResult.CenterX,blobResult.CenterY,blobResult.Area);

                                    AKRSPoint3D point = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(new AKRSPoint3D(matchResult.CenterX,matchResult.CenterY,0));

                                   this.postBondInspection.PostBondEpoxyArea.Add(blobResult.Area);
                                   this.postBondInspection.PostBondEpoxyCenterX.Add(point.X);
                                   this.postBondInspection.PostBondEpoxyCenterY.Add(point.Y);
                               }
                           }
                           else
                           {
                                  AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                  this.stepIndex--;
                                  return;
                           }
                       })
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("点胶模组", true);
        }


        /// <summary>
        /// 点胶1点有参考点焊后检测
        /// </summary>
        private void DispenseInitOnePointWithReferControl()
        {

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    new AssistantConfig(
                        index: 0,
                        descritpion:  $"step 1/2 :移动到芯片上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                 if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1") == null)
                                {
                                    DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                        $"请先制作芯片PR！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                    this.stepIndex--;
                                    return;
                                }

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                               this.postBondInspection.VisionConfig.P1PRName =this.postBondInspection.Name+"1";

                               PREntity pREntity=(PREntity)VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"1");

                               pREntity.DoWork();

                                    if (pREntity.AlgResults.Count == 0)
                                    {
                                         AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                         this.stepIndex--;
                                         return;
                                    }
                               //BlobResult blobResult=(BlobResult)pREntity.AlgResult;

                               // MatchResult matchResult=new MatchResult(blobResult.CenterX,blobResult.CenterY,blobResult.Area);

                               // this.postBondInspection.PostBondEpoxyArea=blobResult.Area;

                               //  // 将值转化成G(0)
                               // AKRSPoint3D point = System2Module.GetInstance().BondModule.ConvertPixelToG0Pos(
                               //     System2Module.GetInstance().BondModule.Get3DRealPosition(),
                               //     matchResult);


                               //this.postBondInspection.VisionConfig.P1VisionMatchResultInModule = this.CoordinateSystem.G0PosToSelf(point + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset);

                               this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                            },
                       doneAction: () =>
                       {
                       }),
                      // 做substrate模板
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"step 2/2 :移动到基板上第一点并示教模板。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;
                        },
                        nextAction: () =>
                        {
                        },
                       doneAction: () =>
                       {
                           // 判断一下是否做了模板
                           if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name + "r1") == null)
                           {
                               DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                   $"请先制作基板PR！",
                                   "Warn",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Warning);
                               this.TileBarTeach.SelectedItem = this.TbiSubSearchPoint;
                               this.stepIndex--;
                               return;
                           }

                           if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                           {
                               return;
                           }

                           this.postBondInspection.VisionConfig.P1ReferName = this.postBondInspection.Name + "r1";
                           PREntity pREntity=(PREntity)VisionEntityRepository.GetInstance().Find(postBondInspection.Name+"r1");

                           //pREntity.DoWork();
                           // if (pREntity.AlgResults.Count == 0)
                           //     {
                           //          AKRSXtraMessageBox.Show("Visual positioning failure , please edit Program retry");
                           //          this.stepIndex--;
                           //          return;
                           //     }
                           //BlobResult blobResult=(BlobResult)pREntity.AlgResult;

                           // MatchResult matchResult=new MatchResult(blobResult.CenterX,blobResult.CenterY,blobResult.Area);

                           // this.postBondInspection.PostBondEpoxyArea=blobResult.Area;

                           //  // 将值转化成G(0)
                           // AKRSPoint3D point = System2Module.GetInstance().BondModule.ConvertPixelToG0Pos(
                           //     System2Module.GetInstance().BondModule.Get3DRealPosition(),
                           //     matchResult);


                            //this.postBondInspection.VisionConfig.P1ReferVisionMatchResultInModule = this.CoordinateSystem.G0PosToSelf(point + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset);

                       }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiComSearchPoint;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("点胶模组", true);
        }


        /// <summary>
        /// 移动到中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveToCenter_Click(object sender, EventArgs e)
        {
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("Object to center");

            if (pREntity != null)
            {
                pREntity.DoWork();

                if (postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.EpoxyCheck)
                {
                    AKRSPoint3D pos = this.dispenseController.GetAxisPos();

                    BlobResult blobResult = (BlobResult)pREntity.AlgResult;
                    MatchResult matchResult = new MatchResult(blobResult.CenterX, blobResult.CenterY, blobResult.Area);
                    AKRSPoint3D CenterInG0 = this.dispenseController.ConvertVisionResultInG0(matchResult, pos);

                    System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(CenterInG0);
                }
                else
                {
                    // 当前坐标
                    AKRSPoint3D pos = this.bondModuleController.Get3DRealPosition();

                    AKRSPoint3D CenterInG0 = System2Module.GetInstance().BondModule.ConvertPixelToG0Pos(pos, (MatchResult)pREntity.AlgResult);

                    this.bondModuleController.MoveToG0Pos(CenterInG0);
                }
            }
        }

        /// <summary>
        /// 编辑PR
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEditPR_Click(object sender, EventArgs e)
        {
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("Object to center");

            BaseVisionEntity visionEntity = null;

            if (pREntity == null)
            {
                pREntity = new PREntity("Object to center");
                pREntity.Alg.AlgBeLong = AlgBeLongEnum.PostBond;
                visionEntity = pREntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }
            else
            {
                visionEntity = pREntity;
            }

            SetPREditorUIControl((PREntity)visionEntity);

            if (this.frmPREditor == null || this.frmPREditor.IsDisposed)
            {
                this.frmPREditor = new FrmPREditor((PREntity)visionEntity, false);
                DialogResult dialog = this.frmPREditor.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    this.frmPREditor.Dispose();
                }
            }
            else
            {
                // 弹出视觉窗体
                DialogResult dialog = this.frmPREditor.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    this.frmPREditor.Dispose();
                }
            }
        }
    }
}
