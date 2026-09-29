using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;
using AKRS.Galaxy2.Communication.Exceptions;
using AKRS.Galaxy2.Component.Simple.MoveControl;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.PR.Controls.AlgControls;
using AKRS.Galaxy2.PR.Models.Algs;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using DevExpress.XtraEditors;

namespace AKRS.Galaxy2.PR.Controls
{
    /// <summary>
    /// PR 编辑窗体
    /// </summary>
    public partial class FrmPREditor : XtraForm
    {
        /// <summary>
        /// 加载模板
        /// </summary>
        public static Func<string, PREntity> LoadPREntityAction { get; set; }

        /// <summary>
        /// 重构PR动作
        /// </summary>
        private PREntity prEntity;

        /// <summary>
        /// 硬件配置对象
        /// </summary>
        public FrmHardwareSet HardwareSet { get; set; }

        /// <summary>
        /// 相机
        /// </summary>
        private AKRSCamera camera;

        /// <summary>
        /// 光源的GroupPanel集合
        /// </summary>
        private List<GroupControl> gpLightList;

        /// <summary>
        /// 亮度调整的TrackBar
        /// </summary>
        private List<TrackBarControl> trackBarIndensityList;

        /// <summary>
        /// 亮度显示的SpinEdit
        /// </summary>
        private List<SpinEdit> spIndensityList;

        /// <summary>
        /// 数据流的释放
        /// </summary>
        private IDisposable refreshImageDispose;

        /// <summary>
        /// 锁
        /// </summary>
        private object lockObj = new object();

        /// <summary>
        /// 方向盘
        /// </summary>
        private UcMoveControl ucMoveControl;

        /// <summary>
        /// 算法设置和交互界面
        /// </summary>
        private UcAlgEditor ucAlgEditor;

