using System;
using System.Collections.Generic;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.CalibSystem.Models;

namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using System.Windows.Forms;

    /// <summary>
    /// 精确标定Bond相机到Bond偏移值
    /// </summary>
    public partial class FrmBondToCameraOffset : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// PR界面
        /// </summary>
        private FrmPRList frmPRList;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmBondToCameraOffset()
        {
            this.InitializeComponent();
            this.panel1.AutoScroll = true;

            this.TileBarTeach.SelectedItem = this.TbiDispenseCenter;
            this.InitControl();
            this.SetUiControl(this.stepIndex);

            if (CalibrateRunPara.GetInstance().BondCamUpLookCamMachinePos != null)
            {
                this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().BondCamUpLookCamMachinePos);
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
            this.stepCount = 2;
            this.BtnDone.Visible = false;

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 拉角度第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/{stepCount} :Position substrate camera over circle on UpLook camera center calibration mark.\r\n",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                        {
                        },
                        nextAction: () => 
                            {
                            },
                       doneAction: () =>
                           {
                               AKRSPoint3D curBondPos = this.bondModuleController.Get3DRealPosition();
                               CalibrateRunPara.GetInstance().BondCamUpLookCamMachinePos = curBondPos;

                               CalibrateTask.GetInstance().StartBondToCamBySameMarkTask();

                               CalibrateRunPara.GetInstance().Save();
                               this.TileBarTeach.SelectedItem = this.TbiDispenseCenter;
                       }),
                };

            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
            UcGuideMove ucGuideMove = new UcGuideMove("精确标定Bond相机到Bond偏移值");
            ucGuideMove.Dock = DockStyle.Fill;
            this.panelControl4.Controls.Add(ucGuideMove);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtnDone.Visible = assistantConfig.IsShowDone;
        }

        /// <summary>
        /// 模版匹配
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnPattern_Click(object sender, EventArgs e)
        {
            this.EditPr("BondCamUpLookCamAlign-Bond");
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