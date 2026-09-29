using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DevExpress.Utils.Design;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Svg;
using DevExpress.XtraSplashScreen;

namespace AKRS.ZX2200.Infrastructure.Controls.Common;

public class TextOverlayPainter : OverlayWindowPainterBase
{
    public string Text { get; set; }

    public Font Font { get; set; }

    public Brush Brush { get; set; }

    protected override void Draw(OverlayWindowCustomDrawContext context)
    {
        context.DefaultDraw();
        GraphicsCache cache = context.DrawArgs.Cache;
        Rectangle bounds = context.DrawArgs.Bounds;
        string msg = this.Text ?? "加载中…";
        cache.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
        SizeF max = new(bounds.Width, bounds.Height);
        SizeF sz = cache.CalcTextSize(this.Text, this.Font, max);
        PointF center = new(bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2);
        PointF pt = center + new SizeF(-sz.Width / 2, context.DrawArgs.Image.Height / 2 + 5);
        cache.DrawString(msg, this.Font, this.Brush, pt);
    }
}