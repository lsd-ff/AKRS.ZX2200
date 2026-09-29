using AKRS.Galaxy2.AutoFocusing;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.LogicHardware.Services;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.DispenseSystem.Modules;
using AKRS.ZX2200.WaferSubSystem.Modules;
using System;
using System.Threading;

namespace AKRS.ZX2200.CalibSystem.Services
{
    using System.Windows.Forms;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    using log4net.Repository.Hierarchy;

    /// <summary>
    /// 标定控制器
    /// </summary>
    public class CalibController
    {
        /// <summary>
        /// 点胶模组
        /// </summary>
        private readonly DispenseModule dispenseModule = new DispenseModule();

        /// <summary>
        /// Bond模组
        /// </summary>
        private BondModule bondModule = new BondModule(); 
         
        /// <summary>
        /// Wafer模组
        /// </summary>
        private WaferTableModule waferTableModule = new WaferTableModule();

        /// <summary>
        /// 移动点胶到机械位置3D
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveDispenseToMachinePos(AKRSPoint3D pos)
        {
            //this.dispenseModule.DispenseAxisZ.AbsoluteMove(0);
            //MotionService.MoveAxesToTargetPosition(
            //    (this.dispenseModule.DispenseAxisX, false, pos.X, AccuracyMode.HighAccuracy),
            //    (this.dispenseModule.DispenseAxisY, false, pos.Y, AccuracyMode.HighAccuracy));

            //this.dispenseModule.DispenseAxisZ.AbsoluteMove(pos.Z);

            this.dispenseModule.AxisZAbsoluteMove(0);
            this.dispenseModule.AxisXYAbsoluteMove(pos.X, pos.Y);
            this.dispenseModule.AxisZAbsoluteMove(pos.Z);
        }

        /// <summary>
        /// 移动点胶到机械位置2D
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveDispenseToMachinePos(AKRSPoint2D pos)
        {
            //double zPos = this.dispenseModule.DispenseAxisZ.GetRealPosition();
            //this.dispenseModule.DispenseAxisZ.AbsoluteMove(0);
            //MotionService.MoveAxesToTargetPosition(
            //    (this.dispenseModule.DispenseAxisX, false, pos.X, AccuracyMode.HighAccuracy),
            //    (this.dispenseModule.DispenseAxisY, false, pos.Y, AccuracyMode.HighAccuracy));
            //this.dispenseModule.DispenseAxisZ.AbsoluteMove(zPos);

            double zPos = this.dispenseModule.GetAxisPos().Z;
            this.MoveDispenseToMachinePos(new AKRSPoint3D(pos.X, pos.Y, zPos));
        }

        /// <summary>
        /// 点胶Z移动到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveDispenseZToMachinePos(double pos)
        {
            // this.dispenseModule.DispenseAxisZ.AbsoluteMove(pos);
            this.dispenseModule.AxisZAbsoluteMove(pos);
        }

        /// <summary>
        /// 点胶Z移动到安全位置
        /// </summary>
        public void MoveDispenseZToSafePos()
        {
            // this.dispenseModule.DispenseAxisZ.AbsoluteMove(0);
            this.dispenseModule.AxisZAbsoluteMove(0);
        }

        /// <summary>
        /// 移动晶圆相机Z到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveWaferCameraZ(double pos)
        {
            this.waferTableModule.WaferCameraAxisZ.AbsoluteMove(pos);
        }

        /// <summary>
        /// 移动晶圆台到指定位置
        /// </summary>
        /// <param name="pos">位置</param>
        public void MoveWaferTableToMachinePos(AKRSPoint3D pos)
        {
            MotionService.MoveAxesToTargetPosition((this.waferTableModule.WaferTableAxisX, true, pos.X, AccuracyMode.HighAccuracy), (this.waferTableModule.WaferTableAxisY, true, pos.Y, AccuracyMode.HighAccuracy));
        }

        /// <summary>
        /// 获取晶圆台真实坐标3d
        /// </summary>
        /// <returns>晶圆台坐标</returns>
        public AKRSPoint3D GetWaferTableRealPos()
        {
            double x = this.waferTableModule.WaferTableAxisX.GetRealPosition();
            double y = this.waferTableModule.WaferTableAxisY.GetRealPosition();
            double z = this.waferTableModule.WaferCameraAxisZ.GetRealPosition();

            return new AKRSPoint3D(x, y, z);
        }

