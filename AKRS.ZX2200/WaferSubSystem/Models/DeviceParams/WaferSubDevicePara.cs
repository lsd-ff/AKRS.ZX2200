using AKRS.Galaxy2.Infrastructure.CommonModel;

namespace AKRS.ZX2200.WaferSubSystem.Models.DeviceParams
{
    using System;

    using AKRS.ZX2200.Infrastructure.Models.Path;
    using AKRS.ZX2200.Infrastructure.Service;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;

    using DevExpress.Map.Native;
    using AKRS.Galaxy2.Infrastructure.Helper;

    /// <summary>
    /// 上晶圆设备参数集合
    /// </summary>
    public class WaferSubDevicePara : SingletonJson<WaferSubDevicePara>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static WaferSubDevicePara()
        {
            SingletonJson<WaferSubDevicePara>.FilePath = ZX2200PathConfig.WaferSubDevicePara;
        }

        /// <summary>
        /// 顶针台模组设备参数
        /// </summary>
        public EjectDevicePara EjectDevicePara { get; set; } = new EjectDevicePara();

        /// <summary>
        /// 翻转模组设备参数
        /// </summary>
        public FlipTableDevicePara FlipChipDevicePara { get; set; } = new FlipTableDevicePara();

        /// <summary>
        /// MagazineBox模组设备参数
        /// </summary>
        public MagazineBoxDevicePara MagazineDevicePara { get; set; } = new MagazineBoxDevicePara();

        /// <summary>
        /// 静态华夫盒设备参数
        /// </summary>
        public StaticWaffleDevicePara StaticWaffleDevicePara { get; set; } = new StaticWaffleDevicePara();

        /// <summary>
        /// 晶圆台模组设备参数
        /// </summary>
        public WaferTableDevicePara WaferTableDevicePara { get; set; } = new WaferTableDevicePara();
    }

    /// <summary>
    /// 单例Json
    /// </summary>
    /// <typeparam name="T">类</typeparam>
    public abstract class SingletonJson<T> where T : class, new()
    {
        /// <summary>
        /// 锁对象
        /// </summary>
        public static object locker = new object();

        /// <summary>
        /// 示例
        /// </summary>
        protected static T instance;

        /// <summary>
        /// 路径
        /// </summary>
        public static string FilePath;

        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns>单例实例</returns>
        public static T GetInstance()
        {
            lock (locker)
            {
                if (instance == null)
                {
                    Load();
                }

                if (instance == null)
                {
                    instance = new T();
                }

                return instance;
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        public static void Load()
        {
            instance = new T();
            instance = JsonFormatHelper<T>.ReadGenericObject(FilePath);
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            lock (locker)
            {
                JsonFormatHelper<T>.SaveGenericObject(instance, FilePath);
                JsonFormatHelper<T>.CopyFile(FilePath, Path.GetDirectoryName(FilePath) + "Baks", true, 50);
            }
        }
    }
}
