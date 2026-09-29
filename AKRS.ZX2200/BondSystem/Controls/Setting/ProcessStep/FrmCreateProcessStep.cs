namespace AKRS.ZX2200.BondSystem.Controls.Setting.ProcessStep
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Main.Machine.Product.ProcessStep;

    /// <summary>
    /// 界面
    /// </summary>
    public partial class FrmCreateProcessStep : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public FrmCreateProcessStep()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// ProcessStep
        /// </summary>
        public ProcessStep NewProcessStep { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            List<ProcessStep> processStepList =
                ProcessStepProgram.GetInstance().ProcessStepList.ToList();

            processStepList.Insert(0, new ProcessStep() { Name = "No Process Step" });

            this.GcTool.DataSource = processStepList;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.TxtName.Text))
            {
                return;
            }

            ProcessStep processStep = this.GvEpoxyApplication.GetFocusedRow() as ProcessStep;

            if (processStep == null)
            {
                processStep = new ProcessStep();
            }

            this.NewProcessStep = JsonFormatHelper<ProcessStep>.DeepGenericCopy<ProcessStep>(processStep);
            this.NewProcessStep.Name = this.TxtName.Text;
            this.NewProcessStep.Order = ProcessStepProgram.GetInstance().GetMaxOrder() + 1;

            // this.NewProcessStep.AddBelongRecipeIds(MachineConfigContext.GetInstance().RecipeID);

            ProcessStepProgram.GetInstance().Add(this.NewProcessStep);

            ProcessStepProgram.GetInstance().Save();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}