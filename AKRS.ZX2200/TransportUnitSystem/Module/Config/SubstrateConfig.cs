using AKRS.ZX2200.TransportUnitSystem.Model;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using System.Collections.Generic;

    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.TransportUnitSystem.Service;

    public class SubstrateConfig : ArrayConfig
    {
        /// <summary>
        /// 墨点检测
        /// </summary>
        [TreeProgramListArgs("墨点检测", (string)null)]
        public bool SubstrateInkDot { get; set; }

        /// <summary>
        /// 对称排列
        /// </summary>
        [TreeProgramListArgs("对称排列", (string)null)]
        public bool IsSymmetric { get; set; }

        /// <summary>
        /// 常见数据类型
        /// </summary>
        // [TreeProgramListArgs("IsSymmetric", (string)null)]
        public bool CommonDataSet { get; set; }

        /// <summary>
        /// 编辑状态
        /// </summary>
        // [TreeProgramListArgs("EditState", (string)null)]
        public EditStateEnum EditState { get; set; } = EditStateEnum.NewCreate;

        /// <summary>
        /// ID搜索的数量
        /// </summary>
        [TreeProgramListArgs("ID搜索的数量", (string)null)]
        public int IdSearchNumber { get; set; }

        /// <summary>
        /// 预搜索
        /// </summary>
        [TreeProgramListArgs("预搜索", (string)null)]
        public bool IdPreSearch { get; set; }

        /// <summary>
        /// 是否启用mapping
        /// </summary>
        [TreeProgramListArgs("是否启用mapping", (string)null)]
        public bool MappingEnable { get; set; }

        /// <summary>
        /// 正常的
        /// </summary>
        [TreeProgramListArgs("排列方式", (string)null)]
        public ArrangementEnum Arrangement { get; set; } = ArrangementEnum.Normal;

        /// <summary>
        /// 工作方向
        /// </summary>
        [TreeProgramListArgs("工作方向", (string)null)]
        public WorkOrderEnum WorkOrderEnum { get; set; } = WorkOrderEnum.LeftDownToRight;

        /// <summary>
        /// 获取点胶分段
        /// </summary>
        /// <returns>索引的集合</returns>
        public List<int> GetFrontSubstrateIndex()
        {
            List<int> substrateIndex = new List<int>();
            
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                for (int i = 0; i < ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.DownConfigs.Count; i++)
                {
                    if (ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.DownConfigs[i].ElementCoordinate.Point.X <= 0)
                    {
                        substrateIndex.Add(i + 1);
                    }
                }
            }
            else
            {
                List<ElementCoordinate> lists = this.ElementCoordinates.GetRange(0, this.ElementCoordinates.Count);

                // 找到所有坐标系的中位数
                lists.Sort((item1, item2) => item1.Point.X.CompareTo(item2.Point.X));

                double divide = lists[lists.Count / 2].Point.X;

                for (int i = 0; i < this.ElementCoordinates.Count; i++)
                {
                    if (this.ElementCoordinates[i].Point.X < divide)
                    {
                        substrateIndex.Add(i + 1);
                    }
                }

                if (this.Arrangement == ArrangementEnum.Snake)
                {
                    substrateIndex = TuService.ToSnakeList(substrateIndex, this.ColumnCount);
                }
            }

            return substrateIndex;
        }


        /// <summary>
        /// 获取点胶分段
        /// </summary>
        /// <returns>索引的集合</returns>
        public List<int> GetBackSubstrateIndex()
        {
            List<int> list = new List<int>();

            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                for (int i = 0; i < ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.DownConfigs.Count; i++)
                {
                    if (ProductConfiguration.GetInstance().OppositeSexConfiguration.OppositeSexConfigs.DownConfigs[i].ElementCoordinate.Point.X > 0)
                    {
                        list.Add(i + 1);
                    }
                }
            }
            else
            {
                List<int> frontList = this.GetFrontSubstrateIndex();

                for (int i = 0; i < this.ElementCoordinates.Count; i++)
                {
                    if (!frontList.Contains(i + 1))
                    {
                        list.Add(i + 1);
                    }
                }

                if (this.Arrangement == ArrangementEnum.Snake)
                {
                    list = TuService.ToSnakeList(list, this.ColumnCount);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取sub的序列号
        /// </summary>
        /// <returns>序列号</returns>
        public List<int> GetSubstrateIndex()
        {
            // 集合
            List<int> list = new List<int>();

            for (int i = 0; i < list.Count; i++)
            {
                list.Add(i);
            }

            if (this.Arrangement == ArrangementEnum.Snake)
            {
                list = TuService.ToSnakeList(list, this.ColumnCount);
            }

            return list;
        }
    }
}
