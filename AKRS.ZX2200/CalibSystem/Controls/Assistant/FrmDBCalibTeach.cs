using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.DispenseSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Modules;

using DevExpress.XtraEditors;

namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    /// <summary>
    /// Bond标定示教窗体
    /// </summary>
    public partial class FrmDBCalibTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 点胶模组
        /// </summary>
        private DispenseController DispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// 运行参数
        /// </summary>
        private CalibrateRunPara CalibrateRunPara => CalibrateRunPara.GetInstance();

        /// <summary>
        /// 运行方法
        /// </summary>
        private CalibrateTask CalibrateTask => CalibrateTask.GetInstance();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 点胶测高模组
        /// </summary>
        private readonly MeasureHeightModule measureHeightModule = new MeasureHeightModule();

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        private DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 2;

        /// <summary>
        /// 点胶点
        /// </summary>
        private AKRSPoint3D dispenseMachinePos;

        /// <summary>
        /// bond点
        /// </summary>
        private AKRSPoint3D bondMachinePos;

        /// <summary>
        /// Initializes a new instance of the <see cref="FrmBondCalibTeach"/> class.
        /// </summary>
        public FrmDBCalibTeach()
        {
            this.InitializeComponent();
            this.TileBarTeach.SelectedItem = this.TbiBmcCenter;
            this.InitControl();
            this.SetUiControl(this.stepIndex);

            this.ucGuideMove2.ChangeModuleName("点胶模组", true);
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
            this.stepCount = 8;
            this.BtnDone.Visible = false;

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 相机BMC中心
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/{stepCount} :Position dispense cameras center over First mark.\r\n",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.dispenseMachinePos = DispenseController.GetAxisPos();

                                CalibrateRunPara.DispenseFirstPoint = this.dispenseMachinePos;

                                // 移动探针到测高位
                                this.calibController.MoveDispenseToMachinePos(CalibrateRunPara.DispenseFirstPoint + CalibrateRunPara.DispenseCameraToPinOffset);
                                this.calibController.MoveDispenseZToMachinePos(-20);

                                this.dispenseMeasureHeightController.OpenDispenseHeightMeasurementCylinder();

                                (ExcuteResult result, double height) = this.dispenseMeasureHeightController.HeightMeasurement(this.measureHeightModule.DispenseAltimemetrySensor);

                                if (result != ExcuteResult.Success)
                                {
                                    AKRSXtraMessageBox.Show("测高失败");
                                }

                                CalibrateRunPara.DispenseFirstPoint.Z = height;

                                this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();

                                this.TileBarTeach.SelectedItem = this.TbiToolCenter;
                            },
                       doneAction: () =>
                       {
                       }),
                    
                    // bond相机晶圆台左点
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"5/{stepCount} ：Position Bond cameras center over First mark.\r\n",
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
                                this.bondMachinePos = this.bondModuleController.Get3DRealPosition();

                                CalibrateRunPara.BondFirstPoint = this.bondMachinePos;

                                AKRSPoint3D bondPos = CalibrateRunPara.BondFirstPoint
                                                      + CalibrateRunPara.BondRotateCenterToCamOffset;

                                bondPos.Z = CalibrateRunPara.BMCMeasureHeightSearchMachinePos.Z + 3;
                                this.bondModuleController.MoveSafeBondXYZ(bondPos);

                                // 测高
                                (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(1, HeightMeasurementFunctionEnum.WithTDSensor);

                                if (res.Ret != ExcuteResult.Success)
                                {
                                    AKRSXtraMessageBox.Show("MeasureHeight fail");
                                }
 
                                bondPos.Z = res.HeightValue;

                                GeneralCoordinateSystem a = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "BondCoordinateSystem");

                                AKRSPoint3D bondAbs3D = a.SelfPosToG0(bondPos);

                                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem(
                                    "DispenseCoordinateSystem",
                                    "G0",
                                    true,
                                    CoordinateSystemTypeEnum.General);

                                GeneralCoordinateSystem b = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance()
                                    .CoordinateSystems.Find(it => it.Name == "DispenseCoordinateSystem");

                                b.CalibModule = TransformTool.CalibModuleEnum.RealisticCoordinateSystem;
                                b.Init(bondAbs3D, CalibrateRunPara.DispenseFirstPoint + CalibrateRunPara.DispenseCameraToPinOffset);
                                MachineCoordinateSystem.GetInstance().Save();
                            }),
                };

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
            this.EditPr("点胶bond模板");
        }

        public void EditPr(string name, AlgBeLongEnum algBeLong = AlgBeLongEnum.Calibration)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
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
        /// 参数赋值
        /// </summary>
        public void RefreshPara()
        {
            CalibrateRunPara.DispenseFirstPoint = this.dispenseMachinePos;
            CalibrateRunPara.BondFirstPoint = this.bondMachinePos;

            CalibrateRunPara.Save();
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
            this.RefreshPara();
        }
    }
}