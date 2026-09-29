using System.Collections.Generic;
using System.Linq;

namespace AKRS.ZX2200.CalibSystem.Models
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.ZX2200.Infrastructure.Models.Path;
    using System;

    /// <summary>
    /// 光源标定参数类
    /// </summary>
    public class LightCalibrationPara : Singleton<LightCalibrationPara>
    {
        // 增：定义全量映射字典（按LightType分组）
        // 唯一基准：Bond→Dispense 正向字典（Key=LightType，Value=0-255全量字典）
        private Dictionary<LightType, Dictionary<int, int>> _bondToDispenseFullMap;
        // 反向绑定：Dispense→Bond 字典（基于正向字典构建，保证双向一致）
        private Dictionary<LightType, Dictionary<int, int>> _dispenseToBondFullMap;
        // 增：标记字典是否已初始化，避免重复计算
        private bool _isFullMapInited = false;

        public LightCalibrationPara()
        {
            Singleton<LightCalibrationPara>.FilePath = ZX2200PathConfig.LightCalibratePara;
            // 初始化字典容器
            _bondToDispenseFullMap = new Dictionary<LightType, Dictionary<int, int>>();
            _dispenseToBondFullMap = new Dictionary<LightType, Dictionary<int, int>>();
        }

        private Dictionary<int, int> GenerateDispenseToBondFullDict(Dictionary<int, int> bondToDispense, LightType lightType)
        {
            var dict = new Dictionary<int, int>();

            // 第一步：获取原始映射表用于插值计算
            var originalMap = GetMapByLightType(lightType);

            // 第二步：生成Dispense到Bond的反向查找表
            // 使用线性插值算法，确保反向映射的精确性

            // 首先处理Dispense=0的特殊情况
            dict[0] = 0;

            // 为每个可能的Dispense值(0-255)计算对应的Bond值
            for (int dispense = 1; dispense <= 255; dispense++)
            {
                dict[dispense] = CalculateBondFromDispense(dispense, originalMap);
            }

            return dict;
        }

        /// <summary>
        /// 根据Dispense值计算对应的Bond值（反向映射）
        /// </summary>
        private int CalculateBondFromDispense(int dispense, List<(int bond, int dispense)> originalMap)
        {
            if (dispense == 0) return 0;

            // 确保映射表有序
            var sortedMap = originalMap.OrderBy(x => x.dispense).ToList();

            // 边界检查
            if (dispense <= sortedMap[0].dispense)
                return sortedMap[0].bond;

            if (dispense >= sortedMap.Last().dispense)
                return sortedMap.Last().bond;

            // 在原始映射表中查找包含目标Dispense的区间
            for (int i = 0; i < sortedMap.Count - 1; i++)
            {
                var point1 = sortedMap[i];
                var point2 = sortedMap[i + 1];

                if (dispense >= point1.dispense && dispense <= point2.dispense)
                {
                    // 使用线性插值计算Bond值
                    double ratio = (double)(dispense - point1.dispense) / (point2.dispense - point1.dispense);
                    double bond = point1.bond + ratio * (point2.bond - point1.bond);

                    // 四舍五入到最接近的整数
                    int roundedBond = (int)Math.Round(bond);

                    // 确保结果在0-255范围内
                    return Math.Max(0, Math.Min(255, roundedBond));
                }
            }

            // 如果找不到区间，返回最后一个点的Bond值
            return sortedMap.Last().bond;
        }

        /// <summary>
        /// 重新设计初始化方法，确保正向和反向映射的一致性
        /// </summary>
        public void InitFullMappingDictionaries()
        {
            if (_isFullMapInited) return;

            foreach (LightType lightType in Enum.GetValues(typeof(LightType)))
            {
                // 1. 生成精确的正向映射
                var bondToDispense = new Dictionary<int, int>();
                for (int bond = 0; bond <= 255; bond++)
                {
                    bondToDispense[bond] = CalculateDispenseIntensityRaw(lightType, bond);
                }

                // 2. 生成精确的反向映射
                var dispenseToBond = GenerateDispenseToBondFullDict(bondToDispense, lightType);

                // 3. 验证映射一致性
                ValidateAndFixMappings(bondToDispense, dispenseToBond, lightType);

                _bondToDispenseFullMap[lightType] = bondToDispense;
                _dispenseToBondFullMap[lightType] = dispenseToBond;
            }

            _isFullMapInited = true;
        }

        /// <summary>
        /// 验证和修复映射，确保双向一致性
        /// </summary>
        private void ValidateAndFixMappings(Dictionary<int, int> bondToDispense,
                                          Dictionary<int, int> dispenseToBond,
                                          LightType lightType)
        {
            bool hasIssues = false;

            // 验证1: 检查反向映射是否能正确映射回原始值
            for (int bond = 0; bond <= 255; bond++)
            {
                int dispense = bondToDispense[bond];
                int reverseBond = dispenseToBond[dispense];

                if (reverseBond != bond)
                {
                    // 记录不一致
                    hasIssues = true;
                    Console.WriteLine($"警告: {lightType} 映射不一致 - Bond={bond} -> Dispense={dispense}, 反向Bond={reverseBond}");
                }
            }

            // 如果有问题，可以采取修复措施
            if (hasIssues)
            {
                // 方法1: 使用更精确的反向计算重新生成
                var originalMap = GetMapByLightType(lightType);
                var newDispenseToBond = new Dictionary<int, int>();

                for (int dispense = 0; dispense <= 255; dispense++)
                {
                    newDispenseToBond[dispense] = CalculateBondFromDispense(dispense, originalMap);
                }

                // 替换原始字典
                dispenseToBond.Clear();
                foreach (var kvp in newDispenseToBond)
                {
                    dispenseToBond[kvp.Key] = kvp.Value;
                }
            }
        }

        /// <summary>
        /// 优化后的正向转换方法
        /// </summary>
        public int GetDispenseIntensity(LightType lightType, int bondIntensity)
        {
            EnsureMappingsInitialized();

            if (bondIntensity < 0 || bondIntensity > 255)
                return bondIntensity;

            if (_bondToDispenseFullMap.TryGetValue(lightType, out var dict) &&
                dict.TryGetValue(bondIntensity, out var val))
            {
                return val;
            }

            // 回退到原始计算
            return CalculateDispenseIntensityRaw(lightType, bondIntensity);
        }

        /// <summary>
        /// 优化后的反向转换方法
        /// </summary>
        public int GetBondIntensity(LightType lightType, int dispenseIntensity)
        {
            EnsureMappingsInitialized();

            if (dispenseIntensity < 0 || dispenseIntensity > 255)
                return dispenseIntensity;

            if (_dispenseToBondFullMap.TryGetValue(lightType, out var dict) &&
                dict.TryGetValue(dispenseIntensity, out var val))
            {
                return val;
            }

            // 回退到反向计算
            var originalMap = GetMapByLightType(lightType);
            return CalculateBondFromDispense(dispenseIntensity, originalMap);
        }

        /// <summary>
        /// 确保映射已初始化
        /// </summary>
        private void EnsureMappingsInitialized()
        {
            if (!_isFullMapInited)
            {
                InitFullMappingDictionaries();
            }
        }


        // 原有方法保留（仅微调：注释掉排序，保持原有逻辑）
        /// <summary>
        /// 原始正向计算（仅用于预生成字典，包含四舍五入）
        /// </summary>
        private int CalculateDispenseIntensityRaw(LightType lightType, int bondIntensity)
        {
            if (bondIntensity == 0) return 0;

            var map = GetMapByLightType(lightType);
            if (map.Count == 0) return bondIntensity;

            // 原有排序逻辑注释保留，不改动
            //if (!IsMapSorted(map, true))
            //    map = map.OrderBy(x => x.bond).ToList();

            if (bondIntensity <= map[0].bond) return map[0].dispense;
            if (bondIntensity >= map.Last().bond) return map.Last().dispense;

            for (int i = 0; i < map.Count - 1; i++)
            {
                if (bondIntensity >= map[i].bond && bondIntensity <= map[i + 1].bond)
                {
                    double ratio = (double)(bondIntensity - map[i].bond) / (map[i + 1].bond - map[i].bond);
                    double dispense = map[i].dispense + ratio * (map[i + 1].dispense - map[i].dispense);
                    return (int)Math.Round(dispense); // 统一四舍五入
                }
            }

            return map.Last().dispense;
        }

        // 删：原有CalculateBondIntensityRaw方法（反向字典改为基于正向字典构建，不再独立插值）

        // 原有方法保留
        /// <summary>
        /// 补全：根据光源类型获取对应映射表（替代重复的switch逻辑）
        /// </summary>
        /// <param name="lightType">光源类型</param>
        /// <returns>对应的映射表</returns>
        private List<(int bond, int dispense)> GetMapByLightType(LightType lightType)
        {
            return lightType switch
            {
                LightType.RedRing => RedRingLightMap,
                LightType.BlueRing => BlueRingLightMap,
                LightType.GreenRing => GreenRingLightMap,
                LightType.RedSpot => RedSpotLightMap,
                LightType.BlueSpot => BlueSpotLightMap,
                LightType.GreenSpot => GreenSpotLightMap,
                _ => new List<(int, int)>()
            };
        }

        //// 改：对外正向转换方法（增加字典初始化兜底，避免空引用）
        ///// <summary>
        ///// 对外暴露：正向转换（直接查表，无动态计算）
        ///// </summary>
        //public int GetDispenseIntensity(LightType lightType, int bondIntensity)
        //{
        //    // 兜底：若字典未初始化，先初始化
        //    if (!_isFullMapInited)
        //    {
        //        InitFullMappingDictionaries();
        //    }

        //    if (bondIntensity < 0 || bondIntensity > 255) return bondIntensity; // 边界防护

        //    if (_bondToDispenseFullMap.TryGetValue(lightType, out var dict)
        //        && dict.TryGetValue(bondIntensity, out var val))
        //    {
        //        return val;
        //    }

        //    return bondIntensity;
        //}

        // 改：对外反向转换方法（增加字典初始化兜底，改用新的反向字典）
        /// <summary>
        /// 对外暴露：反向转换（直接查表，无动态计算，保证双向一致）
        /// </summary>
        //public int GetBondIntensity(LightType lightType, int dispenseIntensity)
        //{
        //    // 兜底：若字典未初始化，先初始化
        //    if (!_isFullMapInited)
        //    {
        //        InitFullMappingDictionaries();
        //    }

        //    if (dispenseIntensity < 0 || dispenseIntensity > 255) return dispenseIntensity; // 边界防护

        //    if (_dispenseToBondFullMap.TryGetValue(lightType, out var dict)
        //        && dict.TryGetValue(dispenseIntensity, out var val))
        //    {
        //        return val;
        //    }

        //    return dispenseIntensity;
        //}


        /// <summary>
        /// 红色环光映射表
        /// </summary>
        public List<(int bondIntensity, int dispenseIntensity)> RedRingLightMap { get; set; }
                    = new List<(int, int)>();

        /// <summary>
        /// 蓝色环光映射表
        /// </summary>
        public List<(int bondIntensity, int dispenseIntensity)> BlueRingLightMap { get; set; }
            = new List<(int, int)>();

        /// <summary>
        /// 绿色环光映射表
        /// </summary>
        public List<(int bondIntensity, int dispenseIntensity)> GreenRingLightMap { get; set; }
            = new List<(int, int)>();

        /// <summary>
        /// 红色点光映射表
        /// </summary>
        public List<(int bondIntensity, int dispenseIntensity)> RedSpotLightMap { get; set; }
            = new List<(int, int)>();

        /// <summary>
        /// 蓝色点光映射表
        /// </summary>
        public List<(int bondIntensity, int dispenseIntensity)> BlueSpotLightMap { get; set; }
            = new List<(int, int)>();

        /// <summary>
        /// 绿色点光映射表
        /// </summary>
        public List<(int bondIntensity, int dispenseIntensity)> GreenSpotLightMap { get; set; }
            = new List<(int, int)>();

       
        /// <summary>
        /// 应用光源亮度映射
        /// </summary>
        /// <param name="lights">光源列表</param>
        /// <param name="originalIntensities">初始亮度</param>
        /// <returns>映射后光源亮度列表</returns>
        public List<int> ApplyLightMapping(List<Light> lights, List<int> originalIntensities)
        {
            List<int> mappedIntensities = new List<int>();

            for (int i = 0; i < lights.Count; i++)
            {
                Light light = lights[i];
                int originalIntensity = originalIntensities[i];

                // 判断光源类型并应用映射
                if (light.HardwareName.Contains("点胶三色环光-红"))
                {
                    mappedIntensities.Add(this.GetDispenseIntensity(LightType.RedRing, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色环光-绿"))
                {
                    mappedIntensities.Add(this.GetDispenseIntensity(LightType.GreenRing, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色环光-蓝"))
                {
                    mappedIntensities.Add(this.GetDispenseIntensity(LightType.BlueRing, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色点光-红"))
                {
                    mappedIntensities.Add(this.GetDispenseIntensity(LightType.RedSpot, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色点光-绿"))
                {
                    mappedIntensities.Add(this.GetDispenseIntensity(LightType.GreenSpot, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色点光-蓝"))
                {
                    mappedIntensities.Add(this.GetDispenseIntensity(LightType.BlueSpot, originalIntensity));
                }
            }

            return mappedIntensities;
        }

        /// <summary>
        /// 应用光源亮度映射（反向）（点胶亮度->Bond亮度）
        /// </summary>
        /// <param name="lights">光源列表</param>
        /// <param name="originalIntensities">亮度</param>
        /// <returns>映射后光源亮度列表</returns>
        public List<int> ApplyLightMappingReverse(List<Light> lights, List<int> originalIntensities)
        {
            List<int> mappedIntensities = new List<int>();

            for (int i = 0; i < lights.Count; i++)
            {
                Light light = lights[i];
                int originalIntensity = originalIntensities[i];

                // 判断光源类型并应用映射
                if (light.HardwareName.Contains("点胶三色环光-红"))
                {
                    mappedIntensities.Add(this.GetBondIntensity(LightType.RedRing, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色环光-绿"))
                {
                    mappedIntensities.Add(this.GetBondIntensity(LightType.GreenRing, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色环光-蓝"))
                {
                    mappedIntensities.Add(this.GetBondIntensity(LightType.BlueRing, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色点光-红"))
                {
                    mappedIntensities.Add(this.GetBondIntensity(LightType.RedSpot, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色点光-绿"))
                {
                    mappedIntensities.Add(this.GetBondIntensity(LightType.GreenSpot, originalIntensity));
                }
                else if (light.HardwareName.Contains("点胶三色点光-蓝"))
                {
                    mappedIntensities.Add(this.GetBondIntensity(LightType.BlueSpot, originalIntensity));
                }
                else
                {
                    // 未知光源类型，使用原始强度
                    mappedIntensities.Add(originalIntensity);
                }
            }

            return mappedIntensities;
        }

        /// <summary>
        /// 光源类型枚举
        /// </summary>
        public enum LightType
        {
            RedRing,
            BlueRing,
            GreenRing,
            RedSpot,
            BlueSpot,
            GreenSpot
        }
    }
}
