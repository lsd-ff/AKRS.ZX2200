using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.PR.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.Galaxy2.PR.Controls
{
    public partial class UcPRLight : UserControl
    {
        public UcPRLight()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 光强度
        /// </summary>
        private int intensity;

        /// <summary>
        /// 红光
        /// </summary>
        public Light RedLight;

        /// <summary>
        /// 绿光
        /// </summary>
        public Light GreenLight;

        /// <summary>
        /// 蓝光
        /// </summary>
        public Light BlueLight;

        /// <summary>
        /// 光的颜色
        /// </summary>
        public LightColorTypeEnum LightColorType;

        /// <summary>
        /// 是否为三色光
        /// </summary>
        public bool IsColored;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="lightName">光源名字</param>
        /// <param name="lightRedName">红光</param>
        /// <param name="lightGreedName">绿光</param>
        /// <param name="lightBlueName">蓝光</param>
        public void Init(string lightName, string lightRedName, string lightGreedName, string lightBlueName)
        {
            if (lightRedName == null || lightGreedName == null || lightBlueName == null || lightName == null)
            {
                // 检测失败，直接不让使用，避免空指针
                this.Enabled = false;
                return;
            }

            this.GpLightName.Text = lightName;

            // 初始化名称
            this.RedLight = HardwareRepositoryService.GetHardware<Light>(lightRedName);

            this.GreenLight = HardwareRepositoryService.GetHardware<Light>(lightGreedName);

            this.BlueLight = HardwareRepositoryService.GetHardware<Light>(lightBlueName);

            if (this.RedLight == null || this.GreenLight == null || this.BlueLight == null)
            {
                this.Enabled = false;
                return;
            }

            // 判断当前光源是什么颜色
            //this.CorrectLightColorTypeAndIntensity();

            this.Enabled = true;
        }



        public void Init(string lightName, PRLight prlightRed, PRLight prlightGreed, PRLight prlightBlue)
        {
            if (prlightRed == null  || lightName == null)
            {
                // 检测失败，直接不让使用，避免空指针
                this.Visible = false;
                return;
            }

            this.GpLightName.Text = lightName;

            // 初始化名称            
            this.RedLight = prlightRed.Light;

            this.RedLight.SetIntensity(prlightRed.LightIntensity);

            if (prlightGreed.Light != null)
            {
                this.GreenLight= prlightGreed.Light;

                this.GreenLight.SetIntensity(prlightGreed.LightIntensity);
            }

            if (prlightBlue.Light != null)
            {
                this.BlueLight = prlightBlue.Light;

                this.BlueLight.SetIntensity(prlightBlue.LightIntensity);
            }
            
            if(this.GreenLight == null || this.BlueLight == null)
            {
                // 单光，只能改变亮度
                IsColored=false;

                ImgCbxColorSelect.Enabled = false;

                this.intensity = this.RedLight.GetIntensity() * 100 / 255;

                this.TbLight.Value = this.intensity;

                this.SpIntensity.Value = (int)this.intensity;
            }
            else
            {
                // 三色光
                IsColored = true;

                // 判断当前光源是什么颜色
                this.CorrectLightColorTypeAndIntensity();
            }
         
            this.Enabled = true;
        }

        /// <summary>
        /// 单种光源
        /// </summary>
        /// <param name="lightName">名</param>
        /// <param name="lightRedName"></param>
        public void Init(string lightName, string lightRedName)
        {

        }


        /// <summary>
        /// 判断当前是什么颜色
        /// </summary>
        private void CorrectLightColorTypeAndIntensity()
        {
            // 获取到当前的亮度数值比例
            int red = this.RedLight.GetIntensity() * 100 / 255;
            int greed = this.GreenLight.GetIntensity() * 100 / 255;
            int blue = this.BlueLight.GetIntensity() * 100 / 255;


            // 获取光的强度
            this.intensity = Math.Max(red, Math.Max(greed, blue));

            // 判断类型 
            if (red == greed && greed == blue)
            {
                this.LightColorType = LightColorTypeEnum.White;
                this.ImgCbxColorSelect.SelectedIndex = 6;
            }
            else if (greed == 0 && blue == 0 && red != 0)
            {
                this.LightColorType = LightColorTypeEnum.Red;
                this.ImgCbxColorSelect.SelectedIndex = 0;
            }
            else if (red == 0 && blue == 0 && greed != 0)
            {
                this.LightColorType = LightColorTypeEnum.Green;
                this.ImgCbxColorSelect.SelectedIndex = 2;
            }
            else if (greed == 0 && red == 0 && blue != 0)
            {
                this.LightColorType = LightColorTypeEnum.Blue;
                this.ImgCbxColorSelect.SelectedIndex = 1;
            }
            else if (red == greed)
            {
                this.LightColorType = LightColorTypeEnum.Yellow;
                this.ImgCbxColorSelect.SelectedIndex = 4;
            }
            else if (red == blue)
            {
                this.LightColorType = LightColorTypeEnum.Magenta;
                this.ImgCbxColorSelect.SelectedIndex = 5;
            }
            else if (greed == blue)
            {
                this.LightColorType = LightColorTypeEnum.Cyan;
                this.ImgCbxColorSelect.SelectedIndex = 3;
            }
            else
            {
                this.LightColorType = LightColorTypeEnum.Customize;
                this.ImgCbxColorSelect.SelectedIndex = 7;
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
            // 传递值
            this.SpIntensity.Value = this.TbLight.Value;

            if (this.RedLight != null)
            {
                if (this.IsColored)
                {
                    // 控制光源的值
                    this.ChangeAllIntensity(this.TbLight.Value);
                }
                else
                {
                    this.ChangeIntensity(this.RedLight, this.TbLight.Value);
                }
                
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

            if (this.RedLight != null)
            {
                // 传递值
                this.TbLight.Value = (int)this.SpIntensity.Value;

                if (this.IsColored)
                {
                    // 控制光源的值
                    this.ChangeAllIntensity((int)this.SpIntensity.Value);
                }
                else
                {
                    this.ChangeIntensity(this.RedLight, (int)this.SpIntensity.Value);
                }            
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
            if (this.LightColorType == LightColorTypeEnum.White)
            {
                this.ChangeIntensity(this.BlueLight, lightIntensity);
                this.ChangeIntensity(this.GreenLight, lightIntensity);
                this.ChangeIntensity(this.RedLight, lightIntensity);
            }
            else if (this.LightColorType == LightColorTypeEnum.Yellow)
            {

                this.ChangeIntensity(this.BlueLight, 0);
                this.ChangeIntensity(this.GreenLight, lightIntensity);
                this.ChangeIntensity(this.RedLight, lightIntensity);
            }
            else if (this.LightColorType == LightColorTypeEnum.Magenta)
            {
                this.ChangeIntensity(this.BlueLight, lightIntensity);
                this.ChangeIntensity(this.GreenLight, 0);
                this.ChangeIntensity(this.RedLight, lightIntensity);
            }
            else if (this.LightColorType == LightColorTypeEnum.Cyan)
            {
                this.ChangeIntensity(this.BlueLight, lightIntensity);
                this.ChangeIntensity(this.GreenLight, lightIntensity);
                this.ChangeIntensity(this.RedLight, 0);
            }
            else if (this.LightColorType == LightColorTypeEnum.Red)
            {
                this.ChangeIntensity(this.BlueLight, 0);
                this.ChangeIntensity(this.GreenLight, 0);
                this.ChangeIntensity(this.RedLight, lightIntensity);
            }
            else if (this.LightColorType == LightColorTypeEnum.Green)
            {
                this.ChangeIntensity(this.BlueLight, 0);
                this.ChangeIntensity(this.GreenLight, lightIntensity);
                this.ChangeIntensity(this.RedLight, 0);
            }
            else if (this.LightColorType == LightColorTypeEnum.Blue)
            {
                this.ChangeIntensity(this.BlueLight, lightIntensity);
                this.ChangeIntensity(this.GreenLight, 0);
                this.ChangeIntensity(this.RedLight, 0);
            }
            else if (this.LightColorType == LightColorTypeEnum.Yellow)
            {
                this.ChangeIntensity(this.BlueLight, lightIntensity);
                this.ChangeIntensity(this.GreenLight, lightIntensity);
                this.ChangeIntensity(this.RedLight, lightIntensity);
            }
        }

        /// <summary>
        /// 改变单个光的强度
        /// </summary>
        /// <param name="light">光</param>
        /// <param name="intensityValue">强度</param>
        private void ChangeIntensity(Light light, int intensityValue)
        {
            if (light != null)
            {
                //if (light.GetIntensity() != 0)
                //{
                //    light.SetIntensity(intensityValue / 100 * 255);
                //}

                light.SetIntensity(intensityValue * 255 / 100);
            }

        }

        /// <summary>
        /// 换成新的颜色初始化
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void ImageComboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.RedLight == null || this.BlueLight == null || this.GreenLight == null)
            {
                return;
            }

            if (this.ImgCbxColorSelect.SelectedIndex == 0)
            {
                this.BlueLight.SetIntensity(0);
                this.GreenLight.SetIntensity(0);
                this.RedLight.SetIntensity(90);
                this.LightColorType = LightColorTypeEnum.Red;
            }
            else if (this.ImgCbxColorSelect.SelectedIndex == 1)
            {
                this.RedLight.SetIntensity(0);
                this.GreenLight.SetIntensity(0);
                this.BlueLight.SetIntensity(90);
                this.LightColorType = LightColorTypeEnum.Blue;
            }
            else if (this.ImgCbxColorSelect.SelectedIndex == 2)
            {
                this.RedLight.SetIntensity(0);
                this.BlueLight.SetIntensity(0);
                this.GreenLight.SetIntensity(90);
                this.LightColorType = LightColorTypeEnum.Green;
            }
            else if (this.ImgCbxColorSelect.SelectedIndex == 3)
            {
                this.RedLight.SetIntensity(0);
                this.GreenLight.SetIntensity(90);
                this.BlueLight.SetIntensity(90);
                this.LightColorType = LightColorTypeEnum.Cyan;
            }
            else if (this.ImgCbxColorSelect.SelectedIndex == 4)
            {
                this.BlueLight.SetIntensity(0);
                this.GreenLight.SetIntensity(90);
                this.RedLight.SetIntensity(90);
                this.LightColorType = LightColorTypeEnum.Yellow;
            }
            else if (this.ImgCbxColorSelect.SelectedIndex == 5)
            {
                this.GreenLight.SetIntensity(0);
                this.RedLight.SetIntensity(90);
                this.BlueLight.SetIntensity(90);
                this.LightColorType = LightColorTypeEnum.Magenta;
            }
            else if (this.ImgCbxColorSelect.SelectedIndex == 6)
            {
                this.RedLight.SetIntensity(90);
                this.BlueLight.SetIntensity(90);
                this.GreenLight.SetIntensity(90);
                this.LightColorType = LightColorTypeEnum.White;
            }
            else if (this.ImgCbxColorSelect.SelectedIndex == 7)
            {
                this.RedLight.SetIntensity(90);
                this.BlueLight.SetIntensity(90);
                this.GreenLight.SetIntensity(90);
                this.LightColorType = LightColorTypeEnum.White;
            }

        }

        /// <summary>
        /// 输出所有光源亮度
        /// </summary>
        /// <returns></returns>
        public (int ,int ,int) GetAllLightIntensity()
        {
            int Redintensity = this.RedLight==null?new int():this.RedLight.GetIntensity();

            int Greedintensity=this.GreenLight==null?new int():this.GreenLight.GetIntensity();

            int Blueintensity=this.BlueLight==null?new int():this.BlueLight.GetIntensity();

            return (Redintensity, Greedintensity, Blueintensity);
        }
    }
}
