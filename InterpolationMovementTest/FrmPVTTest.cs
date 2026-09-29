using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.Infrastructure.Service;
using DevExpress.Utils.MVVM.Services;
using DevExpress.XtraEditors;
using WindowsFormsApp1.Helper;

namespace WindowsFormsApp1
{
    /// <summary>
    /// PVT测试窗体
    /// </summary>
    public partial class FrmPVTTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmPVTTest()
        {
            InitializeComponent();
            // this.Init();
        }

        /// <summary>
        /// 焊头Z轴
        /// </summary>
        private Axis AxisZ => HardwareRepositoryService.GetHardware<Axis>("BondZ");

        /// <summary>
        ///  焊头T轴
        /// </summary>
        private Axis AxisT => HardwareRepositoryService.GetHardware<Axis>("焊头T");

        /// <summary>
        /// BondX轴
        /// </summary>
        public Axis AxisX => HardwareRepositoryService.GetHardware<Axis>("BondX");

        /// <summary>
        /// BondY轴
        /// </summary>
        public Axis AxisY => HardwareRepositoryService.GetHardware<Axis>("BondY");

        /// <summary>
        /// 点位
        /// </summary>
        private MovePositions movePositions = MovePositions.GetInstance();

        /// <summary>
        /// 移动方法
        /// </summary>
        private Movements movements = new Movements();

        /// <summary>
        /// 获取第一点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtGetPT1_Click(object sender, EventArgs e)
        {
            this.SpPT1X.EditValue = AxisX.GetRealPosition();
            this.SpPT1Y.EditValue = AxisY.GetRealPosition();
            this.SpPT1Z.EditValue = AxisZ.GetRealPosition();
            this.SpPT1T.EditValue = AxisT.GetRealPosition();

            this.Save();
        }

        /// <summary>
        /// 获取第2点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtGetPT2_Click(object sender, EventArgs e)
        {
            this.SpPT2X.EditValue = AxisX.GetRealPosition();
            this.SpPT2Y.EditValue = AxisY.GetRealPosition();
            this.SpPT2Z.EditValue = AxisZ.GetRealPosition();
            this.SpPT2T.EditValue = AxisT.GetRealPosition();

            this.Save();
        }

        /// <summary>
        /// 获取第3点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtGetPT3_Click(object sender, EventArgs e)
        {
            this.SpPT3X.EditValue = AxisX.GetRealPosition();
            this.SpPT3Y.EditValue = AxisY.GetRealPosition();
            this.SpPT3Z.EditValue = AxisZ.GetRealPosition();
            this.SpPT3T.EditValue = AxisT.GetRealPosition();

            this.Save();
        }

        /// <summary>
        /// 获取第4点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtGetPT4_Click(object sender, EventArgs e)
        {
            this.SpPT4X.EditValue = AxisX.GetRealPosition();
            this.SpPT4Y.EditValue = AxisY.GetRealPosition();
            this.SpPT4Z.EditValue = AxisZ.GetRealPosition();
            this.SpPT4T.EditValue = AxisT.GetRealPosition();
            this.Save();
        }

