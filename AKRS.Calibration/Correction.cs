using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;

using GlobalVariableModuleCs;

using HalconDotNet;

using VM.Core;
using VM.PlatformSDKCS;
using PointF = System.Drawing.PointF;

namespace AKRS.Calibration
{
    /// <summary>
    /// 纠偏类
    /// </summary>
    public class Correction
    {
        /// <summary>
        /// 晶圆转换流程
        /// </summary>
        private static VmProcedure waferTransPrc;

        /// <summary>
        /// Bond转换流程
        /// </summary>
        private static VmProcedure bondTransPrc;

        /// <summary>
        /// 上视流程
        /// </summary>
        private static VmProcedure upLookTransPrc;

        /// <summary>
        /// 点胶流程
        /// </summary>
        private static VmProcedure dripTransPrc;
        
        /// <summary>
        /// 单例
        /// </summary>
        private static Correction instance;

        /// <summary>
        /// 创建单例
        /// </summary>
        /// <returns>单例对象</returns>
        public static Correction GetInstance()
        {
            if (instance == null)
            {
                instance = new Correction();
            }
            return instance;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Correction()
        {
            //this.Init();

            //// (double x, double y, double a) = CalculateMidpoint(1, 1, 2, 2);
        }

        /// <summary>
        /// 初始化流程
        /// </summary>
        private void Init()
        {
            try
            {
                if (waferTransPrc == null)
                { 
                    waferTransPrc = VmProcedure.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\晶圆台标定转换.prc");
                }

                if (bondTransPrc == null)
                { 
                    bondTransPrc = VmProcedure.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\邦头标定转换.prc");
                }

                if (upLookTransPrc == null)
                {
                    upLookTransPrc = VmProcedure.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上视标定转换.prc");
                }

                if (dripTransPrc == null)
                { 
                    dripTransPrc = VmProcedure.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\点胶标定转换.prc");
                }

                // VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\标定转换.sol");

                // PtoPCorrect(1, 1, 0, 1, 1, 0, out float x, out float y, out float a);
            }
            catch (Exception)
            {
                MessageBox.Show("标定转换方案加载失败");
            }

        }

        /// <summary>
        /// 计算上视相机处旋转后结果距模板中心偏差值
        /// </summary>
        /// <param name="camP1BasePoint">P1模板轴坐标</param>
        /// <param name="p1BasePoint">P1模板坐标</param>
        /// <param name="camP1MarkPoint">P1定位轴坐标</param>
        /// <param name="p1MarkPoint">P1定位坐标</param>
        /// <param name="rotateCenter">旋转中心</param>
        /// <param name="angle">角度</param>
        /// <param name="offsetX">X偏移</param>
        /// <param name="offsetY">Y偏移</param>
        private void CalcUpLookOffset(PointF camP1BasePoint, PointF p1BasePoint, PointF camP1MarkPoint, PointF p1MarkPoint, PointF rotateCenter, double angle, out double offsetX, out double offsetY)
        {
            offsetX = 0; 
            offsetY = 0;

            PointF p1AbsBasePoint = new PointF();
            PointF p1AbsMarkPoint = new PointF();

            double p1BaseWorldX;
            double p1BaseWorldY;
            double p1MarkWorldX;
            double p1MarkWorldY;

            this.InvokeUpLookCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
            this.InvokeUpLookCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);

            p1AbsBasePoint.X = (float)(camP1BasePoint.X + p1BaseWorldX);
            p1AbsBasePoint.Y = (float)(camP1BasePoint.Y + p1BaseWorldY);

            p1AbsMarkPoint.X = (float)(camP1MarkPoint.X + p1MarkWorldX);
            p1AbsMarkPoint.Y = (float)(camP1MarkPoint.Y + p1MarkWorldY);

            PointF rotatedMarkPoint = Rotate(p1AbsMarkPoint, rotateCenter, angle);

            offsetX = rotatedMarkPoint.X - p1AbsBasePoint.X;
            offsetY = rotatedMarkPoint.Y - p1AbsBasePoint.Y;
        }
        
        /// <summary>
        /// 计算邦头处定位结果距模版中心偏差值
        /// </summary>
        /// <param name="workType">类型</param>
        /// <param name="camBaseP1Point">p1轴坐标</param>
        /// <param name="p1BasePoint">p1模板坐标</param>
        /// <param name="camMarkP1Point">p1定位轴坐标</param>
        /// <param name="p1MarkPoint">p1定位坐标</param>
        /// <param name="offsetX">偏移x</param>
        /// <param name="offsetY">偏移y</param>
        private void CalcBondOffset(string workType, PointF camBaseP1Point, PointF p1BasePoint, PointF camMarkP1Point, PointF p1MarkPoint, out double offsetX, out double offsetY)
        {
            PointF p1AbsBasePoint = new PointF();
            PointF p1AbsMarkPoint = new PointF();

            double p1BaseWorldX = 0;
            double p1BaseWorldY = 0;
            double p1MarkWorldX = 0;
            double p1MarkWorldY = 0;

            if (workType == "Drip")
            {
                this.InvokeDripCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeDripCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
            }
            else if (workType == "Bond")
            {
                this.InvokeBondCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeBondCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
            }

            p1AbsBasePoint.X = (float)(camBaseP1Point.X + p1BaseWorldX);
            p1AbsBasePoint.Y = (float)(camBaseP1Point.Y + p1BaseWorldY);

            p1AbsMarkPoint.X = (float)(camMarkP1Point.X + p1MarkWorldX);
            p1AbsMarkPoint.Y = (float)(camMarkP1Point.Y + p1MarkWorldY);

            offsetX = p1AbsMarkPoint.X - p1AbsBasePoint.X;
            offsetY = p1AbsMarkPoint.Y - p1AbsBasePoint.Y;
        }

        /// <summary>
        /// 计算总体偏移量
        /// </summary>
        /// <param name="upLookP1CamBasePoint">上视p1模板轴坐标</param>
        /// <param name="upLookP1BasePoint">上视p1模板坐标</param>
        /// <param name="upLookP1CamMarkPoint">上视p1定位轴坐标</param>
        /// <param name="upLookP1MarkPoint">上视p1定位坐标</param>
        /// <param name="upLookAngle">上视角度</param>
        /// <param name="bondP1CamBasePoint">Bondp1模板轴坐标</param>
        /// <param name="bondP1BasePoint">Bondp1模板坐标</param>
        /// <param name="bondP1CamMarkPoint">Bondp1定位轴坐标</param>
        /// <param name="bondP1MarkPoint">Bondp1定位坐标</param>
        /// <param name="bondAngle">贴片角度</param>
        /// <param name="rotateCenter">旋转中心</param>
        /// <param name="offsetX">x偏移</param>
        /// <param name="offsetY">y偏移</param>
        /// <param name="offsetAngle">角度偏移</param>
        public void CalcOffset(PointF upLookP1CamBasePoint, PointF upLookP1BasePoint, PointF upLookP1CamMarkPoint, PointF upLookP1MarkPoint, double upLookAngle, PointF bondP1CamBasePoint, PointF bondP1BasePoint, PointF bondP1CamMarkPoint, PointF bondP1MarkPoint, double bondAngle, PointF rotateCenter, out double offsetX, out double offsetY, out double offsetAngle)
        {
            double bondOffsetX = 0, bondOffsetY = 0;
            double rotateAngle = 0;
            double upLookOffsetX = 0, upLookOffsetY = 0;

            // 计算焊点定位相对于焊点模版偏移量
            this.CalcBondOffset("Bond", bondP1CamBasePoint, bondP1BasePoint, bondP1CamMarkPoint, bondP1MarkPoint, out bondOffsetX, out bondOffsetY);

            // 计算需矫正角度
            rotateAngle = upLookAngle + bondAngle;

            // 计算上视旋转后距离上视模版偏移量
            this.CalcUpLookOffset(upLookP1CamBasePoint, upLookP1BasePoint, upLookP1CamMarkPoint, upLookP1MarkPoint, rotateCenter, 0, out upLookOffsetX, out upLookOffsetY);

            // 计算整体偏移量（上视与邦头处相加）
            offsetX = bondOffsetX + upLookOffsetX;
            offsetY = bondOffsetY + upLookOffsetY;
            offsetAngle = rotateAngle;
        }


