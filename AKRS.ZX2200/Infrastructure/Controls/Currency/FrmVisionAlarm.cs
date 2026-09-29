namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using AKRS.Galaxy2.Component.Simple.MoveControl;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using DevExpress.XtraEditors;
    using LanguageExt;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// 定位失败报警提示窗体
    /// </summary>
    public partial class FrmVisionAlarm : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  报警弹窗
        /// </summary>
        /// <param name="pREntity">视觉实体</param>
        /// <param name="message">报警信息</param>
        /// <param name="caption">标题</param>
        public FrmVisionAlarm(PREntity pREntity, string message, string caption,PRHardware pRHardware = null)
        {
            this.PRHardware = pRHardware;
            this.InitializeComponent();
            this.entity = pREntity;
            this.SetAxis();
            this.alarmMessage = message;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.caption = caption;
            this.task = Task.Run(() => this.BackgroundWorkerDoWork());
        }

        private Task task;

        /// <summary>
        /// 是否关闭
        /// </summary>
        private bool isclose = false;

        private UcMoveControl ucMoveControl;

        /// <summary>
        /// 视觉硬件
        /// </summary>
        private PRHardware PRHardware;

        /// <summary>
        /// 报警标题
        /// </summary>
        private readonly string caption;

        /// <summary>
        /// 报警信息
        /// </summary>
        private readonly string alarmMessage;

        /// <summary>
        /// 是否正在工作
        /// </summary>
        private bool isWork = false;

        /// <summary>
        /// 是否正在工作
        /// </summary>
        private bool isAllowWork = false;

        /// <summary>
        /// pr实体
        /// </summary>
        private readonly PREntity entity;

        /// <summary>
        /// 最后定位完成的结果
        /// </summary>
        public BaseAlgResult BaseAlgResult { get; set; }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void TimerVision_Tick(object sender, EventArgs e)
        {
            if (this.isWork)
            {
                return;
            }
            else
            {
                this.isWork = true;
            }

            if (this.PRHardware != null)
            {
                lock (this.entity)
                {
                    this.entity.DoWork(PRHardware,false, false);
                    this.BitmapShow();
                }
            }
            else
            {
                this.entity.DoWork(PRHardware,false, false);
                this.BitmapShow();
            }

            this.isWork = false;
        }

        /// <summary>
        /// 后台工作线程，持续采集图像
        /// </summary>
        private void BackgroundWorkerDoWork()
        {
            CommonUtil.SetCurrentThreadName("定位失败实时显示线程");

            try
            {
                while (!this.isclose)
                {
                    if (this.isAllowWork)
                    {
                        this.isWork = true;

                        if (this.entity == null)
                        {
                            AKRSCamera aKRSCamera = HardwareRepositoryService.GetHardware<AKRSCamera>(this.PRHardware.CameraName);
                            Bitmap bitmap = aKRSCamera.SnapShot(false, SnapImageFormat.Format8bppIndexed);

                            this.BeginInvoke(() =>
                            {
                                this.pictureEdit1.Image?.Dispose();
                                this.pictureEdit1.Image = bitmap;
                            });
                        }
                        else
                        {
                            if (this.PRHardware != null)
                            {
                                lock (this.entity)
                                {
                                    this.entity.DoWork(PRHardware, false, false);
                                    this.BitmapShow();
                                }
                            }
                            else
                            {
                                this.entity.DoWork(PRHardware, false, false);
                                this.BitmapShow();
                            }
                        }

                        this.isWork = false;
                    }

                    Thread.Sleep(300);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("定位失败实时显示线程异常" + ex.Message);
            }
        }

        /// <summary>
        /// 图片显示
        /// </summary>
        private void BitmapShow()
        {
            if (this.ucMoveControl.IsMoving || !this.isAllowWork)
            {
                this.entity.AlgResult?.OutPutImg1?.Dispose();
                return;
            }

            this.BeginInvoke(() => 
            {
                if (this.entity != null && this.entity.AlgResult != null
                                          && this.entity.AlgResult.OutPutImg1 != null)
                {
                    if (this.isAllowWork)
                    {
                        this.pictureEdit1.Image?.Dispose();

                        this.pictureEdit1.Image = this.entity.AlgResult.OutPutImg1;
                    }
                }
            });
        }


        /// <summary>
        /// 设置硬件
        /// </summary>
        private void SetAxis()
        {
            if (this.entity != null)
            {
                if (this.PRHardware != null)
                {
                    Axis axisX = HardwareRepositoryService.GetHardware<Axis>(this.PRHardware.AxisXName);
                    Axis axisY = HardwareRepositoryService.GetHardware<Axis>(this.PRHardware.AxisYName);
                    Axis axisZ = HardwareRepositoryService.GetHardware<Axis>(this.PRHardware.AxisZName);
                    AxisConfig axisConfig = new AxisConfig(axisX, axisY, axisZ, null, null, null);

                    this.ucMoveControl = new UcMoveControl(axisConfig, "FrmVisionAlarm");

                    ucMoveControl.Dock = DockStyle.Fill;

                    this.panelControl1.Controls.Add(ucMoveControl);

                    ucMoveControl.SetMoveSpeed(1);
                }
                else
                {
                    Axis axisX = HardwareRepositoryService.GetHardware<Axis>(this.entity.AxisXName);
                    Axis axisY = HardwareRepositoryService.GetHardware<Axis>(this.entity.AxisYName);
                    Axis axisZ = HardwareRepositoryService.GetHardware<Axis>(this.entity.AxisZName);
                    AxisConfig axisConfig = new AxisConfig(axisX, axisY, axisZ, null, null, null);
                    this.ucMoveControl = new UcMoveControl(axisConfig, "FrmVisionAlarm");
                    ucMoveControl.Dock = DockStyle.Fill;

                    this.panelControl1.Controls.Add(ucMoveControl);

                    ucMoveControl.SetMoveSpeed(1);
                }

            }
            else
            {
                Axis axisX = HardwareRepositoryService.GetHardware<Axis>(this.PRHardware.AxisXName);
                Axis axisY = HardwareRepositoryService.GetHardware<Axis>(this.PRHardware.AxisYName);
                Axis axisZ = HardwareRepositoryService.GetHardware<Axis>(this.PRHardware.AxisZName);
                AxisConfig axisConfig = new AxisConfig(axisX, axisY, axisZ, null, null, null);

                this.ucMoveControl = new UcMoveControl(axisConfig, "FrmVisionAlarm");

                ucMoveControl.Dock = DockStyle.Fill;

                this.panelControl1.Controls.Add(ucMoveControl);

                ucMoveControl.SetMoveSpeed(1);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmVisionAlarm_Load(object sender, EventArgs e)
        {
            this.Text = this.caption;

            if (this.entity == null)
            {
                this.BtEditProgram.Enabled = false;
                this.BtReDispense.Enabled = false;
            }

            this.labelControl1.Text = this.alarmMessage;

            this.Text = this.entity?.GetName();

            this.Alarm();

            if (this.entity == null)
            {
                this.isAllowWork = true;
                return;
            }

            // 胶量检测归属PostBond,胶印检测归属于Substrate
            if (this.entity.Alg.AlgFlowType == AlgFlowTypeEnum.EpoxyDetectAlg)
            {
                if (this.entity.Alg.AlgBeLong == AlgBeLongEnum.Substrate)
                {
                    this.BtReDispense.Visible = false;
                    this.BtReDispense.Enabled = false;
                }
                else
                {
                    this.BtReDispense.Visible = true;
                }
            }
            else if (this.entity.CameraName == "晶圆相机")
            {
                if (this.caption == "晶圆线程")
                {
                    this.BtOK.Text = "OK";
                    this.BtSkip.Visible = false;
                    this.BtAbort.Visible = false;
                    this.BtReDispense.Visible = true;
                    this.BtReDispense.Text = "Retry";
                }
                else
                {
                    this.BtReDispense.Visible = true;
                    this.BtReDispense.Text = "提前换料";
                }
            }
            else if (this.entity.CameraName == "上视相机")
            {
                this.BtReDispense.Visible = true;
                this.BtReDispense.Text = "下一颗";
                this.BtSkip.Visible = false;
            }

            this.isAllowWork = true;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtOK_Click(object sender, EventArgs e)
        {
            this.isAllowWork = false;

            // 如果正在工作则等待工作完成
            while (this.isWork)
            {
                Thread.Sleep(100);
            }

            // 如果没有PR实体直接返回成功
            if (this.entity == null)
            {
                this.DialogResult = DialogResult.OK;
                return;
            }

            // 双模板跳过
            if (this.entity.Alg.AlgFlowType == AlgFlowTypeEnum.SymmetricModeleAlg || this.caption == "晶圆线程")
            {
                this.DialogResult = DialogResult.OK;
                return;
            }

            // 胶量检测之后返回结果，点胶OK就是认为可以
            if (this.entity.Alg.AlgFlowType == AlgFlowTypeEnum.EpoxyDetectAlg)
            {
                this.DialogResult = DialogResult.OK;
                return;
            }

            lock (this.entity)
            {
                ExcuteResult result = this.entity.DoWork(this.PRHardware,false, true);

                if (result != ExcuteResult.Success)
                {
                    AKRSXtraMessageBox.Show("没有找到模板，请重试");
                    this.isAllowWork = true;
                }
                else
                {
                    if (this.entity.AlgResult is MatchResult match)
                    {
                        this.BaseAlgResult = new MatchResult(match.CenterX, match.CenterY, match.Angle);
                    }
                    else
                    {
                        this.BaseAlgResult = this.entity.AlgResult;
                    }


                    //// 保存结果图片
                    //Bitmap bitmap = this.entity.AlgResult.OutPutImg1;
                    //string path = "D:\\VisionBitmapData\\" + DateTime.Now.ToString("ddhhmmssfff") + this.entity.GetName() + ".bmp";

                    this.DialogResult = DialogResult.OK;

                    // 如果是胶量检测则提示
                    if (this.entity.Alg.AlgFlowType == AlgFlowTypeEnum.EpoxyDetectAlg)
                    {
                        DialogResult dialogResult = AKRSXtraMessageBox.Show("是否暂停设备", "提示", MessageBoxButtons.OKCancel);
                        if (dialogResult == DialogResult.OK)
                        {
                            Machine.GetInstance().Pause();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtAbort_Click(object sender, EventArgs e)
        {
            Machine.GetInstance().Stop();
            this.DialogResult = DialogResult.Abort;
        }

        /// <summary>
        /// 设置PR
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtEditProgram_Click(object sender, EventArgs e)
        {
            this.isAllowWork = false;

            // 如果正在工作则等待工作完成
            while (this.isWork)
            {
                Thread.Sleep(100);
            }

            Machine.GetInstance().Pause();

            Thread.Sleep(500);

            if (this.PRHardware != null)
            {
                lock (this.entity)
                {
                    if (this.entity.Camera.HardwareName == "点胶相机")
                    {
                        List<Light> lights = System1Domain.GetInstance().DispenseVisionController.GetLights();
                        List<int> intensities = this.entity.PRLightList.Where(a => a.IsUse).Select(a => a.LightIntensity).ToList();
                        List<int> bondIntensities = LightCalibrationPara.GetInstance().ApplyLightMapping(lights, intensities);

                        int index = 0;
                        foreach (PRLight light in this.entity.PRLightList)
                        {
                            if (light.IsUse)
                            {
                                light.LightIntensity = bondIntensities[index];
                                index++;
                            }
                        }
                    }

                    this.entity.SetHardware(this.PRHardware);
                    FrmPREditor editor = new FrmPREditor((PREntity)this.entity, false);
                    editor.ShowDialog();
                    editor.Dispose();

                    if (this.entity.Camera.HardwareName == "点胶相机")
                    {
                        List<Light> lights = System1Domain.GetInstance().DispenseVisionController.GetLights();
                        List<int> intensities = this.entity.PRLightList.Where(a => a.IsUse).Select(a => a.LightIntensity).ToList();
                        List<int> bondIntensities = LightCalibrationPara.GetInstance().ApplyLightMappingReverse(lights, intensities);

                        int index = 0;
                        foreach (PRLight light in this.entity.PRLightList)
                        {
                            if (light.IsUse)
                            {
                                light.LightIntensity = bondIntensities[index];
                                index++;
                            }
                        }
                    }
                }

            }
            else
            {
                FrmPREditor editor = new FrmPREditor((PREntity)this.entity, false);
                editor.ShowDialog();
                editor.Dispose();
            }


            VisionEntityRepository.GetInstance().Save();

            Machine.GetInstance().Continue();

            this.isAllowWork = true;
        }

        /// <summary>
        /// 跳过
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSkip_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Ignore;
        }

        /// <summary>
        /// 退出的时候关闭计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmVisionAlarm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.CloseAlarm();
            while (this.task.Status == TaskStatus.Running)
            {
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 报警
        /// </summary>
        private void Alarm()
        {
            List<Alarmer> alarm = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
            if (alarm == null || !alarm.Any<Alarmer>())
            {
                return;
            }

            alarm[0].SetSecondLevelAlarm();
        }

        /// <summary>
        /// 关闭报警
        /// </summary>
        private void CloseAlarm()
        {
            List<Alarmer> alarm = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
            if (alarm == null || !alarm.Any<Alarmer>())
            {
                return;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmVisionAlarm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.isclose = true;
        }

        /// <summary>
        /// 补胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtReDispense_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
        }
    }
}