using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Experiment.Test
{
    public partial class FrmCalibrationValidate : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Bond模组
        /// </summary>
        private readonly BondModule bondModule = new BondModule();

        public FrmCalibrationValidate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ZWithXY
        /// </summary>
        /*private void ZWithXY()
        {
            for (int k = 0; k < 1; k++)
            {
                this.resultZWithXY = new Dictionary<AKRSPoint2D, AKRSPoint2D>();

                // bond相机去拍照位
                {
                    this.bondModule.BondAxisX.AbsoluteMove(this.GlobalCalibrationDomain.StartNewPoint3D.X);
                    this.bondModule.BondAxisY.AbsoluteMove(this.GlobalCalibrationDomain.StartNewPoint3D.Y);
                    this.bondModule.BondHead.AxisZ.AbsoluteMove(this.GlobalCalibrationDomain.StartNewPoint3D.Z);

                    Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                    this.MoveToCameraCenter();

                    this.GlobalCalibrationDomain.StartNewPoint3D.X = this.bondModule.BondAxisX.GetCmdPosition();
                    this.GlobalCalibrationDomain.StartNewPoint3D.Y = this.bondModule.BondAxisY.GetCmdPosition();
                    this.GlobalCalibrationDomain.Save();
                }

                // Z-XY相机去拍照位
                {
                    this.bondModule.BondHead.AxisZ.AbsoluteMove(this.GlobalCalibrationDomain.ZWithXYCameraHeight);

                    Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                RetryMatchZWithXY:
                    PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.GlobalCalibrationDomain.PrNameZWithXY);
                    ExcuteResult re = pREntity.DoWork(false);
                    if (re == ExcuteResult.Success)
                    {
                        this.firstPointPixel = (MatchResult)pREntity.AlgResult;
                    }
                    else
                    {
                        System.Windows.Forms.DialogResult result = AKRSXtraMessageBox.Show("Z-XY未识别到mark! \r\n 点击yes,则重试;\r\n 点击no,则打开pr编辑;\r\n 点击cancel则结束", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        switch (result)
                        {
                            case DialogResult.Yes:
                                goto RetryMatchZWithXY;
                            case DialogResult.No:
                                this.BtnEditPRZWithXY_Click(null, null);
                                goto RetryMatchZWithXY;
                            case DialogResult.Cancel:
                                this.isStop = true;
                                return;
                        }
                    }
                }

                for (int i = 0; i < this.GlobalCalibrationDomain.RowCountNew; i++)
                {
                    for (int j = 0; j < this.GlobalCalibrationDomain.ColumnCountNew; j++)
                    {
                        if (this.isStop)
                        {
                            this.isStop = false;
                            return;
                        }

                        // Bond相机拍照并移动到相机中心-------------------------------------------------------------------------------------------------
                        AKRSPoint3D point3D = new AKRSPoint3D(
                           this.GlobalCalibrationDomain.StartNewPoint3D.X + j * this.GlobalCalibrationDomain.ColumnSpacingNew,
                           this.GlobalCalibrationDomain.StartNewPoint3D.Y + i * this.GlobalCalibrationDomain.RowSpacingNew,
                           this.GlobalCalibrationDomain.StartNewPoint3D.Z);

                        this.bondModule.BondAxisX.AbsoluteMove(point3D.X);
                        this.bondModule.BondAxisY.AbsoluteMove(point3D.Y);
                        this.bondModule.BondHead.AxisZ.AbsoluteMove(point3D.Z);

                        Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                        // bond相机去拍照位
                        {
                            this.MoveToCameraCenter();
                        }

                        // Z-XY相机去拍照位
                        {
                            this.bondModule.BondHead.AxisZ.AbsoluteMove(this.GlobalCalibrationDomain.ZWithXYCameraHeight);

                            Thread.Sleep(this.GlobalCalibrationDomain.Delay);

                        RetryMatchZWithXY:
                            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.GlobalCalibrationDomain.PrNameZWithXY);
                            ExcuteResult re = pREntity.DoWork(false);
                            if (re == ExcuteResult.Success)
                            {
                                MatchResult result = (MatchResult)pREntity.AlgResult;

                                // 记录
                                AKRSPoint2D offset = new AKRSPoint2D((this.firstPointPixel.CenterX - result.CenterX) * this.ZWithXYCameraPixelRatio.X, (this.firstPointPixel.CenterY - result.CenterY) * this.ZWithXYCameraPixelRatio.Y);
                                this.resultZWithXY.Add(new AKRSPoint2D(this.bondModule.BondAxisX.GetCmdPosition(), this.bondModule.BondAxisY.GetCmdPosition()), offset);
                            }
                            else
                            {
                                System.Windows.Forms.DialogResult result = AKRSXtraMessageBox.Show("Z-XY未识别到mark! \r\n 点击yes,则重试;\r\n 点击no,则打开pr编辑;\r\n 点击cancel则结束", "Prompt", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                                switch (result)
                                {
                                    case DialogResult.Yes:
                                        goto RetryMatchZWithXY;
                                    case DialogResult.No:
                                        this.BtnEditPRZWithXY_Click(null, null);
                                        goto RetryMatchZWithXY;
                                    case DialogResult.Cancel:
                                        this.isStop = true;
                                        return;
                                }
                            }
                        }
                    }
                }

                this.SaveZWithXYData();
            }
        }*/


    }
}