using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using LanguageExt.ClassInstances;

    /// <summary>
    /// 绘制载具单元
    /// </summary>
    public partial class UcTransportUnitView : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 配置对象集合
        /// </summary>
        private List<OppositeSex> List =>
            ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.GetOppositeSexConfigs();

        /// <summary>
        /// 刷新选中
        /// </summary>
        private readonly Action<OppositeSex> refreshSelectAction;

        /// <summary>
        /// 刷新
        /// </summary>
        private readonly Action refreshAction;

        /// <summary>
        /// 刷新
        /// </summary>
        private readonly Action ChangeAction;

        /// <summary>
        /// 画笔
        /// </summary>
        private Graphics g;

        /// <summary>
        /// 缩放比例X
        /// </summary>
        private double ratioX;

        /// <summary>
        /// 缩放比例Y
        /// </summary>
        private double ratioY;

        /// <summary>
        /// 绘制选中单元
        /// </summary>
        /// <param name="refreshSelectAction">事件源</param>
        /// <param name="refreshAction">封装参数</param>
        /// <param name="changeAction">封装参数</param>
        public UcTransportUnitView(Action<OppositeSex> refreshSelectAction, Action refreshAction, Action changeAction)
        {
            this.InitializeComponent();
            this.refreshSelectAction = refreshSelectAction;
            this.refreshAction = refreshAction;
            this.MouseWheel += this.UcTransportShow_MouseWheel;
            this.ChangeAction = changeAction;
        }

        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Point mousePosition;

        /// <summary>
        /// 平移量
        /// </summary>
        private PointF translate;

        /// <summary>
        /// 缩放比例
        /// </summary>
        private float ratio = 1;

        /// <summary>
        /// 边缘比例
        /// </summary>
        private float range => (5.0f / ratio);

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
            this.ratio = Math.Max(1f, Math.Min(this.ratio, 50.0f)); // 限制缩放范围

            // 调整平移使鼠标位置保持在同一图形点上
            this.translate.X = this.mousePosition.X - virtualPos.X * this.ratio;
            this.translate.Y = this.mousePosition.Y - virtualPos.Y * this.ratio;

            if (ratio == 1f)
            {
                this.translate = new PointF();
            }

            this.PlControl.Refresh();
        }

        /// <summary>
        /// 选中的
        /// </summary>
        public OppositeSex SelectOppositeSex { get; set; }

        /// <summary>
        /// 绘制单个对象
        /// </summary>
        /// <param name="oppositeSexConfig">配置文件</param>
        private void DrawSingleObject(OppositeSex oppositeSexConfig)
        {
            if (oppositeSexConfig == null)
            {
                return;
            }

            double sizeX = oppositeSexConfig.SizeX * this.ratioX;
            double sizeY = oppositeSexConfig.SizeY * this.ratioY;
            sizeX = Math.Max(1, sizeX);
            sizeY = Math.Max(1, sizeY);

            Color color = Color.DarkKhaki;

            bool fill = oppositeSexConfig.MatterTypeEnum == EntityTypeEnum.BondPosition;

            if (oppositeSexConfig.MatterTypeEnum == EntityTypeEnum.BondPosition)
            {
                if (oppositeSexConfig.GetMatterProductState() == MatterProductState.Disable)
                {
                    color = Color.Black;
                }
                else
                {
                    color = Color.DarkSalmon;
                }
            }
            
            this.SingleDraw(
                oppositeSexConfig.AbsoluteCenterCoordinate.Point.X * this.ratioX + this.Width / 2.0,
                this.Height / 2.0 - oppositeSexConfig.AbsoluteCenterCoordinate.Point.Y * this.ratioY - sizeY,
                oppositeSexConfig.SizeX * this.ratioX,
                oppositeSexConfig.SizeY * this.ratioY,
                color,
                fill);

            if (oppositeSexConfig == this.SelectOppositeSex)
            {
                color = Color.Red;
                this.SingleDraw(
                    oppositeSexConfig.AbsoluteCenterCoordinate.Point.X  * this.ratioX + this.Width / 2.0,
                    this.Height / 2.0 - oppositeSexConfig.AbsoluteCenterCoordinate.Point.Y * this.ratioY - sizeY,
                    oppositeSexConfig.SizeX * this.ratioX,
                    oppositeSexConfig.SizeY * this.ratioY,
                    color,
                    false);
            }
        }

        /// <summary>
        /// 点位是否在选中的焊点里面
        /// </summary>
        /// <param name="point">事件源</param>
        /// <param name="oppositeSexConfig">封装参数</param>
        private void MouseSelectOppositeSexConfig(PointF point, OppositeSex oppositeSexConfig)
        {
            double sizeX = oppositeSexConfig.SizeX * this.ratioX;
            double sizeY = oppositeSexConfig.SizeY * this.ratioY;
            sizeX = Math.Max(1, sizeX);
            sizeY = Math.Max(1, sizeY);

            double locateX = oppositeSexConfig.AbsoluteCenterCoordinate.Point.X * this.ratioX + this.Width / 2.0
                             - sizeX / 2.0;
            double locateY = this.Height / 2.0 - oppositeSexConfig.AbsoluteCenterCoordinate.Point.Y * this.ratioY
                                               - sizeY
                             + sizeY / 2.0;
            double endX = locateX + sizeX;
            double endY = locateY + sizeY;

            if (point.X > locateX && point.X < endX && point.Y > locateY && point.Y < endY)
            {
                this.SelectOppositeSex = oppositeSexConfig;
            }
        }

        /// <summary>
        /// 绘制一个
        /// </summary>
        /// <param name="startX">开始点</param>
        /// <param name="startY">结束点</param>
        /// <param name="sizeX">长</param>
        /// <param name="sizeY">短</param>
        /// <param name="color">颜色</param>
        /// <param name="fill">是否填充</param>
        private void SingleDraw(double startX, double startY, double sizeX, double sizeY, Color color, bool fill)
        {
            if (sizeX < 1)
            {
                sizeX = 1;
            }

            if (sizeY < 1)
            {
                sizeY = 1;
            }

            if (!fill)
            {
                this.g.DrawRectangle(
                    new Pen(color, range),
                   (float)(startX - sizeX / 2.0), (float)(startY + sizeY / 2.0), (float)sizeX, (float)sizeY);
            }
            else
            {
                this.g.FillRectangle(
                    new SolidBrush(color),
                    new RectangleF(new PointF((float)(startX - sizeX / 2.0), (float)(startY + sizeY / 2.0)), new SizeF((float)sizeX, (float)sizeY)));
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.PlControl.Refresh();
        }

        /// <summary>
        /// 绘画
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PanelControl1_Paint(object sender, PaintEventArgs e)
        {
            // 应用缩放和平移变换
            e.Graphics.TranslateTransform(this.translate.X, this.translate.Y);
            e.Graphics.ScaleTransform(this.ratio, this.ratio);

            ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.InitOppositeSexConfigPosition();

            double maxX = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.GetOppositeSexConfigs()
                .Max(it => it.AbsoluteCenterCoordinate.Point.X + it.SizeX / 2.0);

            double minX = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.GetOppositeSexConfigs()
                .Min(it => it.AbsoluteCenterCoordinate.Point.X - it.SizeX / 2.0);

            double maxY = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.GetOppositeSexConfigs()
                .Max(it => it.AbsoluteCenterCoordinate.Point.Y + it.SizeY / 2.0);

            double minY = ProductDomain.GetInstance().ProductConfig.OppositeSexConfiguration.GetOppositeSexConfigs()
                .Min(it => it.AbsoluteCenterCoordinate.Point.Y - it.SizeY / 2.0);
            
            this.ratioX = this.PlControl.Size.Width / (maxX - minX);

            this.ratioY = this.PlControl.Size.Height / (maxY - minY);

            this.g = e.Graphics;

            foreach (OppositeSex oppositeSexConfig in this.List)
            {
                this.DrawSingleObject(oppositeSexConfig);
            }

            this.DrawSingleObject(this.SelectOppositeSex);
        }
        
        /// <summary>
        /// 单机
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_Click(object sender, EventArgs e)
        {
           
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_MouseDown(object sender, MouseEventArgs e)
        {
            PointF startPoint = new PointF(
               (e.Location.X - this.translate.X) / this.ratio,
               (e.Location.Y - this.translate.Y) / this.ratio);

            this.SelectOppositeSex = null;

            foreach (OppositeSex oppositeSexConfig in this.List)
            {
                this.MouseSelectOppositeSexConfig(startPoint, oppositeSexConfig);
            }

            this.Refresh();
            this.refreshSelectAction(this.SelectOppositeSex);
        }

        /// <summary>
        /// 右击菜单
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                this.popupMenu1.ShowPopup(Control.MousePosition);
            }
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCopy_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OppositeSex oppositeSexConfig = this.SelectOppositeSex;

            if (oppositeSexConfig == null)
            {
                AKRSXtraMessageBox.Show("请先选择需要复制的单元");
                return;
            }

            FrmObjectCopy frmObjectCopy = new FrmObjectCopy(oppositeSexConfig);

            if (frmObjectCopy.ShowDialog() != DialogResult.OK)
            {
                frmObjectCopy.Dispose();
                return;
            }

            frmObjectCopy.Dispose();
            OppositeSex oppositeSexConfigUp = ProductConfiguration.GetInstance().OppositeSexConfiguration
                .GetOppositeSexConfigs().Find(it => it.Id == oppositeSexConfig.ParentId);

            oppositeSexConfigUp.DownConfigs.AddRange(frmObjectCopy.NewOppositeSexConfigs);

            foreach (OppositeSex oppositeSexConfigNew in frmObjectCopy.NewOppositeSexConfigs)
            {
                oppositeSexConfigNew.Name =
                    OppositeSex.GetDefaultName(oppositeSexConfig.MatterTypeEnum.GetDescription());
            }

            this.refreshAction();

            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmAddObject frmAddObject = new FrmAddObject();
            frmAddObject.SelectOppositeSexConfig = this.SelectOppositeSex;
            DialogResult dialogResult = frmAddObject.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                frmAddObject.Dispose();
                return;
            }

            frmAddObject.Dispose();

            ProductConfiguration.GetInstance().Save();
            this.refreshAction();
            this.refreshSelectAction(frmAddObject.OppositeSexConfig);

            this.SelectOppositeSex = frmAddObject.OppositeSexConfig;

            this.Init();
        }

        /// <summary>
        /// 屏蔽
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDisplay_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.SelectOppositeSex.SetMatterProductState(MatterProductState.Disable);
            this.Init();
            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.SelectOppositeSex.SetMatterProductState(MatterProductState.Enable);
            this.Init();
            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PlControl_DoubleClick(object sender, EventArgs e)
        {
            if (this.SelectOppositeSex == null)
            {
                this.translate = new PointF(0, 0);
                this.ratio = 1;
                this.Refresh();
            }
            else
            {
                // 切换页面
                this.ChangeAction();
            }
        }
    }
}
