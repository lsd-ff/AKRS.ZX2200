using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// CreateEndpoint
    /// </summary>
    /// <param name="logicalPoint"></param>
    /// <returns>Endpoint</returns>
    public delegate Endpoint CreateEndpoint(Point2D logicalPoint);

    /// <summary>
    /// 曲线。 线段
    /// </summary>
    /// <remarks>核心概念。由多条线段“首尾相接”组合而成。这些线段又是由一个个“端点”按次序相连而成。</remarks>
    
    public class Curve
    {
        /// <summary>
        /// 点信息集合
        /// </summary>
        private List<EndpointUnit> endpointUnitList = new List<EndpointUnit>();

        /// <summary>
        /// 是否可见的。
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Curve()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="array">点集合</param>
        /// <param name="createEndpoint">创建点委托</param>
        public Curve(EPEPointData[] array, CreateEndpoint createEndpoint)
            : this()
        {
            if (array != null)
            {
                foreach (EPEPointData point in array)
                {
                    Endpoint segmentPoint = createEndpoint.Invoke(point.Point);
                    this.endpointUnitList.Add(new EndpointUnit(segmentPoint, point.Info));
                }
            }
        }

        /// <summary>
        /// 获取Point
        /// </summary>
        /// <returns>EPEPoint 数组</returns>
        public EPEPointData[] GetEPEPoints()
        {
            List<EPEPointData> list = new List<EPEPointData>(this.endpointUnitList.Count);
            foreach (EndpointUnit endpointUnit in this.endpointUnitList)
            {
                list.Add(new EPEPointData(endpointUnit.Endpoint.LogicalPoint, endpointUnit.EndpointInfo));
            }

            return list.ToArray();
        }

        /// <summary>
        /// 清空曲线，即删除所有的端点。
        /// </summary>
        public void Clear()
        {
            this.endpointUnitList.Clear();
        }

        /// <summary>
        /// 获取曲线包含的端点的数量。
        /// </summary>
        public int EndpointCount => this.endpointUnitList.Count;

        /// <summary>
        /// 获取曲线包含的线段的数量。
        /// </summary>
        public int SegmentCount
        {
            get
            {
                int iSegmentCount = this.endpointUnitList.Count - 1;
                if (iSegmentCount < 0)
                {
                    iSegmentCount = 0;
                }

                return iSegmentCount;
            }
        }

        /// <summary>
        /// 追加一个点。
        /// </summary>
        /// <param name="endpoint">点的坐标数据</param>
        /// <param name="endpointInfo">包含点的其他参数如速度、延时。</param>
        /// <returns>新加的端点的Index。</returns>
        public int AppendPoint(Endpoint endpoint, EndpointSetting endpointInfo)
        {
            this.endpointUnitList.Add(new EndpointUnit(endpoint, endpointInfo));
            return this.endpointUnitList.Count - 1;
        }

        /// <summary>
        /// 插入一个点。
        /// </summary>
        /// <param name="iTargetIndex">索引</param>
        /// <param name="endpoint">点的坐标数据</param>
        /// <param name="endpointInfo">包含点的其他参数如速度、延时。</param>
        public void InsertPoint(int iTargetIndex, Endpoint endpoint, EndpointSetting endpointInfo)
        {
            this.endpointUnitList.Insert(iTargetIndex, new EndpointUnit(endpoint, endpointInfo));            
        }

        /// <summary>
        /// 隐藏线段。
        /// </summary>
        /// <param name="iSegmentIndex"></param>
        //public void HideSegment(int iSegmentIndex)
        //{
        //    EndpointUnit endpointUnit = this.endpointUnitList[iSegmentIndex];
        //    EndpointInfo endpointInfo = endpointUnit.EndpointInfo;
        //    if (!endpointInfo.Dummied)
        //    {
        //        endpointInfo.Dummied = true;
        //        endpointUnit.EndpointInfo = endpointInfo;
        //    }
        //}

        /// <summary>
        /// 显示线段。
        /// </summary>
        /// <param name="iSegmentIndex"></param>
        //public void ShowSegment(int iSegmentIndex)
        //{
        //    EndpointUnit endpointUnit = this.endpointUnitList[iSegmentIndex];
        //    EndpointInfo endpointInfo = endpointUnit.EndpointInfo;
        //    if (endpointInfo.Dummied)
        //    {
        //        endpointInfo.Dummied = false;
        //        endpointUnit.EndpointInfo = endpointInfo;
        //    }
        //}

        /// <summary>
        /// 判断线段是否隐藏。
        /// </summary>
        /// <param name="iSegmentIndex"></param>
        /// <returns></returns>
        //public bool IsSegmentHidden(int iSegmentIndex)
        //{
        //    return !this.endpointUnitList[iSegmentIndex].EndpointInfo.Dummied;
        //}

        /// <summary>
        /// 颠倒线段的Flag。
        /// </summary>
        /// <param name="iSegmentIndex"></param>
        //public void ReverseSegmentFlag(int iSegmentIndex)
        //{
        //    EndpointUnit endpointUnit = this.endpointUnitList[iSegmentIndex];
        //    EndpointInfo endpointInfo = endpointUnit.EndpointInfo;
        //    endpointInfo.Dummied = !endpointInfo.Dummied;
        //    endpointUnit.EndpointInfo = endpointInfo;
        //}

        /// <summary>
        /// 获取内部EndpointUnit列表的引用。
        /// </summary>
        internal IList<EndpointUnit> EndpointUnitList => this.endpointUnitList;

        /// <summary>
        /// 获取指定的端点。
        /// </summary>
        /// <param name="iEndpointIndex">索引</param>
        /// <returns>Endpoint</returns>
        public Endpoint GetEndpoint(int iEndpointIndex)
        {
            return this.endpointUnitList[iEndpointIndex].Endpoint;
        }

        /// <summary>
        /// 获取指定端点的信息。
        /// </summary>
        /// <param name="iEndpointIndex">索引</param>
        /// <returns>EndpointInfo</returns>
        public EndpointSetting GetEndpointInfo(int iEndpointIndex)
        {
            return this.endpointUnitList[iEndpointIndex].EndpointInfo;
        }

        /// <summary>
        /// 修改端点。
        /// </summary>
        /// <param name="iEndpointIndex">索引</param>
        /// <param name="endpoint">endpoint</param>
        public void ModifyEndpoint(int iEndpointIndex, Endpoint endpoint)
        {
            this.endpointUnitList[iEndpointIndex].Endpoint = endpoint;
        }

        /// <summary>
        /// 修改端点的信息。
        /// </summary>
        /// <param name="iEndpointIndex">索引</param>
        /// <param name="endpointInfo">endpointinfo</param>
        public void ModifyEndpointInfo(int iEndpointIndex, EndpointSetting endpointInfo)
        {
            this.endpointUnitList[iEndpointIndex].EndpointInfo = endpointInfo;
        }

        /// <summary>
        /// 修改线段的信息。
        /// </summary>
        /// <param name="iSegmentIndex"></param>
        /// <param name="segmentInfo"></param>
        public void ModifySegmentInfo(int iSegmentIndex, EndpointSetting segmentInfo)
        {
            ModifyEndpointInfo(iSegmentIndex, segmentInfo);
        }

        /// <summary>
        /// 平移变换。
        /// </summary>
        /// <param name="xIncrement"></param>
        /// <param name="yIncrement"></param>
        public void Translate(float xIncrement, float yIncrement)
        {
            foreach (EndpointUnit endpointUnit in this.endpointUnitList)
            {
                Point2D logicalPoint = endpointUnit.Endpoint.LogicalPoint;
                logicalPoint.X += xIncrement;
                logicalPoint.Y += yIncrement;

                endpointUnit.Endpoint = new Endpoint(logicalPoint);
            }
        }

        /// <summary>
        /// 旋转变换。
        /// </summary>
        /// <param name="rotationCenter"></param>
        /// <param name="degree"></param>
        /// <param name="additionalPoints"></param>
        public void Rotate(Point2D rotationCenter, float degree, ref Point2D[] additionalPoints)
        {
            Matrix matrix = new Matrix();
            matrix.RotateAt(degree, (PointF)rotationCenter);

            PointF[] points = new PointF[this.endpointUnitList.Count + additionalPoints.Length];
            int i = 0;
            foreach (EndpointUnit endpointUnit in this.endpointUnitList)
            {
                points[i] = (PointF)endpointUnit.Endpoint.LogicalPoint;
                i++;
            }

            foreach (Point2D point in additionalPoints)
            {
                points[i] = (PointF)point;
                i++;
            }

            matrix.TransformPoints(points);

            i = 0;
            foreach (EndpointUnit endpointUnit in this.endpointUnitList)
            {
                endpointUnit.Endpoint = new Endpoint((Point2D)points[i]);
                i++;
            }

            for (int j = 0; j < additionalPoints.Length; j++)
            {
                additionalPoints[j] = (Point2D)points[i];
                i++;
            }
        }

        /// <summary>
        /// 缩放变换。
        /// </summary>
        /// <param name="fixedPoint"></param>
        /// <param name="xScale"></param>
        /// <param name="yScale"></param>
        /// <param name="additionalPoints"></param>
        /// <exception cref="InvalidOperationException">缩放失败。</exception>
        public void Scale(Point2D fixedPoint, float xScale, float yScale, ref Point2D[] additionalPoints)
        {
            PointF[] points = new PointF[this.endpointUnitList.Count + additionalPoints.Length];

            int i = 0;
            foreach (EndpointUnit endpointUnit in this.endpointUnitList)
            {
                points[i] = (PointF)endpointUnit.Endpoint.LogicalPoint;
                i++;
            }

            foreach (Point2D point in additionalPoints)
            {
                points[i] = (PointF)point;
                i++;
            }

            Matrix matrix = new Matrix();
            matrix.Translate(-1 * fixedPoint.X, -1 * fixedPoint.Y, MatrixOrder.Append);
            matrix.Scale(xScale, yScale, MatrixOrder.Append);
            matrix.Translate(fixedPoint.X, fixedPoint.Y, MatrixOrder.Append);
            matrix.TransformPoints(points);

            foreach (Point2D point in points)
            {
                if (float.IsNaN(point.X) || float.IsNaN(point.Y))
                    throw new InvalidOperationException("");
            }

            i = 0;
            foreach (EndpointUnit endpointUnit in this.endpointUnitList)
            {
                endpointUnit.Endpoint = new Endpoint((Point2D)points[i]);
                i++;
            }

            for (int j = 0; j < additionalPoints.Length; j++)
            {
                additionalPoints[j] = (Point2D)points[i];
                i++;
            }


        }

        /// <summary>
        /// 移除指定的端点。
        /// </summary>
        /// <param name="iEndpointIndex"></param>
        public void RemoveEndpoint(int iEndpointIndex)
        {
            this.endpointUnitList.RemoveAt(iEndpointIndex);

            if (this.endpointUnitList.Count == 1)
            {
                this.endpointUnitList.Clear();
            }
        }

        /// <summary>
        /// 移除指定的线段。
        /// </summary>
        /// <param name="iSegmentIndex"></param>
        public void RemoveSegment(int iSegmentIndex)
        {
            try
            {
                int iInitialPointIndex = iSegmentIndex;
                int iTerminalPointIndex = iInitialPointIndex + 1;

                if (iInitialPointIndex == 0)
                {
                    if (iTerminalPointIndex == this.endpointUnitList.Count - 1)
                    {
                        this.endpointUnitList.RemoveRange(iInitialPointIndex, 2);
                    }
                    else
                    {
                        this.endpointUnitList.RemoveAt(iInitialPointIndex);
                    }
                }
                else // PointType.Middle
				{
                    // iInitialPointIndex is a middle point.

                    if (iTerminalPointIndex == this.endpointUnitList.Count - 1)
                    {
                        this.endpointUnitList.RemoveAt(iTerminalPointIndex);
                    }
                    else // PointType.Middle
					{
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
            }
        }                

        /// <summary>
        /// 用指定的方法更新所有端点。
        /// </summary>
        /// <param name="createEndpoint"></param>
        public void UpdateAllEndpoints(CreateEndpoint createEndpoint)
        {
            if (createEndpoint == null)
                throw new ArgumentNullException(nameof(createEndpoint));

            foreach (EndpointUnit endpointUnit in this.endpointUnitList)
            {
                endpointUnit.Endpoint = createEndpoint.Invoke(endpointUnit.Endpoint.LogicalPoint);
            }
        }
            
        public void Paint(Graphics graphics, PaintingToolSet paintTool)
        {
            Paint(graphics,
                    paintTool,
                    EpoxyPatternControl.INVALID_SEGMENT_INDEX,
                    EpoxyPatternControl.INVALID_ENDPOINT_INDEX,
                    null);
        }

        public void Paint(Graphics graphics, PaintingToolSet paintingTool, int iHighlightedSegmentIndex, int iHighlightedEndPointIndex, PaintingToolSet highlightPaintTool)
        {
            if (this.Visible && this.endpointUnitList.Count > 0)
            {
                int iTerminalPointIndex = EpoxyPatternControl.INVALID_ENDPOINT_INDEX;
                int iInitialPointIndex = EpoxyPatternControl.INVALID_ENDPOINT_INDEX;

                if (iHighlightedSegmentIndex != EpoxyPatternControl.INVALID_SEGMENT_INDEX)
                {
                    Debug.Assert(iHighlightedSegmentIndex >= 0);

                    if (iHighlightedSegmentIndex < this.endpointUnitList.Count - 1)
                    {
                        iInitialPointIndex = iHighlightedSegmentIndex;
                        iTerminalPointIndex = iInitialPointIndex + 1;
                    }
                }

                GraphicsState state = graphics.Save();
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // paints the original rectangle.
                graphics.DrawEllipse(paintingTool.Pen, this.endpointUnitList[0].Endpoint.RealRectangle);
                
                //
                // paints other rectangles and arrow lines between the rectangles.
                //
                for (int i = 1; i < this.endpointUnitList.Count; i++)
                {
                    Endpoint initialPoint = this.endpointUnitList[i - 1].Endpoint;
                    Endpoint terminalPoint = this.endpointUnitList[i].Endpoint;
                    EndpointSetting initialPointInfo = this.endpointUnitList[i - 1].EndpointInfo;
                    int circlepointindex = i - 1;
                    RectangleF rect = terminalPoint.RealRectangle;
                    
                    if (!float.IsNaN(rect.X) && !float.IsNaN(rect.Y))
                        graphics.DrawRectangle(paintingTool.Pen, rect.X, rect.Y, rect.Width, rect.Height);

                    if (i != iTerminalPointIndex)
                    {
                        Pen pen = paintingTool.Pen;
                        if (initialPointInfo.Dummied)
                            pen = paintingTool.DummiedPen;
                        
                        Utility.DrawVector(graphics, initialPoint.RealPoint, terminalPoint.RealPoint, pen);
                    }
                }

                if (iTerminalPointIndex != EpoxyPatternControl.INVALID_SEGMENT_INDEX)
                {
                    Endpoint initialPoint = this.endpointUnitList[iInitialPointIndex].Endpoint;
                    Endpoint terminalPoint = this.endpointUnitList[iTerminalPointIndex].Endpoint;
                    EndpointSetting initialPointInfo = this.endpointUnitList[iInitialPointIndex].EndpointInfo;

                    Brush brush = highlightPaintTool.Brush;
                    Pen pen = highlightPaintTool.Pen;
                    if (initialPointInfo.Dummied)
                        pen = highlightPaintTool.DummiedPen;

                    //Utility.FillArrowLineSegment(graphics, initialPoint.RealPoint, terminalPoint.RealPoint, pen, brush);

                    Utility.FillVector(graphics, initialPoint.RealPoint, terminalPoint.RealPoint, pen, brush);
                }

                if (iHighlightedEndPointIndex != EpoxyPatternControl.INVALID_ENDPOINT_INDEX)
                {
                    Endpoint endPoint = this.endpointUnitList[iHighlightedEndPointIndex].Endpoint;

                    if (iHighlightedEndPointIndex == 0)
                        graphics.FillEllipse(highlightPaintTool.Brush, endPoint.RealRectangle);
                    else
                        graphics.FillRectangle(highlightPaintTool.Brush, endPoint.RealRectangle);
                }

                graphics.Restore(state);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="iSegmentIndex"></param>
        /// <param name="midEndpoint"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        public void PaintSplitLineSegments(Graphics graphics, int iSegmentIndex, Endpoint midEndpoint, Pen pen, Brush brush)
        {
            int iInitialPointIndex = iSegmentIndex;
            int iTerminalPointIndex = iSegmentIndex + 1;

            Endpoint initialEndpoint = this.endpointUnitList[iInitialPointIndex].Endpoint;
            Endpoint terminalEndpoint = this.endpointUnitList[iTerminalPointIndex].Endpoint;

            GraphicsState state = graphics.Save();

            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Utility.FillVector(graphics, initialEndpoint.RealPoint, midEndpoint.RealPoint, pen, brush);
            Utility.FillVector(graphics, midEndpoint.RealPoint, terminalEndpoint.RealPoint, pen, brush);

            //double distance = Utility.GetPointsDistance(initialEndpoint.RealPoint, midEndpoint.RealPoint);
            //Font font = new Font("宋体", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            //graphics.DrawString(distance.ToString(), font, Brushes.Red, (PointF)midEndpoint.RealPoint);//新增显示线长度

            RectangleF rect = midEndpoint.RealRectangle;
            graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

            graphics.Restore(state);
        }

        public void HighlightSegment(Graphics g, int iSegmentIndex, PaintingToolSet paintTool)
        {
            int iInitialPointIndex = iSegmentIndex;
            int iTerminalPointIndex = iSegmentIndex + 1;

            Endpoint initialPoint = this.endpointUnitList[iInitialPointIndex].Endpoint;
            Endpoint terminalPoint = this.endpointUnitList[iTerminalPointIndex].Endpoint;
            EndpointSetting info = this.endpointUnitList[iInitialPointIndex].EndpointInfo;

            Brush brush = paintTool.Brush;
            Pen pen = paintTool.Pen;
            if (info.Dummied)
                pen = paintTool.DummiedPen;

            GraphicsState state = g.Save();
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Utility.FillVector(g, initialPoint.RealPoint, terminalPoint.RealPoint, pen, brush);

            g.Restore(state);
        }

        /// <summary>
        /// 高亮指定的端点。
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="iEndpointIndex"></param>
        /// <param name="brush"></param>
        public void HighlightEndpoint(Graphics graphics, int iEndpointIndex, Brush brush)
        {
            Endpoint endpoint = this.endpointUnitList[iEndpointIndex].Endpoint;
            if (iEndpointIndex == 0)
            {
                graphics.FillEllipse(brush, endpoint.RealRectangle);
            }
            else
            {
                graphics.FillRectangle(brush, endpoint.RealRectangle);
            }
        }



        /// <summary>
        /// 判断曲线是否包含指定的端点。
        /// </summary>
        /// <param name="logicalPoint"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="endpoint"></param>
        /// <param name="iEndpointIndex"></param>
        /// <returns></returns>
        public bool ContainsEndpoint(Point2D logicalPoint, float xOffset, float yOffset, out Endpoint endpoint, out int iEndpointIndex)
        {
            endpoint = new Endpoint();

            RectangleF rect = new RectangleF(0, 0, xOffset * 2, yOffset * 2);

            bool bResult = false;
            iEndpointIndex = 0;

            foreach (EndpointUnit endpointUnit in this.endpointUnitList)
            {
                Point2D point = endpointUnit.Endpoint.LogicalPoint;

                rect.X = point.X - xOffset;
                rect.Y = point.Y - yOffset;

                if (rect.Contains((PointF)logicalPoint))
                {
                    endpoint = endpointUnit.Endpoint;
                    bResult = true;
                    break;
                }

                iEndpointIndex++;
            }

            return bResult;
        }

        /// <summary>
        /// 判断曲线是否包含指定的线段。
        /// </summary>
        /// <param name="realPoint">鼠标点</param>
        /// <param name="distance">距离最大范围，超过这个范围则不算在某个线段上</param>
        /// <param name="iSegmentInitialEndpointIndex">返回线段起点。</param>
        /// <returns></returns>
        public bool ContainsSegment(Point2D realPoint, double distance, out int iSegmentInitialEndpointIndex)
        {            
            for (int i = 1; i < this.endpointUnitList.Count; i++)
            {
                Point2D segmentInitialPoint = this.endpointUnitList[i - 1].Endpoint.RealPoint;
                Point2D segmentTerminalPoint = this.endpointUnitList[i].Endpoint.RealPoint;
                double minimumDistance = Utility.GetMinimumDistanceBetweenPointAndSegment(segmentInitialPoint, segmentTerminalPoint, realPoint);

                if (minimumDistance <= distance)
                {
                    iSegmentInitialEndpointIndex = i - 1;
                    return true;
                }
            }

            iSegmentInitialEndpointIndex = EpoxyPatternControl.INVALID_ENDPOINT_INDEX;
            return false;
        }


    }
}
