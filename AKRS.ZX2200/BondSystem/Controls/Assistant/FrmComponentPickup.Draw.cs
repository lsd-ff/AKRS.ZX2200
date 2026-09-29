using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    /// <summary>
    ///  贴片示教窗体，绘制部分
    /// </summary>
    public partial class FrmComponentPickup
    {
        /// <summary>
        /// 焊头移动路径颜色
        /// </summary>
        private Color bondHeadMovementeColor = Color.Black;

        /// <summary>
        /// 箭头颜色
        /// </summary>
        private Color arrowLineColor = Color.SandyBrown;

        /// <summary>
        /// 吹气颜色
        /// </summary>
        private Color blowColor = Color.Blue;

        /// <summary>
        /// 真空颜色
        /// </summary>
        private Color vacuumColor = Color.Red;

        /// <summary>
        /// 芯片高度颜色
        /// </summary>
        private Color componentHeightColor = Color.GreenYellow;

        /// <summary>
        /// 连接线颜色
        /// </summary>
        private Color connectorPenColor = Color.SandyBrown;

        /// <summary>
        /// 折线画笔的宽度
        /// </summary>
        private int polylineWidth = 3;

        /// <summary>
        /// 箭头画笔的宽度
        /// </summary>
        private int arrowLineWidth = 1;

        /// <summary>
        /// 字体
        /// </summary>
        private Font labelFont = new Font("Arial", 6, FontStyle.Regular);

        /// <summary>
        /// 焊头移动路径折线点
        /// </summary>
        private List<Point> bondHeadMovePoints;

        /// <summary>
        /// 真空折线点
        /// </summary>
        private List<Point> vacuumlinePoints;

        ///// <summary>
        ///// 吹气折线点
        ///// </summary>
        //private List<Point> blowlinePoints;

        /// <summary>
        /// 绘制图例
        /// </summary>
        /// <param name="g">画布</param>
        private void DrawLegend(Graphics g)
        {
            // 创建图例项
            List<LegendItem> legendItems = new List<LegendItem>
                              {
                                  new LegendItem { Color = bondHeadMovementeColor, Text = "焊头移动路径" },
                                  new LegendItem { Color = vacuumColor, Text = "真空" },
                                  //new LegendItem { Color = blowColor, Text = "吹气" },
                                  new LegendItem { Color = arrowLineColor, Text = "测量" },
                                  new LegendItem { Color = this.componentHeightColor, Text = "芯片高度" },
                              };

            // 单个图例的高度
            int legendItemHeight = 15;

            // 留边
            int legendPadding = 6;

            // 计算图例位置（右上角）
            int legendX = this.gCEjection.Location.X + this.gCEjection.Size.Width - 120;
            int legendY = this.gCEjection.Location.Y + this.gCEjection.Size.Height + 10;

            // 绘制图例项
            int itemY = legendY + legendPadding;

            foreach (LegendItem item in legendItems)
            {
                // 绘制颜色矩形
                Rectangle colorRect = new Rectangle(
                    legendX + legendPadding,
                    itemY,
                    15,
                    10);

                using (SolidBrush colorBrush = new SolidBrush(item.Color))
                {
                    g.FillRectangle(colorBrush, colorRect);
                }

                g.DrawRectangle(Pens.Black, colorRect);

                // 绘制文本
                g.DrawString(item.Text, this.labelFont, Brushes.Black, legendX + legendPadding + 30, itemY);

                itemY += legendItemHeight;
            }
        }

        /// <summary>
        /// 绘制事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void PnlNozzle_Paint(object sender, PaintEventArgs e)
        {
            // 先调用基类绘制
            base.OnPaint(e);

            Graphics g = e.Graphics;
            DoubleBuffered = true;
            g.SmoothingMode = SmoothingMode.AntiAlias; // 抗锯齿

            #region 焊头移动路径

            // 折线点列表
            this.bondHeadMovePoints = this.GetBondHeadMovePoints();

            // 绘制焊头移动路径
            using (Pen polylinePen = new Pen(this.bondHeadMovementeColor, this.polylineWidth))
            {
                // 绘制连接线
                if (this.bondHeadMovePoints.Count > 1)
                {
                    g.DrawLines(polylinePen, this.bondHeadMovePoints.ToArray());
                }
            }

            // 为每一段折线绘制箭头标示
            for (int i = 1; i < this.bondHeadMovePoints.Count - 2; i++)
            {
                Point startPoint = this.bondHeadMovePoints[i];
                Point endPoint = this.bondHeadMovePoints[i + 1];

                // Y向偏移
                int offset1 = -100;

                // 计算箭头线段位置
                Point arrowStart = new Point(startPoint.X, Math.Max(endPoint.Y, startPoint.Y) + offset1);
                Point arrowEnd = new Point(endPoint.X, Math.Max(endPoint.Y, startPoint.Y) + offset1);

                // 计算箭头线段中点
                Point midPoint = new Point(
                    (arrowStart.X + arrowEnd.X) / 2,
                    (arrowStart.Y + arrowEnd.Y) / 2);


                // 晶圆芯片要增加顶针的箭头
                if (this.component is CarrierWithWaferConfig carrierWithWafer && i == 2)
                {
                    double length = GeometryService.LateralDistance(
                        this.bondHeadMovePoints[2],
                        this.bondHeadMovePoints[3]);

                    // 顶针时间
                    double ejectTime = carrierWithWafer.RelativeHeightWithEjectionReadyLiftPosition
                                       / carrierWithWafer.EjectLiftSpeed * 1000;

                    Point arrowMiddle = new Point(
                        startPoint.X + Convert.ToInt32(length * ejectTime / (ejectTime + this.component.PickupDelay)),
                        arrowStart.Y);

                    Point middlePoint = new Point(
                        startPoint.X + Convert.ToInt32(length * ejectTime / (ejectTime + this.component.PickupDelay)),
                        startPoint.Y);

                    // 绘制带箭头的标示线段
                    using (Pen arrowPen = new Pen(arrowLineColor, arrowLineWidth))
                    {
                        // 绘制线段主体
                        g.DrawLine(arrowPen, arrowStart, arrowMiddle);
                        g.DrawLine(arrowPen, arrowMiddle, arrowEnd);

                        // 在起点绘制箭头
                        this.DrawArrow(g, arrowStart, arrowMiddle, arrowLineColor);
                        this.DrawArrow(g, arrowMiddle, arrowEnd, arrowLineColor);

                        // 在终点绘制箭头（方向相反）
                        this.DrawArrow(g, arrowMiddle, arrowStart, arrowLineColor);
                        this.DrawArrow(g, arrowEnd, arrowMiddle, arrowLineColor);
                    }

                    // 计算箭头线段中点
                    Point midPoint1 = new Point(
                        (arrowStart.X + arrowMiddle.X) / 2,
                        (arrowStart.Y + arrowMiddle.Y) / 2);
                    Point midPoint2 = new Point(
                        (arrowMiddle.X + arrowEnd.X) / 2,
                        (arrowMiddle.Y + arrowEnd.Y) / 2);


                    // 绘制长度标签
                    this.DrawLengthLabel(
                        g,
                        $"B",
                        new Point(midPoint1.X, midPoint1.Y),
                        Color.Black);
                    this.DrawLengthLabel(
                        g,
                        $"A",
                        new Point(midPoint2.X, midPoint2.Y),
                        Color.Black);

                    // 绘制连接线（从折线段到标示线段）
                    using (Pen connectorPen = new Pen(connectorPenColor, 1))
                    {
                        connectorPen.DashStyle = DashStyle.Custom;

                        // 定义自定义模式：像素实线 + 像素空白 + 像素实线 + 像素空白
                        connectorPen.DashPattern = new float[] { 5, 8, 5, 8 };
                        g.DrawLine(connectorPen, startPoint, arrowStart);
                        g.DrawLine(connectorPen, middlePoint, arrowMiddle);
                        g.DrawLine(connectorPen, endPoint, arrowEnd);
                    }
                }
                else
                {
                    // 绘制带箭头的标示线段
                    using (Pen arrowPen = new Pen(arrowLineColor, arrowLineWidth))
                    {
                        // 绘制线段主体
                        g.DrawLine(arrowPen, arrowStart, arrowEnd);

                        // 在起点绘制箭头
                        this.DrawArrow(g, arrowStart, arrowEnd, arrowLineColor);

                        // 在终点绘制箭头（方向相反）
                        this.DrawArrow(g, arrowEnd, arrowStart, arrowLineColor);
                    }

                    // 绘制长度标签
                    switch (i)
                    {
                        case 1:
                            this.DrawLengthLabel(
                                g,
                                $"D",
                                new Point(midPoint.X, midPoint.Y),
                                Color.Black);
                            break;

                        case 2:
                            this.DrawLengthLabel(
                                g,
                                $"A",
                                new Point(midPoint.X, midPoint.Y),
                                Color.Black);

                            break;

                        case 3:
                            this.DrawLengthLabel(
                                g,
                                $"E",
                                new Point(midPoint.X, midPoint.Y),
                                Color.Black);
                            break;
                    }

                    // 绘制连接线（从折线段到标示线段）
                    using (Pen connectorPen = new Pen(this.connectorPenColor, 1))
                    {
                        connectorPen.DashStyle = DashStyle.Custom;

                        // 定义自定义模式：像素实线 + 像素空白 + 像素实线 + 像素空白
                        connectorPen.DashPattern = new float[] { 5, 8, 5, 8 };
                        g.DrawLine(connectorPen, startPoint, arrowStart);
                        g.DrawLine(connectorPen, endPoint, arrowEnd);
                    }
                }
            }

            #endregion

            #region 芯片高度

            Point componentHeightLineStartPos = new Point(this.bondHeadMovePoints[0].X, this.bondHeadMovePoints[3].Y - 2);
            Point componentHeightLineEndPos = new Point(this.bondHeadMovePoints[4].X, this.bondHeadMovePoints[3].Y - 2);

            // 绘制焊点高度虚线
            using (Pen dashPen = new Pen(this.componentHeightColor, 1))
            {
                dashPen.DashStyle = DashStyle.Custom;

                // 定义自定义模式：像素实线 + 像素空白 + 像素实线 + 像素空白
                dashPen.DashPattern = new float[] { 5, 8, 5, 8 };

                g.DrawLine(dashPen, componentHeightLineStartPos, componentHeightLineEndPos);
            }

            #endregion

            #region 真空

            // 真空折线点列表
            this.vacuumlinePoints = this.GetVacuumPoints();

            // 绘制真空折线
            using (Pen connectorPen = new Pen(this.vacuumColor, this.polylineWidth))
            {
                g.DrawLines(connectorPen, this.vacuumlinePoints.ToArray());
            }

            #endregion


            #region 吹气

            //// 吹气折线点
            //blowlinePoints = this.GetBlowPoints();

            //// 绘制吹气折线
            //using (Pen connectorPen = new Pen(this.blowColor, this.polylineWidth))
            //{
            //    g.DrawLines(connectorPen, blowlinePoints.ToArray());
            //}

            //{
            //    // 吹气时间箭头绘制
            //    Point arrowStart = blowlinePoints[1];
            //    Point arrowEnd = this.blowlinePoints.Count == 6
            //                         ? blowlinePoints[4]
            //                         : new Point(bondHeadMovePoints[5].X, this.blowlinePoints[1].Y);

            //    // 计算箭头线段中点
            //    Point midPoint = new Point(
            //        (arrowStart.X + arrowEnd.X) / 2,
            //        (arrowStart.Y + arrowEnd.Y) / 2);

            //    // 绘制带箭头的标示线段
            //    using (Pen arrowPen = new Pen(arrowLineColor, arrowLineWidth))
            //    {
            //        // 绘制线段主体
            //        g.DrawLine(arrowPen, arrowStart, arrowEnd);

            //        // 在起点绘制箭头
            //        DrawArrow(g, arrowStart, arrowEnd, arrowLineColor);

            //        if (this.blowlinePoints.Count == 6)
            //        {
            //            // 在终点绘制箭头（方向相反）
            //            DrawArrow(g, arrowEnd, arrowStart, arrowLineColor);
            //        }
            //    }

            //    // 绘制长度标签
            //    this.DrawLengthLabel(g, $"C", new Point(midPoint.X, midPoint.Y), this.blowColor, Color.LightYellow);
            //}

            #endregion

            // 绘制图例
            this.DrawLegend(g);
        }

        /// <summary>
        /// 获取焊头移动的折线点
        /// </summary>
        /// <returns>点集</returns>
        private List<Point> GetBondHeadMovePoints()
        {
            // 左限
            int leftLimit = this.gCSlowTravelBeforePick.Location.X + this.gCSlowTravelBeforePick.Size.Width + 3;

            // 右限
            int rightLimit = this.gCEjection.Location.X + this.gCEjection.Size.Width - 3;

            // 上限
            int upperLimit = this.gCSlowTravelBeforePick.Location.Y + this.gCSlowTravelBeforePick.Size.Height / 2 + 30;

            // 下限
            int lowerLimit = upperLimit + 90;

            // 二段速时间（ms）
            // +0.1是为了防止除以0
            double slowdownTimeBeforePick = this.component.IsActivateSlowTravelBeforePickup
                                                ? this.component.SlowTravelDistanceBeforePickup
                                                  / (this.component.SlowTravelSpeedBeforePickup + 0.1) * 1000
                                                : 0;

            // 二段速时间（ms）
            // +0.1是为了防止除以0
            double slowdownTimeAfterPick = this.component.IsActivateSlowTravelAfterPickup
                                               ? this.component.SlowTravelDistanceAfterPickup
                                                 / (this.component.SlowTravelSpeedAfterPickup + 0.1) * 1000
                                               : 0;

            // 顶针顶起时间（ms）
            double ejectTime = this.component is CarrierWithWaferConfig carrierWithWafer
                                   ? carrierWithWafer.RelativeHeightWithEjectionReadyLiftPosition
                                     / carrierWithWafer.EjectLiftSpeed * 1000
                                   : 0;

            // 高速段时间（ms）
            double fastDownTime = 0.5;

            double totalTime = slowdownTimeBeforePick + ejectTime + this.component.PickupDelay + slowdownTimeAfterPick
                               + fastDownTime * 2;

            // 第1点
            Point point1 = new Point(leftLimit, upperLimit);

            // 第6点
            Point point6 = new Point(rightLimit, upperLimit);

            // 第1点和第6点之间的横向距离
            double length = GeometryService.LateralDistance(point1, point6);

            // 横向偏移
            int lateralOffset = Convert.ToInt32(length * fastDownTime / totalTime);

            // 第2点
            Point point2 = new Point(
                leftLimit + lateralOffset,
                this.component.IsActivateSlowTravelBeforePickup ? point6.Y + 30 : lowerLimit);

            // 第5点
            Point point5 = new Point(
                rightLimit - lateralOffset,
                this.component.IsActivateSlowTravelAfterPickup ? point6.Y + 30 : lowerLimit);

            // 第3点
            Point point3 = new Point(
                point2.X + Convert.ToInt32(length * slowdownTimeBeforePick / totalTime),
                lowerLimit);

            // 第4点
            Point point4 = new Point(point5.X - Convert.ToInt32(length * slowdownTimeAfterPick / totalTime), lowerLimit);

            // 折线点列表,按顺序添加
            List<Point> points = new List<Point> { };
            points.Add(point1);
            points.Add(point2);
            points.Add(point3);
            points.Add(point4);
            points.Add(point5);
            points.Add(point6);

            return points;
        }

        /// <summary>
        /// 获取真空的折线点
        /// </summary>
        /// <returns>点集</returns>
        private List<Point> GetVacuumPoints()
        {
            int offset = 45;

            // 真空折线点列表
            List<Point> points = new List<Point> { };
            points.Add(new Point(this.bondHeadMovePoints[0].X, this.bondHeadMovePoints[3].Y + offset));
            points.Add(new Point(this.bondHeadMovePoints[2].X, points[0].Y));
            points.Add(new Point(points[1].X, points[1].Y + 30));
            points.Add(new Point(this.bondHeadMovePoints[4].X, points[2].Y));

            return points;
        }

        /// <summary>
        /// 获取吹气的折线点
        /// </summary>
        /// <returns>点集</returns>
        private List<Point> GetBlowPoints()
        {
            // 吹气折线点
            List<Point> points = new List<Point>();
            int offset = 90;
            points.Add(new Point(vacuumlinePoints[0].X, vacuumlinePoints[0].Y + offset));
            points.Add(new Point(vacuumlinePoints[1].X, vacuumlinePoints[1].Y + offset));
            points.Add(new Point(vacuumlinePoints[2].X, points[1].Y - 30));

            int totalLength = this.bondHeadMovePoints[4].X - this.bondHeadMovePoints[2].X;

            // 二段速时间
            // +0.1是为了防止除以0
            double slowdownTimeAfterPick = this.component.IsActivateSlowTravelAfterBonding
                                               ? this.component.SlowTravelDistanceAfterBonding
                                                 / (this.component.SlowTravelSpeedAfterBonding + 0.1) * 1000
                                               : 0;

            points.Add(
                new Point(
                    vacuumlinePoints[2].X + Convert.ToInt32(totalLength * this.component.BondingBlowDelay
                                                           / (this.component.PlacementDelay + slowdownTimeAfterPick)),
                    points[2].Y));

            if (vacuumlinePoints[3].X >= points[3].X)
            {
                points.Add(new Point(
                    points[3].X,
                    vacuumlinePoints[0].Y + offset));

                points.Add(
                    new Point(
                        this.vacuumlinePoints[3].X,
                        points[4].Y));
            }

            return points;
        }

        /// <summary>
        /// 在指定位置绘制箭头
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="from">起始点</param>
        /// <param name="to">终止点</param>
        /// <param name="color">颜色</param>
        private void DrawArrow(Graphics g, Point from, Point to, Color color)
        {
            // 箭头尺寸
            int arrowSize = 5;

            // 箭头角度（度）
            int arrowAngle = 30;

            // 计算线段方向向量
            double dx = to.X - from.X;
            double dy = to.Y - from.Y;

            // 计算线段长度和单位向量
            double length = Math.Sqrt(dx * dx + dy * dy);

            // 避免除零错误
            if (length < 0.1) return;

            double ux = dx / length;
            double uy = dy / length;

            // 计算箭头两侧点的方向向量（旋转角度）
            double angle = arrowAngle * Math.PI / 180.0; // 角度转弧度

            // 计算箭头左侧点
            double leftX = ux * Math.Cos(angle) - uy * Math.Sin(angle);
            double leftY = ux * Math.Sin(angle) + uy * Math.Cos(angle);

            // 计算箭头右侧点
            double rightX = ux * Math.Cos(-angle) - uy * Math.Sin(-angle);
            double rightY = ux * Math.Sin(-angle) + uy * Math.Cos(-angle);

            // 计算箭头点位置
            Point arrowLeft = new Point(
                (int)(from.X + leftX * arrowSize),
                (int)(from.Y + leftY * arrowSize));

            Point arrowRight = new Point(
                (int)(from.X + rightX * arrowSize),
                (int)(from.Y + rightY * arrowSize));

            // 绘制箭头
            using (Pen arrowPen = new Pen(color, arrowLineWidth))
            {
                arrowPen.EndCap = LineCap.Round;
                arrowPen.StartCap = LineCap.Round;

                g.DrawLine(arrowPen, from, arrowLeft);
                g.DrawLine(arrowPen, from, arrowRight);
            }
        }

        /// <summary>
        /// 绘制长度标签
        /// </summary>
        /// <param name="g">画布</param>
        /// <param name="text">文本</param>
        /// <param name="position">位置</param>
        /// <param name="textColor">文本颜色</param>
        private void DrawLengthLabel(Graphics g, string text, Point position, Color textColor)
        {
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                // 绘制文本
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(text, this.labelFont, textBrush, position, sf);
                }
            }
        }
    }
}
