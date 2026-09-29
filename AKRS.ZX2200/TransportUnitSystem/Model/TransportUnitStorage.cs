using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Model
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using Newtonsoft.Json;
    using System.IO;

    /// <summary>
    /// 载具单元存储与读取
    /// </summary>
    public static class TransportUnitStorage
    {
        /// <summary>
        /// 载具单元地址
        /// </summary>
        private static readonly string TransportUnitStoragePath = Path.Combine(
            PathConfig.RecipeDirPath,
            "TransportUnitStoragePath");


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transportUnit">运输单元</param>
        /// <param name="path">地址</param>
        public static void Save(TransportUnit transportUnit, string path)
        {
            if (!Directory.Exists(TransportUnitStoragePath))
            {
                Directory.CreateDirectory(TransportUnitStoragePath);
            }

            string allPath = Path.Combine(TransportUnitStoragePath, path + ".json");
            SaveJson(transportUnit, allPath);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="transportUnit">运输单元</param>
        /// <param name="path">地址</param>
        private static void SaveJson(TransportUnit transportUnit, string path)
        {
            // 序列化
            JsonFormatHelper<TransportUnit>.SaveGenericObject(transportUnit, path);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="path">地址</param>
        /// <returns>载具单元</returns>
        public static TransportUnit Read(string path)
        {
            string allPath = Path.Combine(TransportUnitStoragePath, path + ".json");
            return ReadJson(allPath);
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="path">地址</param>
        /// <returns>载具单元</returns>
        private static TransportUnit ReadJson(string path)
        {
            return JsonFormatHelper<TransportUnit>.ReadGenericObject(path);
        }

        /// <summary>
        /// 获取所有Json文件
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>集合</returns>
        private static List<string> GetAllTransportUnitNames(string path)
        {
            return new List<string>(Directory.GetFiles(path, "*.json", SearchOption.AllDirectories));
        }
    }
}