        /// <summary>
        /// 获取第5点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtGetPT5_Click(object sender, EventArgs e)
        {
            this.SpPT5X.EditValue = AxisX.GetRealPosition();
            this.SpPT5Y.EditValue = AxisY.GetRealPosition();
            this.SpPT5Z.EditValue = AxisZ.GetRealPosition();
            this.SpPT5T.EditValue = AxisT.GetRealPosition();
            this.Save();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.movePositions.Point1.Pos.X = (double)this.SpPT1X.Value;
            this.movePositions.Point2.Pos.X = (double)this.SpPT2X.Value;
            this.movePositions.Point3.Pos.X = (double)this.SpPT3X.Value;
            this.movePositions.Point4.Pos.X = (double)this.SpPT4X.Value;
            this.movePositions.Point5.Pos.X = (double)this.SpPT5X.Value;

            this.movePositions.Point1.Pos.Y = (double)this.SpPT1Y.Value;
            this.movePositions.Point2.Pos.Y = (double)this.SpPT2Y.Value;
            this.movePositions.Point3.Pos.Y = (double)this.SpPT3Y.Value;
            this.movePositions.Point4.Pos.Y = (double)this.SpPT4Y.Value;
            this.movePositions.Point5.Pos.Y = (double)this.SpPT5Y.Value;

            this.movePositions.Point1.Pos.Z = (double)this.SpPT1Z.Value;
            this.movePositions.Point2.Pos.Z = (double)this.SpPT2Z.Value;
            this.movePositions.Point3.Pos.Z = (double)this.SpPT3Z.Value;
            this.movePositions.Point4.Pos.Z = (double)this.SpPT4Z.Value;
            this.movePositions.Point5.Pos.Z = (double)this.SpPT5Z.Value;

            this.movePositions.Point1.Angle = (double)this.SpPT1T.Value;
            this.movePositions.Point2.Angle = (double)this.SpPT2T.Value;
            this.movePositions.Point3.Angle = (double)this.SpPT3T.Value;
            this.movePositions.Point4.Angle = (double)this.SpPT4T.Value;
            this.movePositions.Point5.Angle = (double)this.SpPT5T.Value;

            this.movePositions.Point1.Time = (double)this.SpPT1Time.Value;
            this.movePositions.Point2.Time = (double)this.SpPT2Time.Value;
            this.movePositions.Point3.Time = (double)this.SpPT3Time.Value;
            this.movePositions.Point4.Time = (double)this.SpPT4Time.Value;
            this.movePositions.Point5.Time = (double)this.SpPT5Time.Value;

            this.movePositions.Point1.Vel.X = (double)this.SpPoint1XVel.Value;
            this.movePositions.Point2.Vel.X = (double)this.SpPoint2XVel.Value;
            this.movePositions.Point3.Vel.X = (double)this.SpPoint3XVel.Value;
            this.movePositions.Point4.Vel.X = (double)this.SpPoint4XVel.Value;
            this.movePositions.Point5.Vel.X = (double)this.SpPoint5XVel.Value;

            this.movePositions.Point1.Vel.Y = (double)this.SpPoint1YVel.Value;
            this.movePositions.Point2.Vel.Y = (double)this.SpPoint2YVel.Value;
            this.movePositions.Point3.Vel.Y = (double)this.SpPoint3YVel.Value;
            this.movePositions.Point4.Vel.Y = (double)this.SpPoint4YVel.Value;
            this.movePositions.Point5.Vel.Y = (double)this.SpPoint5YVel.Value;

            this.movePositions.Point1.Vel.Z = (double)this.SpPoint1ZVel.Value;
            this.movePositions.Point2.Vel.Z = (double)this.SpPoint2ZVel.Value;
            this.movePositions.Point3.Vel.Z = (double)this.SpPoint3ZVel.Value;
            this.movePositions.Point4.Vel.Z = (double)this.SpPoint4ZVel.Value;
            this.movePositions.Point5.Vel.Z = (double)this.SpPoint5ZVel.Value;

            this.movePositions.Point1.TVel = (double)this.SpPoint1TVel.Value;
            this.movePositions.Point2.TVel = (double)this.SpPoint2TVel.Value;
            this.movePositions.Point3.TVel = (double)this.SpPoint3TVel.Value;
            this.movePositions.Point4.TVel = (double)this.SpPoint4TVel.Value;
            this.movePositions.Point5.TVel = (double)this.SpPoint5TVel.Value;

           this.movePositions.Save();
        }


