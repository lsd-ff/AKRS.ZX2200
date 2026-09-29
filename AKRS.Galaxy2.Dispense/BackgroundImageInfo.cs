using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 背景图片的信息。
    /// </summary>
	public class BackgroundImageInfo
	{
        Bitmap _image;

        float[] _matrixElements;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image">null表示没有图片。</param>
        public BackgroundImageInfo()
        {
            _image = null;

            _matrixElements = new float[6];
            _matrixElements[0] = 1;
            _matrixElements[1] = 0;
            _matrixElements[2] = 0;
            _matrixElements[3] = 1;
            _matrixElements[4] = 0;
            _matrixElements[5] = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image">null表示没有图片。</param>
        /// <param name="matrixElements">数组的长度必须是6。</param>
        public BackgroundImageInfo(Bitmap image, float[] matrixElements)
            : this()
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image));
            }

            if (matrixElements == null)
            {
                throw new ArgumentNullException(nameof(matrixElements));
            }

            if (matrixElements.Length != 6)
            {
                throw new ArgumentException(nameof(matrixElements));
            }

            _image = image;

            for (int i = 0; i < _matrixElements.Length; i++)
            {
                _matrixElements[i] = matrixElements[i];
            }
        }

		public BackgroundImageInfo(BackgroundImageInfo other)
            : this()
		{
			Set(other);
		}

		public void Set(BackgroundImageInfo other)
		{
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            _image = other._image;

            for (int i = 0; i < 6; i++)
            {
                _matrixElements[i] = other._matrixElements[i];
            }
		}

		public Bitmap Image
		{
			set { _image = value; }
			get { return _image; }
		}

        public float[] MatrixElements
        {
            get { return _matrixElements; }
            set
            {
                for (int i = 0; i < 6; i++)
                {
                    _matrixElements[i] = value[i];
                }
            }
        }
    }
}
