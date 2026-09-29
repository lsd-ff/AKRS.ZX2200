using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Main.Machine.ObjectLevel
{
    /// <summary>
    /// 物体水平
    /// </summary>
    public partial class UcObjectLevel : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 物体水平
        /// </summary>
        public UcObjectLevel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 物体水平
        /// </summary>
        private readonly ObjectLevelData objectLevelData;

        /// <summary>
        /// 物体水平
        /// </summary>
        /// <param name="objectLevelData">水平对象</param>
        public UcObjectLevel(ObjectLevelData objectLevelData)
        {
            this.InitializeComponent();
            this.objectLevelData = objectLevelData;
        }

        /// <summary>
        /// 示教
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistant_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtTest_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcObjectLevel_Load(object sender, EventArgs e)
        {
            this.LbName.Text = this.objectLevelData.Name;
            this.LbLevel.Text = this.objectLevelData.CurrentData;
            this.LbStandard.Text = this.objectLevelData.Standard;
            this.LbQualified.Text = this.objectLevelData.IsQualified;
        }
    }
}
