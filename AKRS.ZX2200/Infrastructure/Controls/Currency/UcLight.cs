namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using System;
    using System.Windows.Forms;

    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;

    using DevExpress.Utils.Extensions;

    /// <summary>
    /// 光源的uc空间
    /// </summary>
    public partial class UcLight : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 光源空间无参构造方法
        /// </summary>
        public UcLight()
        {
            this.InitializeComponent();
            // this.Enabled = false;
        }

        /// <summary>
        /// 改变灯光的时间
        /// </summary>
        private DateTime changeLightTime = DateTime.Now;

        /// <summary>
        /// 改变灯光，100ms一次
        /// </summary>
        private int changeLightDelay = 100; 

        /// <summary>
        /// 光强度
        /// </summary>
        private int intensity;

        /// <summary>
        /// 红光
        /// </summary>
        public Light lightRed;

        /// <summary>
        /// 绿光
        /// </summary>
        public Light lightGreed;

        /// <summary>
        /// 蓝光
        /// </summary>
        public Light lightBlue;

        /// <summary>
        /// 光的颜色
        /// </summary>
        private LightColorTypeEnum lightColorType;

       /// <summary>
       /// 初始化
       /// </summary>
       /// <param name="lightName">光源名字</param>
       /// <param name="lightRedName">红光</param>
       /// <param name="lightGreedName">绿光</param>
       /// <param name="lightBlueName">蓝光</param>
        public void Init(string lightName, string lightRedName, string lightGreedName, string lightBlueName)
        {
            this.GpLightName.Text = lightName;

            // 初始化名称
            this.lightRed = HardwareRepositoryService.GetHardware<Light>(lightRedName);

            this.lightGreed = HardwareRepositoryService.GetHardware<Light>(lightGreedName);

            this.lightBlue = HardwareRepositoryService.GetHardware<Light>(lightBlueName);

            if (this.lightRed == null && this.lightBlue == null && this.lightGreed == null)
            {
                this.Enabled = false;
                return;
            }

            if (this.lightRed == null || this.lightBlue == null || this.lightGreed == null)
            {
                this.imageComboBoxEdit1.SelectedIndex = 7;
                this.imageComboBoxEdit1.Enabled = false;
            }
        }
        
        /// <summary>
        /// 判断当前是什么颜色
        /// </summary>
        private void CorrectLightColorTypeAndIntensity()
        {
            // 获取到当前的亮度数值比例
            int red = this.lightRed.GetIntensity() * 100 / 255;
            int greed = this.lightGreed.GetIntensity() * 100/ 255;
            int blue = this.lightBlue.GetIntensity() * 100 / 255;


            // 获取光的强度
            this.intensity = Math.Max(red, Math.Max(greed,blue));

            // this.intensity = (red + greed + blue) / 3;

           

            // 判断类型 
            if (red == greed && greed == blue)
            {
                this.lightColorType = LightColorTypeEnum.White;
                this.imageComboBoxEdit1.SelectedIndex = 6;
            }
            else if (greed == 0 && blue == 0 && red != 0)
            {
                this.lightColorType = LightColorTypeEnum.Red;
                this.imageComboBoxEdit1.SelectedIndex = 0;
            }
            else if (red == 0 && blue == 0 && greed != 0)
            {
                this.lightColorType = LightColorTypeEnum.Green;
                this.imageComboBoxEdit1.SelectedIndex = 2;
            }
            else if (greed == 0 && red == 0 && blue != 0)
            {
                this.lightColorType = LightColorTypeEnum.Blue;
                this.imageComboBoxEdit1.SelectedIndex = 1;
            }
            else if (red == greed)
            {
                this.lightColorType = LightColorTypeEnum.Yellow;
                this.imageComboBoxEdit1.SelectedIndex = 4;
            }
            else if (red == blue)
            {
                this.lightColorType = LightColorTypeEnum.Magenta;
                this.imageComboBoxEdit1.SelectedIndex = 5;
            }
            else if (greed == blue)
            {
                this.lightColorType = LightColorTypeEnum.Cyan;
                this.imageComboBoxEdit1.SelectedIndex = 3;
            }
            else
            {
                this.lightColorType = LightColorTypeEnum.Customize;
                this.imageComboBoxEdit1.SelectedIndex = 7;
            }

            // 设置显示
            this.TbLight.Value = this.intensity;
            this.SpIntensity.Value = (int)this.intensity;
        }

        /// <summary>
        /// 光源颜色
        /// </summary>
        public enum LightColorTypeEnum
        {
            /// <summary>
            /// 红色
            /// </summary>
            Red,

            /// <summary>
            /// 绿色
            /// </summary>
            Green,

            /// <summary>
            /// 蓝色
            /// </summary>
            Blue,

            /// <summary>
            /// 黄色
            /// </summary>
            Yellow,

            /// <summary>
            /// 青色
            /// </summary>
            Cyan,

            /// <summary>
            /// 酒红色
            /// </summary>
            Magenta,

            /// <summary>
            /// 白色
            /// </summary>
            White,

            /// <summary>
            /// 自定义
            /// </summary>
            Customize
        }

        /// <summary>
        /// 拖动滑块
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void TbLight_EditValueChanged(object sender, EventArgs e)
        {
            if ((DateTime.Now - this.changeLightTime).Milliseconds > 10)
            {
                this.changeLightTime = DateTime.Now;
                //if (this.lightRed != null)
                //{
                //    // 控制光源的值
                //    this.ChangeAllIntensity((int)this.SpIntensity.Value);
                //}

                // 传递值
                this.SpIntensity.Value = this.TbLight.Value;
            }
        }

        /// <summary>
        /// 直接改变数值
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void SpIntensity_EditValueChanged(object sender, EventArgs e)
        {
            // 非负判断
            if ((int)this.SpIntensity.Value <= 0)
            {
                this.SpIntensity.Value = 0;
            }

            if (this.lightRed != null)
            {
                // 控制光源的值
                this.ChangeAllIntensity((int)this.TbLight.Value);

                // 传递值
                this.TbLight.Value = (int)this.SpIntensity.Value;
            }
        }

        /// <summary>
        /// 改变所有光的强度
        /// </summary>
        /// <param name="intensityValue">光的强度</param>
        private void ChangeAllIntensity(int intensityValue)
        {
            int lightIntensity = intensityValue;

            // 根据光的类型，设置不同的光
            if (this.lightColorType == LightColorTypeEnum.White)
            {
                this.ChangeIntensity(this.lightBlue, lightIntensity);
                this.ChangeIntensity(this.lightGreed, lightIntensity);
                this.ChangeIntensity(this.lightRed, lightIntensity);
            }
            else if(this.lightColorType == LightColorTypeEnum.Yellow)
            {
                
                this.ChangeIntensity(this.lightBlue, 0);
                this.ChangeIntensity(this.lightGreed, lightIntensity);
                this.ChangeIntensity(this.lightRed, lightIntensity);
            }
            else if (this.lightColorType == LightColorTypeEnum.Magenta)
            {
                this.ChangeIntensity(this.lightBlue, lightIntensity);
                this.ChangeIntensity(this.lightGreed, 0);
                this.ChangeIntensity(this.lightRed, lightIntensity);
            }
            else if (this.lightColorType == LightColorTypeEnum.Cyan)
            {
                this.ChangeIntensity(this.lightBlue, lightIntensity);
                this.ChangeIntensity(this.lightGreed, lightIntensity);
                this.ChangeIntensity(this.lightRed, 0);
            }
            else if (this.lightColorType == LightColorTypeEnum.Red)
            {
                this.ChangeIntensity(this.lightBlue, 0);
                this.ChangeIntensity(this.lightGreed, 0);
                this.ChangeIntensity(this.lightRed, lightIntensity);
            }
            else if (this.lightColorType == LightColorTypeEnum.Green)
            {
                this.ChangeIntensity(this.lightBlue, 0);
                this.ChangeIntensity(this.lightGreed, lightIntensity);
                this.ChangeIntensity(this.lightRed, 0);
            }
            else if (this.lightColorType == LightColorTypeEnum.Blue)
            {
                this.ChangeIntensity(this.lightBlue, lightIntensity);
                this.ChangeIntensity(this.lightGreed, 0);
                this.ChangeIntensity(this.lightRed, 0);
            }
            else if (this.lightColorType == LightColorTypeEnum.Yellow)
            {
                this.ChangeIntensity(this.lightBlue, lightIntensity);
                this.ChangeIntensity(this.lightGreed, lightIntensity);
                this.ChangeIntensity(this.lightRed, lightIntensity);
            }
        }

        /// <summary>
        /// 改变单个光的强度
        /// </summary>
        /// <param name="light">光</param>
        /// <param name="intensityValue">强度</param>
        private void ChangeIntensity(Light light,int intensityValue)
        {
            if (light != null)
            {
                //if (light.GetIntensity() != 0)
                //{
                //    light.SetIntensity(intensityValue / 100 * 255);
                //}

                light?.SetIntensity(intensityValue * 255 / 100);
            }
        }

        /// <summary>
        /// 换成新的颜色初始化
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void ImageComboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lightRed == null || this.lightBlue == null || this.lightGreed == null)
            {
                return;
            }

            if (this.imageComboBoxEdit1.SelectedIndex == 0)
            {
                this.lightBlue.SetIntensity(0);
                this.lightGreed.SetIntensity(0);
                this.lightRed.SetIntensity(90);
                this.lightColorType = LightColorTypeEnum.Red;
            }
            else if (this.imageComboBoxEdit1.SelectedIndex == 1)
            {
                this.lightRed.SetIntensity(0);
                this.lightGreed.SetIntensity(0);
                this.lightBlue.SetIntensity(90);
                this.lightColorType = LightColorTypeEnum.Blue;
            }
            else if (this.imageComboBoxEdit1.SelectedIndex == 2)
            {
                this.lightRed.SetIntensity(0);
                this.lightBlue.SetIntensity(0);
                this.lightGreed.SetIntensity(90);
                this.lightColorType = LightColorTypeEnum.Green;
            }
            else if (this.imageComboBoxEdit1.SelectedIndex == 3)
            {
                this.lightRed.SetIntensity(0);
                this.lightGreed.SetIntensity(90);
                this.lightBlue.SetIntensity(90);
                this.lightColorType = LightColorTypeEnum.Cyan;
            }
            else if (this.imageComboBoxEdit1.SelectedIndex == 4)
            {
                this.lightBlue.SetIntensity(0);
                this.lightGreed.SetIntensity(90);
                this.lightRed.SetIntensity(90);
                this.lightColorType = LightColorTypeEnum.Yellow;
            }
            else if (this.imageComboBoxEdit1.SelectedIndex == 5)
            {
                this.lightGreed.SetIntensity(0);
                this.lightRed.SetIntensity(90);
                this.lightBlue.SetIntensity(90);
                this.lightColorType = LightColorTypeEnum.Magenta;
            }
            else if (this.imageComboBoxEdit1.SelectedIndex == 6)
            {
                this.lightRed.SetIntensity(90);
                this.lightBlue.SetIntensity(90);
                this.lightGreed.SetIntensity(90);
                this.lightColorType = LightColorTypeEnum.White;
            }
            else if (this.imageComboBoxEdit1.SelectedIndex == 7)
            {
                this.lightRed.SetIntensity(90);
                this.lightBlue.SetIntensity(90);
                this.lightGreed.SetIntensity(90);
                this.lightColorType = LightColorTypeEnum.White;
            }

            //this.SpIntensity.Value = 40;
            //this.TbLight.Value = 40;
        }

        /// <summary>
        /// 鼠标抬起之后才触发事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void TbLight_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.lightRed != null)
            {
                // 控制光源的值
                this.ChangeAllIntensity((int)this.SpIntensity.Value);
            }

            // 传递值
            this.SpIntensity.Value = this.TbLight.Value;
        }

        /// <summary>
        /// 关闭灯光
        /// </summary>
        public void CloseLight()
        {
            this.ChangeIntensity(this.lightBlue, 10);
            this.ChangeIntensity(this.lightGreed, 10);
            this.ChangeIntensity(this.lightRed, 10);
            this.ChangeIntensity(this.lightBlue, 0);
            this.ChangeIntensity(this.lightGreed, 0);
            this.ChangeIntensity(this.lightRed, 0);
        }
    }
}
