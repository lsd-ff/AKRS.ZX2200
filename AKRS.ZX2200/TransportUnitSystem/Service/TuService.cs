
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AKRS.ZX2200.TransportUnitSystem.Service
{
    using Accord.Math;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.Galaxy2.Recipe;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Information;
    using DevExpress.CodeParser;
    using DevExpress.CodeParser.CodeStyle.Formatting;
    using DevExpress.DirectX.NativeInterop.Direct3D;
    using DevExpress.Utils.Extensions;
    using DevExpress.XtraEditors;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using Module = AKRS.ZX2200.TransportUnitSystem.Module.Matter.Module;
    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    public static class TuService
    {
        /// <summary>
        /// 获取行列数
        /// </summary>
        /// <param name="rowCount">行数</param>
        /// <param name="columnCount">列数</param>
        /// <param name="index">索引</param>
        /// <param name="arrangement">排列方式</param>
        /// <returns>结果</returns>
        public static (int, int) RowAndColumn(int rowCount, int columnCount, int index, ArrangementEnum arrangement)
        {
            if (arrangement == ArrangementEnum.Normal)
            {
                int row = index / columnCount + 1;

                if (index % columnCount == 0)
                {
                    row--;
                }

                int column = index - ((row - 1) * columnCount);

                return (row, column);
            }
            else if (arrangement == ArrangementEnum.Snake)
            {
                int row = index / columnCount + 1;

                if (index % columnCount == 0)
                {
                    row--;
                }

                int column;
                if (row % 2 == 0)
                {
                    column = index - ((row - 1) * columnCount);
                }
                else
                {
                    column = columnCount - (row * columnCount - index);
                }

                return (row, column);
            }

            return (0, 0);
        }

        /// <summary>
        /// 获取行列数
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="columnCount">列数</param>
        /// <param name="rowCount">行数</param>
        /// <param name="arrangement">排列方式</param>
        /// <param name="workOrder">工作方式</param>
        /// <returns>结果</returns>
        public static int GetIndex(int index, int columnCount = 1, int rowCount = 1, ArrangementEnum arrangement = ArrangementEnum.Normal, WorkOrderEnum workOrder = WorkOrderEnum.LeftDownToRight)
        {
            int rowIndex = (index - 1) / columnCount + 1;

            int columnIndex = index - (rowIndex - 1) * columnCount;

            int realIndex = 0;

            if (workOrder == WorkOrderEnum.LeftDownToRight)
            {
                if (arrangement == ArrangementEnum.Normal)
                {
                    realIndex = index;
                }
                else
                {
                    if (rowIndex % 2 == 0)
                    {
                        realIndex = rowIndex * columnCount - index + (rowIndex - 1) * columnCount + 1;
                    }
                    else
                    {
                        realIndex = index;
                    }
                }
            }
            else if (workOrder == WorkOrderEnum.LeftDownToUp)
            {
                if (arrangement == ArrangementEnum.Normal)
                {
                    realIndex = rowCount * (columnIndex - 1) + rowIndex;
                }
                else
                {
                    if (columnIndex % 2 == 0)
                    {
                        realIndex = rowCount * (columnIndex - 1) + rowIndex;

                        realIndex = realIndex - rowIndex + rowCount - rowIndex + 1;
                    }
                    else
                    {
                        realIndex = rowCount * (columnIndex - 1) + rowIndex;
                    }
                }
            }
            else if (workOrder == WorkOrderEnum.LeftUpToRight)
            {
                if (arrangement == ArrangementEnum.Normal)
                {
                    realIndex = (rowCount - rowIndex) * columnCount + columnIndex;
                }
                else
                {
                    if ((rowCount - rowIndex) % 2 == 0)
                    {
                        realIndex = (rowCount - rowIndex) * columnCount + columnIndex;
                    }
                    else
                    {
                        realIndex = (rowCount - rowIndex) * columnCount + columnIndex;
                        realIndex = realIndex - columnIndex + columnCount - columnIndex + 1;
                    }
                }
            }
            else if (workOrder == WorkOrderEnum.LeftUpToDown)
            {
                if (arrangement == ArrangementEnum.Normal)
                {
                    realIndex = rowCount * (columnIndex - 1) + (rowCount - rowIndex + 1);
                }
                else
                {
                    if ((columnCount - columnIndex + 1) % 2 == 0)
                    {
                        realIndex = rowCount * (columnIndex - 1) + rowIndex;
                    }
                    else
                    {
                        realIndex = rowCount * (columnIndex - 1) + (rowCount - rowIndex + 1);
                    }
                }
            }
            else if (workOrder == WorkOrderEnum.RightDownToUp)
            {
                if (arrangement == ArrangementEnum.Normal)
                {
                    realIndex = rowCount * (columnCount - columnIndex) + rowIndex;
                }
                else
                {
                    if (columnIndex % 2 == 0)
                    {
                        realIndex = rowCount * (columnCount - columnIndex) + rowIndex;
                    }
                    else
                    {
                        realIndex = rowCount * (columnCount - columnIndex) + rowIndex;
                    }
                }
            }
            else if (workOrder == WorkOrderEnum.RightDownToLeft)
            {
                if (arrangement == ArrangementEnum.Normal)
                {
                    realIndex = columnCount * (rowIndex - 1) + columnCount - columnIndex + 1;
                }
                else
                {
                    if ((rowCount - rowIndex + 1) % 2 == 0)
                    {
                        realIndex = columnCount * (rowIndex - 1) + columnCount - columnIndex + 1;
                    }
                    else
                    {
                        realIndex = columnCount * (rowIndex - 1) + columnCount - columnIndex + 1;
                        realIndex = columnCount * (rowIndex - 1) + columnIndex;
                    }
                }
            }
            else if (workOrder == WorkOrderEnum.RightUpToDown)
            {
                if (arrangement == ArrangementEnum.Normal)
                {
                    realIndex = rowCount * (columnCount - columnIndex) + rowCount - rowIndex + 1;
                }
                else
                {
                    if ((columnCount - columnIndex + 1) % 2 == 0)
                    {
                        realIndex = rowCount * (columnCount - columnIndex) + rowIndex;
                    }
                    else
                    {
                        realIndex = rowCount * (columnCount - columnIndex) + rowIndex;
                        realIndex = rowCount * (columnCount - columnIndex) + rowCount - rowIndex + 1;
                    }
                }
            }
            else if (workOrder == WorkOrderEnum.RightUpToLeft)
            {
                if (arrangement == ArrangementEnum.Normal)
                {
                    realIndex = rowCount * columnCount - index + 1;
                }
                else
                {
                    if ((rowCount - rowIndex) % 2 == 0)
                    {
                        realIndex = rowCount * columnCount - index + 1;
                    }
                    else
                    {
                        realIndex = columnCount * (rowCount - rowIndex) + columnIndex;
                    }
                }
            }
            else
            {
                throw new Exception("顺序出现问题，请联系软件人员");
            }

            return realIndex;
        }

        /// <summary>
        /// 归一化
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="baseEntities">对象</param>
        /// <param name="x">x的值</param>
        /// <param name="y">y的值</param>
        public static void PointFNormalization<T>(List<T> baseEntities, int x, int y) where T : BaseMatter
        {
            List<PointF> list = new List<PointF>();

            // 寻找最大值和最小值
            float maxX = 0;
            float minX = 0;
            float maxY = 0;
            float minY = 0;
            for (int i = 0; i < baseEntities.Count; i++)
            {
                if (i == 0)
                {
                    maxX = (float)baseEntities[i].CoordinateSystem.Distance.X;
                    minX = (float)baseEntities[i].CoordinateSystem.Distance.X;
                    minY = (float)baseEntities[i].CoordinateSystem.Distance.Y;
                    maxY = (float)baseEntities[i].CoordinateSystem.Distance.Y;
                }
                else
                {
                    if (baseEntities[i].CoordinateSystem.Distance.X > maxX)
                    {
                        maxX = (float)baseEntities[i].CoordinateSystem.Distance.X;
                    }

                    if (baseEntities[i].CoordinateSystem.Distance.X < minX)
                    {
                        minX = (float)baseEntities[i].CoordinateSystem.Distance.X;
                    }

                    if (baseEntities[i].CoordinateSystem.Distance.Y > maxY)
                    {
                        maxY = (float)baseEntities[i].CoordinateSystem.Distance.Y;
                    }

                    if (baseEntities[i].CoordinateSystem.Distance.Y < minY)
                    {
                        minY = (float)baseEntities[i].CoordinateSystem.Distance.Y;
                    }
                }
            }

            float distanceX = maxX - minX;
            float distanceY = maxY - minY;

            if (distanceX == 0 || Math.Abs(distanceX) < 3)
            {
                distanceX = (float)x / 2;
            }

            if (distanceY == 0 || Math.Abs(distanceY) < 3)
            {
                distanceY = (float)y / 2;
            }

            for (int i = 0; i < baseEntities.Count; i++)
            {
                // 图像坐标系和显示坐标系是反的，Y轴需要翻转
                baseEntities[i].Rectangle = new Rectangle(
                   Math.Abs((int)((baseEntities[i].CoordinateSystem.Distance.X - minX) / distanceX * x)) + 20,
                  Math.Abs((int)((baseEntities[i].CoordinateSystem.Distance.Y - maxY) / distanceY * y)) + 20,
                    30,
                    20);
            }
        }


        /// <summary>
        /// 归一化
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="baseEntities">对象</param>
        /// <param name="x">x的值</param>
        /// <param name="y">y的值</param>
        public static void PointFNormalization<T>(List<T> baseEntities, PointF min, PointF max) where T : BaseMatter
        {
            List<PointF> list = new List<PointF>();

            if (baseEntities.Count == 1)
            {
                // 图像坐标系和显示坐标系是反的，Y轴需要翻转
                baseEntities[0].Rectangle = new Rectangle(
                    Math.Abs((int)Math.Max(min.X, max.X)),
                    Math.Abs((int)Math.Max(min.Y, max.Y)),
                    0,
                    0);

                return;
            }

            // 寻找最大值和最小值
            float maxX = 0;
            float minX = 0;
            float maxY = 0;
            float minY = 0;
            for (int i = 0; i < baseEntities.Count; i++)
            {
                if (i == 0)
                {
                    maxX = (float)baseEntities[i].CoordinateSystem.Distance.X;
                    minX = (float)baseEntities[i].CoordinateSystem.Distance.X;
                    minY = (float)baseEntities[i].CoordinateSystem.Distance.Y;
                    maxY = (float)baseEntities[i].CoordinateSystem.Distance.Y;
                }
                else
                {
                    if (baseEntities[i].CoordinateSystem.Distance.X > maxX)
                    {
                        maxX = (float)baseEntities[i].CoordinateSystem.Distance.X;
                    }

                    if (baseEntities[i].CoordinateSystem.Distance.X < minX)
                    {
                        minX = (float)baseEntities[i].CoordinateSystem.Distance.X;
                    }

                    if (baseEntities[i].CoordinateSystem.Distance.Y > maxY)
                    {
                        maxY = (float)baseEntities[i].CoordinateSystem.Distance.Y;
                    }

                    if (baseEntities[i].CoordinateSystem.Distance.Y < minY)
                    {
                        minY = (float)baseEntities[i].CoordinateSystem.Distance.Y;
                    }
                }
            }

            float distanceX = maxX - minX;
            float distanceY = maxY - minY;

            if (distanceX == 0)
            {
                distanceX = (max.X + min.X) / 2;
            }

            if (distanceY == 0)
            {
                distanceY = (max.Y + min.Y) / 2;
            }

            for (int i = 0; i < baseEntities.Count; i++)
            {
                // 图像坐标系和显示坐标系是反的，Y轴需要翻转
                baseEntities[i].Rectangle = new Rectangle(
                    Math.Abs((int)((baseEntities[i].CoordinateSystem.Distance.X - minX) / distanceX * (max.X - min.X) + min.X)),
                    Math.Abs((int)((1.0 - (baseEntities[i].CoordinateSystem.Distance.Y - minY) / distanceY) * (max.Y - min.Y) + min.Y)),
                    0,
                    0);
            }
        }

        /// <summary>
        /// 判断某一个点是否在矩形框中
        /// </summary>
        /// <param name="point">点</param>
        /// <param name="rectangle">矩形框</param>
        /// <returns>结果</returns>
        public static bool PointInRectangles(Point point, Rectangle rectangle)
        {
            if (point.X > rectangle.X && point.X < (rectangle.X + rectangle.Width))
            {
                if (point.Y > rectangle.Y && point.Y < (rectangle.Y + rectangle.Height))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 正常定位P1，P2，P3的名称
        /// </summary>
        public enum NormalVisionType
        {
            /// <summary>
            /// 定位点1
            /// </summary>
            P1Vision,

            /// <summary>
            /// 定位点2
            /// </summary>
            P2Vision,

            /// <summary>
            /// 定位点3
            /// </summary>
            P3Vision,

            /// <summary>
            /// 定位点4
            /// </summary>
            P4Vision,
        }

        /// <summary>
        /// 更具定位点类型，获取定位信息
        /// </summary>
        /// <param name="baseEntity">实体</param>
        /// <param name="type">类型</param>
        /// <returns>信息</returns>
        public static (AKRSPoint3D point3D, string pRName,bool Autofocus) GetVisionInfo(BaseMatter baseEntity, NormalVisionType type)
        {
            if (type == NormalVisionType.P1Vision)
            {
                return (baseEntity.Config.LocateConfig.P1VisionRelativePos, baseEntity.Config.LocateConfig.P1PRName, baseEntity.Config.LocateConfig.LocateUseConfig1.Autofocus);
            }
            else if (type == NormalVisionType.P2Vision)
            {
                return (baseEntity.Config.LocateConfig.P2VisionRelativePos, baseEntity.Config.LocateConfig.P2PRName, baseEntity.Config.LocateConfig.LocateUseConfig2.Autofocus);
            }
            else if (type == NormalVisionType.P3Vision)
            {
                return (baseEntity.Config.LocateConfig.P3VisionRelativePos, baseEntity.Config.LocateConfig.P3PRName, baseEntity.Config.LocateConfig.LocateUseConfig3.Autofocus);
            }
            else if (type == NormalVisionType.P4Vision)
            {
                return (baseEntity.Config.LocateConfig.P4VisionRelativePos, baseEntity.Config.LocateConfig.P4PRName, baseEntity.Config.LocateConfig.LocateUseConfig4.Autofocus);
            }

            throw new Exception("获取定位信息异常");
        }


        /// <summary>
        /// 根据类型返回信息
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="baseEntity">实体</param>
        /// <returns>结果</returns>
        public static VisionResult ChooseInfoByType(NormalVisionType type, BaseMatter baseEntity)
        {
            if (type == NormalVisionType.P1Vision)
            {
                return baseEntity.BaseInfo.LocateResultInfo.P1Info;
            }
            else if (type == NormalVisionType.P2Vision)
            {
                return baseEntity.BaseInfo.LocateResultInfo.P2Info;
            }
            else if (type == NormalVisionType.P3Vision)
            {
                return baseEntity.BaseInfo.LocateResultInfo.P3Info;
            }
            else if (type == NormalVisionType.P4Vision)
            {
                return baseEntity.BaseInfo.LocateResultInfo.P4Info;
            }

            return null;
        }

        /// <summary>
        /// 更新坐标系
        /// </summary>
        /// <param name="baseEntity">实体</param>
        /// <param name="type">定位类型类型(</param>
        public static void UpdateEntity(BaseMatter baseEntity, AdjustTypeEnum type)
        {
            if (type == AdjustTypeEnum.OnePoint)
            {
                UpdateEntity(baseEntity, NormalVisionType.P1Vision);
            }
            else if (type == AdjustTypeEnum.TwoPoints)
            {
                UpdateEntity(baseEntity, NormalVisionType.P2Vision);
            }
            else if (type == AdjustTypeEnum.ThreePoints)
            {
                UpdateEntity(baseEntity, NormalVisionType.P3Vision);
            }
            else if (type == AdjustTypeEnum.FourPoints)
            {
                UpdateEntity(baseEntity, NormalVisionType.P4Vision);
            }
        }

        /// <summary>
        /// 更新坐标系
        /// </summary>
        /// <param name="baseEntity">实体</param>
        /// <param name="type">定位类型类型(</param>
        public static void UpdateEntity(BaseMatter baseEntity, NormalVisionType type)
        {
            baseEntity.BaseInfo.LocateResultInfo.P1Info.Bitmap = null;
            baseEntity.BaseInfo.LocateResultInfo.P2Info.Bitmap = null;
            baseEntity.BaseInfo.LocateResultInfo.P3Info.Bitmap = null;
            baseEntity.BaseInfo.LocateResultInfo.P4Info.Bitmap = null;

            // 根据定位的配置计算出原始点位和现在的点位
            AKRSPoint3D oldPoint = CalculateOldPoint(baseEntity.Config.LocateConfig);
            AKRSPoint3D newPoint = CalculateNewPoint(baseEntity.BaseInfo.LocateResultInfo, baseEntity.Config.LocateConfig);


            // 海康的坐标系是顺时针的，所有需要加负号
            double newDegree = -CalculateUpdateAngle(
                baseEntity.BaseInfo.LocateResultInfo,
                baseEntity.Config.LocateConfig);

            double oldDegree = baseEntity.CoordinateSystem.Degree;

            // 上层坐标系现在的角度
            GeneralCoordinateSystem upperSystem = (GeneralCoordinateSystem)baseEntity.CoordinateSystem.UpperCoordinateSystem;
            double upperSystemAngle = upperSystem.DegreeInG0();

            // 更新的角度为：视觉定位的角度 - 示教时的角度 - 上层的角度
            double updateDegree = newDegree - upperSystemAngle - 0;

            if (baseEntity.Config.LocateConfig.IsCalculateAngleByTwoPoint && baseEntity.Config.LocateConfig.AdjustType >= AdjustTypeEnum.TwoPoints)
            {
                double oldAngle = Math.Atan2(
                    baseEntity.Config.LocateConfig.P2VisionRelativePos.Y - baseEntity.Config.LocateConfig.P1VisionRelativePos.Y,
                    baseEntity.Config.LocateConfig.P2VisionRelativePos.X - baseEntity.Config.LocateConfig.P1VisionRelativePos.X);

                double newAngle = Math.Atan2(
                    baseEntity.BaseInfo.LocateResultInfo.P2Info.ResultPoint3D.Y - baseEntity.BaseInfo.LocateResultInfo.P1Info.ResultPoint3D.Y,
                    baseEntity.BaseInfo.LocateResultInfo.P2Info.ResultPoint3D.X - baseEntity.BaseInfo.LocateResultInfo.P1Info.ResultPoint3D.X);

                updateDegree = Math.Atan(newAngle - oldAngle) - upperSystemAngle - 0;
            }

            if (baseEntity.Config.LocateConfig.IsThreePointFittingCircle
                && (baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.ThreePoints || baseEntity.Config.LocateConfig.AdjustType == AdjustTypeEnum.FourPoints))
            {
                oldPoint = MathHelper.GetCentre(
                    baseEntity.Config.LocateConfig.P1VisionRelativePos,
                    baseEntity.Config.LocateConfig.P2VisionRelativePos,
                    baseEntity.Config.LocateConfig.P3VisionRelativePos);

                newPoint = MathHelper.GetCentre(
                    baseEntity.BaseInfo.LocateResultInfo.P1Info.ResultPoint3D,
                    baseEntity.BaseInfo.LocateResultInfo.P2Info.ResultPoint3D,
                    baseEntity.BaseInfo.LocateResultInfo.P3Info.ResultPoint3D);
            }

            baseEntity.CoordinateSystem.UpdateCoordinateSystem(oldPoint, newPoint, updateDegree);

            baseEntity.BaseInfo.IsVisioned = true;
        }

        /// <summary>
        /// 计算新的点位
        /// </summary>
        /// <param name="locateResultInfo">定位结果</param>
        /// <param name="locateConfig">定位使用信息</param>
        /// <returns>定位出来的点位</returns> 
        public static AKRSPoint3D CalculateNewPoint(LocateResultInfo locateResultInfo, LocateConfig locateConfig)
        {
            double x = 0;
            double y = 0;
            int xCount = 0;
            int yCount = 0;

            if (locateConfig.LocateUseConfig1.UseX)
            {
                x = locateResultInfo.P1Info.ResultPoint3D.X;
                xCount++;
            }

            if (locateConfig.LocateUseConfig2.UseX && locateConfig.AdjustType > AdjustTypeEnum.OnePoint)
            {
                x = locateResultInfo.P2Info.ResultPoint3D.X + x;
                xCount++;
            }

            if (locateConfig.LocateUseConfig3.UseX && locateConfig.AdjustType > AdjustTypeEnum.TwoPoints)
            {
                x = locateResultInfo.P3Info.ResultPoint3D.X + x;
                xCount++;
            }

            if (locateConfig.LocateUseConfig4.UseX && locateConfig.AdjustType > AdjustTypeEnum.ThreePoints)
            {
                x = locateResultInfo.P4Info.ResultPoint3D.X + x;
                xCount++;
            }

            if (xCount != 0)
            {
                x = x / xCount;
            }


            if (locateConfig.LocateUseConfig1.UseY)
            {
                y = locateResultInfo.P1Info.ResultPoint3D.Y;
                yCount++;
            }

            if (locateConfig.LocateUseConfig2.UseY && locateConfig.AdjustType > AdjustTypeEnum.OnePoint)
            {
                y = locateResultInfo.P2Info.ResultPoint3D.Y + y;
                yCount++;
            }

            if (locateConfig.LocateUseConfig3.UseY && locateConfig.AdjustType > AdjustTypeEnum.TwoPoints)
            {
                y = locateResultInfo.P3Info.ResultPoint3D.Y + y;
                yCount++;
            }

            if (locateConfig.LocateUseConfig4.UseY && locateConfig.AdjustType > AdjustTypeEnum.ThreePoints)
            {
                y = locateResultInfo.P4Info.ResultPoint3D.Y + y;
                yCount++;
            }

            if (yCount != 0)
            {
                y = y / yCount;
            }

            #region 弃用

            //if (locateConfig.LocateUseConfig1.UseX && locateConfig.LocateUseConfig2.UseX)
            //{
            //    x = ((locateResultInfo.P1Info.ResultPoint3D + locateResultInfo.P2Info.ResultPoint3D) / 2).X;
            //}
            //else if (locateConfig.LocateUseConfig1.UseX && !locateConfig.LocateUseConfig2.UseX)
            //{
            //    x = locateResultInfo.P1Info.ResultPoint3D.X;
            //}
            //else
            //{
            //    x = locateResultInfo.P2Info.ResultPoint3D.X;
            //}

            //if (locateConfig.LocateUseConfig1.UseY && locateConfig.LocateUseConfig2.UseY)
            //{
            //    y = ((locateResultInfo.P1Info.ResultPoint3D + locateResultInfo.P2Info.ResultPoint3D) / 2).Y;
            //}
            //else if (locateConfig.LocateUseConfig1.UseY && !locateConfig.LocateUseConfig2.UseY)
            //{
            //    y = locateResultInfo.P1Info.ResultPoint3D.Y;
            //}
            //else
            //{
            //    y = locateResultInfo.P2Info.ResultPoint3D.Y;
            //}

            #endregion

            return new AKRSPoint3D(x, y, 0);
        }

        /// <summary>
        /// 计算新的点位
        /// </summary>
        /// <param name="locateConfig">定位结果</param>
        /// <returns>定位出来的点位</returns> 
        public static AKRSPoint3D CalculateOldPoint(LocateConfig locateConfig)
        {
            double x = 0;
            double y = 0;
            double xCount = 0;
            double yCount = 0;

            if (locateConfig.LocateUseConfig1.UseX)
            {
                x = locateConfig.P1VisionRelativePos.X;
                xCount++;
            }

            if (locateConfig.LocateUseConfig2.UseX && locateConfig.AdjustType > AdjustTypeEnum.OnePoint)
            {
                x = locateConfig.P2VisionRelativePos.X + x;
                xCount++;
            }

            if (locateConfig.LocateUseConfig3.UseX && locateConfig.AdjustType > AdjustTypeEnum.TwoPoints)
            {
                x = locateConfig.P3VisionRelativePos.X + x;
                xCount++;
            }

            if (locateConfig.LocateUseConfig4.UseX && locateConfig.AdjustType > AdjustTypeEnum.ThreePoints)
            {
                x = locateConfig.P4VisionRelativePos.X + x;
                xCount++;
            }

            if (xCount != 0)
            {
                x = x / xCount;
            }


            if (locateConfig.LocateUseConfig1.UseY)
            {
                y = locateConfig.P1VisionRelativePos.Y;
                yCount++;
            }

            if (locateConfig.LocateUseConfig2.UseY && locateConfig.AdjustType > AdjustTypeEnum.OnePoint)
            {
                y = locateConfig.P2VisionRelativePos.Y + y;
                yCount++;
            }

            if (locateConfig.LocateUseConfig3.UseY && locateConfig.AdjustType > AdjustTypeEnum.TwoPoints)
            {
                y = locateConfig.P3VisionRelativePos.Y + y;
                yCount++;
            }

            if (locateConfig.LocateUseConfig4.UseY && locateConfig.AdjustType > AdjustTypeEnum.ThreePoints)
            {
                y = locateConfig.P4VisionRelativePos.Y + y;
                yCount++;
            }

            if (yCount != 0)
            {
                y = y / yCount;
            }


            #region 弃用

            //if (locateConfig.LocateUseConfig1.UseX && locateConfig.LocateUseConfig2.UseX)
            //{
            //    x = ((locateConfig.P1VisionRelativePos + locateConfig.P2VisionRelativePos) / 2).X;
            //}
            //else if (locateConfig.LocateUseConfig1.UseX && !locateConfig.LocateUseConfig2.UseX)
            //{
            //    x = locateConfig.P1VisionRelativePos.X;
            //}
            //else
            //{
            //    x = locateConfig.P2VisionRelativePos.X;
            //}

            //if (locateConfig.LocateUseConfig1.UseY && locateConfig.LocateUseConfig2.UseY)
            //{
            //    y = ((locateConfig.P1VisionRelativePos + locateConfig.P2VisionRelativePos) / 2).Y;
            //}
            //else if (locateConfig.LocateUseConfig1.UseY && !locateConfig.LocateUseConfig2.UseY)
            //{
            //    y = locateConfig.P1VisionRelativePos.Y;
            //}
            //else
            //{
            //    y = locateConfig.P2VisionRelativePos.Y;
            //}

            #endregion

            return new AKRSPoint3D(x, y, 0);
        }

        /// <summary>
        /// 计算更新的弧度
        /// </summary>
        /// <param name="locateResultInfo">定位结果</param>
        /// <param name="locateConfig">定位使用信息</param>
        /// <returns>定位出来的弧度</returns> 
        public static double CalculateUpdateAngle(LocateResultInfo locateResultInfo, LocateConfig locateConfig)
        {
            double newAngle = 0;
            double newAngleCount = 0;

            /* 这里两点定位的时候也可以用两个点去确认角度 */

            if (locateConfig.LocateUseConfig1.UseAngle)
            {
                newAngle = locateResultInfo.P1Info.MatchResult.Angle;
                newAngleCount++;
            }

            if (locateConfig.LocateUseConfig2.UseAngle && locateConfig.AdjustType > AdjustTypeEnum.OnePoint)
            {
                newAngle = locateResultInfo.P2Info.MatchResult.Angle + newAngle;
                newAngleCount++;
            }

            if (locateConfig.LocateUseConfig3.UseAngle && locateConfig.AdjustType > AdjustTypeEnum.TwoPoints)
            {
                newAngle = locateResultInfo.P3Info.MatchResult.Angle + newAngle;
                newAngleCount++;
            }

            if (locateConfig.LocateUseConfig3.UseAngle && locateConfig.AdjustType > AdjustTypeEnum.ThreePoints)
            {
                newAngle = locateResultInfo.P4Info.MatchResult.Angle + newAngle;
                newAngleCount++;
            }

            if (newAngleCount != 0)
            {
                newAngle = newAngle / newAngleCount;
            }

            return newAngle * Math.PI / 180;
        }

        /// <summary>
        /// 创建matter的坐标系
        /// </summary>
        /// <param name="baseMatter">物体</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        /// <param name="elementCoordinate">坐标系元素</param>
        public static void CreateMatterCoordinate(BaseMatter baseMatter, GeneralCoordinateSystem upperCoordinateSystem, ElementCoordinate elementCoordinate)
        {
            baseMatter.CoordinateSystem = new GeneralCoordinateSystem(
                baseMatter.Name,
                CoordinateSystemTypeEnum.General,
                upperCoordinateSystem);

            baseMatter.CoordinateSystem.Init(elementCoordinate);
        }

        /// <summary>
        /// 对TU进行屏蔽
        /// </summary>
        /// <param name="transportUnit">实体</param>
        /// <param name="config">配置文件</param>
        public static void ModifyMatter(TransportUnit transportUnit, OtherConfig config)
        {
            try
            {
                if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
                {
                    foreach (Substrate substrate in transportUnit.Substrates)
                    {
                        foreach (Module module in substrate.Modules)
                        {
                            foreach (BondPosition bondPosition in module.BondPositions)
                            {
                                bondPosition.MatterProductState = bondPosition.OppositeSex.MatterProductState;
                            }
                        }
                    }
                }
                else
                {
                    if (ProductConfiguration.GetInstance().TransportUnitConfig.LagerNumber)
                    {
                        foreach (Substrate substrate in transportUnit.Substrates)
                        {
                            foreach (Module module in substrate.Modules)
                            {
                                module.SubstrateNum = substrate.Index;
                                foreach (BondPosition bondPosition in module.BondPositions)
                                {
                                    bondPosition.SubstrateNum = substrate.Index;
                                    bondPosition.ModuleNum = module.Index;

                                    List<MatterProductInformationMin> MatterProductInformationMins = config.MatterProductInfoMax[substrate.Index - 1][module.Index - 1];

                                    MatterProductInformationMin matterProductInformationMin = MatterProductInformationMins.Find(it => it.BondPositionName == bondPosition.Name);
                                    bondPosition.MatterProductState = matterProductInformationMin.MatterProductState;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (Substrate substrate in transportUnit.Substrates)
                        {
                            foreach (Module module in substrate.Modules)
                            {
                                module.SubstrateNum = substrate.Index;
                                foreach (BondPosition bondPosition in module.BondPositions)
                                {
                                    bondPosition.SubstrateNum = substrate.Index;
                                    bondPosition.ModuleNum = module.Index;
                                    if (config.MatterProductInfo.Exists(
                                            it => it.SubstrateIndex == substrate.Index && it.ModuleIndex == module.Index
                                                  && it.BondPositionName == bondPosition.Name))
                                    {
                                        MatterProductInformation matterProductInformation = config.MatterProductInfo.Find(
                                            it => it.SubstrateIndex == substrate.Index && it.ModuleIndex == module.Index
                                                  && it.BondPositionName == bondPosition.Name);
                                        bondPosition.MatterProductState = matterProductInformation.MatterProductState;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                config.ClearInformation();
                ProductConfiguration.GetInstance().Save();
            }
        }

        /// <summary>
        /// 两点距离检查
        /// </summary>
        /// <param name="locateConfig">定位配置</param>
        /// <param name="locateResultInfo">定位结构</param>
        /// <returns>结果</returns>
        public static bool TwoPointCheck(LocateConfig locateConfig, LocateResultInfo locateResultInfo)
        {
            if (!locateConfig.DistanceCheck)
            {
                return true;
            }

            double oldDistance = TwoPointDistance(locateConfig.P1VisionRelativePos, locateConfig.P2VisionRelativePos);

            double newDistance = TwoPointDistance(
                locateResultInfo.P1Info.ResultPoint3D,
                locateResultInfo.P2Info.ResultPoint3D);

            if (Math.Abs(oldDistance - newDistance) > locateConfig.DistanceTolerance)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 两点之间距离
        /// </summary>
        /// <param name="point1">点位1</param>
        /// <param name="point2">点位2</param>
        /// <returns>距离</returns>
        public static double TwoPointDistance(AKRSPoint3D point1, AKRSPoint3D point2)
        {
            double distance = Math.Sqrt(
                (point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y));
            return distance;
        }


        /// <summary>
        /// 将集合转化成蛇形
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="columnCount">列数</param>
        /// <returns>蛇形排布</returns>
        public static List<int> ToSnakeList(List<int> list, int columnCount)
        {
            List<int> tpList = new List<int>();

            for (int i = 0; i < list.Count + 1; i++)
            {
                if (i != list.Count && ((list[i] - 1) / columnCount) % 2 != 0)
                {
                    tpList.Add(list[i]);
                }
                else
                {
                    tpList.Reverse();

                    for (int j = 0; j < tpList.Count; j++)
                    {
                        list[i - tpList.Count + j] = tpList[j];
                    }

                    tpList.Clear();
                }
            }

            return list;
        }

        #region 复制程式，弃用

        /// <summary>
        /// 复制程式的时候复制PR的方法
        /// </summary>
        /// <param name="targetName">目标的PR名称</param>
        public static void CopyTuPr(string currentName, string targetName)
        {
            CopyMatter(ProductConfiguration.GetInstance().TransportUnitConfig.LocateConfig, currentName, targetName);
        }

        /// <summary>
        /// 复制matterPR
        /// </summary>
        /// <param name="baseMatter">matter</param>
        /// <param name="currentName">当前名称</param>
        /// <param name="targetName">目标名称</param>
        public static void CopyMatter(LocateConfig locateConfig, string currentName, string targetName)
        {
            CopyPr(locateConfig.P1PRName, currentName, targetName);
            CopyPr(locateConfig.P2PRName, currentName, targetName);
            CopyPr(locateConfig.P3PRName, currentName, targetName);
        }

        /// <summary>
        /// 通过配方名称配置pR
        /// </summary>
        /// <param name="prName">PR名称</param>
        /// <param name="currentName">当前配方名称</param>
        /// <param name="targetName">目标配方名称</param>
        public static void CopyPr(string prName, string currentName, string targetName)
        {
            // 判断当前是否存在
            PREntity originPrEntity = (PREntity)VisionEntityRepository.GetInstance().Find(prName);

            if (originPrEntity != null)
            {
                string newPrName = prName.Remove(0, currentName.Length) + targetName;

                // 获取当前配方的名称
                PREntity.CopyPREntity(prName, newPrName);
            }
        }

        /// <summary>
        /// 复制程式时调用的复制TU的PR
        /// </summary>
        /// <param name="oldName">被复制的程式的名称</param>
        /// <param name="newName">目标程式的名称</param>
        public static void CopyOldRecipePR(string oldName, string newName)
        {
            List<string> prNames = new List<string>();

            // 在PR库里面找到所有包涵旧配方的名称
            List<BaseVisionEntity> baseVisionEntities = VisionEntityRepository.GetInstance().PRVisionList;

            List<string> oldPRNameList = new List<string>();

            // 获取旧程式的ID
            string recipeID = RecipeRepository.GetInstance().GetRecipeIDByName(oldName).ToString();
            // 获取旧程式的文件路径
            ProductConfiguration.FilePath = Path.Combine(PathConfig.RecipeRootDirPath, recipeID, "ProductConfiguration.json");
            // 清除现有实例的
            ProductConfiguration.GetInstance().ClearProductConfigurationInstance();

            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                List<string> oldPRName = ProductConfiguration.GetInstance().OppositeSexConfiguration.BaseConfigs.Select(it => it.LocateConfig.Name).ToList();

                List<string> listMark = new List<string>() { "Mark1", "Mark2", "Mark3", "Mark4" };
                foreach (string item in oldPRName)
                {
                    foreach (var item2 in listMark)
                    {
                        oldPRNameList.Add(oldName + item + item2);
                    }
                } 
            }
            else
            {
                oldPRNameList.Add(oldName + "TransportUnitLeftMark1");
                oldPRNameList.Add(oldName + "TransportUnitLeftMark2");
                oldPRNameList.Add(oldName + "TransportUnitRightMark1");
                oldPRNameList.Add(oldName + "TransportUnitRightMark2");
                oldPRNameList.Add(oldName + "TransportUnitMark1");
                oldPRNameList.Add(oldName + "TransportUnitMark2");
                oldPRNameList.Add(oldName + "SubstrateMark1");
                oldPRNameList.Add(oldName + "SubstrateMark2");
                oldPRNameList.Add(oldName + "SubstrateMark3");
                oldPRNameList.Add(oldName + "SubstrateMark4");
                oldPRNameList.Add(oldName + "ModuleMark1");
                oldPRNameList.Add(oldName + "ModuleMark2");
                oldPRNameList.Add(oldName + "ModuleMark3");
                oldPRNameList.Add(oldName + "ModuleMark4");
                oldPRNameList.Add(oldName + "TransportUnitID识别1");
                oldPRNameList.Add(oldName + "SubstrateID识别1");
                oldPRNameList.Add(oldName + "ModuleID识别1");

                foreach (var bp in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
                {
                    oldPRNameList.Add(oldName + bp.Name + "Mark1");
                    oldPRNameList.Add(oldName + bp.Name + "Mark2");
                    oldPRNameList.Add(oldName + bp.Name + "Mark3");
                    oldPRNameList.Add(oldName + bp.Name + "Mark4");
                    oldPRNameList.Add(oldName + bp.Name + "ID识别1");
                }
            }

            ProductConfiguration.FilePath = ProductConfiguration.FilePath = Path.Combine(PathConfig.RecipeDirPath, "ProductConfiguration.json");
            ProductConfiguration.GetInstance().ClearProductConfigurationInstance();

            foreach (BaseVisionEntity baseVisionEntity in baseVisionEntities)
            {
                foreach (var name in oldPRNameList)
                {
                    if (baseVisionEntity.GetName() == name)
                    {
                        prNames.Add(baseVisionEntity.GetName());
                    }
                }
            }

            // 复制PR
            foreach (string prName in prNames)
            {
                string newPrName = prName.Replace(oldName, newName);

                PREntity.CopyPREntity(prName, newPrName);
            }

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 删除旧程式PR
        /// </summary>
        /// <param name="oldName">旧程式名称</param>
        public static void DeleteOldRecipePR(string oldName)
        {
            List<string> prNames = new List<string>();

            // 在PR库里面找到所有包涵旧配方的名称
            List<BaseVisionEntity> baseVisionEntities = VisionEntityRepository.GetInstance().PRVisionList;

            List<string> oldPRNameList = new List<string>();

            // 获取旧程式的ID
            string recipeID = RecipeRepository.GetInstance().GetRecipeIDByName(oldName).ToString();
            // 获取旧程式的文件路径
            ProductConfiguration.FilePath = Path.Combine(PathConfig.RecipeRootDirPath, recipeID, "ProductConfiguration.json");
            // 清除现有实例的
            ProductConfiguration.GetInstance().ClearProductConfigurationInstance();

            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                List<string> oldPRName = ProductConfiguration.GetInstance().OppositeSexConfiguration.BaseConfigs.Select(it => it.LocateConfig.Name).ToList();

                List<string> listMark = new List<string>() { "Mark1", "Mark2", "Mark3", "Mark4" };
                foreach (string item in oldPRName)
                {
                    foreach (var item2 in listMark)
                    {
                        oldPRNameList.Add(oldName + item + item2);
                    }
                }
            }
            else
            {
                oldPRNameList.Add(oldName + "TransportUnitLeftMark1");
                oldPRNameList.Add(oldName + "TransportUnitLeftMark2");
                oldPRNameList.Add(oldName + "TransportUnitRightMark1");
                oldPRNameList.Add(oldName + "TransportUnitRightMark2");
                oldPRNameList.Add(oldName + "TransportUnitMark1");
                oldPRNameList.Add(oldName + "TransportUnitMark2");
                oldPRNameList.Add(oldName + "SubstrateMark1");
                oldPRNameList.Add(oldName + "SubstrateMark2");
                oldPRNameList.Add(oldName + "SubstrateMark3");
                oldPRNameList.Add(oldName + "SubstrateMark4");
                oldPRNameList.Add(oldName + "ModuleMark1");
                oldPRNameList.Add(oldName + "ModuleMark2");
                oldPRNameList.Add(oldName + "ModuleMark3");
                oldPRNameList.Add(oldName + "ModuleMark4");
                oldPRNameList.Add(oldName + "TransportUnitID识别1");
                oldPRNameList.Add(oldName + "SubstrateID识别1");
                oldPRNameList.Add(oldName + "ModuleID识别1");

                foreach (var bp in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
                {
                    oldPRNameList.Add(oldName + bp.Name + "Mark1");
                    oldPRNameList.Add(oldName + bp.Name + "Mark2");
                    oldPRNameList.Add(oldName + bp.Name + "Mark3");
                    oldPRNameList.Add(oldName + bp.Name + "Mark4");
                    oldPRNameList.Add(oldName + bp.Name + "ID识别1");
                }
            }

            ProductConfiguration.FilePath = ProductConfiguration.FilePath = Path.Combine(PathConfig.RecipeDirPath, "ProductConfiguration.json");
            ProductConfiguration.GetInstance().ClearProductConfigurationInstance();

            foreach (BaseVisionEntity baseVisionEntity in baseVisionEntities)
            {
                foreach (var name in oldPRNameList)
                {
                    //if (baseVisionEntity.GetName().Contains(name))
                    //{
                    //    prNames.Add(baseVisionEntity.GetName());
                    //}

                    // 2025/7/11 改 上面的写法会删除名字有重叠的PR
                    if (baseVisionEntity.GetName() == name)
                    {
                        prNames.Add(baseVisionEntity.GetName());
                    }
                }
            }

            // 删除PRC文件
            foreach (string prName in prNames)
            {
                PREntity.DeletePREntity(prName);
            }

            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 将Tu里面的PR复制一份到系统1，避免系统1和系统2冲突
        /// </summary>
        public static void CopyTransportUnitPrToSystem1()
        {
            CopyOldRecipePR(
                MachineConfigContext.GetInstance().RecipeName,
                MachineConfigContext.GetInstance().RecipeName + "System1");
        }

        /// <summary>
        /// 当系统1不在进行自动工作时，删除副本
        /// </summary>
        public static void DeleteTransportUnitPrInSystem1()
        {
            List<string> prNames = new List<string>();

            // 在PR库里面找到所有包涵旧配方的名称
            List<BaseVisionEntity> baseVisionEntities = VisionEntityRepository.GetInstance().PRVisionList;
            foreach (BaseVisionEntity baseVisionEntity in baseVisionEntities)
            {
                if (baseVisionEntity.GetName().Contains(MachineConfigContext.GetInstance().RecipeName + "System1"))
                {
                    PREntity.DeletePREntity(baseVisionEntity.GetName());
                }
            }
        }

        #endregion


        /// <summary>
        /// 根据行列获取
        /// </summary>
        /// <returns>结果</returns>
        public static int[,] GetMatrixIndex()
        {
            SubstrateConfig substrateConfig = ProductConfiguration.GetInstance().SubstrateConfig;

            int[,] matrix = new int[substrateConfig.RowCount, substrateConfig.ColumnCount];

            for (int i = 1; i <= substrateConfig.Count; i++)
            {
                int index = TuService.GetIndex(
                    i,
                    substrateConfig.ColumnCount,
                    substrateConfig.RowCount,
                    substrateConfig.Arrangement,
                    substrateConfig.WorkOrderEnum);


                int row = i / substrateConfig.ColumnCount + 1;

                if (i % substrateConfig.ColumnCount == 0)
                {
                    row--;
                }

                int column = i - ((row - 1) * substrateConfig.ColumnCount);

                matrix[row - 1, column - 1] = index;
            }

            return matrix;
        }

        /// <summary>
        /// 获取分段前段的sub索引集合
        /// </summary>
        /// <returns>结果</returns>
        public static List<int> GetFirstMatrixIndex()
        {
            List<int> list = new List<int>();
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                for (int i = 0; i < ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.DownConfigs.Count; i++)
                {
                    if (ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.DownConfigs[i].ElementCoordinate.Point.X < 0)
                    {
                        list.Add(i + 1);
                    }
                }
            }
            else
            {
                SubstrateConfig substrateConfig = ProductConfiguration.GetInstance().SubstrateConfig;

                int[,] matrix = GetMatrixIndex();

                for (int i = 0; i < substrateConfig.RowCount; i++)
                {
                    for (int j = 0; j < substrateConfig.ColumnCount / 2; j++)
                    {
                        list.Add(matrix[i, j]);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取分段后段的sub索引集合
        /// </summary>
        /// <returns>结果</returns>
        public static List<int> GetLastMatrixIndex()
        {
            SubstrateConfig substrateConfig = ProductConfiguration.GetInstance().SubstrateConfig;

            int[,] matrix = GetMatrixIndex();

            List<int> list = new List<int>();

            for (int i = 0; i < substrateConfig.RowCount; i++)
            {
                for (int j = substrateConfig.ColumnCount / 2; j < substrateConfig.ColumnCount; j++)
                {
                    list.Add(matrix[i, j]);
                }
            }

            return list;
        }


        /// <summary>
        /// 保存Mapping到本地
        /// </summary>
        /// <param name="transportUnit">载具</param>
        public static void SaveMappingToLocal(TransportUnit transportUnit)
        {
            if (MachineSoftwareConfiguration.GetInstance().AutoSaveBondPositionComponentMapping)
            {
                SaveBondPositionComponentMappingToLocal(transportUnit);
                return;
            }

            try
            {
                List<string> list = new List<string>();

                list.Add(transportUnit.TransportUnitInfo.RecipeName);
                list.Add(transportUnit.TransportUnitInfo.SubstrateLotNumber);
                list.Add(transportUnit.TransportUnitInfo.SubstrateNumber);
                list.Add(transportUnit.TransportUnitInfo.FaceType);

                if (ProductConfiguration.GetInstance().SubstrateConfig.IsMultiple
                    && ProductConfiguration.GetInstance().ModuleConfig.IsMultiple)
                {
                    list.Add(
                        (ProductConfiguration.GetInstance().SubstrateConfig.RowCount
                         * ProductConfiguration.GetInstance().ModuleConfig.RowCount).ToString());

                    list.Add(
                        (ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount
                         * ProductConfiguration.GetInstance().ModuleConfig.ColumnCount).ToString());

                    int totalCount = ProductConfiguration.GetInstance().SubstrateConfig.RowCount
                                      * ProductConfiguration.GetInstance().ModuleConfig.RowCount
                                      * ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount
                                      * ProductConfiguration.GetInstance().ModuleConfig.ColumnCount;

                    list.Add(totalCount.ToString());

                    int successCount = 0;

                    for (int i = 0; i < ProductConfiguration.GetInstance().SubstrateConfig.RowCount; i++)
                    {
                        for (int j = 0; j < ProductConfiguration.GetInstance().ModuleConfig.RowCount; j++)
                        {
                            string columnString = string.Empty;
                            for (int k = 0; k < ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount; k++)
                            {
                                for (int l = 0; l < ProductConfiguration.GetInstance().ModuleConfig.ColumnCount; l++)
                                {
                                    Substrate substrate =
                                        transportUnit.Substrates.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == k + 1);

                                    Module module = substrate.Modules.Find(it => it.RowIndex == j + 1 && it.ColumnIndex == l + 1);

                                    if (module.IsFail())
                                    {
                                        columnString += "X";
                                    }
                                    else if (module.IsModuleProcessFinishedInSystem2)
                                    {
                                        columnString += "2";
                                        successCount++;
                                    }
                                    else
                                    {
                                        columnString += "1";
                                    }
                                }
                            }

                            list.Add(columnString);
                        }
                    }

                    list.Insert(7, $"OK:{successCount.ToString()}");
                    list.Insert(7, $"NG:{(totalCount - successCount).ToString()}");
                }
                else if (ProductConfiguration.GetInstance().SubstrateConfig.IsMultiple)
                {
                    list.Add(ProductConfiguration.GetInstance().SubstrateConfig.RowCount.ToString());

                    list.Add(ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount.ToString());

                    int totalCount = ProductConfiguration.GetInstance().SubstrateConfig.RowCount
                                     * ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount;

                    list.Add(totalCount.ToString());

                    int successCount = 0;

                    for (int i = 0; i < ProductConfiguration.GetInstance().SubstrateConfig.RowCount; i++)
                    {
                        string columnString = string.Empty;

                        for (int j = 0; j < ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount; j++)
                        {
                            Substrate substrate =
                                transportUnit.Substrates.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == j + 1);

                            Module module = substrate.Modules[0];

                            if (module.IsFail())
                            {
                                columnString += "X";
                            }
                            else if (module.IsModuleProcessFinishedInSystem2)
                            {
                                columnString += "2";
                                successCount++;
                            }
                            else
                            {
                                columnString += "1";
                            }
                        }

                        list.Add(columnString);
                    }

                    list.Insert(7, $"OK:{successCount.ToString()}");
                    list.Insert(7, $"NG:{(totalCount - successCount).ToString()}");
                }
                else if (ProductConfiguration.GetInstance().ModuleConfig.IsMultiple)
                {
                    list.Add(ProductConfiguration.GetInstance().ModuleConfig.RowCount.ToString());

                    list.Add(ProductConfiguration.GetInstance().ModuleConfig.ColumnCount.ToString());

                    int totalCount = ProductConfiguration.GetInstance().ModuleConfig.RowCount
                                     * ProductConfiguration.GetInstance().ModuleConfig.ColumnCount;

                    list.Add(totalCount.ToString());

                    int successCount = 0;

                    for (int i = 0; i < ProductConfiguration.GetInstance().ModuleConfig.RowCount; i++)
                    {
                        string columnString = string.Empty;

                        for (int j = 0; j < ProductConfiguration.GetInstance().ModuleConfig.ColumnCount; j++)
                        {
                            Substrate substrate =
                                transportUnit.Substrates[0];

                            Module module =
                                substrate.Modules.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == j + 1);

                            if (module.IsFail())
                            {
                                columnString += "X";
                            }
                            else if (module.IsModuleProcessFinishedInSystem2)
                            {
                                columnString += "2";
                                successCount++;
                            }
                            else
                            {
                                columnString += "1";
                            }
                        }

                        list.Add(columnString);
                    }

                    list.Insert(7, $"OK:{successCount.ToString()}");
                    list.Insert(7, $"NG:{(totalCount - successCount).ToString()}");
                }

                if (!Directory.Exists("D:\\ZX2200存储数据\\" + MachineConfigContext.GetInstance().RecipeName))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(
                        "D:\\ZX2200存储数据\\" + MachineConfigContext.GetInstance().RecipeName);
                    directoryInfo.Create();
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                           "D:\\ZX2200存储数据\\" + MachineConfigContext.GetInstance().RecipeName + "//" + transportUnit.TransportUnitInfo.SubstrateNumber + ".txt",
                           false))
                {
                    foreach (string line in list)
                    {
                        file.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show(e.Message + "保存失败");
            }
        }

        /// <summary>
        /// 保存Bond位置组件Mapping到本地
        /// </summary>
        /// <param name="transportUnit">载具</param>
        public static void SaveBondPositionComponentMappingToLocal(TransportUnit transportUnit)
        {
            try
            {
                List<string> list = new List<string>();

                list.Add(transportUnit.TransportUnitInfo.RecipeName);
                list.Add(transportUnit.TransportUnitInfo.SubstrateLotNumber);
                list.Add(transportUnit.TransportUnitInfo.SubstrateNumber);
                list.Add(transportUnit.TransportUnitInfo.FaceType);

                if (ProductConfiguration.GetInstance().SubstrateConfig.IsMultiple
                    && ProductConfiguration.GetInstance().ModuleConfig.IsMultiple)
                {
                    list.Add(
                        (ProductConfiguration.GetInstance().SubstrateConfig.RowCount
                         * ProductConfiguration.GetInstance().ModuleConfig.RowCount).ToString());

                    list.Add(
                        (ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount
                         * ProductConfiguration.GetInstance().ModuleConfig.ColumnCount).ToString());

                    int totalCount = ProductConfiguration.GetInstance().SubstrateConfig.RowCount
                                      * ProductConfiguration.GetInstance().ModuleConfig.RowCount
                                      * ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount
                                      * ProductConfiguration.GetInstance().ModuleConfig.ColumnCount;

                    list.Add(totalCount.ToString());

                    int successCount = 0;

                    for (int i = 0; i < ProductConfiguration.GetInstance().SubstrateConfig.RowCount; i++)
                    {
                        for (int j = 0; j < ProductConfiguration.GetInstance().ModuleConfig.RowCount; j++)
                        {
                            string columnString = string.Empty;
                            for (int k = 0; k < ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount; k++)
                            {
                                for (int l = 0; l < ProductConfiguration.GetInstance().ModuleConfig.ColumnCount; l++)
                                {
                                    Substrate substrate =
                                        transportUnit.Substrates.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == k + 1);

                                    Module module = substrate.Modules.Find(it => it.RowIndex == j + 1 && it.ColumnIndex == l + 1);

                                    if (module.IsFail())
                                    {
                                        columnString += "X" + " ";
                                    }
                                    else if (module.IsModuleProcessFinishedInSystem2)
                                    {
                                        columnString += module.BondPositions[0].BondPositionInfo.ComponentCode + " ";

                                        successCount++;
                                    }
                                    else
                                    {
                                        columnString += "1" + " ";
                                    }
                                }
                            }

                            list.Add(columnString);
                        }
                    }

                    list.Insert(7, $"OK:{successCount.ToString()}");
                    list.Insert(7, $"NG:{(totalCount - successCount).ToString()}");
                }
                else if (ProductConfiguration.GetInstance().SubstrateConfig.IsMultiple)
                {
                    list.Add(ProductConfiguration.GetInstance().SubstrateConfig.RowCount.ToString());

                    list.Add(ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount.ToString());

                    int totalCount = ProductConfiguration.GetInstance().SubstrateConfig.RowCount
                                     * ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount;

                    list.Add(totalCount.ToString());

                    int successCount = 0;

                    for (int i = 0; i < ProductConfiguration.GetInstance().SubstrateConfig.RowCount; i++)
                    {
                        string columnString = string.Empty;

                        for (int j = 0; j < ProductConfiguration.GetInstance().SubstrateConfig.ColumnCount; j++)
                        {
                            Substrate substrate =
                                transportUnit.Substrates.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == j + 1);

                            Module module = substrate.Modules[0];

                            if (module.IsFail())
                            {
                                columnString += "X" + " ";
                            }
                            else if (module.IsModuleProcessFinishedInSystem2)
                            {
                                columnString += module.BondPositions[0].BondPositionInfo.ComponentCode + " ";

                                successCount++;
                            }
                            else
                            {
                                columnString += "1" + " ";
                            }
                        }

                        list.Add(columnString);
                    }

                    list.Insert(7, $"OK:{successCount.ToString()}");
                    list.Insert(7, $"NG:{(totalCount - successCount).ToString()}");
                }
                else if (ProductConfiguration.GetInstance().ModuleConfig.IsMultiple)
                {
                    list.Add(ProductConfiguration.GetInstance().ModuleConfig.RowCount.ToString());

                    list.Add(ProductConfiguration.GetInstance().ModuleConfig.ColumnCount.ToString());

                    int totalCount = ProductConfiguration.GetInstance().ModuleConfig.RowCount
                                     * ProductConfiguration.GetInstance().ModuleConfig.ColumnCount;

                    list.Add(totalCount.ToString());

                    int successCount = 0;

                    for (int i = 0; i < ProductConfiguration.GetInstance().ModuleConfig.RowCount; i++)
                    {
                        string columnString = string.Empty;

                        for (int j = 0; j < ProductConfiguration.GetInstance().ModuleConfig.ColumnCount; j++)
                        {
                            Substrate substrate =
                                transportUnit.Substrates[0];

                            Module module =
                                substrate.Modules.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == j + 1);

                            if (module.IsFail())
                            {
                                columnString += "X" + " ";
                            }
                            else if (module.IsModuleProcessFinishedInSystem2)
                            {
                                columnString += module.BondPositions[0].BondPositionInfo.ComponentCode + " ";

                                successCount++;
                            }
                            else
                            {
                                columnString += "1" + " ";
                            }
                        }

                        list.Add(columnString);
                    }

                    list.Insert(7, $"OK:{successCount.ToString()}");
                    list.Insert(7, $"NG:{(totalCount - successCount).ToString()}");
                }

                if (!Directory.Exists("D:\\ZX2200存储数据\\" + MachineConfigContext.GetInstance().RecipeName))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(
                        "D:\\ZX2200存储数据\\" + MachineConfigContext.GetInstance().RecipeName);
                    directoryInfo.Create();
                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(
                           "D:\\ZX2200存储数据\\" + MachineConfigContext.GetInstance().RecipeName + "//" + transportUnit.TransportUnitInfo.SubstrateNumber + ".txt",
                           false))
                {
                    foreach (string line in list)
                    {
                        file.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show(e.Message + "保存失败");
            }
        }


        /// <summary>
        /// 选择地址
        /// </summary>
        /// <returns>结果</returns>
        public static string GetTuNameByChoosePath()
        {
            // 创建一个OpenFileDialog实例
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // 设置对话框的标题
            openFileDialog.Title = "选择文件";

            // 设置过滤器，限制用户只能选择特定类型的文件
            openFileDialog.Filter = "TXT文本文件|*.txt";

            // 默认文件夹
            openFileDialog.InitialDirectory = "D:\\ZX2200存储数据\\" + MachineConfigContext.GetInstance().RecipeName;

            // 设置是否允许选择多个文件
            openFileDialog.Multiselect = false;

            // 显示对话框，并判断用户是否点击了"确定"
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // 获取用户选择的文件路径
                return openFileDialog.FileName;
            }

            return null;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <returns>结果</returns>
        public static TransportUnit ReadTransportMapping()
        {
            string fileName = GetTuNameByChoosePath();

            if (fileName == null)
            {
                return null;
            }

            return ReadTransportMapping(fileName);
        }

        /// <summary>
        /// 读取mapping
        /// </summary>
        /// <param name="transportId">Id</param>
        /// <returns>载具</returns>
        public static TransportUnit ReadTransportMapping(string transportId)
        {
            //try
            //{

            if (MachineSoftwareConfiguration.GetInstance().AutoSaveBondPositionComponentMapping)
            {
                return ReadTransportMappingByComponent(transportId);
            }

            if (!File.Exists(transportId))
            {
                AKRSXtraMessageBox.Show("未找到框架Mapping，请确认二维码是否有误");
                return null;
            }

            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            List<string> strs1 = new List<string>(File.ReadAllLines(transportId));

            TransportUnit transportUnit = new TransportUnit("Mapping反序列化");

            // 注入制程
            System2Domain.GetInstance().ActionNodesService.InjectSteps(transportUnit);
            System1Domain.GetInstance().ActionNodeController.InjectSteps(transportUnit);

            transportUnit.TransportUnitInfo.RecipeName = strs1[0];
            transportUnit.TransportUnitInfo.SubstrateLotNumber = strs1[1];
            transportUnit.TransportUnitInfo.SubstrateNumber = strs1[2];
            transportUnit.TransportUnitInfo.FaceType = strs1[3];

            if (productConfiguration.SubstrateConfig.IsMultiple
                && productConfiguration.ModuleConfig.IsMultiple)
            {
                for (int i = 0; i < productConfiguration.SubstrateConfig.RowCount; i++)
                {
                    for (int j = 0; j < productConfiguration.ModuleConfig.RowCount; j++)
                    {
                        char[] columnString = strs1[i * productConfiguration.ModuleConfig.RowCount + j + 9].ToCharArray();

                        for (int k = 0; k < productConfiguration.SubstrateConfig.ColumnCount; k++)
                        {
                            for (int l = 0; l < productConfiguration.ModuleConfig.ColumnCount; l++)
                            {
                                Substrate substrate =
                                    transportUnit.Substrates.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == k + 1);

                                Module module = substrate.Modules.Find(it => it.RowIndex == j + 1 && it.ColumnIndex == l + 1);

                                if (columnString[k * productConfiguration.ModuleConfig.ColumnCount + l].ToString() == "X")
                                {
                                    module.SetMatterDisable();
                                }
                                else if (columnString[k * productConfiguration.ModuleConfig.ColumnCount + l].ToString() == "2")
                                {
                                    foreach (BondPosition bp in module.BondPositions)
                                    {
                                        bp.BondPositionInfo.SetFinishedInSystem1();
                                        bp.BondPositionInfo.SetFinishedInSystem2();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (productConfiguration.SubstrateConfig.IsMultiple)
            {
                for (int i = 0; i < productConfiguration.SubstrateConfig.RowCount; i++)
                {
                    char[] columnString = strs1[i + 9].ToCharArray();

                    for (int j = 0; j < productConfiguration.SubstrateConfig.ColumnCount; j++)
                    {
                        Substrate substrate =
                            transportUnit.Substrates.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == j + 1);

                        Module module = substrate.Modules[0];

                        if (columnString[j].ToString() == "X")
                        {
                            substrate.SetMatterDisable();
                        }
                        else if (columnString[j].ToString() == "2")
                        {
                            foreach (BondPosition bp in module.BondPositions)
                            {
                                bp.BondPositionInfo.SetFinishedInSystem1();
                                bp.BondPositionInfo.SetFinishedInSystem2();
                            }
                        }
                    }
                }
            }
            else if (productConfiguration.ModuleConfig.IsMultiple)
            {
                for (int i = 0; i < productConfiguration.ModuleConfig.RowCount; i++)
                {
                    char[] columnString = strs1[i + 9].ToCharArray();

                    for (int j = 0; j < productConfiguration.ModuleConfig.ColumnCount; j++)
                    {
                        Substrate substrate = transportUnit.Substrates[0];
                        Module module = substrate.Modules.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == j + 1);

                        if (columnString[j].ToString() == "X")
                        {
                            module.SetMatterDisable();
                        }
                        else if (columnString[j].ToString() == "2")
                        {
                            foreach (BondPosition bp in module.BondPositions)
                            {
                                bp.BondPositionInfo.SetFinishedInSystem1();
                                bp.BondPositionInfo.SetFinishedInSystem2();
                            }
                        }
                    }
                }
            }

            return transportUnit;
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);

            //    return null;
            //}
        }

        /// <summary>
        /// 读取mapping
        /// </summary>
        /// <param name="transportId">Id</param>
        /// <returns>载具</returns>
        public static TransportUnit ReadTransportMappingByComponent(string transportId)
        {
            //try
            //{
            if (!File.Exists(transportId))
            {
                AKRSXtraMessageBox.Show("未找到框架Mapping，请确认二维码是否有误");
                return null;
            }

            ProductConfiguration productConfiguration = ProductConfiguration.GetInstance();

            List<string> strs1 = new List<string>(File.ReadAllLines(transportId));

            TransportUnit transportUnit = new TransportUnit("Mapping反序列化");

            // 注入制程
            System2Domain.GetInstance().ActionNodesService.InjectSteps(transportUnit);
            System1Domain.GetInstance().ActionNodeController.InjectSteps(transportUnit);

            transportUnit.TransportUnitInfo.RecipeName = strs1[0];
            transportUnit.TransportUnitInfo.SubstrateLotNumber = strs1[1];
            transportUnit.TransportUnitInfo.SubstrateNumber = strs1[2];
            transportUnit.TransportUnitInfo.FaceType = strs1[3];

            if (productConfiguration.SubstrateConfig.IsMultiple
                && productConfiguration.ModuleConfig.IsMultiple)
            {
                for (int i = 0; i < productConfiguration.SubstrateConfig.RowCount; i++)
                {
                    for (int j = 0; j < productConfiguration.ModuleConfig.RowCount; j++)
                    {
                        string[] columnString = strs1[i * productConfiguration.ModuleConfig.RowCount + j + 9].Split();

                        for (int k = 0; k < productConfiguration.SubstrateConfig.ColumnCount; k++)
                        {
                            for (int l = 0; l < productConfiguration.ModuleConfig.ColumnCount; l++)
                            {
                                Substrate substrate =
                                    transportUnit.Substrates.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == k + 1);

                                Module module = substrate.Modules.Find(it => it.RowIndex == j + 1 && it.ColumnIndex == l + 1);

                                if (columnString[k * productConfiguration.ModuleConfig.ColumnCount + l].ToString() == "X")
                                {
                                    module.SetMatterDisable();
                                }
                                else if (columnString[k * productConfiguration.ModuleConfig.ColumnCount + l].ToString() != "1")
                                {
                                    foreach (BondPosition bp in module.BondPositions)
                                    {
                                        bp.BondPositionInfo.SetFinishedInSystem1();
                                        bp.BondPositionInfo.SetFinishedInSystem2();
                                        bp.BondPositionInfo.ComponentCode = columnString[k * productConfiguration.ModuleConfig.ColumnCount + l].ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (productConfiguration.SubstrateConfig.IsMultiple)
            {
                for (int i = 0; i < productConfiguration.SubstrateConfig.RowCount; i++)
                {
                    string[] columnString = strs1[i + 9].Split();

                    for (int j = 0; j < productConfiguration.SubstrateConfig.ColumnCount; j++)
                    {
                        Substrate substrate =
                            transportUnit.Substrates.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == j + 1);

                        Module module = substrate.Modules[0];

                        if (columnString[j].ToString() == "X")
                        {
                            substrate.SetMatterDisable();
                        }
                        else if (columnString[j].ToString() != "1")
                        {
                            foreach (BondPosition bp in module.BondPositions)
                            {
                                bp.BondPositionInfo.SetFinishedInSystem1();
                                bp.BondPositionInfo.SetFinishedInSystem2();
                                bp.BondPositionInfo.ComponentCode = columnString[j].ToString();
                            }
                        }
                    }
                }
            }
            else if (productConfiguration.ModuleConfig.IsMultiple)
            {
                for (int i = 0; i < productConfiguration.ModuleConfig.RowCount; i++)
                {
                    string[] columnString = strs1[i + 9].Split();

                    for (int j = 0; j < productConfiguration.ModuleConfig.ColumnCount; j++)
                    {
                        Substrate substrate = transportUnit.Substrates[0];
                        Module module = substrate.Modules.Find(it => it.RowIndex == i + 1 && it.ColumnIndex == j + 1);

                        if (columnString[j].ToString() == "X")
                        {
                            module.SetMatterDisable();
                        }
                        else if (columnString[j].ToString() != "1")
                        {
                            foreach (BondPosition bp in module.BondPositions)
                            {
                                bp.BondPositionInfo.SetFinishedInSystem1();
                                bp.BondPositionInfo.SetFinishedInSystem2();
                                bp.BondPositionInfo.ComponentCode = columnString[j].ToString();
                            }
                        }
                    }
                }
            }

            return transportUnit;
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);

            //    return null;
            //}
        }
    }
}