        #region 角度计算
        /// <summary>
        /// 确定角度偏移
        /// </summary>
        /// <param name="angleType">确定角度类型</param>
        /// <param name="basePoints">点集</param>
        /// <returns>角度</returns>
        private double GetAngle(GetAngleEnum angleType, List<PointF> basePoints)
        {
            double angle = 0;
            double x, y = 0;
            switch (angleType)
            {
                case GetAngleEnum.两点连线:
                    (x, y, angle) = this.CalculateMidpoint(basePoints[0].X, basePoints[0].Y, basePoints[1].X, basePoints[1].Y);
                    break;
                case GetAngleEnum.多点拟合:
                    angle = this.CalciLinearRegressionAngle(basePoints);
                    break;
                default:
                    break;
            }

            return angle;
        }

        /// <summary>
        /// 计算P1P2中点坐标及角度
        /// </summary>
        /// <param name="x1">x1</param>
        /// <param name="y1">y1</param>
        /// <param name="x2">x2</param>
        /// <param name="y2">y2</param>
        /// <returns>x,y,angle</returns>
        public (double, double, double) CalculateMidpoint(double x1, double y1, double x2, double y2)
        {
            double midX = (x1 + x2) / 2.0;
            double midY = (y1 + y2) / 2.0;

            // 计算两点之间的角度（弧度）
            double angleRad = Math.Atan2(y2 - y1, x2 - x1);

            // 将弧度转换为角度
            double angleDegrees = angleRad * (180.0 / Math.PI);

            return (midX, midY, angleDegrees);
        }

        /// <summary>
        /// 计算多点线性回归线角度
        /// </summary>
        /// <param name="points">点集</param>
        /// <returns>角度</returns>
        public double CalciLinearRegressionAngle(List<PointF> points)
        {
            double sumX = points.Sum(point => point.X);
            double sumY = points.Sum(point => point.Y);
            double sumXY = points.Sum(point => point.X * point.Y);
            double sumX2 = points.Sum(point => point.X * point.X);

            int n = points.Count;

            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double angleRad = Math.Atan(slope);
            double angleDegrees = angleRad * (180 / Math.PI);

            return angleDegrees;
        }

        /// <summary>
        /// 计算相机角度
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="p3">点3</param>
        /// <param name="camAngleX">x角度</param>
        /// <param name="camAngleY">y角度</param>
        public void CalcCamAngle(PointF p1, PointF p2, PointF p3, out double camAngleX, out double camAngleY)
        {
            camAngleX = camAngleY = 0;
            HOperatorSet.AngleLx(p1.X, p1.Y, p2.X, p2.Y, out HTuple radX);
            HOperatorSet.AngleLx(p1.X, p1.Y, p3.X, p3.Y, out HTuple radY);

            HOperatorSet.TupleDeg(radX, out HTuple angleX);
            HOperatorSet.TupleDeg(radY, out HTuple angleY);

            camAngleX = angleX.D;
            camAngleY = angleY.D;
        }
        #endregion

        #region 偏移量计算

        /// <summary>
        /// P1确定XY偏移量
        /// </summary>
        /// <param name="workType">工作类型</param>
        /// <param name="camBaseP1Point">p1模板轴坐标</param>
        /// <param name="p1BasePoint">p1模板坐标</param>
        /// <param name="p1MarkPoint">p1定位坐标</param>
        /// <param name="offsetX">x偏移</param>
        /// <param name="offsetY">y偏移</param>
        public void GetXYAngOffsetByP1(string workType, PointF camBaseP1Point, PointF p1BasePoint, PointF p1MarkPoint, out double offsetX, out double offsetY)
        {
            PointF p1AbsBasePoint = new PointF();
            PointF p1AbsMarkPoint = new PointF();

            double p1BaseWorldX = 0;
            double p1BaseWorldY = 0;
            double p1MarkWorldX = 0;
            double p1MarkWorldY = 0;

            if (workType == "Drip")
            {
                this.InvokeDripCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeDripCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
            }
            else if (workType == "Bond")
            {
                this.InvokeBondCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeBondCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
            }

            p1AbsBasePoint.X = (float)(camBaseP1Point.X + p1BaseWorldX);
            p1AbsBasePoint.Y = (float)(camBaseP1Point.Y + p1BaseWorldY);

            p1AbsMarkPoint.X = (float)(camBaseP1Point.X + p1MarkWorldX);
            p1AbsMarkPoint.Y = (float)(camBaseP1Point.Y + p1MarkWorldY);

            offsetX = p1AbsMarkPoint.X - p1AbsBasePoint.X;
            offsetY = p1AbsMarkPoint.Y - p1AbsBasePoint.Y;
        }

        /// <summary>
        /// P1P2确定XY偏移量及角度偏移量
        /// </summary>
        /// <param name="workType">类型</param>
        /// <param name="camBaseP1Point">p1模板轴坐标</param>
        /// <param name="camBaseP2Point">p2模板轴坐标</param>
        /// <param name="p1BasePoint">p1模板坐标</param>
        /// <param name="p2BasePoint">p2模板坐标</param>
        /// <param name="p1MarkPoint">p1定位坐标</param>
        /// <param name="p2MarkPoint">p2定位坐标</param>
        /// <param name="offsetX">x偏移</param>
        /// <param name="offsetY">y偏移</param>
        /// <param name="angleOffset">角度偏移</param>
        public void GetXYAngOffsetByP1P2(string workType, PointF camBaseP1Point, PointF camBaseP2Point, PointF p1BasePoint, PointF p2BasePoint, PointF p1MarkPoint, PointF p2MarkPoint, out double offsetX, out double offsetY, out double angleOffset)
        {
            PointF p1AbsBasePoint = new PointF();
            PointF p2AbsBasePoint = new PointF();
            PointF p1AbsMarkPoint = new PointF();
            PointF p2AbsMarkPoint = new PointF();

            double p1BaseWorldX = 0;
            double p1BaseWorldY = 0;
            double p2BaseWorldX = 0;
            double p2BaseWorldY = 0;
            double p1MarkWorldX = 0;
            double p1MarkWorldY = 0;
            double p2MarkWorldX = 0;
            double p2MarkWorldY = 0;

            if (workType == "Drip")
            {
                this.InvokeDripCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeDripCalibTrans(p2BasePoint.X, p2BasePoint.Y, out p2BaseWorldX, out p2BaseWorldY);
                this.InvokeDripCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
                this.InvokeDripCalibTrans(p2MarkPoint.X, p2MarkPoint.Y, out p2MarkWorldX, out p2MarkWorldY);
            }
            else if (workType == "Bond")
            {
                this.InvokeBondCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeBondCalibTrans(p2BasePoint.X, p2BasePoint.Y, out p2BaseWorldX, out p2BaseWorldY);
                this.InvokeBondCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
                this.InvokeBondCalibTrans(p2MarkPoint.X, p2MarkPoint.Y, out p2MarkWorldX, out p2MarkWorldY);
            }

            p1AbsBasePoint.X = (float)(camBaseP1Point.X + p1BaseWorldX);
            p1AbsBasePoint.Y = (float)(camBaseP1Point.Y + p1BaseWorldY);
            p2AbsBasePoint.X = (float)(camBaseP2Point.X + p2BaseWorldX);
            p2AbsBasePoint.Y = (float)(camBaseP2Point.Y + p2BaseWorldY);

            p1AbsMarkPoint.X = (float)(camBaseP1Point.X + p1MarkWorldX);
            p1AbsMarkPoint.Y = (float)(camBaseP1Point.Y + p1MarkWorldY);
            p2AbsMarkPoint.X = (float)(camBaseP2Point.X + p2MarkWorldX);
            p2AbsMarkPoint.Y = (float)(camBaseP2Point.Y + p2MarkWorldY);

            (double baseMidX, double baseMidY, double baseAngle) = this.CalculateMidpoint(p1AbsBasePoint.X, p1AbsBasePoint.X, p2AbsBasePoint.X, p2AbsBasePoint.Y);
            (double markMidX, double markMidY, double markAngle) = this.CalculateMidpoint(p1AbsMarkPoint.X, p1AbsMarkPoint.X, p2AbsMarkPoint.X, p2AbsMarkPoint.Y);

            offsetX = markMidX - baseMidX;
            offsetY = markMidY - baseMidY;
            angleOffset = markAngle - baseAngle;
        }
        #endregion

