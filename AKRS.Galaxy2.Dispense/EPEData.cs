using System;
using System.Collections.Generic;
using System.Drawing;
using AKRS.Galaxy2.Dispense;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 点胶数据
    /// </summary>
	public class EPEData
	{
        /// <summary>
        /// 像素比
        /// </summary>
        public float MicronPerPixel { get; set; }

        /// <summary>
        /// Die width
        /// </summary>
        public float DieWidth { get; set; }

        /// <summary>
        /// DieHeight
        /// </summary>
        public float DieHeight { get; set; }

        /// <summary>
        /// 点的列表，其中包含点的坐标和点的信息。
        /// </summary>
        public List<EPEPointData[]> EpePointList { get; set; }

        /// <summary>
        /// 图像信息。
        /// </summary>
		public BackgroundImageInfo BackgroundImageInfo { get; set; }

        /// <summary>
        /// 点的速度等配置
        /// </summary>
        public EndpointSetting DefaultInfo { get; set; }

        /// <summary>
        /// </summary>
        /// <param name="micronPerPixel">像素比
        /// </param>
        /// <param name="dieWidth"> Die Width
        /// </param>
        /// <param name="dieHeight"> Die Height
        /// </param>
        /// <param name="list">
        /// 点集合 可以为null。
        /// </param>
        /// <param name="image">
        /// 背景图片 可以为null。
        /// </param>
        /// <param name="imageMatrixElements">
        /// The image Matrix Elements.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
		public EPEData(
            float micronPerPixel,
            float dieWidth,
            float dieHeight,
            List<EPEPointData[]> list,
            Bitmap image,
            /*
            float imageOffsetX,
            float imageOffsetY,
            float imageRotationDegree
            */
            float[] imageMatrixElements)
		{
            if (micronPerPixel <= 0)
            {
                throw new ArgumentOutOfRangeException($"the value of \"{micronPerPixel}\" must be positive");
            }

            if (dieWidth <= 0)
            {
                throw new ArgumentOutOfRangeException($"the value of \"{dieWidth}\" must be positive");
            }

            if (dieHeight <= 0)
            {
                throw new ArgumentOutOfRangeException($"the value of \"{dieHeight}\" must be positive");
            }

            this.MicronPerPixel = micronPerPixel;
            this.DieWidth = dieWidth;
            this.DieHeight = dieHeight;
			
			// deep copy.
			if (list != null)
			{
				this.EpePointList = new List<EPEPointData[]>(list.Count);
				foreach (EPEPointData[] elem in list)
				{
					EPEPointData[] points = new EPEPointData[elem.Length];
					Array.Copy(elem, points, points.Length);
                    this.EpePointList.Add(points);
				}
			}
			else
			{
                this.EpePointList = new List<EPEPointData[]>();
			}

            // BackgroundImageInfo = new BackgroundImageInfo(image, imageOffsetX, imageOffsetY, imageRotationDegree);
            if (imageMatrixElements == null)
            {
                this.BackgroundImageInfo = new BackgroundImageInfo(image, new float[] { 1, 0, 0, 1, 0, 0 });
            }
            else
            {
                this.BackgroundImageInfo = new BackgroundImageInfo(image, imageMatrixElements);
            }
		}

        //public EPEData(float micronPerPixel, float dieWidth, float dieHeight, List<EPEPoint[]> list)
        //          : this(micronPerPixel, dieWidth, dieHeight, list, null, null)
        //{}

        /// <summary>
        /// 设置 defaultInfo
        /// </summary>
        /// <param name="defaultInfo">defaultInfo</param>
        public void SetDefaultValues(EndpointSetting defaultInfo)
		{
			this.DefaultInfo = defaultInfo;
		}
    }
}
