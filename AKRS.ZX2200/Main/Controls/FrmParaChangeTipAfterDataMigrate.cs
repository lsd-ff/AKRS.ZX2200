using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using AKRS.ZX2200.Main.Machine.MachineSupport;

namespace AKRS.ZX2200.Main.Controls
{
    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 参数修改提示窗体（数据迁移后）
    /// </summary>
    public partial class FrmParaChangeTipAfterDataMigrate : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmParaChangeTipAfterDataMigrate()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        ///  界面初始化
        /// </summary>
        private void InitControl()
        {
            this.richTextBox1.Text += "\r\n1、参数界面->机器->机器数据->系统2模组参数：模组安全位、换静态华夫盒避让位\r\n"
                                     + "2、参数界面->机器->机器数据->晶圆设备参数：bond在上视模式下的避让晶圆相机的位置、bond在IPT模式下的避让晶圆相机的位置、bond避让静态华夫盒的位置\r\n"
                                     + "3、参数界面->机器->机器数据->机器硬件配置：所有参数\r\n" + "4、参数界面->机器->机器数据->机器软件配置：所有参数\r\n"
                                     + "4、设备功能->设备参数示教：所有参数重新示教";

            richTextBox1.Select(0, 40);
            richTextBox1.SelectionColor = Color.Red;
            richTextBox1.SelectionFont = new Font("微软雅黑", 15, FontStyle.Bold);
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            Machine.GetInstance().ReSetMachineInfo();
            this.DialogResult = DialogResult.OK;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}