        #region 偏移确定位置
        /// <summary>
        /// 基于P1点偏移确定焊点位置
        /// </summary>
        /// <param name="p1CamPoint">p1轴坐标</param>
        /// <param name="p1Point">p1坐标</param>
        /// <param name="xoffset">x偏移</param>
        /// <param name="yoffset">y偏移</param>
        /// <returns>焊点位置</returns>
        private PointF GetJointPositionByP1Offset(PointF p1CamPoint, PointF p1Point, double xoffset, double yoffset)
        {
            double markPointX;
            double markPointY;

            double p1WorldOffsetX, p1WorldOffsetY;
            double p1AbsWorldX, p1AbsWorldY;

            this.InvokeBondCalibTrans(p1Point.X, p1Point.Y, out p1WorldOffsetX, out p1WorldOffsetY);

            p1AbsWorldX = p1CamPoint.X + p1WorldOffsetX;
            p1AbsWorldY = p1CamPoint.Y + p1WorldOffsetY;

            markPointX = p1AbsWorldX + xoffset;
            markPointY = p1AbsWorldY + yoffset;

            return new PointF((float)markPointX, (float)markPointY);
        }

        /// <summary>
        /// 基于P1P2中点偏移确定焊点位置
        /// </summary>
        /// <param name="p1CamPoint">p1轴坐标</param>
        /// <param name="p1Point">p1坐标</param>
        /// <param name="p2CamPoint">p2轴坐标</param>
        /// <param name="p2Point">p2坐标</param>
        /// <param name="xoffset">x偏移</param>
        /// <param name="yoffset">y偏移</param>
        /// <returns>焊点位置</returns>
        private PointF GetJointPositionByP1P2Offset(PointF p1CamPoint, PointF p1Point, PointF p2CamPoint, PointF p2Point, double xoffset, double yoffset)
        {
            double markPointX;
            double markPointY;

            double p1WorldOffsetX, p1WorldOffsetY, p2WorldOffsetX, p2WorldOffsetY;
            double p1AbsWorldX, p1AbsWorldY, p2AbsWorldX, p2AbsWorldY;

            this.InvokeBondCalibTrans(p1Point.X, p1Point.Y, out p1WorldOffsetX, out p1WorldOffsetY);
            this.InvokeBondCalibTrans(p2Point.X, p2Point.Y, out p2WorldOffsetX, out p2WorldOffsetY);

            p1AbsWorldX = p1CamPoint.X + p1WorldOffsetX;
            p1AbsWorldY = p1CamPoint.Y + p1WorldOffsetY;

            p2AbsWorldX = p2CamPoint.X + p2WorldOffsetX;
            p2AbsWorldY = p2CamPoint.Y + p2WorldOffsetY;

            (double midX, double midY, double angle) = this.CalculateMidpoint(p1AbsWorldX, p1AbsWorldY, p2AbsWorldX, p2AbsWorldY);

            markPointX = midX + xoffset;
            markPointY = midY + yoffset;

            return new PointF((float)markPointX, (float)markPointY);
        }
        #endregion

        #region 仿射确定位置
        /// <summary>
        /// 基于P1P2点定位仿射变换确定焊点位置
        /// </summary>
        /// <param name="p1BasePoint">p1基坐标</param>
        /// <param name="p2BasePoint">p2基坐标</param>
        /// <param name="p1MarkPoint">p1定位坐标</param>
        /// <param name="p2MarkPoint">p2定位坐标</param>
        /// <param name="initialPoint">初始点</param>
        /// <returns>转换坐标</returns>
        private PointF GetJointPositionByP1P2Affine(PointF p1BasePoint, PointF p2BasePoint, PointF p1MarkPoint, PointF p2MarkPoint, PointF initialPoint)
        {
            PointF transPoint = new PointF(0, 0);
            HTuple homMat2d = new HTuple();

            this.CalcuSubMapP1P2(p1BasePoint, p2BasePoint, p1MarkPoint, p2MarkPoint, out homMat2d);
            this.MapPointTrans(initialPoint, homMat2d, out transPoint);

            return transPoint;
        }

        /// <summary>
        /// 基于P1P2P3点定位仿射变换确定焊点位置
        /// </summary>
        /// <param name="p1BasePoint">p1基坐标</param>
        /// <param name="p2BasePoint">p2基坐标</param>
        /// <param name="p3BasePoint">p3基坐标</param>
        /// <param name="p1MarkPoint">p1转换坐标</param>
        /// <param name="p2MarkPoint">p2转换坐标</param>
        /// <param name="p3MarkPoint">p3转换坐标</param>
        /// <param name="initialPoint">初始坐标</param>
        /// <returns>转换坐标</returns>
        private PointF GetJointPositionByP1P2P3Affine(PointF p1BasePoint, PointF p2BasePoint, PointF p3BasePoint, PointF p1MarkPoint, PointF p2MarkPoint, PointF p3MarkPoint, PointF initialPoint)
        {
            PointF transPoint = new PointF(0, 0);
            HTuple homMat2d = new HTuple();

            // this.CalcuSubMapP1P2P3(p1BasePoint, p2BasePoint, p3BasePoint, p1MarkPoint, p2MarkPoint, p3MarkPoint, out homMat2d);
            this.MapPointTrans(initialPoint, homMat2d, out transPoint);

            return transPoint;
        }
        #endregion

        #region 定位确定位置
        /// <summary>
        /// 基于P1定位确定焊点位置
        /// </summary>
        /// <param name="xyzPosition">当前位置</param>
        /// <param name="p1BasePoint">p1基坐标</param>
        /// <param name="p1MarkPoint">p1定位坐标</param>
        /// <returns>焊点位置</returns>
        private PointF GetJointPositionByP1Location(PointF xyzPosition, PointF p1BasePoint, PointF p1MarkPoint)
        {
            double curMarkWorldX = 0;
            double curMarkWorldY = 0;
            double curWorldX = 0;
            double curWorldY = 0;
            double curBaseWorldX = 0;
            double curBaseWorldY = 0;

            this.InvokeBondCalibTrans(p1BasePoint.X, p1BasePoint.Y, out curBaseWorldX, out curBaseWorldY);
            this.InvokeBondCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out curMarkWorldX, out curMarkWorldY);

            curWorldX = xyzPosition.X + curMarkWorldX - curBaseWorldX;
            curWorldY = xyzPosition.Y + curMarkWorldY - curBaseWorldY;

            return new PointF((float)curWorldX, (float)curWorldY);
        }

