using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRSAKRS.Galaxy2.PR.Models.Algs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.PR.Models.Algs
{
    /// <summary>
    /// 算法工厂
    /// </summary>
    public class AlgsFactory
    {
        /// <summary>
        /// 创建方法模型
        /// </summary>
        /// <param name="algFlowType">算法类型</param>
        /// <param name="name">算法名称</param>
        /// <returns>算法对象</returns>
        public static BaseAlg Create(AlgFlowTypeEnum algFlowType, string name)
        {
            switch (algFlowType)
            {
                case AlgFlowTypeEnum.FastModelAlg:
                    return new FastModelAlg(name);

                case AlgFlowTypeEnum.HighPreModelAlg:
                    return new HighPreModelAlg(name);

                case AlgFlowTypeEnum.NccModelAlg:
                    return new NccModelAlg(name);

                case AlgFlowTypeEnum.XldModelAlg:
                    return new XldModelAlg(name);

                case AlgFlowTypeEnum.PreventWrongAlg:
                    return new PreventWrongAlg(name);

                case AlgFlowTypeEnum.PreventReverseAlg:
                    return new PreventReverseAlg(name);

                case AlgFlowTypeEnum.PostBondDetectAlg:
                    return new PostBondDetectAlg(name);

                case AlgFlowTypeEnum.EpoxyDetectAlg:
                    return new EpoxyDetectAlg(name);

                case AlgFlowTypeEnum.InkDotDetectAlg:
                    return new InkDotDetectAlg(name);

                case AlgFlowTypeEnum.CornerDetectAlg:
                    return new CornerDetectAlg(name);

                case AlgFlowTypeEnum.RectangleDetectAlg:
                    return new RectangleDetectAlg(name);

                case AlgFlowTypeEnum.CircleFindAlg:
                    return new CircleFindAlg(name);

                case AlgFlowTypeEnum.FocusMeasureAlg:
                    return new FocusAlg(name);

                case AlgFlowTypeEnum.CenterSearchAlg:
                    return new CenterSearchAlg(name);

                case AlgFlowTypeEnum.CrossSearchAlg:
                    return new CrossSearchAlg(name);

                case AlgFlowTypeEnum.FiducialFindAlg:
                    return new FiducialFindAlg(name);

                case AlgFlowTypeEnum.LineDetectAlg:
                    return new LineDetectAlg(name);

                case AlgFlowTypeEnum.HighPreModelAlgWithRefer:
                    return new HighPreModelAlgWithRefer(name);

                case AlgFlowTypeEnum.HighPreModelAlgWithBlob:
                    return new HighPreModelAlgWithBlob(name);

                case AlgFlowTypeEnum.DistanceMeasureAlg:
                    return new DistanceMeasure(name);

                case AlgFlowTypeEnum.RectangleSecondDetectAlg:
                    return new RectangleSecondDetectAlg(name);

                case AlgFlowTypeEnum.SymmetricModeleAlg:
                    return new SymmetricModeleAlg(name);

                case AlgFlowTypeEnum.HighPreModelAlgWithCorner:
                    return new HighPreModelAlgWithCorner(name);

                case AlgFlowTypeEnum.HybridPositionAlg:
                    return new HybridPositionAlg(name);

                case AlgFlowTypeEnum.FcUplookModelAlg:
                    return new FcUplookModelAlg(name);

                case AlgFlowTypeEnum.FcEdgeBreakDetectAlg:
                    return new FcEdgeBreakDetectAlg(name);
                case AlgFlowTypeEnum.SurfaceDetectAlg:
                    return new SurfaceDetectAlg(name);

                case AlgFlowTypeEnum.EdgeBreakDetectAlg:
                    return new EdgeBreakDetectAlg(name);

                case AlgFlowTypeEnum.QrCodeDetectAlg:
                    return new QrCodeDetectAlg(name);

                case AlgFlowTypeEnum.CircleAndLineAlg:
                    return new CircleAndLineAlg(name);

                default: 
                    return new FastModelAlg(name);
            }
        }
    }
}
