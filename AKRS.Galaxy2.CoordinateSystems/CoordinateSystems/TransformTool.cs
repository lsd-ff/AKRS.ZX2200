using IMVSCalibTransformModuCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.CoordinateSystems.CoordinateSystems
{
    using System.IO;
    using System.Runtime.CompilerServices;

    using AKRS.Galaxy2.Infrastructure.CommonModel;

    using IMVSNPointCalibModuCs;
    using VisionDesigner;
    using VisionDesigner.CalibTrans;
    using VisionDesigner.NPointCalib;

    using VM.Core;
    using VM.PlatformSDKCS;

    /// <summary>
    /// 转换工具
    /// </summary>
    public class TransformTool
    {
        /// <summary>
        /// 锁
        /// </summary>
        private readonly static object Locker = new object();

        /// <summary>
        /// 创建标定文件
        /// </summary>
        /// <param name="reality">现实坐标点集</param>
        /// <param name="virtually">图像坐标点集</param>
        /// <param name="name">路径</param>
        /// <returns>是否标定成功</returns>
        [Obsolete]
        public static bool CreateCalibrationFile(List<AKRSPoint3D> reality, List<AKRSPoint3D> virtually, string name)
        {
            lock (Locker)
            {
                string pathName = Path.Combine(StaticPara.CoordinateSystemPath, "标定.prc");
                VmProcedure calibPrc = VmProcedure.Load(pathName);
                float[] imageX = new float[virtually.Count];
                float[] imageY = new float[virtually.Count];
                float[] worldX = new float[reality.Count];
                float[] worldY = new float[reality.Count];

                for (int i = 0; i < virtually.Count; i++)
                {
                    imageX[i] = (float)virtually[i].X;
                    imageY[i] = (float)virtually[i].Y;
                }
                for (int i = 0; i < reality.Count; i++)
                {
                    worldX[i] = (float)reality[i].X;
                    worldY[i] = (float)reality[i].Y;
                }

                ProcedureParam vmPrcInput = calibPrc.ModuParams;
                vmPrcInput.SetInputFloat("imageX", imageX);
                vmPrcInput.SetInputFloat("imageY", imageY);
                vmPrcInput.SetInputFloat("worldX", worldX);
                vmPrcInput.SetInputFloat("worldY", worldY);

                IMVSNPointCalibModuTool calibModule = (IMVSNPointCalibModuTool)calibPrc.Modules[0];
                calibModule.ModuParams.CalibPathName = Path.Combine(StaticPara.CoordinateSystemPath, name + ".xml");
                // calibPrc.SaveAs(pathName);
                calibPrc.Run();

                ProcedureResult vmPrcOutput = calibPrc.ModuResult;
                int isSuccess = vmPrcOutput.GetOutputInt("isSuccess").pIntVal[0];

                // calibPrc.Dispose();
                if (isSuccess != 0)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="point">初始点</param>
        /// <param name="prcName">流程</param>
        /// <returns>转换点</returns>
        [Obsolete]
        public static AKRSPoint3D Transformation(AKRSPoint3D point, VmProcedure prcName)
        {
            lock (Locker)
            {
                float[] initialX = new float[1] { (float)point.X };
                float[] initialY = new float[1] { (float)point.Y };

                ProcedureParam vmPrcInput = prcName.ModuParams;

                vmPrcInput.SetInputFloat("initialX", initialX);
                vmPrcInput.SetInputFloat("initialY", initialY);

                prcName.Run();

                ProcedureResult vmPrcOutput = prcName.ModuResult;
                FloatDataArray transXArray = vmPrcOutput.GetOutputFloat("transX");
                float transX = vmPrcOutput.GetOutputFloat("transX").pFloatVal[0];
                float transY = vmPrcOutput.GetOutputFloat("transY").pFloatVal[0];

                return new AKRSPoint3D(transX, transY, point.Z);
            }
        }

        /// <summary>
        /// 加载流程
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>流程</returns>
        [Obsolete]
        public static VmProcedure LoadVmProcedure(string fileName)
        {
            lock (Locker)
            {
                string prcFileName = fileName + ".prc";

                // 流程地址
                string path = Path.Combine(StaticPara.CoordinateSystemPath, prcFileName);

                // 加载流程
                VmProcedure transPrc = VmProcedure.Load(path);

                // 获取工具
                IMVSCalibTransformModuTool transModule = (IMVSCalibTransformModuTool)transPrc.Modules[0];

                // 设置工具路径
                string xmlName = fileName + ".xml";

                // 流程地址
                string path1 = Path.Combine(StaticPara.CoordinateSystemPath, xmlName);

                transModule.ModuParams.LoadCalibPath = path1;

                // transPrc.SaveAs(path);

                return transPrc;
            }
        }

        [Obsolete]
        public static void CalibTrans()
        {
            VisionDesigner.CalibTrans.CCalibTransTool cCalibTransToolObj = new CCalibTransTool();

            cCalibTransToolObj.BasicParam.Coordinate = new MVD_POINT_F(0, 0);
            cCalibTransToolObj.ImportCalibFile("D:\\NPointCalib.iwcal");

            cCalibTransToolObj.Run();

            MVD_POINT_F a = cCalibTransToolObj.Result.Coordinate;

        }

        private static object locker1 = new object(); 

        public static CCalibTransTool GetCCalibTransTool(string name)
        {
            lock (locker1)
            {
                VisionDesigner.CalibTrans.CCalibTransTool cCalibTransToolObj = new CCalibTransTool();

                string path = Path.Combine(StaticPara.CoordinateSystemPath, name + ".iwcal");

                cCalibTransToolObj.ImportCalibFile(path);

                return cCalibTransToolObj;
            }
        }

        [Obsolete]
        public static AKRSPoint3D GetPoint3D(CCalibTransTool cCalibTransTool,AKRSPoint3D point3D,bool invert,CalibModuleEnum calibModule,bool isvm)
        {
            lock (Locker)
            {
                cCalibTransTool.BasicParam.Coordinate = new MVD_POINT_F(1224, 1024);

                cCalibTransTool.Run();

                MVD_POINT_F result1 = cCalibTransTool.Result.Coordinate;

                if (!isvm)
                {
                    result1 = new MVD_POINT_F(0, 0);
                }

                if (invert)
                {
                    cCalibTransTool.SetRunParam("TransformType", "Negative");
                }
                else
                {
                    cCalibTransTool.SetRunParam("TransformType", "Position");
                }

                cCalibTransTool.BasicParam.Coordinate = new MVD_POINT_F((float)point3D.X, (float)point3D.Y);

                cCalibTransTool.Run();

                MVD_POINT_F result = cCalibTransTool.Result.Coordinate;

                if (calibModule == CalibModuleEnum.AxisAndCameraIsTogether)
                {
                    return new AKRSPoint3D(result1.fX - result.fX, result1.fY - result.fY, 0);
                }
                else if (calibModule == CalibModuleEnum.CameraStaticUp)
                {

                    return new AKRSPoint3D(result1.fX - result.fX, result1.fY - result.fY, 0);
                }
                else if (calibModule == CalibModuleEnum.CameraStaticDown)
                {
                    return new AKRSPoint3D(result1.fX - result.fX, result1.fY - result.fY, 0);
                }


                return null;
            }
        }

        /// <summary>
        /// 计算坐标
        /// </summary>
        /// <param name="cCalibTransTool">工具</param>
        /// <param name="point3D">点位</param>
        /// <param name="calibModule">模式</param>
        /// <param name="invert">是否反转</param>
        /// <returns>结果</returns>

        public static AKRSPoint3D GetPoint3D(CCalibTransTool cCalibTransTool, AKRSPoint3D point3D,CalibModuleEnum calibModule, bool invert = false)
        {
            if (calibModule == CalibModuleEnum.RealisticCoordinateSystem)
            {
                MVD_POINT_F resultF = CalculateMvdPointF(cCalibTransTool, point3D, invert);

                return new AKRSPoint3D(resultF.fX, resultF.fY, 0);
            }

            MVD_POINT_F center = CalculateMvdPointF(cCalibTransTool, new AKRSPoint3D(1224, 1024, 0), invert);

            MVD_POINT_F result = CalculateMvdPointF(cCalibTransTool, point3D, invert);

            if (calibModule == CalibModuleEnum.AxisAndCameraIsTogether)
            {
                return new AKRSPoint3D(center.fX - result.fX, center.fY - result.fY, 0);
            }
            else if (calibModule == CalibModuleEnum.CameraStaticUp)
            {

                return new AKRSPoint3D(center.fX - result.fX, center.fY - result.fY, 0);
            }
            else if (calibModule == CalibModuleEnum.CameraStaticDown)
            {
                return new AKRSPoint3D(center.fX - result.fX, center.fY - result.fY, 0);
            }

            return null;
        }

        /// <summary>
        /// 计算坐标
        /// </summary>
        /// <param name="transTool">转换工具</param>
        /// <param name="point3D">点位</param>
        /// <param name="invert">是否反转</param>
        /// <returns>结果</returns>
        public static MVD_POINT_F CalculateMvdPointF(CCalibTransTool transTool, AKRSPoint3D point3D,bool invert)
        {
            if (invert)
            {
                transTool.SetRunParam("TransformType", "Negative");
            }
            else
            {
                transTool.SetRunParam("TransformType", "Position");
            }

            transTool.BasicParam.Coordinate = new MVD_POINT_F((float)point3D.X, (float)point3D.Y);

            transTool.Run();

            return transTool.Result.Coordinate;
        }

        public static void Save(List<AKRSPoint3D> reality, List<AKRSPoint3D> virtually, string name)
        {
            VisionDesigner.NPointCalib.CNPointCalibTool cNPointCalibToolObj = new CNPointCalibTool();


            List<AKRSPoint3D> points = virtually;
            List<AKRSPoint3D> point2 = reality;

            // 设置相机模式

            cNPointCalibToolObj.BasicParam.OffsetPointList.Clear();

            for (int i = 0; i < reality.Count; i++)
            {
                // 设置标定点
                MVD_CALIB_POINT_F stCalibPoint = new MVD_CALIB_POINT_F();
                stCalibPoint.stImageCoordinate.fX = (float)points[i].X;
                stCalibPoint.stImageCoordinate.fY = (float)points[i].Y;
                stCalibPoint.stWorldCoordinate.fX = (float)point2[i].X;
                stCalibPoint.stWorldCoordinate.fY = (float)point2[i].Y;
                cNPointCalibToolObj.BasicParam.OffsetPointList.Add(stCalibPoint);
            }

            cNPointCalibToolObj.Run();

            string path = Path.Combine(StaticPara.CoordinateSystemPath, name + ".iwcal");
            cNPointCalibToolObj.ExportCalibFile(path);

            // 获取标定结果
            VisionDesigner.NPointCalib.CNPointCalibResult cNPointCalibRes = cNPointCalibToolObj.Result;
            List<float> homoMatrix = cNPointCalibRes.PlatformCalibInfo.HomoMatrix;
        }

        /// <summary>
        /// 计算转换矩阵，标定点数需要设置在4~32个范围内
        /// </summary>
        /// <param name="cameraPoints">像素坐标点</param>
        /// <param name="axisPoints">轴坐标点</param>
        /// <returns>矩阵元素</returns>
        public static List<float> CalcTransMatrix(List<AKRSPoint3D> cameraPoints, List<AKRSPoint3D> axisPoints)
        {
            // 检查相机点数和轴点数是否一致
            if (cameraPoints.Count != axisPoints.Count)
            {
                throw new ArgumentException("相机点数和轴点数不一致", nameof(cameraPoints));
            }

            int pointNum = cameraPoints.Count;
            if (pointNum < 4 || pointNum > 32)
            {
                throw new ArgumentOutOfRangeException(nameof(cameraPoints), "标定点数需要设置在4~32个范围内");
            }

            CNPointCalibTool cNPointCalibToolObj = new();

            // 设置相机模式
            cNPointCalibToolObj.BasicParam.OffsetPointList.Clear();

            for (int i = 0; i < pointNum; i++)
            {
                // 设置标定点
                MVD_CALIB_POINT_F stCalibPoint = new();
                stCalibPoint.stImageCoordinate.fX = (float)cameraPoints[i].X;
                stCalibPoint.stImageCoordinate.fY = (float)cameraPoints[i].Y;
                stCalibPoint.stWorldCoordinate.fX = (float)axisPoints[i].X;
                stCalibPoint.stWorldCoordinate.fY = (float)axisPoints[i].Y;
                cNPointCalibToolObj.BasicParam.OffsetPointList.Add(stCalibPoint);
            }

            cNPointCalibToolObj.Run();

            // 获取标定结果
            CNPointCalibResult cNPointCalibRes = cNPointCalibToolObj.Result;
            List<float> homoMatrix = cNPointCalibRes.PlatformCalibInfo.HomoMatrix;
            return homoMatrix;
        }


        public enum CalibModuleEnum
        {
            AxisAndCameraIsTogether,
            CameraStaticUp,
            CameraStaticDown,
            RealisticCoordinateSystem

        }
    }
}