        /// <summary>
        /// 基于P1P2定位确定焊点位置
        /// </summary>
        /// <param name="xyzPosition">当前位置</param>
        /// <param name="p1BasePoint">p1基坐标</param>
        /// <param name="p2BasePoint">p2基坐标</param>
        /// <param name="p1MarkPoint">p1定位坐标</param>
        /// <param name="p2MarkPoint">p2定位坐标</param>
        /// <returns>焊点位置</returns>
        private PointF GetJointPositionByP1P2Location(PointF xyzPosition, PointF p1BasePoint, PointF p2BasePoint, PointF p1MarkPoint, PointF p2MarkPoint)
        {
            double curMarkWorldX = 0;
            double curMarkWorldY = 0;
            double curWorldX = 0;
            double curWorldY = 0;
            double curBaseWorldX = 0;
            double curBaseWorldY = 0;

            (double baseMidPointX, double baseMidPointY, double basePointAngle) = this.CalculateMidpoint(p1BasePoint.X, p1BasePoint.Y, p2BasePoint.X, p2BasePoint.Y);
            (double markMidPointX, double markMidPointY, double markPointAngle) = this.CalculateMidpoint(p1MarkPoint.X, p1MarkPoint.Y, p2MarkPoint.X, p2MarkPoint.Y);

            this.InvokeBondCalibTrans(baseMidPointX, baseMidPointY, out curBaseWorldX, out curBaseWorldY);
            this.InvokeBondCalibTrans(markMidPointX, markMidPointY, out curMarkWorldX, out curMarkWorldY);

            curWorldX = xyzPosition.X + curMarkWorldX - curBaseWorldX;
            curWorldY = xyzPosition.Y + curMarkWorldY - curBaseWorldY;

            return new PointF((float)curWorldX, (float)curWorldY);
        }
        #endregion

        #region 旋转计算
        #region 一点绕旋转中心旋转后坐标计算方法
        /// <summary>
        /// 点绕旋转中心后坐标
        /// </summary>
        /// <param name="point">当前点</param>
        /// <param name="center">中心</param>
        /// <param name="angle">角度</param>
        /// <returns>转后点</returns>
        public static PointF Rotate(PointF point, PointF center, double angle)
        {
            // 将角度转换为弧度
            double angleInRadians = angle * (Math.PI / 180);

            // 计算相对于旋转中心的原始坐标
            double dx = point.X - center.X;
            double dy = point.Y - center.Y;

            // 计算旋转后的坐标
            float rotatedX = (float)(center.X + (dx * Math.Cos(angleInRadians)) - (dy * Math.Sin(angleInRadians)));
            float rotatedY = (float)(center.Y + (dx * Math.Sin(angleInRadians)) + (dy * Math.Cos(angleInRadians)));

            // 将旋转后的坐标返回
            return new PointF(rotatedX, rotatedY);
        }
        #endregion

        /// <summary>
        /// 根据偏移量及角度计算旋转后偏移量
        /// </summary>
        /// <param name="offsetX">x偏移</param>
        /// <param name="offsetY">y偏移</param>
        /// <param name="rotationAngle">旋转角度</param>
        /// <returns>偏移</returns>
        public PointF CalculateRotatedOffset(double offsetX, double offsetY, double rotationAngle)
        {
            // 将角度转换为弧度
            double rotationAngleInRadians = rotationAngle * Math.PI / 180.0f;

            // 计算旋转后的偏移量
            double rotatedOffsetX = offsetX * Math.Cos(rotationAngleInRadians) - offsetY * Math.Sin(rotationAngleInRadians);
            double rotatedOffsetY = offsetX * Math.Sin(rotationAngleInRadians) + offsetY * Math.Cos(rotationAngleInRadians);

            // 返回旋转后的偏移量
            return new PointF((float)rotatedOffsetX, (float)rotatedOffsetY);
        }

        #endregion  

        #region 计算第三点坐标方法2
        /*
        private static void CalculateRotatedThirdPoint(double startX, double startY, double endX, double endY,
            double rotatedStartX, double rotatedStartY, double rotatedEndX, double rotatedEndY,
            double initialThirdPointX, double initialThirdPointY,
            out double rotatedThirdPointX, out double rotatedThirdPointY)
        {
            // 计算旋转前和旋转后的角度差
            double angleDiffRad = GetAngleDifference(startX, startY, endX, endY, rotatedStartX, rotatedStartY, rotatedEndX, rotatedEndY);

            // 计算第三个点相对于线段中点的向量
            double midX = (startX + endX) / 2.0;
            double midY = (startY + endY) / 2.0;
            double dx_third_initial = initialThirdPointX - midX;
            double dy_third_initial = initialThirdPointY - midY;

            // 使用旋转矩阵计算旋转后的第三个点位置
            double cosAngle = Math.Cos(angleDiffRad);
            double sinAngle = Math.Sin(angleDiffRad);
            rotatedThirdPointX = midX + (dx_third_initial * cosAngle - dy_third_initial * sinAngle);
            rotatedThirdPointY = midY + (dx_third_initial * sinAngle + dy_third_initial * cosAngle);
        }

        private static double GetAngleDifference(double startX, double startY, double endX, double endY,
            double rotatedStartX, double rotatedStartY, double rotatedEndX, double rotatedEndY)
        {
            // 计算旋转前的角度
            double angleRad = Math.Atan2(endY - startY, endX - startX);

            // 计算旋转后的角度
            double angleRadRotated = Math.Atan2(rotatedEndY - rotatedStartY, rotatedEndX - rotatedStartX);

            // 计算角度差
            double angleDiffRad = angleRadRotated - angleRad;

            return angleDiffRad;
        }
        */
        #endregion

        #region P1对点
        /// <summary>
        /// P1仿射变换确定转换点
        /// </summary>
        /// <param name="workName">工作类型</param>
        /// <param name="camBaseP1Point">p1基轴坐标</param>
        /// <param name="p1BasePoint">p1基坐标</param>
        /// <param name="camMarkP1Point">p1定位轴坐标</param>
        /// <param name="p1MarkPoint">p1定位坐标</param>
        /// <param name="p1Angle">定位角度</param>
        /// <param name="initialPoints">初始点</param>
        /// <param name="transPoints">转换点</param>
        public void GetJointPositionByP1Affine(string workName, PointF camBaseP1Point, PointF p1BasePoint, PointF camMarkP1Point, PointF p1MarkPoint, double p1Angle, List<PointF> initialPoints, out List<PointF> transPoints)
        {
            transPoints = new List<PointF>();
            PointF p1AbsBasePoint = new PointF();
            PointF p1AbsMarkPoint = new PointF();
            PointF transPoint = new PointF();
            HTuple homMat2d = new HTuple();

            double p1BaseWorldX = 0;
            double p1BaseWorldY = 0;
            double p1MarkWorldX = 0;
            double p1MarkWorldY = 0;

            if (workName == "Drip")
            {
                this.InvokeDripCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeDripCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
            }
            else if (workName == "Bond")
            {
                this.InvokeBondCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeBondCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
            }

            p1AbsBasePoint.X = (float)(camBaseP1Point.X + p1BaseWorldX);
            p1AbsBasePoint.Y = (float)(camBaseP1Point.Y + p1BaseWorldY);

            p1AbsMarkPoint.X = (float)(camMarkP1Point.X + p1MarkWorldX);
            p1AbsMarkPoint.Y = (float)(camMarkP1Point.Y + p1MarkWorldY);

            this.CalcuSubMapP1(p1AbsBasePoint, p1AbsMarkPoint, p1Angle, out homMat2d);

            for (int i = 0; i < initialPoints.Count; i++)
            {
                this.MapPointTrans(initialPoints[i], homMat2d, out transPoint);
                transPoints.Add(transPoint);
            }
        }

        /// <summary>
        /// p1计算转换矩阵
        /// </summary>
        /// <param name="basePoint1">p1基坐标</param>
        /// <param name="markP1">p1转换坐标</param>
        /// <param name="p1Angle">p1角度</param>
        /// <param name="homat2dTrans">转换矩阵</param>
        private void CalcuSubMapP1(PointF basePoint1, PointF markP1, double p1Angle, out HTuple homat2dTrans)
        {
            double oldX1 = basePoint1.X;
            double oldY1 = basePoint1.Y;
            double newX1 = markP1.X;
            double newY1 = markP1.Y;

            double offsetX = newX1 - oldX1;
            double offsetY = newY1 - oldY1;

            HOperatorSet.TupleRad(p1Angle, out HTuple P1Rad);

            //// HOperatorSet.VectorAngleToRigid(OldY1, OldY1, 0, NewX1, NewY1, -P1Rad, out Homat2dTrans);

            HOperatorSet.VectorAngleToRigid(basePoint1.X, basePoint1.Y, 0, markP1.X, markP1.Y, P1Rad, out homat2dTrans);

            // HOperatorSet.HomMat2dIdentity(out HTuple homMat2DIdentity);
            // HOperatorSet.HomMat2dTranslate(homMat2DIdentity, OffsetY, OffsetX, out HTuple homMat2DTranslate);
            // HOperatorSet.HomMat2dRotate(homMat2DTranslate, -P1Rad, NewX1, NewY1, out Homat2dTrans);         
        }
        #endregion

