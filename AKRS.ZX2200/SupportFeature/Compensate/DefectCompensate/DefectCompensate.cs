using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Controls.Manual;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.Compensate.DefectCompensate
{
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using LanguageExt;

    /// <summary>
    /// 焊后补偿
    /// </summary>
    public static class DefectCompensate
    {
        /// <summary>
        /// 焊后数据实时曲线
        /// </summary>
        public static Dictionary<string, List<DefectStatisticsEntity>> DefectDataS { get; set; } =
            new Dictionary<string, List<DefectStatisticsEntity>>();

        /// <summary>
        /// 刷新集合
        /// </summary>
        public static Action<DefectStatisticsEntity> RePostBondInspection { get; set; } =
            new Action<DefectStatisticsEntity>(
                (defectStatisticsEntity) =>
                    {
                        if (!DefectCompensate.DefectDataS.ContainsKey(defectStatisticsEntity.DefectName))
                        {
                            DefectCompensate.DefectDataS.Add(defectStatisticsEntity.DefectName, new List<DefectStatisticsEntity>());
                        }

                        DefectCompensate.DefectDataS[defectStatisticsEntity.DefectName].Insert(0, defectStatisticsEntity);

                        if (DefectCompensate.DefectDataS[defectStatisticsEntity.DefectName].Count > 2000)
                        {
                            DefectCompensate.DefectDataS[defectStatisticsEntity.DefectName].RemoveAt(2000);
                        }
                    });

        /// <summary>
        /// 获取焊后补偿
        /// </summary>
        /// <param name="postBondInspection">焊点名称</param>
        /// <returns>结果</returns>
        public static AKRSPoint3D GetPostCompensation(PostBondInspection postBondInspection)
        {
            // 判断焊后是否存在这个检测
            if (!DefectDataS.ContainsKey(postBondInspection.Name))
            {
                return new AKRSPoint3D();
            }

            List<DefectStatisticsEntity> list = new List<DefectStatisticsEntity>(DefectDataS[postBondInspection.Name]);

            // 如果焊后数量不足则不补偿
            if (list.Count < postBondInspection.BondPostCompensationNumber)
            {
                return new AKRSPoint3D();
            }

            list = list.Take((int)postBondInspection.BondPostCompensationNumber).ToList();

            double x = list.FindAll(it => Math.Abs(it.OffsetX) < 50).Average(it => it.OffsetX) / 1000.0
                               * postBondInspection.BondPostCompensationRation
                               / 100.0;

            double y = list.FindAll(it => Math.Abs(it.OffsetY) < 50).Average(it => it.OffsetY) / 1000.0
                       * postBondInspection.BondPostCompensationRation
                       / 100.0;

            return new AKRSPoint3D(x, y, 0);
        }

        /// <summary>
        /// 归一化数据
        /// </summary>
        /// <param name="tuId">基板的Id</param>
        public static void NormalizationOffset(string tuId)
        {
            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsCompensateAutoByLast)
            {
                return;
            }

            foreach (string key in DefectDataS.Keys)
            {
                List<DefectStatisticsEntity> defect = DefectDataS[key].FindAll(it => it.Id.ToString() == tuId);

                PostBondInspection postBondInspection = BondProgram.GetInstance().PostBondProgram.PostBondInspections
                    .Find(it => it.Name == key);

                if (postBondInspection == null || postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.EpoxyCheck
                    || !postBondInspection.IsBondPostCompensation)
                {
                    continue;
                }

                if (defect.Count == 0)
                {
                    continue;
                }

                SingleBondPositionConfig singleBondPositionConfig = ProductConfiguration.GetInstance()
                    .BondPositionConfig.SingleBpPositionConfigList.Find(it => it.Name == defect[0].BondPositionName);

                if (singleBondPositionConfig != null)
                {
                    double x = defect.FindAll(it => Math.Abs(it.OffsetX) < 50).Average(it => it.OffsetX) / 1000.0
                               * ProductConfiguration.GetInstance().TransportUnitConfig.CompensateAutoByLastRation
                               / 100.0;

                    x = Math.Round(x, 3);

                    double y = defect.FindAll(it => Math.Abs(it.OffsetY) < 50).Average(it => it.OffsetY) / 1000.0
                               * ProductConfiguration.GetInstance().TransportUnitConfig.CompensateAutoByLastRation
                               / 100.0;

                    y = Math.Round(y, 3);

                    singleBondPositionConfig.BondPosOffset = new AKRSPoint3D(
                        singleBondPositionConfig.BondPosOffset.X - x,
                        singleBondPositionConfig.BondPosOffset.Y - y,
                        singleBondPositionConfig.BondPosOffset.Z);

                    if (ProductConfiguration.GetInstance().TransportUnitConfig.IsCompensateSingleBp)
                    {
                        foreach (DefectStatisticsEntity defectStatisticsEntity in defect)
                        {
                            BpOffsetSeparateInformation bpOffsetSeparateInformation = ProductConfiguration.GetInstance()
                                .OtherConfig.BpSingleSeparates.Find(
                                    it => it.SubstrateIndex == defectStatisticsEntity.SubIndex
                                          && it.ModuleIndex == defectStatisticsEntity.ModuleIndex
                                          && it.BondPositionName == defectStatisticsEntity.BondPositionName);

                            if (bpOffsetSeparateInformation == null)
                            {
                                bpOffsetSeparateInformation = new BpOffsetSeparateInformation(
                                    defectStatisticsEntity.SubIndex,
                                    defectStatisticsEntity.ModuleIndex,
                                    defectStatisticsEntity.BondPositionName,
                                    0,
                                    0,
                                    0,
                                    0);

                                ProductConfiguration.GetInstance().OtherConfig.BpSingleSeparates
                                    .Add(bpOffsetSeparateInformation);
                            }

                            if (Math.Abs(defectStatisticsEntity.OffsetX  - x) > 50
                                || Math.Abs(defectStatisticsEntity.OffsetY - y) > 50)
                            {
                                continue;
                            }


                            bpOffsetSeparateInformation.OffsetX -= (defectStatisticsEntity.OffsetX / 1000.0 - x) * ProductConfiguration.GetInstance().TransportUnitConfig.CompensateAutoByLastRation / 100.0;
                            bpOffsetSeparateInformation.OffsetY -= (defectStatisticsEntity.OffsetY / 1000.0 - y) * ProductConfiguration.GetInstance().TransportUnitConfig.CompensateAutoByLastRation / 100.0;
                        }

                        // 刷新界面数据
                        //FrmSingleBpData.ReFreshDataAction();
                    }
                }
            }

            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 获取补偿
        /// </summary>
        /// <param name="bpName">焊点名称</param>
        /// <returns>结果</returns>
        public static AKRSPoint3D GetPostBondCompensation(string bpName)
        {
            // 如果开启按基板补偿则退出，避免出现冲突
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsCompensateAutoByLast)
            {
                return new AKRSPoint3D();
            }

            List<SingleProcessStep> defectList = ProcessDomain.GetInstance().S2SystemProcess.ProcessSteps
                .FindAll(it => it.BondPositionName == bpName && it.DefectName != "Null");
           List<PostBondInspection> postBondInspections = new List<PostBondInspection>();

           foreach (SingleProcessStep singleProcessStep in defectList)
           {
               postBondInspections.Add(
                   PostBondInspectionRepository.GetInstance().GetPostBondInspection(singleProcessStep.DefectName));
           }

           List<AKRSPoint3D> point3Ds = new List<AKRSPoint3D>();

           foreach (PostBondInspection postBondInspection in postBondInspections)
           {
               if (postBondInspection.IsBondPostCompensation)
               {
                   point3Ds.Add(GetPostCompensation(postBondInspection));
               }
           }

            if (point3Ds.Count != 0)
            {
                AKRSPoint3D point3D = new AKRSPoint3D()
                {
                    X = point3Ds.Average(it => it.X),
                    Y = point3Ds.Average(it => it.Y),
                    Z = point3Ds.Average(it => it.X)
                };

                return point3D;
            }

            return new AKRSPoint3D();
        }

        /// <summary>
        /// 获取焊点的补偿
        /// </summary>
        /// <param name="bondPosition">焊点实体</param>
        /// <returns>结果</returns>
        public static AKRSPoint3D GetBpSeparateOffset(BondPosition bondPosition)
        {
            // 如果没有开启则退出
            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsCompensateAutoByLast)
            {
                return new AKRSPoint3D();
            }

            BpOffsetSeparateInformation bpOffsetSeparateInformation = ProductConfiguration.GetInstance().OtherConfig.BpSingleSeparates.Find(it => it.SubstrateIndex == bondPosition.SubstrateNum
            && it.ModuleIndex == bondPosition.ModuleNum && it.BondPositionName == bondPosition.Name);

            if (bpOffsetSeparateInformation == null)
            {
                return new AKRSPoint3D();
            }

            return new AKRSPoint3D(bpOffsetSeparateInformation.OffsetX, bpOffsetSeparateInformation.OffsetY, 0);
        }

        ///// <summary>
        ///// 获取焊后的补偿
        ///// </summary>
        ///// <param name="bondPosition">焊点实体</param>
        ///// <returns>结果</returns>
        //public static AKRSPoint3D GetDefectCompensate(BondPosition bondPosition)
        //{
        //}
    }
}
