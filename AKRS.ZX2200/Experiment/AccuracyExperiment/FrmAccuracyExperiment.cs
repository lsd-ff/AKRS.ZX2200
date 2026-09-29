using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Models.Enums;
using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.Experiment.AccuracyExperiment
{
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.Services;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Test;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using LanguageExt;
    using PostSharp;
    using System.Threading;
    using System.Windows.Forms;

    using DevExpress.XtraEditors;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 精度实验
    /// </summary>
    public partial class FrmAccuracyExperiment : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 精度实验
        /// </summary>
        public FrmAccuracyExperiment()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 固晶模组
        /// </summary>
        private readonly BondModule bondModule = new BondModule();

        /// <summary>
        /// 固晶相机拍照位置
        /// </summary>
        private AKRSPoint3D BondCameraVisionPos =>
            new AKRSPoint3D(
                (double)this.BtBondVisionPosX.Value,
                (double)this.BtBondVisionPosY.Value,
                (double)this.BtBondVisionPosZ.Value);

        /// <summary>
        /// 上视相机拍照位置
        /// </summary>
        private AKRSPoint3D UpLookCameraVisionPos =>
            new AKRSPoint3D(
                (double)this.BtUpLookVisionPosX.Value,
                (double)this.BtUpLookVisionPosY.Value,
                (double)this.BtUpLookVisionPosZ.Value);

        /// <summary>
        /// 固晶相机拍照位置
        /// </summary>
        private AKRSPoint3D PickPos =>
            new AKRSPoint3D(
                (double)this.BtPickPosX.Value,
                (double)this.SpPickPosY.Value,
                (double)this.SpPickPosZ.Value);

        /// <summary>
        /// 固晶相机拍照位置
        /// </summary>
        private AKRSPoint3D ReadyPos =>
            new AKRSPoint3D(
                (double)this.SpReadyPosX.Value,
                (double)this.SpReadyPosY.Value,
                (double)this.SpReadyPosZ.Value);

        /// <summary>
        /// 移动到点位
        /// </summary>
        /// <param name="point3D">点位</param>
        private void MoveToPos(AKRSPoint3D point3D)
        {
            double velX = this.bondModule.BondAxisX.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            double velY = this.bondModule.BondAxisY.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            double velZ = this.bondModule.BondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            //this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(5, velZ);

            //// 等轴到位
            //MotionService.WaitAxesArrival((this.bondModule.BondHead.AxisZ, true, 5, AccuracyMode.HighAccuracy));

           

            this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(point3D.Z, velZ);

            MotionService.WaitAxesArrival((this.bondModule.BondHead.AxisZ, true, point3D.Z, AccuracyMode.HighAccuracy));

            this.bondModule.BondAxisX.SendAbsoluteMoveCommand(point3D.X, velX);
            this.bondModule.BondAxisY.SendAbsoluteMoveCommand(point3D.Y, velY);

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisX, true, point3D.X, AccuracyMode.HighAccuracy),
                (this.bondModule.BondAxisY, true, point3D.Y, AccuracyMode.HighAccuracy));

           

           
        }

        /// <summary>
        /// 三轴直接移动到点位
        /// </summary>
        /// <param name="point3D">点位</param>
        private void MoveToPosAbsolute(AKRSPoint3D point3D)
        {
            double velX = this.bondModule.BondAxisX.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            double velY = this.bondModule.BondAxisY.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            double velZ = this.bondModule.BondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            this.bondModule.BondAxisX.SendAbsoluteMoveCommand(point3D.X, velX);
            this.bondModule.BondAxisY.SendAbsoluteMoveCommand(point3D.Y, velY);
            this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(point3D.Z, velZ);


            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisX, true, point3D.X, AccuracyMode.HighAccuracy),
                (this.bondModule.BondAxisY, true, point3D.Y, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisZ, true, point3D.Z, AccuracyMode.HighAccuracy));
        }

        /// <summary>
        /// 插补移动到上视拍照位置
        /// </summary>
        private void MoveToUpLookVisionPosContinueMove()
        {

        }

        /// <summary>
        /// 编辑固晶视觉模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEditBondCameraPR_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxBondCameraPrName.Text);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(this.TxBondCameraPrName.Text);

                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Calibration;


                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 编辑固晶视觉模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEditUpLookCameraPR_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(this.TxUpLookCameraPrName.Text);

                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Calibration;


                visionEntity = prEntity;

                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);

            editor.ShowDialog();

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 设置当前坐标
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtGetBondVisionPos_Click(object sender, EventArgs e)
        {
            AKRSPoint3D point3D = this.bondModule.Get3DRealPosition();
            this.BtBondVisionPosX.Value = (decimal)point3D.X;
            this.BtBondVisionPosY.Value = (decimal)point3D.Y;
            this.BtBondVisionPosZ.Value = (decimal)point3D.Z;
        }

        /// <summary>
        /// 设置当前坐标
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtGetUpLookVisionPos_Click(object sender, EventArgs e)
        {
            AKRSPoint3D point3D = this.bondModule.Get3DRealPosition();
            this.BtUpLookVisionPosX.Value = (decimal)point3D.X;
            this.BtUpLookVisionPosY.Value = (decimal)point3D.Y;
            this.BtUpLookVisionPosZ.Value = (decimal)point3D.Z;
        }

        /// <summary>
        /// 设置当前坐标
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtGetPickPos_Click(object sender, EventArgs e)
        {
            AKRSPoint3D point3D = this.bondModule.Get3DRealPosition();
            this.BtPickPosX.Value = (decimal)point3D.X;
            this.SpPickPosY.Value = (decimal)point3D.Y;
            this.SpPickPosZ.Value = (decimal)point3D.Z;
        }

        /// <summary>
        /// 固晶相机连续拍照实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtBondCameraContinueVisionTest_Click(object sender, EventArgs e)
        {
            this.MoveToPos(this.BondCameraVisionPos);

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxBondCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep((int)this.SpBondCameraDelay.Value);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// 上视相机连续拍照实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtUpLookCameraContinueVisionTest_Click(object sender, EventArgs e)
        {
            this.MoveToPos(this.UpLookCameraVisionPos);

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep((int)this.SpUpLookCameraDelay.EditValue);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// X轴运动实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtXAxisMoveTest_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxBondCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            double velX = this.bondModule.BondAxisX.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            this.MoveToPos(this.BondCameraVisionPos);

            for (int i = (int)this.bondModule.BondAxisX.AxisSetPara.NLimit + 1;
                 i < (int)this.bondModule.BondAxisX.AxisSetPara.PLimit - 1;
                 i = i + 5)
            {
                this.bondModule.BondAxisX.SendAbsoluteMoveCommand(i, velX);
                MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisX, true, i, AccuracyMode.HighAccuracy));

                this.bondModule.BondAxisX.SendAbsoluteMoveCommand((double)this.BtBondVisionPosX.Value, velX);

                MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisX, true, (double)this.BtBondVisionPosX.Value, AccuracyMode.HighAccuracy));

                Thread.Sleep((int)this.SpBondCameraDelay.Value);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
                
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// Y轴运动实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtYAxisMoveTest_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxBondCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            double velY = this.bondModule.BondAxisY.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            this.MoveToPos(this.BondCameraVisionPos);

            for (int i = (int)this.bondModule.BondAxisY.AxisSetPara.NLimit + 1;
                 i < (int)this.bondModule.BondAxisY.AxisSetPara.PLimit - 1;
                 i = i + 5)
            {
                this.bondModule.BondAxisY.SendAbsoluteMoveCommand(i, velY);
                MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisY, true, i, AccuracyMode.HighAccuracy));

                this.bondModule.BondAxisY.SendAbsoluteMoveCommand((double)this.BtBondVisionPosY.Value, velY);

                MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisY, true, (double)this.BtBondVisionPosY.Value, AccuracyMode.HighAccuracy));

                Thread.Sleep((int)this.SpBondCameraDelay.Value);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// XY轴运动实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtXYAxisMoveTest_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            this.MoveToPos(this.BondCameraVisionPos);

            for (int i = 0; i < 100; i++)
            {
                Random random = new Random();
                double x = random.Next(-300, 300) + (double)this.BtBondVisionPosX.Value;
                double y = random.Next(-300, 300) + (double)this.BtBondVisionPosY.Value;

                if (x > this.bondModule.BondAxisX.AxisSetPara.PLimit
                    || x < this.bondModule.BondAxisX.AxisSetPara.NLimit)
                {
                    i--;
                    continue;
                }

                if (y > this.bondModule.BondAxisY.AxisSetPara.PLimit
                    || y < this.bondModule.BondAxisY.AxisSetPara.NLimit)
                {
                    i--;
                    continue;
                }

                this.MoveToPosAbsolute(new AKRSPoint3D(x, y, (double)this.BtBondVisionPosZ.Value));

                this.MoveToPosAbsolute(this.BondCameraVisionPos);

                Thread.Sleep((int)this.SpUpLookCameraDelay.EditValue);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// X轴上视移动拍照
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtXAxisMoveTestUpLook_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            double velX = this.bondModule.BondAxisX.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            this.MoveToPos(this.UpLookCameraVisionPos);

            for (int i = (int)this.bondModule.BondAxisX.AxisSetPara.NLimit + 1;
                 i < (int)this.bondModule.BondAxisX.AxisSetPara.PLimit - 1;
                 i = i + 5)
            {
                this.bondModule.BondAxisX.SendAbsoluteMoveCommand(i, velX);
                MotionService.WaitAxesArrival(
                    (this.bondModule.BondAxisX, true, i, AccuracyMode.HighAccuracy));

                this.bondModule.BondAxisX.SendAbsoluteMoveCommand((double)this.BtUpLookVisionPosX.Value, velX);

                MotionService.WaitAxesArrival(
                    (this.bondModule.BondAxisX, true, (double)this.BtUpLookVisionPosX.Value, AccuracyMode.HighAccuracy));

                Thread.Sleep((int)this.SpUpLookCameraDelay.Value);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// Y轴上视移动拍照
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtYAxisMoveTestUpLook_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            double velX = this.bondModule.BondAxisY.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            this.MoveToPos(this.UpLookCameraVisionPos);

            for (int i = (int)this.bondModule.BondAxisY.AxisSetPara.NLimit + 1;
                 i < (int)this.bondModule.BondAxisY.AxisSetPara.PLimit - 1;
                 i = i + 5)
            {
                this.bondModule.BondAxisY.SendAbsoluteMoveCommand(i, velX);
                MotionService.WaitAxesArrival(
                    (this.bondModule.BondAxisY, true, i, AccuracyMode.HighAccuracy));

                this.bondModule.BondAxisY.SendAbsoluteMoveCommand((double)this.BtUpLookVisionPosY.Value, velX);

                MotionService.WaitAxesArrival(
                    (this.bondModule.BondAxisY, true, (double)this.BtUpLookVisionPosY.Value, AccuracyMode.HighAccuracy));

                Thread.Sleep((int)this.SpUpLookCameraDelay.Value);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// XY上视测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtXYAxisMoveTestUpLook_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            this.MoveToPos(this.BondCameraVisionPos);

            for (int i = 0; i < 100; i++)
            {
                Random random = new Random();
                double x = random.Next(-300, 300) + (double)this.BtBondVisionPosX.Value;
                double y = random.Next(-300, 300) + (double)this.BtBondVisionPosY.Value;

                if (x > this.bondModule.BondAxisX.AxisSetPara.PLimit
                    || x < this.bondModule.BondAxisX.AxisSetPara.NLimit)
                {
                    i--;
                    continue;
                }

                if (y > this.bondModule.BondAxisY.AxisSetPara.PLimit
                    || y < this.bondModule.BondAxisY.AxisSetPara.NLimit)
                {
                    i--;
                    continue;
                }

                this.MoveToPosAbsolute(new AKRSPoint3D(x, y, (double)this.BtBondVisionPosZ.Value));

                this.MoveToPosAbsolute(this.UpLookCameraVisionPos);

                Thread.Sleep((int)this.SpUpLookCameraDelay.EditValue);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// 固晶相机移动到相机中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtBondCameraMoveToCenter_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxBondCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                Random random = new Random();
                double x = random.Next(-200, 200) / 100.0 + (double)this.BtBondVisionPosX.Value;
                double y = random.Next(-200, 200) / 100.0 + (double)this.BtBondVisionPosY.Value;
                
                // 移动到随机位置拍照
                this.MoveToPos(new AKRSPoint3D(x, y, (double)this.BtBondVisionPosZ.Value));
                Thread.Sleep((int)this.SpBondCameraDelay.EditValue);
                prEntity.DoWork();
                MatchResult matchResult = (MatchResult)prEntity.AlgResult;

                // 转化定位结果
                AKRSPoint3D point3D = this.bondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
                this.MoveToPosAbsolute(this.bondModule.Get3DRealPosition() + point3D);
                Thread.Sleep((int)this.SpBondCameraDelay.EditValue);
                prEntity.DoWork();
                MatchResult matchResultAfterMove = (MatchResult)prEntity.AlgResult;

                result.Add(
                    new List<double>()
                        {
                            matchResultAfterMove.CenterX,
                            matchResultAfterMove.CenterY,
                            matchResult.CenterX,
                            matchResult.CenterY
                        });
            }

            List<string> listName = new List<string>() { "回中心精度X", "回中心精度Y", "拍照结果X", "拍照结果Y" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// 上视相机移动到相机中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtUpLookCameraMoveToCenter_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                Random random = new Random();
                double x = random.Next(-200, 200) / 100.0 + (double)this.BtUpLookVisionPosX.Value;
                double y = random.Next(-200, 200) / 100.0 + (double)this.BtUpLookVisionPosY.Value;

                // 移动到随机位置拍照
                this.MoveToPos(new AKRSPoint3D(x, y, (double)this.BtUpLookVisionPosZ.Value));
                Thread.Sleep((int)this.SpUpLookCameraDelay.EditValue);
                prEntity.DoWork();
                MatchResult matchResult = (MatchResult)prEntity.AlgResult;

                // 转化定位结果
                UpLookModule upLookModule = new UpLookModule();
                AKRSPoint3D point3D = upLookModule.UpLookCameraCoordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0));
                this.MoveToPosAbsolute(this.bondModule.Get3DRealPosition() + point3D);
                Thread.Sleep((int)this.SpBondCameraDelay.EditValue);
                prEntity.DoWork();
                MatchResult matchResultAfterMove = (MatchResult)prEntity.AlgResult;

                result.Add(
                    new List<double>()
                        {
                            matchResultAfterMove.CenterX,
                            matchResultAfterMove.CenterY,
                            matchResult.CenterX,
                            matchResult.CenterY
                        });
            }

            List<string> listName = new List<string>() { "回中心精度X", "回中心精度Y", "拍照结果X", "拍照结果Y" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// 取片完去看一个点的精度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtContinuePick_Click(object sender, EventArgs e)
        {
            PREntity prEntityBond = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);
            PREntity prEntityUpLook = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);
            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                this.MoveToPos(this.PickPos);

                Thread.Sleep(100);

                this.MoveToPos(this.UpLookCameraVisionPos);

                Thread.Sleep((int)this.SpUpLookCameraDelay.EditValue);

                prEntityUpLook.DoWork();

                MatchResult matchResult1 = (MatchResult)prEntityUpLook.AlgResult;

                this.MoveToPos(this.BondCameraVisionPos);

                Thread.Sleep((int)this.SpUpLookCameraDelay.EditValue);
                
                prEntityBond.DoWork();

                MatchResult matchResult2 = (MatchResult)prEntityBond.AlgResult;

                result.Add(new List<double>() { matchResult1.CenterX, matchResult1.CenterY, matchResult2.CenterX, matchResult2.CenterY });
            }

            List<string> listName = new List<string>() { "上视精度X", "上视精度Y", "固晶精度X", "固晶精度Y" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// 移动到上视的拍照精度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtMoveToUpLookAfterPick_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxUpLookCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            for (int i = 0; i < 100; i++)
            {
                // 移动到取片位
                this.MoveToPos(this.PickPos);

                // 移动到上视
                this.MoveToUpLookVisionPosContinueMove();

                // 定位
                Thread.Sleep((int)this.SpBondCameraDelay.EditValue);
                prEntity.DoWork();
                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "上视精度X", "上视精度Y" };
            List<double> listStandard = new List<double>() { 0.5, 0.5, 0.05 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }

        /// <summary>
        /// 中转台连续取放测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtPickDownTest_Click(object sender, EventArgs e)
        {
            // 移动到中转台
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAccuracyExperiment_Load(object sender, EventArgs e)
        {
           DialogResult dialog = AKRSXtraMessageBox.Show("本实验需要经过培训的专业人员操作，未经允许不得开启", "提示", MessageBoxButtons.YesNo);

           if (dialog != DialogResult.Yes)
           {
                this.Close();
           }
        }

        /// <summary>
        /// 获取待机位置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtGetReadyPos_Click(object sender, EventArgs e)
        {
            AKRSPoint3D point3D = this.bondModule.Get3DRealPosition();
            this.SpReadyPosX.Value = (decimal)point3D.X;
            this.SpReadyPosY.Value = (decimal)point3D.Y;
            this.SpReadyPosZ.Value = (decimal)point3D.Z;
        }

        /// <summary>
        /// 相机来回实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDownLookCycleTest_Click(object sender, EventArgs e)
        {
            double cycle = XtraInputBox.Show("实验次数", "次数", 10);

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.TxBondCameraPrName.Text);

            List<List<double>> result = new List<List<double>>();

            double velX = this.bondModule.BondAxisY.AxisMovePara.AbsoluteMoveSpeed * (double)this.SpSpeed.Value / 100.0;

            for (int i = 0; i < cycle; i++)
            {
                this.MoveToPos(this.ReadyPos);

                this.MoveToPos(this.BondCameraVisionPos);

                Thread.Sleep((int)this.SpUpLookCameraDelay.Value);

                prEntity.DoWork();

                MatchResult matchResult = (MatchResult)prEntity.AlgResult;
                result.Add(new List<double>() { matchResult.CenterX, matchResult.CenterY });
            }

            List<string> listName = new List<string>() { "精度X", "精度Y", "精度角度" };
            List<double> listStandard = new List<double>() { 0, 0, 0 };

            FrmAccuracyExperimentResult frmAccuracyExperimentResult =
                new FrmAccuracyExperimentResult(result, listName, listStandard);
            frmAccuracyExperimentResult.ShowDialog();
        }
    }
}