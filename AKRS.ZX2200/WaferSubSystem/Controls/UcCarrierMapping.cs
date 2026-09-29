using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Controls
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Services;

    /// <summary>
    /// UcCarrierMapping
    /// </summary>
    public partial class UcCarrierMapping : DevExpress.XtraEditors.XtraUserControl
    {
        #region Events

        /// <summary>
        /// UcCarrierMapping_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void UcCarrierMapping_Load(object sender, EventArgs e)
        {
            this.Dock = DockStyle.Fill;
            this.DoubleBuffered = true;
            this.penSelected = new Pen(Color.DarkCyan, 2);
            this.penUnSelected = new Pen(this.PnlCarrier.BackColor, 2);
            this.SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint,
                true);
        }

        /// <summary>
        /// PnlCarrier_Paint
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PnlCarrier_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            // Reset coordinate system
            e.Graphics.ResetTransform();

            e.Graphics.Clear(this.PnlCarrier.BackColor);

            // Here comes everything that has to be calculated on each resize/redraw
            // Just do this calculations once
            // Let's find the best Size for the outline
            float w = this.Width * this.Zoom;
            float h = this.Height * this.Zoom;

            float size = w < h ? w : h;

            // Wafersize is size-2 because we're not drawing the first and the last pixels
            SizeF wafersize = new SizeF(size - 2, size - 2);
            PointF starting = new PointF((w - size) / 2f, (h - size) / 2f);
            this.boundingBox = new RectangleF(starting.Offset(50, 50), wafersize.Offset(-100, -100));

            // e.Graphics.DrawRectangle(new Pen(Color.Black, 1), this.boundingBox.X, this.boundingBox.Y, this.boundingBox.Width, this.boundingBox.Height);

            // Let's calculate everything needed for drawing the dies
            if (this.CarrierMapping != null && this.CarrierMapping.WaffleSlots.Length > 0)
            {
                int maxX = this.CarrierMapping.RowCount;
                int maxY = this.CarrierMapping.ColumnCount;
                float sizeX = this.boundingBox.Width / (float)maxY;
                float sizeY = this.boundingBox.Height / (float)maxX;
                this.dieSize = new SizeF(sizeX, sizeY);

                // Draw the acupoint
                this.DrawDies(e.Graphics);
            }

            // Draw selected acupoints
            this.DrawSelectedAcupoints(e.Graphics);

            // Draw start acupoint
            this.DrawStartAcupoint(e.Graphics);

            // Draw current acupoint
            this.DrawCurrentAcupoint(e.Graphics);

            if (this.isShowScale)
            {
                // Draw seqNo
                this.DrawSeqNo(e.Graphics);
            }
        }

        /// <summary>
        /// PnlCarrier_MouseDown
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PnlCarrier_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.CarrierMapping == null)
            {
                return;
            }

            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Control)
            {
                this.selectStartPoint = e.Location;
            }
            else if (e.Button == MouseButtons.Left)
            {
                // cancel select
                this.selectedRegions.Clear();
                this.selectedAcupoints.Clear();

                this.selectStartPoint = e.Location;

                float x_coord = ((float)e.Y - this.boundingBox.Y) / this.dieSize.Height;
                float y_coord = ((float)e.X - this.boundingBox.X) / this.dieSize.Width;
                int x = (int)Math.Floor(x_coord);
                int y = (int)Math.Floor(y_coord);
                this.currentAcupoint = this.CarrierMapping.GetAcupoint(x, y);
            }

            this.PnlCarrier.Refresh();
        }

        /// <summary>
        /// PnlCarrier_MouseMove
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PnlCarrier_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.CarrierMapping == null)
            {
                return;
            }

            if (this.selectStartPoint.IsEmpty == false)
            {
                this.selectEndPoint = e.Location;

                RectangleF selectedRect = new RectangleF(
                    Math.Min(this.selectStartPoint.X, this.selectEndPoint.X),
                    Math.Min(this.selectStartPoint.Y, this.selectEndPoint.Y),
                    Math.Abs(this.selectEndPoint.X - this.selectStartPoint.X),
                    Math.Abs(this.selectEndPoint.Y - this.selectStartPoint.Y));

                this.selectedAcupoints.Clear();
                this.selectedAcupoints.AddRange(this.GetSelectedAcupoints(selectedRect));

                foreach (RectangleF rect in this.selectedRegions)
                {
                    this.selectedAcupoints.AddRange(this.GetSelectedAcupoints(rect));
                }

                // Distinct
                this.selectedAcupoints = this.selectedAcupoints.Distinct().ToList();

                this.PnlCarrier.Refresh();
            }
        }

        /// <summary>
        /// PnlCarrier_MouseUp
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PnlCarrier_MouseUp(object sender, MouseEventArgs e)
        {
            RectangleF selectedRect = new RectangleF(
                Math.Min(this.selectStartPoint.X, this.selectEndPoint.X),
                Math.Min(this.selectStartPoint.Y, this.selectEndPoint.Y),
                Math.Abs(this.selectEndPoint.X - this.selectStartPoint.X),
                Math.Abs(this.selectEndPoint.Y - this.selectStartPoint.Y));

            this.selectedRegions.Add(selectedRect);

            this.selectStartPoint = PointF.Empty;

            // Right-click menu
            this.ShowPopup(e, this.pMenuCarrier);
        }

        /// <summary>
        /// PnlCarrier_MouseWheel
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void PnlCarrier_MouseWheel(object sender, MouseEventArgs e)
        {
            this.Zoom *= (e.Delta > 0) ? 1.1f : 0.9f;
            this.PnlCarrier.Refresh();
        }

        /// <summary>
        /// BarBtSetState_ItemClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BarBtSetState_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.CarrierMapping == null)
            {
                return;
            }

            this.GetSelectedAcupoints().ForEach(a => a.SlotState = (SlotStatuEnum)e.Item.Tag);
            if (this.currentAcupoint != null)
            {
                this.currentAcupoint.SlotState = (SlotStatuEnum)e.Item.Tag;
            }

            this.PnlCarrier.Refresh();
        }

        /// <summary>
        /// BtnSeqToOne_ItemClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnSeqToOne_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.isSeqToOne = !this.isSeqToOne;
            this.PnlCarrier.Refresh();
        }

        /// <summary>
        /// BtnSetStart_ItemClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnSetStart_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 
            this.CarrierMapping.CurrentIndex = this.currentAcupoint.Index;
            this.PnlCarrier.Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="adapterSlotEntity">adapterSlotEntity</param>
        public UcCarrierMapping(AdapterSlotEntity adapterSlotEntity)
        {
            this.InitializeComponent();
            this.CarrierMapping = adapterSlotEntity;
            this.RegisterEvents();
        }

        public void SetCarrierMapping(AdapterSlotEntity adapterSlotEntity)
        {
            this.CarrierMapping = adapterSlotEntity;
        }

        /// <summary>
        /// RegisterEvents
        /// </summary>
        private void RegisterEvents()
        {
            // Event to be registered
            this.Load += this.UcCarrierMapping_Load;
            this.PnlCarrier.Paint += this.PnlCarrier_Paint;
            this.PnlCarrier.MouseDown += this.PnlCarrier_MouseDown;
            this.PnlCarrier.MouseMove += this.PnlCarrier_MouseMove;
            this.PnlCarrier.MouseUp += this.PnlCarrier_MouseUp;
            this.PnlCarrier.MouseWheel += this.PnlCarrier_MouseWheel;

            this.InitStatePMenu();
        }

        /// <summary>
        /// DrawDies
        /// </summary>
        /// <param name="g">g</param>
        private void DrawDies(Graphics g)
        {
            for (int x = 0; x < this.CarrierMapping.RowCount; x++)
            {
                for (int y = 0; y < this.CarrierMapping.ColumnCount; y++)
                {
                    PointF position = new PointF(
                        this.boundingBox.X + (float)y * this.dieSize.Width,
                        this.boundingBox.Y + (float)x * this.dieSize.Height);

                    float scale = 1;
                    RectangleF die = new RectangleF(
                        position.X,
                        position.Y,
                        (float)(this.dieSize.Width * scale),
                        (float)(this.dieSize.Height * scale));

                    AcupointEntity acupoint = this.CarrierMapping.GetAcupoint(x, y);
                    if (acupoint != null)
                    {
                        acupoint.SpecPos = new PointF(die.Right, die.Bottom);
                        acupoint.Rect = die;

                        (string description, Brush brush) colorBrush = acupoint.GetColorBrushDescription();
                        g.FillRectangle(colorBrush.brush, die);
                        g.DrawRectangle(new Pen(this.PnlCarrier.BackColor, 2), die.X, die.Y, die.Width, die.Height);
                    }
                }
            }
        }

        /// <summary>
        /// DrawStartAcupoint
        /// </summary>
        /// <param name="g">g</param>
        private void DrawStartAcupoint(Graphics g)
        {
            RectangleF die;
            if (this.CarrierMapping.CurrentIndex >= 0 && this.CarrierMapping.CurrentIndex < this.CarrierMapping.WaffleSlotsList.Count)
            {
                die = this.CarrierMapping.GetWaffleSlot().Rect;
            }
            else
            {
                if (this.CarrierMapping.RowCount == 0 || this.CarrierMapping.ColumnCount == 0)
                {
                    return;
                }

                die = this.CarrierMapping.GetAcupointRect(0);
            }

            g.DrawRectangle(new Pen(Color.Red, 3), die.X, die.Y, die.Width, die.Height);
        }

        /// <summary>
        /// DrawCurrentAcupoint
        /// </summary>
        /// <param name="g">g</param>
        private void DrawCurrentAcupoint(Graphics g)
        {
            if (this.currentAcupoint != null)
            {
                RectangleF die = this.currentAcupoint.Rect;
                g.DrawRectangle(new Pen(Color.Peru, 5), die.X, die.Y, die.Width, die.Height);
            }
        }

        /// <summary>
        /// DrawSeqNo
        /// </summary>
        /// <param name="g">g</param>
        public void DrawSeqNo(Graphics g)
        {
            if (this.CarrierMapping == null || this.CarrierMapping.RowCount == 0 || this.CarrierMapping.ColumnCount == 0)
            {
                return;
            }

            Pen pen = new Pen(Color.Black);
            Brush brush = Brushes.Black;
            Pen penCurrent = new Pen(Color.Red);
            Brush brushCurrent = Brushes.Red;
            Font font = new Font("Arial", 9);
            Font fontCurrent = new Font("Arial", 9);

            PointF seqPositionOffsetY = new PointF(-30, 0);
            PointF seqPositionOffsetX = new PointF(0, -30);
            float scaleLineOffset = -20;
            float scale = 5;

            RectangleF rect;
            PointF pointF1;
            PointF pointF2;
            PointF seqPointF;
            int drawString;

            // y
            for (int i = 0; i < this.CarrierMapping.RowCount; i++)
            {
                rect = this.CarrierMapping.GetAcupointRect(i, 0);
                if (rect == RectangleF.Empty)
                {
                    continue;
                }

                pointF1 = new PointF(this.boundingBox.X, (rect.Y + rect.Bottom) / 2);
                pointF2 = new PointF(this.boundingBox.X + scaleLineOffset, (rect.Y + rect.Bottom) / 2);
                seqPointF = new PointF(this.boundingBox.X, (rect.Y + rect.Bottom) / 2).Offset(seqPositionOffsetY);
                if (this.isSeqToOne)
                {
                    drawString = i + 1;
                }
                else
                {
                    drawString = i;
                }

                if (drawString % 5 == 0 || i == 0)
                {
                    g.DrawLine(pen, pointF1, pointF2);
                    g.DrawString(drawString.ToString(), font, brush, seqPointF);
                }
                else
                {
                    pointF2 = new PointF(this.boundingBox.X + scaleLineOffset / scale, (rect.Y + rect.Bottom) / 2);
                    g.DrawLine(pen, pointF1, pointF2);
                }

                if (this.currentAcupoint != null)
                {
                    RectangleF rectCurrent = this.CarrierMapping.GetAcupointRect(this.currentAcupoint.RowIndex, 0);
                    if (rectCurrent == rect)
                    {
                        pointF2 = new PointF(this.boundingBox.X + scaleLineOffset, (rect.Y + rect.Bottom) / 2);
                        g.DrawLine(penCurrent, pointF1, pointF2);
                        g.DrawString(drawString.ToString(), fontCurrent, brushCurrent, seqPointF);
                    }
                }
            }

            // x
            for (int j = 0; j < this.CarrierMapping.ColumnCount; j++)
            {
                rect = this.CarrierMapping.GetAcupointRect(0, j);
                if (rect == RectangleF.Empty)
                {
                    continue;
                }

                pointF1 = new PointF((rect.X + rect.Right) / 2, this.boundingBox.Y);
                pointF2 = new PointF((rect.X + rect.Right) / 2, this.boundingBox.Y + scaleLineOffset);
                seqPointF = new PointF((rect.X + rect.Right) / 2, this.boundingBox.Y).Offset(seqPositionOffsetX);
                if (this.isSeqToOne)
                {
                    drawString = j + 1;
                }
                else
                {
                    drawString = j;
                }

                if (drawString % 5 == 0 || j == 0)
                {
                    g.DrawLine(pen, pointF1, pointF2);
                    g.DrawString(drawString.ToString(), font, brush, seqPointF);
                }
                else
                {
                    pointF2 = new PointF((rect.X + rect.Right) / 2, this.boundingBox.Y + scaleLineOffset / scale);
                    g.DrawLine(pen, pointF1, pointF2);
                }

                if (this.currentAcupoint != null)
                {
                    RectangleF rectCurrent = this.CarrierMapping.GetAcupointRect(0, this.currentAcupoint.ColumnIndex);
                    if (rectCurrent == rect)
                    {
                        pointF2 = new PointF((rect.X + rect.Right) / 2, this.boundingBox.Y + scaleLineOffset);
                        g.DrawLine(penCurrent, pointF1, pointF2);
                        g.DrawString(drawString.ToString(), fontCurrent, brushCurrent, seqPointF);
                    }
                }
            }

            pen.Dispose();
        }

        /// <summary>
        /// DrawSelectedAcupoints
        /// </summary>
        /// <param name="g">g</param>
        private void DrawSelectedAcupoints(Graphics g)
        {
            if (this.selectedAcupoints.Any())
            {
                if (this.CarrierMapping.WaffleSlots != null)
                {
                    foreach (AcupointEntity acupoint in this.CarrierMapping.WaffleSlots)
                    {
                        RectangleF rect = new RectangleF(
                            acupoint.Rect.X,
                            acupoint.Rect.Y,
                            acupoint.Rect.Width,
                            acupoint.Rect.Height);

                        if (this.selectedAcupoints.Contains(acupoint))
                        {
                            g.DrawRectangle(this.penSelected, rect.X, rect.Y, rect.Width, rect.Height);
                        }
                        else
                        {
                            if (this.isReDrawUnSelected)
                            {
                                g.DrawRectangle(this.penUnSelected, rect.X, rect.Y, rect.Width, rect.Height);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// GetSelectedAcupoints
        /// </summary>
        /// <param name="selectedRect">selectedRect</param>
        /// <returns>result</returns>
        private List<AcupointEntity> GetSelectedAcupoints(RectangleF selectedRect)
        {
            List<AcupointEntity> selAcupoints = new List<AcupointEntity>();
            if (this.CarrierMapping.WaffleSlots != null)
            {
                foreach (AcupointEntity acupoint in this.CarrierMapping.WaffleSlots)
                {
                    PointF picCenterPoint = this.CarrierMapping.GetPosSpec(acupoint);
                    bool ret = selectedRect.Contains(picCenterPoint);

                    if (ret)
                    {
                        selAcupoints.Add(acupoint);
                    }
                }
            }

            return selAcupoints;
        }

        /// <summary>
        /// GetSelectedAcupoints
        /// </summary>
        /// <returns>result</returns>
        private List<AcupointEntity> GetSelectedAcupoints()
        {
            return this.selectedAcupoints;
        }

        /// <summary>
        /// InitStatePMenu
        /// </summary>
        private void InitStatePMenu()
        {
            int index = 1;
            foreach (SlotStatuEnum temp in Enum.GetValues(typeof(SlotStatuEnum)))
            {
                BarButtonItem barBtSetState = new BarButtonItem();

                AcupointAttribute cb = (AcupointAttribute)EnumHelper.GetCustomAttributes<AcupointAttribute>(temp);
                barBtSetState.Caption = cb.StateDescription;
                barBtSetState.Tag = temp;
                barBtSetState.Name = "barBtSetState" + index;
                barBtSetState.ItemClick += this.BarBtSetState_ItemClick;

                this.barManager1.Items.AddRange(
                    new BarItem[]
                        {
                            barBtSetState
                        });

                this.BarSubItemState.LinksPersistInfo.AddRange(
                    new[]
                        {
                            new LinkPersistInfo(barBtSetState),
                        });
            }

            this.BtnSetStart.ItemClick += this.BtnSetStart_ItemClick;
            this.BtnSeqToOne.ItemClick += this.BtnSeqToOne_ItemClick;
        }

        /// <summary>
        /// ShowPopup
        /// </summary>
        /// <param name="e">e</param>
        /// <param name="popupMenu">popupMenu</param>
        public void ShowPopup(MouseEventArgs e, PopupMenu popupMenu)
        {
            if (e.Button == MouseButtons.Right)
            {
                popupMenu.ShowPopup(Control.MousePosition);
            }
        }

        #endregion

        #region Field

        /// <summary>
        /// CarrierMapping
        /// </summary>
        private AdapterSlotEntity CarrierMapping { get; set; }

        /// <summary>
        /// penSelected
        /// </summary>
        private Pen penSelected;

        /// <summary>
        /// penUnSelected
        /// </summary>
        private Pen penUnSelected;

        /// <summary>
        /// selectStartPoint
        /// </summary>
        private PointF selectStartPoint;

        /// <summary>
        /// selectEndPoint
        /// </summary>
        private PointF selectEndPoint;

        /// <summary>
        /// selectedRegions
        /// </summary>
        private List<RectangleF> selectedRegions = new List<RectangleF>();

        /// <summary>
        /// selectedAcupoints
        /// </summary>
        private List<AcupointEntity> selectedAcupoints = new List<AcupointEntity>();

        /// <summary>
        /// currentAcupoint
        /// </summary>
        private AcupointEntity currentAcupoint;

        /// <summary>
        /// Zoom
        /// </summary>
        public float Zoom { get; set; } = 1f;

        /// <summary>
        /// boundingBox
        /// </summary>
        private RectangleF boundingBox;

        /// <summary>
        /// dieSize
        /// </summary>
        private SizeF dieSize;

        /// <summary>
        /// isShowScale
        /// </summary>
        private bool isShowScale = true;

        /// <summary>
        /// isShowScale
        /// </summary>
        private bool isSeqToOne = true;

        /// <summary>
        /// isReDrawUnSelected
        /// </summary>
        private bool isReDrawUnSelected = false;

        #endregion
    }
}