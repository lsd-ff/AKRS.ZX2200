using System.Collections.Generic;
using AKRS.Galaxy2.Infrastructure.CommonModel;

namespace AKRS.Galaxy2.CoordinateSystems.CoordinateSystems
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using Newtonsoft.Json;

    /// <summary>
    /// 设备坐标系
    /// </summary>
    public class MachineCoordinateSystem /*: Singleton<MachineCoordinateSystem>*/
    {
        /// <summary>
        /// 单例锁
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// 单例
        /// </summary>
        private static MachineCoordinateSystem instance;

        /// <summary>
        /// 存储位置
        /// </summary>
        public static string FilePath => StaticPara.MachineCoordinateSystem;

        /// <summary>
        /// 设备单例
        /// </summary>
        /// <returns>单例</returns>
        public static MachineCoordinateSystem GetInstance()
        {
            lock (Locker)
            {
                if (instance == null)
                {
                    Load();
                }

                return instance;
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        public static void Load()
        {
            instance = JsonFormatHelper<MachineCoordinateSystem>.ReadGenericObject(FilePath);

            if (instance == null)
            {
                instance = new MachineCoordinateSystem();
            }

            Init();
        }

        /// <summary>
        /// 刷新坐标系
        /// </summary>
        public static void Refresh()
        {
            instance = null;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            foreach (BaseCoordinateSystem baseCoordinateSystem in MachineCoordinateSystem.GetInstance().CoordinateSystems)
            {
                if (baseCoordinateSystem.Name != "G0")
                {
                    baseCoordinateSystem.UpperCoordinateSystem = MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == baseCoordinateSystem.UpperName);
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            JsonFormatHelper<MachineCoordinateSystem>.SaveGenericObject(instance, FilePath);
        }

        /// <summary>
        /// 坐标系集合 
        /// </summary>
        public List<BaseCoordinateSystem> CoordinateSystems { get; set; } = new List<BaseCoordinateSystem>();

        /// <summary>
        /// 寻找坐标系
        /// </summary>
        /// <param name="selfName">自己的名字</param>
        /// <param name="upperName">上层的名字</param>
        /// <param name="isGeneralCoordinateSystem">是不是普通坐标系</param>
        /// <param name="type">类型</param>
        /// <returns>是否成功</returns>
        public BaseCoordinateSystem CreateCoordinateSystem(string selfName, string upperName, bool isGeneralCoordinateSystem, CoordinateSystemTypeEnum type)
        {
            // 寻找上一层的坐标系
            BaseCoordinateSystem upperCoordinateSystem = this.CoordinateSystems.Find(it => it.Name == upperName);

            if (upperCoordinateSystem == null)
            {
                // 如果
                if (upperName == "G0")
                {
                    upperCoordinateSystem = new GeneralCoordinateSystem("G0", CoordinateSystemTypeEnum.General, null);
                    this.CoordinateSystems.Add(upperCoordinateSystem);
                    this.Save();
                    return upperCoordinateSystem;
                }
                else
                {
                    throw new System.InvalidOperationException("上层坐标系丢失");
                }
            }

            this.CoordinateSystems.RemoveAll(it => it.Name == selfName);

            BaseCoordinateSystem system = isGeneralCoordinateSystem ? 
                  new GeneralCoordinateSystem(selfName, type, upperCoordinateSystem)
                : new DependentCoordinateSystem(selfName, type, upperCoordinateSystem);

            this.CoordinateSystems.Add(system);
            this.Save();
            return system;
        }

        /// <summary>
        /// 根据名称找到坐标系
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>坐标系</returns>
        public BaseCoordinateSystem Find(string name)
        {
            return this.CoordinateSystems.Find(it => it.Name == name);
        }

        /// <summary>
        /// 删除坐标系
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>结果</returns>
        public bool DeleteCoordinateSystem(string name)
        {
            this.Save();
            return false;
        }

        /// <summary>
        /// 晶圆坐标系
        /// </summary>
        [JsonIgnore]
        public GeneralCoordinateSystem WaferCoordinateSystem =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems
                .Find(item => item.Name == "WaferTableCoordinateSystem");

        /// <summary>
        /// Bond坐标系
        /// </summary>
        [JsonIgnore]
        public GeneralCoordinateSystem BondCoordinateSystem =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems
                .Find(item => item.Name == "BondCoordinateSystem");

        /// <summary>
        /// Bond坐标系
        /// </summary>
        [JsonIgnore]
        public GeneralCoordinateSystem DispesenSystem =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems
                .Find(item => item.Name == "DispenseCoordinateSystem");

        /// <summary>
        /// Bond坐标系
        /// </summary>
        [JsonIgnore]
        public GeneralCoordinateSystem TransportSystem =>
            (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems
                .Find(item => item.Name == "TransportCoordinateSystem");

        /// <summary>
        /// 空坐标系
        /// </summary>
        [JsonIgnore]
        public GeneralCoordinateSystem NullCoordinateSystem
        {
            get
            {
                if (this.nullCoordinateSystem == null)
                {
                    this.nullCoordinateSystem = this.IniGeneralCoordinateSystem(this.nullCoordinateSystem, "NullCoordinateSystem", new AKRSPoint3D(), new AKRSPoint3D());
                }

                return this.nullCoordinateSystem;
            }
        }

        /// <summary>
        /// 空坐标系
        /// </summary>
        [JsonIgnore]
        private GeneralCoordinateSystem nullCoordinateSystem;

        /// <summary>
        /// 晶圆相机坐标系
        /// </summary>
        [JsonIgnore]
        public DependentCoordinateSystem WaferCameraCoordinateSystem =>
            (DependentCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(item => item.Name == "WaferCameraCoordinateSystem");

        /// <summary>
        /// 初始化坐标系
        /// </summary>
        /// <param name="g">g</param>
        /// <param name="selfName">selfName</param>
        /// <param name="upperPoint3D">upperPoint3D</param>
        /// <param name="ownPoint3D">ownPoint3D</param>
        /// <param name="degree">degree</param>
        /// <returns>result</returns>
        private GeneralCoordinateSystem IniGeneralCoordinateSystem(GeneralCoordinateSystem g, string selfName, AKRSPoint3D upperPoint3D, AKRSPoint3D ownPoint3D, double degree = 0)
        {
            this.CreateCoordinateSystem(selfName, "G0", true, CoordinateSystemTypeEnum.General);
            g = (GeneralCoordinateSystem)this.CoordinateSystems.Find(item => item.Name == selfName);
            g.Init(upperPoint3D, ownPoint3D, degree);
            this.Save();

            return g;
        }
    }
}
