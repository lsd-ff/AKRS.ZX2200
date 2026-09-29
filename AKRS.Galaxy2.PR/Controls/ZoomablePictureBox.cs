using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.Galaxy2.PR.Controls
{
    public partial class ZoomablePictureBox : UserControl
    {
        private Image _image;
        private float _zoomFactor = 1.0f;
        private const float ZoomIncrement = 0.1f;
        private const float MinZoom = 0.1f;
        private const float MaxZoom = 10.0f;
        private PointF _imagePosition = PointF.Empty;
        private bool _isDragging = false;
        private Point _lastMousePosition;

        // Helper to detect disposed image safely
        private bool TryGetImageSize(out int width, out int height)
        {
            width = 0;
            height = 0;
            if (_image == null) return false;
            try
            {
                width = _image.Width;
                height = _image.Height;
                return true;
            }
            catch (ObjectDisposedException)
            {
                // Image has been disposed elsewhere - clear reference and treat as no image
                _image = null;
                return false;
            }
            catch
            {
                // Any other unexpected error treat as no image
                _image = null;
                return false;
            }
        }

        public Image Image
        {
            get => _image;
            set
            {
                // If incoming image is already disposed, ignore and keep image null
                if (value == null)
                {
                    _image = null;
                }
                else
                {
                    try
                    {
                        // Access a property to ensure it's not disposed
                        var w = value.Width;
                        var h = value.Height;
                        _image = value;
                    }
                    catch (ObjectDisposedException)
                    {
                        _image = null;
                    }
                }

                _zoomFactor = 1f;
                CenterImage();
                Invalidate();
            }
        }

        public ZoomablePictureBox()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.MouseWheel += OnMouseWheel;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!TryGetImageSize(out var imgW, out var imgH)) return;

            var scaledSize = new Size((int)(imgW * _zoomFactor), (int)(imgH * _zoomFactor));
            var destRect = new Rectangle((int)_imagePosition.X, (int)_imagePosition.Y, scaledSize.Width, scaledSize.Height);

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            try
            {
                e.Graphics.DrawImage(_image, destRect);
            }
            catch (ObjectDisposedException)
            {
                // Image disposed between the check and draw - clear and skip
                _image = null;
            }
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            var zoomChange = e.Delta > 0 ? ZoomIncrement : -ZoomIncrement;
            var newZoomFactor = Math.Max(MinZoom, Math.Min(MaxZoom, _zoomFactor + zoomChange));

            if (Math.Abs(newZoomFactor - _zoomFactor) > 0.01f)
            {
                if (!TryGetImageSize(out var imgW, out var imgH)) return;

                // avoid divide by zero
                if (imgW == 0 || imgH == 0) return;

                var mousePos = this.PointToClient(MousePosition);
                var offsetX = (mousePos.X - _imagePosition.X) / (imgW * _zoomFactor);
                var offsetY = (mousePos.Y - _imagePosition.Y) / (imgH * _zoomFactor);

                _zoomFactor = newZoomFactor;

                _imagePosition.X = mousePos.X - offsetX * (imgW * _zoomFactor);
                _imagePosition.Y = mousePos.Y - offsetY * (imgH * _zoomFactor);

                Invalidate();
            }
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                _lastMousePosition = e.Location;
                Cursor = Cursors.Hand;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging)
            {
                var deltaX = e.X - _lastMousePosition.X;
                var deltaY = e.Y - _lastMousePosition.Y;

                _imagePosition.X += deltaX;
                _imagePosition.Y += deltaY;

                _lastMousePosition = e.Location;

                Invalidate();
            }
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _isDragging = false;
                Cursor = Cursors.Default;
            }
        }

        private void CenterImage()
        {
            if (!TryGetImageSize(out var imgW, out var imgH))
            {
                // no image, center to control
                _imagePosition = PointF.Empty;
                Invalidate();
                return;
            }

            var scaledSize = new Size((int)(imgW * _zoomFactor), (int)(imgH * _zoomFactor));
            _imagePosition.X = (ClientSize.Width - scaledSize.Width) / 2f;
            _imagePosition.Y = (ClientSize.Height - scaledSize.Height) / 2f;

            Invalidate();
        }
    }
}