        #region P1P2对点
        /// <summary>
        /// P1P2仿射变换确定转换点
        /// </summary>
        /// <param name="workName">工作类型</param>
        /// <param name="camBaseP1Point">p1基轴坐标</param>
        /// <param name="camBaseP2Point">p2基轴坐标</param>
        /// <param name="p1BasePoint">p1基坐标</param>
        /// <param name="p2BasePoint">p2基坐标</param>
        /// <param name="camMarkP1Point">p1定位轴坐标</param>
        /// <param name="camMarkP2Point">p2定位轴坐标</param>
        /// <param name="p1MarkPoint">p1定位坐标</param>
        /// <param name="p2MarkPoint">p2定位坐标</param>
        /// <param name="initialPoints">初始点</param>
        /// <param name="transPoints">转换点</param>
        public void GetJointPositionByP1P2Affine(string workName, PointF camBaseP1Point, PointF camBaseP2Point, PointF p1BasePoint, PointF p2BasePoint, PointF camMarkP1Point, PointF camMarkP2Point, PointF p1MarkPoint, PointF p2MarkPoint, List<PointF> initialPoints,out List<PointF> transPoints)
        {
            transPoints = new List<PointF>();
            PointF p1AbsBasePoint = new PointF();
            PointF p2AbsBasePoint = new PointF();
            PointF p1AbsMarkPoint = new PointF();
            PointF p2AbsMarkPoint = new PointF();

            PointF transPoint = new PointF();
            HTuple homMat2d = new HTuple();

            double p1BaseWorldX = 0;
            double p1BaseWorldY = 0;
            double p2BaseWorldX = 0;
            double p2BaseWorldY = 0;
            double p1MarkWorldX = 0;
            double p1MarkWorldY = 0;
            double p2MarkWorldX = 0;
            double p2MarkWorldY = 0;

            if (workName == "Drip")
            {
                this.InvokeDripCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeDripCalibTrans(p2BasePoint.X, p2BasePoint.Y, out p2BaseWorldX, out p2BaseWorldY);
                this.InvokeDripCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
                this.InvokeDripCalibTrans(p2MarkPoint.X, p2MarkPoint.Y, out p2MarkWorldX, out p2MarkWorldY);
            }
            else if (workName == "Bond")
            {
                this.InvokeBondCalibTrans(p1BasePoint.X, p1BasePoint.Y, out p1BaseWorldX, out p1BaseWorldY);
                this.InvokeBondCalibTrans(p2BasePoint.X, p2BasePoint.Y, out p2BaseWorldX, out p2BaseWorldY);
                this.InvokeBondCalibTrans(p1MarkPoint.X, p1MarkPoint.Y, out p1MarkWorldX, out p1MarkWorldY);
                this.InvokeBondCalibTrans(p2MarkPoint.X, p2MarkPoint.Y, out p2MarkWorldX, out p2MarkWorldY);
            }

            p1AbsBasePoint.X = (float)(camBaseP1Point.X + p1BaseWorldX);
            p1AbsBasePoint.Y = (float)(camBaseP1Point.Y + p1BaseWorldY);
            p2AbsBasePoint.X = (float)(camBaseP2Point.X + p2BaseWorldX);
            p2AbsBasePoint.Y = (float)(camBaseP2Point.Y + p2BaseWorldY);

            p1AbsMarkPoint.X = (float)(camMarkP1Point.X + p1MarkWorldX);
            p1AbsMarkPoint.Y = (float)(camMarkP1Point.Y + p1MarkWorldY);
            p2AbsMarkPoint.X = (float)(camMarkP2Point.X + p2MarkWorldX);
            p2AbsMarkPoint.Y = (float)(camMarkP2Point.Y + p2MarkWorldY);

            this.CalcuSubMapP1P2(p1AbsBasePoint, p2AbsBasePoint, p1AbsMarkPoint, p2AbsMarkPoint, out homMat2d);

            for (int i = 0; i < initialPoints.Count; i++)
            {
                this.MapPointTrans(initialPoints[i], homMat2d, out transPoint);
                transPoints.Add(transPoint);
            }
        }

        /// <summary>
        /// P1P2确定仿射矩阵
        /// </summary>
        /// <param name="basePoint1">p1基坐标</param>
        /// <param name="basePoint2">p2基坐标</param>
        /// <param name="markP1">p1转换坐标</param>
        /// <param name="markP2">p2转换坐标</param>
        /// <param name="homat2dTrans">转换矩阵</param>
        private void CalcuSubMapP1P2(PointF basePoint1, PointF basePoint2, PointF markP1, PointF markP2, out HTuple homat2dTrans)
        {
            double oldX1 = basePoint1.X;
            double oldY1 = basePoint1.Y;
            double oldX2 = basePoint2.X;
            double oldY2 = basePoint2.Y;
            double newX1 = markP1.X;
            double newY1 = markP1.Y;
            double newX2 = markP2.X;
            double newY2 = markP2.Y;

            HTuple oldAngle, newAngle;

            HOperatorSet.AngleLx(oldX1, oldY1, oldX2, oldY2, out oldAngle);
            HOperatorSet.AngleLx(newX1, newY1, newX2, newY2, out newAngle);

            HTuple oldX = (oldX1 + oldX2) / 2;
            HTuple oldY = (oldY1 + oldY2) / 2;
            HTuple newX = (newX1 + newX2) / 2;
            HTuple newY = (newY1 + newY2) / 2;

            HOperatorSet.VectorAngleToRigid(oldX, oldY, oldAngle, newX, newY, newAngle, out homat2dTrans);
           
            HOperatorSet.AffineTransPoint2d(homat2dTrans,1224,1024, out HTuple qx, out HTuple qy);
        }
        #endregion

