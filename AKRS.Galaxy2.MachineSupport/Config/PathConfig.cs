#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/10/12 17:11:54
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.Galaxy2.MachineSupport.Config
{
    /// <summary>
    /// 描述：设备路径配置
    /// </summary>
    public abstract class PathConfig
    {
        #region 静态域
        /// <summary>
        /// 文件根目录
        /// </summary>
        private static string Root { get; } = Application.StartupPath;

        /// <summary>
        /// 配置文件存放路径
        /// </summary>
        public static string SettingsPath => Path.Combine(Root, "Settings");

        /// <summary>
        /// machineConfig 文件存储路径
        /// </summary>
        public static string MachineConfigFileName =>
            Path.Combine(Application.StartupPath, "Settings", "MachineConfig.json");

        /// <summary>
        /// 硬件存储目录
        /// </summary>
        public static string HardwareDirPath => Path.Combine(SettingsPath, "Hardwares");

        /// <summary>
        /// 硬件备份目录
        /// </summary>
        public static string HardwareBakDirPath => Path.Combine(SettingsPath, "HardwareBaks");

        /// <summary>
        /// 硬件存储文件路径
        /// </summary>
        public static string HardwareFileName => Path.Combine(HardwareDirPath, "Hardware.json");

        /// <summary>
        /// 硬件存储文件备份路径
        /// </summary>
        public static string HardwareBakFileName => Path.Combine(HardwareBakDirPath, $"Hardware_{DateTime.Now.Year}{DateTime.Now.Month}{DateTime.Now.Day}{DateTime.Now.Hour}{DateTime.Now.Minute}{DateTime.Now.Second}.json");

        /// <summary>
        /// 环形限位存储文件路径
        /// </summary>
        public static string AntiCollisionSystemFileName => Path.Combine(HardwareDirPath, "AntiCollisionSystem.json");

        /// <summary>
        /// Recipe 根路径
        /// settings/RecipeParams
        /// </summary>
        public static string RecipeRootDirPath => Path.Combine(SettingsPath, "RecipeParams");

        /// <summary>
        /// Recipe 路径
        /// settings/RecipeParams/RecipeID/
        /// </summary>
        public static string RecipeDirPath => Path.Combine(RecipeRootDirPath, MachineConfigContext.GetInstance().RecipeID.ToString());

        /// <summary>
        /// 设备参数路径
        /// settings/DeviceParams
        /// </summary>
        public static string DeviceDirPath => Path.Combine(SettingsPath, "DeviceParams");

        /// <summary>
        /// 模块和组件配置目录
        /// </summary>
        public static string ControlConfigDirPath => Path.Combine(SettingsPath, "ControlConfigs");

        #endregion

        /// <summary>
        /// 获取模块的档案文件
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>模块的文件名称</returns>
        //public static string GetModuleRecipeConfigFileName(string moduleName)
        //{
        //    return Path.Combine(PathConfig.RecipeDirPath, MachineConfigContext.GetInstance().RecipeID.ToString(), moduleName + ".json");
        //}

        /// <summary>
        /// 获取PR模板路径
        /// </summary>
        /// <returns>模块的文件名称</returns>
        public static string GetPRDirPath()
        {
            return Path.Combine(PathConfig.RecipeDirPath, "PR模板\\");
        }

        /// <summary>
        /// 获取模块或组件的配置文件
        /// </summary>
        /// <param name="controlName">模块名称</param>
        /// <returns>模块的文件名称</returns>
        public static string GetModuleConfigFileName(string controlName)
        {
            return Path.Combine(ControlConfigDirPath, controlName + ".json");
        }

        /// <summary>
        /// Recipe列表文件路径
        /// </summary>
        public static string RecipeListFileName => Path.Combine(SettingsPath, "RecipeList.json");

        /// <summary>
        /// PR仓库存储路径
        /// </summary>
        public static string TemplateConfigRepositoryPath => Path.Combine(PathConfig.DeviceDirPath, "TemplateConfigRepository.json");

        /// <summary>
        /// 获取模块的档案文件
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns>模块的文件名称</returns>
        public static string GetModuleRecipeConfigFileName(string moduleName)
        {
            return Path.Combine(PathConfig.RecipeDirPath, MachineConfigContext.GetInstance().RecipeID.ToString(), moduleName + ".json");
        }
    }
}