        /// <summary>
        /// 保存
        /// </summary>
        private void Init()
        {
            this.SpPT1X.EditValue = this.movePositions.Point1.Pos.X;
            this.SpPT2X.EditValue = this.movePositions.Point2.Pos.X;
            this.SpPT3X.EditValue = this.movePositions.Point3.Pos.X;
            this.SpPT4X.EditValue = this.movePositions.Point4.Pos.X;
            this.SpPT5X.EditValue = this.movePositions.Point5.Pos.X;

            this.SpPT1Y.EditValue = this.movePositions.Point1.Pos.Y;
            this.SpPT2Y.EditValue = this.movePositions.Point2.Pos.Y;
            this.SpPT3Y.EditValue = this.movePositions.Point3.Pos.Y;
            this.SpPT4Y.EditValue = this.movePositions.Point4.Pos.Y;
            this.SpPT5Y.EditValue = this.movePositions.Point5.Pos.Y;

            this.SpPT1Z.EditValue = this.movePositions.Point1.Pos.Z;
            this.SpPT2Z.EditValue = this.movePositions.Point2.Pos.Z;
            this.SpPT3Z.EditValue = this.movePositions.Point3.Pos.Z;
            this.SpPT4Z.EditValue = this.movePositions.Point4.Pos.Z;
            this.SpPT5Z.EditValue = this.movePositions.Point5.Pos.Z;

            this.SpPT1T.EditValue = this.movePositions.Point1.Angle;
            this.SpPT2T.EditValue = this.movePositions.Point2.Angle;
            this.SpPT3T.EditValue = this.movePositions.Point3.Angle;
            this.SpPT4T.EditValue = this.movePositions.Point4.Angle;
            this.SpPT5T.EditValue = this.movePositions.Point5.Angle;

            this.SpPoint1XVel.EditValue = this.movePositions.Point1.Vel.X;
            this.SpPoint2XVel.EditValue = this.movePositions.Point2.Vel.X;
            this.SpPoint3XVel.EditValue = this.movePositions.Point3.Vel.X;
            this.SpPoint4XVel.EditValue = this.movePositions.Point4.Vel.X;
            this.SpPoint5XVel.EditValue = this.movePositions.Point5.Vel.X;

            this.SpPoint1YVel.EditValue = this.movePositions.Point1.Vel.Y;
            this.SpPoint2YVel.EditValue = this.movePositions.Point2.Vel.Y;
            this.SpPoint3YVel.EditValue = this.movePositions.Point3.Vel.Y;
            this.SpPoint4YVel.EditValue = this.movePositions.Point4.Vel.Y;
            this.SpPoint5YVel.EditValue = this.movePositions.Point5.Vel.Y;

            this.SpPoint1ZVel.EditValue = this.movePositions.Point1.Vel.Z;
            this.SpPoint2ZVel.EditValue = this.movePositions.Point2.Vel.Z;
            this.SpPoint3ZVel.EditValue = this.movePositions.Point3.Vel.Z;
            this.SpPoint4ZVel.EditValue = this.movePositions.Point4.Vel.Z;
            this.SpPoint5ZVel.EditValue = this.movePositions.Point5.Vel.Z;

            this.SpPoint1TVel.EditValue = this.movePositions.Point1.TVel;
            this.SpPoint2TVel.EditValue = this.movePositions.Point2.TVel;
            this.SpPoint3TVel.EditValue = this.movePositions.Point3.TVel;
            this.SpPoint4TVel.EditValue = this.movePositions.Point4.TVel;
            this.SpPoint5TVel.EditValue = this.movePositions.Point5.TVel;

            this.SpPT1Time.EditValue = this.movePositions.Point1.Time;
            this.SpPT2Time.EditValue = this.movePositions.Point2.Time;
            this.SpPT3Time.EditValue = this.movePositions.Point3.Time;
            this.SpPT4Time.EditValue = this.movePositions.Point4.Time;
            this.SpPT5Time.EditValue = this.movePositions.Point5.Time;
        }

        /// <summary>
        /// 两轴PT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtStartTwoAxisPT_Click(object sender, EventArgs e)
        {
            this.movements.PTWithTwoAxis();
        }

        /// <summary>
        /// 两轴PVT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtStartTwoAxisPVT_Click(object sender, EventArgs e)
        {
            this.movements.PVTWithTwoAxis();
        }

        /// <summary>
        /// 4轴PT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtStartFourAxisPT_Click(object sender, EventArgs e)
        {
            this.movements.PTWithFourAxis();
        }

        /// <summary>
        /// 4轴PVT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtStartFourAxisPVT_Click(object sender, EventArgs e)
        {
            this.movements.PVTWithFourAxis();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            this.Save();

            DialogResult dialog = XtraMessageBox.Show(
                $"Save  success!",
                "Info",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void BtTwoAxisInterpolation_Click(object sender, EventArgs e)
        {
            // this.movements.Interpolation2();
            this.movements.UmlMovement();
        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtFourAxisInterpolation_Click(object sender, EventArgs e)
        {
            this.movements.InterpolationMovementWithFourAxis();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            DBService.Instance.ToString();

           // DBService.DisposeConnection();  
        }
    }
}