        /// <summary>
        /// 移动到相机中心
        /// </summary>
        /// <param name="camCoordinateType">坐标系类型</param>
        /// <param name="locateResult">定位坐标</param>
        public void MoveToCamCenter(CamCoordinateType camCoordinateType, MatchResult locateResult)
        {
            if (camCoordinateType == CamCoordinateType.Bond)
            {
                CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, locateResult, "BondCameraCoordinateSystem");
            }
            else if (camCoordinateType == CamCoordinateType.UpLook)
            {
                CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, locateResult, "UpLookCameraCoordinateSystem");
            }
            else if (camCoordinateType == CamCoordinateType.Dispense)
            {
                CalibService.MoveToCamCenterCoo(this.dispenseModule.GetDispenseXAxis(), this.dispenseModule.GetDispenseYAxis(), locateResult, "DispenseCameraCoordinateSystem");
            }
            else if (camCoordinateType == CamCoordinateType.Wafer)
            {
                CalibService.MoveToCamCenterCoo(this.waferTableModule.WaferTableAxisX, this.waferTableModule.WaferTableAxisY, locateResult, "WaferCameraCoordinateSystem");
            }
        }

        /// <summary>
        /// 定位目标并移动至指定像素点中心（1224,1024），含偏差校验与重试
        /// </summary>
        /// <param name="prName">PR模板名称</param>
        /// <param name="camType">坐标系类型</param>
        /// <param name="pixelTolerance">像素偏差阈值（单位：像素）</param>
        /// <param name="maxRetry">最大重试次数</param>
        /// <returns>是否成功将目标移动至阈值范围内</returns>
        public bool LocateAndMoveToTargetPixel(string prName, CamCoordinateType camType, double pixelTolerance, int maxRetry)
        {
            int retryCount = 0;
            MatchResult currentMatchResult = null;
            bool isWithinTolerance = false;

            // 目标像素点（固定为1224,1024）
            (double targetX, double targetY) = (1224, 1024);

            // 记录最终偏差（用于日志）
            (double finalDeltaX, double finalDeltaY) = (0, 0);

            while (retryCount < maxRetry)
            {
                // 1. 首次定位并移动
                currentMatchResult = LocateAndMoveOnce(prName, camType);
                if (currentMatchResult == null || !currentMatchResult.IsSuccess)
                {
                    retryCount++;
                    continue;
                }

                // 2. 重新定位（偏差计算专用）并计算与目标像素点的差值
                var(deltaX, deltaY) = CalculatePixelDeviation(prName, targetX, targetY);

                (finalDeltaX, finalDeltaY) = (deltaX, deltaY);


                // 3. 检查偏差是否在阈值内
                if (Math.Abs(deltaX) < pixelTolerance && Math.Abs(deltaY) < pixelTolerance)
                {
                    isWithinTolerance = true;
                    break;
                }

                retryCount++;
            }

            if (!isWithinTolerance)
            {
                // var finalDeviation = CalculatePixelDeviation(prName, targetX, targetY);

                DialogResult dialog = AKRSXtraMessageBox.Show($"超过最大重试次数（{ maxRetry}次），当前与目标像素点(1224, 1024)偏差：X方向{ finalDeltaX: F2}像素，Y方向{ finalDeltaY: F2}像素",
                    "Prompt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            return isWithinTolerance;
        }

        /// <summary>
        /// 单次定位并移动到相机中心
        /// </summary>
        /// <param name="prName">PR名称</param>
        /// <param name="camType">相机类型</param>
        /// <returns></returns>
        private MatchResult LocateAndMoveOnce(string prName, CamCoordinateType camType)
        {
            var matchResult = this.LocatePosition(prName);
            if (matchResult != null && matchResult.IsSuccess)
            {
                this.MoveToCamCenter(camType, matchResult);
            }
            return matchResult;
        }

        /// <summary>
        /// 重新定位并计算与目标像素点的偏差（像素单位）
        /// </summary>
        /// <param name="prName">PR模板名称</param>
        /// <param name="targetX">目标X像素</param>
        /// <param name="targetY">目标Y像素</param>
        /// <returns>偏差值（像素）</returns>
        private (double deltaX, double deltaY) CalculatePixelDeviation(string prName, double targetX, double targetY)
        {
            // 重新定位获取最新位置（偏差计算专用）
            var latestResult = this.LocatePosition(prName);
            if (latestResult == null || !latestResult.IsSuccess)
            {
                return (double.MaxValue, double.MaxValue); // 定位失败时返回最大偏差
            }

            // 计算当前位置与目标像素点的差值（像素单位）
            double deltaX = latestResult.CenterX - targetX;
            double deltaY = latestResult.CenterY - targetY;

            // 返回欧氏距离作为偏差（也可根据需求返回X/Y轴独立偏差）
            return (deltaX, deltaY);
        }

        public bool MoveWaferAndEjectorZ(double targetZCam, double targetZEjector)
        {
            try
            {


                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// 自动对焦
        /// </summary>
        /// <param name="curCam">当前相机</param>
        /// <param name="startPos">开始位置</param>
        /// <param name="endPos">结束位置</param>
        public void AutoFocus(CamCoordinateType curCam, double startPos, double endPos)
        {
            if (curCam == CamCoordinateType.Bond)
            {
                AutoFocusing autofocus = new AutoFocusing();
                autofocus.AutoFocus(this.bondModule.BondCamera, this.bondModule.BondHead.AxisZ, endPos, startPos);
            }
            else if (curCam == CamCoordinateType.UpLook)
            {
                AutoFocusing autofocus = new AutoFocusing();
                autofocus.AutoFocus(this.bondModule.BondCamera, this.bondModule.BondHead.AxisZ, endPos, startPos);
            }
            else if (curCam == CamCoordinateType.Dispense)
            {

            }
            else if (curCam == CamCoordinateType.Wafer)
            {

            }
        }

        /// <summary>
        /// 相机坐标系类型
        /// </summary>
        public enum CamCoordinateType
        {
            // Bond相机坐标系
            Bond,

            // UpLook相机坐标系
            UpLook,

            // 晶圆相机坐标系
            Wafer,

            // 点胶相机坐标系
            Dispense
        }

        /// <summary>
        /// 模板定位
        /// </summary>
        /// <param name="patternName">模板名称</param>
        /// <returns>定位结果</returns>
        public MatchResult LocatePosition(string patternName)
        {
            // 获取Pr实体
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(patternName);

            Thread.Sleep(200);
            ExcuteResult excuteResult = pREntity.DoWork();

            // 拍照失败，直接返回错误
            if (excuteResult != ExcuteResult.Success)
            {
                throw new ArgumentNullException(patternName, "The" + patternName + " excuteResult is fail.");
            }

            // 获取定位结果
            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            return matchResult;
        }
    }
}