        /// <summary>
        /// 是否开启采图线程
        /// </summary>
        private bool isGrab = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmPREditor()
        {
            InitializeComponent();
            this.BringToFront();          
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="prEntity">PR实体</param>
        /// <param name="isStartGrab">是否需要弹出后立即显示采图</param>
        public FrmPREditor(PREntity prEntity, bool isStartGrab)
        {
            
            this.InitializeComponent();

            this.prEntity = prEntity;

            if (this.prEntity == null)
            {
                return;
            }

            RefreshInterface();

            Task.Run(RealTimeRefreshImg);
        }

        /// <summary>
        ///  采集的图片副本
        /// </summary>
        private Bitmap bitmapClone;

        /// <summary>
        /// 实时刷新图片
        /// </summary>
        private void RealTimeRefreshImg() 
        {
            while (isGrab) 
            {
                //if (this.ucMoveControl.IsMoving)
                //{
                //    Thread.Sleep(100);
                //    continue;
                //}

                if (TSContinuous.Checked && camera != null)
                {
                   Bitmap bitmap = camera.SnapShot(false, SnapImageFormat.Format8bppIndexed);
                   if (bitmap != null)
                   {
                       bitmapClone?.Dispose(); 
                       bitmapClone = (Bitmap)bitmap.Clone();
                       bitmap.Dispose();

                        //if (!this.ucMoveControl.IsMoving)
                        //{
                        //    this.ucAlgEditor.RefreshImg(bitmapClone);
                        //}

                        this.ucAlgEditor.RefreshImg(bitmapClone);
                    }
                }
                
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void RefreshInterface()
        {

            ucMoveControl = new UcMoveControl() { Dock = DockStyle.Fill };
            panelControl4.Controls.Add(ucMoveControl);

            // 如果PR光源列表里不包含任何元素， 那么这个PR是新建的 或者原来里面就没添加光源，那么在此处
            // 初始化，默认6个光源，这里初始化的指令千万不能移动到PRActionEntity 里的构造函数里 否则
            // 反序列化后 又会把这6个空光源加入到PRLightList 里造成光源数量是原来的两倍
            if (!this.prEntity.PRLightList.Any())
            {
                this.prEntity.PRLightList = new List<PRLight>()
                                           {
                                               new PRLight(),
                                               new PRLight(),
                                               new PRLight(),
                                               new PRLight(),
                                               new PRLight(),
                                               new PRLight()
                                           };
            }
            else
            {
                int tmp = 6 - this.prEntity.PRLightList.Count;
                for (int i = 0; i < tmp; i++)
                {
                    this.prEntity.PRLightList.Add(new PRLight());
                }
            }

            gpLightList = new List<GroupControl>(new GroupControl[]
                                                     {
                                                         GpLightSetting1, GpLightSetting2, GpLightSetting3,
                                                         GpLightSetting4, GpLightSetting5, GpLightSetting6
                                                     });

            trackBarIndensityList = new List<TrackBarControl>(new TrackBarControl[]
                                                                  {
                                                                      TrackBarIndensity1, TrackBarIndensity2, TrackBarIndensity3,
                                                                      TrackBarIndensity4, TrackBarIndensity5, TrackBarIndensity6
                                                                  });

            spIndensityList = new List<SpinEdit>(new SpinEdit[]
                                                     {
                                                         SpIndensity1, SpIndensity2, SpIndensity3,
                                                         SpIndensity4, SpIndensity5, SpIndensity6
                                                     });


            this.InitModel();
         
            this.HardwareSet = new FrmHardwareSet(this.prEntity);

            this.TrackBarExposure.Properties.Maximum = this.camera == null ? 0 : 60000;
            this.TrackBarExposure.Properties.Minimum = this.camera == null ? 0 : 1;
            this.TrackBarGain.Properties.Maximum = this.camera == null ? 0 : 20;
            this.TrackBarGain.Properties.Minimum = 0;
            this.TrackBarGamma.Properties.Maximum = this.camera == null ? 0 : 40;
            this.TrackBarGamma.Properties.Minimum = 0;

            this.TrackBarExposure.Value = this.camera == null ? 0 : (int)this.prEntity.Exposure;

            this.TSFlash.Checked = this.prEntity.IsFlash;

            this.TSisSaveImage.Checked = this.prEntity.IsSaveImage;

            if (this.camera != null)
            {
                this.camera.SetExposureTime(this.prEntity.Exposure);
            }

            this.TrackBarGain.Value = this.camera == null ? 0 : (int)this.prEntity.Gain;
            this.TrackBarGamma.Value = this.camera == null ? 0 : (int)this.prEntity.Gamma;


            if (this.camera != null)
            {
                this.camera.SetGain(this.prEntity.Gain);
            }

            if (this.camera != null)
            {
                this.camera.SetGamma(this.prEntity.Gamma / 10);
            }

            this.SpExposure.Value = this.camera == null ? 0 : (decimal)this.prEntity.Exposure;

            this.SpGain.Value = this.camera == null ? 0 : (decimal)this.prEntity.Gain;

            this.SpGamma.Value = this.camera == null ? 0 : (decimal)this.prEntity.Gamma;
        }


        /// <summary>
        /// 当前图片
        /// </summary>
        private Bitmap currentBmp;

        /// <summary>
        /// 相机采集图片后的刷新通知
        /// </summary>
        /// <param name="safeBmp">图片数据</param>
        private void RefreshNotification(SafeBitmap safeBmp)
        {
            if (safeBmp == null || this.IsDisposed)
            {
                safeBmp?.Dispose();
                return;
            }

            try
            {
                if (this.ucAlgEditor != null && this.ucAlgEditor.IsHandleCreated)
                {
                    this.ucAlgEditor.Invoke(
              new Action<SafeBitmap>(
                  (bitmap) =>
                  {
                      try
                      {
                          if (bitmap == null || !this.IsHandleCreated || this.IsDisposed)
                          {
                              bitmap?.Dispose();
                              return;
                          }

                          lock (this.lockObj)
                          {
                              Bitmap tempImage = bitmap?.GetBitmapClone();
                              if (tempImage == null)
                              {
                                  return;
                              }

                              this.currentBmp?.Dispose();
                              this.currentBmp = null;
                              this.currentBmp = tempImage?.Clone() as Bitmap;
                              tempImage?.Dispose();
                          }
                      }
                      catch
                      {
                      }
                  }));
                }
            }
            catch
            {

            }
        }


        /// <summary>
        /// 初始化模型
        /// </summary>
        private void InitModel()
        {
            // 绑定 
            this.BindingControl();

            this.BindingMoveControl();

            this.SuspendLayout();

            // 修改窗体标题
            this.Text = !string.IsNullOrWhiteSpace(this.prEntity.GetName()) ? this.prEntity.GetName() : "模板编辑窗口";

            if (prEntity.CameraName == string.Empty)
            {
                this.camera = new AKRSCamera();
            }
            else
            {
                this.camera = HardwareRepositoryService.GetHardware<AKRSCamera>(prEntity.CameraName);
            }

            // 刷新图像                         
            ActionBlock<SafeBitmap> actionBlock = new ActionBlock<SafeBitmap>(async safeBmp =>
            {
                this.RefreshNotification(safeBmp);
            });
             
            refreshImageDispose = this.prEntity.Camera?.ImageBlock.LinkTo(actionBlock);
            
            string name = prEntity.GetName();



            if (string.IsNullOrEmpty(name))
            {
                FrmName frmSavePR = new FrmName("新名字");

            ReNameLabel:
                DialogResult dr = frmSavePR.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Name = frmSavePR.CreateName;

                    DirectoryInfo directoryInfo = new DirectoryInfo(this.prEntity.Alg.GetPRSavePath() + Name);

                    if (directoryInfo.Exists)
                    {
                        XtraMessageBox.Show("模板已存在，请重命名！");
                        goto ReNameLabel;
                    }

                    prEntity.SetName(name);
                    prEntity.Alg = new FastModelAlg(Name);
                    frmSavePR.Dispose();
                }
                else
                {
                    return;
                }
            }

            this.PnlAlgSetting.Controls.Clear();
            if (this.ucAlgEditor == null)
            {
                this.ucAlgEditor = new UcAlgEditor(this.prEntity.Alg) { Dock = DockStyle.Fill };
            }         
            
            this.PnlAlgSetting.Controls.Add(this.ucAlgEditor);
            this.ResumeLayout();
        }

        /// <summary>
        ///  将PREntity保存
        /// </summary>
        /// <param name="oldName">PREntity对象的旧名字</param>
        /// <param name="newName">PREntity对象的新名字</param>
        /// <returns>是否保存成功</returns>
        public bool IsSaveSuccess(string oldName, string newName)
        {
            try
            {
                prEntity.ChangeFilePath(oldName, newName);
                this.prEntity.Alg = this.ucAlgEditor.GetBaseAlg();
                this.prEntity.Alg.Name = newName;
                return true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"模板保存失败{ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">参数</param>
        private void BarBtSaveTemplate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 因为此模板被修改过所以后面都要重新复制才能进行定位
            this.prEntity.IsSystem1Copy=false;

            // 光源参数保存
            for (int i = 0; i < this.prEntity.PRLightList.Count; i++)
            {
                 this.prEntity.PRLightList[i].IsUse = gpLightList[i].Visible;

                if (this.prEntity.PRLightList[i].LightName != null)
                {
                    Light light = HardwareRepositoryService.GetHardware<Light>(this.prEntity.PRLightList[i].LightName);
                    this.prEntity.PRLightList[i].Light = light;
                }
              
                // 绑定光源亮度
                if (this.prEntity.PRLightList[i].IsUse)
                {
                   this.prEntity.PRLightList[i].LightIntensity = trackBarIndensityList[i].Value;
                }
            }

            this.prEntity.Exposure = this.TrackBarExposure.Value;
            this.prEntity.Gain = this.TrackBarGain.Value;
            this.prEntity.Gamma = this.TrackBarGamma.Value;

            this.prEntity.IsFlash = this.TSFlash.Checked;
            this.prEntity.IsSaveImage=this.TSisSaveImage.Checked;
            string oldName = prEntity.GetName();

            bool isSuccess1 = this.IsSaveSuccess(oldName, oldName);
            bool isSuccess2 = this.ucAlgEditor.GetBaseAlg().SaveProcedure("");
            if (isSuccess1 && isSuccess2)
            {
                DialogResult dialog = XtraMessageBox.Show(
               $"保存成功",
               "提示",
               MessageBoxButtons.OK,
               MessageBoxIcon.Information);

                if (dialog == DialogResult.OK)
                {
                    VisionEntityRepository.GetInstance().Save();
                }
            }
            else
            {
                XtraMessageBox.Show("保存失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">数据封装</param>
        private void PREditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isGrab = false;
            this.TSContinuous.Checked = false;
            this.prEntity.Alg = this.ucAlgEditor.GetBaseAlg();
            VisionEntityRepository.GetInstance().Save();
            this.refreshImageDispose?.Dispose();
            this.ucAlgEditor?.Dispose();   
            this.ucMoveControl?.Dispose();
            this.bitmapClone?.Dispose();
            this.HardwareSet?.Dispose();
        }

        /// <summary>
        /// 控件绑定
        /// </summary>
        private void BindingControl()
        {
            // 光源绑定
            for (int i = 0; i < this.prEntity.PRLightList.Count; i++)
            {
                gpLightList[i].Visible = this.prEntity.PRLightList[i].IsUse;
                if (this.prEntity.PRLightList[i].LightName != null)
                {
                    Light light = HardwareRepositoryService.GetHardware<Light>(this.prEntity.PRLightList[i].LightName);
                    this.prEntity.PRLightList[i].Light = light;
                }         
                if (this.prEntity.PRLightList[i].Light != null)
                {
                    gpLightList[i].DataBindings.Clear();
                    gpLightList[i].DataBindings.Add(
                        "Text",
                        this.prEntity.PRLightList[i].Light,
                        "HardwareName",
                        false,
                        DataSourceUpdateMode.OnPropertyChanged);
                }
              
                    // 绑定光源亮度
                    if (this.prEntity.PRLightList[i].IsUse)
                    {
                        Light light = HardwareRepositoryService.GetHardware<Light>(this.prEntity.PRLightList[i].LightName);
                        trackBarIndensityList[i].Tag = this.prEntity.PRLightList[i].Light;
                        trackBarIndensityList[i].EditValue = this.prEntity.PRLightList[i].LightIntensity;
                        spIndensityList[i].EditValue = trackBarIndensityList[i].Value;
                    }               
            }
        }

        /// <summary>
        /// 绑定方向盘
        /// </summary>
        private void BindingMoveControl()
        {
            AxisConfig axisConfig = new AxisConfig(this.prEntity.AxisX, this.prEntity.AxisY, this.prEntity.AxisZ, null, null, null);
            ucMoveControl.Init(axisConfig, "FrmPREditor");
        }

        /// <summary>
        /// 设置曝光
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackbarExposure_EditValueChanged(object sender, EventArgs e)
        {
            if (this.camera != null)
            {
                this.camera.SetExposureTime(TrackBarExposure.Value);
                this.SpExposure.EditValue=TrackBarExposure.Value;
            }
        }

        /// <summary>
        /// 光源调节
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackBarIndensity_EditValueChanged(object sender, EventArgs e)
        {
            TrackBarControl trackBarLight = sender as TrackBarControl;
            Light light = trackBarLight?.Tag as Light;
            if (light == null)
            {
                return;
            }

            try
            {

                light.SetIntensity(trackBarLight.Value);
                int index = this.prEntity.PRLightList.FindIndex(obj => obj.Light == light);
                spIndensityList[index].EditValue = trackBarIndensityList[index].Value;
            }
            catch (CommunicationException cex)
            {
                XtraMessageBox.Show($"{cex.ToShowText()}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ArgumentNullException aex)
            {
                XtraMessageBox.Show("光源控制器驱动为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 硬件设置
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">参数封装</param>
        private void BarBtHardwareSet_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (HardwareSet.ShowDialog() == DialogResult.OK)
            {
                InitModel();

                // 重新绑定硬件 赋值硬件参数
                this.prEntity.Exposure = 3000;
                this.prEntity.Gain = 1;
                this.prEntity.Gamma = 10;
               

                for (int i = 0; i < 6; i++)
                {
                    // 绑定光源亮度
                    if (this.prEntity.PRLightList[i].IsUse)
                    {         
                        Light light = HardwareRepositoryService.GetHardware<Light>(this.prEntity.PRLightList[i].LightName);
                        trackBarIndensityList[i].EditValue = light.GetIntensity();
                        spIndensityList[i].EditValue = trackBarIndensityList[i].EditValue;
                    }
                }

                this.TrackBarExposure.Properties.Maximum = this.camera == null ? 0 : 60000;
                this.TrackBarExposure.Properties.Minimum = this.camera == null ? 0 : 1;
                this.TrackBarGain.Properties.Maximum = this.camera == null ? 0 : 20;
                this.TrackBarGain.Properties.Minimum = this.camera == null ? 0 : 0;
                this.TrackBarGamma.Properties.Maximum = this.camera == null ? 0 : 40;
                this.TrackBarGamma.Properties.Minimum = this.camera == null ? 0 : 0;

                this.TrackBarExposure.Value = this.camera == null ? 0 : (int)this.prEntity.Exposure;
                this.TrackBarGain.Value = this.camera == null ? 0 : (int)this.prEntity.Gain;
                this.TrackBarGamma.Value = this.camera == null ? 0 : (int)this.prEntity.Gamma;
                this.SpExposure.Value = this.camera == null ? 0 : (decimal)this.prEntity.Exposure;
                this.SpGain.Value = this.camera == null ? 0 : (decimal)this.prEntity.Gain;
                this.SpGamma.Value = this.camera == null ? 0 : (decimal)this.prEntity.Gamma;
            }
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void TrackBarGain_EditValueChanged(object sender, EventArgs e)
        {
            this.camera?.SetGain(TrackBarGain.Value);
            this.SpGain.EditValue = TrackBarGain.Value;
        }

        private void TrackBarGamma_EditValueChanged(object sender, EventArgs e)
        {
            double gamma = (double)TrackBarGamma.Value / 10;
            this.camera?.SetGamma(gamma);
            this.SpGamma.EditValue = TrackBarGamma.Value;
        }
    }
}
      