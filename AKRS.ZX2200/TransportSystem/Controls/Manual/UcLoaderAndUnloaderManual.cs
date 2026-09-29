using AKRS.ZX2200.TransportSystem.Controllers;
using DevExpress.XtraEditors;
using LanguageExt.TypeClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportSystem.Controls.Manual
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.Programs;

    /// <summary>
    ///  上下料调试窗体
    /// </summary>
    public partial class UcLoaderAndUnloaderManual : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UcLoaderAndUnloaderManual()
        {
            InitializeComponent();
            this.RefreshControl();

            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= timer1_Tick;
                    this.timer1.Dispose();
                };
        }

        /// <summary>
        /// 上料仓控制器
        /// </summary>
        private LoaderBinController loaderBinController = new LoaderBinController();

        /// <summary>
        /// 下料仓程式
        /// </summary>
        private UnLoaderBinProgram unLoaderBinProgram = TransportProgram.GetInstance().UnLoaderBinProgram;

        /// <summary>
        /// 上料仓程式
        /// </summary>
        private LoaderBinProgram loaderBinProgram = TransportProgram.GetInstance().LoaderBinProgram;

        /// <summary>
        /// 下料仓控制器
        /// </summary>
        private UnLoaderBinController unLoaderBinController = new UnLoaderBinController();

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void RefreshControl()
        {
            this.LbLoaderCurrentBin.Text = "当前料仓:" + loaderBinProgram.CurBinType.ToString();
            this.LbLoaderCurrentTablet.Text = "当前料片:" + loaderBinProgram.CurPlaceLayer.ToString();
            this.LbUnLoaderCurrentBin.Text = "当前料仓:" + unLoaderBinProgram.CurBinType.ToString();
            this.LbUnLoaderCurrentTablet.Text = "当前料片:" + unLoaderBinProgram.CurPlaceLayer.ToString();
        }

        /// <summary>
        /// 从头开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLoadFromFirst_Click(object sender, EventArgs e)
        {
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                //this.loaderBinController.MoveToBinA();
                this.loaderBinController.MoveToTabletLevel(1);
                this.loaderBinController.ResetCurPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        /// 上料仓上一片料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLastTablet_Click(object sender, EventArgs e)
        {
            //if (this.loaderBinController.GetCurPlaceLayer() <= 1)
            //{
            //    return;
            //}

            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.loaderBinController.MoveToLastTablet();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }        
        }

        /// <summary>
        /// 上料仓下一片料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnNextTablet_Click(object sender, EventArgs e)
        {
            if (this.loaderBinController.IsOutOfTabletLimit(this.loaderBinController.GetCurPlaceLayer() + 1)) 
            {
                return;
            } 

            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.loaderBinController.MoveToNextPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        ///  选择料片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnChooseTablet_Click(object sender, EventArgs e)
        {
            int num;

        // 弹出选择窗体
        RetryCommand:
        string input = XtraInputBox.Show("请输入料片号", "料片号", "1");

            if (string.IsNullOrEmpty(input) )
            {
                return;
            }

            // 输入列数
            if (!int.TryParse(input, out num))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"参数错误，请重试！",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                goto RetryCommand;
            }

            this.BtnLoaderChooseTablet.Enabled = false;
            this.BtnLoaderChooseTablet.BackColor = Color.Yellow;

            if (this.loaderBinController.IsOutOfTabletLimit(num))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"料片号超限!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                goto RetryCommand;
            }

            try
            {
                // 设置为当前料片
                this.loaderBinController.SetCurPlaceLayer(num);

                // 移动
                this.loaderBinController.MoveToCurrentPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"移动当前料片失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.BtnLoaderChooseTablet.Enabled = true;
                this.BtnLoaderChooseTablet.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 上料仓设置上一个料盒
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLastBin_Click(object sender, EventArgs e)
        {
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.loaderBinController.MoveToNextBin();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }         
        }

        /// <summary>
        ///  上料仓设置下一个料盒
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnNextBin_Click(object sender, EventArgs e)
        {
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.loaderBinController.MoveToNextBin();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        /// 下料从头开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderStartFromFirst_Click(object sender, EventArgs e)
        {
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                //this.unLoaderBinController.MoveToBinA();
                this.unLoaderBinController.MoveToTabletLevel(1);
                this.unLoaderBinController.ResetCurPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        /// 下料选择料片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderChooseTablet_Click(object sender, EventArgs e)
        {
            int num;

        // 弹出选择窗体
        RetryCommand:
        string input = XtraInputBox.Show("请输入料片号", "料片号", "1");

            if (string.IsNullOrEmpty(input))
        {
            return;
        }

        // 输入列数
        if (!int.TryParse(input, out num))
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"参数错误，请重试！",
                "报警",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

                goto RetryCommand;
        }

        this.BtnUnLoaderChooseTablet.Enabled = false;
        this.BtnUnLoaderChooseTablet.BackColor = Color.Yellow;

            try
            {
                // 设置为当前料片
                this.unLoaderBinController.SetCurPlaceLayer(num);

                // 移动
                this.unLoaderBinController.MoveToCurrentPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"移动当前料片失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.BtnUnLoaderChooseTablet.Enabled = true;
                this.BtnUnLoaderChooseTablet.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 上料仓上一片料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderLastTablet_Click(object sender, EventArgs e)
        {
            //if (this.unLoaderBinController.GetCurPlaceLayer() <= 1)
            //{
            //    return;
            //}

            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.unLoaderBinController.MoveToLastTablet();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"下料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        /// 下料仓去下一片料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderNextTablet_Click(object sender, EventArgs e)
        {
            if (this.unLoaderBinController.IsOutOfTabletLimit())
            {
                return;
            }

            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.unLoaderBinController.MoveToNextPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"下料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        /// 上料仓设置上一个料盒
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderLastBin_Click(object sender, EventArgs e)
        {
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.unLoaderBinController.MoveToNextBin();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"下料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }      
        }

        /// <summary>
        ///  下料仓设置下一个料盒
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderNextBin_Click(object sender, EventArgs e)
        {

            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.unLoaderBinController.MoveToNextBin();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"下料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        /// 上料推杆推出缩回
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnloaderPushRod_Click(object sender, EventArgs e)
        {
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                if (this.loaderBinController.IsPushRodSafe())
                {
                    // 推杆推出
                    this.loaderBinController.PushTabletToDispense();
                    this.BtnloaderPushRod.Appearance.BackColor = Color.Yellow;
                }
                else
                {
                    // 推杆缩回
                    this.loaderBinController.MovePushRodHome();
                    this.BtnloaderPushRod.Appearance.BackColor = default;
                }
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"下料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
           
        }

        /// <summary>
        /// 下料推杆推出缩回
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnloaderPushRod_Click(object sender, EventArgs e)
        {
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                if (this.unLoaderBinController.IsPushRodSafe())
                {
                    // 推杆推出
                    this.unLoaderBinController.PushTabletToUnloader();

                    this.BtnUnloaderPushRod.BackColor = Color.Yellow;
                }
                else
                {
                    // 推杆缩回
                    this.unLoaderBinController.MovePushRodHome();
                    this.BtnUnloaderPushRod.BackColor = default;
                }
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"下料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }     
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.RefreshControl();
        }
    }
}