        #region P1P2P3对点
        /*
        /// <summary>
        /// P1P2P3仿射变换确定转换点
        /// </summary>
        public void GetJointPositionByP1P2P3Affine(string WorkName, PointF CamBaseP1Point, PointF CamBaseP2Point, PointF CamBaseP3Point, PointF P1BasePoint, PointF P2BasePoint, PointF P3BasePoint, PointF CamMarkP1Point, PointF CamMarkP2Point, PointF CamMarkP3Point, PointF P1MarkPoint, PointF P2MarkPoint, PointF P3MarkPoint, List<PointF> InitialPoints, out List<PointF> TransPoints)
        {
            TransPoints = new List<PointF>();
            PointF P1AbsBasePoint = new PointF();
            PointF P2AbsBasePoint = new PointF();
            PointF P3AbsBasePoint = new PointF();
            PointF P1AbsMarkPoint = new PointF();
            PointF P2AbsMarkPoint = new PointF();
            PointF P3AbsMarkPoint = new PointF();

            PointF TransPoint = new PointF();
            HTuple HomMat2d = new HTuple();

            double P1BaseWorldX = 0;
            double P1BaseWorldY = 0;
            double P2BaseWorldX = 0;
            double P2BaseWorldY = 0;
            double P3BaseWorldX = 0;
            double P3BaseWorldY = 0;
            double P1MarkWorldX = 0;
            double P1MarkWorldY = 0;
            double P2MarkWorldX = 0;
            double P2MarkWorldY = 0;
            double P3MarkWorldX = 0;
            double P3MarkWorldY = 0;

            if (WorkName == "Drip")
            {
                InvokeDripCalibTrans(P1BasePoint.X, P1BasePoint.Y, out P1BaseWorldX, out P1BaseWorldY);
                InvokeDripCalibTrans(P2BasePoint.X, P2BasePoint.Y, out P2BaseWorldX, out P2BaseWorldY);
                InvokeDripCalibTrans(P3BasePoint.X, P3BasePoint.Y, out P3BaseWorldX, out P3BaseWorldY);
                InvokeDripCalibTrans(P1MarkPoint.X, P1MarkPoint.Y, out P1MarkWorldX, out P1MarkWorldY);
                InvokeDripCalibTrans(P2MarkPoint.X, P2MarkPoint.Y, out P2MarkWorldX, out P2MarkWorldY);
                InvokeDripCalibTrans(P3MarkPoint.X, P3MarkPoint.Y, out P3MarkWorldX, out P3MarkWorldY);

            }
            else if (WorkName == "Bond")
            {
                InvokeBondCalibTrans(P1BasePoint.X, P1BasePoint.Y, out P1BaseWorldX, out P1BaseWorldY);
                InvokeBondCalibTrans(P2BasePoint.X, P2BasePoint.Y, out P2BaseWorldX, out P2BaseWorldY);
                InvokeBondCalibTrans(P3BasePoint.X, P3BasePoint.Y, out P3BaseWorldX, out P3BaseWorldY);
                InvokeBondCalibTrans(P1MarkPoint.X, P1MarkPoint.Y, out P1MarkWorldX, out P1MarkWorldY);
                InvokeBondCalibTrans(P2MarkPoint.X, P2MarkPoint.Y, out P2MarkWorldX, out P2MarkWorldY);
                InvokeBondCalibTrans(P3MarkPoint.X, P3MarkPoint.Y, out P3MarkWorldX, out P3MarkWorldY);
            }

            P1AbsBasePoint.X = (float)(CamBaseP1Point.X + P1BaseWorldX);
            P1AbsBasePoint.Y = (float)(CamBaseP1Point.Y + P1BaseWorldY);
            P2AbsBasePoint.X = (float)(CamBaseP2Point.X + P2BaseWorldX);
            P2AbsBasePoint.Y = (float)(CamBaseP2Point.Y + P2BaseWorldY);
            P3AbsBasePoint.X = (float)(CamBaseP3Point.X + P3BaseWorldX);
            P3AbsBasePoint.Y = (float)(CamBaseP3Point.Y + P3BaseWorldY);

            P1AbsMarkPoint.X = (float)(CamMarkP1Point.X + P1MarkWorldX);
            P1AbsMarkPoint.Y = (float)(CamMarkP1Point.Y + P1MarkWorldY);
            P2AbsMarkPoint.X = (float)(CamMarkP2Point.X + P2MarkWorldX);
            P2AbsMarkPoint.Y = (float)(CamMarkP2Point.Y + P2MarkWorldY);
            P3AbsMarkPoint.X = (float)(CamMarkP3Point.X + P3MarkWorldX);
            P3AbsMarkPoint.Y = (float)(CamMarkP3Point.Y + P3MarkWorldY);

            CalcuSubMapP1P2P3(P1AbsBasePoint, P2AbsBasePoint, P3AbsBasePoint, P1AbsMarkPoint, P2AbsMarkPoint, P3AbsMarkPoint, out HomMat2d);

            for (int i = 0; i < InitialPoints.Count; i++)
            {
                MapPointTrans(InitialPoints[i], HomMat2d, out TransPoint);
                TransPoints.Add(TransPoint);
            }
        }

        /// <summary>
        /// P1P2P3确定仿射缩放矩阵
        /// </summary>
        /// <param name="BasePoint1"></param>
        /// <param name="BasePoint2"></param>
        /// <param name="BasePoint3"></param>
        /// <param name="MarkP1"></param>
        /// <param name="MarkP2"></param>
        /// <param name="MarkP3"></param>
        /// <param name="Homat2dTrans"></param>
        private void CalcuSubMapP1P2P3(PointF BasePoint1, PointF BasePoint2, PointF BasePoint3, PointF MarkP1, PointF MarkP2, PointF MarkP3, out HTuple Homat2dTrans)
        {
            double OldX1 = BasePoint1.X;
            double OldY1 = BasePoint1.Y;
            double OldX2 = BasePoint2.X;
            double OldY2 = BasePoint2.Y;
            double OldX3 = BasePoint3.X;
            double OldY3 = BasePoint3.Y;
            double NewX1 = MarkP1.X;
            double NewY1 = MarkP1.Y;
            double NewX2 = MarkP2.X;
            double NewY2 = MarkP2.Y;
            double NewX3 = MarkP3.X;
            double NewY3 = MarkP3.Y;

            HTuple oldPointsX = new HTuple();
            HTuple oldPointsY = new HTuple();
            HTuple newPointsX = new HTuple();
            HTuple newPointsY = new HTuple();

            oldPointsX.TupleConcat(OldX1);
            oldPointsX.TupleConcat(OldX2);
            oldPointsX.TupleConcat(OldX3);
            oldPointsY.TupleConcat(OldY1);
            oldPointsY.TupleConcat(OldY2);
            oldPointsY.TupleConcat(OldY3);

            newPointsX.TupleConcat(NewX1);
            newPointsX.TupleConcat(NewX2);
            newPointsX.TupleConcat(NewX3);
            newPointsY.TupleConcat(NewY1);
            newPointsY.TupleConcat(NewY2);
            newPointsY.TupleConcat(NewY3);

            HOperatorSet.VectorToHomMat2d(oldPointsX, oldPointsY, newPointsX, newPointsY, out Homat2dTrans);
        }
        */
        #endregion

        /// <summary>
        /// 矩阵转换确定转换后坐标
        /// </summary>
        /// <param name="initialPoint">初始点</param>
        /// <param name="homat2d">转换矩阵</param>
        /// <param name="transPoint">转换点</param>
        private void MapPointTrans(PointF initialPoint, HTuple homat2d, out PointF transPoint)
        {
            double initialX = initialPoint.X;
            double initialY = initialPoint.Y;
            float px, py;
            HTuple transX, transY;
            HOperatorSet.AffineTransPoint2d(homat2d, initialX, initialY, out transX, out transY);

            px = (float)transX[0].D;
            py = (float)transY[0].D;
            transPoint = new PointF(px, py);
        }

        #region 标定转换
        /// <summary>
        /// 晶圆图像点转晶圆台机械坐标
        /// </summary>
        /// <param name="waferX">x</param>
        /// <param name="waferY">y</param>
        /// <param name="waferWorldX">世界x</param>
        /// <param name="waferWorldY">世界y</param>
        /// <returns>是否成功</returns>
        public bool InvokeWaferCalibTrans(double waferX, double waferY, out double waferWorldX, out double waferWorldY)
        {
            bool isSuccess = this.WaferCalibTrans(waferX, waferY, out waferWorldX, out waferWorldY);
            return isSuccess;
        }

        /// <summary>
        /// 点胶图像点转点胶机械坐标
        /// </summary>
        /// <param name="dripX">x</param>
        /// <param name="dripY">y</param>
        /// <param name="dripWorldX">世界x</param>
        /// <param name="dripWorldY">世界y</param>
        /// <returns>是否成功</returns>
        public bool InvokeDripCalibTrans(double dripX, double dripY, out double dripWorldX, out double dripWorldY)
        {
            bool isSuccess = this.DripCalibTrans(dripX, dripY, out dripWorldX, out dripWorldY);
            return isSuccess;
        }

        /// <summary>
        /// 上视图像点转模组机械坐标
        /// </summary>
        /// <param name="chipX">x</param>
        /// <param name="chipY">y</param>
        /// <param name="chipWorldX">世界x</param>
        /// <param name="chipWorldY">世界y</param>
        /// <returns>是否成功</returns>
        public bool InvokeUpLookCalibTrans(double chipX, double chipY, out double chipWorldX, out double chipWorldY)
        {
            bool isSuccess = this.UpLookCalibTrans(chipX, chipY, out chipWorldX, out chipWorldY);
            return isSuccess;
        }

