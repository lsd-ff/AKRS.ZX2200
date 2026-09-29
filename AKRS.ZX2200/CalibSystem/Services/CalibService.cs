using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using HalconDotNet;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AKRS.ZX2200.CalibSystem.Services
{
    using System.Windows.Forms;

    using Axis = AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers.Axis;

    /// <summary>
    /// 标定服务
    /// </summary>
    public class CalibService
    {
        /// <summary>
        /// 获取当前定位点现实坐标(坐标系)
        /// </summary>
        /// <param name="curAxisPoint">当前轴坐标</param>
        /// <param name="locationPoint">定位图像坐标</param>
        /// <param name="coordinateName">相机类型</param>
        /// <returns>当前绝对坐标</returns>
        public static AKRSPoint2D GetMachinePosByPixelPos(
            AKRSPoint2D curAxisPoint,
            MatchResult locationPoint,
            string coordinateName)
        {
            if (locationPoint == null)
            {
                throw new ArgumentNullException(nameof(locationPoint), "The locationPoint parameter cannot be null.");
            }

            DependentCoordinateSystem coordinateSystem = (DependentCoordinateSystem)MachineCoordinateSystem
                .GetInstance().CoordinateSystems.Find(it => it.Name == coordinateName);

            AKRSPoint3D locationCenter =
                coordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(locationPoint.CenterX, locationPoint.CenterY, 0));

            return new AKRSPoint2D(curAxisPoint.X + locationCenter.X, curAxisPoint.Y + locationCenter.Y);
        }

        /// <summary>
        /// 移动到相机中心(坐标系)
        /// </summary>
        /// <param name="axisX">X轴</param>
        /// <param name="axisY">Y轴</param>
        /// <param name="locationPoint">定位坐标</param>
        /// <param name="coordinateName">坐标系名称</param>
        public static void MoveToCamCenterCoo(Axis axisX, Axis axisY, MatchResult locationPoint, string coordinateName)
        {
            DependentCoordinateSystem coordinateSystem = (DependentCoordinateSystem)MachineCoordinateSystem
                .GetInstance().CoordinateSystems.Find(it => it.Name == coordinateName);

            AKRSPoint3D locationCenter =
                coordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(locationPoint.CenterX, locationPoint.CenterY, 0));

            double xRealPosition = axisX.GetCmdPosition();
            double yRealPosition = axisY.GetCmdPosition();

            axisX.AbsoluteMove(xRealPosition + locationCenter.X);
            axisY.AbsoluteMove(yRealPosition + locationCenter.Y);

            //axisX.RelativeMove(locationCenter.X);
            //axisY.RelativeMove(locationCenter.Y);
        }

        /// <summary>
        /// 根据中心点及间距生成点位列表
        /// </summary>
        /// <param name="centerPoint">中心点</param>
        /// <param name="distance">间距</param>
        /// <param name="numRows">行数</param>
        /// <param name="numColumns">列数</param>
        /// <returns>点集</returns>
        public static List<AKRSPoint2D> GeneratePointList(
            AKRSPoint3D centerPoint,
            double distance,
            int numRows,
            int numColumns)
        {
            List<AKRSPoint2D> pointList = new List<AKRSPoint2D>();

            double startX = centerPoint.X - ((numColumns - 1) / 2.0 * distance); // 左上角点的X坐标
            double startY = centerPoint.Y - ((numRows - 1) / 2.0 * distance); // 左上角点的Y坐标

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    double x = startX + (j * distance);
                    double y = startY + (i * distance);
                    AKRSPoint2D point = new AKRSPoint2D(x, y);
                    pointList.Add(point);
                }
            }

            return pointList;
        }

        /// <summary>
        /// 根据中心点及X Y 间距生成点位列表
        /// </summary>
        /// <param name="centerPoint">中心点</param>
        /// <param name="distanceX">间距</param>
        /// <param name="distanceY">间距</param>
        /// <param name="numRows">行数</param>
        /// <param name="numColumns">列数</param>
        /// <returns>点集</returns>
        public static List<AKRSPoint2D> GeneratePointList(
            AKRSPoint3D centerPoint,
            double distanceX,
            double distanceY,
            int numRows,
            int numColumns)
        {
            List<AKRSPoint2D> pointList = new List<AKRSPoint2D>();

            double startX = centerPoint.X - ((numColumns - 1) / 2.0 * distanceX); // 左上角点的X坐标
            double startY = centerPoint.Y - ((numRows - 1) / 2.0 * distanceY); // 左上角点的Y坐标

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    double x = startX + (j * distanceX);
                    double y = startY + (i * distanceY);
                    AKRSPoint2D point = new AKRSPoint2D(x, y);
                    pointList.Add(point);
                }
            }

            return pointList;
        }

        /// <summary>
        /// 点集拟合圆
        /// </summary>
        /// <param name="points">点集</param>
        /// <param name="centerX">中心坐标X</param>
        /// <param name="centerY">中心坐标Y</param>
        /// <param name="radius">半径</param>
        public static void FitCircle(
            List<AKRSPoint2D> points,
            out double centerX,
            out double centerY,
            out double radius)
        {
            HObject circleContour;
            HOperatorSet.GenEmptyObj(out circleContour);

            HTuple rows = new HTuple();
            HTuple columns = new HTuple();

            foreach (var point in points)
            {
                rows = rows.TupleConcat(point.Y);
                columns = columns.TupleConcat(point.X);
            }

            HOperatorSet.GenContourPolygonXld(out circleContour, rows, columns);

            HOperatorSet.FitCircleContourXld(
                circleContour,
                "algebraic",
                -1,
                0,
                0,
                3,
                2,
                out HTuple hv_Row,
                out HTuple hv_Col,
                out HTuple hv_Radius,
                out HTuple hv_StartPhi,
                out HTuple hv_EndPhi,
                out HTuple hv_PointOrder);

            centerX = hv_Col.D;
            centerY = hv_Row.D;
            radius = hv_Radius.D;

            circleContour.Dispose();
        }

        /// <summary>
        /// N点转换工具(Halcon)
        /// </summary>
        /// <param name="originalPoints">初始点</param>
        /// <param name="transformedPoints">转换点</param>
        /// <param name="homMatTrans">转换矩阵</param>
        public static void NPointTrans(
            List<AKRSPoint2D> originalPoints,
            List<AKRSPoint2D> transformedPoints,
            out HTuple homMatTrans)
        {
            HTuple px = new HTuple();
            HTuple py = new HTuple();
            HTuple qx = new HTuple();
            HTuple qy = new HTuple();

            for (int i = 0; i < originalPoints.Count; i++)
            {
                px = px.TupleConcat(originalPoints[i].X);
                py = py.TupleConcat(originalPoints[i].Y);
            }

            for (int i = 0; i < transformedPoints.Count; i++)
            {
                qx = qx.TupleConcat(transformedPoints[i].X);
                qy = qy.TupleConcat(transformedPoints[i].Y);
            }

            HOperatorSet.VectorToHomMat2d(px, py, qx, qy, out homMatTrans);
        }

        public static List<double> CalcTransMatrixHalcon(List<AKRSPoint2D> cameraPoints, List<AKRSPoint2D> axisPoints)
        {
            NPointTrans(cameraPoints, axisPoints, out HTuple homMat2D);
            double[] homMat2DArray = homMat2D.DArr;

            return homMat2DArray.ToList();
        }

        /// <summary>
        /// 转换点(Halcon)
        /// </summary>
        /// <param name="originalPoint">初始点</param>
        /// <param name="homat2d">转换矩阵</param>
        /// <param name="transPoint">转换点</param>
        public static void TransPoint(AKRSPoint2D originalPoint, HTuple homat2d, out AKRSPoint2D transPoint)
        {
            double initialX = originalPoint.X;
            double initialY = originalPoint.Y;
            HTuple transX, transY;
            HOperatorSet.AffineTransPoint2d(homat2d, initialX, initialY, out transX, out transY);

            transPoint = new AKRSPoint2D(transX.D, transY.D);
        }

        public static AKRSPoint2D TransPoint(AKRSPoint2D originalPoint, double x1y1, double x1y2, double x2y1, double x2y2)
        {
            HTuple value = new HTuple(x1y1, x1y2, 0, x2y1, x2y2, 0, 0, 0, 1);
            HHomMat2D homMat2D = new HHomMat2D(value) ;
        
            //HOperatorSet.CreateMatrix(3, 3, value, out HTuple matrixID);
            //HOperatorSet.GetFullMatrix
            double initialX = originalPoint.X;
            double initialY = originalPoint.Y;
            HTuple transX, transY;
            HOperatorSet.AffineTransPoint2d(homMat2D, initialX, initialY, out transX, out transY);

            return new AKRSPoint2D(transX, transY);
        }

        /// <summary>
        /// 计算两点角度
        /// </summary>
        /// <param name="pointA">点A</param>
        /// <param name="pointB">点B</param>
        /// <returns>角度</returns>
        public static double CalculateAngle(AKRSPoint2D pointA, AKRSPoint2D pointB)
        {
            // 创建两个点的对象
            HObject point1, point2;
            HOperatorSet.GenRegionPoints(out point1, pointA.X, pointA.Y);
            HOperatorSet.GenRegionPoints(out point2, pointB.X, pointB.Y);

            // 计算两点之间的角度
            HTuple angle;
            HOperatorSet.AngleLx(pointA.X, pointA.Y, pointB.X, pointB.Y, out angle);

            // 将角度转换为度数
            double angleInDegrees = angle.D * 180 / Math.PI;

            point1.Dispose();
            point2.Dispose();

            return angleInDegrees;
        }

        /// <summary>
        /// 求两点距离
        /// </summary>
        /// <param name="point1">点1</param>
        /// <param name="point2">点2</param>
        /// <returns>距离</returns>
        public static double CalculateDistance(AKRSPoint3D point1, AKRSPoint2D point2)
        {
            double result = 0;
            result = Math.Sqrt(
                (point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
            return Math.Round(result, 6);
        }


        /// <summary>
        /// 创建像素比对象
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        /// <param name="cameraScaleX">x像素比</param>
        /// <param name="cameraScaleY">y像素比</param>
        /// <param name="cameraSlantTheta">相机倾斜角度</param>
        /// <returns>相机像素比对象</returns>
        public static CalibCamScaleResult CreateCalibCamScale(
            string cameraType,
            double cameraScaleX,
            double cameraScaleY,
            double cameraSlantTheta)
        {
            return new CalibCamScaleResult
            {
                CameraType = cameraType,
                CameraScaleX = cameraScaleX,
                CameraScaleY = cameraScaleY,
                CameraSlantTheata = cameraSlantTheta
            };
        }

        /// <summary>
        /// 创建转换数据对象
        /// </summary>
        /// <param name="paraName">参数名</param>
        /// <param name="transX">平行X</param>
        /// <param name="transY">平行Y</param>
        /// <param name="relativeTheta">角度</param>
        /// <returns>转换结果对象</returns>
        public static CalibTransResult CreateCalibTransResult(
            string paraName,
            double transX,
            double transY,
            double relativeTheta)
        {
            return new CalibTransResult
            {
                ParaName = paraName,
                RelativeX = transX,
                RelativeY = transY,
                RelativeTheata = relativeTheta
            };
        }
    }
}
