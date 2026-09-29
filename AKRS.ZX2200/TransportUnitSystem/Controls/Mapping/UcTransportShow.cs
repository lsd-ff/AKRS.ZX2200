using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.CodeParser;
using DevExpress.XtraEditors;
using VMControls.WPF;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    using System.Diagnostics;
    using System.Drawing.Drawing2D;
    using System.Threading;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using DevExpress.Snap.Core.Native;
    using DevExpress.Utils.Extensions;
    using DevExpress.XtraBars;
    using LanguageExt.ClassInstances.Pred;
    using MathNet.Numerics;

    /// <summary>
    /// 绘图窗体
    /// </summary>
    public partial class UcTransportShow : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 传输载具显示
        /// </summary>
        /// <param name="transportUnit">传输载具实体</param>
        /// <param name="system">系统</param>
        public UcTransportShow(TransportUnit transportUnit, CurrentMachineSystemEnum system)
        {
            this.InitializeComponent();

            this.MouseWheel += this.UcTransportShow_MouseWheel;

            this.transportUnit = transportUnit;

            if (this.transportUnit != null)
            {
                if (this.transportUnit.TransportUnitInfo != null)
                {
                    this.transportUnit.TransportUnitInfo.IsProduct = true;
                }
            }

            this.system = system;
        }

        /// <summary>
        /// 传输载具显示
        /// </summary>
        /// <param name="transportUnit">传输载具实体</param>
        /// <param name="system">系统</param>
        public UcTransportShow(TransportUnit transportUnit)
        {
            this.InitializeComponent();

            this.MouseWheel += this.UcTransportShow_MouseWheel;

            this.transportUnit = transportUnit;

            this.popupMenu1.LinksPersistInfo.RemoveAt(2);

            this.IsRealTu = false;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

        /// <summary>
        /// 是不是实载具
        /// </summary>
        private bool IsRealTu = true;

        /// <summary>
        /// 当前系统
        /// </summary>
        private CurrentMachineSystemEnum system;

        /// <summary>
        /// 画布
        /// </summary>
        private Graphics graphics;

        /// <summary>
        /// 载具
        /// </summary>
        private TransportUnit transportUnit;

        /// <summary>
        /// 框架的列数
        /// </summary>
        private int SubRowCount => ProductDomain.GetInstance().ProductConfig.SubstrateConfig.RowCount;

        /// <summary>
        /// 框架的列数
        /// </summary>
        private int SubColumnCount => ProductDomain.GetInstance().ProductConfig.SubstrateConfig.ColumnCount;

        /// <summary>
        /// 框架的列数
        /// </summary>
        private int ModuleRowCount => ProductDomain.GetInstance().ProductConfig.ModuleConfig.RowCount;

        /// <summary>
        /// 框架的列数
        /// </summary>
        private int ModuleColumnCount => ProductDomain.GetInstance().ProductConfig.ModuleConfig.ColumnCount;

        /// <summary>
        /// 是否发生改变
        /// </summary>
        public bool IsChange { get; private set; } = false;

        /// <summary>
        /// 行间距
        /// </summary>
        private float subRowSpace;

        /// <summary>
        /// 列间距
        /// </summary>
        private float subColumnSpace;

        /// <summary>
        /// 框架到边缘的距离
        /// </summary>
        private readonly int frameworkDistanceToEdge = 40;

        /// <summary>
        /// 笔刷宽度
        /// </summary>
        private readonly int penWidth = 1;

        /// <summary>
        /// 框架绘制的开始点
        /// </summary>
        private PointF frameworkStartPointF;

        /// <summary>
        /// 框架绘制的开始点
        /// </summary>
        private PointF frameworkEndPointF;

        /// <summary>
        /// 鼠标起始点
        /// </summary>
        private PointF mouseStartPoint;

        /// <summary>
        /// 鼠标结束点
        /// </summary>
        private PointF mouseEndPoint;

        /// <summary>
        /// 缩放比例
        /// </summary>
        private float ratio = 1;

        /// <summary>
        /// 平移量
        /// </summary>
        private PointF translate;

        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Point mousePosition;

        /// <summary>
        /// 是否在正在选择
        /// </summary>
        private bool isSelecting = false;

        /// <summary>
        /// 绘制主框架
        /// </summary>
        private void PaintFramework()
        {
            int rectangleStartX = this.PlControl.Size.Width / 2;

            int rectangleStartY = this.PlControl.Size.Height / 2;

            int rectangleStartSizeX = this.PlControl.Size.Width - 2 * this.frameworkDistanceToEdge;

            int rectangleStartSizeY = this.PlControl.Size.Height - 2 * this.frameworkDistanceToEdge;

            this.DrawRectangle(
                new Pen(Color.BurlyWood, this.penWidth),
                rectangleStartX,
                rectangleStartY,
                rectangleStartSizeX,
                rectangleStartSizeY);

            this.frameworkStartPointF = new PointF(
                rectangleStartX - rectangleStartSizeX / 2,
                rectangleStartY - rectangleStartSizeY / 2);

            this.frameworkEndPointF = new PointF(
                rectangleStartX + rectangleStartSizeX / 2,
                rectangleStartY + rectangleStartSizeY / 2);

            this.subColumnSpace = (float)rectangleStartSizeX / this.SubColumnCount;

            this.subRowSpace = (float)rectangleStartSizeY / this.SubRowCount;
        }

        /// <summary>
        /// 选中的集合
        /// </summary>
        private readonly List<BaseMatter> selectedBaseMatters = new List<BaseMatter>();

        // 框架边缘大小
        int frameworkEdge = 2;

        /// <summary>
        /// 绘画实体位置
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="baseMatters">实体集合</param>
        /// <param name="leftRightPointF">开始点</param>
        /// <param name="rightDownPointF">结束点</param>
        /// <param name="column">列数</param>
        /// <param name="row">行数</param>
        /// <param name="color">颜色</param>
        private void PaintEntity<T>(List<T> baseMatters, PointF leftRightPointF, PointF rightDownPointF, int column, int row, Color color) where T : BaseMatter
        {
            double sizeRation = 0.7;

            float distanceX = (rightDownPointF.X - leftRightPointF.X) / column;

            float distanceY = (rightDownPointF.Y - leftRightPointF.Y) / row;

            TuService.PointFNormalization(
                baseMatters,
                new PointF(leftRightPointF.X + distanceX / 2, leftRightPointF.Y + distanceY / 2),
                new PointF(rightDownPointF.X - distanceX / 2, rightDownPointF.Y - distanceY / 2));

            if (baseMatters.Count == 1)
            {
                sizeRation = 1;
            }

            foreach (var item in baseMatters)
            {
                this.DrawRectangle(
                    new Pen(color, this.frameworkEdge),
                    item.Rectangle.X,
                    item.Rectangle.Y,
                    (int)(distanceX * sizeRation),
                    (int)(distanceY * sizeRation));

                item.Rectangle = new Rectangle(item.Rectangle.X, item.Rectangle.Y, (int)(distanceX * sizeRation), (int)(distanceY * sizeRation));
            }
        }

        /// <summary>
        /// 绘画实体位置
        /// </summary>
        /// <param name="bondPosition">实体集合</param>
        /// <param name="leftRightPointF">开始点</param>
        /// <param name="rightDownPointF">结束点</param>
        /// <param name="color">颜色</param>
        private void PaintBondPosition(List<BondPosition> bondPosition, PointF leftRightPointF, PointF rightDownPointF, Color color)
        {
            float distanceX = (rightDownPointF.X - leftRightPointF.X) / bondPosition.Count;

            float distanceY = (rightDownPointF.Y - leftRightPointF.Y) / bondPosition.Count;

            double sizeRation = 0.5;

            if (bondPosition.Count == 1)
            {
                sizeRation = 1;
            }

            TuService.PointFNormalization(
                bondPosition,
                new PointF(leftRightPointF.X + distanceX / 2, leftRightPointF.Y + distanceY / 2),
                new PointF(rightDownPointF.X - distanceX / 2, rightDownPointF.Y - distanceY / 2));

            foreach (var item in bondPosition)
            {
                if (item.MatterProductState == MatterProductState.Disable)
                {
                    this.DrawFillRectangle(
                        new Pen(Color.Black, this.frameworkEdge),
                        item.Rectangle.X,
                        item.Rectangle.Y,
                        (int)(distanceX * sizeRation),
                        (int)(distanceY * sizeRation));
                }
                else if (!this.IsRealTu && item.MatterProductState == MatterProductState.EnableInSystem1)
                {
                    this.DrawFillRectangle(
                           new Pen(Color.Cyan, this.frameworkEdge),
                           item.Rectangle.X,
                           item.Rectangle.Y,
                           (int)(distanceX * sizeRation),
                           (int)(distanceY * sizeRation));
                }
                else if (!this.IsRealTu && item.MatterProductState == MatterProductState.EnableInSystem2)
                {
                    this.DrawFillRectangle(
                           new Pen(Color.Crimson, this.frameworkEdge),
                           item.Rectangle.X,
                           item.Rectangle.Y,
                           (int)(distanceX * sizeRation),
                           (int)(distanceY * sizeRation));
                }
                else if (this.IsRealTu && (item.BondPositionInfo.IsFinishedInSystem1() && this.system == CurrentMachineSystemEnum.System1)
                         || (item.BondPositionInfo.IsFinishedInSystem2() && this.system == CurrentMachineSystemEnum.System2))
                {
                    this.DrawFillRectangle(
                        new Pen(Color.Green, this.frameworkEdge),
                        item.Rectangle.X,
                        item.Rectangle.Y,
                        (int)(distanceX * sizeRation),
                        (int)(distanceY * sizeRation));
                }
                else
                {
                    this.DrawFillRectangle(
                        new Pen(Color.BurlyWood, this.frameworkEdge),
                        item.Rectangle.X,
                        item.Rectangle.Y,
                        (int)(distanceX * sizeRation),
                        (int)(distanceY * sizeRation));
                }



                item.Rectangle = new Rectangle(item.Rectangle.X, item.Rectangle.Y, (int)(distanceX * sizeRation), (int)(distanceY * sizeRation));
            }
        }

        /// <summary>
        /// 鼠标滚轮缩放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcTransportShow_MouseWheel(object sender, MouseEventArgs e)
        {
            this.mousePosition = e.Location;

            // 计算缩放前的图形坐标
            PointF virtualPos = new PointF(
                (this.mousePosition.X - this.translate.X) / this.ratio,
                (this.mousePosition.Y - this.translate.Y) / this.ratio);

            // 调整缩放因子
            float zoom = e.Delta > 0 ? 1.1f : 1 / 1.1f;
            this.ratio *= zoom;
            this.ratio = Math.Max(0.1f, Math.Min(this.ratio, 10.0f)); // 限制缩放范围

            // 调整平移使鼠标位置保持在同一图形点上
            this.translate.X = this.mousePosition.X - virtualPos.X * this.ratio;
            this.translate.Y = this.mousePosition.Y - virtualPos.Y * this.ratio;

            this.PlControl.Refresh();
        }

        /// <summary>
        /// 绘画行列
        /// </summary>
        /// <param name="startPointFx">横轴开始点</param>
        /// <param name="startPointFx2">起始点2</param>
        /// <param name="xCount">X总数</param>
        /// <param name="intervalLength">间隔</param>
        /// <param name="length">显示长度</param>
        private void PaintScaleX(PointF startPointFx, PointF startPointFx2, double xCount, float intervalLength, int length)
        {
            Font f = new Font("Arial", 10, FontStyle.Bold);

            int withPen = 6;

            for (int i = 0; i < xCount; i++)
            {
                if (xCount > 30)
                {
                    if ((i + 1) % 5 != 0)
                    {
                        continue;
                    }
                }

                this.graphics.DrawLine(
                    new Pen(Color.BurlyWood, withPen),
                    startPointFx.X + i * intervalLength + intervalLength / 2,
                    startPointFx.Y + length,
                    startPointFx.X + i * intervalLength + intervalLength / 2,
                    startPointFx.Y);

                this.graphics.DrawLine(
                    new Pen(Color.BurlyWood, withPen),
                    startPointFx2.X + i * intervalLength + intervalLength / 2,
                    startPointFx2.Y - length,
                    startPointFx2.X + i * intervalLength + intervalLength / 2,
                    startPointFx2.Y);

                double offsetX = 0;

                if (i >= 99)
                {
                    offsetX = -10;
                }
                else if (i >= 9)
                {
                    offsetX = -5;
                }

                this.graphics.DrawString(
                    (i + 1).ToString(),
                    f,
                    Brushes.Black,
                    new PointF(startPointFx.X + i * intervalLength + intervalLength / 2 - 5 + (float)offsetX, startPointFx.Y + length));

                this.graphics.DrawString(
                    (i + 1).ToString(),
                    f,
                    Brushes.Black,
                    new PointF(
                        startPointFx2.X + i * intervalLength - 5 + intervalLength / 2 + (float)offsetX,
                        startPointFx2.Y - 2 * length));
            }
        }

        /// <summary>
        /// 绘画行列
        /// </summary>
        /// <param name="startPointFy">横轴开始点</param>
        /// <param name="yCount">y总数</param>
        /// <param name="intervalLength">间隔</param>
        /// <param name="length">显示长度</param>
        private void PaintScaleY(PointF startPointFy, PointF startPointFx2, double xCount, float intervalLength, int length)
        {
            Font f = new Font("Arial", 10, FontStyle.Bold);

            int withPen = 6;

            for (int i = 0; i < xCount; i++)
            {
                if (xCount > 30)
                {
                    if ((i + 1) % 5 != 0)
                    {
                        continue;
                    }
                }

                this.graphics.DrawLine(
                    new Pen(Color.BurlyWood, withPen),
                    startPointFy.X - length,
                    startPointFy.Y + i * intervalLength + intervalLength / 2,
                    startPointFy.X,
                    startPointFy.Y + i * intervalLength + intervalLength / 2);

                this.graphics.DrawLine(
                    new Pen(Color.BurlyWood, withPen),
                    startPointFx2.X + length,
                    startPointFx2.Y + i * intervalLength + intervalLength / 2,
                    startPointFx2.X,
                    startPointFx2.Y + i * intervalLength + intervalLength / 2);

                this.graphics.DrawString(
                    (i + 1).ToString(),
                    f,
                    Brushes.Black,
                    new PointF(startPointFy.X - 2 * length, startPointFy.Y + i * intervalLength - 5 + intervalLength / 2));

                this.graphics.DrawString(
                    (i + 1).ToString(),
                    f,
                    Brushes.Black,
                    new PointF(startPointFx2.X + length, startPointFx2.Y + i * intervalLength - 5 + intervalLength / 2));
            }
        }

        /// <summary>
        /// 绘画
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcTransportShow_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// 绘画
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_Paint(object sender, PaintEventArgs e)
        {
            if (this.transportUnit == null)
            {
                this.labelControl1.Visible = true;
                return;
            }

            this.labelControl1.Visible = false;

            this.graphics = e.Graphics;

            this.graphics.Clear(this.BackColor);

            // 应用缩放和平移变换
            e.Graphics.TranslateTransform(this.translate.X, this.translate.Y);
            e.Graphics.ScaleTransform(this.ratio, this.ratio);

            // 绘制主框架
            this.PaintFramework();

            // 绘制X轴
            this.PaintScaleX(
                new PointF(this.frameworkStartPointF.X, this.frameworkEndPointF.Y),
                this.frameworkStartPointF,
                this.SubColumnCount,
                this.subColumnSpace,
                15);

            // 绘制Y轴
            this.PaintScaleY(
                this.frameworkStartPointF,
                new PointF(this.frameworkEndPointF.X, this.frameworkStartPointF.Y),
                this.SubRowCount,
                this.subRowSpace,
                15);

            // 绘制sub
            this.PaintEntity(
                this.transportUnit.Substrates,
                this.frameworkStartPointF,
                this.frameworkEndPointF,
                this.SubColumnCount,
                this.SubRowCount,
                Color.BurlyWood);

            // 绘制Module
            foreach (var substrate in this.transportUnit.Substrates)
            {
                this.PaintEntity(
                    substrate.Modules,
                    new PointF(substrate.Rectangle.X - substrate.Rectangle.Width / 2, substrate.Rectangle.Y - substrate.Rectangle.Height / 2),
                    new PointF(substrate.Rectangle.X + substrate.Rectangle.Width / 2, substrate.Rectangle.Y + substrate.Rectangle.Height / 2),
                    this.ModuleColumnCount,
                    this.ModuleRowCount,
                Color.BurlyWood);
            }


            foreach (var substrate in this.transportUnit.Substrates)
            {
                foreach (var VARIABLE in substrate.Modules)
                {
                    this.PaintBondPosition(
                        VARIABLE.BondPositions,
                        new PointF(
                            VARIABLE.Rectangle.X - VARIABLE.Rectangle.Width / 2.0f,
                            VARIABLE.Rectangle.Y - VARIABLE.Rectangle.Height / 2.0f),
                        new PointF(
                            VARIABLE.Rectangle.X + VARIABLE.Rectangle.Width / 2.0f,
                            VARIABLE.Rectangle.Y + VARIABLE.Rectangle.Height / 2.0f),
                        Color.Coral);
                }
            }

            foreach (var VARIABLE in this.selectedBaseMatters)
            {
                this.DrawRectangle(
                    new Pen(Color.Blue, 3),
                    VARIABLE.Rectangle.X,
                    VARIABLE.Rectangle.Y,
                    VARIABLE.Rectangle.Width,
                    VARIABLE.Rectangle.Height);
            }

            if (this.isSelecting)
            {
                this.DrawSelected(this.mouseStartPoint, this.mouseEndPoint);
            }

            if (this.transportUnit.Substrates.Count > 2500)
            {
                if (ratio < 2.5)
                {
                    return;
                }
            }

            float size = 8 / ratio;

            // 在基板中心绘制基板的 index
            using (Font idxFont = new Font("Arial", size, FontStyle.Bold))
            {
                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    // 根据背景亮度选择文本颜色，确保对比度
                    Brush textBrush = Brushes.DarkRed;

                    foreach (var substrate in this.transportUnit.Substrates)
                    {
                        // substrate.Rectangle 的 X,Y 已为中心点
                        this.graphics.DrawString(substrate.Index.ToString(), idxFont, textBrush, new PointF(substrate.Rectangle.X, substrate.Rectangle.Y), sf);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制矩形
        /// 默认的矩形不好用，所以重构一下
        /// </summary>
        /// <param name="pen">笔刷</param>
        /// <param name="x">中心点X</param>
        /// <param name="y">中心点Y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        private void DrawRectangle(Pen pen, float x, float y, float width, float height)
        {
            this.graphics.DrawRectangle(pen, x - (width / 2), y - (height / 2), width, height);
        }

        /// <summary>
        /// 绘制矩形
        /// 默认的矩形不好用，所以重构一下
        /// </summary>
        /// <param name="pen">笔刷</param>
        /// <param name="x">中心点X</param>
        /// <param name="y">中心点Y</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        private void DrawFillRectangle(Pen pen, int x, int y, int width, int height)
        {
            this.graphics.FillRectangle(new SolidBrush(pen.Color), x - (width / 2), y - (height / 2), width, height);
        }

        /// <summary>
        /// 实体是否在显示区域
        /// </summary>
        /// <param name="startPointF">开始点</param>
        /// <param name="enPointF">结束点</param>
        /// <param name="baseMatter">实体</param>
        /// <returns>结果</returns>
        private bool IsInSelectedArea(PointF startPointF, PointF enPointF, BaseMatter baseMatter)
        {
            double maxX = Math.Max(startPointF.X, enPointF.X);
            double minX = Math.Min(startPointF.X, enPointF.X);
            double maxY = Math.Max(startPointF.Y, enPointF.Y);
            double minY = Math.Min(startPointF.Y, enPointF.Y);

            //return maxX > baseMatter.Rectangle.X + baseMatter.Rectangle.Width / 2
            //        && minX < baseMatter.Rectangle.X - baseMatter.Rectangle.Width / 2
            //        && maxY > baseMatter.Rectangle.Y + baseMatter.Rectangle.Height / 2
            //        && minY < baseMatter.Rectangle.Y - baseMatter.Rectangle.Height / 2;

            return maxX > baseMatter.Rectangle.X
                    && minX < baseMatter.Rectangle.X
                    && maxY > baseMatter.Rectangle.Y
                    && minY < baseMatter.Rectangle.Y;
        }

        /// <summary>
        /// 选项框是否生效
        /// </summary>
        /// <param name="startPointF">起始点</param>
        /// <param name="enPointF">结束点</param>
        /// <returns>结果</returns>
        public bool IsChooseEnable(PointF startPointF, PointF enPointF)
        {
            double distance = Math.Sqrt(
                (startPointF.X - enPointF.X) * (startPointF.X - enPointF.X)
                + (startPointF.Y - enPointF.Y) * (startPointF.Y - enPointF.Y));

            if (distance > 10 / this.ratio)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 实体是否在显示区域
        /// </summary>
        /// <param name="startPointF">开始点</param>
        /// <param name="baseMatter">实体</param>
        /// <returns>结果</returns>
        private bool IsInSelectedArea(PointF startPointF, BaseMatter baseMatter)
        {
            return startPointF.X < baseMatter.Rectangle.X + baseMatter.Rectangle.Width / 2
                   && startPointF.X > baseMatter.Rectangle.X - baseMatter.Rectangle.Width / 2
                   && startPointF.Y < baseMatter.Rectangle.Y + baseMatter.Rectangle.Height / 2
                   && startPointF.Y > baseMatter.Rectangle.Y - baseMatter.Rectangle.Height / 2;
        }

        /// <summary>
        /// 绘画选项框
        /// </summary>
        /// <param name="startPointF">开始点</param>
        /// <param name="enPointF">结束点</param>
        private void DrawSelected(PointF startPointF, PointF enPointF)
        {
            float maxX = Math.Max(startPointF.X, enPointF.X);
            float minX = Math.Min(startPointF.X, enPointF.X);
            float maxY = Math.Max(startPointF.Y, enPointF.Y);
            float minY = Math.Min(startPointF.Y, enPointF.Y);

            this.graphics.DrawRectangle(new Pen(Color.Aqua, 5 / this.ratio), minX, minY, maxX - minX, maxY - minY);
        }

        #region 事件

        /// <summary>
        /// 双击清除状态
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_DoubleClick(object sender, EventArgs e)
        {
            this.ratio = 1;
            this.translate = new PointF();
            this.Refresh();
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.transportUnit == null)
            {
                return;
            }

            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            PointF pointF = new PointF(
                (e.Location.X - this.translate.X) / this.ratio,
                (e.Location.Y - this.translate.Y) / this.ratio);

            this.mouseEndPoint = pointF;

            if (!this.IsChooseEnable(this.mouseStartPoint, pointF))
            {
                return;
            }

            this.isSelecting = true;

            Pen pen = new Pen(Color.Blue, 3);
            pen.DashStyle = DashStyle.Dot;

            this.selectedBaseMatters.Clear();

            foreach (var sub in this.transportUnit.Substrates)
            {
                if (this.IsInSelectedArea(this.mouseStartPoint, pointF, sub))
                {
                    this.selectedBaseMatters.Add(sub);
                }

                foreach (Module module in sub.Modules)
                {
                    if (this.IsInSelectedArea(this.mouseStartPoint, pointF, module))
                    {
                        this.selectedBaseMatters.Add(module);
                    }

                    foreach (BondPosition bondPosition in module.BondPositions)
                    {
                        if (this.IsInSelectedArea(this.mouseStartPoint, pointF, bondPosition))
                        {
                            this.selectedBaseMatters.Add(bondPosition);
                        }
                    }
                }
            }

            this.PlControl.Refresh();
        }

        /// <summary>
        /// 鼠标按下时发生
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.transportUnit == null)
            {
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                Point point = new Point();
                GetCursorPos(out point);
                this.popupMenu1.ShowPopup(point);

                return;
            }

            PointF startPoint = new PointF(
                (e.Location.X - this.translate.X) / this.ratio,
                (e.Location.Y - this.translate.Y) / this.ratio);

            this.mouseStartPoint = startPoint;

            foreach (var sub in this.transportUnit.Substrates)
            {
                if (this.IsInSelectedArea(startPoint, sub))
                {
                    bool isSelectModule = false;

                    foreach (Module module in sub.Modules)
                    {
                        if (this.IsInSelectedArea(startPoint, module))
                        {
                            bool isSelectBp = false;
                            foreach (BondPosition bondPosition in module.BondPositions)
                            {
                                if (this.IsInSelectedArea(startPoint, bondPosition))
                                {
                                    isSelectBp = true;
                                    isSelectModule = true;
                                    this.selectedBaseMatters.Add(bondPosition);
                                }
                            }

                            if (!isSelectBp)
                            {
                                this.selectedBaseMatters.Add(module);
                                isSelectModule = true;
                            }
                        }
                    }

                    if (!isSelectModule)
                    {
                        this.selectedBaseMatters.Add(sub);
                    }
                }
            }

            this.PlControl.Refresh();
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_MouseUp(object sender, MouseEventArgs e)
        {
            this.isSelecting = false;
            this.Refresh();
        }

        #endregion

        /// <summary>
        /// 刷新UI
        /// </summary>
        /// <param name="transportUnit">产品</param>
        public void ReFreshUi(TransportUnit transportUnit)
        {
            this.selectedBaseMatters.Clear();
            this.transportUnit = transportUnit;
            this.PlControl.Refresh();
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDisable_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Working)
            {
                AKRSMessageBoxExt.Show("当前机器正在运行，无法修改状态", "提示", new string[] { "确定" }, new DialogResult[] { DialogResult.OK });
                this.selectedBaseMatters.Clear();

                this.PlControl.Refresh();

                this.IsChange = true;
                return;
            }

            foreach (BaseMatter matter in this.selectedBaseMatters)
            {
                matter.SetMatterDisable();

                if (matter is BondPosition)
                {
                    BondPosition b = (BondPosition)matter;
                    b.BondPositionInfo.SetUnFinishedInSystem1();
                    b.BondPositionInfo.SetUnFinishedInSystem2();
                }
            }

            this.selectedBaseMatters.Clear();

            this.PlControl.Refresh();

            this.IsChange = true;
        }

        /// <summary>
        /// 正常
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtNormal_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Working)
            {
                AKRSMessageBoxExt.Show("当前机器正在运行，无法修改状态", "提示", new string[] { "确定" }, new DialogResult[] { DialogResult.OK });
                this.selectedBaseMatters.Clear();

                this.PlControl.Refresh();

                this.IsChange = true;
                return;
            }

            foreach (BaseMatter matter in this.selectedBaseMatters)
            {
                matter.SetMatterEnable();

                if (matter is BondPosition)
                {
                    BondPosition b = (BondPosition)matter;
                    b.MatterProductState = MatterProductState.Enable;
                    b.BondPositionInfo.SetUnFinishedInSystem1();
                    b.BondPositionInfo.SetUnFinishedInSystem2();
                }
            }

            this.selectedBaseMatters.Clear();

            this.PlControl.Refresh();

            this.IsChange = true;
        }

        /// <summary>
        /// 正常
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSuccess_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Working)
            {
                AKRSMessageBoxExt.Show("当前机器正在运行，无法修改状态", "提示", new string[] { "确定" }, new DialogResult[] { DialogResult.OK });
                this.selectedBaseMatters.Clear();

                this.PlControl.Refresh();

                this.IsChange = true;
                return;
            }

            foreach (BaseMatter matter in this.selectedBaseMatters)
            {
                if (matter is BondPosition)
                {
                    BondPosition b = (BondPosition)matter;
                    b.MatterProductState = MatterProductState.Enable;
                    b.BondPositionInfo.SetFinishedInSystem1();
                    b.BondPositionInfo.SetFinishedInSystem2();
                }
            }

            this.selectedBaseMatters.Clear();

            this.PlControl.Refresh();

            this.IsChange = true;
        }

        /// <summary>
        /// 只贴片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtOnlyBond_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Machine.GetInstance().IsWorking())
            {
                return;
            }

            foreach (BaseMatter matter in this.selectedBaseMatters)
            {
                if (matter is BondPosition)
                {
                    BondPosition b = (BondPosition)matter;
                    b.MatterProductState = MatterProductState.EnableInSystem2;
                }
            }

            this.selectedBaseMatters.Clear();

            this.PlControl.Refresh();

            this.IsChange = true;
        }

        /// <summary>
        /// 只点胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtOnlyDispense_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Machine.GetInstance().IsWorking())
            {
                return;
            }

            foreach (BaseMatter matter in this.selectedBaseMatters)
            {
                if (matter is BondPosition)
                {
                    BondPosition b = (BondPosition)matter;
                    b.MatterProductState = MatterProductState.EnableInSystem1;
                }
            }

            this.selectedBaseMatters.Clear();

            this.PlControl.Refresh();

            this.IsChange = true;
        }
    }
}