        /// <summary>
        /// 基板图像点转基板机械坐标
        /// </summary>
        /// <param name="subX">x</param>
        /// <param name="subY">y</param>
        /// <param name="subWorldX">世界x</param>
        /// <param name="subWorldY">世界y</param>
        /// <returns>是否成功</returns>
        public bool InvokeBondCalibTrans(double subX, double subY, out double subWorldX, out double subWorldY)
        {
            bool isSuccess = this.BondCalibTrans(subX, subY, out subWorldX, out subWorldY);
            return isSuccess;
        }

        /// <summary>
        /// 晶圆转换
        /// </summary>
        /// <param name="waferX">x</param>
        /// <param name="waferY">y</param>
        /// <param name="waferWorldX">wx</param>
        /// <param name="waferWorldY">wy</param>
        /// <returns>istrue</returns>
        private bool WaferCalibTrans(double waferX, double waferY, out double waferWorldX, out double waferWorldY)
        {
            waferWorldX = 0; 
            waferWorldY = 0;
            float[] worldX = new float[1];
            float[] worldY = new float[1];

            worldX[0] = (float)waferX;
            worldY[0] = (float)waferY;
            try
            { 
                waferTransPrc = VmProcedure.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\晶圆台标定转换.prc");

                ProcedureParam procedureParam = waferTransPrc.ModuParams;
                ProcedureResult procedureResult = waferTransPrc.ModuResult;
                GlobalVariableModuleTool globalVariable = (GlobalVariableModuleTool)VmSolution.Instance["全局变量1"];

                // globalVariable.SetGlobalVar("晶圆X", WaferX.ToString());
                // globalVariable.SetGlobalVar("晶圆Y", WaferY.ToString());
                procedureParam.SetInputFloat("晶圆X", worldX);
                procedureParam.SetInputFloat("晶圆Y", worldY);

                waferTransPrc.Run();

                waferWorldX = procedureResult.GetOutputFloat("晶圆台转换X").pFloatVal[0];
                waferWorldY = procedureResult.GetOutputFloat("晶圆台转换Y").pFloatVal[0];

                waferTransPrc.Dispose();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 点胶转换
        /// </summary>
        /// <param name="dripX">x</param>
        /// <param name="dripY">y</param>
        /// <param name="dripWorldX">wx</param>
        /// <param name="dripWorldY">wy</param>
        /// <returns>istrue</returns>
        private bool DripCalibTrans(double dripX, double dripY, out double dripWorldX, out double dripWorldY)
        {
            dripWorldX = 0; 
            dripWorldY = 0;
            float[] worldX = new float[1];
            float[] worldY = new float[1];

            worldX[0] = (float)dripX;
            worldY[0] = (float)dripY;
            try
            { 
                dripTransPrc = VmProcedure.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\点胶标定转换.prc");

                ProcedureParam procedureParam = dripTransPrc.ModuParams;
                ProcedureResult procedureResult = dripTransPrc.ModuResult;

                // globalVariable.SetGlobalVar("点胶X", DripX.ToString());
                // globalVariable.SetGlobalVar("点胶Y", DripY.ToString());
                procedureParam.SetInputFloat("点胶X", worldX);
                procedureParam.SetInputFloat("点胶Y", worldY);

                dripTransPrc.Run();

                dripWorldX = procedureResult.GetOutputFloat("点胶转换X").pFloatVal[0];
                dripWorldY = procedureResult.GetOutputFloat("点胶转换Y").pFloatVal[0];
                dripTransPrc.Dispose();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 上视转换
        /// </summary>
        /// <param name="chipX">x</param>
        /// <param name="chipY">y</param>
        /// <param name="chipWorldX">wx</param>
        /// <param name="chipWorldY">wy</param>
        /// <returns>istrue</returns>
        private bool UpLookCalibTrans(double chipX, double chipY, out double chipWorldX, out double chipWorldY)
        {
            chipWorldX = 0; 
            chipWorldY = 0;
            float[] worldX = new float[1];
            float[] worldY = new float[1];

            worldX[0] = (float)chipX;
            worldY[0] = (float)chipY;
            try
            { 
                upLookTransPrc = VmProcedure.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上视标定转换.prc");
                ProcedureParam procedureParam = upLookTransPrc.ModuParams;
                ProcedureResult procedureResult = upLookTransPrc.ModuResult;

                procedureParam.SetInputFloat("芯片X", worldX);
                procedureParam.SetInputFloat("芯片Y", worldY);

                upLookTransPrc.Run();

                chipWorldX = procedureResult.GetOutputFloat("芯片转换X").pFloatVal[0];
                chipWorldY = procedureResult.GetOutputFloat("芯片转换Y").pFloatVal[0];

                upLookTransPrc.Dispose();

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Bond转换
        /// </summary>
        /// <param name="subX">x</param>
        /// <param name="subY">y</param>
        /// <param name="subWorldX">wx</param>
        /// <param name="subWorldY">wy</param>
        /// <returns>istrue</returns>
        private bool BondCalibTrans(double subX, double subY, out double subWorldX, out double subWorldY)
        {
            subWorldX = 0;
            subWorldY = 0;
            float[] worldX = new float[1];
            float[] worldY = new float[1];

            worldX[0] = (float)subX;
            worldY[0] = (float)subY;
            try
            { 
                bondTransPrc = VmProcedure.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\邦头标定转换.prc");

               //// VmProcedure procedure = VmSolution.Instance["5邦头标定转换"] as VmProcedure;

                ProcedureParam procedureParam = bondTransPrc.ModuParams;
                ProcedureResult procedureResult = bondTransPrc.ModuResult;
                GlobalVariableModuleTool globalVariable = (GlobalVariableModuleTool)VmSolution.Instance["全局变量1"];

                // globalVariable.SetGlobalVar("基板X", SubX.ToString());
                // globalVariable.SetGlobalVar("基板Y", SubY.ToString());
                procedureParam.SetInputFloat("基板X", worldX);
                procedureParam.SetInputFloat("基板Y", worldY);

                bondTransPrc.Run();

                subWorldX = procedureResult.GetOutputFloat("基板转换X").pFloatVal[0];
                subWorldY = procedureResult.GetOutputFloat("基板转换Y").pFloatVal[0];

                bondTransPrc.Dispose();

            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// 计算距相机中心偏差
        /// </summary>
        /// <param name="centerX">x</param>
        /// <param name="centerY">y</param>
        /// <param name="offsetX">偏移x</param>
        /// <param name="offsetY">偏移y</param>
        /// <param name="name">类型</param>
        public void GetOffsetWorld(double centerX, double centerY, out double offsetX, out double offsetY, string name = null)
        {
            offsetX = 0; 
            offsetY = 0;
            double x1, x2, y1, y2;
            switch (name)
            {
                case "Drip":
                    this.InvokeDripCalibTrans(centerX, centerY, out x1, out y1);
                    this.InvokeDripCalibTrans(1224, 1024, out x2, out y2);
                    break;
                case "Wafer":
                    this.InvokeWaferCalibTrans(centerX, centerY, out x1, out y1);
                    this.InvokeWaferCalibTrans(1224, 1024, out x2, out y2);
                    break;
                case "UpLook":
                    this.InvokeUpLookCalibTrans(centerX, centerY, out x1, out y1);
                    this.InvokeUpLookCalibTrans(1224, 1024, out x2, out y2);
                    break;
                case "Bond":
                    this.InvokeBondCalibTrans(centerX, centerY, out x1, out y1);
                    this.InvokeBondCalibTrans(1224, 1024, out x2, out y2);
                    break;
                default: return;
            }
            offsetX = x1 - x2;
            offsetY = y1 - y2;
        }

        /// <summary>
        /// 点胶标定
        /// </summary>
        /// <param name="imageX">ImageX</param>
        /// <param name="imageY">ImageY</param>
        /// <param name="worldX">WorldX</param>
        /// <param name="worldY">WorldY</param>
        /// <param name="cameraScale">像素比</param>
        /// <param name="transX">平移x</param>
        /// <param name="transY">平移y</param>
        /// <returns>是否成功</returns>
        public bool DripCalib(float[] imageX, float[] imageY, float[] worldX, float[] worldY, out double cameraScale, out double transX, out double transY)
        {
            int isSuccess = 0;
            cameraScale = 0;
            transX = 0;
            transY = 0;
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");
                VmProcedure procedure = VmSolution.Instance["4点胶9点标定"] as VmProcedure;

                ProcedureParam procedureParam = procedure.ModuParams;
                ProcedureResult procedureResult = procedure.ModuResult;

                procedureParam.SetInputFloat("WorldX", worldX);
                procedureParam.SetInputFloat("WorldY", worldY);

                procedureParam.SetInputFloat("ImageX", imageX);
                procedureParam.SetInputFloat("ImageY", imageY);

                procedure.Run();

                FloatDataArray cameraScaleArray = procedureResult.GetOutputFloat("CameraScale");
                FloatDataArray transXArray = procedureResult.GetOutputFloat("TransX");
                FloatDataArray transYArray = procedureResult.GetOutputFloat("TransY");

                if (cameraScaleArray.nValueNum != 0)
                {
                    cameraScale = procedureResult.GetOutputFloat("CameraScale").pFloatVal[0];
                }
                if (transXArray.nValueNum != 0)
                {
                    transX = procedureResult.GetOutputFloat("TransX").pFloatVal[0];
                }
                if (transYArray.nValueNum != 0)
                {
                    transY = procedureResult.GetOutputFloat("TransY").pFloatVal[0];
                }

                if (isSuccess == procedureResult.GetOutputInt("isSuccess").pIntVal[0])
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Bond相机9点标定接口
        /// </summary>
        /// <param name="imageX">ImageX</param>
        /// <param name="imageY">ImageY</param>
        /// <param name="worldX">WorldX</param>
        /// <param name="worldY">WorldY</param>
        /// <param name="cameraScale">像素比</param>
        /// <param name="transX">平移x</param>
        /// <param name="transY">平移y</param>
        /// <returns>是否成功</returns>
        public bool BondCamCalib(float[] imageX, float[] imageY, float[] worldX, float[] worldY, out double cameraScale, out double transX, out double transY)
        {
            int isSuccess = 0;
            cameraScale = 0;
            transX = 0;
            transY = 0;
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");
                VmProcedure procedure = VmSolution.Instance["3上相机12点"] as VmProcedure;

                ProcedureParam procedureParam = procedure.ModuParams;
                ProcedureResult procedureResult = procedure.ModuResult;

                procedureParam.SetInputFloat("WorldX", worldX);
                procedureParam.SetInputFloat("WorldY", worldY);

                procedureParam.SetInputFloat("ImageX", imageX);
                procedureParam.SetInputFloat("ImageY", imageY);

                procedure.Run();

                // FloatDataArray CameraScaleArray = procedureResult.GetOutputFloat("CameraScale");
                // FloatDataArray TransXArray = procedureResult.GetOutputFloat("TransX");
                // FloatDataArray TransYArray = procedureResult.GetOutputFloat("TransY");

                // if (CameraScaleArray.nValueNum != 0)
                // {
                //     CameraScale = procedureResult.GetOutputFloat("CameraScale").pFloatVal[0];
                // }
                // if (TransXArray.nValueNum != 0)
                // {
                //    TransX = procedureResult.GetOutputFloat("TransX").pFloatVal[0];
                // }
                // if (TransYArray.nValueNum != 0)
                // {
                //    TransY = procedureResult.GetOutputFloat("TransY").pFloatVal[0];
                //// }

                if (isSuccess == procedureResult.GetOutputInt("isSuccess").pIntVal[0])
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 晶圆相机9点标定接口
        /// </summary>
        /// <param name="imageX">ImageX</param>
        /// <param name="imageY">ImageY</param>
        /// <param name="worldX">WorldX</param>
        /// <param name="worldY">WorldY</param>
        /// <param name="cameraScale">像素比</param>
        /// <param name="transX">平移x</param>
        /// <param name="transY">平移y</param>
        /// <returns>是否成功</returns>
        public bool WaferCamCalib(float[] imageX, float[] imageY, float[] worldX, float[] worldY, out double cameraScale, out double transX, out double transY)
        {
            int isSuccess = 0;
            cameraScale = 0;
            transX = 0;
            transY = 0;
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");

                VmProcedure procedure = VmSolution.Instance["5晶圆相机9点"] as VmProcedure;

                ProcedureParam procedureParam = procedure.ModuParams;
                ProcedureResult procedureResult = procedure.ModuResult;

                procedureParam.SetInputFloat("WorldX", worldX);
                procedureParam.SetInputFloat("WorldY", worldY);
                procedureParam.SetInputFloat("ImageX", imageX);
                procedureParam.SetInputFloat("ImageY", imageY);

                // FloatDataArray CameraScaleArray = procedureResult.GetOutputFloat("CameraScale");
                // FloatDataArray TransXArray = procedureResult.GetOutputFloat("TransX");
                //// FloatDataArray TransYArray = procedureResult.GetOutputFloat("TransY");

                procedure.Run();

                /*
                if (CameraScaleArray.nValueNum != 0)
                {
                    CameraScale = procedureResult.GetOutputFloat("CameraScale").pFloatVal[0];
                }
                if (TransXArray.nValueNum != 0)
                {
                    TransX = procedureResult.GetOutputFloat("TransX").pFloatVal[0];
                }
                if (TransYArray.nValueNum != 0)
                {
                    TransY = procedureResult.GetOutputFloat("TransY").pFloatVal[0];
                }
                */

                if (isSuccess == procedureResult.GetOutputInt("isSuccess").pIntVal[0])
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 上视相机9点标定接口
        /// </summary>
        /// <param name="imageX">ImageX</param>
        /// <param name="imageY">ImageY</param>
        /// <param name="worldX">WorldX</param>
        /// <param name="worldY">WorldY</param>
        /// <param name="cameraScale">像素比</param>
        /// <param name="transX">平移x</param>
        /// <param name="transY">平移y</param>
        /// <returns>是否成功</returns>
        public bool UpLookCamCalib(float[] imageX, float[] imageY, float[] worldX, float[] worldY, out double cameraScale, out double transX, out double transY)
        {
            int isSuccess = 0;
            cameraScale = 0;
            transX = 0;
            transY = 0;
            try
            {
                VmSolution.Load("C:\\Users\\user\\Desktop\\视觉标定项目\\标定方案\\上下相机映射对位_V4.3.0.sol");

                VmProcedure procedure = VmSolution.Instance["1下相机12点"] as VmProcedure;

                ProcedureParam procedureParam = procedure.ModuParams;
                ProcedureResult procedureResult = procedure.ModuResult;

                procedureParam.SetInputFloat("WorldX", worldX);
                procedureParam.SetInputFloat("WorldY", worldY);

                procedureParam.SetInputFloat("ImageX", imageX);
                procedureParam.SetInputFloat("ImageY", imageY);

                procedure.Run();

                /*
                FloatDataArray CameraScaleArray = procedureResult.GetOutputFloat("CameraScale");
                FloatDataArray TransXArray = procedureResult.GetOutputFloat("TransX");
                FloatDataArray TransYArray = procedureResult.GetOutputFloat("TransY");

                if (CameraScaleArray.nValueNum != 0)
                {
                    CameraScale = procedureResult.GetOutputFloat("CameraScale").pFloatVal[0];
                }
                if (TransXArray.nValueNum != 0)
                {
                    TransX = procedureResult.GetOutputFloat("TransX").pFloatVal[0];
                }
                if (TransYArray.nValueNum != 0)
                {
                    TransY = procedureResult.GetOutputFloat("TransY").pFloatVal[0];
                }
                */

                if (isSuccess == procedureResult.GetOutputInt("isSuccess").pIntVal[0])
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
