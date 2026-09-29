using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 检测
    /// </summary>
    public partial class FrmAssistantDefect : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 方向胖
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 焊后检测示教窗体
        /// </summary>
        /// <param name="postBondInspection">焊后检测对象</param>
        /// <param name="bondPosition">焊点对象</param>
        public FrmAssistantDefect(PostBondInspection postBondInspection, BondPosition bondPosition)
        {
            this.InitializeComponent();
            this.postBondInspection = postBondInspection;
            this.bondPosition = bondPosition;
            this.tileBarItem1.Visible = this.TwoPoint;
            this.tileBarItem2.Visible = this.postBondInspection.PostBondInspectionMode
                                        == PostBondInspectionModeEnum.WithReferenceSearch;
            this.tileBarItem3.Visible = this.FourPoint;
        }

        /// <summary>
        /// 焊后检测示教窗体
        /// </summary>
        /// <param name="postBondInspection">焊后检测对象</param>
        /// <param name="point3D">焊点对象</param>
        public FrmAssistantDefect(PostBondInspection postBondInspection, AKRSPoint3D point3D)
        {
            this.InitializeComponent();
            this.postBondInspection = postBondInspection;
            this.defectPoint3D = point3D;
        }

        /// <summary>
        /// 检测的位置
        /// </summary>
        private AKRSPoint3D defectPoint3D;

        /// <summary>
        /// 焊后检测对象
        /// </summary>
        private readonly PostBondInspection postBondInspection;

        /// <summary>
        /// 焊点对象
        /// </summary>
        private readonly BondPosition bondPosition;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmAssistantDefect_Load(object sender, EventArgs e)
        {
            if (this.postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.EpoxyCheck)
            {
                if (!this.postBondInspection.IsPreDispenseCheck)
                {
                    this.InitControlEpoxyCheck();
                }
                else
                {
                    this.InitControlPreDispense();
                }
            }
            else if (this.postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.AfterBondCheck)
            {
                this.InitControlAfterBond();
            }

            this.SetUiControl();
        }

        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 1;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 编辑的结果
        /// </summary>
        public bool EditResult { get; set; }

        /// <summary>
        /// 一点编辑模板
        /// </summary>
        private bool OnePoint => this.postBondInspection.MeasurePointNumber == MeasurePointNumberEnum.OnePoint
                                && this.postBondInspection.PostBondInspectionMode
                                == PostBondInspectionModeEnum.WithoutReferenceSearch;

        /// <summary>
        /// 两点编辑模板
        /// </summary>
        private bool TwoPoint => this.postBondInspection.MeasurePointNumber == MeasurePointNumberEnum.TwoPoint
                                 && this.postBondInspection.PostBondInspectionMode
                                 == PostBondInspectionModeEnum.WithoutReferenceSearch;

        /// <summary>
        /// 三点编辑模板
        /// </summary>
        private bool ThreePoint => this.postBondInspection.ReferencePointNumber == MeasurePointNumberEnum.OnePoint
                                 && this.postBondInspection.PostBondInspectionMode
                                 == PostBondInspectionModeEnum.WithReferenceSearch;

        /// <summary>
        /// 四点编辑模板
        /// </summary>
        private bool FourPoint => this.postBondInspection.ReferencePointNumber == MeasurePointNumberEnum.TwoPoint
                                 && this.postBondInspection.PostBondInspectionMode
                                 == PostBondInspectionModeEnum.WithReferenceSearch;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControlAfterBond()
        {
            string message1 = $"步骤 1/{stepCount}:编辑模板:{postBondInspection.Name}\r\n";

            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 定位点1
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: message1,
                                                   isShowTitle: true,
                                                   isShowBack: false,
                                                   isShowNext: !this.OnePoint,
                                                   isShowDone: this.OnePoint,
                                                   backAction: () =>
                                                       {
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.postBondInspection.VisionConfig.P1VisionPos =
                                                               this.GetVisionPos(this.postBondInspection.VisionConfig.P1PRName);

                                                           if (this.postBondInspection.MeasurePointNumber != MeasurePointNumberEnum.TwoPoint
                                                               && this.postBondInspection.VisionConfig.P1VisionPos != null)
                                                           {
                                                               this.stepIndex++;
                                                           }
                                                       },
                                                   doneAction: () =>
                                                       {
                                                           this.postBondInspection.VisionConfig.P1VisionPos =
                                                               this.GetVisionPos(this.postBondInspection.VisionConfig.P1PRName);
                                                           this.DialogResult = DialogResult.OK;
                                                       }),

                                               // 定位点2
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: message1,
                                                   isShowTitle: true,
                                                   isShowBack: false,
                                                   isShowNext: !this.TwoPoint,
                                                   isShowDone: this.TwoPoint,
                                                   backAction: () =>
                                                       {
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.postBondInspection.VisionConfig.P2VisionPos =
                                                               this.GetVisionPos(this.postBondInspection.VisionConfig.P2PRName);
                                                       },
                                                   doneAction: () =>
                                                       {
                                                           this.postBondInspection.VisionConfig.P2VisionPos =
                                                               this.GetVisionPos(this.postBondInspection.VisionConfig.P2PRName);
                                                           this.DialogResult = DialogResult.OK;
                                                       }),

                                               // 定位点3
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: message1,
                                                   isShowTitle: true,
                                                   isShowBack: false,
                                                   isShowNext: !this.ThreePoint,
                                                   isShowDone: this.ThreePoint,
                                                   backAction: () =>
                                                       {
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.postBondInspection.VisionConfig.P1ReferVisionPos =
                                                               this.GetVisionPos(
                                                                   postBondInspection.VisionConfig.P1ReferName);
                                                       },
                                                   doneAction: () =>
                                                       {
                                                           this.postBondInspection.VisionConfig.P1ReferVisionPos =
                                                               this.GetVisionPos(
                                                                    postBondInspection.VisionConfig.P1ReferName);
                                                           this.DialogResult = DialogResult.OK;
                                                       }),

                                               // 定位点4
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: message1,
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
                                                           this.postBondInspection.VisionConfig.P2ReferVisionPos =
                                                               this.GetVisionPos(
                                                                    postBondInspection.VisionConfig.P2ReferName);
                                                           this.DialogResult = DialogResult.OK;
                                                       }),
                                           };

            ucGuideMove = new UcGuideMove("FrmAssistantDefect1");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControlEpoxyCheck()
        {
            string message1 = $"步骤 1/{stepCount}:编辑模板:{postBondInspection.Name}\r\n 注意视觉模板仅能设置胶量检测";

            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 定位点1
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: message1,
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
                                                           if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name + "1") == null)
                                                           {
                                                              AKRSXtraMessageBox.Show(
                                                                   $"请先制作芯片PR！",
                                                                   "提示",
                                                                   MessageBoxButtons.OK,
                                                                   MessageBoxIcon.Warning);
                                                               return;
                                                           }

                                                           if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                                           {
                                                               return;
                                                           }
                                                           
                                                           PREntity pREntity = (PREntity)VisionEntityRepository
                                                               .GetInstance().Find(postBondInspection.Name + "1");
                                                           if (pREntity.GetAlgFlowType() != AlgFlowTypeEnum.EpoxyDetectAlg)
                                                           {
                                                               AKRSXtraMessageBox.Show("请重新设置胶量模板，模板必须为胶量检测");
                                                               return;
                                                           }

                                                           pREntity.DoWork();

                                                           // 设置模板归属
                                                           pREntity.Alg.AlgBeLong = AlgBeLongEnum.PostBond;

                                                           // 当前位置为拍照位
                                                           this.postBondInspection.VisionConfig.P1VisionPos =
                                                               TUAssistantHelper.GetPosBySystem(
                                                                   this.bondPosition.CoordinateSystem);

                                                           AKRSPoint3D point3D = new AKRSPoint3D();

                                                           if (pREntity.AlgResults.Count > 0)
                                                           {
                                                               this.postBondInspection.PostBondEpoxyArea =
                                                                   new List<double>();
                                                               this.postBondInspection.PostBondEpoxyCenterX =
                                                                   new List<double>();
                                                               this.postBondInspection.PostBondEpoxyCenterY =
                                                                   new List<double>();
                                                               this.postBondInspection.PostBondEpoxyNum =
                                                                   pREntity.AlgResults.Count;

                                                               for (int i = 0; i < pREntity.AlgResults.Count; i++)
                                                               {
                                                                   BlobResult blobResult = (BlobResult)pREntity.AlgResults[i];
                                                                   MatchResult matchResult = new MatchResult(
                                                                       blobResult.CenterX,
                                                                       blobResult.CenterY,
                                                                       blobResult.Area);
                                                                   
                                                                   AKRSPoint3D point = System2Module.GetInstance()
                                                                       .BondModule.BondCameraCoordinateSystem
                                                                       .ForwardConvertCoordinate(
                                                                           new AKRSPoint3D(
                                                                               matchResult.CenterX,
                                                                               matchResult.CenterY,
                                                                               0));

                                                                   this.postBondInspection.PostBondEpoxyArea.Add(
                                                                       blobResult.Area);
                                                                   this.postBondInspection.PostBondEpoxyCenterX.Add(
                                                                       point.X);
                                                                   this.postBondInspection.PostBondEpoxyCenterY.Add(
                                                                       point.Y);
                                                                   point3D = point3D + point;
                                                                   this.DialogResult = DialogResult.OK;
                                                               }

                                                               // TODO 这个位置算出来的有问题，后续和视觉讨论
                                                               //this.postBondInspection.VisionConfig.P1VisionPos +=
                                                               //    point3D;
                                                           }
                                                           else
                                                           {
                                                               AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                                               return;
                                                           }
                                                       }),
                                           };

            ucGuideMove = new UcGuideMove("FrmAssistantDefect2");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControlPreDispense()
        {
            string message1 = $"步骤 1/{stepCount}:编辑模板:{postBondInspection.Name}\r\n 注意视觉模板仅能设置胶量检测";

            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 定位点1
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: message1,
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
                                                           if (VisionEntityRepository.GetInstance().Find(postBondInspection.Name + "1") == null)
                                                           {
                                                              AKRSXtraMessageBox.Show(
                                                                   $"请先制作芯片PR！",
                                                                   "提示",
                                                                   MessageBoxButtons.OK,
                                                                   MessageBoxIcon.Warning);
                                                               this.stepIndex--;
                                                               return;
                                                           }

                                                           if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                                           {
                                                               return;
                                                           }

                                                           PREntity pREntity = (PREntity)VisionEntityRepository
                                                               .GetInstance().Find(postBondInspection.Name + "1");

                                                           // 设置模板归属
                                                           pREntity.Alg.AlgBeLong = AlgBeLongEnum.PostBond;

                                                           pREntity.DoWork();

                                                           // 当前位置为拍照位
                                                           this.postBondInspection.VisionConfig.P1VisionPos =
                                                               TUAssistantHelper.GetPosBySystem(null) - defectPoint3D;

                                                           if (pREntity.AlgResults.Count > 0)
                                                           {
                                                               this.postBondInspection.PostBondEpoxyArea =
                                                                   new List<double>();
                                                               this.postBondInspection.PostBondEpoxyCenterX =
                                                                   new List<double>();
                                                               this.postBondInspection.PostBondEpoxyCenterY =
                                                                   new List<double>();
                                                               this.postBondInspection.PostBondEpoxyNum =
                                                                   pREntity.AlgResults.Count;

                                                               for (int i = 0; i < pREntity.AlgResults.Count; i++)
                                                               {
                                                                   BlobResult blobResult = (BlobResult)pREntity.AlgResults[i];
                                                                   MatchResult matchResult = new MatchResult(
                                                                       blobResult.CenterX,
                                                                       blobResult.CenterY,
                                                                       blobResult.Area);

                                                                   AKRSPoint3D point = System2Module.GetInstance()
                                                                       .BondModule.BondCameraCoordinateSystem
                                                                       .ForwardConvertCoordinate(
                                                                           new AKRSPoint3D(
                                                                               matchResult.CenterX,
                                                                               matchResult.CenterY,
                                                                               0));

                                                                   this.postBondInspection.PostBondEpoxyArea.Add(
                                                                       blobResult.Area);
                                                                   this.postBondInspection.PostBondEpoxyCenterX.Add(
                                                                       point.X);
                                                                   this.postBondInspection.PostBondEpoxyCenterY.Add(
                                                                       point.Y);
                                                                   this.DialogResult = DialogResult.OK;
                                                               }
                                                           }
                                                           else
                                                           {
                                                               AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                                                               this.stepIndex--;
                                                               return;
                                                           }
                                                       }),
                                           };

            ucGuideMove = new UcGuideMove("FrmAssistantDefect3");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl();
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

            if (this.stepIndex == 0 && this.postBondInspection.MeasurePointNumber == MeasurePointNumberEnum.OnePoint)
            {
                this.stepIndex--;
            }

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
        }

        /// <summary>
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAutoFocus_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.AutoFocus(sender, this);
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEditPR_Click(object sender, EventArgs e)
        {
            AlgFlowTypeEnum algFlowTypeEnum =
                this.postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.EpoxyCheck
                    ? AlgFlowTypeEnum.EpoxyDetectAlg
                    : AlgFlowTypeEnum.XldModelAlg;

            if (this.stepIndex == 0)
            {
                TUAssistantHelper.EditPr(
                    this.postBondInspection.VisionConfig.P1PRName,
                    CameraTypeEnum.BondCamera,
                    algFlowTypeEnum);
            }
            else if (this.stepIndex == 1)
            {
                TUAssistantHelper.EditPr(
                    this.postBondInspection.VisionConfig.P2PRName,
                    CameraTypeEnum.BondCamera,
                    algFlowTypeEnum);
            }
            else if (this.stepIndex == 2)
            {
                TUAssistantHelper.EditPr(
                    this.postBondInspection.VisionConfig.P1ReferName,
                    CameraTypeEnum.BondCamera,
                    algFlowTypeEnum);
            }
            else if (this.stepIndex == 3)
            {
                TUAssistantHelper.EditPr(
                    this.postBondInspection.VisionConfig.P2ReferName,
                    CameraTypeEnum.BondCamera,
                    algFlowTypeEnum);
            }
        }

        /// <summary>
        /// 获取视觉定位点位
        /// </summary>
        /// <param name="name">视觉模板名称</param>
        /// <returns>结果</returns>
        private AKRSPoint3D GetVisionPos(string name)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return new AKRSPoint3D();
            }

            if (VisionEntityRepository.GetInstance().Find(name) == null)
            {
                AKRSXtraMessageBox.Show(
                    $"请先制作芯片PR！",
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                this.stepIndex--;
                return null;
            }

            // 定位成功，计算偏移值
            (bool success, AKRSPoint3D point, double angle) =
                TUAssistantHelper.AssistantPRAndAngle(
                    name);
            if (success)
            {
                return this.bondPosition.CoordinateSystem.G0PosToSelf(point);
            }
            else
            {
                AKRSXtraMessageBox.Show("定位失败，请重新编辑模板");
                this.stepIndex--;
                return null;
            }
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAssistantDefect_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove.Dispose();
            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
        }
    }
}