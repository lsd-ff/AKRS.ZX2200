using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;

namespace AKRS.ZX2200.TransportUnitSystem.Model
{
    using System.Drawing;

    /// <summary>
    /// 视觉定位的结果
    /// </summary>
    public class VisionResult
    {
        /// <summary>
        /// 定位是否成功
        /// </summary>
        public bool IsVisionSuccess { get; set; } = false;

        /// <summary>
        /// 定位结果
        /// </summary>
        public MatchResult MatchResult { get; set; }

        /// <summary>
        /// 定位结果在自己坐标系的坐标
        /// </summary>
        public AKRSPoint3D ResultPoint3D { get; set; }

        /// <summary>
        /// 定位结果在G0中的坐标
        /// </summary>
        public AKRSPoint3D ResultPoint3DInG0 { get; set; }

        /// <summary>
        /// G0中的拍照位
        /// </summary>
        public AKRSPoint3D VisionPoint3DInG0 { get; set; }

        /// <summary>
        /// 拍照时的点位(机械坐标系)
        /// </summary>
        public AKRSPoint3D VisionPos { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap Bitmap { get; set; }

        /// <summary>
        /// 是否更新过
        /// </summary>
        public bool IsUpData { get; set; } = false;

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="matchResult">定位结果</param>
        /// <param name="resultPoint3D">定位结果在自己坐标系的坐标</param>
        /// <param name="resultPoint3DInG0">定位结果在G0中的坐标</param>
        /// <param name="visionPoint3DInG0">G0中的拍照位</param>
        public void SaveInfo(MatchResult matchResult, AKRSPoint3D resultPoint3D, AKRSPoint3D resultPoint3DInG0, AKRSPoint3D visionPoint3DInG0)
        {
            this.MatchResult = new MatchResult(matchResult.CenterX, matchResult.CenterY, matchResult.Angle);
            this.ResultPoint3D = resultPoint3D;
            this.ResultPoint3DInG0 = resultPoint3DInG0;
            this.VisionPoint3DInG0 = visionPoint3DInG0;
            this.IsVisionSuccess = true;
            this.IsUpData = true;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="visionPoint3DInMachine">机械位置</param>
        /// <param name="bitmap">获取的图片</param>
        public void SaveInfo(AKRSPoint3D visionPoint3DInMachine, Bitmap bitmap)
        {
            this.VisionPos = visionPoint3DInMachine;
            this.Bitmap = bitmap;
        }
    }
}
