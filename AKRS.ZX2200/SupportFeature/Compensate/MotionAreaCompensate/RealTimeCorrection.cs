using Accord.Math;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.Infrastructure.Utils;
using LanguageExt.ClassInstances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    /// <summary>
    /// 实时矫正
    /// </summary>
    public class RealTimeCorrection : Singleton<RealTimeCorrection>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RealTimeCorrection()
        {
            RealTimeCorrection.FilePath = ZX2200PathConfig.RealTimeCorrectionPath;
        }

        /// <summary>
        /// 最后一次扫描的时间
        /// </summary>
        private DateTime LastTime = DateTime.Now;

        /// <summary>
        /// 基准点位
        /// </summary>
        public AKRSPoint3D[] ReferencePoints { get; set; }

        /// <summary>
        /// 基准点位的补偿值
        /// </summary>
        public AKRSPoint3D[] ReferencePointsCompensate { get; set; }

        /// <summary>
        /// 是否示教过
        /// </summary>
        public bool IsAssistanted { get; set; } = false;

        /// <summary>
        /// 视觉名称
        /// </summary>
        public string PrName { get; set; } = "实时矫正Mark";

        /// <summary>
        /// 偏移量
        /// </summary>
        /// <param name="point3D">事件源</param>
        /// <returns>封装参数</returns>
        public AKRSPoint3D RealTimeVision(AKRSPoint3D point3D)
        {
            if (ReferencePoints == null)
            {
                throw new ArgumentNullException("实时补偿没有数据");
            }

            if (point3D == null)
            {
                throw new ArgumentNullException("实时补偿传入点位为空");
            }

            #region 获取最近的两个点

            // 寻找离坐标最近的两个点
            AKRSPoint3D point3DRight = (AKRSPoint3D)ReferencePoints.Find(it => it.X > point3D.X);

            if (point3DRight == null)
            {
                throw new ArgumentNullException("实时补偿传入数据X过大，未找到补偿点位");
            }

            // 索引
            int index = ReferencePoints.IndexOf(point3DRight);

            if (index == 0)
            {
                throw new ArgumentNullException("实时补偿传入数据X过小，未找到补偿点位");
            }

            AKRSPoint3D point3DLeft = ReferencePoints[index - 1];

            #endregion

            #region 定位

            MatchResult matchResultRight = (MatchResult)System2Domain.GetInstance().System2CommonVision(point3DRight, PrName, PrName);

            if (matchResultRight == null)
            {
                throw new Exception("实时补偿定位失败");
            }

            AKRSPoint3D posRight = System2Domain.GetInstance().BondModuleController.ConvertPixelToG0Pos(System2Domain.GetInstance().BondModuleController.Get3DRealPosition(), matchResultRight);

            MatchResult matchResultLeft = (MatchResult)System2Domain.GetInstance().System2CommonVision(point3DLeft, PrName, PrName);

            if (matchResultLeft == null)
            {
                throw new Exception("实时补偿定位失败");
            }

            AKRSPoint3D posLeft = System2Domain.GetInstance().BondModuleController.ConvertPixelToG0Pos(System2Domain.GetInstance().BondModuleController.Get3DRealPosition(), matchResultLeft);

            #endregion

            #region 计算结果

            double distance = point3DRight.X - point3DLeft.X;

            // 右边比例
            double rightRatio = 1 - (point3DRight.X - point3D.X) / distance;

            // 左边比例
            double leftRatio = 1 - (point3D.X - point3DLeft.X) / distance;

            // 总的补偿值
            AKRSPoint3D compensate = (posRight - point3DRight) * rightRatio + (posLeft - point3DLeft) * leftRatio;

            if (Math.Abs(compensate.X) > 0.5 || Math.Abs(compensate.Y) > 0.5)
            {
                throw new ArgumentNullException("实时补偿获取数据过大，请检查标定点是否发生改变");
            }

            return compensate;

            #endregion
        }


        /// <summary>
        /// 获取补偿值
        /// </summary>
        /// <param name="iptPickPos">中转台取片位置</param>
        /// <param name="iptVisionPos">中转台视觉位置</param>
        /// <param name="bondPos">贴片位置</param>
        /// <param name="bondVisonPos">焊后位置</param>
        /// <returns></returns>
        public AKRSPoint3D GetCompensatePoint3D(AKRSPoint3D iptPickPos, AKRSPoint3D iptVisionPos, AKRSPoint3D bondPos, AKRSPoint3D bondVisonPos) 
        {
            //return new AKRSPoint3D(/*compensate.X*/0, 0, 0);

            try
            {
                System2RunTimeProvider.RecordTime("BondAction", "获取补偿值开始 ----------------");

                if (!System2Configuration.GetInstance().IsRealTimeCompensate)
                {
                    return new AKRSPoint3D();
                }

                if (iptPickPos == null || iptVisionPos == null || bondPos == null || bondVisonPos == null)
                {
                    throw new ArgumentNullException("实时补偿矫正传入数据为空");
                }

                if (this.ReferencePoints == null || this.ReferencePointsCompensate == null)
                {
                    throw new ArgumentNullException("实时补偿矫正本地数据为空");
                }

                if (!this.IsAssistanted)
                {
                    throw new ArgumentNullException("实时补偿矫正未示教");
                }

                AKRSPoint3D iptPickPosCompensate = this.GetCompensatePosByPoint(iptPickPos);
                AKRSPoint3D iptVisionPosCompensate = this.GetCompensatePosByPoint(iptVisionPos);
                AKRSPoint3D bondPosCompensate = this.GetCompensatePosByPoint(bondPos);
                AKRSPoint3D bondVisonPosCompensate = this.GetCompensatePosByPoint(bondVisonPos);

                AKRSPoint3D compensate = (iptPickPosCompensate - iptVisionPosCompensate) - (bondPosCompensate - bondVisonPosCompensate);

                if (Math.Abs(compensate.X) > 0.5 || Math.Abs(compensate.Y) > 0.5)
                {
                    return new AKRSPoint3D(/*compensate.X*/0, 0, 0);
                }

                //return compensate;

                System2RunTimeProvider.RecordTime("BondAction", "获取补偿值结束 ----------------");

                return new AKRSPoint3D(compensate.X, 0, 0);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// 获取补偿值
        /// </summary>
        /// <param name="point3D">原始点位</param>
        /// <returns>补偿值</returns>
        private AKRSPoint3D GetCompensatePosByPoint(AKRSPoint3D point3D)
        {
            #region 获取最近的两个点

            AKRSPoint3D point3DRight = null;

            if (point3D.X >= this.ReferencePoints.Max(it => it.X))
            {
                point3DRight = this.ReferencePoints[this.ReferencePoints.Length - 1];
            }
            else
            {
                // 寻找离坐标最近的两个点
                point3DRight = (AKRSPoint3D)this.ReferencePoints.Find(it => it.X > point3D.X);
            }

            if (point3DRight == null)
            {
                throw new ArgumentNullException("实时补偿传入数据X过大，未找到补偿点位");
            }

            // 索引
            int index = this.ReferencePoints.IndexOf(point3DRight);

            if (index == 0)
            {
                throw new ArgumentNullException("实时补偿传入数据X过小，未找到补偿点位");
            }

            AKRSPoint3D point3DLeft = this.ReferencePoints[index - 1];

            AKRSPoint3D posRight = this.ReferencePointsCompensate[index];

            AKRSPoint3D posLeft = this.ReferencePointsCompensate[index - 1];

            #endregion

            #region 计算结果

            double distance = point3DRight.X - point3DLeft.X;

            // 右边比例
            double rightRatio = 1 - (point3DRight.X - point3D.X) / distance;

            // 左边比例
            double leftRatio = 1 - (point3D.X - point3DLeft.X) / distance;

            // 总的补偿值
            AKRSPoint3D compensate = (posRight - point3DRight) * rightRatio + (posLeft - point3DLeft) * leftRatio;

            if (Math.Abs(compensate.X) > 0.5 || Math.Abs(compensate.Y) > 0.5)
            {
                throw new ArgumentNullException("实时补偿获取数据过大，请检查标定点是否发生改变");
            }

            return compensate;

            #endregion
        }

        /// <summary>
        /// 扫描
        /// </summary>
        public bool ScanReferencePoints(bool first = false)
        {
            if (!System2Configuration.GetInstance().IsRealTimeCompensate)
            {
                return true;
            }

            List<List<double>> doubles = new List<List<double>>();

            if (first /*|| (DateTime.Now - this.LastTime).Minutes > System2Configuration.GetInstance().IsRealTimeCompensateTime*/)
            {
                AKRSPoint3D[] point3Ds = new AKRSPoint3D[this.ReferencePoints.Length];

                for (int i = 0; i < this.ReferencePoints.Length; i++)
                {
                    MatchResult matchResultRight = (MatchResult)System2Domain.GetInstance().System2CommonVision(this.ReferencePoints[i], PrName, PrName);

                    if (matchResultRight == null)
                    {
                        return false;
                    }

                    AKRSPoint3D posCurrent = System2Domain.GetInstance().BondModuleController.ConvertPixelToG0Pos(System2Domain.GetInstance().BondModuleController.Get3DRealPosition(), matchResultRight);

                    point3Ds[i] = posCurrent;

                    doubles.Add(new List<double>() { matchResultRight.CenterX, matchResultRight.CenterY });
                }

                this.ReferencePointsCompensate = point3Ds;

                this.Save();

                this.LastTime = DateTime.Now;

                string path = FileHelper.CreateFileByDate("标定尺子数据");

                FileHelper.SaveDoubleExcel(doubles, Path.Combine(path, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff") + ".xlsx"));
            }

            return true;
        }

        /// <summary>
        /// 扫描
        /// </summary>
        public void FirstScanReferencePoints(AKRSPoint3D[] pos)
        {
            AKRSPoint3D[] point3Ds = new AKRSPoint3D[pos.Length];

            for (int i = 0; i < pos.Length; i++)
            {
                MatchResult matchResultRight = (MatchResult)System2Domain.GetInstance().System2CommonVision(pos[i], PrName, PrName);

                if (matchResultRight == null)
                {
                    throw new Exception("实时补偿定位失败");
                }

                AKRSPoint3D posCurrent = System2Domain.GetInstance().BondModuleController.ConvertPixelToG0Pos(System2Domain.GetInstance().BondModuleController.Get3DRealPosition(), matchResultRight);

                point3Ds[i] = posCurrent;
            }

            this.ReferencePoints = point3Ds;

            this.Save();
        }
    }
}
