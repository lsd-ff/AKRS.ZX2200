namespace AKRS.Galaxy2.Dispense
{
	partial class EpoxyPatternControl
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region 组件设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.tsmiExitAction = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCancel = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMovePoint = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiRotateBackgroundImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTranslateBackgroundImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiResetBackgroundImage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorBetweenBasicAndCurve = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiAddCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAppendPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiInsertPoint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiErasePoint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorAfterAction = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiTranslateCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScaleCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRotateCurve = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorBeforeExitAndCancel = new System.Windows.Forms.ToolStripSeparator();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsmiExitAction
            // 
            this.tsmiExitAction.Name = "tsmiExitAction";
            this.tsmiExitAction.Size = new System.Drawing.Size(148, 22);
            this.tsmiExitAction.Tag = "exit_action";
            this.tsmiExitAction.Text = "结束动作";
            this.tsmiExitAction.Click += new System.EventHandler(this.tsmiExitAction_Click);
            // 
            // tsmiCancel
            // 
            this.tsmiCancel.Name = "tsmiCancel";
            this.tsmiCancel.Size = new System.Drawing.Size(148, 22);
            this.tsmiCancel.Tag = "cancel";
            this.tsmiCancel.Text = "取消";
            this.tsmiCancel.Click += new System.EventHandler(this.tsmiCancel_Click);
            // 
            // tsmiMovePoint
            // 
            this.tsmiMovePoint.Name = "tsmiMovePoint";
            this.tsmiMovePoint.Size = new System.Drawing.Size(148, 22);
            this.tsmiMovePoint.Tag = "action_move_point";
            this.tsmiMovePoint.Text = "移动点";
            this.tsmiMovePoint.Click += new System.EventHandler(this.tsmiMovePoint_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiRotateBackgroundImage,
            this.tsmiTranslateBackgroundImage,
            this.tsmiResetBackgroundImage,
            this.toolStripSeparatorBetweenBasicAndCurve,
            this.tsmiAddCurve,
            this.tsmiAppendPoint,
            this.tsmiInsertPoint,
            this.tsmiMovePoint,
            this.tsmiErasePoint,
            this.toolStripSeparatorAfterAction,
            this.tsmiTranslateCurve,
            this.tsmiScaleCurve,
            this.tsmiRotateCurve,
            this.toolStripSeparatorBeforeExitAndCancel,
            this.tsmiExitAction,
            this.tsmiCancel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 308);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.MouseLeave += new System.EventHandler(this.contextMenuStrip1_MouseLeave);
            // 
            // tsmiRotateBackgroundImage
            // 
            this.tsmiRotateBackgroundImage.Name = "tsmiRotateBackgroundImage";
            this.tsmiRotateBackgroundImage.Size = new System.Drawing.Size(148, 22);
            this.tsmiRotateBackgroundImage.Tag = "basic_action_rotate_background_image";
            this.tsmiRotateBackgroundImage.Text = "旋转背景图片";
            this.tsmiRotateBackgroundImage.Click += new System.EventHandler(this.tsmiRotateBackgroundImage_Click);
            // 
            // tsmiTranslateBackgroundImage
            // 
            this.tsmiTranslateBackgroundImage.Name = "tsmiTranslateBackgroundImage";
            this.tsmiTranslateBackgroundImage.Size = new System.Drawing.Size(148, 22);
            this.tsmiTranslateBackgroundImage.Tag = "basic_action_translate_background_image";
            this.tsmiTranslateBackgroundImage.Text = "平移背景图片";
            this.tsmiTranslateBackgroundImage.Click += new System.EventHandler(this.tsmiTranslateBackgroundImage_Click);
            // 
            // tsmiResetBackgroundImage
            // 
            this.tsmiResetBackgroundImage.Name = "tsmiResetBackgroundImage";
            this.tsmiResetBackgroundImage.Size = new System.Drawing.Size(148, 22);
            this.tsmiResetBackgroundImage.Tag = "basic_action_reset_background_image";
            this.tsmiResetBackgroundImage.Text = "重置背景图片";
            this.tsmiResetBackgroundImage.Click += new System.EventHandler(this.tsmiResetBackgroundImageAngle_Click);
            // 
            // toolStripSeparatorBetweenBasicAndCurve
            // 
            this.toolStripSeparatorBetweenBasicAndCurve.Name = "toolStripSeparatorBetweenBasicAndCurve";
            this.toolStripSeparatorBetweenBasicAndCurve.Size = new System.Drawing.Size(145, 6);
            this.toolStripSeparatorBetweenBasicAndCurve.Tag = "separator_between_basic_and_curve";
            // 
            // tsmiAddCurve
            // 
            this.tsmiAddCurve.Name = "tsmiAddCurve";
            this.tsmiAddCurve.Size = new System.Drawing.Size(148, 22);
            this.tsmiAddCurve.Tag = "action_add_curve";
            this.tsmiAddCurve.Text = "添加曲线";
            this.tsmiAddCurve.Click += new System.EventHandler(this.tsmiAddCurve_Click);
            // 
            // tsmiAppendPoint
            // 
            this.tsmiAppendPoint.Name = "tsmiAppendPoint";
            this.tsmiAppendPoint.Size = new System.Drawing.Size(148, 22);
            this.tsmiAppendPoint.Tag = "action_append_point";
            this.tsmiAppendPoint.Text = "追加点";
            this.tsmiAppendPoint.Click += new System.EventHandler(this.tsmiAppendPoint_Click);
            // 
            // tsmiInsertPoint
            // 
            this.tsmiInsertPoint.Name = "tsmiInsertPoint";
            this.tsmiInsertPoint.Size = new System.Drawing.Size(148, 22);
            this.tsmiInsertPoint.Tag = "action_insert_point";
            this.tsmiInsertPoint.Text = "插入点";
            this.tsmiInsertPoint.Click += new System.EventHandler(this.tsmiInsertPoint_Click);
            // 
            // tsmiErasePoint
            // 
            this.tsmiErasePoint.Name = "tsmiErasePoint";
            this.tsmiErasePoint.Size = new System.Drawing.Size(148, 22);
            this.tsmiErasePoint.Tag = "action_erase_point";
            this.tsmiErasePoint.Text = "删除点";
            this.tsmiErasePoint.Click += new System.EventHandler(this.tsmiErasePoint_Click);
            // 
            // toolStripSeparatorAfterAction
            // 
            this.toolStripSeparatorAfterAction.Name = "toolStripSeparatorAfterAction";
            this.toolStripSeparatorAfterAction.Size = new System.Drawing.Size(145, 6);
            this.toolStripSeparatorAfterAction.Tag = "separator_embedded_in_curve_actions";
            // 
            // tsmiTranslateCurve
            // 
            this.tsmiTranslateCurve.Name = "tsmiTranslateCurve";
            this.tsmiTranslateCurve.Size = new System.Drawing.Size(148, 22);
            this.tsmiTranslateCurve.Tag = "translate_curve";
            this.tsmiTranslateCurve.Text = "平移曲线";
            this.tsmiTranslateCurve.Click += new System.EventHandler(this.tsmiTranslateCurve_Click);
            // 
            // tsmiScaleCurve
            // 
            this.tsmiScaleCurve.Name = "tsmiScaleCurve";
            this.tsmiScaleCurve.Size = new System.Drawing.Size(148, 22);
            this.tsmiScaleCurve.Tag = "scale_curve";
            this.tsmiScaleCurve.Text = "缩放曲线";
            this.tsmiScaleCurve.Click += new System.EventHandler(this.tsmiScaleCurve_Click);
            // 
            // tsmiRotateCurve
            // 
            this.tsmiRotateCurve.Name = "tsmiRotateCurve";
            this.tsmiRotateCurve.Size = new System.Drawing.Size(148, 22);
            this.tsmiRotateCurve.Tag = "rotate_curve";
            this.tsmiRotateCurve.Text = "旋转曲线";
            this.tsmiRotateCurve.Click += new System.EventHandler(this.tsmiRotateCurve_Click);
            // 
            // toolStripSeparatorBeforeExitAndCancel
            // 
            this.toolStripSeparatorBeforeExitAndCancel.Name = "toolStripSeparatorBeforeExitAndCancel";
            this.toolStripSeparatorBeforeExitAndCancel.Size = new System.Drawing.Size(145, 6);
            // 
            // EpoxyPatternControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.DoubleBuffered = true;
            this.Name = "EpoxyPatternControl";
            this.Size = new System.Drawing.Size(506, 506);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}


        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAfterAction;
        private System.Windows.Forms.ToolStripMenuItem tsmiInsertPoint;
        private System.Windows.Forms.ToolStripMenuItem tsmiAppendPoint;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddCurve;
        private System.Windows.Forms.ToolStripMenuItem tsmiScaleCurve;
        private System.Windows.Forms.ToolStripMenuItem tsmiRotateCurve;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorBeforeExitAndCancel;
        private System.Windows.Forms.ToolStripMenuItem tsmiTranslateCurve;
        private System.Windows.Forms.ToolStripMenuItem tsmiExitAction;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancel;
        private System.Windows.Forms.ToolStripMenuItem tsmiMovePoint;
        private System.Windows.Forms.ToolStripMenuItem tsmiRotateBackgroundImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiTranslateBackgroundImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiResetBackgroundImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiErasePoint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorBetweenBasicAndCurve;
    }
}
