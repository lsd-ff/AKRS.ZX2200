namespace AKRS.Galaxy2.CoordinateSystems.CoordinateSystems
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using VisionDesigner.CalibTrans;
    using static AKRS.Galaxy2.CoordinateSystems.CoordinateSystems.TransformTool;

    /// <summary>
    /// 坐标类
    /// </summary>
    public abstract class BaseCoordinateSystem
    {
        /// <summary>
        /// 坐标系名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 坐标系描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 两个坐标系坐标之间的差值
        /// </summary>
        public AKRSPoint3D Distance { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 两个坐标系之间的角度
        /// </summary>
        public double Degree { get; set; }

        /// <summary>
        /// 上层坐标系的名字
        /// </summary>
        public string UpperName { get; set; }

        /// <summary>
        /// 原始二维坐标，因为VM目前只支持2维
        /// </summary>
        public List<AKRSPoint3D> OwnPoint3D { get; set; } = new List<AKRSPoint3D>();

        /// <summary>
        /// 转出后的二维坐标
        /// </summary>
        public List<AKRSPoint3D> UpperPoint3D { get; set; } = new List<AKRSPoint3D>();

        /// <summary>
        /// 坐标转换工具
        /// </summary>
        [JsonIgnore]
        public TransformTool TransformTool { get; set; }

        /// <summary>
        /// 坐标转换流程
        /// </summary>
        [JsonIgnore]
        public CCalibTransTool CCalibTransTool { get; set; }

        /// <summary>
        /// 是否相机和轴
        /// </summary>
        public CalibModuleEnum CalibModule { get; set; } = CalibModuleEnum.AxisAndCameraIsTogether;

        /// <summary>
        /// 坐标系类型
        /// </summary>
        public CoordinateSystemTypeEnum Type { get; set; }

        /// <summary>
        /// 上层坐标系
        /// </summary>
        [JsonIgnore]
        public BaseCoordinateSystem UpperCoordinateSystem { get; set; }

        /// <summary>
        /// 向前转换
        /// </summary>
        /// <param name="value">点位</param>
        /// <returns>结果</returns>
        public AKRSPoint3D ForwardConvertCoordinate(AKRSPoint3D value)
        {
            AKRSPoint3D point = new AKRSPoint3D();
            
            if (this.Type == CoordinateSystemTypeEnum.General)
            {
                point.X = this.Distance.X + (value.X * Math.Cos(this.Degree) - value.Y * Math.Sin(this.Degree));

                point.Y = this.Distance.Y + (value.X * Math.Sin(this.Degree) + value.Y * Math.Cos(this.Degree));

                point.Z = value.Z + this.Distance.Z;
            }
            else if (this.Type == CoordinateSystemTypeEnum.Vm)
            {
                if (this.CCalibTransTool == null)
                {
                   this.CCalibTransTool = TransformTool.GetCCalibTransTool(this.Name);
                }

                point = TransformTool.GetPoint3D(CCalibTransTool, value, this.CalibModule);

                // 赋值 Z
                point.Z = value.Z + this.Distance.Z;
            }

            return point;
        }

        /// <summary>
        /// 向前转换
        /// </summary>
        /// <param name="value">点位</param>
        /// <returns>结果</returns>
        [Obsolete]
        public AKRSPoint2D ForwardConvertCoordinate(AKRSPoint2D value)
        {
            AKRSPoint2D point = new AKRSPoint2D();

            if (this.Type == CoordinateSystemTypeEnum.General)
            {
                point.X = this.Distance.X + (value.X * Math.Cos(this.Degree) - value.Y * Math.Sin(this.Degree));

                point.Y = this.Distance.Y + (value.X * Math.Sin(this.Degree) + value.Y * Math.Cos(this.Degree));
            }
            else if (this.Type == CoordinateSystemTypeEnum.Vm)
            {
            }

            return point;
        }

        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="value">G(0)坐标系上的点</param>
        /// <returns>自己坐标系上的点</returns>
        public AKRSPoint3D BackConvertCoordinate(AKRSPoint3D value)
        {
            AKRSPoint3D point = new AKRSPoint3D();

            if (this.Type == CoordinateSystemTypeEnum.General)
            {
                double distanceX = value.X - this.Distance.X;
                double distanceY = value.Y - this.Distance.Y;

                double length = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

                double degree = Math.Atan2(distanceY, distanceX);

                point.X = length * Math.Cos(degree - this.Degree);
                point.Y = length * Math.Sin(degree - this.Degree);
                point.Z = value.Z - this.Distance.Z;
            }
            else if (this.Type == CoordinateSystemTypeEnum.Vm)
            {
                throw new InvalidOperationException("不支持海康标定的逆运算");
            }

            return point;
        }

        /// <summary>
        /// 坐标转换
        /// </summary>
        /// <param name="value">G(0)坐标系上的点</param>
        /// <returns>自己坐标系上的点</returns>
        [Obsolete]
        public AKRSPoint2D BackConvertCoordinate(AKRSPoint2D value)
        {
            AKRSPoint2D point = new AKRSPoint2D();

            if (this.Type == CoordinateSystemTypeEnum.General)
            {
                double distanceX = value.X - this.Distance.X;
                double distanceY = value.Y - this.Distance.Y;

                double length = Math.Sqrt(distanceX * distanceX + distanceY * distanceY);

                double degree = Math.Atan2(distanceY, distanceX);

                point.X = length * Math.Cos(degree - this.Degree);
                point.Y = length * Math.Sin(degree - this.Degree);
            }
            else if (this.Type == CoordinateSystemTypeEnum.Vm)
            {
                if (this.TransformTool == null)
                {
                    this.TransformTool = new TransformTool();
                }
            }

            return point;
        }

        /// <summary>
        /// 更新坐标系
        /// </summary>
        /// <param name="oldPoint3D">原来的点</param>
        /// <param name="newPoint3D">更新之后的点</param>
        /// <param name="degree">弧度</param>
        public void UpdateCoordinateSystem(AKRSPoint3D oldPoint3D, AKRSPoint3D newPoint3D, double degree)
        {
            #region 新代码

            // 转换到上层坐标系计算
            AKRSPoint3D upperOldPoint = this.ForwardConvertCoordinate(oldPoint3D);

            // 转换到上层坐标系计算
            AKRSPoint3D upperNewPoint3D = this.ForwardConvertCoordinate(newPoint3D);

            // 差值
            AKRSPoint3D distance = upperNewPoint3D - upperOldPoint;

            if (Math.Abs(distance.X) > 10 || Math.Abs(distance.Y) > 10 || Math.Abs(distance.Z) > 10)
            {
                throw new Exception("坐标系矫正XYZ超出范围，矫正失败");
            }

            if (Math.Abs(degree) > 0.2)
            {
                throw new Exception("坐标系矫正角度超出范围，矫正失败");
            }

            AKRSPoint3D rotatePoint = this.Distance + distance;

            this.Distance.X = (rotatePoint.X - upperNewPoint3D.X) * Math.Cos(degree) - (rotatePoint.Y - upperNewPoint3D.Y) * Math.Sin(degree) + upperNewPoint3D.X;
            this.Distance.Y = (rotatePoint.Y - upperNewPoint3D.Y) * Math.Cos(degree) + (rotatePoint.X - upperNewPoint3D.X) * Math.Sin(degree) + upperNewPoint3D.Y;

            this.Degree = this.Degree + degree;

            this.Distance.Z = this.Distance.Z + distance.Z;

            #endregion
        }

        /// <summary>
        /// 更新坐标系
        /// </summary>
        /// <param name="elementCoordinate">坐标系</param>
        [Obsolete]
        public void UpdateCoordinateSystem(ElementCoordinate elementCoordinate)
        {
            if (Math.Abs(elementCoordinate.Point.X) > 5 || Math.Abs(elementCoordinate.Point.Y) > 5 || Math.Abs(elementCoordinate.Point.Z) > 2)
            {
                throw new Exception("坐标系矫正XY超出范围，矫正失败");
            }

            if (Math.Abs(elementCoordinate.Degree) > 0.2)
            {
                throw new Exception("坐标系矫正角度超出范围，矫正失败");
            }

            this.Distance = this.Distance + elementCoordinate.Point;
            this.Degree = elementCoordinate.Degree + this.Degree;
        }

        /// <summary>
        /// 建立坐标系
        /// </summary>
        /// <param name="upperPoint3D">点位在上层坐标中的位置</param>
        /// <param name="ownPoint3D">点位在自己中的位置</param>
        /// <param name="degree">角度</param>
        /// <param name="moduleEnum">类型</param>
        public void Init(AKRSPoint3D upperPoint3D, AKRSPoint3D ownPoint3D, double degree = 0, CalibModuleEnum moduleEnum = CalibModuleEnum.AxisAndCameraIsTogether)
        {
            this.OwnPoint3D.Clear();
            this.UpperPoint3D.Clear();
            this.Distance = upperPoint3D - ownPoint3D;
            this.Degree = degree;
            this.CalibModule = moduleEnum;
        }

        /// <summary>
        /// 建立坐标系
        /// </summary>
        /// <param name="elementCoordinate">坐标系元素</param>
        /// <param name="moduleEnum">类型</param>
        public void Init(ElementCoordinate elementCoordinate, CalibModuleEnum moduleEnum = CalibModuleEnum.AxisAndCameraIsTogether)
        {
            this.OwnPoint3D.Clear();
            this.UpperPoint3D.Clear();
            this.Distance = new AKRSPoint3D(
                elementCoordinate.Point.X,
                elementCoordinate.Point.Y,
                elementCoordinate.Point.Z);
            this.Degree = elementCoordinate.Degree;
            this.CalibModule = moduleEnum;
        }

        /// <summary>
        /// 建立坐标系
        /// </summary>
        /// <param name="upperPoint3D">上层坐标系点位</param>
        /// <param name="ownPoint3D">自己的坐标系点位</param>
        /// <param name="moduleEnum">类型</param>
        public void Init(List<AKRSPoint3D> upperPoint3D, List<AKRSPoint3D> ownPoint3D, CalibModuleEnum moduleEnum = CalibModuleEnum.AxisAndCameraIsTogether)
        {
            this.UpperPoint3D = upperPoint3D;
            this.OwnPoint3D = ownPoint3D;
            this.CalibModule = moduleEnum;
            TransformTool.Save(upperPoint3D, ownPoint3D, this.Name);

            // 因为 Z 传入的都是 0 所以注释掉
            //AKRSPoint3D point = new AKRSPoint3D();

            //for (int i = 0; i < upperPoint3D.Count; i++)
            //{
            //    point = point + upperPoint3D[i] - ownPoint3D[i];
            //}

            // this.Distance = point / upperPoint3D.Count;
        }

        /// <summary>
        /// 添加偏移
        /// </summary>
        /// <param name="point3D">偏移值</param>
        /// <param name="degree">角度</param>
        public void InitOffSet(AKRSPoint3D point3D, double degree)
        {
            this.Distance = this.Distance + point3D;
            this.Degree = this.Degree + degree;
        }
    }

    /// <summary>
    /// 坐标系类型
    /// </summary>
    public enum CoordinateSystemTypeEnum
    {
        /// <summary>
        /// 海康坐标系
        /// </summary>
        Vm,

        /// <summary>
        /// 普通坐标系
        /// </summary>
        General,
    }
}
