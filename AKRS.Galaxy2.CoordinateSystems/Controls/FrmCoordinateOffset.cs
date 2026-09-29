using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace AKRS.Galaxy2.CoordinateSystems.Controls
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;

    /// <summary>
    /// 偏移值
    /// </summary>
    public partial class FrmCoordinateOffset : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCoordinateOffset()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 偏移值增加
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtOffsetAdd_Click(object sender, EventArgs e)
        {
            string coordinateName = (string)this.CmbCoordinate.SelectedItem;

            GeneralCoordinateSystem generalCoordinateSystem =
                (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().Find(coordinateName);

            if (generalCoordinateSystem == null)
            {
                XtraMessageBox.Show($"未找到坐标系 {coordinateName}");
                return;
            }

            generalCoordinateSystem.Distance = new AKRSPoint3D(
                (double)this.spinEdit1.Value,
                (double)this.spinEdit2.Value,
                (double)this.spinEdit3.Value);
            generalCoordinateSystem.Degree = (double)this.SpOffsetAngle.Value;

            MachineCoordinateSystem.GetInstance().Save();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmCoordinateOffset_Load(object sender, EventArgs e)
        {
            foreach (BaseCoordinateSystem coordinateSystem in MachineCoordinateSystem.GetInstance().CoordinateSystems)
            {
                if (coordinateSystem is GeneralCoordinateSystem)
                {
                    this.CmbCoordinate.Properties.Items.Add(coordinateSystem.Name);
                }
            }

            // 设置默认选中项
            if (this.CmbCoordinate.Properties.Items.Count > 0)
            {
                this.CmbCoordinate.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 选择发生变化
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbCoordinate_SelectedIndexChanged(object sender, EventArgs e)
        {
            string coordinateName = (string)this.CmbCoordinate.SelectedItem;

            GeneralCoordinateSystem generalCoordinateSystem =
                (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().Find(coordinateName);

            if (generalCoordinateSystem == null)
            {
                this.spinEdit1.EditValue = 0;
                this.spinEdit2.EditValue = 0;
                this.spinEdit3.EditValue = 0;
                this.SpOffsetAngle.EditValue = 0;
                return;
            }

            this.spinEdit1.EditValue = generalCoordinateSystem.Distance.X;
            this.spinEdit2.EditValue = generalCoordinateSystem.Distance.Y;
            this.spinEdit3.EditValue = generalCoordinateSystem.Distance.Z;
            this.SpOffsetAngle.EditValue = generalCoordinateSystem.Degree;
        }
    }
}