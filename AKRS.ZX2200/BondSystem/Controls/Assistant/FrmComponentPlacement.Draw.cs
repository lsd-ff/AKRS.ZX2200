using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.Services;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    /// <summary>
    ///  贴片示教窗体，绘制部分
    /// </summary>
    public partial class FrmComponentPlacement
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
        /// 焊点高度颜色
        /// </summary>
        private Color bondHeightColor = Color.GreenYellow;

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

        /// <summary>
        /// 吹气折线点
        /// </summary>
        private List<Point> blowlinePoints;

        /// <summary>
        /// 绘制图例
        /// </summary>
        /// <param name="g">画布</param>
        private void DrawLegend(Graphics g)
        {
            // 创建图例项
            List<LegendItem> legendItems = new List<LegendItem>
                              {
                                  new LegendItem { Color = this.bondHeadMovementeColor, Text = "焊头移动路径" },
                                  new LegendItem { Color = this.vacuumColor, Text = "真空" },
                                  new LegendItem { Color = this.blowColor, Text = "吹气" },
                                  new LegendItem { Color = this.arrowLineColor, Text = "测量" },
                                  new LegendItem { Color = this.bondHeightColor, Text = "焊点高度" },
                              };

            // 单个图例的高度
            int legendItemHeight = 15;

            // 留边
            int legendPadding = 6;

            // 计算图例位置（右上角）
            int legendX = gCManual.Location.X - 120;
            int legendY = gCManual.Location.Y + 20;

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
        private void tablePanel1_Paint(object sender, PaintEventArgs e)
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

                // 绘制带箭头的标示线段
                using (Pen arrowPen = new Pen(this.arrowLineColor, this.arrowLineWidth))
                {
                    // 绘制线段主体
                    g.DrawLine(arrowPen, arrowStart, arrowEnd);

                    // 在起点绘制箭头
                    DrawArrow(g, arrowStart, arrowEnd, this.arrowLineColor);

                    // 在终点绘制箭头（方向相反）
                    DrawArrow(g, arrowEnd, arrowStart, this.arrowLineColor);
                }

                // 绘制长度标签
                switch (i)
                {
                    case 1:

                        this.DrawLengthLabel(
                            g,
                            $"D",
                            new Point(midPoint.X, midPoint.Y),
                            Color.Black,
                            Color.LightYellow);
                        break;

                    case 2:
                        this.DrawLengthLabel(
                            g,
                            $"A",
                            new Point(midPoint.X, midPoint.Y),
                            Color.Black,
                            Color.LightYellow);

                        break;

                    case 3:
                        this.DrawLengthLabel(
                            g,
                            $"E",
                            new Point(midPoint.X, midPoint.Y),
                            Color.Black,
                            Color.LightYellow);
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

            #endregion

            #region 焊点高度

            Point bpHeightLineStartPos = new Point(this.bondHeadMovePoints[0].X, this.bondHeadMovePoints[3].Y - 2);
            Point bpHeightLineEndPos = new Point(this.bondHeadMovePoints[4].X, this.bondHeadMovePoints[3].Y - 2);

            // 绘制焊点高度虚线
            using (Pen dashPen = new Pen(this.bondHeightColor, 1))
            {
                dashPen.DashStyle = DashStyle.Custom;

                // 定义自定义模式：像素实线 + 像素空白 + 像素实线 + 像素空白
                dashPen.DashPattern = new float[] { 5, 8, 5, 8 };

                g.DrawLine(dashPen, bpHeightLineStartPos, bpHeightLineEndPos);
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

            // 真空时间箭头绘制
            {
                // 计算箭头线段位置
                Point arrowStart = this.vacuumlinePoints[1];
                Point arrowEnd = new Point(this.bondHeadMovePoints[3].X, this.vacuumlinePoints[1].Y);

                // 计算箭头线段中点
                Point midPoint = new Point(
                    (arrowStart.X + arrowEnd.X) / 2,
                    (arrowStart.Y + arrowEnd.Y) / 2);

                // 绘制带箭头的标示线段
                using (Pen arrowPen = new Pen(this.arrowLineColor, this.arrowLineWidth))
                {
                    // 绘制线段主体
                    g.DrawLine(arrowPen, arrowStart, arrowEnd);

                    // 在起点绘制箭头
                    DrawArrow(g, arrowStart, arrowEnd, this.arrowLineColor);

                    // 在终点绘制箭头（方向相反）
                    DrawArrow(g, arrowEnd, arrowStart, this.arrowLineColor);
                }

                // 绘制长度标签
                this.DrawLengthLabel(g, $"B", new Point(midPoint.X, midPoint.Y), this.vacuumColor, Color.LightYellow);

                // 绘制连接线（从折线段到标示线段）
                using (Pen connectorPen = new Pen(this.connectorPenColor, 1))
                {
                    connectorPen.DashStyle = DashStyle.Custom;

                    // 定义自定义模式：像素实线 + 像素空白 + 像素实线 + 像素空白
                    connectorPen.DashPattern = new float[] { 5, 8, 5, 8 };
                    g.DrawLine(connectorPen, arrowEnd, new Point(this.bondHeadMovePoints[3].X, this.vacuumlinePoints[3].Y));
                }
            }

            #endregion


            #region 吹气

            // 吹气折线点
            this.blowlinePoints = this.GetBlowPoints();

            // 绘制吹气折线
            using (Pen connectorPen = new Pen(this.blowColor, this.polylineWidth))
            {
                g.DrawLines(connectorPen, this.blowlinePoints.ToArray());
            }

            {
                // 吹气时间箭头绘制
                Point arrowStart = this.blowlinePoints[1];
                Point arrowEnd = this.blowlinePoints.Count == 6
                                     ? this.blowlinePoints[4]
                                     : new Point(this.bondHeadMovePoints[5].X, this.blowlinePoints[1].Y);

                // 计算箭头线段中点
                Point midPoint = new Point(
                    (arrowStart.X + arrowEnd.X) / 2,
                    (arrowStart.Y + arrowEnd.Y) / 2);

                // 绘制带箭头的标示线段
                using (Pen arrowPen = new Pen(this.arrowLineColor, this.arrowLineWidth))
                {
                    // 绘制线段主体
                    g.DrawLine(arrowPen, arrowStart, arrowEnd);

                    // 在起点绘制箭头
                    this.DrawArrow(g, arrowStart, arrowEnd, this.arrowLineColor);

                    if (this.blowlinePoints.Count == 6)
                    {
                        // 在终点绘制箭头（方向相反）
                        this.DrawArrow(g, arrowEnd, arrowStart, this.arrowLineColor);
                    }
                }

                // 绘制长度标签
                this.DrawLengthLabel(g, $"C", new Point(midPoint.X, midPoint.Y), this.blowColor, Color.LightYellow);
            }

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
            int leftLimit = groupControl3.Location.X + groupControl3.Size.Width + 3;

            // 右限
            int rightLimit = this.gCManual.Location.X - 3;

            // 上限
            int upperLimit = groupControl3.Location.Y + groupControl3.Size.Height / 2 + 30;

            // 下限
            int lowerLimit = upperLimit + 90;

            // 二段速时间
            // +0.1是为了防止除以0
            double slowdownTimeBeforeBonding = this.component.IsActivateSlowTravelBeforeBonding
                                                   ? this.component.SlowTravelDistanceBeforeBonding
                                                     / (this.component.SlowTravelSpeedBeforeBonding + 0.1) * 1000
                                                   : 0;

            // 二段速时间
            // +0.1是为了防止除以0
            double slowdownTimeAfterBonding = this.component.IsActivateSlowTravelAfterBonding
                                                  ? this.component.SlowTravelDistanceAfterBonding
                                                    / (this.component.SlowTravelSpeedAfterBonding + 0.1) * 1000
                                                  : 0;

            // 高速段时间（ms）
            double fastDownTime = 0.5;

            double totalTime = slowdownTimeBeforeBonding + this.component.PlacementDelay + slowdownTimeAfterBonding
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
                this.component.IsActivateSlowTravelBeforeBonding ? point1.Y + 30 : lowerLimit);

            // 第5点
            Point point5 = new Point(
                rightLimit - lateralOffset,
                this.component.IsActivateSlowTravelAfterBonding ? point6.Y + 30 : lowerLimit);

            // 第3点
            Point point3 = new Point(
                point2.X + Convert.ToInt32(length * slowdownTimeBeforeBonding / totalTime),
                lowerLimit);

            // 第4点
            Point point4 = new Point(
                point5.X - Convert.ToInt32(length * slowdownTimeAfterBonding / totalTime),
                lowerLimit);

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
            int totalLength = this.bondHeadMovePoints[3].X - this.bondHeadMovePoints[2].X;

            // 真空折线点列表
            List<Point> points = new List<Point> { };
            points.Add(new Point(this.bondHeadMovePoints[0].X, this.bondHeadMovePoints[3].Y + offset));
            points.Add(
                new Point(
                    this.bondHeadMovePoints[2].X + totalLength
                    * (this.component.PlacementDelay - this.component.VacuumOffDelay)
                    / (this.component.PlacementDelay == 0 ? 1 : this.component.PlacementDelay),
                    points[0].Y));

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
            points.Add(new Point(this.vacuumlinePoints[0].X, this.vacuumlinePoints[0].Y + offset));
            points.Add(new Point(this.vacuumlinePoints[1].X, this.vacuumlinePoints[1].Y + offset));
            points.Add(new Point(this.vacuumlinePoints[2].X, points[1].Y - 30));

            int totalLength = this.bondHeadMovePoints[4].X - this.bondHeadMovePoints[2].X;

            // 二段速时间
            // +0.1是为了防止除以0
            double slowdownTimeAfterPick = this.component.IsActivateSlowTravelAfterBonding
                                               ? this.component.SlowTravelDistanceAfterBonding
                                                 / (this.component.SlowTravelSpeedAfterBonding + 0.1) * 1000
                                               : 0;

            points.Add(
                new Point(
                    this.vacuumlinePoints[2].X + Convert.ToInt32(
                        totalLength * this.component.BondingBlowDelay
                        / (this.component.PlacementDelay + slowdownTimeAfterPick)),
                    points[2].Y));

            if (this.vacuumlinePoints[3].X >= points[3].X)
            {
                points.Add(new Point(
                    points[3].X,
                    this.vacuumlinePoints[0].Y + offset));

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
        /// <param name="bgColor">背景颜色</param>
        private void DrawLengthLabel(Graphics g, string text, Point position, Color textColor, Color bgColor)
        {
            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;

                // 绘制文本
                using (Brush textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(text, labelFont, textBrush, position, sf);
                }
            }
        }
    }

    /// <summary>
    /// 图例项类
    /// </summary>
    public class LegendItem
    {
        /// <summary>
        /// 颜色
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }
    }
}
